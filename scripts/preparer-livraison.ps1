<#
.SYNOPSIS
    Prépare la livraison complète d'une version de GSB-CR (fichiers joints à la release GitHub).

.DESCRIPTION
    Produit dans publication/livraison-<version>/ :
      - GSB-CR-<version>-poste-win-x64.zip : application avec le runtime .NET inclus, installateur
        (Installer.cmd), désinstallateur et notice ; aucun identifiant n'est inclus ;
      - GSB-CR-<version>-base-oracle.zip : scripts d'installation de la base de production,
        script de hachage du mot de passe administrateur, procédure de mise en exploitation ;
      - SHA256SUMS.txt : empreintes des deux archives, pour vérifier le téléchargement ;
      - NOTES.md : notes de version (section du CHANGELOG et instructions d'installation).
    Utilisé en local et par le workflow GitHub Actions « Release » (à la pose d'un tag vX.Y.Z).

.EXAMPLE
    pwsh ./scripts/preparer-livraison.ps1
#>
$ErrorActionPreference = 'Stop'
$racine = Split-Path -Parent $PSScriptRoot
$version = ([xml](Get-Content (Join-Path $racine 'Directory.Build.props'))).Project.PropertyGroup.Version
$sortie = Join-Path $racine "publication/livraison-$version"
$travail = Join-Path $sortie 'travail'

Write-Host "Livraison de GSB-CR $version -> $sortie"
if (Test-Path $sortie) { Remove-Item $sortie -Recurse -Force }
New-Item -ItemType Directory -Path $travail | Out-Null

# --- 1. Paquet du poste client ---
$nomPoste = "GSB-CR-$version-poste-win-x64"
$dossierPoste = Join-Path $travail $nomPoste
$application = Join-Path $dossierPoste 'application'

dotnet publish (Join-Path $racine 'src/GSB.CR.IHM/GSB.CR.IHM.vbproj') `
    --configuration Release --runtime win-x64 --self-contained true `
    --output $application -p:DebugType=none -p:GenerateDocumentationFile=false --nologo
if ($LASTEXITCODE -ne 0) { throw "Échec de la publication (code $LASTEXITCODE)." }

# Jamais d'identifiants : l'installateur crée appsettings.Local.json sur le poste
Remove-Item (Join-Path $application 'appsettings.Local*.json') -ErrorAction SilentlyContinue
Get-ChildItem $application -Filter '*.xml' | Remove-Item
Copy-Item (Join-Path $racine 'deploiement/poste/*') $dossierPoste

# --- 2. Kit de la base Oracle ---
$nomBase = "GSB-CR-$version-base-oracle"
$dossierBase = Join-Path $travail $nomBase
New-Item -ItemType Directory -Path (Join-Path $dossierBase 'bdd') | Out-Null
$scriptsSql = '00_creation_utilisateur.sql', '01_tables.sql', '02_vues.sql', '03_triggers.sql',
              '04_referentiels.sql', '06_compte_applicatif.sql', 'installer_production.sql'
foreach ($f in $scriptsSql) { Copy-Item (Join-Path $racine "bdd/$f") (Join-Path $dossierBase 'bdd') }
Copy-Item (Join-Path $racine 'scripts/hacher-mot-de-passe.ps1') $dossierBase
Copy-Item (Join-Path $racine 'docs/mise-en-exploitation.md') $dossierBase
Copy-Item (Join-Path $racine 'deploiement/base/LISEZMOI.txt') $dossierBase

# Contrôle : aucun fichier d'identifiants ni jeu d'essai dans la livraison
$interdits = Get-ChildItem $travail -Recurse -File |
    Where-Object { $_.Name -like 'appsettings.Local*.json' -or $_.Name -in '05_jeu_essai.sql', 'installer.sql', '99_suppression.sql' }
if ($interdits) { throw "Fichiers interdits dans la livraison : $($interdits.FullName -join ', ')" }

# --- 3. Archives et empreintes ---
foreach ($nom in $nomPoste, $nomBase) {
    Compress-Archive -Path (Join-Path $travail "$nom/*") -DestinationPath (Join-Path $sortie "$nom.zip")
}
$empreintes = Get-ChildItem $sortie -Filter '*.zip' | Sort-Object Name | ForEach-Object {
    "$((Get-FileHash $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant())  $($_.Name)"
}
[System.IO.File]::WriteAllLines((Join-Path $sortie 'SHA256SUMS.txt'), $empreintes)

# --- 4. Notes de version : section du CHANGELOG + installation ---
$changelog = Get-Content (Join-Path $racine 'CHANGELOG.md') -Raw
$motif = "(?ms)^## \[$([regex]::Escape($version))\][^\n]*\n(.*?)(?=^## \[|\z)"
$section = [regex]::Match($changelog, $motif)
if (-not $section.Success) { throw "Section [$version] absente de CHANGELOG.md." }

$notes = @"
## Installation

| Fichier | Pour qui | Contenu |
|---|---|---|
| ``$nomPoste.zip`` | Chaque poste Windows 10/11 (64 bits) | Application prête à l'emploi (runtime .NET inclus) et son installateur |
| ``$nomBase.zip`` | Administrateur de la base | Scripts Oracle de production, hachage du mot de passe administrateur, procédure |
| ``SHA256SUMS.txt`` | — | Empreintes pour vérifier les téléchargements |

**Base de données (une fois)** : décompresser ``$nomBase.zip`` et suivre ``LISEZMOI.txt`` (création du schéma, installation, compte des postes).

**Postes** : décompresser ``$nomPoste.zip``, double-cliquer sur **``Installer.cmd``** et répondre aux questions (serveur, service, mot de passe du compte des postes). L'application est installée dans ``C:\Program Files\GSB-CR`` avec un raccourci sur le bureau. Pour une mise à jour, relancer l'installateur de la nouvelle version : la configuration est conservée.

Documentation : [mise en exploitation](https://github.com/NeuTroNBZh/ProjetAPS3GSB/blob/v$version/docs/mise-en-exploitation.md) · [documentation utilisateur](https://github.com/NeuTroNBZh/ProjetAPS3GSB/blob/v$version/docs/utilisateur/README.md)

## Nouveautés

$($section.Groups[1].Value.Trim())
"@
[System.IO.File]::WriteAllText((Join-Path $sortie 'NOTES.md'), $notes, [System.Text.UTF8Encoding]::new($false))

Remove-Item $travail -Recurse -Force
Get-ChildItem $sortie | ForEach-Object { '{0,-45} {1,8:N1} Mo' -f $_.Name, ($_.Length / 1MB) } | Write-Host
