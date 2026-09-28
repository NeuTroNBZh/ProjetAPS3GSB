<#
.SYNOPSIS
    Produit le dossier de projet en PDF (docs/dossier/GSB-CR-dossier-de-projet.pdf) à partir de la documentation Markdown.

.DESCRIPTION
    Assemble dans l'ordre du dossier : page de garde, sommaire, puis chaque document de docs/ (une partie par document,
    chacune sur une nouvelle page). Les liens entre documents deviennent des renvois internes, les autres liens pointent
    vers le dépôt GitHub, les diagrammes UML sont insérés en SVG (vectoriel) sur des pages paysage, et les schémas Mermaid
    sont dessinés à l'impression. L'impression PDF est faite par Microsoft Edge (ou Google Chrome) sans interface.
    À relancer après toute modification de la documentation.

.EXAMPLE
    pwsh ./scripts/generer-dossier.ps1
#>
$ErrorActionPreference = 'Stop'
$racine = Split-Path -Parent $PSScriptRoot
$version = ([xml](Get-Content (Join-Path $racine 'Directory.Build.props'))).Project.PropertyGroup.Version
$depot = 'https://github.com/NeuTroNBZh/ProjetAPS3GSB'
$travail = Join-Path $racine 'publication/dossier'
$sortie = Join-Path $racine 'docs/dossier/GSB-CR-dossier-de-projet.pdf'

# Parties du dossier : (titre dans le sommaire, fichier, annexe ?)
$parties = @(
    @('Présentation du projet', 'README.md', $false),
    @('Contexte métier', 'docs/contexte.md', $false),
    @('Exigences', 'docs/exigences.md', $false),
    @('Spécifications fonctionnelles générales', 'docs/specifications-generales.md', $false),
    @('Spécifications fonctionnelles détaillées', 'docs/specifications-detaillees.md', $false),
    @('Maquettes et charte graphique', 'docs/maquettes/README.md', $false),
    @('Modèle de données', 'docs/modele-donnees.md', $false),
    @('Conception UML 2', 'docs/uml/README.md', $false),
    @('Décisions', 'docs/decisions.md', $false),
    @('Documentation technique', 'docs/technique/README.md', $false),
    @('Cahier de recette', 'docs/cahier-de-recette.md', $false),
    @('Gestion de projet', 'docs/gestion-de-projet.md', $false),
    @('Mise en exploitation', 'docs/mise-en-exploitation.md', $false),
    @('Documentation utilisateur', 'docs/utilisateur/README.md', $false),
    @('Démarrer', 'docs/utilisateur/01-demarrer.md', $false),
    @('Module Visiteur', 'docs/utilisateur/02-visiteur.md', $false),
    @('Module Délégué régional', 'docs/utilisateur/03-delegue.md', $false),
    @('Module Responsable de secteur', 'docs/utilisateur/04-responsable.md', $false),
    @('Messagerie', 'docs/utilisateur/05-messagerie.md', $false),
    @('Module Administration', 'docs/utilisateur/06-administration.md', $false),
    @('Questions fréquentes', 'docs/utilisateur/07-questions-frequentes.md', $false),
    @('Glossaire', 'docs/glossaire.md', $false),
    @('Comptes de test', 'docs/comptes-test.md', $true),
    @('Journal des versions', 'CHANGELOG.md', $true),
    @('Référence des classes', 'docs/technique/reference-classes.md', $true)
)

function Uri-Fichier([string]$chemin) { ([Uri](Resolve-Path $chemin).Path).AbsoluteUri }

# Fichier .md (chemin normalisé depuis la racine) -> identifiant de sa partie
$ancres = @{}
for ($i = 0; $i -lt $parties.Count; $i++) { $ancres[$parties[$i][1]] = "partie-$($i + 1)" }

function Convertir-Partie([int]$numero, [string]$fichier) {
    $chemin = Join-Path $racine $fichier
    $dossier = Split-Path -Parent $chemin
    $html = (ConvertFrom-Markdown -InputObject (Get-Content $chemin -Raw)).Html
    $prefixe = "p$numero-"

    # Identifiants et renvois internes propres à la partie (évite les doublons entre documents)
    $html = [regex]::Replace($html, ' id="([^"]+)"', { param($m) " id=""$prefixe$($m.Groups[1].Value)""" })
    $html = [regex]::Replace($html, 'href="#([^"]+)"', { param($m) "href=""#$prefixe$($m.Groups[1].Value)""" })

    # Images locales : chemin absolu
    $html = [regex]::Replace($html, 'src="(?!https?:|data:)([^"]+)"', {
        param($m)
        $cible = Join-Path $dossier ([Uri]::UnescapeDataString($m.Groups[1].Value))
        if (Test-Path $cible) { "src=""$(Uri-Fichier $cible)""" } else { $m.Value }
    })

    # Liens relatifs : vers une partie du dossier si possible, sinon vers le dépôt GitHub
    $html = [regex]::Replace($html, 'href="(?!https?:|#|mailto:)([^"#]+)(#[^"]*)?"', {
        param($m)
        $cible = [IO.Path]::GetFullPath((Join-Path $dossier ([Uri]::UnescapeDataString($m.Groups[1].Value))))
        $relatif = [IO.Path]::GetRelativePath($racine, $cible).Replace('\', '/')
        if ($ancres.ContainsKey($relatif)) { "href=""#$($ancres[$relatif])""" }
        else { "href=""$depot/blob/main/$relatif""" }
    })

    # Schémas Mermaid : dessinés par mermaid.js à l'impression
    $html = [regex]::Replace($html, '<pre><code class="language-mermaid">([\s\S]*?)</code></pre>', {
        param($m) "<div class=""mermaid"">$($m.Groups[1].Value)</div>"
    })
    return $html
}

# --- Diagrammes UML : une page paysage par diagramme, titre repris du tableau du README
function Figures-Uml() {
    $readme = Get-Content (Join-Path $racine 'docs/uml/README.md') -Raw
    $figures = foreach ($m in [regex]::Matches($readme, '\|\s*(\d+)\s*\|\s*\[([^\]]+)\]\(([^)]+\.svg)\)\s*\|\s*([^|]+)\|\s*([^|]+)\|')) {
        $svg = Join-Path $racine "docs/uml/$($m.Groups[3].Value)"
        "<figure class=""uml""><figcaption>Diagramme $($m.Groups[1].Value) — $($m.Groups[2].Value) <span>($($m.Groups[4].Value.Trim()) · $($m.Groups[5].Value.Trim()))</span></figcaption><img src=""$(Uri-Fichier $svg)"" alt=""$($m.Groups[2].Value)""></figure>"
    }
    return -join $figures
}

$date = (Get-Date).ToString('d MMMM yyyy', [Globalization.CultureInfo]::GetCultureInfo('fr-FR'))
$sommaire = for ($i = 0; $i -lt $parties.Count; $i++) {
    $classe = if ($parties[$i][2]) { ' class="annexe"' } else { '' }
    $prefixeAnnexe = if ($parties[$i][2]) { 'Annexe — ' } else { '' }
    "<li$classe><a href=""#partie-$($i + 1)"">$prefixeAnnexe$($parties[$i][0])</a></li>"
}
$corps = for ($i = 0; $i -lt $parties.Count; $i++) {
    $contenu = Convertir-Partie ($i + 1) $parties[$i][1]
    if ($parties[$i][1] -eq 'docs/uml/README.md') { $contenu += Figures-Uml }
    "<section class=""partie"" id=""partie-$($i + 1)""><p class=""fil"">$($i + 1). $($parties[$i][0])</p>$contenu</section>"
}

$page = @"
<!doctype html>
<html lang="fr">
<head>
<meta charset="utf-8">
<title>GSB-CR — Dossier de projet</title>
<style>
  @page { size: A4; margin: 18mm 16mm 20mm 16mm;
          @bottom-right { content: "GSB-CR · dossier de projet · " counter(page); font: 8.5pt 'Segoe UI', sans-serif; color: #5A626E; } }
  @page couverture { margin: 0; @bottom-right { content: none; } }
  @page paysage { size: A4 landscape; margin: 12mm 12mm 16mm 12mm; }
  body { font: 10pt/1.45 'Segoe UI', Arial, sans-serif; color: #1B2B40; }
  h1 { font: 600 22pt 'Segoe UI Semibold', 'Segoe UI', sans-serif; color: #00529B; border-bottom: 3px solid #00529B; padding-bottom: 4pt; margin-top: 0; }
  h2 { font: 600 15pt 'Segoe UI Semibold', 'Segoe UI', sans-serif; color: #00386C; margin-top: 18pt; break-after: avoid; }
  h3 { font: 600 12pt 'Segoe UI Semibold', 'Segoe UI', sans-serif; color: #00529B; break-after: avoid; }
  h4 { font-size: 10.5pt; color: #00386C; break-after: avoid; }
  a { color: #00529B; text-decoration: none; }
  table { border-collapse: collapse; width: 100%; margin: 6pt 0 10pt; font-size: 8.5pt; }
  th { background: #00529B; color: #fff; text-align: left; padding: 3pt 5pt; }
  td { border-bottom: 1px solid #CEE1F5; padding: 3pt 5pt; vertical-align: top; }
  tr:nth-child(even) td { background: #F3F8FD; }
  tr, img, figure, pre, .mermaid { break-inside: avoid; }
  code { font: 8.5pt Consolas, monospace; background: #EEF3F8; padding: 0 2pt; border-radius: 2pt; }
  pre { background: #EEF3F8; padding: 6pt 8pt; border-radius: 4pt; white-space: pre-wrap; }
  pre code { background: none; padding: 0; }
  img { max-width: 100%; max-height: 200mm; }
  blockquote { border-left: 3px solid #00529B; margin: 6pt 0; padding: 2pt 10pt; color: #3D4A5C; background: #F3F8FD; }
  .mermaid { text-align: center; margin: 8pt 0; }
  .partie { break-before: page; }
  .fil { font-size: 8pt; text-transform: uppercase; letter-spacing: 1pt; color: #5A626E; margin: 0 0 4pt; }
  figure.uml { page: paysage; break-before: page; margin: 0; text-align: center; }
  figure.uml img { max-height: 165mm; max-width: 100%; }
  figcaption { font: 600 12pt 'Segoe UI Semibold', 'Segoe UI', sans-serif; color: #00529B; text-align: left; margin-bottom: 6pt; }
  figcaption span { font: 9pt 'Segoe UI', sans-serif; color: #5A626E; }
  .couverture { page: couverture; height: 297mm; background: #0F2238; color: #F7F9FC; padding: 40mm 22mm; box-sizing: border-box; position: relative; }
  .couverture .sur { color: #8FC1EE; font-size: 11pt; letter-spacing: 1.5pt; text-transform: uppercase; }
  .couverture h1 { color: #F7F9FC; border: none; font-size: 54pt; margin: 14mm 0 4mm; }
  .couverture .sous { font-size: 18pt; color: #DCE7F3; max-width: 150mm; }
  .couverture .infos { position: absolute; left: 22mm; bottom: 30mm; font-size: 11pt; color: #BFD3E8; line-height: 1.8; }
  .couverture .bande { position: absolute; left: 0; right: 0; top: 0; height: 10mm; background: #00529B; }
  .sommaire { break-before: page; }
  .sommaire ol { columns: 1; font-size: 11pt; line-height: 1.9; }
  .sommaire li.annexe { color: #5A626E; }
</style>
<script src="https://cdn.jsdelivr.net/npm/mermaid@11.17.2/dist/mermaid.min.js" integrity="sha384-EOXBFmc3gx5mb+vn0vPvvGqACToJD24hhacX5Yx+8NUUQrHIle/Qi5Bg9o3zKwW2" crossorigin="anonymous"></script>
</head>
<body>
<div class="couverture">
  <div class="bande"></div>
  <p class="sur">BTS SIO option SLAM · Atelier de professionnalisation</p>
  <h1>GSB-CR</h1>
  <p class="sous">Dossier de projet : application de saisie et de suivi des comptes-rendus de visite des visiteurs médicaux de Galaxy Swiss Bourdin</p>
  <div class="infos">
    Réalisé par Louis<br>
    Version de l'application : $version · dossier généré le $date<br>
    Code source et releases : $depot
  </div>
</div>
<div class="sommaire">
  <h1>Sommaire</h1>
  <ol>$(-join $sommaire)</ol>
</div>
$(-join $corps)
<script>mermaid.initialize({ startOnLoad: true, theme: 'neutral', securityLevel: 'strict' });</script>
</body>
</html>
"@

New-Item -ItemType Directory -Force $travail, (Split-Path $sortie) | Out-Null
$fichierHtml = Join-Path $travail 'dossier.html'
[IO.File]::WriteAllText($fichierHtml, $page, [Text.UTF8Encoding]::new($false))

$navigateur = @("${env:ProgramFiles(x86)}\Microsoft\Edge\Application\msedge.exe",
                "$env:ProgramFiles\Microsoft\Edge\Application\msedge.exe",
                "$env:ProgramFiles\Google\Chrome\Application\chrome.exe") | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $navigateur) { throw "Microsoft Edge ou Google Chrome est nécessaire pour produire le PDF." }

$profil = Join-Path $travail 'profil-navigateur'
& $navigateur --headless=new --disable-gpu --no-first-run --user-data-dir="$profil" --allow-file-access-from-files `
    --no-pdf-header-footer --virtual-time-budget=30000 --print-to-pdf="$sortie" (Uri-Fichier $fichierHtml) 2>$null | Out-Null
if (-not (Test-Path $sortie)) { throw "Le PDF n'a pas été produit." }
Write-Host "Dossier de projet : $sortie ($([math]::Round((Get-Item $sortie).Length / 1MB, 1)) Mo)"
