''' <summary>
''' Compte-rendu de visite complet — tables RAPPORT_VISITE, PRESENTER et OFFRIR.
''' </summary>
Public Class RapportVisite

    ''' <summary>Numéro du rapport (Nothing pour un nouveau rapport).</summary>
    Public Property Numero As Integer?

    ''' <summary>Matricule de l'auteur (imposé par l'application : le collaborateur connecté).</summary>
    Public Property MatriculeAuteur As String = ""

    ''' <summary>Praticien titulaire du cabinet, pour lequel la visite est faite.</summary>
    Public Property NumeroPraticien As Integer?

    ''' <summary>Nom du praticien titulaire (affichage).</summary>
    Public Property NomPraticien As String

    ''' <summary>Remplaçant réellement rencontré (Nothing si c'est le titulaire).</summary>
    Public Property NumeroRemplacant As Integer?

    ''' <summary>Nom du remplaçant (affichage).</summary>
    Public Property NomRemplacant As String

    Public Property DateVisite As Date?

    Public Property CodeMotif As String

    ''' <summary>Précision libre, uniquement pour le motif « Autre ».</summary>
    Public Property PrecisionMotif As String

    Public Property Bilan As String

    ''' <summary>Confiance du praticien dans les produits GSB, de 1 (faible) à 5 (forte).</summary>
    Public Property CoefConfiance As Integer?

    Public Property DateProchaineVisite As Date?

    Public Property Etat As EtatRapport = EtatRapport.Brouillon

    Public Property DateSaisie As DateTime?

    Public Property DateModification As DateTime?

    Public Property DateValidation As DateTime?

    ''' <summary>Dépôts légaux des produits présentés, dans l'ordre (2 au maximum).</summary>
    Public Property ProduitsPresentes As New List(Of String)

    ''' <summary>Échantillons offerts (indépendants des produits présentés).</summary>
    Public Property Echantillons As New List(Of EchantillonOffert)

    Public ReadOnly Property EstNouveau As Boolean
        Get
            Return Not Numero.HasValue
        End Get
    End Property

    ''' <summary>Nombre total d'échantillons offerts.</summary>
    Public ReadOnly Property TotalEchantillons As Integer
        Get
            Return Echantillons.Sum(Function(e) e.Quantite)
        End Get
    End Property

End Class
