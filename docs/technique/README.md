# Documentation technique — GSB-CR

Guide destiné au développeur qui reprend l'application (transfert de compétences, EX-60 et EX-62). Version décrite : 1.1.0.

| Document | Contenu |
|---|---|
| Ce guide | Architecture, bibliothèques, environnement, conception de chaque couche, tests, fiches pratiques |
| [Référence des classes](reference-classes.md) | Rôle de chaque classe et de ses membres, généré depuis le code |
| [Modèle de données](../modele-donnees.md) | Tables, contraintes, vues, déclencheurs, scripts |
| [Diagrammes UML](../uml/README.md) | Classes, composants, paquetages, séquences, déploiement… |
| [Décisions](../decisions.md) | Choix techniques et fonctionnels, avec leur justification |
| [Mise en exploitation](../mise-en-exploitation.md) | Installation, déploiement, sauvegardes |

## 1. Architecture

Application de bureau **VB.NET / WinForms** sur **.NET 10**, base **Oracle** (SQL compatible 19c). Quatre projets, une couche chacun :

```
GSB.CR.IHM  ──►  GSB.CR.Metier  ──►  GSB.CR.Donnees  ──►  Oracle (schéma GSB)
     │                 │                   │
     └─────────────────┴───────────────────┴──►  GSB.CR.Modeles
```

| Projet | Rôle | Dépend de |
|---|---|---|
| `GSB.CR.Modeles` | Entités du domaine (`Collaborateur`, `RapportVisite`, `Praticien`…) : des classes de données, sans logique ni dépendance | — |
| `GSB.CR.Donnees` | Accès à Oracle : configuration, connexions, un DAO par domaine et son interface (`IRapportDao`…) | Modeles, ODP.NET, Microsoft.Extensions.Configuration |
| `GSB.CR.Metier` | Règles de gestion, droits, services appelés par les écrans ; ne connaît les DAO que par leurs interfaces | Modeles, Donnees |
| `GSB.CR.IHM` | Écrans, composants visuels, point d'entrée | Modeles, Metier |
| `GSB.CR.Tests` | Tests unitaires (services avec des DAO en mémoire) et d'intégration (Oracle réel) | Modeles, Metier, Donnees |

**Règle d'architecture** : l'IHM n'accède jamais aux DAO ni à Oracle ; elle appelle les services métier. Les diagrammes [de composants](../uml/10_composants.svg) et [de paquetages](../uml/11_paquetages.svg) le montrent.

Réglages communs à tous les projets (`Directory.Build.props`) : `Option Strict On`, `Option Explicit On`, génération de la documentation XML, numéro de version de l'application.

## 2. Bibliothèques et composants utilisés

| Bibliothèque | Version | Projet | Rôle |
|---|---|---|---|
| .NET | 10 | Tous | Plateforme d'exécution |
| Windows Forms | .NET 10 | IHM | Interface graphique |
| Oracle.ManagedDataAccess.Core (ODP.NET) | 23.26.301 | Donnees | Pilote Oracle entièrement managé (aucun client Oracle à installer) |
| Microsoft.Extensions.Configuration.Json | 10.0.12 | Donnees | Lecture de `appsettings.json` et `appsettings.Local.json` |
| Microsoft.Extensions.Configuration.Binder | 10.0.12 | Donnees | Liaison de la section `Oracle` sur `ConfigurationOracle` |
| System.Security.Cryptography (.NET) | .NET 10 | Metier | PBKDF2 (`Rfc2898DeriveBytes.Pbkdf2`), comparaison en temps constant, aléa cryptographique |
| MSTest | 4.0.2 | Tests | Framework de tests unitaires |

Outils : SQLcl (scripts de la base), PlantUML (diagrammes), GitHub Actions (intégration continue), PowerShell 7 (scripts de publication).

## 3. Mettre en place le poste de développement

1. Installer le **SDK .NET 10**, un éditeur (Visual Studio avec la charge de travail « Développement .NET Desktop », ou VS Code), **SQLcl** et Git.
2. Cloner le dépôt.
3. Copier `src/GSB.CR.IHM/appsettings.Local.example.json` en `appsettings.Local.json` et y mettre l'utilisateur et le mot de passe du schéma de développement (fichier ignoré par Git).
4. Installer la base de développement (efface le schéma puis charge le jeu d'essai) :

   ```bash
   echo "" | sql -S GSB/<mdp>@//100.109.217.110:1521/FREEPDB1 @bdd/installer.sql
   ```

5. Compiler, tester, lancer :

   ```bash
   dotnet build GSB.CR.slnx
   dotnet test GSB.CR.slnx --filter "TestCategory!=Integration"   # unitaires, comme la CI
   dotnet test GSB.CR.slnx                                          # tous, base joignable
   dotnet run --project src/GSB.CR.IHM
   ```

Comptes du jeu d'essai : [comptes-test.md](../comptes-test.md) (mot de passe commun `Gsb2026!`).

## 4. Conception par couche

### 4.1 Modèles

Classes simples, une par fichier, qui reflètent les tables (`RapportVisite` ↔ `RAPPORT_VISITE`) ou des vues calculées (`SyntheseActivite`, `LigneStock`, `FichePraticien`). Les colonnes facultatives sont des types nullables (`Date?`, `Integer?`). Aucune règle de gestion ici : elles sont dans la couche Métier et la base.

### 4.2 Données

- **Configuration** : `ConfigurationOracle.Charger()` fusionne `appsettings.json` (serveur, port, service, schéma) et `appsettings.Local.json` (identifiants). Si `Schema` est renseigné, `ConnexionOracle.Ouvrir()` exécute `ALTER SESSION SET CURRENT_SCHEMA` (compte applicatif `GSB_APP`, décision D-21).
- **`DaoOracle`** : classe de base de tous les DAO.
  - `Executer` ouvre une connexion et traduit toute `OracleException` en `AccesDonneesException` (avec le code Oracle ; `EstDoublon` pour ORA-00001).
  - `ExecuterTransaction` valide si tout réussit, sinon annule : aucune écriture partielle.
  - `Lister`, `ExecuterMiseAJour`, `Commande`, `Parametre` et les lecteurs `TexteOuRien`, `DateOuRien`… évitent de répéter le code ADO.NET.
- **Requêtes toujours paramétrées** (`:nom`, `BindByName`) : aucune saisie n'est concaténée dans le SQL.
- **Périmètres** : le module `SqlPerimetre` produit la condition SQL « collaborateurs d'une région, d'un secteur, ou un seul collaborateur » à partir d'un `Perimetre`, et lie son paramètre. Elle est partagée par les DAO d'activité, d'équipe, d'échantillons et de comptes-rendus.
- **Interfaces** (`IRapportDao`…) : la couche Métier ne connaît qu'elles, ce qui permet de la tester avec des DAO en mémoire.

Exemple de méthode de DAO :

```vb
Public Function ListerMotifs() As List(Of Motif) Implements IReferentielDao.ListerMotifs
    Return Lister("Lecture des motifs impossible.",
        "select mot_code, mot_libelle, mot_actif from MOTIF order by mot_ordre",
        Nothing,
        Function(l) New Motif() With {.Code = l.GetString(0), .Libelle = l.GetString(1), .Actif = l.GetString(2) = "O"})
End Function
```

### 4.3 Métier

- **Services** : un par module (`ServiceRapports`, `ServiceActivite`, `ServiceEquipe`, `ServiceEchantillons`, `ServiceMessagerie`, `ServiceConsultation`, `ServiceAuthentification`, `ServiceAdministration`). `FabriqueServices.DepuisConfiguration()` les crée avec leurs DAO Oracle ; l'IHM ne les construit jamais elle-même.
- **Droits** : chaque méthode publique vérifie d'abord le profil de l'utilisateur (`Autorisations.PeutAcceder`, `VerifierAcces`) ; le menu n'est qu'une première barrière.
- **Deux façons de signaler un problème** :
  - une **saisie refusée** renvoie un résultat avec la liste des erreurs (`ResultatEnregistrement`, `ResultatOperation`), que l'écran affiche en rouge ;
  - une **situation anormale** (profil non autorisé, donnée inexistante, base injoignable) lève une `ErreurMetierException` avec un message destiné à l'utilisateur ; les `AccesDonneesException` sont converties en « Le serveur est indisponible… ».
- **Règles isolées et testables** : `ValidateurRapport` (contrôles d'un compte-rendu), `Periodicite` (6 à 8 mois), `PolitiqueMotDePasse`, `HacheurMotDePasse`, `GenerateurMotDePasse`, `Perimetres`, `ExportStatistiques` (contenu CSV d'une synthèse, EX-51), `ServiceAdministration.CodeDosage` (code d'un dosage).
- **Temps** : les services reçoivent un `TimeProvider` (`TimeProvider.System` en production, une horloge fixe dans les tests) ; aucune règle n'appelle `Date.Now` directement.

### 4.4 IHM

- **Point d'entrée** (`Program.Main`) : boucle connexion → changement de mot de passe éventuel → menu, tant que l'utilisateur se déconnecte. Il installe une fois pour toutes le contexte de synchronisation WinForms, pour que la suite de chaque `Await` revienne sur le thread de l'interface.
- **Accès aux données en arrière-plan**, selon le même schéma partout :

  ```vb
  Dim liste = Await Task.Run(Function() _service.RechercherPraticiens(_utilisateur, texte, portefeuille, inactifs))
  If IsDisposed Then Return   ' fenêtre fermée pendant la recherche
  ```

- **Charte** : module `Theme` (couleurs, styles des boutons, grilles, en-têtes) ; ne jamais coder une couleur en dur dans un écran.
- **Composants réutilisables** : `TuileModule` (menu), `PanneauFiche` (fiche détaillée : titres, badges, sections, tableaux), `GraphiqueBarres`, `TuileIndicateur`, `FrmFiche` (fiche en lecture seule), `FrmFormulaire` (fenêtre de saisie générique de l'administration), `RenduSynthese` et `RenduARevoir` (rendus partagés par « Mon activité » et « Ma région »).
- **Écrans** : un formulaire par module (`FrmMesComptesRendus`, `FrmCompteRendu`, `FrmEquipe`…) ; `FrmAdministration` est découpé en classes partielles, une par onglet ; `FrmDetailsMedicament` (composition, interactions, posologie) s'ouvre depuis l'onglet Référentiels. `OutilsEcran` construit les barres de boutons et les grilles des écrans d'administration ; `EnregistrementExport` enregistre un export CSV (boîte « Enregistrer sous », fichier UTF-8 avec BOM).

## 5. Base de données

Scripts dans `bdd/` ; détail dans [modele-donnees.md](../modele-donnees.md).

| Script | Rôle |
|---|---|
| `01_tables.sql`, `02_vues.sql`, `03_triggers.sql` | Structure : 28 tables, 6 vues, 3 déclencheurs |
| `04_referentiels.sql` | Données de base (régions, médicaments, motifs…) |
| `05_jeu_essai.sql` | Données de test (développement) |
| `installer.sql` / `installer_production.sql` | Installation de développement (efface tout) / de production (refuse un schéma existant) |
| `06_compte_applicatif.sql` | Compte `GSB_APP` des postes |
| `tests_regles.sql` | 21 opérations interdites qui doivent être refusées par la base |

Les règles simples sont doublées dans la base (contraintes `CHECK`, index uniques, déclencheurs : date de visite non future, dates de saisie automatiques…), pour que les données restent cohérentes même hors de l'application (D-16). Le serveur de développement est un Oracle 26ai Free, mais le SQL reste compatible 19c : pas de type `BOOLEAN` (colonnes `CHAR(1)` 'O'/'N'), pas de `IF [NOT] EXISTS`, pas de `GROUP BY` sur un alias (D-11).

## 6. Tests

| Type | Où | Principe |
|---|---|---|
| Unitaires | `tests/GSB.CR.Tests/*Tests.vb` | Services et règles testés avec des DAO en mémoire (`Doublures/Faux*Dao.vb`) et une horloge fixe (`HorlogeFixe`) |
| Intégration | Classes marquées `<TestCategory("Integration")>` | DAO et services sur la base de développement ; exclus de la CI |
| Règles de la base | `bdd/tests_regles.sql` | Contraintes et déclencheurs |
| Recette | [cahier-de-recette.md](../cahier-de-recette.md) | Scénarios manuels par exigence |

Convention de nommage : `Methode_Situation_ResultatAttendu` (par exemple `Enregistrer_ValiderIncomplet_RefuseSansRienEcrire`). Les tests d'intégration s'appuient sur le jeu d'essai (réinstaller la base avec `bdd/installer.sql` en cas de doute) ; ceux qui modifient des données communes portent `<DoNotParallelize>`.

L'intégration continue (`.github/workflows/ci.yml`, Windows) compile en Release et lance les tests unitaires à chaque envoi sur `main`.

## 7. Fiches pratiques

### Ajouter un champ à un compte-rendu

1. Base : ajouter la colonne dans `bdd/01_tables.sql` **et** écrire un script de migration `bdd/migrations/<version>_<objet>.sql` (`alter table …`) pour les bases existantes.
2. Modèle : ajouter la propriété à `RapportVisite`.
3. Données : lire et écrire la colonne dans `RapportDao` (`Charger`, `Creer`, `Modifier`).
4. Métier : si le champ a une règle, l'ajouter à `ValidateurRapport` **avec ses tests** dans `ValidateurRapportTests`.
5. IHM : ajouter le contrôle dans `FrmCompteRendu` (concepteur) et le lier dans `LireFormulaire` et le remplissage du formulaire.
6. Mettre à jour les spécifications détaillées, le cahier de recette et le CHANGELOG.

### Ajouter un module

1. Ajouter une valeur à `ModuleApplication`, son titre et sa description dans `LibellesModules`, et les profils autorisés dans `Autorisations.ModulesAccessibles` (avec un test dans `AutorisationsTests`).
2. Créer l'interface et le DAO dans `GSB.CR.Donnees` (hériter de `DaoOracle`), puis le service dans `GSB.CR.Metier` (vérification des droits en tête de chaque méthode) et sa fabrication dans `FabriqueServices`.
3. Écrire les tests du service avec un faux DAO dans `Doublures`.
4. Créer l'écran dans `GSB.CR.IHM` et l'ouvrir depuis `FrmAccueil.OuvrirModule`.
5. Si de nouvelles tables sont créées, relancer `bdd/06_compte_applicatif.sql` après la migration.

### Livrer une version

1. Tous les tests passent (`dotnet test`), la CI est verte.
2. Mettre à jour `<Version>` dans `Directory.Build.props` et ajouter l'entrée du `CHANGELOG.md`.
3. Mettre à jour la documentation (`docs/`) et régénérer la référence des classes : `pwsh ./scripts/generer-reference.ps1`.
4. Vérifier les paquets en local : `pwsh ./scripts/preparer-livraison.ps1` (résultat dans `publication/livraison-<version>/`).
5. Commit, puis tag `vX.Y.Z` et envoi : `git tag vX.Y.Z && git push origin main vX.Y.Z`.
6. Le workflow **Release** (`.github/workflows/release.yml`) vérifie que le tag correspond à la version, compile, exécute les tests, construit les paquets et publie la release GitHub avec les notes tirées du `CHANGELOG.md`.

## 8. Sécurité (mise en œuvre)

| Mesure | Où |
|---|---|
| Hachage PBKDF2-SHA256, 100 000 itérations, sel de 16 octets, comparaison en temps constant | `HacheurMotDePasse` |
| Verrouillage après 5 échecs, journalisation de toutes les tentatives | `ServiceAuthentification`, `CollaborateurDao` |
| Message identique pour login inconnu et mot de passe faux | `ServiceAuthentification` |
| Mots de passe provisoires aléatoires, affichés une fois | `GenerateurMotDePasse`, `ServiceAdministration` |
| Requêtes paramétrées ; nom de schéma contrôlé avant usage | `DaoOracle`, `ConfigurationOracle.InstructionSchema` |
| Droits vérifiés dans chaque service, pas seulement dans le menu | `Autorisations`, `VerifierAcces` |
| Aucun secret dans le dépôt ni dans le paquet ; compte Oracle des postes sans droit sur la structure | `.gitignore`, `scripts/publier.ps1`, `bdd/06_compte_applicatif.sql` |

## 9. Pièges connus

| Piège | Solution retenue |
|---|---|
| SQLcl sous Git Bash plante sans entrée standard, ou transforme les chemins | Lancer avec `echo "" \| MSYS_NO_PATHCONV=1 sql …` |
| Une moyenne Oracle a plus de chiffres que le type `Decimal` de .NET (dépassement à la lecture) | Arrondir dans le SQL : `round(avg(…), 2)` |
| Contrôles créés par code trop petits sur un écran à 150 % | Multiplier leurs dimensions par `DeviceDpi / 96` (fonction `Echelle` de `FrmAdministration`, calcul équivalent dans `GraphiqueBarres`) |
| Propriété publique d'un formulaire sérialisée par le concepteur (avertissement WFO1000) | Attribut `<DesignerSerializationVisibility(Hidden)>` |
| Comparaison de deux `Nullable` en VB (`=` renvoie `Nothing`) | `Nullable.Equals(a, b)` ou `.GetValueOrDefault()` |
| Événements de sélection déclenchés pendant le remplissage d'une grille | Indicateur `_remplissage` qui les ignore |
| Fenêtre fermée pendant un chargement en arrière-plan | Tester `IsDisposed` après chaque `Await` |
