Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>
''' Implémentation Oracle de <see cref="IRapportDao"/> (tables RAPPORT_VISITE, PRESENTER, OFFRIR,
''' SESSION_SAISIE et vue V_RAPPORT_DETAIL).
''' </summary>
Public Class RapportDao
    Inherits DaoOracle
    Implements IRapportDao

    Public Sub New(connexion As ConnexionOracle)
        MyBase.New(connexion)
    End Sub

    Public Function ListerParPerimetre(perimetre As Perimetre, depuis As Date, inclureBrouillons As Boolean) As List(Of RapportResume) Implements IRapportDao.ListerParPerimetre
        Dim sql = $"select rap_num, rap_date_visite, pra_nom_complet, pra_ville, remplacant_nom_complet,
                           motif, rap_etat, rap_date_saisie, rap_date_modif, d.col_matricule, c.col_prenom || ' ' || c.col_nom
                      from V_RAPPORT_DETAIL d
                      join COLLABORATEUR c on c.col_matricule = d.col_matricule
                     where {SqlPerimetre.Condition("d.col_matricule", perimetre)}
                       and rap_date_visite >= :depuis
                       and (:brouillons = 1 or rap_etat = 'V')
                     order by rap_date_visite desc, rap_num desc"

        Return Lister("Lecture des comptes-rendus impossible.", sql,
                      Sub(cmd)
                          SqlPerimetre.Lier(cmd, perimetre)
                          Parametre(cmd, "depuis", OracleDbType.Date, depuis)
                          Parametre(cmd, "brouillons", OracleDbType.Int32, If(inclureBrouillons, 1, 0))
                      End Sub,
                      Function(l) New RapportResume() With {
                          .Numero = l.GetInt32(0),
                          .DateVisite = l.GetDateTime(1),
                          .Praticien = l.GetString(2),
                          .Ville = TexteOuRien(l, 3),
                          .Remplacant = TexteOuRien(l, 4),
                          .Motif = TexteOuRien(l, 5),
                          .Etat = VersEtat(l.GetString(6)),
                          .DateSaisie = l.GetDateTime(7),
                          .DateModification = DateOuRien(l, 8),
                          .MatriculeAuteur = l.GetString(9),
                          .Auteur = l.GetString(10)
                      })
    End Function

    Public Function Charger(numero As Integer) As RapportVisite Implements IRapportDao.Charger
        Const sqlRapport As String =
            "select r.rap_num, r.col_matricule, r.pra_num, p.pra_nom || ' ' || p.pra_prenom,
                    r.pra_num_remplacant, rp.pra_nom || ' ' || rp.pra_prenom,
                    r.rap_date_visite, r.mot_code, r.rap_motif_autre, r.rap_bilan, r.rap_coef_confiance,
                    r.rap_date_prochaine_visite, r.rap_etat, r.rap_date_saisie, r.rap_date_modif, r.rap_date_validation,
                    mo.mot_libelle
               from RAPPORT_VISITE r
               join PRATICIEN p       on p.pra_num = r.pra_num
               left join PRATICIEN rp on rp.pra_num = r.pra_num_remplacant
               left join MOTIF mo     on mo.mot_code = r.mot_code
              where r.rap_num = :numero"
        Const sqlPresentes As String =
            "select p.med_depot_legal, m.med_nom_commercial
               from PRESENTER p join MEDICAMENT m on m.med_depot_legal = p.med_depot_legal
              where p.rap_num = :numero order by p.pst_ordre"
        Const sqlEchantillons As String =
            "select o.med_depot_legal, m.med_nom_commercial, o.off_quantite
               from OFFRIR o
               join MEDICAMENT m on m.med_depot_legal = o.med_depot_legal
              where o.rap_num = :numero
              order by m.med_nom_commercial"

        Return Executer("Lecture du compte-rendu impossible.",
            Function(cnx)
                Dim parNumero As Action(Of OracleCommand) = Sub(cmd) Parametre(cmd, "numero", OracleDbType.Int32, numero)

                Dim rapport = Lister(cnx, sqlRapport, parNumero,
                    Function(l) New RapportVisite() With {
                        .Numero = l.GetInt32(0),
                        .MatriculeAuteur = l.GetString(1),
                        .NumeroPraticien = l.GetInt32(2),
                        .NomPraticien = l.GetString(3),
                        .NumeroRemplacant = EntierOuRien(l, 4),
                        .NomRemplacant = If(l.IsDBNull(4), Nothing, l.GetString(5)),
                        .DateVisite = l.GetDateTime(6),
                        .CodeMotif = TexteOuRien(l, 7),
                        .PrecisionMotif = TexteOuRien(l, 8),
                        .Bilan = TexteOuRien(l, 9),
                        .CoefConfiance = EntierOuRien(l, 10),
                        .DateProchaineVisite = DateOuRien(l, 11),
                        .Etat = VersEtat(l.GetString(12)),
                        .DateSaisie = l.GetDateTime(13),
                        .DateModification = DateOuRien(l, 14),
                        .DateValidation = DateOuRien(l, 15),
                        .LibelleMotif = TexteOuRien(l, 16)
                    }).FirstOrDefault()
                If rapport Is Nothing Then Return Nothing

                Dim presentes = Lister(cnx, sqlPresentes, parNumero, Function(l) (Depot:=l.GetString(0), Nom:=l.GetString(1)))
                rapport.ProduitsPresentes = presentes.Select(Function(x) x.Depot).ToList()
                rapport.NomsProduitsPresentes = presentes.Select(Function(x) x.Nom).ToList()
                rapport.Echantillons = Lister(cnx, sqlEchantillons, parNumero,
                    Function(l) New EchantillonOffert() With {
                        .DepotLegal = l.GetString(0), .NomCommercial = l.GetString(1), .Quantite = l.GetInt32(2)})
                Return rapport
            End Function)
    End Function

    Public Function Creer(rapport As RapportVisite) As Integer Implements IRapportDao.Creer
        ArgumentNullException.ThrowIfNull(rapport)
        Const sql As String =
            "insert into RAPPORT_VISITE (col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code,
                                         rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat)
             values (:auteur, :praticien, :remplacant, :date_visite, :motif,
                     :precision, :bilan, :confiance, :prochaine, :etat)
             returning rap_num into :numero"

        Return ExecuterTransaction("Enregistrement du compte-rendu impossible.",
            Function(cnx)
                Dim numero As Integer
                Using cmd = Commande(cnx, sql)
                    Parametre(cmd, "auteur", OracleDbType.Varchar2, rapport.MatriculeAuteur)
                    ParametresRapport(cmd, rapport)
                    Dim sortie = cmd.Parameters.Add("numero", OracleDbType.Int32, Data.ParameterDirection.Output)
                    cmd.ExecuteNonQuery()
                    numero = EntierSortie(sortie)
                End Using
                EcrireLignes(cnx, numero, rapport)
                Return numero
            End Function)
    End Function

    Public Sub Modifier(rapport As RapportVisite) Implements IRapportDao.Modifier
        ArgumentNullException.ThrowIfNull(rapport)
        If rapport.EstNouveau Then Throw New ArgumentException("Le rapport n'a pas encore été créé.", NameOf(rapport))

        ' L'auteur n'est jamais modifié (le déclencheur le refuserait de toute façon)
        Const sql As String =
            "update RAPPORT_VISITE
                set pra_num = :praticien, pra_num_remplacant = :remplacant, rap_date_visite = :date_visite,
                    mot_code = :motif, rap_motif_autre = :precision, rap_bilan = :bilan,
                    rap_coef_confiance = :confiance, rap_date_prochaine_visite = :prochaine, rap_etat = :etat
              where rap_num = :numero"

        ExecuterTransaction("Modification du compte-rendu impossible.",
            Function(cnx)
                Using cmd = Commande(cnx, sql)
                    ParametresRapport(cmd, rapport)
                    Parametre(cmd, "numero", OracleDbType.Int32, rapport.Numero.Value)
                    cmd.ExecuteNonQuery()
                End Using
                For Each table In {"PRESENTER", "OFFRIR"}
                    Using cmd = Commande(cnx, $"delete from {table} where rap_num = :numero")
                        Parametre(cmd, "numero", OracleDbType.Int32, rapport.Numero.Value)
                        cmd.ExecuteNonQuery()
                    End Using
                Next
                EcrireLignes(cnx, rapport.Numero.Value, rapport)
                Return True
            End Function)
    End Sub

    Public Function SupprimerBrouillon(numero As Integer) As Boolean Implements IRapportDao.SupprimerBrouillon
        ' Produits, échantillons et sessions sont supprimés en cascade
        Return ExecuterMiseAJour("Suppression du compte-rendu impossible.",
                                 "delete from RAPPORT_VISITE where rap_num = :numero and rap_etat = 'B'",
                                 Sub(cmd) Parametre(cmd, "numero", OracleDbType.Int32, numero)) > 0
    End Function

    Public Sub AjouterSessionSaisie(numero As Integer, matricule As String, debut As DateTime, fin As DateTime) Implements IRapportDao.AjouterSessionSaisie
        ExecuterMiseAJour("Enregistrement du temps de saisie impossible.",
            "insert into SESSION_SAISIE (rap_num, col_matricule, ses_debut, ses_fin)
             values (:numero, :matricule, :debut, :fin)",
            Sub(cmd)
                Parametre(cmd, "numero", OracleDbType.Int32, numero)
                Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
                Parametre(cmd, "debut", OracleDbType.TimeStamp, debut)
                Parametre(cmd, "fin", OracleDbType.TimeStamp, fin)
            End Sub)
    End Sub

    ' ------------------------------------------------------------------

    Private Shared Sub ParametresRapport(cmd As OracleCommand, r As RapportVisite)
        Parametre(cmd, "praticien", OracleDbType.Int32, r.NumeroPraticien)
        Parametre(cmd, "remplacant", OracleDbType.Int32, r.NumeroRemplacant)
        Parametre(cmd, "date_visite", OracleDbType.Date, r.DateVisite)
        Parametre(cmd, "motif", OracleDbType.Varchar2, r.CodeMotif)
        Parametre(cmd, "precision", OracleDbType.Varchar2, r.PrecisionMotif)
        Parametre(cmd, "bilan", OracleDbType.Varchar2, r.Bilan)
        Parametre(cmd, "confiance", OracleDbType.Int32, r.CoefConfiance)
        Parametre(cmd, "prochaine", OracleDbType.Date, r.DateProchaineVisite)
        Parametre(cmd, "etat", OracleDbType.Char, If(r.Etat = EtatRapport.Valide, "V", "B"))
    End Sub

    ''' <summary>Insère les produits présentés (ordre 1, 2) et les échantillons du rapport.</summary>
    Private Shared Sub EcrireLignes(cnx As OracleConnection, numero As Integer, r As RapportVisite)
        For i = 0 To r.ProduitsPresentes.Count - 1
            Using cmd = Commande(cnx, "insert into PRESENTER (rap_num, med_depot_legal, pst_ordre) values (:numero, :produit, :ordre)")
                Parametre(cmd, "numero", OracleDbType.Int32, numero)
                Parametre(cmd, "produit", OracleDbType.Varchar2, r.ProduitsPresentes(i))
                Parametre(cmd, "ordre", OracleDbType.Int32, i + 1)
                cmd.ExecuteNonQuery()
            End Using
        Next
        For Each e In r.Echantillons
            Using cmd = Commande(cnx, "insert into OFFRIR (rap_num, med_depot_legal, off_quantite) values (:numero, :produit, :quantite)")
                Parametre(cmd, "numero", OracleDbType.Int32, numero)
                Parametre(cmd, "produit", OracleDbType.Varchar2, e.DepotLegal)
                Parametre(cmd, "quantite", OracleDbType.Int32, e.Quantite)
                cmd.ExecuteNonQuery()
            End Using
        Next
    End Sub

    Private Shared Function VersEtat(code As String) As EtatRapport
        Return If(code = "V", EtatRapport.Valide, EtatRapport.Brouillon)
    End Function

End Class
