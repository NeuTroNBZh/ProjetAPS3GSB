''' <summary>Médicament du laboratoire — table MEDICAMENT (vue simplifiée pour la saisie des CR).</summary>
Public Class Medicament

    ''' <summary>Numéro de dépôt légal (identifiant).</summary>
    Public Property DepotLegal As String = ""

    Public Property NomCommercial As String = ""

    Public Property CodeFamille As String = ""

    Public Property LibelleFamille As String

    Public Property PrixEchantillon As Decimal

    Public Property Actif As Boolean = True

    Public Overrides Function ToString() As String
        Return NomCommercial
    End Function

End Class
