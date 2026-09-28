Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Authentification des collaborateurs (EX-01, EX-05 à EX-09).
''' </summary>
Public Class ServiceAuthentification

    ''' <summary>Nombre d'échecs consécutifs entraînant le verrouillage du compte (EX-06).</summary>
    Public Const MaxEchecs As Integer = 5

    Friend Const MessageIdentifiantsInvalides As String = "Identifiant ou mot de passe incorrect."
    Friend Const MessageVerrouille As String = "Votre compte est verrouillé. Contactez l'administrateur."
    Friend Const MessageInactif As String = "Ce compte n'est plus actif. Contactez l'administrateur."
    Friend Const MessageServeur As String = "Le serveur est indisponible. Réessayez dans quelques instants."

    Private ReadOnly _dao As ICollaborateurDao
    Private ReadOnly _horloge As TimeProvider

    ''' <param name="dao">Accès aux collaborateurs.</param>
    ''' <param name="horloge">Source de la date du jour (TimeProvider.System en production).</param>
    Public Sub New(dao As ICollaborateurDao, horloge As TimeProvider)
        ArgumentNullException.ThrowIfNull(dao)
        ArgumentNullException.ThrowIfNull(horloge)
        _dao = dao
        _horloge = horloge
    End Sub

    ''' <summary>Tente de connecter un collaborateur. Ne lève pas d'exception : tout est dans le résultat.</summary>
    Public Function Connecter(login As String, motDePasse As String) As ResultatConnexion
        login = If(login, "").Trim()
        If login.Length = 0 OrElse String.IsNullOrEmpty(motDePasse) Then
            Return ResultatConnexion.Refusee(StatutConnexion.IdentifiantsInvalides, "Saisissez votre identifiant et votre mot de passe.")
        End If

        Try
            Dim collaborateur = _dao.TrouverParLogin(login)

            ' Login inconnu : même message qu'un mauvais mot de passe (on ne révèle pas les comptes existants)
            If collaborateur Is Nothing Then
                _dao.Journaliser(login, Nothing, succes:=False)
                Return ResultatConnexion.Refusee(StatutConnexion.IdentifiantsInvalides, MessageIdentifiantsInvalides)
            End If

            If collaborateur.Verrouille Then
                _dao.Journaliser(login, collaborateur.Matricule, succes:=False)
                Return ResultatConnexion.Refusee(StatutConnexion.CompteVerrouille, MessageVerrouille)
            End If

            If Not HacheurMotDePasse.Verifier(motDePasse, collaborateur.MotDePasseHache) Then
                Dim nbEchecs = _dao.EnregistrerEchec(collaborateur.Matricule, MaxEchecs)
                _dao.Journaliser(login, collaborateur.Matricule, succes:=False)
                If nbEchecs >= MaxEchecs Then
                    Return ResultatConnexion.Refusee(StatutConnexion.CompteVerrouille, MessageVerrouille)
                End If
                Dim restants = MaxEchecs - nbEchecs
                Return ResultatConnexion.Refusee(StatutConnexion.IdentifiantsInvalides,
                    If(restants <= 2,
                       $"{MessageIdentifiantsInvalides} Attention : encore {restants} essai(s) avant le verrouillage du compte.",
                       MessageIdentifiantsInvalides))
            End If

            ' Mot de passe correct : le compte doit encore être actif
            Dim affectation = If(collaborateur.EstParti(Aujourdhui()), Nothing, _dao.TrouverAffectationEnCours(collaborateur.Matricule))
            If affectation Is Nothing Then
                _dao.Journaliser(login, collaborateur.Matricule, succes:=False)
                Return ResultatConnexion.Refusee(StatutConnexion.CompteInactif, MessageInactif)
            End If

            _dao.EnregistrerSucces(collaborateur.Matricule)
            _dao.Journaliser(login, collaborateur.Matricule, succes:=True)
            Return ResultatConnexion.Acceptee(New UtilisateurConnecte(collaborateur, affectation), collaborateur.MotDePasseAChanger)

        Catch ex As AccesDonneesException
            Return ResultatConnexion.Refusee(StatutConnexion.ServeurIndisponible, MessageServeur)
        End Try
    End Function

    ''' <summary>
    ''' Change le mot de passe de l'utilisateur connecté (EX-07).
    ''' Renvoie la liste des erreurs (vide si le changement a réussi).
    ''' </summary>
    Public Function ChangerMotDePasse(utilisateur As UtilisateurConnecte, ancien As String,
                                      nouveau As String, confirmation As String) As IReadOnlyList(Of String)
        ArgumentNullException.ThrowIfNull(utilisateur)
        Dim erreurs As New List(Of String)

        If Not HacheurMotDePasse.Verifier(ancien, utilisateur.Collaborateur.MotDePasseHache) Then
            erreurs.Add("Le mot de passe actuel est incorrect.")
        End If
        If nouveau <> confirmation Then
            erreurs.Add("La confirmation ne correspond pas au nouveau mot de passe.")
        End If
        If Not String.IsNullOrEmpty(nouveau) AndAlso nouveau = ancien Then
            erreurs.Add("Le nouveau mot de passe doit être différent de l'actuel.")
        End If
        erreurs.AddRange(PolitiqueMotDePasse.Verifier(nouveau))
        If erreurs.Count > 0 Then Return erreurs

        Dim hache = HacheurMotDePasse.Hacher(nouveau)
        Try
            _dao.ChangerMotDePasse(utilisateur.Matricule, hache)
        Catch ex As AccesDonneesException
            Return {MessageServeur}
        End Try

        utilisateur.Collaborateur.MotDePasseHache = hache
        utilisateur.Collaborateur.MotDePasseAChanger = False
        Return erreurs
    End Function

    Private Function Aujourdhui() As Date
        Return _horloge.GetLocalNow().Date
    End Function

End Class
