Imports GSB.CR.Modeles

''' <summary>
''' Composition des équipes (région, secteur).
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base.
''' </summary>
Public Interface IEquipeDao

    ''' <summary>Visiteurs et délégués actuellement affectés au périmètre, triés par région puis par nom.</summary>
    Function ListerMembres(perimetre As Perimetre) As List(Of MembreEquipe)

End Interface
