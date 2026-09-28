''' <summary>Groupe de destinataires (tous les membres d'une région ou d'un secteur).</summary>
Public Class GroupeDestinataires

    Public Property Libelle As String = ""

    Public Property Matricules As New List(Of String)

    Public Overrides Function ToString() As String
        Return $"{Libelle} ({Matricules.Count})"
    End Function

End Class
