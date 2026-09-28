# Contexte métier — GSB

Synthèse de `GSB-Expression de besoins V3.docx`.

## Historique

- **2013** : premier projet d'informatisation des CR sous **Access**, partiellement développé, diffusé à quelques visiteurs seulement puis abandonné (données praticiens jamais mises à jour).
- **2023** : entretiens auprès des 3 niveaux hiérarchiques pour actualiser le besoin. On repart de l'étude 2013 pour produire une **nouvelle application**.
- Depuis la fusion avec **Galaxy**, il n'y a plus de départements par sous-laboratoire (Swiss / Bourdin / Autres) : tout le monde est sous la même enseigne.

## Pourquoi des visiteurs médicaux ?

Un médicament remboursé n'est jamais vendu directement au patient, il est **prescrit** par le médecin, et la publicité sur ces médicaments est interdite. Le laboratoire promeut donc ses produits directement auprès des **praticiens** via ses visiteurs médicaux.

## Hiérarchie (organisation par secteurs géographiques)

```
Secteur  ──► Responsable de secteur
  └─ Région ──► Délégué régional
        └─ Visiteurs médicaux
```

| Rôle | Activité | Accès dans l'appli |
|---|---|---|
| **Visiteur** | Visite les praticiens (portefeuille propre : un médecin n'est visité que par un seul visiteur du labo). Rédige les CR. | Ses propres CR (saisie, consultation sur 3 ans, synthèse) |
| **Délégué régional** | Visiteur à 3/4 temps + intermédiaire région ↔ responsable. Réunions bilans mensuelles. Enregistre les attributions d'échantillons. | Ses CR + activité des visiteurs de sa région |
| **Responsable de secteur** | Ne va plus sur le terrain. Formation, objectifs, budgets, évaluation/notation des visiteurs. | Activité des visiteurs du secteur (stats, graphiques, par visiteur ou par région) |

Aujourd'hui tous sont des « collaborateurs ». Turn-over important → on garde l'historique des régions par lesquelles un visiteur est passé.

## Les données du domaine

- **Visiteur / collaborateur** : matricule, identité, adresse, date d'embauche, historique des régions (une région peut revenir plusieurs fois).
- **Praticien** : état civil, coordonnées, type (généraliste, spécialiste, hôpital, pharmacien, infirmier…), **coef. de notoriété** et **coef. de prescription** (fournis par des organismes externes), spécialité(s), diplôme le plus haut.
- **Produit (médicament)** : n° de dépôt légal, nom commercial, famille (antibiotique, antidépresseur…), effets thérapeutiques, contre-indications, composition (composants + quantité), interactions avec d'autres médicaments, posologie (par type d'individu : adulte, jeune adulte, enfant, jeune enfant, nourrisson — dépend de la présentation et du dosage), **prix de l'échantillon**.
- **Rapport de visite (CR)** : voir `exigences.md`.

## Irritants actuels (à résoudre)

- CR papier et fichiers Excel « Classeur1.xls » → on ne sait plus qui a envoyé quoi.
- CR en texte libre difficilement exploitables.
- CR incomplets (sans bilan, sans médecin, sans date).
- Remplaçants non tracés (les visiteurs tiennent des « fiches bis »).
- Saisie des échantillons jugée pénible par les visiteurs — mais **obligatoire à l'unité près** (obligations légales et comptables).
