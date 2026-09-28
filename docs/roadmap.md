# Roadmap — livrables de l'AP

Liste issue de `AP3.pptx` (« Travail à faire : TOUT »). Cocher au fil de l'eau.

## Phase 0 — Mise en place
- [x] Documentation du projet (`docs/`)
- [x] Dépôt Git + GitHub (privé)
- [x] Solution Visual Studio (4 projets + tests) — `GSB.CR.slnx`
- [x] Intégration continue GitHub Actions (build + tests)
- [x] Base Oracle : schéma GSB sur 100.109.217.110/FREEPDB1, connexion testée depuis l'appli

## Phase 1 — Conception
- [ ] Spécifications fonctionnelles générales (cas d'utilisation par module)
- [ ] Spécifications fonctionnelles détaillées (écrans, règles de gestion, `EX-xx`)
- [x] MCD / MLD définitif (`docs/modele-donnees.md`) + scripts SQL `bdd/` (installé et testé : 21 règles OK)
- [ ] Maquettes des écrans (bleu/blanc)

## Phase 2 — Programmation (module Visiteur d'abord)
- [x] Connexion + redirection par profil (EX-01, EX-02, EX-05 à EX-09) — v0.2.0
- [x] Saisie d'un CR (EX-10 → EX-19, EX-25 → EX-29) — v0.3.0
- [x] Consultation CR 3 ans (EX-20) — v0.3.0
- [x] Fiches praticiens / produits (EX-21, EX-22) — v0.4.0
- [x] Synthèse d'activité et praticiens à revoir (EX-23, EX-24) — v0.5.0
- [x] Module Délégué (EX-30 → EX-35) — v0.6.0
- [x] Module Responsable (EX-40, EX-41) — v0.6.0

## Phase 3 — Qualité
- [ ] Tests unitaires couche Métier
- [ ] Plan de tests / cahier de recette
- [ ] Intégration continue (GitHub Actions : build + tests)

## Phase 4 — Livraison
- [ ] Gestion de projet (planning, suivi)
- [ ] Documentation technique (classes, bibliothèques — EX-60, EX-62)
- [ ] Documentation utilisateur / mode opératoire par module (EX-61)
- [ ] Mise en exploitation (procédure d'installation, déploiement)
