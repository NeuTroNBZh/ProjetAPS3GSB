''' <summary>Message complet — tables MESSAGE et MESSAGE_DESTINATAIRE.</summary>
Public Class MessageDetaille

    Public Property Id As Integer

    Public Property MatriculeExpediteur As String = ""

    Public Property Expediteur As String = ""

    Public Property Objet As String = ""

    Public Property Contenu As String = ""

    Public Property DateEnvoi As DateTime

    Public Property Destinataires As New List(Of DestinataireMessage)

End Class
