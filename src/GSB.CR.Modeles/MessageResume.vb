''' <summary>Ligne d'une boîte de réception ou de la liste des messages envoyés.</summary>
Public Class MessageResume

    Public Property Id As Integer

    Public Property Objet As String = ""

    Public Property DateEnvoi As DateTime

    ''' <summary>Expéditeur (message reçu) ou liste abrégée des destinataires (message envoyé).</summary>
    Public Property Correspondant As String = ""

    ''' <summary>Message reçu : vrai si déjà ouvert.</summary>
    Public Property Lu As Boolean

    ''' <summary>Message envoyé : nombre de destinataires et nombre de lectures.</summary>
    Public Property NbDestinataires As Integer

    Public Property NbLus As Integer

End Class
