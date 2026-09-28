''' <summary>Horloge figée à une date donnée, pour des tests reproductibles.</summary>
Public Class HorlogeFixe
    Inherits TimeProvider

    Private ReadOnly _maintenant As DateTimeOffset

    Public Sub New(jour As Date)
        _maintenant = New DateTimeOffset(jour.Date.AddHours(12), TimeSpan.Zero)
    End Sub

    Public Overrides Function GetUtcNow() As DateTimeOffset
        Return _maintenant
    End Function

    Public Overrides ReadOnly Property LocalTimeZone As TimeZoneInfo
        Get
            Return TimeZoneInfo.Utc
        End Get
    End Property

End Class
