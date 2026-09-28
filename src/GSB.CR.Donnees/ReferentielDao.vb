Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>Implémentation Oracle de <see cref="IReferentielDao"/>.</summary>
Public Class ReferentielDao
    Inherits DaoOracle
    Implements IReferentielDao

    ' Colonnes communes aux requêtes qui lisent un praticien (voir LirePraticien)
    Private Const ColonnesPraticien As String =
        "p.pra_num, p.pra_nom, p.pra_prenom, p.pra_adresse, p.pra_cp, p.pra_ville, p.pra_telephone,
         p.pra_email, p.typ_code, t.typ_libelle, p.pra_coef_notoriete, p.pra_actif"

    Public Sub New(connexion As ConnexionOracle)
        MyBase.New(connexion)
    End Sub

    Public Function ListerPortefeuille(matricule As String) As List(Of Praticien) Implements IReferentielDao.ListerPortefeuille
        Dim sql = $"select {ColonnesPraticien}
                      from PRATICIEN p
                      join TYPE_PRATICIEN t on t.typ_code = p.typ_code
                      join PORTEFEUILLE pf  on pf.pra_num = p.pra_num and pf.ptf_date_fin is null
                     where pf.col_matricule = :matricule
                       and p.pra_actif = 'O'
                     order by p.pra_nom, p.pra_prenom"
        Return Lister("Lecture du portefeuille impossible.", sql,
                      Sub(cmd) Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule),
                      AddressOf LirePraticien)
    End Function

    Public Function RechercherPraticiens(debut As String, maximum As Integer) As List(Of Praticien) Implements IReferentielDao.RechercherPraticiens
        Dim sql = $"select * from (
                      select {ColonnesPraticien}
                        from PRATICIEN p
                        join TYPE_PRATICIEN t on t.typ_code = p.typ_code
                       where p.pra_actif = 'O'
                         and (lower(p.pra_nom) like lower(:debut) || '%' or lower(p.pra_prenom) like lower(:debut) || '%')
                       order by p.pra_nom, p.pra_prenom)
                     where rownum <= :maximum"
        Return Lister("Recherche de praticiens impossible.", sql,
                      Sub(cmd)
                          Parametre(cmd, "debut", OracleDbType.Varchar2, If(debut, "").Trim())
                          Parametre(cmd, "maximum", OracleDbType.Int32, maximum)
                      End Sub,
                      AddressOf LirePraticien)
    End Function

    Public Function TrouverPraticien(numero As Integer) As Praticien Implements IReferentielDao.TrouverPraticien
        Dim sql = $"select {ColonnesPraticien}
                      from PRATICIEN p
                      join TYPE_PRATICIEN t on t.typ_code = p.typ_code
                     where p.pra_num = :numero"
        Return Lister("Lecture du praticien impossible.", sql,
                      Sub(cmd) Parametre(cmd, "numero", OracleDbType.Int32, numero),
                      AddressOf LirePraticien).FirstOrDefault()
    End Function

    Public Function CreerPraticien(praticien As Praticien) As Integer Implements IReferentielDao.CreerPraticien
        ArgumentNullException.ThrowIfNull(praticien)
        Const sql As String =
            "insert into PRATICIEN (pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone,
                                    pra_email, pra_coef_notoriete, typ_code)
             values (:nom, :prenom, :adresse, :cp, :ville, :telephone, :email, :notoriete, :type)
             returning pra_num into :numero"

        Return Executer("Création du praticien impossible.",
            Function(cnx)
                Using cmd = Commande(cnx, sql)
                    Parametre(cmd, "nom", OracleDbType.Varchar2, praticien.Nom)
                    Parametre(cmd, "prenom", OracleDbType.Varchar2, praticien.Prenom)
                    Parametre(cmd, "adresse", OracleDbType.Varchar2, praticien.Adresse)
                    Parametre(cmd, "cp", OracleDbType.Varchar2, praticien.CodePostal)
                    Parametre(cmd, "ville", OracleDbType.Varchar2, praticien.Ville)
                    Parametre(cmd, "telephone", OracleDbType.Varchar2, praticien.Telephone)
                    Parametre(cmd, "email", OracleDbType.Varchar2, praticien.Email)
                    Parametre(cmd, "notoriete", OracleDbType.Decimal, praticien.CoefNotoriete)
                    Parametre(cmd, "type", OracleDbType.Varchar2, praticien.CodeType)
                    Dim numero = cmd.Parameters.Add("numero", OracleDbType.Int32, Data.ParameterDirection.Output)
                    cmd.ExecuteNonQuery()
                    Return EntierSortie(numero)
                End Using
            End Function)
    End Function

    Public Function ListerTypesPraticien() As List(Of TypePraticien) Implements IReferentielDao.ListerTypesPraticien
        Return Lister("Lecture des types de praticien impossible.",
                      "select typ_code, typ_libelle from TYPE_PRATICIEN order by typ_libelle",
                      Nothing,
                      Function(l) New TypePraticien() With {.Code = l.GetString(0), .Libelle = l.GetString(1)})
    End Function

    Public Function ListerMedicaments() As List(Of Medicament) Implements IReferentielDao.ListerMedicaments
        Const sql As String =
            "select m.med_depot_legal, m.med_nom_commercial, m.fam_code, f.fam_libelle,
                    m.med_prix_echantillon, m.med_actif
               from MEDICAMENT m
               join FAMILLE f on f.fam_code = m.fam_code
              order by m.med_nom_commercial"
        Return Lister("Lecture des médicaments impossible.", sql, Nothing,
                      Function(l) New Medicament() With {
                          .DepotLegal = l.GetString(0),
                          .NomCommercial = l.GetString(1),
                          .CodeFamille = l.GetString(2),
                          .LibelleFamille = l.GetString(3),
                          .PrixEchantillon = l.GetDecimal(4),
                          .Actif = l.GetString(5) = "O"
                      })
    End Function

    Public Function ListerMotifs() As List(Of Motif) Implements IReferentielDao.ListerMotifs
        Return Lister("Lecture des motifs impossible.",
                      "select mot_code, mot_libelle, mot_actif from MOTIF order by mot_ordre",
                      Nothing,
                      Function(l) New Motif() With {.Code = l.GetString(0), .Libelle = l.GetString(1), .Actif = l.GetString(2) = "O"})
    End Function

    Private Shared Function LirePraticien(l As OracleDataReader) As Praticien
        Return New Praticien() With {
            .Numero = l.GetInt32(0),
            .Nom = l.GetString(1),
            .Prenom = l.GetString(2),
            .Adresse = TexteOuRien(l, 3),
            .CodePostal = TexteOuRien(l, 4),
            .Ville = TexteOuRien(l, 5),
            .Telephone = TexteOuRien(l, 6),
            .Email = TexteOuRien(l, 7),
            .CodeType = l.GetString(8),
            .LibelleType = l.GetString(9),
            .CoefNotoriete = DecimalOuRien(l, 10),
            .Actif = l.GetString(11) = "O"
        }
    End Function

End Class
