''' <summary>Spécialité d'un praticien avec son diplôme le plus haut — table POSSEDER.</summary>
Public Class SpecialitePraticien

    Public Property Code As String = ""

    Public Property Libelle As String = ""

    Public Property Diplome As String

    ''' <summary>Coefficient de prescription dans la spécialité (donnée achetée).</summary>
    Public Property CoefPrescription As Decimal?

End Class
