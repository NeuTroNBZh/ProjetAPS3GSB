Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>Implémentation Oracle de <see cref="IConsultationDao"/>.</summary>
Public Class ConsultationDao
    Inherits DaoOracle
    Implements IConsultationDao

    ''' <summary>Dernière visite validée de chaque praticien (en tant que titulaire) et prochaine visite prévue.</summary>
    Private Const SousRequeteDerniereVisite As String =
        "select pra_num,
                max(rap_date_visite) as date_derniere,
                max(rap_date_prochaine_visite) keep (dense_rank last order by rap_date_visite) as date_prochaine
           from RAPPORT_VISITE
          where rap_etat = 'V'
          group by pra_num"

    Public Sub New(connexion As ConnexionOracle)
        MyBase.New(connexion)
    End Sub

    Public Function RechercherPraticiens(texte As String, perimetre As Perimetre,
                                         inclureInactifs As Boolean, maximum As Integer) As List(Of PraticienResume) Implements IConsultationDao.RechercherPraticiens
        Dim sql = $"select * from (
                      select p.pra_num, p.pra_nom, p.pra_prenom, t.typ_libelle, p.pra_cp, p.pra_ville, p.pra_telephone,
                             p.pra_actif, pf.col_matricule, c.col_prenom || ' ' || c.col_nom, dv.date_derniere, dv.date_prochaine
                        from PRATICIEN p
                        join TYPE_PRATICIEN t      on t.typ_code = p.typ_code
                        left join PORTEFEUILLE pf  on pf.pra_num = p.pra_num and pf.ptf_date_fin is null
                        left join COLLABORATEUR c  on c.col_matricule = pf.col_matricule
                        left join ({SousRequeteDerniereVisite}) dv on dv.pra_num = p.pra_num
                       where (:texte is null
                              or lower(p.pra_nom)    like '%' || lower(:texte) || '%'
                              or lower(p.pra_prenom) like '%' || lower(:texte) || '%'
                              or lower(p.pra_ville)  like '%' || lower(:texte) || '%')
                         and {If(perimetre Is Nothing, "1 = 1", SqlPerimetre.Condition("pf.col_matricule", perimetre))}
                         and (:inactifs = 1 or p.pra_actif = 'O')
                       order by p.pra_nom, p.pra_prenom)
                     where rownum <= :maximum"

        Return Lister("Recherche des praticiens impossible.", sql,
            Sub(cmd)
                Dim t = If(texte, "").Trim()
                Parametre(cmd, "texte", OracleDbType.Varchar2, If(t.Length = 0, Nothing, t))
                If perimetre IsNot Nothing Then SqlPerimetre.Lier(cmd, perimetre)
                Parametre(cmd, "inactifs", OracleDbType.Int32, If(inclureInactifs, 1, 0))
                Parametre(cmd, "maximum", OracleDbType.Int32, maximum)
            End Sub,
            Function(l) New PraticienResume() With {
                .Numero = l.GetInt32(0),
                .Nom = l.GetString(1),
                .Prenom = l.GetString(2),
                .LibelleType = l.GetString(3),
                .CodePostal = TexteOuRien(l, 4),
                .Ville = TexteOuRien(l, 5),
                .Telephone = TexteOuRien(l, 6),
                .Actif = l.GetString(7) = "O",
                .MatriculeVisiteur = TexteOuRien(l, 8),
                .NomVisiteur = If(l.IsDBNull(8), Nothing, l.GetString(9)),
                .DateDerniereVisite = DateOuRien(l, 10),
                .DateProchainePrevue = DateOuRien(l, 11)
            })
    End Function

    Public Function ChargerFichePraticien(numero As Integer) As FichePraticien Implements IConsultationDao.ChargerFichePraticien
        Dim sqlPraticien = $"select p.pra_num, p.pra_nom, p.pra_prenom, p.pra_adresse, p.pra_cp, p.pra_ville,
                                    p.pra_telephone, p.pra_email, p.typ_code, t.typ_libelle, p.pra_coef_notoriete, p.pra_actif,
                                    c.col_prenom || ' ' || c.col_nom, pf.col_matricule, dv.date_derniere, dv.date_prochaine
                               from PRATICIEN p
                               join TYPE_PRATICIEN t      on t.typ_code = p.typ_code
                               left join PORTEFEUILLE pf  on pf.pra_num = p.pra_num and pf.ptf_date_fin is null
                               left join COLLABORATEUR c  on c.col_matricule = pf.col_matricule
                               left join ({SousRequeteDerniereVisite}) dv on dv.pra_num = p.pra_num
                              where p.pra_num = :numero"
        Const sqlSpecialites As String =
            "select s.spe_code, s.spe_libelle, po.pos_diplome, po.pos_coef_prescription
               from POSSEDER po
               join SPECIALITE s on s.spe_code = po.spe_code
              where po.pra_num = :numero
              order by s.spe_libelle"
        ' Visites où le praticien était le titulaire OU le remplaçant rencontré
        Const sqlVisites As String =
            "select r.rap_num, r.rap_date_visite, c.col_prenom || ' ' || c.col_nom,
                    case when r.mot_code = 'AUTRE' then r.rap_motif_autre else m.mot_libelle end,
                    r.rap_coef_confiance,
                    case when r.pra_num = :numero then 0 else 1 end,
                    case when r.pra_num = :numero
                         then case when rp.pra_num is not null then rp.pra_nom || ' ' || rp.pra_prenom end
                         else pt.pra_nom || ' ' || pt.pra_prenom end
               from RAPPORT_VISITE r
               join COLLABORATEUR c   on c.col_matricule = r.col_matricule
               join PRATICIEN pt      on pt.pra_num = r.pra_num
               left join PRATICIEN rp on rp.pra_num = r.pra_num_remplacant
               left join MOTIF m      on m.mot_code = r.mot_code
              where r.rap_etat = 'V'
                and (r.pra_num = :numero or r.pra_num_remplacant = :numero)
              order by r.rap_date_visite desc, r.rap_num desc"

        Return Executer("Lecture de la fiche praticien impossible.",
            Function(cnx)
                Dim parNumero As Action(Of OracleCommand) = Sub(cmd) Parametre(cmd, "numero", OracleDbType.Int32, numero)

                Dim fiche = Lister(cnx, sqlPraticien, parNumero,
                    Function(l) New FichePraticien() With {
                        .Praticien = New Praticien() With {
                            .Numero = l.GetInt32(0), .Nom = l.GetString(1), .Prenom = l.GetString(2),
                            .Adresse = TexteOuRien(l, 3), .CodePostal = TexteOuRien(l, 4), .Ville = TexteOuRien(l, 5),
                            .Telephone = TexteOuRien(l, 6), .Email = TexteOuRien(l, 7),
                            .CodeType = l.GetString(8), .LibelleType = l.GetString(9),
                            .CoefNotoriete = DecimalOuRien(l, 10), .Actif = l.GetString(11) = "O"},
                        .NomVisiteur = If(l.IsDBNull(13), Nothing, l.GetString(12)),
                        .DateDerniereVisite = DateOuRien(l, 14),
                        .DateProchainePrevue = DateOuRien(l, 15)
                    }).FirstOrDefault()
                If fiche Is Nothing Then Return Nothing

                fiche.Specialites = Lister(cnx, sqlSpecialites, parNumero,
                    Function(l) New SpecialitePraticien() With {
                        .Code = l.GetString(0), .Libelle = l.GetString(1),
                        .Diplome = TexteOuRien(l, 2), .CoefPrescription = DecimalOuRien(l, 3)})

                fiche.Visites = Lister(cnx, sqlVisites, parNumero,
                    Function(l) New VisitePraticien() With {
                        .NumeroRapport = l.GetInt32(0), .DateVisite = l.GetDateTime(1), .Visiteur = l.GetString(2),
                        .Motif = TexteOuRien(l, 3), .CoefConfiance = EntierOuRien(l, 4),
                        .VuCommeRemplacant = l.GetInt32(5) = 1, .PraticienLie = TexteOuRien(l, 6)})
                Return fiche
            End Function)
    End Function

    Public Function ChargerFicheMedicament(depotLegal As String) As FicheMedicament Implements IConsultationDao.ChargerFicheMedicament
        Const sqlMedicament As String =
            "select m.med_depot_legal, m.med_nom_commercial, m.fam_code, f.fam_libelle, m.med_prix_echantillon,
                    m.med_actif, m.med_effets, m.med_contre_indic, m.med_date_commercialisation
               from MEDICAMENT m
               join FAMILLE f on f.fam_code = m.fam_code
              where m.med_depot_legal = :depot"
        Const sqlComposition As String =
            "select c.cmp_libelle, co.cst_quantite, co.cst_unite
               from CONSTITUER co
               join COMPOSANT c on c.cmp_code = co.cmp_code
              where co.med_depot_legal = :depot
              order by c.cmp_libelle"
        ' Interactions dans les deux sens
        Const sqlInteractions As String =
            "select m.med_depot_legal, m.med_nom_commercial, 1, i.itr_description
               from INTERAGIR i join MEDICAMENT m on m.med_depot_legal = i.med_perturbe
              where i.med_perturbateur = :depot
             union all
             select m.med_depot_legal, m.med_nom_commercial, 0, i.itr_description
               from INTERAGIR i join MEDICAMENT m on m.med_depot_legal = i.med_perturbateur
              where i.med_perturbe = :depot
             order by 2"
        ' Du plus âgé au plus jeune
        Const sqlPosologie As String =
            "select t.tin_libelle, pr.pre_libelle, d.dos_quantite, d.dos_unite, p.prs_posologie
               from PRESCRIRE p
               join TYPE_INDIVIDU t on t.tin_code = p.tin_code
               join PRESENTATION pr on pr.pre_code = p.pre_code
               join DOSAGE d        on d.dos_code = p.dos_code
              where p.med_depot_legal = :depot
              order by case t.tin_code when 'ADU' then 1 when 'JAD' then 2 when 'ENF' then 3
                                       when 'JEN' then 4 when 'NOU' then 5 else 6 end, pr.pre_libelle"

        Return Executer("Lecture de la fiche médicament impossible.",
            Function(cnx)
                Dim parDepot As Action(Of OracleCommand) = Sub(cmd) Parametre(cmd, "depot", OracleDbType.Varchar2, depotLegal)

                Dim fiche = Lister(cnx, sqlMedicament, parDepot,
                    Function(l) New FicheMedicament() With {
                        .Medicament = New Medicament() With {
                            .DepotLegal = l.GetString(0), .NomCommercial = l.GetString(1), .CodeFamille = l.GetString(2),
                            .LibelleFamille = l.GetString(3), .PrixEchantillon = l.GetDecimal(4), .Actif = l.GetString(5) = "O"},
                        .Effets = TexteOuRien(l, 6),
                        .ContreIndications = TexteOuRien(l, 7),
                        .DateCommercialisation = DateOuRien(l, 8)
                    }).FirstOrDefault()
                If fiche Is Nothing Then Return Nothing

                fiche.Composition = Lister(cnx, sqlComposition, parDepot,
                    Function(l) New LigneComposition() With {.Composant = l.GetString(0), .Quantite = l.GetDecimal(1), .Unite = l.GetString(2)})
                fiche.Interactions = Lister(cnx, sqlInteractions, parDepot,
                    Function(l) New InteractionMedicamenteuse() With {
                        .DepotLegalAutre = l.GetString(0), .NomAutre = l.GetString(1),
                        .EstPerturbateur = l.GetInt32(2) = 1, .Description = TexteOuRien(l, 3)})
                fiche.Posologies = Lister(cnx, sqlPosologie, parDepot,
                    Function(l) New Posologie() With {
                        .TypeIndividu = l.GetString(0), .Presentation = l.GetString(1),
                        .Dosage = $"{l.GetDecimal(2):0.###} {l.GetString(3)}", .Texte = l.GetString(4)})
                Return fiche
            End Function)
    End Function

End Class
