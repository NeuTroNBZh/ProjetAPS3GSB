''' <summary>Fiche détaillée d'un médicament (EX-22).</summary>
Public Class FicheMedicament

    Public Property Medicament As New Medicament()

    Public Property Effets As String

    Public Property ContreIndications As String

    Public Property DateCommercialisation As Date?

    Public Property Composition As New List(Of LigneComposition)

    Public Property Interactions As New List(Of InteractionMedicamenteuse)

    Public Property Posologies As New List(Of Posologie)

End Class
