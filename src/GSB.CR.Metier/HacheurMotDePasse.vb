Imports System.Security.Cryptography

''' <summary>
''' Hachage et vérification des mots de passe (PBKDF2-SHA256).
''' Format stocké : <c>PBKDF2-SHA256$&lt;iterations&gt;$&lt;sel base64&gt;$&lt;cle base64&gt;</c>
''' (identique à celui du jeu d'essai, voir docs/modele-donnees.md).
''' </summary>
Public NotInheritable Class HacheurMotDePasse

    Private Const Prefixe As String = "PBKDF2-SHA256"
    Private Const Iterations As Integer = 100_000
    Private Const TailleSel As Integer = 16
    Private Const TailleCle As Integer = 32

    Private Sub New()
    End Sub

    ''' <summary>Hache un mot de passe avec un sel aléatoire.</summary>
    Public Shared Function Hacher(motDePasse As String) As String
        ArgumentNullException.ThrowIfNull(motDePasse)
        Dim sel = RandomNumberGenerator.GetBytes(TailleSel)
        Dim cle = Rfc2898DeriveBytes.Pbkdf2(motDePasse, sel, Iterations, HashAlgorithmName.SHA256, TailleCle)
        Return $"{Prefixe}${Iterations}${Convert.ToBase64String(sel)}${Convert.ToBase64String(cle)}"
    End Function

    ''' <summary>
    ''' Vérifie un mot de passe contre sa valeur hachée. Renvoie Faux si le format est invalide.
    ''' La comparaison se fait en temps constant.
    ''' </summary>
    Public Shared Function Verifier(motDePasse As String, hache As String) As Boolean
        If motDePasse Is Nothing OrElse String.IsNullOrEmpty(hache) Then Return False

        Dim parties = hache.Split("$"c)
        If parties.Length <> 4 OrElse parties(0) <> Prefixe Then Return False

        Dim nbIterations As Integer
        If Not Integer.TryParse(parties(1), nbIterations) OrElse nbIterations <= 0 Then Return False

        Dim sel As Byte(), attendu As Byte()
        Try
            sel = Convert.FromBase64String(parties(2))
            attendu = Convert.FromBase64String(parties(3))
        Catch ex As FormatException
            Return False
        End Try

        Dim calcule = Rfc2898DeriveBytes.Pbkdf2(motDePasse, sel, nbIterations, HashAlgorithmName.SHA256, attendu.Length)
        Return CryptographicOperations.FixedTimeEquals(calcule, attendu)
    End Function

End Class
