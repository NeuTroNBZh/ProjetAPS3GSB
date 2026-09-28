''' <summary>Secteur géographique — table SECTEUR.</summary>
Public Class Secteur

    Public Property Code As String = ""

    Public Property Libelle As String = ""

    Public Overrides Function ToString() As String
        Return Libelle
    End Function

End Class
