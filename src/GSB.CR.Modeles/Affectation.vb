''' <summary>
''' Affectation d'un collaborateur : profil et rattachement (région ou secteur) — table AFFECTATION.
''' </summary>
Public Class Affectation

    Public Property Profil As Profil

    ''' <summary>Région de rattachement (visiteur et délégué), sinon Nothing.</summary>
    Public Property CodeRegion As String

    Public Property NomRegion As String

    ''' <summary>Secteur (celui du responsable, ou celui de la région).</summary>
    Public Property CodeSecteur As String

    Public Property LibelleSecteur As String

    Public Property DateDebut As Date

    ''' <summary>Date de fin (Nothing pour l'affectation en cours).</summary>
    Public Property DateFin As Date?

    ''' <summary>Libellé du rattachement pour l'affichage (« Région Aquitaine », « Secteur Est »…).</summary>
    Public ReadOnly Property LibelleRattachement As String
        Get
            If Not String.IsNullOrEmpty(NomRegion) Then Return $"Région {NomRegion}"
            If Not String.IsNullOrEmpty(LibelleSecteur) Then Return $"Secteur {LibelleSecteur}"
            Return "Siège"
        End Get
    End Property

End Class
