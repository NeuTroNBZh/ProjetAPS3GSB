''' <summary>Fiche détaillée d'un praticien (EX-21) : identité, spécialités, suivi et historique des visites.</summary>
Public Class FichePraticien

    Public Property Praticien As New Praticien()

    Public Property Specialites As New List(Of SpecialitePraticien)

    ''' <summary>Visiteur qui suit actuellement le praticien, sinon Nothing.</summary>
    Public Property NomVisiteur As String

    Public Property DateDerniereVisite As Date?

    Public Property DateProchainePrevue As Date?

    ''' <summary>Visites validées, de la plus récente à la plus ancienne (titulaire ou remplaçant).</summary>
    Public Property Visites As New List(Of VisitePraticien)

End Class
