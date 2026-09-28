''' <summary>
''' Élément d'une petite table de référence (composant, type d'individu, présentation, dosage) :
''' un code et un libellé affiché dans les listes.
''' </summary>
Public Class ElementReferentiel

    Public Property Code As String = ""

    Public Property Libelle As String = ""

    Public Overrides Function ToString() As String
        Return Libelle
    End Function

End Class
