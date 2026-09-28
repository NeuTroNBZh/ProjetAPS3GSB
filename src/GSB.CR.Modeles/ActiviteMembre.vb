''' <summary>Activité d'un membre de l'équipe sur une période (CR validés).</summary>
Public Class ActiviteMembre

    Public Property Matricule As String = ""

    Public Property NomComplet As String = ""

    Public Property Profil As Profil

    Public Property NomRegion As String

    Public Property NbVisites As Integer

    Public Property NbPraticiens As Integer

    Public Property ConfianceMoyenne As Decimal?

    Public Property NbEchantillons As Integer

    Public Property CoutEchantillons As Decimal

    ''' <summary>Brouillons en attente (toutes dates confondues).</summary>
    Public Property NbBrouillons As Integer

    Public Property DateDerniereVisite As Date?

End Class
