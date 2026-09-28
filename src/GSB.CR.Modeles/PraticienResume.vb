''' <summary>Ligne de la liste des praticiens : identité, suivi et dernière visite.</summary>
Public Class PraticienResume

    Public Property Numero As Integer

    Public Property Nom As String = ""

    Public Property Prenom As String = ""

    Public Property LibelleType As String = ""

    Public Property CodePostal As String

    Public Property Ville As String

    Public Property Telephone As String

    Public Property Actif As Boolean = True

    ''' <summary>Visiteur qui suit actuellement le praticien (portefeuille), sinon Nothing.</summary>
    Public Property MatriculeVisiteur As String

    Public Property NomVisiteur As String

    ''' <summary>Date de la dernière visite validée (en tant que titulaire).</summary>
    Public Property DateDerniereVisite As Date?

    ''' <summary>Prochaine visite prévue lors de la dernière visite.</summary>
    Public Property DateProchainePrevue As Date?

    Public ReadOnly Property NomComplet As String
        Get
            Return $"{Nom} {Prenom}"
        End Get
    End Property

End Class
