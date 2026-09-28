# Journal des versions — GSB-CR

Numérotation `MAJEUR.MINEUR.CORRECTIF` : la version est définie dans `Directory.Build.props`
et chaque version livrée est marquée par un tag Git `vX.Y.Z`.

## [Non publié]

## [0.8.0] — 2026-09-28

### Ajouté
- Module Administration pour le profil Administrateur (EX-70 à EX-74) :
  - collaborateurs : création avec mot de passe provisoire généré, modification, changement d'affectation (historique conservé), départ, réinitialisation du mot de passe, verrouillage / déverrouillage ; l'administrateur ne peut ni se verrouiller ni se retirer ses droits ;
  - portefeuilles : praticiens sans visiteur, attribution, réattribution d'un praticien et transfert d'un portefeuille complet ;
  - référentiels : praticiens (création, modification, désactivation), prix et statut des médicaments, motifs de visite (« Autre » toujours proposé en dernier) ;
  - journal des connexions filtrable par période, login et échecs.
- Formulaire de saisie générique réutilisable par les écrans d'administration.

### Modifié
- Les erreurs Oracle transmettent leur code : un doublon (login, matricule, code) produit un message clair.

## [0.7.0] — 2026-09-28

### Ajouté
- Messagerie interne pour tous les profils (EX-50) : envoi à une ou plusieurs personnes ou à un groupe (région, secteur), boîte de réception avec messages non lus en évidence, messages envoyés avec suivi de lecture par destinataire, réponse avec citation.
- Nombre de messages non lus affiché sur la tuile Messagerie du menu principal.

## [0.6.0] — 2026-09-28

### Ajouté
- Module Délégué, « Ma région » (EX-30 à EX-35) et module Responsable, « Mon secteur » (EX-40, EX-41), construits sur la notion de périmètre (région ou secteur) :
  - synthèse de l'équipe sur une période ; activité de chaque visiteur (visiteurs sans visite signalés) et détail d'un visiteur ;
  - comptes-rendus validés de l'équipe, filtrables par visiteur et praticien, consultables en lecture seule (les brouillons restent privés) ;
  - praticiens suivis par l'équipe à revoir, avec le visiteur qui les suit.
- Échantillons (EX-33, EX-34) : contrôle de stock mensuel attribué / distribué avec dépassements signalés ; saisie, correction et suppression des dotations par le délégué pour les visiteurs de sa région ; consultation seule pour le responsable.

### Modifié
- Accès aux données : filtre commun par périmètre (collaborateur, région, secteur) réutilisé par l'activité, les comptes-rendus et les praticiens.

## [0.5.0] — 2026-09-28

### Ajouté
- Module « Mon activité » (visiteurs et délégués) :
  - synthèse sur une période prédéfinie ou personnalisée, limitée aux trois dernières années (EX-23) : visites, praticiens vus, remplaçants rencontrés, confiance moyenne, échantillons et leur coût, temps de saisie, brouillons à terminer ;
  - visites par mois (graphique et tableau), répartition par motif, produits présentés, échantillons par produit ;
  - praticiens du portefeuille à revoir, classés par urgence : plus de 8 mois, jamais visités, visite prévue dépassée, 6 à 8 mois (EX-24) ; saisie directe d'un compte-rendu pour le praticien choisi.
- Composants d'interface réutilisables : histogramme et tuiles d'indicateurs.

### Corrigé
- Boutons désactivés grisés pour ne pas sembler cliquables.
- Moyennes arrondies côté base (la précision d'Oracle dépasse celle du type Decimal).

## [0.4.0] — 2026-09-28

### Ajouté
- Fiches praticiens, pour tous les profils (EX-21) : recherche par nom, prénom ou ville, filtre « mon portefeuille » (visiteurs et délégués), praticiens inactifs ; coordonnées, spécialités avec diplôme et coefficient de prescription, notoriété, visiteur qui le suit ; historique des visites, y compris celles où le praticien a été rencontré comme remplaçant.
- Périodicité des visites : à jour (moins de 6 mois), à revoir bientôt (6 à 8 mois), à revoir (plus de 8 mois), jamais visité ; prochaine visite conseillée.
- Fiches médicaments, pour tous les profils (EX-22) : recherche, filtre par famille, médicaments retirés ; effets, contre-indications, composition, interactions dans les deux sens, posologie par type de patient, présentation et dosage.
- Contrôle réutilisable de fiche détaillée pour l'interface.

### Corrigé
- Les suites des traitements en arrière-plan reviennent toujours sur le thread de l'interface, y compris entre deux fenêtres modales.

## [0.3.0] — 2026-09-28

### Ajouté
- Module « Mes comptes-rendus » (visiteurs et délégués) :
  - liste des comptes-rendus des trois dernières années, filtres par état et par praticien (EX-20) ;
  - saisie d'un compte-rendu : praticien du portefeuille, remplaçant rencontré (avec création de sa fiche), date de visite, motif standardisé ou « Autre » précisé, deux produits présentés au maximum, échantillons à l'unité, bilan, coefficient de confiance, prochaine visite (EX-10 à EX-19, EX-28) ;
  - enregistrement en brouillon ou validation ; un compte-rendu validé reste modifiable mais ne repasse pas en brouillon ; suppression des brouillons (EX-25, EX-29) ;
  - traçage du temps de saisie (EX-26) ; date de visite non future (EX-27).
- Couche Données : classe de base commune des DAO, écritures en transaction (tout ou rien).
- Diagramme de séquence de l'enregistrement d'un compte-rendu ; diagramme de classes mis à jour.
- Documentation UML 2 (`docs/uml/`) : cas d'utilisation, classes du domaine, classes de l'application, séquence de connexion, états d'un compte-rendu, activité de saisie, déploiement.
- Exigence EX-29 : suppression d'un brouillon, un CR validé ne repasse pas en brouillon.
- Tests : 97 tests unitaires et 24 tests d'intégration.

## [0.2.0] — 2026-09-28

### Ajouté
- Connexion à l'application (EX-01, EX-02) : page d'accueil limitée à l'identification, ouverture du menu correspondant au profil (visiteur, délégué, responsable, administrateur).
- Sécurité (EX-05 à EX-09) : mots de passe hachés PBKDF2, verrouillage après 5 échecs, refus des collaborateurs partis, changement de mot de passe obligatoire à la première connexion, journal des connexions.
- Changement de mot de passe à la demande, avec règles de robustesse.
- Menu principal aux couleurs GSB avec les modules accessibles selon le profil.
- Tests : 56 tests unitaires (couche Métier) et 13 tests d'intégration sur la base.

## [0.1.0] — 2026-09-28

### Ajouté
- Documentation du projet : contexte métier, exigences numérotées (EX-xx), glossaire, décisions techniques, feuille de route.
- Solution `GSB.CR.slnx` (.NET 10, VB.NET) : projets Modeles, Donnees, Metier, IHM (WinForms) et Tests (MSTest).
- Accès Oracle : lecture de la configuration (`appsettings.json` + `appsettings.Local.json` non versionné) et connexion.
- Intégration continue GitHub Actions : compilation et tests unitaires à chaque push.
- Base de données Oracle (schéma GSB) : 28 tables, 6 vues, 3 déclencheurs, jeu d'essai, script d'installation et tests des règles de gestion (21 règles vérifiées).
