Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>Implémentation Oracle de <see cref="IActiviteDao"/>.</summary>
Public Class ActiviteDao
    Inherits DaoOracle
    Implements IActiviteDao

    Public Sub New(connexion As ConnexionOracle)
        MyBase.New(connexion)
    End Sub

    Public Function ChargerSynthese(perimetre As Perimetre, debut As Date, fin As Date) As SyntheseActivite Implements IActiviteDao.ChargerSynthese
        ' Filtre commun : CR validés du périmètre sur la période (alias r pour RAPPORT_VISITE)
        Dim filtre = $"{SqlPerimetre.Condition("r.col_matricule", perimetre)}
                       and r.rap_etat = 'V' and r.rap_date_visite between :debut and :fin"

        Dim sqlIndicateurs = $"select count(*), count(distinct r.pra_num),
                                      sum(case when r.pra_num_remplacant is not null then 1 else 0 end),
                                      round(avg(r.rap_coef_confiance), 2)   -- round : avg a 38 chiffres, trop pour Decimal
                                 from RAPPORT_VISITE r
                                where {filtre}"
        Dim sqlEchantillons = $"select m.med_depot_legal, m.med_nom_commercial, sum(o.off_quantite),
                                       sum(o.off_quantite * m.med_prix_echantillon)
                                  from OFFRIR o
                                  join RAPPORT_VISITE r on r.rap_num = o.rap_num
                                  join MEDICAMENT m     on m.med_depot_legal = o.med_depot_legal
                                 where {filtre}
                                 group by m.med_depot_legal, m.med_nom_commercial
                                 order by 3 desc, 2"
        Dim sqlMotifs = $"select nvl(mo.mot_libelle, 'Non renseigné'), count(*)
                            from RAPPORT_VISITE r
                            left join MOTIF mo on mo.mot_code = r.mot_code
                           where {filtre}
                           group by nvl(mo.mot_libelle, 'Non renseigné')
                           order by 2 desc, 1"
        Dim sqlMois = $"select trunc(r.rap_date_visite, 'MM'), count(*)
                          from RAPPORT_VISITE r
                         where {filtre}
                         group by trunc(r.rap_date_visite, 'MM')
                         order by 1"
        Dim sqlProduits = $"select m.med_nom_commercial, count(*)
                              from PRESENTER p
                              join RAPPORT_VISITE r on r.rap_num = p.rap_num
                              join MEDICAMENT m     on m.med_depot_legal = p.med_depot_legal
                             where {filtre}
                             group by m.med_nom_commercial
                             order by 2 desc, 1"
        Dim sqlTemps = $"select round(avg(t.duree_secondes)), round(nvl(sum(t.duree_secondes), 0))
                           from V_TEMPS_SAISIE t
                           join RAPPORT_VISITE r on r.rap_num = t.rap_num
                          where {filtre}"
        Dim sqlBrouillons = $"select count(*) from RAPPORT_VISITE r
                               where {SqlPerimetre.Condition("r.col_matricule", perimetre)} and r.rap_etat = 'B'"

        Return Executer("Lecture de l'activité impossible.",
            Function(cnx)
                Dim parPeriode As Action(Of OracleCommand) =
                    Sub(cmd)
                        SqlPerimetre.Lier(cmd, perimetre)
                        Parametre(cmd, "debut", OracleDbType.Date, debut.Date)
                        Parametre(cmd, "fin", OracleDbType.Date, fin.Date)
                    End Sub

                Dim s As New SyntheseActivite() With {.Debut = debut.Date, .Fin = fin.Date}
                Lister(cnx, sqlIndicateurs, parPeriode,
                    Function(l)
                        s.NbVisites = l.GetInt32(0)
                        s.NbPraticiens = l.GetInt32(1)
                        s.NbVisitesRemplacant = If(l.IsDBNull(2), 0, l.GetInt32(2))
                        s.ConfianceMoyenne = If(l.IsDBNull(3), CType(Nothing, Decimal?), l.GetDecimal(3))
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
                        s.TempsSaisieMoyen = If(l.IsDBNull(0), CType(Nothing, Decimal?), l.GetDecimal(0))
                        s.TempsSaisieTotal = l.GetDecimal(1)
                        Return True
                    End Function)

                s.NbBrouillons = Lister(cnx, sqlBrouillons, Sub(cmd) SqlPerimetre.Lier(cmd, perimetre),
                                        Function(l) l.GetInt32(0)).Single()
                Return s
            End Function)
    End Function

    Public Function ActiviteParMembre(perimetre As Perimetre, debut As Date, fin As Date) As List(Of ActiviteMembre) Implements IActiviteDao.ActiviteParMembre
        ' Tous les membres actuels du périmètre apparaissent, même sans visite sur la période
        Dim sql = $"select a.col_matricule, a.col_prenom || ' ' || a.col_nom, a.pro_code, a.reg_nom,
                           nvl(v.nb, 0), nvl(v.nb_praticiens, 0), v.confiance, nvl(e.quantite, 0), nvl(e.cout, 0),
                           nvl(b.nb, 0), v.derniere
                      from V_AFFECTATION_EN_COURS a
                      left join (select col_matricule, count(*) nb, count(distinct pra_num) nb_praticiens,
                                        round(avg(rap_coef_confiance), 2) confiance, max(rap_date_visite) derniere
                                   from RAPPORT_VISITE
                                  where rap_etat = 'V' and rap_date_visite between :debut and :fin
                                  group by col_matricule) v on v.col_matricule = a.col_matricule
                      left join (select r.col_matricule, sum(o.off_quantite) quantite,
                                        sum(o.off_quantite * m.med_prix_echantillon) cout
                                   from OFFRIR o
                                   join RAPPORT_VISITE r on r.rap_num = o.rap_num
                                   join MEDICAMENT m     on m.med_depot_legal = o.med_depot_legal
                                  where r.rap_etat = 'V' and r.rap_date_visite between :debut and :fin
                                  group by r.col_matricule) e on e.col_matricule = a.col_matricule
                      left join (select col_matricule, count(*) nb
                                   from RAPPORT_VISITE where rap_etat = 'B'
                                  group by col_matricule) b on b.col_matricule = a.col_matricule
                     where a.pro_code in ('VIS', 'DEL')
                       and {SqlPerimetre.Condition("a.col_matricule", perimetre)}
                     order by a.reg_nom, a.col_nom, a.col_prenom"

        Return Lister("Lecture de l'activité de l'équipe impossible.", sql,
            Sub(cmd)
                SqlPerimetre.Lier(cmd, perimetre)
                Parametre(cmd, "debut", OracleDbType.Date, debut.Date)
                Parametre(cmd, "fin", OracleDbType.Date, fin.Date)
            End Sub,
            Function(l) New ActiviteMembre() With {
                .Matricule = l.GetString(0), .NomComplet = l.GetString(1),
                .Profil = ProfilCodes.DepuisCode(l.GetString(2)), .NomRegion = TexteOuRien(l, 3),
                .NbVisites = l.GetInt32(4), .NbPraticiens = l.GetInt32(5), .ConfianceMoyenne = DecimalOuRien(l, 6),
                .NbEchantillons = l.GetInt32(7), .CoutEchantillons = l.GetDecimal(8), .NbBrouillons = l.GetInt32(9),
                .DateDerniereVisite = DateOuRien(l, 10)})
    End Function

End Class
