-- =====================================================================
-- GSB-CR : vues (lecture simplifiée pour l'application et les statistiques)
-- =====================================================================
whenever sqlerror exit failure

-- Situation actuelle de chaque collaborateur présent (profil, région, secteur)
create or replace view V_AFFECTATION_EN_COURS as
select c.col_matricule,
       c.col_nom,
       c.col_prenom,
       c.col_login,
       a.pro_code,
       p.pro_libelle,
       a.reg_code,
       r.reg_nom,
       coalesce(a.sec_code, r.sec_code) as sec_code,
       s.sec_libelle,
       a.aff_date_debut
  from COLLABORATEUR c
  join AFFECTATION a  on a.col_matricule = c.col_matricule and a.aff_date_fin is null
  join PROFIL p       on p.pro_code = a.pro_code
  left join REGION r  on r.reg_code = a.reg_code
  left join SECTEUR s on s.sec_code = coalesce(a.sec_code, r.sec_code)
 where c.col_date_depart is null or c.col_date_depart > sysdate;

-- Rapports avec les libellés utiles à l'affichage
create or replace view V_RAPPORT_DETAIL as
select r.rap_num,
       r.col_matricule,
       c.col_nom || ' ' || c.col_prenom           as col_nom_complet,
       r.pra_num,
       p.pra_nom || ' ' || p.pra_prenom           as pra_nom_complet,
       p.pra_ville,
       r.pra_num_remplacant,
       case when rp.pra_num is not null
            then rp.pra_nom || ' ' || rp.pra_prenom end as remplacant_nom_complet,
       r.rap_date_visite,
       r.mot_code,
       case when r.mot_code = 'AUTRE' then r.rap_motif_autre else m.mot_libelle end as motif,
       r.rap_bilan,
       r.rap_coef_confiance,
       r.rap_date_prochaine_visite,
       r.rap_etat,
       r.rap_date_saisie,
       r.rap_date_modif,
       r.rap_date_validation
  from RAPPORT_VISITE r
  join COLLABORATEUR c   on c.col_matricule = r.col_matricule
  join PRATICIEN p       on p.pra_num = r.pra_num
  left join PRATICIEN rp on rp.pra_num = r.pra_num_remplacant
  left join MOTIF m      on m.mot_code = r.mot_code;

-- Aide à la périodicité : dernière visite validée de chaque praticien actif
-- et visiteur qui le suit actuellement (revoir tous les 6 à 8 mois)
create or replace view V_DERNIERE_VISITE as
select p.pra_num,
       p.pra_nom,
       p.pra_prenom,
       p.pra_ville,
       pf.col_matricule,
       max(r.rap_date_visite)                                 as date_derniere_visite,
       trunc(sysdate - max(r.rap_date_visite))                as jours_depuis,
       max(r.rap_date_prochaine_visite)
           keep (dense_rank last order by r.rap_date_visite)  as date_prochaine_prevue
  from PRATICIEN p
  left join PORTEFEUILLE pf  on pf.pra_num = p.pra_num and pf.ptf_date_fin is null
  left join RAPPORT_VISITE r on r.pra_num = p.pra_num and r.rap_etat = 'V'
 where p.pra_actif = 'O'
 group by p.pra_num, p.pra_nom, p.pra_prenom, p.pra_ville, pf.col_matricule;

-- Activité mensuelle par collaborateur (CR validés uniquement)
create or replace view V_ACTIVITE_MENSUELLE as
select r.col_matricule,
       trunc(r.rap_date_visite, 'MM')                        as mois,
       count(distinct r.rap_num)                             as nb_visites,
       count(distinct r.pra_num)                             as nb_praticiens,
       nvl(sum(o.off_quantite), 0)                           as nb_echantillons,
       nvl(sum(o.off_quantite * m.med_prix_echantillon), 0)  as cout_echantillons
  from RAPPORT_VISITE r
  left join OFFRIR o     on o.rap_num = r.rap_num
  left join MEDICAMENT m on m.med_depot_legal = o.med_depot_legal
 where r.rap_etat = 'V'
 group by r.col_matricule, trunc(r.rap_date_visite, 'MM');

-- Contrôle de stock : échantillons attribués (dotations) vs distribués (CR validés)
create or replace view V_STOCK_ECHANTILLON as
select col_matricule,
       med_depot_legal,
       mois,
       sum(attribue)                   as qte_attribuee,
       sum(distribue)                  as qte_distribuee,
       sum(attribue) - sum(distribue)  as ecart
  from (select d.col_matricule, d.med_depot_legal, d.dot_mois as mois,
               d.dot_quantite as attribue, 0 as distribue
          from DOTATION d
        union all
        select r.col_matricule, o.med_depot_legal, trunc(r.rap_date_visite, 'MM'),
               0, o.off_quantite
          from OFFRIR o
          join RAPPORT_VISITE r on r.rap_num = o.rap_num
         where r.rap_etat = 'V')
 group by col_matricule, med_depot_legal, mois;

-- Temps de saisie par rapport, en secondes (somme des sessions de saisie)
create or replace view V_TEMPS_SAISIE as
select s.rap_num,
       s.col_matricule,
       count(*) as nb_sessions,
       sum(  extract(day    from (s.ses_fin - s.ses_debut)) * 86400
           + extract(hour   from (s.ses_fin - s.ses_debut)) * 3600
           + extract(minute from (s.ses_fin - s.ses_debut)) * 60
           + extract(second from (s.ses_fin - s.ses_debut))) as duree_secondes
  from SESSION_SAISIE s
 group by s.rap_num, s.col_matricule;
