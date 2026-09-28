# Spécifications fonctionnelles générales — GSB-CR

Version de l'application décrite : 1.0.0. Ce document décrit **ce que fait** l'application : acteurs, cas d'utilisation et règles de gestion. Le détail des écrans et des champs est dans les [spécifications détaillées](specifications-detaillees.md) ; les exigences d'origine sont dans [exigences.md](exigences.md).

## 1. Objet et périmètre

GSB-CR remplace les comptes-rendus de visite papier et les fichiers Excel des visiteurs médicaux du laboratoire Galaxy Swiss Bourdin. Chaque visiteur saisit ses visites chez les praticiens ; sa hiérarchie (délégué régional, responsable de secteur) suit l'activité de son équipe ; un administrateur gère les comptes et les données de référence.

| Dans le périmètre | Hors périmètre |
|---|---|
| Application de bureau Windows, base Oracle centrale | Version mobile ou tablette, accès par navigateur |
| Saisie, modification et consultation des comptes-rendus | Lien avec l'intranet ou le site web de GSB |
| Suivi d'activité par visiteur, région et secteur | Export des statistiques (EX-51, optionnelle, non réalisée) |
| Échantillons : dotations et contrôle de stock | Gestion des stocks physiques d'échantillons |
| Messagerie interne entre collaborateurs | Courrier électronique externe |
| Administration des comptes, affectations, portefeuilles et référentiels | Saisie de la composition et de la posologie des médicaments (chargées par script) |

## 2. Acteurs

| Acteur | Description | Rattachement |
|---|---|---|
| **Visiteur médical** | Visite les praticiens de son portefeuille et rédige un compte-rendu après chaque visite | Une région |
| **Délégué régional** | Visiteur qui encadre aussi les visiteurs de sa région et leur attribue les échantillons | Une région |
| **Responsable de secteur** | Suit l'activité de toutes les régions de son secteur | Un secteur |
| **Administrateur** | Gère les comptes, les affectations, les portefeuilles, les référentiels ; surveille les connexions | Siège |

Un collaborateur a **une seule affectation en cours** (profil et rattachement) ; l'historique des affectations est conservé. Il y a un délégué par région et un responsable par secteur. Le délégué « est un » visiteur : il dispose de toutes ses fonctions.

### Droits par module

| Module | Visiteur | Délégué | Responsable | Administrateur |
|---|:---:|:---:|:---:|:---:|
| Mes comptes-rendus | ✓ | ✓ | | |
| Mon activité | ✓ | ✓ | | |
| Praticiens, Médicaments | ✓ | ✓ | ✓ | ✓ |
| Ma région | | ✓ | | |
| Mon secteur | | | ✓ | |
| Échantillons | | saisie et contrôle | contrôle seul | |
| Messagerie | ✓ | ✓ | ✓ | ✓ |
| Administration | | | | ✓ |

Les droits sont vérifiés deux fois : le menu n'affiche que les modules autorisés, et chaque service métier refuse une demande d'un profil non autorisé.

## 3. Cas d'utilisation

Diagramme : [uml/01_cas_utilisation.svg](uml/01_cas_utilisation.svg).

| N° | Cas d'utilisation | Acteurs | Exigences |
|---|---|---|---|
| UC-01 | S'authentifier | Tous | EX-01, EX-02, EX-05, EX-06, EX-08, EX-09 |
| UC-02 | Changer son mot de passe | Tous | EX-07 |
| UC-03 | Consulter les praticiens | Tous | EX-21 |
| UC-04 | Consulter les médicaments | Tous | EX-22 |
| UC-05 | Échanger des messages | Tous | EX-50 |
| UC-10 | Saisir un compte-rendu | Visiteur, délégué | EX-10 à EX-17, EX-19, EX-25 à EX-28 |
| UC-11 | Déclarer un remplaçant (extension de UC-10) | Visiteur, délégué | EX-16, EX-28 |
| UC-12 | Modifier ou valider un compte-rendu | Visiteur, délégué | EX-18, EX-25, EX-29 |
| UC-13 | Supprimer un brouillon | Visiteur, délégué | EX-29 |
| UC-14 | Consulter ses comptes-rendus | Visiteur, délégué | EX-20 |
| UC-15 | Consulter la synthèse de son activité | Visiteur, délégué | EX-23 |
| UC-16 | Voir les praticiens à revoir | Visiteur, délégué | EX-24 |
| UC-20 | Consulter l'activité de la région | Délégué | EX-31, EX-35 |
| UC-21 | Consulter les comptes-rendus de la région | Délégué | EX-32 |
| UC-22 | Attribuer des échantillons | Délégué | EX-33 |
| UC-23 | Contrôler le stock d'échantillons | Délégué, responsable | EX-34 |
| UC-30 | Consulter l'activité du secteur | Responsable | EX-40 |
| UC-31 | Consulter les comptes-rendus du secteur | Responsable | EX-41 |
| UC-40 | Gérer les collaborateurs | Administrateur | EX-70 |
| UC-41 | Gérer les affectations | Administrateur | EX-71 |
| UC-42 | Gérer les portefeuilles | Administrateur | EX-72 |
| UC-43 | Gérer les référentiels | Administrateur | EX-73 |
| UC-44 | Consulter le journal des connexions | Administrateur | EX-74 |

## 4. Fiches des cas d'utilisation

Les règles citées (`RG-xx`) sont regroupées en section 5.

### UC-01 — S'authentifier

- **Acteur** : tout collaborateur.
- **Précondition** : l'application est lancée ; seule la fenêtre de connexion est affichée.
- **Scénario nominal**
  1. Le collaborateur saisit son identifiant et son mot de passe, puis valide.
  2. Le système vérifie le mot de passe (RG-01), remet à zéro le compteur d'échecs, enregistre la date de connexion et journalise la tentative réussie (RG-04).
  3. Le système affiche le menu avec les modules de son profil.
- **Alternatives et erreurs**
  - 2a. Identifiant inconnu ou mot de passe faux : message « Identifiant ou mot de passe incorrect. » (le même dans les deux cas), compteur d'échecs augmenté, tentative journalisée. Au 5e échec consécutif, le compte est verrouillé (RG-02).
  - 2b. Compte verrouillé : « Votre compte est verrouillé. Contactez l'administrateur. »
  - 2c. Collaborateur parti ou sans affectation en cours : « Ce compte n'est plus actif. Contactez l'administrateur. » (RG-03).
  - 2d. Mot de passe provisoire : le système enchaîne sur UC-02 ; le menu n'est accessible qu'une fois le mot de passe changé.
  - 2e. Base injoignable : « Le serveur est indisponible. Réessayez dans quelques instants. »
- **Postcondition** : le collaborateur est connecté jusqu'à ce qu'il se déconnecte ou ferme l'application.

### UC-02 — Changer son mot de passe

- **Acteur** : tout collaborateur connecté (obligatoire après UC-01 cas 2d).
- **Scénario nominal** : le collaborateur saisit son mot de passe actuel puis deux fois le nouveau ; le système vérifie l'ancien, la politique (RG-05) et la confirmation, puis enregistre le nouveau mot de passe haché et lève l'obligation de changement.
- **Erreurs** : mot de passe actuel faux, confirmation différente, nouveau mot de passe identique à l'ancien ou non conforme à la politique : message, rien n'est modifié.

### UC-03 — Consulter les praticiens

- **Scénario nominal** : le collaborateur recherche un praticien (nom, prénom, ville), limité par défaut à son portefeuille pour un visiteur ; il choisit un praticien et voit sa fiche : coordonnées, type, spécialités et coefficients, notoriété, visiteur qui le suit, date de dernière visite, prochaine visite conseillée (RG-20), historique des visites validées.
- **Variantes** : inclure les praticiens inactifs ; élargir à tous les praticiens.

### UC-04 — Consulter les médicaments

- **Scénario nominal** : le collaborateur recherche un médicament (nom commercial, dépôt légal, famille) et consulte sa fiche : famille, effets, contre-indications, composition, interactions, posologies par type d'individu et prix de l'échantillon. Les médicaments retirés ne sont affichés que sur demande.

### UC-05 — Échanger des messages

- **Scénario nominal**
  1. Le collaborateur crée un message et choisit un ou plusieurs destinataires, individuellement ou par groupe (une région, un secteur).
  2. Il saisit l'objet et le texte, puis envoie (RG-40).
  3. Chaque destinataire voit le message dans sa boîte de réception, non lu ; le nombre de messages non lus apparaît sur la tuile du menu.
  4. À l'ouverture par un destinataire, le message est marqué lu à la date de lecture, visible par l'expéditeur dans ses messages envoyés.
- **Variante** : répondre à un message reçu (destinataire, objet et texte cité pré-remplis).
- **Erreurs** : aucun destinataire, objet ou texte vide, limites dépassées (RG-40).

### UC-10 — Saisir un compte-rendu

- **Acteur** : visiteur ou délégué.
- **Précondition** : le collaborateur a au moins un praticien dans son portefeuille.
- **Scénario nominal**
  1. Le visiteur crée un compte-rendu ; le système démarre une session de saisie (RG-13) et rattache le compte-rendu au visiteur connecté (RG-10).
  2. Il choisit le praticien visité dans son portefeuille, la date de visite, le motif.
  3. Il indique au plus deux produits présentés et les échantillons offerts avec leur quantité.
  4. Il rédige le bilan, indique le coefficient de confiance et éventuellement la date de la prochaine visite.
  5. Il valide. Le système contrôle la saisie (RG-11, RG-12), enregistre le compte-rendu à l'état **validé** avec ses dates de saisie et de validation, et clôt la session de saisie.
- **Extensions**
  - 2a. La personne rencontrée est un remplaçant : UC-11.
  - 5a. Le visiteur enregistre en **brouillon** : seuls le praticien et la date sont exigés ; le compte-rendu n'est pas compté dans les statistiques (RG-14).
  - 5b. Contrôle en échec : les erreurs sont affichées, rien n'est enregistré, le visiteur corrige.
- **Postcondition** : le compte-rendu est enregistré en entier ou pas du tout (transaction).

### UC-11 — Déclarer un remplaçant

- **Scénario nominal** : pendant UC-10, le visiteur indique que la personne rencontrée remplace le praticien titulaire ; il choisit le remplaçant parmi les praticiens connus ou crée sa fiche (nom, prénom, type ; téléphone et e-mail facultatifs). Le compte-rendu garde le titulaire du cabinet et la personne réellement rencontrée.
- **Règle** : le remplaçant doit être différent du titulaire. Sa fiche est conservée, même s'il s'installe ailleurs.

### UC-12 — Modifier ou valider un compte-rendu

- **Scénario nominal** : le visiteur ouvre l'un de **ses** comptes-rendus, le modifie et enregistre ; une nouvelle session de saisie est enregistrée et la date de modification mise à jour. Seule la dernière version est conservée.
- **Règles** : un brouillon peut être validé ; un compte-rendu validé reste modifiable mais ne repasse jamais en brouillon ; il doit rester complet (RG-11). Un collaborateur ne modifie que ses propres comptes-rendus (RG-10).

### UC-13 — Supprimer un brouillon

- **Scénario nominal** : le visiteur sélectionne un de ses brouillons, demande la suppression, confirme ; le brouillon est supprimé définitivement.
- **Erreur** : un compte-rendu validé ne peut pas être supprimé.

### UC-14 — Consulter ses comptes-rendus

- **Scénario nominal** : le visiteur voit ses comptes-rendus des trois dernières années (RG-15), du plus récent au plus ancien, filtrables par état (tous, brouillons, validés) et par praticien ou ville ; il en ouvre un pour le consulter ou le modifier (UC-12).

### UC-15 — Consulter la synthèse de son activité

- **Scénario nominal** : le visiteur choisit une période prédéfinie (ce mois-ci, le mois dernier, les 3 ou 12 derniers mois, les 3 dernières années) ou des dates libres ; le système affiche, sur ses comptes-rendus **validés** : nombre de visites (dont avec remplaçant), praticiens vus, confiance moyenne, échantillons et leur coût, temps de saisie, brouillons en attente, visites par mois, répartition par motif, produits présentés, échantillons par produit.
- **Erreur** : période invalide ou antérieure aux trois dernières années (RG-15).

### UC-16 — Voir les praticiens à revoir

- **Scénario nominal** : le système liste les praticiens du portefeuille à planifier selon la périodicité (RG-20), du plus urgent au moins urgent ; le visiteur peut lancer directement la saisie d'un compte-rendu pour l'un d'eux (UC-10).

### UC-20 — Consulter l'activité de la région

- **Acteur** : délégué.
- **Scénario nominal** : pour une période, le délégué voit la synthèse de la région (mêmes indicateurs que UC-15), l'activité de chaque visiteur de la région (visiteurs sans visite signalés), le détail d'un visiteur, et les praticiens suivis par l'équipe qui sont à revoir, avec le visiteur qui les suit.
- **Règle** : seuls les comptes-rendus validés des collaborateurs affectés à la région sont pris en compte (RG-30).

### UC-21 — Consulter les comptes-rendus de la région

- **Scénario nominal** : le délégué liste les comptes-rendus validés des visiteurs de sa région, filtre par visiteur ou praticien et en consulte un en **lecture seule**.
- **Règles** : les brouillons des autres ne sont jamais visibles ; un compte-rendu hors de la région est refusé (RG-30).

### UC-22 — Attribuer des échantillons

- **Acteur** : délégué.
- **Scénario nominal** : le délégué choisit le mois, un visiteur de sa région, un produit et une quantité ; le système enregistre la dotation. Une nouvelle saisie pour le même visiteur, produit et mois remplace la précédente. Une dotation peut être supprimée.
- **Règles** : RG-31.

### UC-23 — Contrôler le stock d'échantillons

- **Acteurs** : délégué (sa région), responsable (son secteur, consultation seule).
- **Scénario nominal** : pour un mois, le système affiche par visiteur et par produit la quantité attribuée, la quantité distribuée (échantillons des comptes-rendus validés du mois) et l'écart ; les écarts négatifs sont signalés comme dépassements (RG-32). Filtre possible sur les seuls dépassements.

### UC-30 et UC-31 — Consulter l'activité et les comptes-rendus du secteur

- **Acteur** : responsable de secteur.
- **Scénario nominal** : identiques à UC-20 et UC-21 sur le périmètre du **secteur** (toutes ses régions) ; l'activité par visiteur indique la région de chacun.

### UC-40 — Gérer les collaborateurs

- **Acteur** : administrateur.
- **Scénarios**
  - **Créer** un collaborateur avec son affectation : le système génère un mot de passe provisoire (RG-06), affiché une seule fois, à changer à la première connexion.
  - **Modifier** l'identité, les coordonnées, le login.
  - **Réinitialiser le mot de passe** : nouveau mot de passe provisoire, compte déverrouillé.
  - **Verrouiller / déverrouiller** un compte.
  - **Enregistrer un départ** : le compte devient inactif ; l'affectation et le portefeuille sont clos ; les comptes-rendus sont conservés.
- **Règles** : RG-50 à RG-52.

### UC-41 — Gérer les affectations

- **Scénario nominal** : l'administrateur change le profil, la région ou le secteur d'un collaborateur à partir d'une date d'effet ; l'affectation en cours est close la veille et la nouvelle ouverte (historique conservé, RG-53).

### UC-42 — Gérer les portefeuilles

- **Scénarios** : confier des praticiens sans visiteur à un visiteur ; réattribuer un praticien à un autre visiteur ; transférer tout le portefeuille d'un visiteur à un autre (en une seule transaction).
- **Règles** : RG-21 et RG-54.

### UC-43 — Gérer les référentiels

- **Scénarios** : créer, modifier, désactiver ou réactiver un praticien ; modifier le prix d'échantillon et le statut (commercialisé ou retiré) d'un médicament ; créer un motif de visite, modifier son libellé, le désactiver.
- **Règles** : RG-55.

### UC-44 — Consulter le journal des connexions

- **Scénario nominal** : l'administrateur choisit une période, éventuellement un login et « échecs seulement » ; le système affiche les tentatives (date, login saisi, collaborateur reconnu, résultat), 1 000 au plus, avec le nombre d'échecs.

## 5. Règles de gestion

### Sécurité

| N° | Règle |
|---|---|
| RG-01 | Les mots de passe sont stockés **hachés** (PBKDF2-SHA256, 100 000 itérations, sel aléatoire) ; ils ne sont jamais affichés ni stockés en clair. |
| RG-02 | Après **5 échecs consécutifs** de connexion, le compte est verrouillé ; seul l'administrateur le déverrouille. Une connexion réussie remet le compteur à zéro. |
| RG-03 | Un collaborateur dont la date de départ est passée, ou sans affectation en cours, ne peut plus se connecter ; ses comptes-rendus sont conservés. |
| RG-04 | Toute tentative de connexion est journalisée (date, login saisi, collaborateur reconnu, succès ou échec). |
| RG-05 | Un mot de passe choisi comporte au moins 8 caractères, dont une majuscule, une minuscule, un chiffre et un caractère spécial. |
| RG-06 | Un mot de passe provisoire (création, réinitialisation) est généré aléatoirement (12 caractères), affiché une seule fois et doit être changé à la connexion suivante. |

### Comptes-rendus

| N° | Règle |
|---|---|
| RG-10 | Un compte-rendu est rattaché au collaborateur connecté qui le crée ; son auteur ne change jamais ; seul l'auteur le modifie. |
| RG-11 | Pour **valider** : praticien, date de visite, motif, bilan et coefficient de confiance (1 à 5) obligatoires. |
| RG-12 | Contrôles de cohérence : date de visite non future ; au plus **2 produits présentés**, différents ; échantillons de 1 à 9 999 unités par produit, un produit une seule fois ; précision obligatoire (200 caractères au plus) si le motif est « Autre » ; prochaine visite postérieure à la visite ; remplaçant différent du titulaire ; bilan de 4 000 octets au plus. |
| RG-13 | Chaque ouverture du formulaire de saisie crée une **session de saisie** (début, fin) qui mesure le temps passé. |
| RG-14 | Un compte-rendu est **brouillon** ou **validé**. Seuls les validés comptent dans les statistiques et le contrôle de stock. Un validé ne redevient jamais brouillon et ne peut pas être supprimé ; un brouillon peut l'être par son auteur. |
| RG-15 | La consultation (comptes-rendus, statistiques) porte sur les **trois dernières années**. |

### Praticiens et périodicité

| N° | Règle |
|---|---|
| RG-20 | Un praticien doit être revu tous les **6 à 8 mois**. Ordre d'urgence : plus de 8 mois sans visite ; jamais visité ; visite prévue dépassée ; entre 6 et 8 mois. |
| RG-21 | Un praticien est suivi par **un seul visiteur à la fois** ; l'historique des suivis est conservé. |
| RG-22 | Un praticien inactif n'est plus proposé à la saisie mais reste consultable avec son historique. |

### Équipe et échantillons

| N° | Règle |
|---|---|
| RG-30 | Le délégué voit les comptes-rendus **validés** des collaborateurs affectés à sa région ; le responsable, ceux de toutes les régions de son secteur ; toujours en lecture seule. |
| RG-31 | Seul le délégué attribue des échantillons, uniquement aux visiteurs de sa région, pour un mois compris entre le mois prochain et 12 mois en arrière, en quantité de 1 à 999 999. |
| RG-32 | Écart = quantité attribuée − quantité distribuée (échantillons des comptes-rendus validés du mois). Un écart négatif est un **dépassement** à justifier. |

### Messagerie

| N° | Règle |
|---|---|
| RG-40 | Un message a de 1 à 500 destinataires, un objet (100 caractères au plus) et un texte non vide (4 000 octets au plus). Seul un destinataire peut lire un message reçu ; sa lecture est datée. |

### Administration

| N° | Règle |
|---|---|
| RG-50 | Matricule : 1 à 10 lettres minuscules ou chiffres, unique. Login : 3 à 30 caractères (a-z, 0-9, point, tiret, souligné), unique. |
| RG-51 | Une affectation de visiteur ou de délégué porte une région ; celle d'un responsable, un secteur ; celle d'un administrateur, aucun rattachement. Un seul délégué par région et un seul responsable par secteur. |
| RG-52 | L'administrateur ne peut ni verrouiller son propre compte, ni enregistrer son propre départ, ni se retirer le profil Administrateur. |
| RG-53 | Une nouvelle affectation commence après le début de l'affectation actuelle et au plus tard dans un an ; l'ancienne est close la veille. |
| RG-54 | Un portefeuille n'est confié qu'à un visiteur ou un délégué en poste. |
| RG-55 | Le motif « Autre » existe toujours, reste actif et est proposé en dernier. Un nouveau motif a un code de 2 à 6 lettres. |

## 6. Exigences non fonctionnelles

| Exigence | Réponse |
|---|---|
| EX-03 Application de bureau non reliée à l'intranet | Application Windows (WinForms) connectée uniquement à la base Oracle |
| EX-04 Charte bleu et blanc, prise en main facile | Charte commune à tous les écrans ; tuiles par module ; erreurs listées en clair |
| EX-05 Sécurité | RG-01 à RG-06 ; requêtes SQL paramétrées ; compte Oracle des postes sans droit sur la structure |
| Fiabilité | Enregistrements en transaction (tout ou rien) ; règles doublées par des contraintes et déclencheurs Oracle |
| Performance | Chargements en arrière-plan : l'interface reste utilisable pendant les accès à la base ; listes limitées (500 praticiens, 1 000 lignes de journal) |
| Maintenabilité | Architecture en 4 couches (IHM, Métier, Données, Modèles) ; 248 tests automatisés ; intégration continue |

## 7. Traçabilité

Chaque exigence est reliée à un ou plusieurs cas d'utilisation (tableau de la section 3) et à des scénarios de test ([cahier de recette](cahier-de-recette.md), section 5).
