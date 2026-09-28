# Modèle de données (ébauche)

Première proposition déduite du cahier des charges. À affiner si GSB fournit sa base exemple / modélisation (le commanditaire s'y engage dans le CDC). Préfixes de colonnes façon GSB (`pra_`, `vis_`…).

## Entités principales

```
SECTEUR (sec_code, sec_libelle)
REGION (reg_code, reg_nom, #sec_code)
COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville,
               col_date_embauche, col_login, col_mdp_hash, #rol_code)
ROLE (rol_code, rol_libelle)                 -- VIS, DEL, RES
AFFECTATION (#col_matricule, #reg_code, aff_date_debut, aff_date_fin)
                                             -- historique des régions (même région possible plusieurs fois)
TYPE_PRATICIEN (typ_code, typ_libelle, typ_lieu)
PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville,
           pra_coef_notoriete, pra_coef_prescription, #typ_code, #col_matricule)
                                             -- col_matricule = visiteur en charge (portefeuille)
SPECIALITE (spe_code, spe_libelle)
POSSEDER (#pra_num, #spe_code, pos_diplome)
FAMILLE (fam_code, fam_libelle)
MEDICAMENT (med_depot_legal, med_nom_commercial, med_composition, med_effets,
            med_contre_indic, med_prix_echantillon, #fam_code)
INTERAGIR (#med_perturbateur, #med_perturbe)
MOTIF (mot_code, mot_libelle)                -- PERIO, NOUV, REMONT, SOLLIC, AUTRE

RAPPORT_VISITE (rap_num, #col_matricule, #pra_num, rap_date_visite, #mot_code,
                rap_motif_autre, rap_bilan, rap_coef_confiance,
                rap_remplacant (O/N), #pra_num_remplacant NULL,
                rap_date_prochaine_visite NULL,
                rap_date_saisie, rap_date_modif)
PRESENTER (#rap_num, #med_depot_legal, pre_ordre)   -- max 2 par rapport
OFFRIR (#rap_num, #med_depot_legal, off_quantite)   -- échantillons à l'unité
ATTRIBUER (#col_matricule, #med_depot_legal, att_mois, att_quantite)
                                             -- saisi par le délégué (contrôle de stock)
```

## Points ouverts

- Le remplaçant : table `PRATICIEN` réutilisée (avec un type « remplaçant ») ou table dédiée ? → proposé : réutiliser `PRATICIEN` pour garder l'historique quand il s'installe.
- Posologie (par type d'individu, présentation, dosage) : tables `PRESENTATION`, `DOSAGE`, `TYPE_INDIVIDU`, `PRESCRIRE` si on veut l'afficher en détail.
- Composition : texte libre ou table `COMPOSANT` / `CONSTITUER` ?
- Règle « max 2 produits présentés » : contrôle applicatif (couche Métier) + éventuellement trigger Oracle.
