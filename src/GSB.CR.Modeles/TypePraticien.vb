''' <summary>Type de praticien (médecin de ville, hospitalier, pharmacien…) — table TYPE_PRATICIEN.</summary>
Public Class TypePraticien

    Public Property Code As String = ""

    Public Property Libelle As String = ""

    Public Overrides Function ToString() As String
        Return Libelle
    End Function

End Class
