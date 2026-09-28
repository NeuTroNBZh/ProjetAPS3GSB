Imports System.Text
Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Messagerie interne (EX-50) : envoi à une personne ou à un groupe (région, secteur),
''' boîte de réception avec état lu / non lu, messages envoyés avec suivi de lecture.
''' </summary>
Public Class ServiceMessagerie

    Public Const MaxLongueurObjet As Integer = 100
    Public Const MaxOctetsContenu As Integer = 4000
    Public Const MaxDestinataires As Integer = 500

    Private ReadOnly _dao As IMessagerieDao

    Public Sub New(dao As IMessagerieDao)
        ArgumentNullException.ThrowIfNull(dao)
        _dao = dao
    End Sub

    Public Function BoiteDeReception(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of MessageResume)
        VerifierAcces(utilisateur)
        Return Appeler(Function() _dao.ListerRecus(utilisateur.Matricule))
    End Function

    Public Function MessagesEnvoyes(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of MessageResume)
        VerifierAcces(utilisateur)
        Return Appeler(Function() _dao.ListerEnvoyes(utilisateur.Matricule))
    End Function

    Public Function NombreNonLus(utilisateur As UtilisateurConnecte) As Integer
        VerifierAcces(utilisateur)
        Return Appeler(Function() _dao.CompterNonLus(utilisateur.Matricule))
    End Function

    ''' <summary>
    ''' Ouvre un message : réservé à l'expéditeur et aux destinataires.
    ''' Pour un destinataire, la lecture est enregistrée.
    ''' </summary>
    Public Function Lire(utilisateur As UtilisateurConnecte, id As Integer) As MessageDetaille
        VerifierAcces(utilisateur)
        Dim message = Appeler(Function() _dao.Charger(id))
        If message Is Nothing Then Throw New ErreurMetierException("Ce message n'existe plus.")

        Dim destinataire = message.Destinataires.FirstOrDefault(Function(d) d.Matricule = utilisateur.Matricule)
        If destinataire Is Nothing AndAlso message.MatriculeExpediteur <> utilisateur.Matricule Then
            Throw New ErreurMetierException("Vous n'êtes pas destinataire de ce message.")
        End If
        If destinataire IsNot Nothing AndAlso Not destinataire.DateLecture.HasValue Then
            Appeler(Function()
                        _dao.MarquerLu(id, utilisateur.Matricule)
                        Return True
                    End Function)
            destinataire.DateLecture = DateTime.Now
        End If
        Return message
    End Function

    ''' <summary>Collaborateurs à qui écrire (l'utilisateur lui-même exclu).</summary>
    Public Function Annuaire(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of MembreEquipe)
        VerifierAcces(utilisateur)
        Return Appeler(Function() _dao.ListerAnnuaire()).Where(Function(c) c.Matricule <> utilisateur.Matricule).ToList()
    End Function

    ''' <summary>
    ''' Groupes de destinataires : chaque région (visiteurs et délégué) et chaque secteur (tous ses membres,
    ''' responsable compris). La région et le secteur de l'utilisateur sont proposés en premier.
    ''' </summary>
    Public Shared Function Groupes(annuaire As IEnumerable(Of MembreEquipe), utilisateur As UtilisateurConnecte) As IReadOnlyList(Of GroupeDestinataires)
        Dim liste = annuaire.ToList()
        Dim regions = liste.Where(Function(c) c.CodeRegion IsNot Nothing).
            GroupBy(Function(c) (c.CodeRegion, c.NomRegion)).
            Select(Function(g) (Premier:=g.Key.CodeRegion = utilisateur.Affectation.CodeRegion,
                                Groupe:=New GroupeDestinataires() With {.Libelle = $"Région {g.Key.NomRegion}",
                                                                        .Matricules = g.Select(Function(c) c.Matricule).ToList()}))
        Dim secteurs = liste.Where(Function(c) c.CodeSecteur IsNot Nothing).
            GroupBy(Function(c) (c.CodeSecteur, c.LibelleSecteur)).
            Select(Function(g) (Premier:=g.Key.CodeSecteur = utilisateur.Affectation.CodeSecteur,
                                Groupe:=New GroupeDestinataires() With {.Libelle = $"Secteur {g.Key.LibelleSecteur}",
                                                                        .Matricules = g.Select(Function(c) c.Matricule).ToList()}))
        Return regions.Concat(secteurs).
            OrderByDescending(Function(x) x.Premier).ThenBy(Function(x) x.Groupe.Libelle).
            Select(Function(x) x.Groupe).ToList()
    End Function

    ''' <summary>Envoie un message. Renvoie les erreurs (vide si envoyé).</summary>
    Public Function Envoyer(utilisateur As UtilisateurConnecte, objet As String, contenu As String,
                            destinataires As IEnumerable(Of String)) As IReadOnlyList(Of String)
        VerifierAcces(utilisateur)
        objet = If(objet, "").Trim()
        contenu = If(contenu, "").Trim()
        ' Sans doublon et sans l'expéditeur lui-même (un groupe peut le contenir)
        Dim matricules = If(destinataires, Enumerable.Empty(Of String)()).
            Where(Function(m) Not String.IsNullOrWhiteSpace(m) AndAlso m <> utilisateur.Matricule).Distinct().ToList()

        Dim erreurs As New List(Of String)
        If matricules.Count = 0 Then erreurs.Add("Choisissez au moins un destinataire.")
        If matricules.Count > MaxDestinataires Then erreurs.Add($"Un message ne peut pas avoir plus de {MaxDestinataires} destinataires.")
        If objet.Length = 0 Then erreurs.Add("Indiquez l'objet du message.")
        If objet.Length > MaxLongueurObjet Then erreurs.Add($"L'objet ne doit pas dépasser {MaxLongueurObjet} caractères.")
        If contenu.Length = 0 Then erreurs.Add("Le message est vide.")
        If Encoding.UTF8.GetByteCount(contenu) > MaxOctetsContenu Then erreurs.Add("Le message est trop long.")
        If erreurs.Count > 0 Then Return erreurs

        ' Tous les destinataires doivent être des collaborateurs en poste
        Dim enPoste = Annuaire(utilisateur).Select(Function(c) c.Matricule).ToHashSet()
        If matricules.Any(Function(m) Not enPoste.Contains(m)) Then Return {"Un des destinataires n'est plus en poste."}

        Try
            _dao.Envoyer(utilisateur.Matricule, objet, contenu, matricules)
        Catch ex As AccesDonneesException
            Return {ServiceRapports.MessageServeur}
        End Try
        Return erreurs
    End Function

    ''' <summary>Objet d'une réponse : « RE : » ajouté une seule fois.</summary>
    Public Shared Function ObjetReponse(objet As String) As String
        Dim o = If(objet, "").Trim()
        Dim reponse = If(o.StartsWith("RE :", StringComparison.OrdinalIgnoreCase), o, $"RE : {o}")
        Return If(reponse.Length > MaxLongueurObjet, reponse.Substring(0, MaxLongueurObjet), reponse)
    End Function

    Private Shared Sub VerifierAcces(utilisateur As UtilisateurConnecte)
        ArgumentNullException.ThrowIfNull(utilisateur)
        If Not Autorisations.PeutAcceder(utilisateur.Profil, ModuleApplication.Messagerie) Then
            Throw New ErreurMetierException("Votre profil ne permet pas d'utiliser la messagerie.")
        End If
    End Sub

    Private Shared Function Appeler(Of T)(appel As Func(Of T)) As T
        Try
            Return appel()
        Catch ex As AccesDonneesException
            Throw New ErreurMetierException(ServiceRapports.MessageServeur, ex)
        End Try
    End Function

End Class
