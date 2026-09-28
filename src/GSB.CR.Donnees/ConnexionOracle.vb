Imports Oracle.ManagedDataAccess.Client

''' <summary>
''' Fabrique de connexions Oracle pour la couche Données.
''' Chaque appel renvoie une connexion ouverte à utiliser dans un bloc Using.
''' </summary>
Public Class ConnexionOracle

    Private ReadOnly _chaine As String

    ''' <summary>Crée la fabrique à partir d'une configuration Oracle.</summary>
    Public Sub New(config As ConfigurationOracle)
        ArgumentNullException.ThrowIfNull(config)
        _chaine = config.ChaineDeConnexion()
    End Sub

    ''' <summary>Ouvre et renvoie une nouvelle connexion.</summary>
    Public Function Ouvrir() As OracleConnection
        Dim connexion As New OracleConnection(_chaine)
        connexion.Open()
        Return connexion
    End Function

    ''' <summary>
    ''' Vérifie que la base répond. Renvoie la version du serveur Oracle.
    ''' </summary>
    Public Function Tester() As String
        Using connexion = Ouvrir()
            Return connexion.ServerVersion
        End Using
    End Function

End Class
