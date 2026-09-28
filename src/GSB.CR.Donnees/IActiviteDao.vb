Imports GSB.CR.Modeles

''' <summary>
''' Statistiques d'activité d'un collaborateur (EX-23).
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base.
''' </summary>
Public Interface IActiviteDao

    ''' <summary>
    ''' Synthèse des CR validés de <paramref name="matricule"/> visités entre <paramref name="debut"/>
    ''' et <paramref name="fin"/> (inclus). <see cref="SyntheseActivite.ParMois"/> ne contient que
    ''' les mois ayant au moins une visite.
    ''' </summary>
    Function ChargerSynthese(matricule As String, debut As Date, fin As Date) As SyntheseActivite

End Interface
