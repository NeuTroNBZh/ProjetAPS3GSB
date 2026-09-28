# Cahier de recette — GSB-CR

Version testée : **0.8.0**. Ce document décrit comment vérifier que l'application répond à chaque exigence (`EX-xx`, voir [exigences.md](exigences.md)) avant la mise en exploitation.

## 1. Stratégie de test

La recette repose sur trois niveaux complémentaires.

| Niveau | Quoi | Outil | Quand |
|---|---|---|---|
| Tests unitaires | Règles de gestion de la couche Métier (validation d'un CR, droits, périodicité, mots de passe, services) avec des DAO en mémoire | MSTest (`tests/GSB.CR.Tests`) | À chaque commit, automatiquement (GitHub Actions) |
| Tests d'intégration | Accès Oracle réel : requêtes, transactions, déclencheurs, contraintes | MSTest, catégorie `Integration` | Avant chaque version, sur la base de développement |
| Recette fonctionnelle | Scénarios utilisateur de bout en bout dans l'application (ce document) | Manuel, avec les comptes du jeu d'essai | Avant la livraison |

Commandes :

```bash
dotnet test GSB.CR.slnx --filter "TestCategory!=Integration"   # unitaires (comme la CI)
dotnet test GSB.CR.slnx                                          # tous, base Oracle joignable
```

Résultat de référence pour la version 1.1.0 : **288 tests, 0 échec** (dont 66 d'intégration Oracle).

## 2. Environnement de recette

- Poste Windows 10 ou 11 avec le runtime .NET 10 (Windows Desktop).
- Base Oracle réinstallée avec le jeu d'essai : `bdd/installer.sql` (efface et recrée le schéma GSB). Les règles de la base se vérifient avec `bdd/tests_regles.sql` (21 règles).
- Fichier `appsettings.Local.json` renseigné (identifiants du schéma, jamais versionnés).
- Comptes : voir [comptes-test.md](comptes-test.md). Mot de passe commun : `Gsb2026!`.

Réinstaller la base avant une campagne complète : plusieurs scénarios modifient les données (verrouillage, création de CR, transferts).

## 3. Critères d'acceptation

- Tous les tests automatisés passent.
- Tous les scénarios de priorité **P1** sont au statut OK.
- Aucune anomalie bloquante ouverte sur les priorités P2 et P3 ; les anomalies mineures sont listées au procès-verbal (section 6) avec leur correctif prévu.

Statuts : **OK** (résultat conforme), **KO** (anomalie, à décrire en section 6), **NT** (non testé, à justifier).

## 4. Scénarios

Colonnes « Obtenu » et « Statut » à remplir pendant la campagne.

### 4.1 Connexion et sécurité

| N° | Exigence | Compte | Étapes | Résultat attendu | Tests auto | Obtenu | Statut |
|---|---|---|---|---|---|---|---|
| R-01 | EX-01 | — | Lancer l'application. | Seule la fenêtre « Connexion » s'affiche (identifiant, mot de passe, « Se connecter »). Aucun module accessible sans identification. | — | | |
| R-02 | EX-02 | `lvillechalane` | Se connecter. | Menu avec les tuiles du visiteur : Mes comptes-rendus, Mon activité, Praticiens, Médicaments, Messagerie. Pas de « Ma région », « Échantillons » ni « Administration ». | AutorisationsTests | | |
| R-03 | EX-02 | `cbedos`, `clemoine`, `admin` | Se connecter successivement avec chaque compte. | Délégué : tuiles du visiteur + Ma région + Échantillons. Responsable : Mon secteur, Échantillons (consultation), Praticiens, Médicaments, Messagerie. Administrateur : Administration, Praticiens, Médicaments, Messagerie. | AutorisationsTests | | |
| R-04 | EX-05 | `lvillechalane` | Saisir un mauvais mot de passe. | Message « Identifiant ou mot de passe incorrect. », identique à celui d'un login inconnu (on ne révèle pas si le login existe). | ServiceAuthentificationTests | | |
| R-05 | EX-06 | `fbunisset` | Saisir 5 fois un mauvais mot de passe. | Au 5e échec : « Votre compte est verrouillé. Contactez l'administrateur. » Le bon mot de passe est ensuite refusé. Les derniers essais affichent le nombre de tentatives restantes. | ServiceAuthentificationTests, AuthentificationIntegrationTests | | |
| R-06 | EX-06, EX-70 | `admin` | Administration > Collaborateurs, sélectionner F. Bunisset, « Déverrouiller ». Se reconnecter avec `fbunisset`. | Compte à nouveau « Actif », connexion acceptée avec le bon mot de passe. | ServiceAdministrationTests | | |
| R-07 | EX-07 | `dbunisset` | Se connecter. | La fenêtre de changement de mot de passe s'ouvre d'office ; impossible d'accéder au menu sans changer le mot de passe. | ServiceAuthentificationTests | | |
| R-08 | EX-07 | `dbunisset` | Proposer un nouveau mot de passe trop simple (ex. `abc`), puis un mot de passe conforme. | Refus avec la règle affichée (au moins 8 caractères, dont une majuscule, une minuscule, un chiffre et un caractère spécial), puis acceptation et accès au menu. | PolitiqueMotDePasseTests | | |
| R-09 | EX-07 | `lvillechalane` | Menu principal > bouton « Mot de passe ». | Changement possible à tout moment ; l'ancien mot de passe est demandé. | ServiceAuthentificationTests | | |
| R-10 | EX-08, EX-74 | `admin` | Après R-04 et R-05 : Administration > Journal des connexions, cocher « Échecs seulement », « Afficher ». | Les tentatives échouées apparaissent avec la date, le login saisi, le collaborateur et le résultat « Échec » en rouge. | AdministrationDaoIntegrationTests | | |
| R-11 | EX-09 | `fdaburon` | Se connecter avec le bon mot de passe. | Refus : « Ce compte n'est plus actif. Contactez l'administrateur. » | ServiceAuthentificationTests, AuthentificationIntegrationTests | | |
| R-12 | EX-05 | — | Consulter la table `COLLABORATEUR` (SQLcl). | Colonne mot de passe au format `PBKDF2-SHA256$100000$…` : aucun mot de passe en clair. | HacheurMotDePasseTests | | |

### 4.2 Saisie d'un compte-rendu (module Visiteur)

| N° | Exigence | Compte | Étapes | Résultat attendu | Tests auto | Obtenu | Statut |
|---|---|---|---|---|---|---|---|
| R-20 | EX-28 | `lvillechalane` | Mes comptes-rendus > « Nouveau compte-rendu ». Ouvrir la liste des praticiens. | Seuls les praticiens de son portefeuille sont proposés, avec recherche par nom ou ville. | ServiceRapportsTests | | |
| R-21 | EX-11 | `lvillechalane` | Choisir un praticien, laisser motif et bilan vides, « Valider le compte-rendu ». | Refus avec la liste des champs manquants (motif, bilan, confiance). Rien n'est enregistré. | ValidateurRapportTests | | |
| R-22 | EX-12 | `lvillechalane` | Choisir le motif « Autre » sans précision, valider. | Refus : la précision est obligatoire pour le motif « Autre ». Avec une précision : accepté. | ValidateurRapportTests | | |
| R-23 | EX-13 | `lvillechalane` | Choisir le même produit en produit 1 et produit 2. | Refus : deux produits présentés maximum, et différents. | ValidateurRapportTests | | |
| R-24 | EX-14 | `lvillechalane` | Ajouter deux échantillons (quantités 3 et 1), dont un produit non présenté. Puis une quantité 0. | Échantillons acceptés indépendamment des produits présentés ; quantité 0 refusée. | ValidateurRapportTests | | |
| R-25 | EX-27 | `lvillechalane` | Date de visite = demain. | Refus : la date de visite ne peut pas être dans le futur (contrôle applicatif et déclencheur Oracle). | ValidateurRapportTests, RapportDaoIntegrationTests | | |
| R-26 | EX-19 | `lvillechalane` | Prochaine visite antérieure à la date de visite. | Refus ; avec une date postérieure : accepté. | ValidateurRapportTests | | |
| R-27 | EX-15 | `lvillechalane` | Valider sans coefficient de confiance, puis avec 4. | Refus puis acceptation ; le coefficient va de 1 à 5. | ValidateurRapportTests | | |
| R-28 | EX-16 | `lvillechalane` | Cocher « La personne rencontrée est un remplaçant », « Nouveau… », « Créer la fiche », valider le CR. | Le CR garde le titulaire du cabinet et la personne rencontrée ; le remplaçant est créé et réutilisable. | ServiceRapportsTests | | |
| R-29 | EX-10, EX-17 | `lvillechalane` | Valider un CR complet, le rouvrir. | Auteur = Louis Villechalane (non modifiable) ; date de saisie renseignée automatiquement. | ServiceRapportsTests, RapportDaoIntegrationTests | | |
| R-30 | EX-25 | `lvillechalane` | Saisir seulement praticien et date, « Enregistrer le brouillon ». | Accepté en brouillon ; le CR n'apparaît pas dans Mon activité (seuls les validés comptent). | ValidateurRapportTests, ServiceActiviteTests | | |
| R-31 | EX-25, EX-29 | `dandre` | Ouvrir son brouillon, le compléter, « Valider le compte-rendu ». | Passe à l'état Validé ; la date de validation est enregistrée. Il ne peut plus repasser en brouillon. | ServiceRapportsTests | | |
| R-32 | EX-29 | `dandre` | Sur un autre brouillon : « Supprimer le brouillon ». Sur un CR validé : chercher la suppression. | Brouillon supprimé après confirmation ; aucune suppression possible pour un CR validé. | ServiceRapportsTests, RapportDaoIntegrationTests | | |
| R-33 | EX-18 | `lvillechalane` | Modifier le bilan d'un CR validé, enregistrer. | Modification acceptée ; seule la dernière version est conservée ; date de modification mise à jour. | ServiceRapportsTests | | |
| R-34 | EX-26 | `lvillechalane` | Ouvrir un CR, attendre, enregistrer. Rouvrir et enregistrer à nouveau. | Chaque ouverture crée une session de saisie (début, fin) ; le temps total est visible dans Mon activité. | ServiceRapportsTests | | |
| R-35 | EX-10 | `cbedos` | Essayer d'ouvrir en modification un CR d'un visiteur de sa région. | Consultation en lecture seule : on ne modifie que ses propres comptes-rendus. | ServiceRapportsTests | | |

### 4.3 Consultation (module Visiteur)

| N° | Exigence | Compte | Étapes | Résultat attendu | Tests auto | Obtenu | Statut |
|---|---|---|---|---|---|---|---|
| R-40 | EX-20 | `lvillechalane` | Mes comptes-rendus : filtrer par période puis par praticien. | Seuls les CR des 3 dernières années sont listés (son CR de plus de 3 ans n'apparaît pas) ; filtres opérants. | ServiceRapportsTests, RapportDaoIntegrationTests | | |
| R-41 | EX-21 | `lvillechalane` | Praticiens : rechercher « Martin », ouvrir la fiche. | Coordonnées, type, spécialités, date de dernière visite et historique des visites. | ServiceConsultationTests, ConsultationDaoIntegrationTests | | |
| R-42 | EX-22 | `lvillechalane` | Médicaments : ouvrir AMOPIL. | Famille, composition, effets, contre-indications, interactions et posologie. | ConsultationDaoIntegrationTests | | |
| R-43 | EX-23 | `lvillechalane` | Mon activité : choisir « Les 12 derniers mois », puis une période personnalisée. | Nombre de visites, praticiens vus, échantillons et leur coût, graphique par mois (et tableau), motifs, temps de saisie. Période limitée à 3 ans. | ServiceActiviteTests, ActiviteDaoIntegrationTests | | |
| R-44 | EX-24 | `lvillechalane` | Mon activité > Praticiens à revoir. | Classement du plus urgent au moins urgent : plus de 8 mois, jamais visités, visite prévue dépassée, 6 à 8 mois. | PeriodiciteTests, ServiceActiviteTests | | |
| R-45 | EX-51 | `lvillechalane` | Mon activité : « Les 12 derniers mois », « Afficher », puis « Exporter en CSV » ; enregistrer dans Documents et ouvrir le fichier avec Excel. | Nom proposé `GSB-activite-Mon-activite-<début>-au-<fin>.csv` ; colonnes séparées, accents corrects, virgule décimale ; mêmes chiffres qu'à l'écran, tous les mois de la période. Réessayer avec le fichier ouvert dans Excel : message « Impossible d'écrire le fichier ». | ExportStatistiquesTests | | |

### 4.4 Module Délégué régional

| N° | Exigence | Compte | Étapes | Résultat attendu | Tests auto | Obtenu | Statut |
|---|---|---|---|---|---|---|---|
| R-50 | EX-30 | `cbedos` | Saisir un compte-rendu comme un visiteur. | Mêmes fonctions et contrôles que R-20 à R-34. | AutorisationsTests | | |
| R-51 | EX-31 | `cbedos` | Ma région : choisir une période. | Synthèse de la région, activité par visiteur (les visiteurs sans visite sont signalés), graphique ; double-clic : détail d'un visiteur. | ServiceEquipeTests, EquipeIntegrationTests | | |
| R-52 | EX-32 | `cbedos` | Ma région > Comptes-rendus : filtrer par visiteur. | CR validés des visiteurs d'Aquitaine, en lecture seule ; les brouillons des visiteurs ne sont pas visibles. | ServiceEquipeTests | | |
| R-53 | EX-33 | `ecadic` | Échantillons : choisir un mois, un visiteur, un produit, une quantité, « Enregistrer la dotation ». Saisir à nouveau pour le même trio. | Dotation enregistrée ; la seconde saisie remplace la première. Impossible de doter un visiteur hors de sa région. | ServiceEchantillonsTests | | |
| R-54 | EX-34 | `ecadic` | Échantillons : contrôle de stock, cocher « Seulement les dépassements ». | L'écart négatif de Denise Bunisset sur EQUILAR est signalé. | ServiceEchantillonsTests, LigneStockTests | | |
| R-55 | EX-35 | `cbedos` | Ma région > Praticiens à revoir. | Praticiens de la région non visités depuis plus de 8 mois, avec le visiteur qui les suit. | ServiceEquipeTests | | |
| R-56 | EX-51 | `cbedos` | Ma région : « Afficher » puis « Exporter en CSV ». | Le fichier contient en plus la section « Activité par visiteur » (une ligne par visiteur de la région). | ExportStatistiquesTests | | |

### 4.5 Module Responsable de secteur

| N° | Exigence | Compte | Étapes | Résultat attendu | Tests auto | Obtenu | Statut |
|---|---|---|---|---|---|---|---|
| R-60 | EX-40 | `pgarnier` | Mon secteur : choisir une période, puis double-cliquer sur un visiteur. | Statistiques et graphique du secteur Ouest ; activité de chaque visiteur avec sa région ; détail d'un visiteur. | ServiceEquipeTests, EquipeIntegrationTests | | |
| R-61 | EX-41 | `pgarnier` | Mon secteur > Comptes-rendus. | CR validés des visiteurs du secteur, en lecture seule. | ServiceEquipeTests | | |
| R-62 | EX-34 | `clemoine` | Échantillons. | Contrôle de stock consultable ; pas de saisie de dotation. | ServiceEchantillonsTests | | |
| R-63 | EX-51 | `pgarnier` | Mon secteur : « Exporter en CSV ». | Activité par visiteur avec la région de chacun. | ExportStatistiquesTests | | |

### 4.6 Messagerie

| N° | Exigence | Compte | Étapes | Résultat attendu | Tests auto | Obtenu | Statut |
|---|---|---|---|---|---|---|---|
| R-70 | EX-50 | `cbedos` | Messagerie > « Nouveau message », « Ajouter tout un groupe » (région Aquitaine), objet et message, « Envoyer ». | Message envoyé à tous les membres de la région ; visible dans « Envoyés » avec le suivi de lecture. | ServiceMessagerieTests, MessagerieDaoIntegrationTests | | |
| R-71 | EX-50 | `lvillechalane` | Se connecter après R-70. | La tuile Messagerie affiche le nombre de messages non lus ; le message est en gras dans « Reçus », puis marqué lu à l'ouverture. | MessagerieDaoIntegrationTests | | |
| R-72 | EX-50 | `lvillechalane` | « Répondre » au message. | Réponse pré-remplie (destinataire, objet, citation) ; l'expéditeur la reçoit. | ServiceMessagerieTests | | |

### 4.7 Module Administration

| N° | Exigence | Compte | Étapes | Résultat attendu | Tests auto | Obtenu | Statut |
|---|---|---|---|---|---|---|---|
| R-80 | EX-70 | `admin` | Collaborateurs > « Nouveau collaborateur » : matricule, nom, prénom, login, profil Visiteur, région Normandie. | Compte créé ; mot de passe provisoire affiché une seule fois et copié. Première connexion : changement obligatoire. | ServiceAdministrationTests, AdministrationDaoIntegrationTests | | |
| R-81 | EX-70 | `admin` | Créer un collaborateur avec un login déjà utilisé. | Refus avec un message clair (doublon). | ServiceAdministrationTests | | |
| R-82 | EX-70 | `admin` | « Réinitialiser le mot de passe » d'un visiteur. | Nouveau mot de passe provisoire ; compte déverrouillé ; changement imposé à la connexion suivante. | ServiceAdministrationTests, AdministrationDaoIntegrationTests | | |
| R-83 | EX-70 | `admin` | « Enregistrer le départ » d'un visiteur ayant un portefeuille. | Avertissement sur le portefeuille ; après confirmation, le compte est « Parti », ses praticiens passent « sans visiteur », ses CR restent consultables. | ServiceAdministrationTests, AdministrationDaoIntegrationTests | | |
| R-84 | EX-70 | `admin` | Essayer de verrouiller son propre compte ou de se retirer le profil Administrateur. | Refusé. | ServiceAdministrationTests | | |
| R-85 | EX-71 | `admin` | « Changer d'affectation » d'un visiteur (autre région, date d'effet demain). | Nouvelle affectation à la date d'effet ; l'ancienne est close la veille ; l'historique apparaît dans la fiche. | AdministrationDaoIntegrationTests | | |
| R-86 | EX-72 | `admin` | Portefeuilles : sélectionner des praticiens sans visiteur, « Confier les praticiens sélectionnés à ce visiteur ». | Praticiens ajoutés au portefeuille ; la liste « sans visiteur » diminue. | AdministrationDaoIntegrationTests | | |
| R-87 | EX-72 | `admin` | « Transférer tout le portefeuille… » vers un autre visiteur. | Tous les praticiens sont transférés en une transaction ; message « n praticien(s) transféré(s) » ; l'historique des visites est conservé. | ServiceAdministrationTests, AdministrationDaoIntegrationTests | | |
| R-88 | EX-73 | `admin` | Référentiels : créer un praticien, désactiver un praticien, modifier le prix d'un médicament, créer un motif. | Enregistrements pris en compte ; le praticien désactivé n'est plus proposé à la saisie ; le nouveau motif apparaît avant « Autre », qui ne peut pas être désactivé. | ServiceAdministrationTests | | |
| R-94 | EX-73 | `admin` | Référentiels > Médicaments : NOVELIX > « Composition, interactions, posologie ». « Nouveau composant… » (code KETP, Kétoprofène), puis l'ajouter (50 mg) ; l'ajouter une seconde fois ; puis « Retirer ». | Composant créé et ajouté ; second ajout refusé (« figure déjà dans la composition ») ; retrait après confirmation. La fiche Médicaments d'un visiteur reflète chaque changement. | ServiceAdministrationMedicamentsTests, MedicamentsDaoIntegrationTests | | |
| R-95 | EX-73 | `admin` | Onglet Interactions : ajouter DOLORIL, sens « NOVELIX perturbe l'effet de l'autre médicament » ; essayer avec NOVELIX lui-même impossible (absent de la liste). Ouvrir la fiche de DOLORIL. | L'interaction apparaît sur les deux fiches, dans le bon sens. | MedicamentsDaoIntegrationTests | | |
| R-96 | EX-73 | `admin` | Onglet Posologie : « Nouveau dosage… » (200 mg), puis « Ajouter une posologie » Enfant, Comprimé, 200 mg ; recommencer la même combinaison. | Dosage 200 mg proposé ; posologie ajoutée ; doublon refusé (« Une posologie existe déjà… »). | ServiceAdministrationMedicamentsTests | | |
| R-89 | EX-74 | `admin` | Journal des connexions : filtrer par login. | Seules les tentatives du login saisi sont listées, avec le nombre d'échecs. | AdministrationDaoIntegrationTests | | |

### 4.8 Exigences non fonctionnelles

| N° | Exigence | Vérification | Résultat attendu | Obtenu | Statut |
|---|---|---|---|---|---|
| R-90 | EX-03 | Revue de l'architecture et du déploiement. | Application de bureau autonome, sans lien vers l'intranet ni le site web ; aucune version mobile. | | |
| R-91 | EX-04 | Revue des écrans. | Charte bleu et blanc sur toutes les fenêtres ; boutons principaux en bleu GSB. | | |
| R-92 | EX-05 | Revue du code (`src/GSB.CR.Donnees`). | Toutes les requêtes utilisent des paramètres (`:param`), aucune concaténation de saisie. | | |
| R-93 | — | Couper l'accès au serveur Oracle puis tenter une connexion. | Message « Le serveur est indisponible. Réessayez dans quelques instants. », pas de plantage. | | |

## 5. Matrice de couverture

| Exigence | Scénarios | Couverte par des tests automatisés |
|---|---|---|
| EX-01 | R-01 | non (écran) |
| EX-02 | R-02, R-03 | oui |
| EX-03 | R-90 | non (architecture) |
| EX-04 | R-91 | non (charte) |
| EX-05 | R-04, R-12, R-92 | oui |
| EX-06 | R-05, R-06 | oui |
| EX-07 | R-07, R-08, R-09 | oui |
| EX-08 | R-10 | oui |
| EX-09 | R-11 | oui |
| EX-10 | R-29, R-35 | oui |
| EX-11 | R-21 | oui |
| EX-12 | R-22 | oui |
| EX-13 | R-23 | oui |
| EX-14 | R-24 | oui |
| EX-15 | R-27 | oui |
| EX-16 | R-28 | oui |
| EX-17 | R-29 | oui |
| EX-18 | R-33 | oui |
| EX-19 | R-26 | oui |
| EX-20 | R-40 | oui |
| EX-21 | R-41 | oui |
| EX-22 | R-42 | oui |
| EX-23 | R-43 | oui |
| EX-24 | R-44 | oui |
| EX-25 | R-30, R-31 | oui |
| EX-26 | R-34 | oui |
| EX-27 | R-25 | oui |
| EX-28 | R-20 | oui |
| EX-29 | R-31, R-32 | oui |
| EX-30 | R-50 | oui |
| EX-31 | R-51 | oui |
| EX-32 | R-52 | oui |
| EX-33 | R-53 | oui |
| EX-34 | R-54, R-62 | oui |
| EX-35 | R-55 | oui |
| EX-40 | R-60 | oui |
| EX-41 | R-61 | oui |
| EX-50 | R-70, R-71, R-72 | oui |
| EX-51 | R-45, R-56, R-63 | oui |
| EX-70 | R-06, R-80 à R-84 | oui |
| EX-71 | R-85 | oui |
| EX-72 | R-86, R-87 | oui |
| EX-73 | R-88, R-94 à R-96 | oui |
| EX-74 | R-10, R-89 | oui |

## 6. Procès-verbal de recette

| Campagne | Date | Version | Testeur | Scénarios OK / KO / NT | Décision |
|---|---|---|---|---|---|
| 1 | | 1.1.0 | | | |

### Anomalies relevées

| N° | Scénario | Description | Gravité (bloquante, majeure, mineure) | Correctif | Statut |
|---|---|---|---|---|---|
| | | | | | |
