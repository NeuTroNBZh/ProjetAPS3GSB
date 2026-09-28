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
| 9 | [Objets](09_objets.svg) | Structure | Exemple d'instantané : un compte-rendu validé avec son auteur, son praticien et ses échantillons |
| 10 | [Composants](10_composants.svg) | Structure | Assemblages .NET, interfaces fournies / requises (services, DAO, ADO.NET) |
| 11 | [Paquetages](11_paquetages.svg) | Structure | Couches, sous-paquetages et dépendances (import, access, realize) |
| 12 | [Structure composite](12_structure_composite.svg) | Structure | Parties, ports et connecteurs de l'application et du service des comptes-rendus |
| 13 | [États : compte d'un collaborateur](13_etats_compte.svg) | Comportement | Mot de passe provisoire, actif, verrouillé, parti |
| 14 | [Séquence : transfert de portefeuille](14_sequence_transfert_portefeuille.svg) | Interaction | Contrôles, transaction et rafraîchissement lors d'un transfert (EX-72) |
| 15 | [Communication : validation d'un CR](15_communication_validation_cr.svg) | Interaction | Messages numérotés entre les objets lors de la validation |
| 16 | [Timing : verrouillage après échecs](16_timing_connexion.svg) | Interaction | Évolution du compte et du compteur d'échecs dans le temps |
| 17 | [Vue d'ensemble des interactions](17_vue_ensemble_interactions.svg) | Interaction | Enchaînement des interactions d'une session (références aux séquences) |

Couverture UML 2 : les 7 diagrammes de structure (classes, objets, composants, déploiement, paquetages, structure composite ; le diagramme de profils ne s'applique pas ici) et les 7 diagrammes de comportement (cas d'utilisation, activité, états-transitions, séquence, communication, timing, vue d'ensemble des interactions).

## Régénérer les images

Avec [PlantUML](https://plantuml.com) (Java requis), depuis ce dossier :

```bash
java -jar plantuml.jar -charset UTF-8 -tsvg 0*.puml
java -DPLANTUML_LIMIT_SIZE=12000 -jar plantuml.jar -charset UTF-8 -tpng 0*.puml   # limite relevée : grands diagrammes
```

Extension VS Code conseillée : *PlantUML* (aperçu en direct avec `Alt+D`).
