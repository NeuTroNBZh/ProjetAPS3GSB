<#
.SYNOPSIS
    Désinstalle GSB-CR du poste : dossier de l'application, configuration et raccourcis.
    Les données restent dans la base Oracle : rien n'est supprimé côté serveur.
#>
param(
    [string]$Destination = (Join-Path $env:ProgramFiles 'GSB-CR')
)

$ErrorActionPreference = 'Stop'

Write-Host ''
Write-Host '=== Désinstallation de GSB-CR ===' -ForegroundColor Cyan
if (Get-Process -Name 'GSB.CR.IHM' -ErrorAction SilentlyContinue) {
    throw "GSB-CR est ouvert sur ce poste : fermez l'application puis relancez la désinstallation."
}
$reponse = Read-Host "Supprimer GSB-CR et sa configuration de ce poste ($Destination) ? (o/N)"
if ($reponse -notmatch '^[oOyY]') { Write-Host 'Désinstallation annulée.'; return }

foreach ($lien in @(
        (Join-Path ([Environment]::GetFolderPath('CommonPrograms')) 'GSB-CR.lnk'),
        (Join-Path ([Environment]::GetFolderPath('CommonDesktopDirectory')) 'GSB-CR.lnk'))) {
    if (Test-Path $lien) { Remove-Item $lien -Force }
}
$identifiants = Join-Path $Destination 'appsettings.Local.json'
if (Test-Path $identifiants) { & icacls $identifiants /reset | Out-Null }   # fichier protégé en écriture
if (Test-Path $Destination) { Remove-Item $Destination -Recurse -Force }

Write-Host 'GSB-CR a été désinstallé. Les données de la base Oracle ne sont pas touchées.' -ForegroundColor Green
