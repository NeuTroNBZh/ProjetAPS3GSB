<#
.SYNOPSIS
    Hache un mot de passe au format de GSB-CR (PBKDF2-SHA256, 100 000 itérations, sel 16 octets, clé 32 octets).

.DESCRIPTION
    Sert à l'installation de production : le résultat est passé à bdd/installer_production.sql
    pour créer le compte administrateur. Le mot de passe en clair n'est jamais écrit sur le disque.
    Sans paramètre, un mot de passe provisoire aléatoire est généré et affiché une seule fois.

.PARAMETER Saisir
    Demande le mot de passe au clavier (saisie masquée) au lieu d'en générer un.

.EXAMPLE
    ./scripts/hacher-mot-de-passe.ps1
    ./scripts/hacher-mot-de-passe.ps1 -Saisir
#>
param([switch]$Saisir)

$ErrorActionPreference = 'Stop'

function Nouveau-MotDePasse {
    # 12 caractères sans caractères ambigus, avec au moins une majuscule, une minuscule, un chiffre et un caractère spécial
    $groupes = 'ABCDEFGHJKLMNPQRSTUVWXYZ', 'abcdefghijkmnpqrstuvwxyz', '23456789', '!#%*+-=?@'
    $tous = -join $groupes
    $car = [System.Collections.Generic.List[char]]::new()
    foreach ($g in $groupes) { $car.Add($g[[System.Security.Cryptography.RandomNumberGenerator]::GetInt32($g.Length)]) }
    while ($car.Count -lt 12) { $car.Add($tous[[System.Security.Cryptography.RandomNumberGenerator]::GetInt32($tous.Length)]) }
    # mélange (Fisher-Yates)
    for ($i = $car.Count - 1; $i -gt 0; $i--) {
        $j = [System.Security.Cryptography.RandomNumberGenerator]::GetInt32($i + 1)
        $tmp = $car[$i]; $car[$i] = $car[$j]; $car[$j] = $tmp
    }
    -join $car
}

if ($Saisir) {
    $securise = Read-Host 'Mot de passe provisoire' -AsSecureString
    $clair = [System.Net.NetworkCredential]::new('', $securise).Password
} else {
    $clair = Nouveau-MotDePasse
}

$sel = [byte[]]::new(16)
[System.Security.Cryptography.RandomNumberGenerator]::Fill($sel)
$derive = [System.Security.Cryptography.Rfc2898DeriveBytes]::new(
    $clair, $sel, 100000, [System.Security.Cryptography.HashAlgorithmName]::SHA256)
$cle = $derive.GetBytes(32)
$derive.Dispose()
$hache = 'PBKDF2-SHA256$100000$' + [Convert]::ToBase64String($sel) + '$' + [Convert]::ToBase64String($cle)

if (-not $Saisir) {
    Write-Host ''
    Write-Host "Mot de passe provisoire de l'administrateur : $clair" -ForegroundColor Yellow
    Write-Host "Notez-le maintenant : il ne sera plus affiché. Il devra être changé à la première connexion."
}
Write-Host ''
Write-Host 'Valeur à passer à bdd/installer_production.sql :'
Write-Output $hache
