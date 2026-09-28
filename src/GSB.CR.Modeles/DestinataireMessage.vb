''' <summary>Destinataire d'un message et date à laquelle il l'a lu.</summary>
Public Class DestinataireMessage

    Public Property Matricule As String = ""

    Public Property NomComplet As String = ""

    ''' <summary>Nothing tant que le message n'a pas été ouvert.</summary>
    Public Property DateLecture As DateTime?

End Class
