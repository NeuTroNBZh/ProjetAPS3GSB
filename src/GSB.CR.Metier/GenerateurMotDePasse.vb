Imports System.Security.Cryptography

''' <summary>
''' Génère des mots de passe provisoires robustes (conformes à <see cref="PolitiqueMotDePasse"/>),
''' sans caractères ambigus à la lecture (O/0, l/1/I) puisqu'ils sont communiqués au collaborateur.
''' </summary>
Public NotInheritable Class GenerateurMotDePasse

    Private Const Majuscules As String = "ABCDEFGHJKLMNPQRSTUVWXYZ"
    Private Const Minuscules As String = "abcdefghijkmnopqrstuvwxyz"
    Private Const Chiffres As String = "23456789"
    Private Const Speciaux As String = "!#$%&*+-=?@"

    Private Sub New()
    End Sub

    Public Shared Function Generer(Optional longueur As Integer = 12) As String
        If longueur < PolitiqueMotDePasse.LongueurMinimale Then
            Throw New ArgumentOutOfRangeException(NameOf(longueur))
        End If
        Dim tous = Majuscules & Minuscules & Chiffres & Speciaux
        ' Un caractère de chaque catégorie garanti, le reste au hasard, puis mélange
        Dim caracteres As New List(Of Char) From {
            Tirer(Majuscules), Tirer(Minuscules), Tirer(Chiffres), Tirer(Speciaux)}
        While caracteres.Count < longueur
            caracteres.Add(Tirer(tous))
        End While
        For i = caracteres.Count - 1 To 1 Step -1
            Dim j = RandomNumberGenerator.GetInt32(i + 1)
            Dim tmp = caracteres(i)
            caracteres(i) = caracteres(j)
            caracteres(j) = tmp
        Next
        Return New String(caracteres.ToArray())
    End Function

    Private Shared Function Tirer(alphabet As String) As Char
        Return alphabet(RandomNumberGenerator.GetInt32(alphabet.Length))
    End Function

End Class
