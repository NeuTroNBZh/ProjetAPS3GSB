Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>Implémentation Oracle de <see cref="IAdministrationDao"/>.</summary>
Public Class AdministrationDao
    Inherits DaoOracle
    Implements IAdministrationDao

    Public Sub New(connexion As ConnexionOracle)
        MyBase.New(connexion)
    End Sub

    ' ------------------------------------------------------------------
    ' Organisation
    ' ------------------------------------------------------------------

    Public Function ListerRegions() As List(Of Region) Implements IAdministrationDao.ListerRegions
        Return Lister("Lecture des régions impossible.", "select reg_code, reg_nom, sec_code from REGION order by reg_nom", Nothing,
                      Function(l) New Region() With {.Code = l.GetString(0), .Nom = l.GetString(1), .CodeSecteur = l.GetString(2)})
    End Function

    Public Function ListerSecteurs() As List(Of Secteur) Implements IAdministrationDao.ListerSecteurs
        Return Lister("Lecture des secteurs impossible.", "select sec_code, sec_libelle from SECTEUR order by sec_libelle", Nothing,
                      Function(l) New Secteur() With {.Code = l.GetString(0), .Libelle = l.GetString(1)})
    End Function

    ' ------------------------------------------------------------------
    ' Collaborateurs
    ' ------------------------------------------------------------------

    Public Function ListerCollaborateurs() As List(Of FicheCollaborateur) Implements IAdministrationDao.ListerCollaborateurs
        ' Le mot de passe haché n'est jamais relu ici
        Const sql As String =
            "select c.col_matricule, c.col_nom, c.col_prenom, c.col_adresse, c.col_cp, c.col_ville, c.col_telephone, c.col_email,
                    c.col_date_embauche, c.col_date_depart, c.col_login, c.col_mdp_a_changer, c.col_nb_echecs, c.col_verrouille,
                    c.col_derniere_connexion,
                    a.pro_code, a.reg_code, r.reg_nom, coalesce(a.sec_code, r.sec_code), s.sec_libelle, a.aff_date_debut,
                    (select count(*) from PORTEFEUILLE pf where pf.col_matricule = c.col_matricule and pf.ptf_date_fin is null)
               from COLLABORATEUR c
               left join AFFECTATION a on a.col_matricule = c.col_matricule and a.aff_date_fin is null
               left join REGION r      on r.reg_code = a.reg_code
               left join SECTEUR s     on s.sec_code = coalesce(a.sec_code, r.sec_code)
              order by c.col_nom, c.col_prenom"
        Return Lister("Lecture des collaborateurs impossible.", sql, Nothing,
            Function(l) New FicheCollaborateur() With {
                .Collaborateur = New Collaborateur() With {
                    .Matricule = l.GetString(0), .Nom = l.GetString(1), .Prenom = l.GetString(2),
                    .Adresse = TexteOuRien(l, 3), .CodePostal = TexteOuRien(l, 4), .Ville = TexteOuRien(l, 5),
                    .Telephone = TexteOuRien(l, 6), .Email = TexteOuRien(l, 7),
                    .DateEmbauche = l.GetDateTime(8), .DateDepart = DateOuRien(l, 9), .Login = l.GetString(10),
                    .MotDePasseAChanger = l.GetString(11) = "O", .NbEchecsConnexion = l.GetInt32(12),
                    .Verrouille = l.GetString(13) = "O", .DerniereConnexion = DateOuRien(l, 14)},
                .AffectationEnCours = If(l.IsDBNull(15), Nothing, New Affectation() With {
                    .Profil = ProfilCodes.DepuisCode(l.GetString(15)), .CodeRegion = TexteOuRien(l, 16), .NomRegion = TexteOuRien(l, 17),
                    .CodeSecteur = TexteOuRien(l, 18), .LibelleSecteur = TexteOuRien(l, 19), .DateDebut = l.GetDateTime(20)}),
                .TaillePortefeuille = l.GetInt32(21)})
    End Function

    Public Function ListerAffectations(matricule As String) As List(Of Affectation) Implements IAdministrationDao.ListerAffectations
        Const sql As String =
            "select a.pro_code, a.reg_code, r.reg_nom, coalesce(a.sec_code, r.sec_code), s.sec_libelle, a.aff_date_debut, a.aff_date_fin
               from AFFECTATION a
               left join REGION r  on r.reg_code = a.reg_code
               left join SECTEUR s on s.sec_code = coalesce(a.sec_code, r.sec_code)
              where a.col_matricule = :matricule
              order by a.aff_date_debut desc"
        Return Lister("Lecture des affectations impossible.", sql,
                      Sub(cmd) Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule),
                      Function(l) New Affectation() With {
                          .Profil = ProfilCodes.DepuisCode(l.GetString(0)), .CodeRegion = TexteOuRien(l, 1), .NomRegion = TexteOuRien(l, 2),
                          .CodeSecteur = TexteOuRien(l, 3), .LibelleSecteur = TexteOuRien(l, 4),
                          .DateDebut = l.GetDateTime(5), .DateFin = DateOuRien(l, 6)})
    End Function

    Public Sub CreerCollaborateur(c As Collaborateur, motDePasseHache As String, affectation As Affectation) Implements IAdministrationDao.CreerCollaborateur
        ExecuterTransaction("Création du collaborateur impossible.",
            Function(cnx)
                Using cmd = Commande(cnx,
                    "insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone,
                                                col_email, col_date_embauche, col_login, col_mdp, col_mdp_a_changer)
                     values (:matricule, :nom, :prenom, :adresse, :cp, :ville, :telephone, :email, :embauche, :login, :mdp, 'O')")
                    ParametresIdentite(cmd, c)
                    Parametre(cmd, "embauche", OracleDbType.Date, c.DateEmbauche.Date)
                    Parametre(cmd, "mdp", OracleDbType.Varchar2, motDePasseHache)
                    cmd.ExecuteNonQuery()
                End Using
                InsererAffectation(cnx, c.Matricule, affectation, c.DateEmbauche.Date)
                Return True
            End Function)
    End Sub

    Public Sub ModifierCollaborateur(c As Collaborateur) Implements IAdministrationDao.ModifierCollaborateur
        ExecuterMiseAJour("Modification du collaborateur impossible.",
            "update COLLABORATEUR
                set col_nom = :nom, col_prenom = :prenom, col_adresse = :adresse, col_cp = :cp, col_ville = :ville,
                    col_telephone = :telephone, col_email = :email, col_login = :login
              where col_matricule = :matricule",
            Sub(cmd) ParametresIdentite(cmd, c))
    End Sub

    Public Sub ChangerAffectation(matricule As String, nouvelle As Affectation, dateEffet As Date) Implements IAdministrationDao.ChangerAffectation
        ExecuterTransaction("Changement d'affectation impossible.",
            Function(cnx)
                Using cmd = Commande(cnx, "update AFFECTATION set aff_date_fin = :veille where col_matricule = :matricule and aff_date_fin is null")
                    Parametre(cmd, "veille", OracleDbType.Date, dateEffet.Date.AddDays(-1))
                    Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
                    cmd.ExecuteNonQuery()
                End Using
                InsererAffectation(cnx, matricule, nouvelle, dateEffet.Date)
                Return True
            End Function)
    End Sub

    Public Sub EnregistrerDepart(matricule As String, dateDepart As Date) Implements IAdministrationDao.EnregistrerDepart
        ExecuterTransaction("Enregistrement du départ impossible.",
            Function(cnx)
                For Each sql In {"update COLLABORATEUR set col_date_depart = :jour where col_matricule = :matricule",
                                 "update AFFECTATION set aff_date_fin = :jour where col_matricule = :matricule and aff_date_fin is null",
                                 "update PORTEFEUILLE set ptf_date_fin = :jour where col_matricule = :matricule and ptf_date_fin is null"}
                    Using cmd = Commande(cnx, sql)
                        Parametre(cmd, "jour", OracleDbType.Date, dateDepart.Date)
                        Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
                        cmd.ExecuteNonQuery()
                    End Using
                Next
                Return True
            End Function)
    End Sub

    Public Sub ReinitialiserMotDePasse(matricule As String, motDePasseHache As String) Implements IAdministrationDao.ReinitialiserMotDePasse
        ExecuterMiseAJour("Réinitialisation du mot de passe impossible.",
            "update COLLABORATEUR
                set col_mdp = :mdp, col_mdp_a_changer = 'O', col_nb_echecs = 0, col_verrouille = 'N'
              where col_matricule = :matricule",
            Sub(cmd)
                Parametre(cmd, "mdp", OracleDbType.Varchar2, motDePasseHache)
                Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
            End Sub)
    End Sub

    Public Sub DefinirVerrouillage(matricule As String, verrouille As Boolean) Implements IAdministrationDao.DefinirVerrouillage
        ExecuterMiseAJour("Modification du verrouillage impossible.",
            "update COLLABORATEUR set col_verrouille = :etat, col_nb_echecs = 0 where col_matricule = :matricule",
            Sub(cmd)
                Parametre(cmd, "etat", OracleDbType.Char, If(verrouille, "O", "N"))
                Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
            End Sub)
    End Sub

    ' ------------------------------------------------------------------
    ' Portefeuilles
    ' ------------------------------------------------------------------

    Public Function ListerPraticiensSansVisiteur() As List(Of PraticienResume) Implements IAdministrationDao.ListerPraticiensSansVisiteur
        Const sql As String =
            "select p.pra_num, p.pra_nom, p.pra_prenom, t.typ_libelle, p.pra_cp, p.pra_ville, p.pra_telephone,
                    (select max(r.rap_date_visite) from RAPPORT_VISITE r where r.pra_num = p.pra_num and r.rap_etat = 'V')
               from PRATICIEN p
               join TYPE_PRATICIEN t on t.typ_code = p.typ_code
              where p.pra_actif = 'O'
                and not exists (select 1 from PORTEFEUILLE pf where pf.pra_num = p.pra_num and pf.ptf_date_fin is null)
              order by p.pra_nom, p.pra_prenom"
        Return Lister("Lecture des praticiens sans visiteur impossible.", sql, Nothing,
            Function(l) New PraticienResume() With {
                .Numero = l.GetInt32(0), .Nom = l.GetString(1), .Prenom = l.GetString(2), .LibelleType = l.GetString(3),
                .CodePostal = TexteOuRien(l, 4), .Ville = TexteOuRien(l, 5), .Telephone = TexteOuRien(l, 6),
                .DateDerniereVisite = DateOuRien(l, 7)})
    End Function

    Public Sub AttribuerPraticiens(numeros As IEnumerable(Of Integer), matricule As String, dateEffet As Date) Implements IAdministrationDao.AttribuerPraticiens
        Dim liste = numeros.ToList()
        ExecuterTransaction("Attribution des praticiens impossible.",
            Function(cnx)
                For Each numero In liste
                    Attribuer(cnx, numero, matricule, dateEffet.Date)
                Next
                Return True
            End Function)
    End Sub

    Public Function TransfererPortefeuille(deMatricule As String, versMatricule As String, dateEffet As Date) As Integer Implements IAdministrationDao.TransfererPortefeuille
        Return ExecuterTransaction("Transfert du portefeuille impossible.",
            Function(cnx)
                Dim numeros = Lister(cnx, "select pra_num from PORTEFEUILLE where col_matricule = :matricule and ptf_date_fin is null",
                                     Sub(cmd) Parametre(cmd, "matricule", OracleDbType.Varchar2, deMatricule),
                                     Function(l) l.GetInt32(0))
                For Each numero In numeros
                    Attribuer(cnx, numero, versMatricule, dateEffet.Date)
                Next
                Return numeros.Count
            End Function)
    End Function

    ''' <summary>
    ''' Un seul visiteur en cours par praticien : le suivi en cours est clos la veille, ou supprimé
    ''' s'il commençait le jour même (correction d'une attribution), puis le nouveau suivi est ouvert.
    ''' </summary>
    Private Shared Sub Attribuer(cnx As OracleConnection, numero As Integer, matricule As String, dateEffet As Date)
        For Each sql In {"delete from PORTEFEUILLE where pra_num = :numero and ptf_date_fin is null and ptf_date_debut >= :jour",
                         "update PORTEFEUILLE set ptf_date_fin = :jour - 1 where pra_num = :numero and ptf_date_fin is null"}
            Using cmd = Commande(cnx, sql)
                Parametre(cmd, "numero", OracleDbType.Int32, numero)
                Parametre(cmd, "jour", OracleDbType.Date, dateEffet)
                cmd.ExecuteNonQuery()
            End Using
        Next
        Using cmd = Commande(cnx, "insert into PORTEFEUILLE (pra_num, col_matricule, ptf_date_debut) values (:numero, :matricule, :jour)")
            Parametre(cmd, "numero", OracleDbType.Int32, numero)
            Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
            Parametre(cmd, "jour", OracleDbType.Date, dateEffet)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    ' ------------------------------------------------------------------
    ' Référentiels
    ' ------------------------------------------------------------------

    Public Sub ModifierPraticien(p As Praticien) Implements IAdministrationDao.ModifierPraticien
        ExecuterMiseAJour("Modification du praticien impossible.",
            "update PRATICIEN
                set pra_nom = :nom, pra_prenom = :prenom, pra_adresse = :adresse, pra_cp = :cp, pra_ville = :ville,
                    pra_telephone = :telephone, pra_email = :email, pra_coef_notoriete = :notoriete,
                    typ_code = :type, pra_actif = :actif
              where pra_num = :numero",
            Sub(cmd)
                Parametre(cmd, "nom", OracleDbType.Varchar2, p.Nom)
                Parametre(cmd, "prenom", OracleDbType.Varchar2, p.Prenom)
                Parametre(cmd, "adresse", OracleDbType.Varchar2, p.Adresse)
                Parametre(cmd, "cp", OracleDbType.Varchar2, p.CodePostal)
                Parametre(cmd, "ville", OracleDbType.Varchar2, p.Ville)
                Parametre(cmd, "telephone", OracleDbType.Varchar2, p.Telephone)
                Parametre(cmd, "email", OracleDbType.Varchar2, p.Email)
                Parametre(cmd, "notoriete", OracleDbType.Decimal, p.CoefNotoriete)
                Parametre(cmd, "type", OracleDbType.Varchar2, p.CodeType)
                Parametre(cmd, "actif", OracleDbType.Char, If(p.Actif, "O", "N"))
                Parametre(cmd, "numero", OracleDbType.Int32, p.Numero)
            End Sub)
    End Sub

    Public Sub ModifierMedicament(depotLegal As String, prixEchantillon As Decimal, actif As Boolean) Implements IAdministrationDao.ModifierMedicament
        ExecuterMiseAJour("Modification du médicament impossible.",
            "update MEDICAMENT set med_prix_echantillon = :prix, med_actif = :actif where med_depot_legal = :depot",
            Sub(cmd)
                Parametre(cmd, "prix", OracleDbType.Decimal, prixEchantillon)
                Parametre(cmd, "actif", OracleDbType.Char, If(actif, "O", "N"))
                Parametre(cmd, "depot", OracleDbType.Varchar2, depotLegal)
            End Sub)
    End Sub

    Public Sub CreerMotif(code As String, libelle As String) Implements IAdministrationDao.CreerMotif
        ExecuterTransaction("Création du motif impossible.",
            Function(cnx)
                Dim ordre = Lister(cnx, "select nvl(max(mot_ordre), 0) + 1 from MOTIF where mot_code <> 'AUTRE'", Nothing,
                                   Function(l) l.GetInt32(0)).Single()
                Using cmd = Commande(cnx, "insert into MOTIF (mot_code, mot_libelle, mot_ordre) values (:code, :libelle, :ordre)")
                    Parametre(cmd, "code", OracleDbType.Varchar2, code)
                    Parametre(cmd, "libelle", OracleDbType.Varchar2, libelle)
                    Parametre(cmd, "ordre", OracleDbType.Int32, ordre)
                    cmd.ExecuteNonQuery()
                End Using
                ' « Autre » reste toujours en fin de liste
                Using cmd = Commande(cnx, "update MOTIF set mot_ordre = greatest(mot_ordre, :apres) where mot_code = 'AUTRE'")
                    Parametre(cmd, "apres", OracleDbType.Int32, ordre + 1)
                    cmd.ExecuteNonQuery()
                End Using
                Return True
            End Function)
    End Sub

    Public Sub ModifierMotif(code As String, libelle As String, actif As Boolean) Implements IAdministrationDao.ModifierMotif
        ExecuterMiseAJour("Modification du motif impossible.",
            "update MOTIF set mot_libelle = :libelle, mot_actif = :actif where mot_code = :code",
            Sub(cmd)
                Parametre(cmd, "libelle", OracleDbType.Varchar2, libelle)
                Parametre(cmd, "actif", OracleDbType.Char, If(actif, "O", "N"))
                Parametre(cmd, "code", OracleDbType.Varchar2, code)
            End Sub)
    End Sub

    ' ------------------------------------------------------------------
    ' Journal
    ' ------------------------------------------------------------------

    Public Function ListerJournal(debut As Date, fin As Date, texte As String, echecsSeulement As Boolean, maximum As Integer) As List(Of EntreeJournal) Implements IAdministrationDao.ListerJournal
        Const sql As String =
            "select * from (
               select j.jco_date, j.jco_login, c.col_prenom || ' ' || c.col_nom, j.jco_succes
                 from JOURNAL_CONNEXION j
                 left join COLLABORATEUR c on c.col_matricule = j.col_matricule
                where j.jco_date >= :debut and j.jco_date < :fin_exclue
                  and (:texte is null or lower(j.jco_login) like '%' || lower(:texte) || '%')
                  and (:echecs = 0 or j.jco_succes = 'N')
                order by j.jco_date desc)
             where rownum <= :maximum"
        Return Lister("Lecture du journal des connexions impossible.", sql,
            Sub(cmd)
                Parametre(cmd, "debut", OracleDbType.Date, debut.Date)
                Parametre(cmd, "fin_exclue", OracleDbType.Date, fin.Date.AddDays(1))
                Dim t = If(texte, "").Trim()
                Parametre(cmd, "texte", OracleDbType.Varchar2, If(t.Length = 0, Nothing, t))
                Parametre(cmd, "echecs", OracleDbType.Int32, If(echecsSeulement, 1, 0))
                Parametre(cmd, "maximum", OracleDbType.Int32, maximum)
            End Sub,
            Function(l) New EntreeJournal() With {
                .DateHeure = l.GetDateTime(0), .LoginSaisi = l.GetString(1),
                .NomCollaborateur = If(l.IsDBNull(2) OrElse l.GetString(2).Trim().Length = 0, Nothing, l.GetString(2)),
                .Succes = l.GetString(3) = "O"})
    End Function

    ' ------------------------------------------------------------------

    Private Shared Sub ParametresIdentite(cmd As OracleCommand, c As Collaborateur)
        Parametre(cmd, "matricule", OracleDbType.Varchar2, c.Matricule)
        Parametre(cmd, "nom", OracleDbType.Varchar2, c.Nom)
        Parametre(cmd, "prenom", OracleDbType.Varchar2, c.Prenom)
        Parametre(cmd, "adresse", OracleDbType.Varchar2, c.Adresse)
        Parametre(cmd, "cp", OracleDbType.Varchar2, c.CodePostal)
        Parametre(cmd, "ville", OracleDbType.Varchar2, c.Ville)
        Parametre(cmd, "telephone", OracleDbType.Varchar2, c.Telephone)
        Parametre(cmd, "email", OracleDbType.Varchar2, c.Email)
        Parametre(cmd, "login", OracleDbType.Varchar2, c.Login)
    End Sub

    Private Shared Sub InsererAffectation(cnx As OracleConnection, matricule As String, a As Affectation, dateDebut As Date)
        Using cmd = Commande(cnx,
            "insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut)
             values (:matricule, :profil, :region, :secteur, :debut)")
            Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
            Parametre(cmd, "profil", OracleDbType.Varchar2, ProfilCodes.VersCode(a.Profil))
            ' Visiteur / délégué : une région ; responsable : un secteur ; administrateur : rien
            Parametre(cmd, "region", OracleDbType.Varchar2, If(a.Profil = Profil.Visiteur OrElse a.Profil = Profil.Delegue, a.CodeRegion, Nothing))
            Parametre(cmd, "secteur", OracleDbType.Varchar2, If(a.Profil = Profil.Responsable, a.CodeSecteur, Nothing))
            Parametre(cmd, "debut", OracleDbType.Date, dateDebut)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

End Class
