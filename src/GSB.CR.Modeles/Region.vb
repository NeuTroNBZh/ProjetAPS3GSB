''' <summary>Région commerciale — table REGION.</summary>
Public Class Region

    Public Property Code As String = ""

    Public Property Nom As String = ""

    Public Property CodeSecteur As String = ""

    Public Overrides Function ToString() As String
        Return Nom
    End Function

End Class
