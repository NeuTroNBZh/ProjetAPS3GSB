# Journal des décisions techniques

Format : date — décision — raison — statut (proposée / validée / abandonnée).

| # | Date | Décision | Raison | Statut |
|---|---|---|---|---|
| D-01 | 2026-09-28 | VB.NET + Oracle 19c | Imposé par le cahier des charges | Validée |
| D-02 | 2026-09-28 | .NET 10 + WinForms | Version utilisée en cours ; appli poste de travail simple, bien outillée en VB.NET | Validée |
| D-03 | 2026-09-28 | Oracle.ManagedDataAccess.Core (NuGet) | Driver officiel Oracle, sans client Oracle à installer | Validée |
| D-04 | 2026-09-28 | Architecture 4 couches (Modèles / Données / Métier / IHM) | « Conventions d'usage » exigées, testabilité, doc des classes | Validée |
| D-05 | 2026-09-28 | MSTest pour les tests unitaires | Intégré à Visual Studio et `dotnet test` | Validée |
| D-06 | 2026-09-28 | Git + GitHub (dépôt privé) + GitHub Actions pour l'intégration continue | Livrable « intégration continue » de l'AP | Validée |
| D-07 | 2026-09-28 | Mots de passe hachés (PBKDF2 via `Rfc2898DeriveBytes`) | Sécurité, aucune dépendance externe | Proposée |
| D-08 | 2026-09-28 | Serveur : `100.109.217.110:1521`, service **FREEPDB1**, schéma dédié **GSB** créé de zéro (`bdd/00_creation_utilisateur.sql`) | Pas de base exemple fournie ; schéma séparé des autres TP (GESPROD, SCOTT…) | Validée |
| D-09 | 2026-09-28 | Config : `appsettings.json` (serveur, versionné) + `appsettings.Local.json` (identifiants, **non versionné**) | Aucun secret dans Git | Validée |
| D-10 | 2026-09-28 | Tests nécessitant Oracle marqués `<TestCategory("Integration")>` et exclus de la CI | La CI GitHub n'a pas accès au serveur | Validée |
| D-11 | 2026-09-28 | Le serveur est en **Oracle 26ai Free (23.26)** alors que le CDC impose 19c → SQL écrit **compatible 19c** : pas de type `BOOLEAN` SQL (utiliser `CHAR(1)` O/N ou `NUMBER(1)`), pas de `IF [NOT] EXISTS`, pas de `GROUP BY` sur alias, pas de `SELECT` sans `FROM` | Rester conforme au cahier des charges tout en utilisant le serveur disponible | Validée |
| D-12 | 2026-09-28 | Modèle complet : 28 tables, 6 vues, 3 déclencheurs (`docs/modele-donnees.md`) | « Penser à tout » : historique, portefeuille, posologie, composition, stock, messagerie, sécurité | Validée |
| D-13 | 2026-09-28 | Remplaçant = PRATICIEN à part entière, référencé par `pra_num_remplacant` sur le rapport | Garder son historique s'il reprend un cabinet | Validée |
| D-14 | 2026-09-28 | CR en **brouillon / validé** ; contraintes de complétude seulement à la validation | Saisie en plusieurs fois sans perdre la garantie de CR complets | Validée |
| D-15 | 2026-09-28 | Format mot de passe `PBKDF2-SHA256$100000$<sel>$<clé>` (sel 16 o, clé 32 o) | Compatible `Rfc2898DeriveBytes.Pbkdf2` et le jeu d'essai | Validée |
| D-16 | 2026-09-28 | Règles simples en **base** (CHECK, index uniques, triggers) + règles de droits / périodes en couche **Métier** | Données cohérentes même hors de l'appli, logique métier testable en VB | Validée |
| D-17 | 2026-09-28 | Module **Administration** (profil ADM) ajouté au périmètre | Gestion des comptes et référentiels non prévue par le CDC | Validée (v0.8.0) |
| D-18 | 2026-09-28 | Documentation UML 2 en PlantUML (`docs/uml/`) : sources versionnées + images SVG/PNG | Diagrammes modifiables et comparables dans Git, rendu direct sur GitHub | Validée |
| D-19 | 2026-09-28 | Mot de passe provisoire **généré** (12 caractères aléatoires, sans caractères ambigus) à la création et à la réinitialisation, affiché une seule fois, changement imposé à la connexion | L'administrateur ne choisit ni ne connaît durablement le mot de passe ; seul le haché est stocké | Validée |
| D-20 | 2026-09-28 | Portefeuilles et affectations **historisés** : un transfert clôt le suivi la veille et en ouvre un nouveau, jamais de suppression | Les CR et statistiques passés restent attribués au bon visiteur | Validée |
| D-21 | 2026-09-28 | Postes connectés avec un compte Oracle **GSB_APP** (lecture et écriture des données seulement) ; paramètre `Schema` pour travailler sur le schéma GSB | Le mot de passe stocké sur les postes ne donne aucun droit sur la structure de la base | Validée |
| D-22 | 2026-09-28 | Référentiels séparés du jeu d'essai ; installation de production dédiée (`installer_production.sql`) qui refuse un schéma existant et crée un seul administrateur au mot de passe provisoire haché hors du dépôt | Aucune donnée fictive ni mot de passe connu en production, aucune suppression accidentelle | Validée |
| D-23 | 2026-09-28 | Livraison en **releases GitHub** publiées automatiquement à la pose d'un tag (workflow « Release ») : paquet poste avec runtime .NET inclus et installateur PowerShell 5.1 (`Installer.cmd`, mode silencieux), kit base Oracle, empreintes SHA-256 | Installation sans prérequis ni édition de fichiers sur les postes, paquets reproductibles construits par la CI après les tests | Validée (v1.0.0) |
| D-24 | 2026-09-28 | Export des statistiques en **CSV « Excel français »** (point-virgule, virgule décimale, UTF-8 avec BOM, cellules commençant par = + - @ préfixées d'une apostrophe) plutôt qu'un fichier .xlsx ; code d'un nouveau dosage **calculé** à partir de sa valeur (« 500MG », « 0V5PC ») | Aucune bibliothèque supplémentaire, ouverture directe par double-clic dans Excel, pas d'injection de formule ; codes de dosage lisibles, identiques au jeu d'essai, doublons détectés par la clé primaire | Validée (v1.1.0) |
