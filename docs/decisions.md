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
| D-08 | 2026-09-28 | Serveur Oracle 19c : `100.109.217.110:1521`, base créée **de zéro** (pas de base exemple fournie) | Infos du projet | Validée |
| D-09 | 2026-09-28 | Config : `appsettings.json` (serveur, versionné) + `appsettings.Local.json` (identifiants, **non versionné**) | Aucun secret dans Git | Validée |
| D-10 | 2026-09-28 | Tests nécessitant Oracle marqués `<TestCategory("Integration")>` et exclus de la CI | La CI GitHub n'a pas accès au serveur | Validée |
