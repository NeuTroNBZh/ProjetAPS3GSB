''' <summary>Période d'analyse de l'activité (bornes incluses).</summary>
Public Class Periode

    Public Sub New(libelle As String, debut As Date, fin As Date)
        Me.Libelle = libelle
        Me.Debut = debut.Date
        Me.Fin = fin.Date
    End Sub

    Public ReadOnly Property Libelle As String

    Public ReadOnly Property Debut As Date

    Public ReadOnly Property Fin As Date

    Public Overrides Function ToString() As String
        Return Libelle
    End Function

End Class
