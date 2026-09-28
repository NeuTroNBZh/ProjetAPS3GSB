''' <summary>
''' Utilisateur authentifié pour la session en cours : le collaborateur et son affectation actuelle.
''' </summary>
Public Class UtilisateurConnecte

    Public Sub New(collaborateur As Collaborateur, affectation As Affectation)
        ArgumentNullException.ThrowIfNull(collaborateur)
        ArgumentNullException.ThrowIfNull(affectation)
        Me.Collaborateur = collaborateur
        Me.Affectation = affectation
    End Sub

    Public ReadOnly Property Collaborateur As Collaborateur

    Public ReadOnly Property Affectation As Affectation

    Public ReadOnly Property Matricule As String
        Get
            Return Collaborateur.Matricule
        End Get
    End Property

    Public ReadOnly Property Profil As Profil
        Get
            Return Affectation.Profil
        End Get
    End Property

End Class
