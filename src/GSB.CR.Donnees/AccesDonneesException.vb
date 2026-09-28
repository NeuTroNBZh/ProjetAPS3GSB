''' <summary>
''' Erreur d'accès à la base de données (serveur injoignable, requête refusée…).
''' Masque les types Oracle aux couches supérieures.
''' </summary>
Public Class AccesDonneesException
    Inherits Exception

    Public Sub New(message As String, interne As Exception)
        MyBase.New(message, interne)
    End Sub

End Class
