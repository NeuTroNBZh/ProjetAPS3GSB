<#
.SYNOPSIS
    Installe (ou met à jour) GSB-CR sur un poste Windows.

.DESCRIPTION
    Lancé par « Installer.cmd » avec les droits administrateur :
      1. copie l'application dans C:\Program Files\GSB-CR ;
      2. demande les paramètres de connexion Oracle et écrit appsettings.json et appsettings.Local.json ;
      3. limite la lecture du fichier d'identifiants aux utilisateurs du poste ;
      4. crée les raccourcis du menu Démarrer et du bureau.
    Lors d'une mise à jour, la configuration existante peut être conservée.
    Compatible Windows PowerShell 5.1 (présent sur tous les postes Windows 10 et 11).

    Installation sans questions (déploiement sur de nombreux postes) : passer -Silencieux avec
    -Serveur, -Service et -MotDePasse ; les autres paramètres ont une valeur par défaut.
    Avec -Silencieux et une configuration déjà présente, celle-ci est conservée sauf si -Serveur est fourni.

.EXAMPLE
    powershell -ExecutionPolicy Bypass -File installer.ps1 -Silencieux -Serveur srv-oracle -Service GSBPDB `
        -MotDePasse (Read-Host -AsSecureString)
#>
param(
    [string]$Destination = (Join-Path $env:ProgramFiles 'GSB-CR'),
    [string]$Serveur,
    [int]$Port = 0,
    [string]$Service,
    [string]$Schema = 'GSB',
    [string]$Utilisateur = 'GSB_APP',
    [securestring]$MotDePasse,
    [switch]$Silencieux,
    [switch]$SansRaccourcis
)

$ErrorActionPreference = 'Stop'
$source = Join-Path $PSScriptRoot 'application'
$exe = 'GSB.CR.IHM.exe'

function Demander([string]$libelle, [string]$defaut) {
    $saisie = Read-Host "$libelle [$defaut]"
    if ([string]::IsNullOrWhiteSpace($saisie)) { return $defaut }
    return $saisie.Trim()
}

function EcrireJson([string]$chemin, $objet) {
    $json = $objet | ConvertTo-Json -Depth 3
    [System.IO.File]::WriteAllText($chemin, $json, (New-Object System.Text.UTF8Encoding($false)))
}

Write-Host ''
Write-Host '=== Installation de GSB-CR ===' -ForegroundColor Cyan
Write-Host "Dossier d'installation : $Destination"
Write-Host ''

if (-not (Test-Path (Join-Path $source $exe))) {
    throw "Dossier « application » introuvable à côté de ce script : décompressez toute l'archive avant de lancer l'installation."
}
if (Get-Process -Name 'GSB.CR.IHM' -ErrorAction SilentlyContinue) {
    throw "GSB-CR est ouvert sur ce poste : fermez l'application puis relancez l'installation."
}

# --- Configuration : reprise de l'existante ou saisie ---
$fichierPublic = Join-Path $Destination 'appsettings.json'
$fichierLocal = Join-Path $Destination 'appsettings.Local.json'
$conserver = $false
if ((Test-Path $fichierPublic) -and (Test-Path $fichierLocal)) {
    if ($Silencieux) { $conserver = [string]::IsNullOrWhiteSpace($Serveur) }
    else {
        $reponse = Read-Host 'Une configuration existe déjà sur ce poste. La conserver ? (O/n)'
        $conserver = ($reponse -eq '' -or $reponse -match '^[oOyY]')
    }
}

if ($conserver) {
    $configPublique = Get-Content $fichierPublic -Raw
    $configLocale = Get-Content $fichierLocal -Raw
} else {
    $modele = (Get-Content (Join-Path $source 'appsettings.json') -Raw | ConvertFrom-Json).Oracle
    if ($Silencieux) {
        if ([string]::IsNullOrWhiteSpace($Serveur) -or [string]::IsNullOrWhiteSpace($Service) -or $null -eq $MotDePasse) {
            throw 'Mode silencieux : -Serveur, -Service et -MotDePasse sont obligatoires.'
        }
        $hote = $Serveur; $service = $Service; $schemaTables = $Schema; $compte = $Utilisateur
        $numeroPort = if ($Port -gt 0) { $Port } else { 1521 }
    } else {
        Write-Host 'Paramètres de connexion à la base Oracle (Entrée = valeur proposée) :'
        $hote = Demander '  Serveur Oracle (nom ou adresse IP)' $(if ($Serveur) { $Serveur } else { $modele.Hote })
        $numeroPort = [int](Demander '  Port' $(if ($Port -gt 0) { [string]$Port } else { [string]$modele.Port }))
        $service = Demander '  Nom du service' $(if ($Service) { $Service } else { $modele.Service })
        $schemaTables = Demander '  Schéma des tables' $Schema
        $compte = Demander '  Compte Oracle des postes' $Utilisateur
        if ($null -eq $MotDePasse) { $MotDePasse = Read-Host '  Mot de passe de ce compte' -AsSecureString }
    }
    $ptr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($MotDePasse)
    try { $mdp = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($ptr) }
    finally { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($ptr) }
    if ([string]::IsNullOrEmpty($mdp)) { throw 'Le mot de passe ne peut pas être vide.' }

    $configPublique = @{ Oracle = [ordered]@{ Hote = $hote; Port = $numeroPort; Service = $service; Schema = $schemaTables } }
    $configLocale = @{ Oracle = [ordered]@{ Utilisateur = $compte; MotDePasse = $mdp } }
}

# --- Copie des fichiers (l'ancienne version est remplacée entièrement) ---
Write-Host ''
Write-Host "Copie de l'application..."
# Le fichier d'identifiants est protégé en écriture : on rétablit ses droits hérités avant de le remplacer
if (Test-Path $fichierLocal) { & icacls $fichierLocal /reset | Out-Null }
if (Test-Path $Destination) { Remove-Item (Join-Path $Destination '*') -Recurse -Force }
else { New-Item -ItemType Directory -Path $Destination | Out-Null }
Copy-Item (Join-Path $source '*') $Destination -Recurse -Force

# --- Écriture de la configuration ---
if ($conserver) {
    [System.IO.File]::WriteAllText($fichierPublic, $configPublique, (New-Object System.Text.UTF8Encoding($false)))
    [System.IO.File]::WriteAllText($fichierLocal, $configLocale, (New-Object System.Text.UTF8Encoding($false)))
} else {
    EcrireJson $fichierPublic $configPublique
    EcrireJson $fichierLocal $configLocale
    $mdp = $null
}

# Identifiants : lisibles par les utilisateurs du poste, modifiables par les seuls administrateurs
# (SID universels : S-1-5-32-544 = Administrateurs, S-1-5-32-545 = Utilisateurs, S-1-5-18 = Système)
& icacls $fichierLocal /inheritance:r /grant:r '*S-1-5-32-544:F' '*S-1-5-18:F' '*S-1-5-32-545:R' | Out-Null

# --- Raccourcis ---
if (-not $SansRaccourcis) {
    $shell = New-Object -ComObject WScript.Shell
    foreach ($dossier in 'CommonPrograms', 'CommonDesktopDirectory') {
        $raccourci = $shell.CreateShortcut((Join-Path ([Environment]::GetFolderPath($dossier)) 'GSB-CR.lnk'))
        $raccourci.TargetPath = Join-Path $Destination $exe
        $raccourci.WorkingDirectory = $Destination
        $raccourci.Description = 'GSB-CR - Comptes-rendus de visite'
        $raccourci.Save()
    }
}

$version = (Get-Item (Join-Path $Destination $exe)).VersionInfo.ProductVersion -replace '\+.*$', ''
Write-Host ''
Write-Host "GSB-CR $version est installé." -ForegroundColor Green
if (-not $SansRaccourcis) { Write-Host 'Raccourcis « GSB-CR » créés dans le menu Démarrer et sur le bureau.' }
Write-Host 'Pour modifier la connexion plus tard : relancer Installer.cmd et répondre « n » à la question sur la configuration.'
