Imports Oracle.ManagedDataAccess.Client

''' <summary>
''' Base commune des DAO Oracle : ouverture de connexion, transactions, traduction des erreurs
''' Oracle en <see cref="AccesDonneesException"/> et lecture des valeurs nullables.
''' </summary>
Public MustInherit Class DaoOracle

    Private ReadOnly _connexion As ConnexionOracle

    Protected Sub New(connexion As ConnexionOracle)
        ArgumentNullException.ThrowIfNull(connexion)
        _connexion = connexion
    End Sub

    ''' <summary>Ouvre une connexion, exécute le traitement et traduit les erreurs Oracle.</summary>
    Protected Function Executer(Of T)(messageErreur As String, traitement As Func(Of OracleConnection, T)) As T
        Try
            Using cnx = _connexion.Ouvrir()
                Return traitement(cnx)
            End Using
        Catch ex As OracleException
            Throw New AccesDonneesException(messageErreur, ex)
        End Try
    End Function

    ''' <summary>
    ''' Exécute le traitement dans une transaction : validée s'il se termine normalement,
    ''' annulée en cas d'erreur (aucune écriture partielle).
    ''' </summary>
    Protected Function ExecuterTransaction(Of T)(messageErreur As String, traitement As Func(Of OracleConnection, T)) As T
        Return Executer(messageErreur,
            Function(cnx)
                Using transaction = cnx.BeginTransaction()
                    Dim resultat = traitement(cnx)
                    transaction.Commit()
                    Return resultat
                End Using
            End Function)
    End Function

    ''' <summary>Exécute une requête de mise à jour paramétrée et renvoie le nombre de lignes touchées.</summary>
    Protected Function ExecuterMiseAJour(messageErreur As String, sql As String, parametrer As Action(Of OracleCommand)) As Integer
        Return Executer(messageErreur,
            Function(cnx)
                Using cmd = Commande(cnx, sql)
                    parametrer(cmd)
                    Return cmd.ExecuteNonQuery()
                End Using
            End Function)
    End Function

    ''' <summary>Exécute une requête et transforme chaque ligne avec <paramref name="lire"/>.</summary>
    Protected Function Lister(Of T)(messageErreur As String, sql As String, parametrer As Action(Of OracleCommand),
                                   lire As Func(Of OracleDataReader, T)) As List(Of T)
        Return Executer(messageErreur, Function(cnx) Lister(cnx, sql, parametrer, lire))
    End Function

    ''' <summary>Variante de <see cref="Lister"/> sur une connexion déjà ouverte.</summary>
    Protected Shared Function Lister(Of T)(cnx As OracleConnection, sql As String, parametrer As Action(Of OracleCommand),
                                          lire As Func(Of OracleDataReader, T)) As List(Of T)
        Dim resultat As New List(Of T)
        Using cmd = Commande(cnx, sql)
            parametrer?.Invoke(cmd)
            Using lecteur = cmd.ExecuteReader()
                While lecteur.Read()
                    resultat.Add(lire(lecteur))
                End While
            End Using
        End Using
        Return resultat
    End Function

    Protected Shared Function Commande(cnx As OracleConnection, sql As String) As OracleCommand
        Return New OracleCommand(sql, cnx) With {.BindByName = True}
    End Function

    ''' <summary>Ajoute un paramètre ; Nothing est envoyé comme NULL.</summary>
    Protected Shared Sub Parametre(cmd As OracleCommand, nom As String, type As OracleDbType, valeur As Object)
        cmd.Parameters.Add(nom, type).Value = If(valeur, DBNull.Value)
    End Sub

    Protected Shared Function TexteOuRien(lecteur As OracleDataReader, index As Integer) As String
        Return If(lecteur.IsDBNull(index), Nothing, lecteur.GetString(index))
    End Function

    Protected Shared Function DateOuRien(lecteur As OracleDataReader, index As Integer) As DateTime?
        Return If(lecteur.IsDBNull(index), CType(Nothing, DateTime?), lecteur.GetDateTime(index))
    End Function

    Protected Shared Function EntierOuRien(lecteur As OracleDataReader, index As Integer) As Integer?
        Return If(lecteur.IsDBNull(index), CType(Nothing, Integer?), lecteur.GetInt32(index))
    End Function

    Protected Shared Function DecimalOuRien(lecteur As OracleDataReader, index As Integer) As Decimal?
        Return If(lecteur.IsDBNull(index), CType(Nothing, Decimal?), lecteur.GetDecimal(index))
    End Function

    ''' <summary>Lit la valeur d'un paramètre de sortie numérique (RETURNING … INTO).</summary>
    Protected Shared Function EntierSortie(parametre As OracleParameter) As Integer
        Return CInt(CType(parametre.Value, Oracle.ManagedDataAccess.Types.OracleDecimal).Value)
    End Function

End Class
