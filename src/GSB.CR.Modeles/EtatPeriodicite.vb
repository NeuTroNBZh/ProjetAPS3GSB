''' <summary>Situation d'un praticien au regard de la périodicité des visites (tous les 6 à 8 mois).</summary>
Public Enum EtatPeriodicite
    ''' <summary>Visité il y a moins de 6 mois.</summary>
    AJour
    ''' <summary>Visité il y a 6 à 8 mois : à planifier.</summary>
    ARevoirBientot
    ''' <summary>Visité il y a plus de 8 mois.</summary>
    ARevoir
    ''' <summary>Aucune visite validée.</summary>
    JamaisVisite
End Enum
