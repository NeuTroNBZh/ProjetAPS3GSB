''' <summary>Motif de visite standardisé — table MOTIF.</summary>
Public Class Motif

    ''' <summary>Code du motif « Autre », qui exige une précision libre.</summary>
    Public Const CodeAutre As String = "AUTRE"

    Public Property Code As String = ""

    Public Property Libelle As String = ""

    Public Property Actif As Boolean = True

    Public ReadOnly Property EstAutre As Boolean
        Get
            Return Code = CodeAutre
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Libelle
    End Function

End Class
