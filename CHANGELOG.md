# Journal des versions — GSB-CR

Numérotation `MAJEUR.MINEUR.CORRECTIF` : la version est définie dans `Directory.Build.props`
et chaque version livrée est marquée par un tag Git `vX.Y.Z`.

## [Non publié]

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
