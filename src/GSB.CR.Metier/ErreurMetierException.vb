''' <summary>
''' Erreur destinée à l'utilisateur : action refusée par une règle de gestion ou serveur indisponible.
''' Le message est affichable tel quel.
''' </summary>
Public Class ErreurMetierException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, interne As Exception)
        MyBase.New(message, interne)
    End Sub

End Class
