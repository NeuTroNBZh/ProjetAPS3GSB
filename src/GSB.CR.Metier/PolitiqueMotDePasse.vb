''' <summary>
''' Règles de robustesse d'un nouveau mot de passe (EX-07).
''' </summary>
Public NotInheritable Class PolitiqueMotDePasse

    ''' <summary>Longueur minimale d'un mot de passe.</summary>
    Public Const LongueurMinimale As Integer = 8

    Private Sub New()
    End Sub

    ''' <summary>Texte des règles, à afficher à l'utilisateur.</summary>
    Public Shared ReadOnly Property Description As String =
        $"Au moins {LongueurMinimale} caractères, dont une majuscule, une minuscule, un chiffre et un caractère spécial."

    ''' <summary>Renvoie la liste des règles non respectées (vide si le mot de passe est valide).</summary>
    Public Shared Function Verifier(motDePasse As String) As IReadOnlyList(Of String)
        Dim erreurs As New List(Of String)
        Dim mdp = If(motDePasse, "")

        If mdp.Length < LongueurMinimale Then erreurs.Add($"Le mot de passe doit contenir au moins {LongueurMinimale} caractères.")
        If Not mdp.Any(AddressOf Char.IsUpper) Then erreurs.Add("Le mot de passe doit contenir au moins une majuscule.")
        If Not mdp.Any(AddressOf Char.IsLower) Then erreurs.Add("Le mot de passe doit contenir au moins une minuscule.")
        If Not mdp.Any(AddressOf Char.IsDigit) Then erreurs.Add("Le mot de passe doit contenir au moins un chiffre.")
        If mdp.All(AddressOf Char.IsLetterOrDigit) Then erreurs.Add("Le mot de passe doit contenir au moins un caractère spécial.")
        If mdp.Any(AddressOf Char.IsWhiteSpace) Then erreurs.Add("Le mot de passe ne doit pas contenir d'espace.")

        Return erreurs
    End Function

End Class
