Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>Implémentation Oracle de <see cref="IEchantillonDao"/> (table DOTATION, vue V_STOCK_ECHANTILLON).</summary>
Public Class EchantillonDao
    Inherits DaoOracle
    Implements IEchantillonDao

    Public Sub New(connexion As ConnexionOracle)
        MyBase.New(connexion)
    End Sub

    Public Function Stock(perimetre As Perimetre, mois As Date) As List(Of LigneStock) Implements IEchantillonDao.Stock
        Dim sql = $"select s.col_matricule, c.col_prenom || ' ' || c.col_nom, s.med_depot_legal, m.med_nom_commercial,
                           s.mois, s.qte_attribuee, s.qte_distribuee
                      from V_STOCK_ECHANTILLON s
                      join COLLABORATEUR c on c.col_matricule = s.col_matricule
                      join MEDICAMENT m    on m.med_depot_legal = s.med_depot_legal
                     where s.mois = :mois
                       and {SqlPerimetre.Condition("s.col_matricule", perimetre)}
                     order by c.col_nom, c.col_prenom, m.med_nom_commercial"
        Return Lister("Lecture du stock d'échantillons impossible.", sql,
            Sub(cmd)
                Parametre(cmd, "mois", OracleDbType.Date, PremierDuMois(mois))
                SqlPerimetre.Lier(cmd, perimetre)
            End Sub,
            Function(l) New LigneStock() With {
                .Matricule = l.GetString(0), .Visiteur = l.GetString(1), .DepotLegal = l.GetString(2),
                .Produit = l.GetString(3), .Mois = l.GetDateTime(4), .Attribue = l.GetInt32(5), .Distribue = l.GetInt32(6)})
    End Function

    Public Sub EnregistrerDotation(matricule As String, depotLegal As String, mois As Date, quantite As Integer, saisiPar As String) Implements IEchantillonDao.EnregistrerDotation
        ' Une seule dotation par visiteur, produit et mois : création ou remplacement
        Const sql As String =
            "merge into DOTATION d
             using (select :matricule as col_matricule, :produit as med_depot_legal, :mois as dot_mois from dual) s
                on (d.col_matricule = s.col_matricule and d.med_depot_legal = s.med_depot_legal and d.dot_mois = s.dot_mois)
              when matched then update
                   set d.dot_quantite = :quantite, d.col_matricule_saisie = :saisi_par, d.dot_date_saisie = systimestamp
              when not matched then insert (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie)
                   values (s.col_matricule, s.med_depot_legal, s.dot_mois, :quantite, :saisi_par)"
        ExecuterMiseAJour("Enregistrement de la dotation impossible.", sql,
            Sub(cmd)
                Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
                Parametre(cmd, "produit", OracleDbType.Varchar2, depotLegal)
                Parametre(cmd, "mois", OracleDbType.Date, PremierDuMois(mois))
                Parametre(cmd, "quantite", OracleDbType.Int32, quantite)
                Parametre(cmd, "saisi_par", OracleDbType.Varchar2, saisiPar)
            End Sub)
    End Sub

    Public Function SupprimerDotation(matricule As String, depotLegal As String, mois As Date) As Boolean Implements IEchantillonDao.SupprimerDotation
        Return ExecuterMiseAJour("Suppression de la dotation impossible.",
            "delete from DOTATION where col_matricule = :matricule and med_depot_legal = :produit and dot_mois = :mois",
            Sub(cmd)
                Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
                Parametre(cmd, "produit", OracleDbType.Varchar2, depotLegal)
                Parametre(cmd, "mois", OracleDbType.Date, PremierDuMois(mois))
            End Sub) > 0
    End Function

    Private Shared Function PremierDuMois(d As Date) As Date
        Return New Date(d.Year, d.Month, 1)
    End Function

End Class
