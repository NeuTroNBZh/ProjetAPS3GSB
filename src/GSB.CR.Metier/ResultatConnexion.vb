Imports GSB.CR.Modeles

''' <summary>Résultat d'une tentative de connexion : statut, message à afficher et utilisateur si accepté.</summary>
Public Class ResultatConnexion

    Private Sub New(statut As StatutConnexion, message As String, utilisateur As UtilisateurConnecte)
        Me.Statut = statut
        Me.Message = message
        Me.Utilisateur = utilisateur
    End Sub

    Public ReadOnly Property Statut As StatutConnexion

    ''' <summary>Message destiné à l'utilisateur (vide si la connexion est réussie).</summary>
    Public ReadOnly Property Message As String

    ''' <summary>Utilisateur connecté (Nothing si la connexion est refusée).</summary>
    Public ReadOnly Property Utilisateur As UtilisateurConnecte

    ''' <summary>Vrai si l'utilisateur peut entrer (éventuellement après changement de mot de passe).</summary>
    Public ReadOnly Property EstAcceptee As Boolean
        Get
            Return Statut = StatutConnexion.Reussie OrElse Statut = StatutConnexion.ChangementMotDePasseRequis
        End Get
    End Property

    Friend Shared Function Acceptee(utilisateur As UtilisateurConnecte, changementRequis As Boolean) As ResultatConnexion
        Return If(changementRequis,
                  New ResultatConnexion(StatutConnexion.ChangementMotDePasseRequis, "Vous devez changer votre mot de passe.", utilisateur),
                  New ResultatConnexion(StatutConnexion.Reussie, "", utilisateur))
    End Function

    Friend Shared Function Refusee(statut As StatutConnexion, message As String) As ResultatConnexion
        Return New ResultatConnexion(statut, message, Nothing)
    End Function

End Class
