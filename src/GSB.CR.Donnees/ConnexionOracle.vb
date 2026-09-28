Imports Oracle.ManagedDataAccess.Client

''' <summary>
''' Fabrique de connexions Oracle pour la couche Données.
''' Chaque appel renvoie une connexion ouverte à utiliser dans un bloc Using.
''' </summary>
Public Class ConnexionOracle

    Private ReadOnly _chaine As String
    Private ReadOnly _instructionSchema As String

    ''' <summary>Crée la fabrique à partir d'une configuration Oracle.</summary>
    Public Sub New(config As ConfigurationOracle)
        ArgumentNullException.ThrowIfNull(config)
        _chaine = config.ChaineDeConnexion()
        _instructionSchema = config.InstructionSchema()
    End Sub

    ''' <summary>Ouvre et renvoie une nouvelle connexion, placée sur le schéma configuré le cas échéant.</summary>
    Public Function Ouvrir() As OracleConnection
        Dim connexion As New OracleConnection(_chaine)
        connexion.Open()
        If _instructionSchema.Length > 0 Then
            Try
                Using cmd = connexion.CreateCommand()
                    cmd.CommandText = _instructionSchema
                    cmd.ExecuteNonQuery()
                End Using
            Catch
                connexion.Dispose()
                Throw
            End Try
        End If
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
