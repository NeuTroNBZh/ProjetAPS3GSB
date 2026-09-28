''' <summary>État d'un compte-rendu de visite (colonne rap_etat : B / V).</summary>
Public Enum EtatRapport
    ''' <summary>Enregistré mais incomplet ou non définitif ; exclu des statistiques.</summary>
    Brouillon
    ''' <summary>Définitif et complet ; pris en compte dans les statistiques.</summary>
    Valide
End Enum
