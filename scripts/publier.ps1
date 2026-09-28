<#
.SYNOPSIS
    Produit le paquet d'installation du poste client GSB-CR (dossier + archive zip).

.DESCRIPTION
    Compile l'application en Release pour Windows 64 bits et prépare un dossier prêt à copier
    sur les postes : exécutable, bibliothèques, appsettings.json et le modèle appsettings.Local.example.json.
    Les identifiants de développement (appsettings.Local.json) ne sont JAMAIS inclus.

.PARAMETER Autonome
    Inclut le runtime .NET dans le paquet (plus volumineux, mais aucun prérequis sur le poste).
    Sans ce paramètre, le poste doit avoir le « .NET Desktop Runtime 10 » installé.

.PARAMETER Sortie
    Dossier de destination (par défaut : publication).

.EXAMPLE
    ./scripts/publier.ps1
    ./scripts/publier.ps1 -Autonome
#>
param(
    [switch]$Autonome,
    [string]$Sortie = 'publication'
)

$ErrorActionPreference = 'Stop'
$racine = Split-Path -Parent $PSScriptRoot
$version = ([xml](Get-Content (Join-Path $racine 'Directory.Build.props'))).Project.PropertyGroup.Version
$suffixe = if ($Autonome) { 'autonome' } else { 'runtime' }
$nom = "GSB-CR-$version-win-x64-$suffixe"
$dossier = Join-Path $racine (Join-Path $Sortie $nom)

Write-Host "Publication de GSB-CR $version ($suffixe) vers $dossier"
if (Test-Path $dossier) { Remove-Item $dossier -Recurse -Force }

dotnet publish (Join-Path $racine 'src/GSB.CR.IHM/GSB.CR.IHM.vbproj') `
    --configuration Release --runtime win-x64 --self-contained:$($Autonome.IsPresent) `
    --output $dossier -p:DebugType=none -p:GenerateDocumentationFile=false --nologo
if ($LASTEXITCODE -ne 0) { throw "Échec de la publication (code $LASTEXITCODE)." }

# Jamais d'identifiants dans le paquet : on retire le fichier local et on fournit le modèle
Remove-Item (Join-Path $dossier 'appsettings.Local.json') -ErrorAction SilentlyContinue
Copy-Item (Join-Path $racine 'src/GSB.CR.IHM/appsettings.Local.example.json') $dossier
Get-ChildItem $dossier -Filter '*.xml' | Remove-Item

$archive = "$dossier.zip"
if (Test-Path $archive) { Remove-Item $archive }
Compress-Archive -Path (Join-Path $dossier '*') -DestinationPath $archive

$taille = [math]::Round((Get-Item $archive).Length / 1MB, 1)
Write-Host "Paquet prêt : $archive ($taille Mo)"
Write-Host "Sur chaque poste : décompresser, compléter appsettings.json (serveur, schéma) et créer appsettings.Local.json à partir du modèle."
