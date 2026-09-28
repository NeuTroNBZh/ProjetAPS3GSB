''' <summary>Collaborateur vu par l'administrateur : identité, état du compte et affectation en cours.</summary>
Public Class FicheCollaborateur

    Public Property Collaborateur As New Collaborateur()

    ''' <summary>Affectation en cours (Nothing si le collaborateur est parti ou sans affectation).</summary>
    Public Property AffectationEnCours As Affectation

    ''' <summary>Nombre de praticiens actuellement dans son portefeuille.</summary>
    Public Property TaillePortefeuille As Integer

End Class
