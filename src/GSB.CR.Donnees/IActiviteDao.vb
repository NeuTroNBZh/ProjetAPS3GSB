Imports GSB.CR.Modeles

''' <summary>
''' Statistiques d'activité d'un collaborateur, d'une région ou d'un secteur (EX-23, EX-31, EX-40).
''' Seuls les comptes-rendus validés sont comptés.
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base.
''' </summary>
Public Interface IActiviteDao

    ''' <summary>
    ''' Synthèse des CR validés du périmètre visités entre <paramref name="debut"/> et <paramref name="fin"/>
    ''' (inclus). <see cref="SyntheseActivite.ParMois"/> ne contient que les mois ayant au moins une visite.
    ''' </summary>
    Function ChargerSynthese(perimetre As Perimetre, debut As Date, fin As Date) As SyntheseActivite

    ''' <summary>Activité de chaque membre actuel du périmètre (membres sans visite compris).</summary>
    Function ActiviteParMembre(perimetre As Perimetre, debut As Date, fin As Date) As List(Of ActiviteMembre)

End Interface
