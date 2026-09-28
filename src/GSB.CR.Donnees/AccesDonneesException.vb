''' <summary>
''' Erreur d'accès à la base de données (serveur injoignable, requête refusée…).
''' Masque les types Oracle aux couches supérieures.
''' </summary>
Public Class AccesDonneesException
    Inherits Exception

    Public Sub New(message As String, interne As Exception)
        MyBase.New(message, interne)
    End Sub

    Public Sub New(message As String, codeOracle As Integer, interne As Exception)
        MyBase.New(message, interne)
        Me.CodeOracle = codeOracle
    End Sub

    ''' <summary>Numéro de l'erreur Oracle (ex. 1 = valeur unique en double, 2290 = contrainte CHECK), 0 si inconnu.</summary>
    Public ReadOnly Property CodeOracle As Integer

    ''' <summary>Vrai si l'erreur vient d'une contrainte d'unicité (ORA-00001).</summary>
    Public ReadOnly Property EstDoublon As Boolean
        Get
            Return CodeOracle = 1
        End Get
    End Property

End Class
