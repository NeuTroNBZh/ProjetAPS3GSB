''' <summary>Visite validée figurant dans l'historique d'un praticien.</summary>
Public Class VisitePraticien

    Public Property NumeroRapport As Integer

    Public Property DateVisite As Date

    ''' <summary>Collaborateur ayant effectué la visite.</summary>
    Public Property Visiteur As String = ""

    Public Property Motif As String

    Public Property CoefConfiance As Integer?

    ''' <summary>Vrai si le praticien a été vu en tant que remplaçant d'un autre praticien.</summary>
    Public Property VuCommeRemplacant As Boolean

    ''' <summary>
    ''' Praticien lié : le titulaire remplacé (si <see cref="VuCommeRemplacant"/>),
    ''' ou le remplaçant rencontré à la place du praticien. Nothing sinon.
    ''' </summary>
    Public Property PraticienLie As String

End Class
