Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>Implémentation Oracle de <see cref="IActiviteDao"/>.</summary>
Public Class ActiviteDao
    Inherits DaoOracle
    Implements IActiviteDao

    ' Filtre commun : CR validés de l'auteur sur la période (alias r pour RAPPORT_VISITE)
    Private Const Filtre As String =
        "r.col_matricule = :matricule and r.rap_etat = 'V' and r.rap_date_visite between :debut and :fin"

    Public Sub New(connexion As ConnexionOracle)
        MyBase.New(connexion)
    End Sub

    Public Function ChargerSynthese(matricule As String, debut As Date, fin As Date) As SyntheseActivite Implements IActiviteDao.ChargerSynthese
        Dim sqlIndicateurs = $"select count(*), count(distinct r.pra_num),
                                      sum(case when r.pra_num_remplacant is not null then 1 else 0 end),
                                      round(avg(r.rap_coef_confiance), 2)   -- round : avg a 38 chiffres, trop pour Decimal
                                 from RAPPORT_VISITE r
                                where {Filtre}"
        Dim sqlEchantillons = $"select m.med_depot_legal, m.med_nom_commercial, sum(o.off_quantite),
                                       sum(o.off_quantite * m.med_prix_echantillon)
                                  from OFFRIR o
                                  join RAPPORT_VISITE r on r.rap_num = o.rap_num
                                  join MEDICAMENT m     on m.med_depot_legal = o.med_depot_legal
                                 where {Filtre}
                                 group by m.med_depot_legal, m.med_nom_commercial
                                 order by 3 desc, 2"
        Dim sqlMotifs = $"select nvl(mo.mot_libelle, 'Non renseigné'), count(*)
                            from RAPPORT_VISITE r
                            left join MOTIF mo on mo.mot_code = r.mot_code
                           where {Filtre}
                           group by nvl(mo.mot_libelle, 'Non renseigné')
                           order by 2 desc, 1"
        Dim sqlMois = $"select trunc(r.rap_date_visite, 'MM'), count(*)
                          from RAPPORT_VISITE r
                         where {Filtre}
                         group by trunc(r.rap_date_visite, 'MM')
                         order by 1"
        Dim sqlProduits = $"select m.med_nom_commercial, count(*)
                              from PRESENTER p
                              join RAPPORT_VISITE r on r.rap_num = p.rap_num
                              join MEDICAMENT m     on m.med_depot_legal = p.med_depot_legal
                             where {Filtre}
                             group by m.med_nom_commercial
                             order by 2 desc, 1"
        Dim sqlTemps = $"select round(avg(t.duree_secondes)), round(nvl(sum(t.duree_secondes), 0))
                           from V_TEMPS_SAISIE t
                           join RAPPORT_VISITE r on r.rap_num = t.rap_num
                          where {Filtre}"
        Const sqlBrouillons As String =
            "select count(*) from RAPPORT_VISITE where col_matricule = :matricule and rap_etat = 'B'"

        Return Executer("Lecture de l'activité impossible.",
            Function(cnx)
                Dim parPeriode As Action(Of OracleCommand) =
                    Sub(cmd)
                        Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule)
                        Parametre(cmd, "debut", OracleDbType.Date, debut.Date)
                        Parametre(cmd, "fin", OracleDbType.Date, fin.Date)
                    End Sub

                Dim s As New SyntheseActivite() With {.Debut = debut.Date, .Fin = fin.Date}
                Lister(cnx, sqlIndicateurs, parPeriode,
                    Function(l)
                        s.NbVisites = l.GetInt32(0)
                        s.NbPraticiens = l.GetInt32(1)
                        s.NbVisitesRemplacant = If(l.IsDBNull(2), 0, l.GetInt32(2))
                        s.ConfianceMoyenne = If(l.IsDBNull(3), CType(Nothing, Decimal?), Math.Round(l.GetDecimal(3), 2))
                        Return True
                    End Function)

                s.Echantillons = Lister(cnx, sqlEchantillons, parPeriode,
                    Function(l) New EchantillonsDistribues() With {
                        .DepotLegal = l.GetString(0), .NomCommercial = l.GetString(1),
                        .Quantite = l.GetInt32(2), .Cout = l.GetDecimal(3)})
                s.NbEchantillons = s.Echantillons.Sum(Function(e) e.Quantite)
                s.CoutEchantillons = s.Echantillons.Sum(Function(e) e.Cout)

                s.ParMotif = Lister(cnx, sqlMotifs, parPeriode,
                    Function(l) New Repartition() With {.Libelle = l.GetString(0), .Nombre = l.GetInt32(1)})
                s.ParMois = Lister(cnx, sqlMois, parPeriode,
                    Function(l) New VisitesDuMois() With {.Mois = l.GetDateTime(0), .Nombre = l.GetInt32(1)})
                s.ProduitsPresentes = Lister(cnx, sqlProduits, parPeriode,
                    Function(l) New Repartition() With {.Libelle = l.GetString(0), .Nombre = l.GetInt32(1)})

                Lister(cnx, sqlTemps, parPeriode,
                    Function(l)
                        s.TempsSaisieMoyen = If(l.IsDBNull(0), CType(Nothing, Decimal?), Math.Round(l.GetDecimal(0), 0))
                        s.TempsSaisieTotal = l.GetDecimal(1)
                        Return True
                    End Function)

                s.NbBrouillons = Lister(cnx, sqlBrouillons,
                                        Sub(cmd) Parametre(cmd, "matricule", OracleDbType.Varchar2, matricule),
                                        Function(l) l.GetInt32(0)).Single()
                Return s
            End Function)
    End Function

End Class
