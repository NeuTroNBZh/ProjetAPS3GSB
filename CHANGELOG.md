# Journal des versions — GSB-CR

Numérotation `MAJEUR.MINEUR.CORRECTIF` : la version est définie dans `Directory.Build.props`
et chaque version livrée est marquée par un tag Git `vX.Y.Z`.

## [Non publié]

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
