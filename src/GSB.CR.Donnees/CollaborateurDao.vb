Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>
''' Implémentation Oracle de <see cref="ICollaborateurDao"/> (tables COLLABORATEUR, JOURNAL_CONNEXION,
''' vue V_AFFECTATION_EN_COURS). Requêtes paramétrées uniquement.
''' </summary>
Public Class CollaborateurDao
    Implements ICollaborateurDao

    Private ReadOnly _connexion As ConnexionOracle

    Public Sub New(connexion As ConnexionOracle)
        ArgumentNullException.ThrowIfNull(connexion)
        _connexion = connexion
    End Sub

    Public Function TrouverParLogin(login As String) As Collaborateur Implements ICollaborateurDao.TrouverParLogin
        Const sql As String =
            "select col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email,
                    col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer,
                    col_nb_echecs, col_verrouille, col_derniere_connexion
               from COLLABORATEUR
              where lower(col_login) = lower(:login)"

        Return Executer(
            "Lecture du collaborateur impossible.",
            Function(cnx)
                Using cmd = Commande(cnx, sql)
                    cmd.Parameters.Add("login", OracleDbType.Varchar2).Value = login
                    Using lecteur = cmd.ExecuteReader()
                        If Not lecteur.Read() Then Return Nothing
                        Return New Collaborateur() With {
                            .Matricule = lecteur.GetString(0),
                            .Nom = lecteur.GetString(1),
                            .Prenom = lecteur.GetString(2),
                            .Adresse = TexteOuRien(lecteur, 3),
                            .CodePostal = TexteOuRien(lecteur, 4),
                            .Ville = TexteOuRien(lecteur, 5),
                            .Telephone = TexteOuRien(lecteur, 6),
                            .Email = TexteOuRien(lecteur, 7),
                            .DateEmbauche = lecteur.GetDateTime(8),
                            .DateDepart = DateOuRien(lecteur, 9),
                            .Login = lecteur.GetString(10),
                            .MotDePasseHache = lecteur.GetString(11),
                            .MotDePasseAChanger = lecteur.GetString(12) = "O",
                            .NbEchecsConnexion = lecteur.GetInt32(13),
                            .Verrouille = lecteur.GetString(14) = "O",
                            .DerniereConnexion = DateOuRien(lecteur, 15)
                        }
                    End Using
                End Using
            End Function)
    End Function

    Public Function TrouverAffectationEnCours(matricule As String) As Affectation Implements ICollaborateurDao.TrouverAffectationEnCours
        Const sql As String =
            "select pro_code, reg_code, reg_nom, sec_code, sec_libelle, aff_date_debut
               from V_AFFECTATION_EN_COURS
              where col_matricule = :matricule"

        Return Executer(
            "Lecture de l'affectation impossible.",
            Function(cnx)
                Using cmd = Commande(cnx, sql)
                    cmd.Parameters.Add("matricule", OracleDbType.Varchar2).Value = matricule
                    Using lecteur = cmd.ExecuteReader()
                        If Not lecteur.Read() Then Return Nothing
                        Return New Affectation() With {
                            .Profil = ProfilCodes.DepuisCode(lecteur.GetString(0)),
                            .CodeRegion = TexteOuRien(lecteur, 1),
                            .NomRegion = TexteOuRien(lecteur, 2),
                            .CodeSecteur = TexteOuRien(lecteur, 3),
                            .LibelleSecteur = TexteOuRien(lecteur, 4),
                            .DateDebut = lecteur.GetDateTime(5)
                        }
                    End Using
                End Using
            End Function)
    End Function

    Public Function EnregistrerEchec(matricule As String, maxEchecs As Integer) As Integer Implements ICollaborateurDao.EnregistrerEchec
        ' Incrément et verrouillage dans la même requête : pas de concurrence possible
        Const sql As String =
            "update COLLABORATEUR
                set col_nb_echecs  = col_nb_echecs + 1,
                    col_verrouille = case when col_nb_echecs + 1 >= :max then 'O' else col_verrouille end
              where col_matricule = :matricule
          returning col_nb_echecs into :nb"

        Return Executer(
            "Enregistrement de l'échec de connexion impossible.",
            Function(cnx)
                Using cmd = Commande(cnx, sql)
                    cmd.Parameters.Add("max", OracleDbType.Int32).Value = maxEchecs
                    cmd.Parameters.Add("matricule", OracleDbType.Varchar2).Value = matricule
                    Dim nb = cmd.Parameters.Add("nb", OracleDbType.Int32, Data.ParameterDirection.Output)
                    cmd.ExecuteNonQuery()
                    Return CInt(CType(nb.Value, Oracle.ManagedDataAccess.Types.OracleDecimal).Value)
                End Using
            End Function)
    End Function

    Public Sub EnregistrerSucces(matricule As String) Implements ICollaborateurDao.EnregistrerSucces
        Const sql As String =
            "update COLLABORATEUR
                set col_nb_echecs = 0, col_derniere_connexion = systimestamp
              where col_matricule = :matricule"

        ExecuterMiseAJour("Enregistrement de la connexion impossible.", sql,
                          Sub(cmd) cmd.Parameters.Add("matricule", OracleDbType.Varchar2).Value = matricule)
    End Sub

    Public Sub ChangerMotDePasse(matricule As String, motDePasseHache As String) Implements ICollaborateurDao.ChangerMotDePasse
        Const sql As String =
            "update COLLABORATEUR
                set col_mdp = :mdp, col_mdp_a_changer = 'N'
              where col_matricule = :matricule"

        ExecuterMiseAJour("Changement du mot de passe impossible.", sql,
                          Sub(cmd)
                              cmd.Parameters.Add("mdp", OracleDbType.Varchar2).Value = motDePasseHache
                              cmd.Parameters.Add("matricule", OracleDbType.Varchar2).Value = matricule
                          End Sub)
    End Sub

    Public Sub Journaliser(login As String, matricule As String, succes As Boolean) Implements ICollaborateurDao.Journaliser
        Const sql As String =
            "insert into JOURNAL_CONNEXION (jco_login, col_matricule, jco_succes)
             values (substr(:login, 1, 30), :matricule, :succes)"

        ExecuterMiseAJour("Journalisation de la connexion impossible.", sql,
                          Sub(cmd)
                              cmd.Parameters.Add("login", OracleDbType.Varchar2).Value = If(String.IsNullOrEmpty(login), "(vide)", login)
                              cmd.Parameters.Add("matricule", OracleDbType.Varchar2).Value = If(matricule, CObj(DBNull.Value))
                              cmd.Parameters.Add("succes", OracleDbType.Char).Value = If(succes, "O", "N")
                          End Sub)
    End Sub

    ' ------------------------------------------------------------------
    ' Outils communs
    ' ------------------------------------------------------------------

    ''' <summary>Ouvre une connexion, exécute le traitement et traduit les erreurs Oracle.</summary>
    Private Function Executer(Of T)(messageErreur As String, traitement As Func(Of OracleConnection, T)) As T
        Try
            Using cnx = _connexion.Ouvrir()
                Return traitement(cnx)
            End Using
        Catch ex As OracleException
            Throw New AccesDonneesException(messageErreur, ex)
        End Try
    End Function

    Private Sub ExecuterMiseAJour(messageErreur As String, sql As String, parametrer As Action(Of OracleCommand))
        Executer(messageErreur,
                 Function(cnx)
                     Using cmd = Commande(cnx, sql)
                         parametrer(cmd)
                         Return cmd.ExecuteNonQuery()
                     End Using
                 End Function)
    End Sub

    Private Shared Function Commande(cnx As OracleConnection, sql As String) As OracleCommand
        Return New OracleCommand(sql, cnx) With {.BindByName = True}
    End Function

    Private Shared Function TexteOuRien(lecteur As OracleDataReader, index As Integer) As String
        Return If(lecteur.IsDBNull(index), Nothing, lecteur.GetString(index))
    End Function

    Private Shared Function DateOuRien(lecteur As OracleDataReader, index As Integer) As DateTime?
        Return If(lecteur.IsDBNull(index), CType(Nothing, DateTime?), lecteur.GetDateTime(index))
    End Function

End Class
