Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>Implémentation Oracle de <see cref="IMessagerieDao"/> (tables MESSAGE et MESSAGE_DESTINATAIRE).</summary>
Public Class MessagerieDao
    Inherits DaoOracle
    Implements IMessagerieDao

    Public Sub New(connexion As ConnexionOracle)
        MyBase.New(connexion)
    End Sub

    Public Function ListerAnnuaire() As List(Of MembreEquipe) Implements IMessagerieDao.ListerAnnuaire
        Const sql As String =
            "select col_matricule, col_nom, col_prenom, pro_code, reg_code, reg_nom, sec_code, sec_libelle
               from V_AFFECTATION_EN_COURS
              order by col_nom, col_prenom"
        Return Lister("Lecture de l'annuaire impossible.", sql, Nothing,
                      Function(l) New MembreEquipe() With {
                          .Matricule = l.GetString(0), .Nom = l.GetString(1), .Prenom = l.GetString(2),
                          .Profil = ProfilCodes.DepuisCode(l.GetString(3)),
                          .CodeRegion = TexteOuRien(l, 4), .NomRegion = TexteOuRien(l, 5),
                          .CodeSecteur = TexteOuRien(l, 6), .LibelleSecteur = TexteOuRien(l, 7)})
    End Function

    Public Function ListerRecus(matricule As String) As List(Of MessageResume) Implements IMessagerieDao.ListerRecus
        Const sql As String =
            "select m.msg_id, m.msg_objet, m.msg_date_envoi, c.col_prenom || ' ' || c.col_nom, d.dst_date_lecture
               from MESSAGE_DESTINATAIRE d
               join MESSAGE m       on m.msg_id = d.msg_id
               join COLLABORATEUR c on c.col_matricule = m.col_expediteur
              where d.col_matricule = :matricule
              order by m.msg_date_envoi desc, m.msg_id desc"
        Return Lister("Lecture des messages reçus impossible.", sql,
                      Sub(cmd) Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule),
                      Function(l) New MessageResume() With {
                          .Id = l.GetInt32(0), .Objet = l.GetString(1), .DateEnvoi = l.GetDateTime(2),
                          .Correspondant = l.GetString(3), .Lu = Not l.IsDBNull(4)})
    End Function

    Public Function ListerEnvoyes(matricule As String) As List(Of MessageResume) Implements IMessagerieDao.ListerEnvoyes
        ' listagg ... on overflow truncate : liste abrégée même pour un envoi à tout un secteur
        Const sql As String =
            "select m.msg_id, m.msg_objet, m.msg_date_envoi,
                    listagg(c.col_prenom || ' ' || c.col_nom, ', ' on overflow truncate '…' without count)
                        within group (order by c.col_nom, c.col_prenom),
                    count(*), count(d.dst_date_lecture)
               from MESSAGE m
               join MESSAGE_DESTINATAIRE d on d.msg_id = m.msg_id
               join COLLABORATEUR c        on c.col_matricule = d.col_matricule
              where m.col_expediteur = :matricule
              group by m.msg_id, m.msg_objet, m.msg_date_envoi
              order by m.msg_date_envoi desc, m.msg_id desc"
        Return Lister("Lecture des messages envoyés impossible.", sql,
                      Sub(cmd) Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule),
                      Function(l) New MessageResume() With {
                          .Id = l.GetInt32(0), .Objet = l.GetString(1), .DateEnvoi = l.GetDateTime(2),
                          .Correspondant = l.GetString(3), .NbDestinataires = l.GetInt32(4), .NbLus = l.GetInt32(5)})
    End Function

    Public Function Charger(id As Integer) As MessageDetaille Implements IMessagerieDao.Charger
        Const sqlMessage As String =
            "select m.msg_id, m.col_expediteur, c.col_prenom || ' ' || c.col_nom, m.msg_objet, m.msg_contenu, m.msg_date_envoi
               from MESSAGE m
               join COLLABORATEUR c on c.col_matricule = m.col_expediteur
              where m.msg_id = :id"
        Const sqlDestinataires As String =
            "select d.col_matricule, c.col_prenom || ' ' || c.col_nom, d.dst_date_lecture
               from MESSAGE_DESTINATAIRE d
               join COLLABORATEUR c on c.col_matricule = d.col_matricule
              where d.msg_id = :id
              order by c.col_nom, c.col_prenom"

        Return Executer("Lecture du message impossible.",
            Function(cnx)
                Dim parId As Action(Of OracleCommand) = Sub(cmd) Parametre(cmd, "id", OracleDbType.Int32, id)
                Dim message = Lister(cnx, sqlMessage, parId,
                    Function(l) New MessageDetaille() With {
                        .Id = l.GetInt32(0), .MatriculeExpediteur = l.GetString(1), .Expediteur = l.GetString(2),
                        .Objet = l.GetString(3), .Contenu = l.GetString(4), .DateEnvoi = l.GetDateTime(5)}).FirstOrDefault()
                If message Is Nothing Then Return Nothing
                message.Destinataires = Lister(cnx, sqlDestinataires, parId,
                    Function(l) New DestinataireMessage() With {
                        .Matricule = l.GetString(0), .NomComplet = l.GetString(1), .DateLecture = DateOuRien(l, 2)})
                Return message
            End Function)
    End Function

    Public Function Envoyer(expediteur As String, objet As String, contenu As String,
                            destinataires As IEnumerable(Of String)) As Integer Implements IMessagerieDao.Envoyer
        Const sqlMessage As String =
            "insert into MESSAGE (col_expediteur, msg_objet, msg_contenu)
             values (:expediteur, :objet, :contenu)
             returning msg_id into :id"
        Const sqlDestinataire As String =
            "insert into MESSAGE_DESTINATAIRE (msg_id, col_matricule) values (:id, :matricule)"

        Return ExecuterTransaction("Envoi du message impossible.",
            Function(cnx)
                Dim id As Integer
                Using cmd = Commande(cnx, sqlMessage)
                    Parametre(cmd, "expediteur", OracleDbType.Varchar2, expediteur)
                    Parametre(cmd, "objet", OracleDbType.Varchar2, objet)
                    Parametre(cmd, "contenu", OracleDbType.Varchar2, contenu)
                    Dim sortie = cmd.Parameters.Add("id", OracleDbType.Int32, Data.ParameterDirection.Output)
                    cmd.ExecuteNonQuery()
                    id = EntierSortie(sortie)
                End Using
                For Each matricule In destinataires
                    Using cmd = Commande(cnx, sqlDestinataire)
                        Parametre(cmd, "id", OracleDbType.Int32, id)
                        Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
                        cmd.ExecuteNonQuery()
                    End Using
                Next
                Return id
            End Function)
    End Function

    Public Sub MarquerLu(id As Integer, matricule As String) Implements IMessagerieDao.MarquerLu
        ExecuterMiseAJour("Mise à jour de la lecture impossible.",
            "update MESSAGE_DESTINATAIRE set dst_date_lecture = systimestamp
              where msg_id = :id and col_matricule = :matricule and dst_date_lecture is null",
            Sub(cmd)
                Parametre(cmd, "id", OracleDbType.Int32, id)
                Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
            End Sub)
    End Sub

    Public Function CompterNonLus(matricule As String) As Integer Implements IMessagerieDao.CompterNonLus
        Return Lister("Lecture des messages impossible.",
                      "select count(*) from MESSAGE_DESTINATAIRE where col_matricule = :matricule and dst_date_lecture is null",
                      Sub(cmd) Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule),
                      Function(l) l.GetInt32(0)).Single()
    End Function

End Class
