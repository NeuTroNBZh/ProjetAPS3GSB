<#
.SYNOPSIS
    Génère la référence des classes (docs/technique/reference-classes.md) à partir des commentaires
    de documentation XML (''') produits par la compilation.

.DESCRIPTION
    Compile la solution en Release, lit GSB.CR.<Projet>.xml pour chaque projet et écrit un fichier
    Markdown : pour chaque type, sa description puis celle de ses membres documentés.
    À relancer après toute modification du code pour garder la documentation à jour.

.EXAMPLE
    pwsh ./scripts/generer-reference.ps1
#>
$ErrorActionPreference = 'Stop'
$racine = Split-Path -Parent $PSScriptRoot
$sortie = Join-Path $racine 'docs/technique/reference-classes.md'

dotnet build (Join-Path $racine 'GSB.CR.slnx') --configuration Release --nologo -v q | Out-Null
if ($LASTEXITCODE -ne 0) { throw "Échec de la compilation." }

$projets = [ordered]@{
    'Modeles' = 'Entités du domaine, sans dépendance (utilisées par toutes les couches).'
    'Donnees' = 'Accès à Oracle : configuration, connexions, DAO et leurs interfaces.'
    'Metier'  = 'Règles de gestion, droits et services appelés par les écrans.'
    'IHM'     = 'Écrans WinForms, composants visuels et point d''entrée.'
}

function Texte($noeud) {
    if ($null -eq $noeud) { return '' }
    # PowerShell renvoie une simple chaîne pour un élément qui ne contient que du texte
    if ($noeud -is [string]) { return (($noeud -replace '\s+', ' ').Trim() -replace '\|', '\|') }
    $morceaux = foreach ($n in $noeud.ChildNodes) {
        switch ($n.NodeType) {
            'Text' { $n.Value }
            'Element' {
                switch ($n.Name) {
                    'see'      { '`' + (($n.GetAttribute('cref') -replace '^\w:', '') -split '\.')[-1] + '`' }
                    'paramref' { '`' + $n.GetAttribute('name') + '`' }
                    'c'        { '`' + $n.InnerText + '`' }
                    default    { Texte $n }
                }
            }
        }
    }
    ((-join $morceaux) -replace '\s+', ' ').Trim() -replace '\|', '\|'
}

function NomCourt([string]$nom) {
    # « M:GSB.CR.Metier.ServiceRapports.Enregistrer(GSB.CR.Modeles.RapportVisite,...) » -> « Enregistrer(RapportVisite, …) »
    $sansPrefixe = $nom.Substring(2)
    $parenthese = $sansPrefixe.IndexOf('(')
    $chemin = if ($parenthese -ge 0) { $sansPrefixe.Substring(0, $parenthese) } else { $sansPrefixe }
    $court = ($chemin -split '\.')[-1] -replace '^#ctor$', 'New'
    if ($parenthese -ge 0) {
        $params = $sansPrefixe.Substring($parenthese + 1).TrimEnd(')') -split ',' |
            ForEach-Object { (($_ -replace '\{.*\}', '') -split '\.')[-1] -replace '@$', '' }
        $court += '(' + ($params -join ', ') + ')'
    } elseif ($nom.StartsWith('M:')) { $court += '()' }
    $court
}

$lignes = [System.Collections.Generic.List[string]]::new()
$lignes.Add('# Référence des classes — GSB-CR')
$lignes.Add('')
$lignes.Add("> Fichier généré par ``scripts/generer-reference.ps1`` à partir des commentaires de documentation du code. Ne pas modifier à la main.")
$lignes.Add('')
$lignes.Add('Voir [le guide technique](README.md) pour l''architecture et les choix de conception.')
$lignes.Add('')

foreach ($projet in $projets.Keys) {
    $fichier = Get-ChildItem (Join-Path $racine "src/GSB.CR.$projet/bin/Release") -Recurse -Filter "GSB.CR.$projet.xml" | Select-Object -First 1
    [xml]$xml = Get-Content $fichier.FullName -Raw
    $membres = $xml.doc.members.member
    $types = $membres | Where-Object { $_.name.StartsWith('T:') } | Sort-Object { ($_.name -split '\.')[-1] }

    $lignes.Add("## GSB.CR.$projet")
    $lignes.Add('')
    $lignes.Add($projets[$projet] + " $($types.Count) types documentés.")
    $lignes.Add('')
    $lignes.Add('| Type | Rôle |')
    $lignes.Add('|---|---|')
    foreach ($t in $types) {
        $nom = ($t.name -split '\.')[-1]
        $lignes.Add("| [$nom](#$($nom.ToLower())) | $(Texte $t.summary) |")
    }
    $lignes.Add('')

    foreach ($t in $types) {
        $nomComplet = $t.name.Substring(2)
        $nom = ($nomComplet -split '\.')[-1]
        $lignes.Add("### $nom")
        $lignes.Add('')
        $lignes.Add((Texte $t.summary))
        $lignes.Add('')
        $siens = $membres | Where-Object {
            -not $_.name.StartsWith('T:') -and
            ($_.name.Substring(2) -replace '\(.*$', '') -match ('^' + [regex]::Escape($nomComplet) + '\.[^.]+$')
        }
        if ($siens) {
            $lignes.Add('| Membre | Description |')
            $lignes.Add('|---|---|')
            foreach ($m in $siens) {
                $desc = Texte $m.summary
                $retour = Texte $m.returns
                if ($retour) { $desc += " Renvoie : $retour" }
                $lignes.Add("| ``$(NomCourt $m.name)`` | $desc |")
            }
            $lignes.Add('')
        }
    }
}

New-Item -ItemType Directory -Force (Split-Path $sortie) | Out-Null
[System.IO.File]::WriteAllLines($sortie, $lignes, [System.Text.UTF8Encoding]::new($false))
Write-Host "Référence générée : $sortie ($($lignes.Count) lignes)"
