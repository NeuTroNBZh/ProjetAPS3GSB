''' <summary>Praticien visité (médecin, pharmacien, personnel de santé…) — table PRATICIEN.</summary>
Public Class Praticien

    ''' <summary>Numéro du praticien (Nothing tant qu'il n'est pas enregistré).</summary>
    Public Property Numero As Integer?

    Public Property Nom As String = ""

    Public Property Prenom As String = ""

    Public Property Adresse As String

    Public Property CodePostal As String

    Public Property Ville As String

    Public Property Telephone As String

    Public Property Email As String

    ''' <summary>Code du type de praticien (MV, MH, PO, PH, PS).</summary>
    Public Property CodeType As String = ""

    Public Property LibelleType As String

    ''' <summary>Coefficient de notoriété (donnée achetée).</summary>
    Public Property CoefNotoriete As Decimal?

    Public Property Actif As Boolean = True

    ''' <summary>Nom puis prénom, pour les listes triées.</summary>
    Public ReadOnly Property NomComplet As String
        Get
            Return $"{Nom} {Prenom}"
        End Get
    End Property

    ''' <summary>Libellé affiché dans les listes déroulantes (nom, prénom et ville).</summary>
    Public Overrides Function ToString() As String
        Return If(String.IsNullOrEmpty(Ville), NomComplet, $"{NomComplet} — {Ville}")
    End Function

End Class
