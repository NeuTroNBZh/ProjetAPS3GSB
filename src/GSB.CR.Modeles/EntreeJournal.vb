''' <summary>Tentative de connexion — table JOURNAL_CONNEXION (EX-74).</summary>
Public Class EntreeJournal

    Public Property DateHeure As DateTime

    ''' <summary>Identifiant saisi (même s'il ne correspond à aucun compte).</summary>
    Public Property LoginSaisi As String = ""

    ''' <summary>Collaborateur correspondant, Nothing si le login est inconnu.</summary>
    Public Property NomCollaborateur As String

    Public Property Succes As Boolean

End Class
