# Diagrammes UML 2 — GSB-CR

Sources PlantUML (`.puml`) versionnées ; images générées à côté (`.svg` pour l'affichage, `.png` pour les documents).
Charte commune : `style.iuml` (bleu / blanc).

| # | Diagramme | Type UML 2 | Ce qu'il montre |
|---|---|---|---|
| 1 | [Cas d'utilisation](01_cas_utilisation.svg) | Comportement | Acteurs (visiteur, délégué, responsable, administrateur) et fonctionnalités par module |
| 2 | [Classes du domaine](02_classes_domaine.svg) | Structure | Modèle métier complet : attributs, associations, multiplicités, contraintes |
| 3 | [Classes de l'application](03_classes_application.svg) | Structure | Architecture en couches (IHM, Métier, Données, Modèles) et classes du code |
| 4 | [Séquence : connexion](04_sequence_connexion.svg) | Interaction | Déroulement complet d'une connexion avec tous les cas d'échec |
| 5 | [États : compte-rendu](05_etats_compte_rendu.svg) | Comportement | Cycle de vie d'un CR (brouillon → validé) |
| 6 | [Activité : saisie d'un CR](06_activite_saisie_cr.svg) | Comportement | Étapes de la saisie d'un compte-rendu (visiteur / système) |
| 7 | [Déploiement](07_deploiement.svg) | Structure | Poste client, serveur Oracle, dépôt et intégration continue |
| 8 | [Séquence : enregistrement d'un CR](08_sequence_enregistrement_cr.svg) | Interaction | Contrôles, transaction et temps de saisie lors de l'enregistrement d'un compte-rendu |

## Régénérer les images

Avec [PlantUML](https://plantuml.com) (Java requis), depuis ce dossier :

```bash
java -jar plantuml.jar -charset UTF-8 -tsvg 0*.puml
java -jar plantuml.jar -charset UTF-8 -tpng 0*.puml
```

Extension VS Code conseillée : *PlantUML* (aperçu en direct avec `Alt+D`).
