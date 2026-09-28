# Exigences — GSB-CR

Chaque exigence a un identifiant `EX-xx` à citer dans le code, les tests et les commits.
Priorité : **P1** = module Visiteur (urgent), **P2** = Délégué, **P3** = Responsable / confort.

## Général / sécurité

| ID | Exigence | Prio |
|---|---|---|
| EX-01 | La page d'accueil ne propose **qu'une zone d'identification** (login / mot de passe). | P1 |
| EX-02 | Après connexion, l'utilisateur accède au module correspondant à son **profil** (Visiteur, Délégué, Responsable). | P1 |
| EX-03 | Application poste de travail, **non reliée** à l'intranet ni au site de l'entreprise. Pas de version mobile. | P1 |
| EX-04 | Charte **bleu / blanc** (logo GSB), interface conviviale et facile à prendre en main. | P1 |
| EX-05 | Mots de passe stockés **hachés** ; requêtes SQL paramétrées. | P1 |

## Module Visiteur — saisie d'un compte-rendu

| ID | Exigence | Prio |
|---|---|---|
| EX-10 | Le CR est automatiquement rattaché au **visiteur connecté** (plus jamais de CR anonyme). | P1 |
| EX-11 | Champs **obligatoires** : praticien, date de visite, motif, bilan. Le CR ne peut pas être enregistré s'il en manque un. | P1 |
| EX-12 | Le **motif** est choisi dans une **liste** : périodicité, nouveautés/actualisation, remontage (baisse de prescription), sollicitation du praticien, **autre** (avec précision libre obligatoire). | P1 |
| EX-13 | **Au plus 2 produits présentés** par visite. | P1 |
| EX-14 | **Échantillons offerts** : liste de produits (0 à N, indépendante des produits présentés) avec **quantité à l'unité près**. | P1 |
| EX-15 | **Coefficient de confiance** du praticien envers les produits du labo (distinct des coefs de notoriété et de prescription). | P1 |
| EX-16 | Possibilité d'indiquer que la personne vue est un **remplaçant** : on garde le praticien du cabinet + la personne réellement visitée. | P1 |
| EX-17 | La **date de saisie** du CR est enregistrée automatiquement (en plus de la date de visite). | P1 |
| EX-18 | Un CR peut être **modifié** après saisie ; seule la dernière version compte. Conserver la date de dernière modification. | P1 |
| EX-19 | Le bilan peut indiquer si une **autre visite est planifiée** (date de prochaine visite). | P1 |

## Module Visiteur — consultation

| ID | Exigence | Prio |
|---|---|---|
| EX-20 | Consulter ses CR des **3 années précédentes** (recherche par période, praticien). | P1 |
| EX-21 | Consulter les **fiches praticiens** (coordonnées, infos détaillées, date de dernière visite). | P1 |
| EX-22 | Consulter les **fiches produits** (description, composition, effets, contre-indications, interactions, posologie). | P1 |
| EX-23 | **Vue synthétique** de son activité : nombre de visites sur une période, statistiques diverses. | P1 |
| EX-24 | Aide à la périodicité : repérer les praticiens non vus depuis 6 à 8 mois. | P3 |

## Module Délégué régional

| ID | Exigence | Prio |
|---|---|---|
| EX-30 | Toutes les fonctions du module Visiteur (le délégué fait aussi des visites). | P2 |
| EX-31 | Voir l'activité des visiteurs de **sa région** : statistiques, graphiques, activité par collaborateur. | P2 |
| EX-32 | Consulter les CR des visiteurs de sa région. | P2 |
| EX-33 | Enregistrer les **attributions d'échantillons** aux visiteurs (contrôle de stock : attribué vs distribué). | P2 |

## Module Responsable de secteur

| ID | Exigence | Prio |
|---|---|---|
| EX-40 | Voir l'activité des visiteurs de **son secteur** : stats et graphiques, pour un visiteur ou pour l'équipe d'une région. | P3 |
| EX-41 | Consulter les CR de ses subordonnés. | P3 |

## Optionnel / pistes

- EX-50 : outil de communication entre personnes ou vers un groupe (messagerie interne). P3
- EX-51 : export des statistiques. P3

## Non fonctionnel / documentation

- EX-60 : documentation présentant l'application pour chaque module + descriptif des classes et bibliothèques utilisées.
- EX-61 : mode opératoire (doc utilisateur) propre à chaque module.
- EX-62 : documentation technique permettant un transfert de compétences.
