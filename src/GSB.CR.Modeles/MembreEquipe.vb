''' <summary>Visiteur ou délégué membre d'un périmètre (région ou secteur).</summary>
Public Class MembreEquipe

    Public Property Matricule As String = ""

    Public Property Nom As String = ""

    Public Property Prenom As String = ""

    Public Property Profil As Profil

    Public Property CodeRegion As String

    Public Property NomRegion As String

    Public Property CodeSecteur As String

    Public Property LibelleSecteur As String

    Public ReadOnly Property NomComplet As String
        Get
            Return $"{Prenom} {Nom}"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return If(Profil = Profil.Delegue, $"{NomComplet} (délégué)", NomComplet)
    End Function

End Class
