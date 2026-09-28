Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Module « Mes comptes-rendus » : consultation, saisie, modification et suppression des CR
''' du collaborateur connecté (EX-10 à EX-29).
''' Les erreurs de base de données sont transformées en <see cref="ErreurMetierException"/>.
''' </summary>
Public Class ServiceRapports

    ''' <summary>Profondeur de consultation de ses propres CR (EX-20).</summary>
    Public Const AnneesConsultation As Integer = 3

    ''' <summary>Nombre maximal de praticiens renvoyés par une recherche.</summary>
    Public Const MaxResultatsRecherche As Integer = 200

    Friend Const MessageServeur As String = "Le serveur est indisponible. Réessayez dans quelques instants."

    Private ReadOnly _rapports As IRapportDao
    Private ReadOnly _referentiel As IReferentielDao
    Private ReadOnly _horloge As TimeProvider

    Public Sub New(rapports As IRapportDao, referentiel As IReferentielDao, horloge As TimeProvider)
        ArgumentNullException.ThrowIfNull(rapports)
        ArgumentNullException.ThrowIfNull(referentiel)
        ArgumentNullException.ThrowIfNull(horloge)
        _rapports = rapports
        _referentiel = referentiel
        _horloge = horloge
    End Sub

    ''' <summary>Date et heure actuelles (utilisées pour la fin des sessions de saisie).</summary>
    Public ReadOnly Property Maintenant As DateTime
        Get
            Return _horloge.GetLocalNow().DateTime
        End Get
    End Property

    Private ReadOnly Property Aujourdhui As Date
        Get
            Return Maintenant.Date
        End Get
    End Property

    ' ------------------------------------------------------------------
    ' Consultation
    ' ------------------------------------------------------------------

    ''' <summary>CR de l'utilisateur sur les 3 dernières années, du plus récent au plus ancien (EX-20).</summary>
    Public Function MesRapports(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of RapportResume)
        VerifierAcces(utilisateur)
        Return Appeler(Function() _rapports.ListerParPerimetre(Perimetre.DuCollaborateur(utilisateur.Matricule), Aujourdhui.AddYears(-AnneesConsultation), inclureBrouillons:=True))
    End Function

    ''' <summary>Nouveau CR vide, daté du jour, au nom de l'utilisateur.</summary>
    Public Function NouveauRapport(utilisateur As UtilisateurConnecte) As RapportVisite
        VerifierAcces(utilisateur)
        Return New RapportVisite() With {.MatriculeAuteur = utilisateur.Matricule, .DateVisite = Aujourdhui}
    End Function

    ''' <summary>Charge un CR de l'utilisateur. Refuse les CR d'un autre auteur.</summary>
    Public Function Charger(utilisateur As UtilisateurConnecte, numero As Integer) As RapportVisite
        VerifierAcces(utilisateur)
        Dim rapport = Appeler(Function() _rapports.Charger(numero))
        If rapport Is Nothing Then Throw New ErreurMetierException("Ce compte-rendu n'existe plus.")
        If rapport.MatriculeAuteur <> utilisateur.Matricule Then
            Throw New ErreurMetierException("Vous ne pouvez ouvrir que vos propres comptes-rendus.")
        End If
        Return rapport
    End Function

    ' ------------------------------------------------------------------
    ' Enregistrement
    ' ------------------------------------------------------------------

    ''' <summary>
    ''' Enregistre le CR en brouillon ou validé. L'auteur est toujours l'utilisateur connecté (EX-10),
    ''' un CR validé ne peut pas repasser en brouillon (EX-29) et le temps de saisie est tracé (EX-26).
    ''' </summary>
    ''' <param name="valider">Vrai pour valider le CR, faux pour l'enregistrer en brouillon.</param>
    ''' <param name="debutSaisie">Heure d'ouverture du formulaire.</param>
    Public Function Enregistrer(utilisateur As UtilisateurConnecte, rapport As RapportVisite,
                                valider As Boolean, debutSaisie As DateTime) As ResultatEnregistrement
        VerifierAcces(utilisateur)
        ArgumentNullException.ThrowIfNull(rapport)

        Try
            If Not rapport.EstNouveau Then
                Dim existant = _rapports.Charger(rapport.Numero.Value)
                If existant Is Nothing Then Return ResultatEnregistrement.Echec("Ce compte-rendu n'existe plus.")
                If existant.MatriculeAuteur <> utilisateur.Matricule Then
                    Return ResultatEnregistrement.Echec("Vous ne pouvez modifier que vos propres comptes-rendus.")
                End If
                If existant.Etat = EtatRapport.Valide AndAlso Not valider Then
                    Return ResultatEnregistrement.Echec("Un compte-rendu validé ne peut pas repasser en brouillon.")
                End If
            End If

            rapport.MatriculeAuteur = utilisateur.Matricule
            rapport.Etat = If(valider, EtatRapport.Valide, EtatRapport.Brouillon)
            Normaliser(rapport)

            Dim erreurs = ValidateurRapport.Verifier(rapport, Aujourdhui)
            If erreurs.Count > 0 Then Return ResultatEnregistrement.Echec(erreurs)

            Dim numero As Integer
            If rapport.EstNouveau Then
                numero = _rapports.Creer(rapport)
                rapport.Numero = numero
            Else
                numero = rapport.Numero.Value
                _rapports.Modifier(rapport)
            End If

            Dim fin = Maintenant
            _rapports.AjouterSessionSaisie(numero, utilisateur.Matricule, If(debutSaisie > fin, fin, debutSaisie), fin)
            Return ResultatEnregistrement.Succes(numero)

        Catch ex As AccesDonneesException
            Return ResultatEnregistrement.Echec(MessageServeur)
        End Try
    End Function

    ''' <summary>Supprime un brouillon de l'utilisateur (EX-29). Un CR validé ne peut pas être supprimé.</summary>
    Public Sub SupprimerBrouillon(utilisateur As UtilisateurConnecte, numero As Integer)
        Dim rapport = Charger(utilisateur, numero)
        If rapport.Etat = EtatRapport.Valide Then
            Throw New ErreurMetierException("Un compte-rendu validé ne peut pas être supprimé.")
        End If
        If Not Appeler(Function() _rapports.SupprimerBrouillon(numero)) Then
            Throw New ErreurMetierException("Ce brouillon n'a pas pu être supprimé.")
        End If
    End Sub

    ' ------------------------------------------------------------------
    ' Référentiels pour le formulaire de saisie
    ' ------------------------------------------------------------------

    ''' <summary>Praticiens du portefeuille de l'utilisateur (EX-28).</summary>
    Public Function Portefeuille(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of Praticien)
        VerifierAcces(utilisateur)
        Return Appeler(Function() _referentiel.ListerPortefeuille(utilisateur.Matricule))
    End Function

    Public Function RechercherPraticiens(debut As String) As IReadOnlyList(Of Praticien)
        Return Appeler(Function() _referentiel.RechercherPraticiens(debut, MaxResultatsRecherche))
    End Function

    Public Function TrouverPraticien(numero As Integer) As Praticien
        Return Appeler(Function() _referentiel.TrouverPraticien(numero))
    End Function

    Public Function TypesPraticien() As IReadOnlyList(Of TypePraticien)
        Return Appeler(Function() _referentiel.ListerTypesPraticien())
    End Function

    ''' <summary>Tous les médicaments ; l'IHM ne propose que les actifs (plus ceux déjà présents dans le CR).</summary>
    Public Function Medicaments() As IReadOnlyList(Of Medicament)
        Return Appeler(Function() _referentiel.ListerMedicaments())
    End Function

    ''' <summary>Tous les motifs ; l'IHM ne propose que les actifs (plus celui déjà présent dans le CR).</summary>
    Public Function Motifs() As IReadOnlyList(Of Motif)
        Return Appeler(Function() _referentiel.ListerMotifs())
    End Function

    ''' <summary>
    ''' Crée la fiche d'un remplaçant rencontré lors d'une visite (EX-16, EX-28).
    ''' Renvoie la liste des erreurs, vide en cas de succès (le numéro est alors renseigné).
    ''' </summary>
    Public Function CreerRemplacant(praticien As Praticien) As IReadOnlyList(Of String)
        ArgumentNullException.ThrowIfNull(praticien)
        praticien.Nom = If(praticien.Nom, "").Trim()
        praticien.Prenom = If(praticien.Prenom, "").Trim()

        Dim erreurs As New List(Of String)
        If praticien.Nom.Length = 0 Then erreurs.Add("Le nom du remplaçant est obligatoire.")
        If praticien.Prenom.Length = 0 Then erreurs.Add("Le prénom du remplaçant est obligatoire.")
        If String.IsNullOrEmpty(praticien.CodeType) Then erreurs.Add("Choisissez le type de praticien.")
        If praticien.Nom.Length > 50 OrElse praticien.Prenom.Length > 50 Then erreurs.Add("Le nom et le prénom ne doivent pas dépasser 50 caractères.")
        If erreurs.Count > 0 Then Return erreurs

        praticien.Numero = Appeler(Function() _referentiel.CreerPraticien(praticien))
        Return erreurs
    End Function

    ' ------------------------------------------------------------------

    ''' <summary>Nettoie la saisie : espaces superflus, champs vides, précision réservée au motif « Autre ».</summary>
    Private Shared Sub Normaliser(r As RapportVisite)
        r.Bilan = Nettoyer(r.Bilan)
        r.PrecisionMotif = If(r.CodeMotif = Motif.CodeAutre, Nettoyer(r.PrecisionMotif), Nothing)
        If String.IsNullOrEmpty(r.CodeMotif) Then r.CodeMotif = Nothing
        r.DateVisite = r.DateVisite?.Date
        r.DateProchaineVisite = r.DateProchaineVisite?.Date
    End Sub

    Private Shared Function Nettoyer(texte As String) As String
        Return If(String.IsNullOrWhiteSpace(texte), Nothing, texte.Trim())
    End Function

    Private Shared Sub VerifierAcces(utilisateur As UtilisateurConnecte)
        ArgumentNullException.ThrowIfNull(utilisateur)
        If Not Autorisations.PeutAcceder(utilisateur.Profil, ModuleApplication.MesComptesRendus) Then
            Throw New ErreurMetierException("Votre profil ne permet pas de saisir des comptes-rendus.")
        End If
    End Sub

    ''' <summary>Exécute un appel à la base en traduisant les erreurs techniques.</summary>
    Private Shared Function Appeler(Of T)(appel As Func(Of T)) As T
        Try
            Return appel()
        Catch ex As AccesDonneesException
            Throw New ErreurMetierException(MessageServeur, ex)
        End Try
    End Function

End Class
