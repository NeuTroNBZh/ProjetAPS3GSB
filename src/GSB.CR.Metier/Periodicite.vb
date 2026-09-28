Imports GSB.CR.Modeles

''' <summary>
''' Règle de périodicité des visites : chaque praticien doit être revu tous les 6 à 8 mois
''' (motif principal des visites selon les visiteurs, EX-24).
''' </summary>
Public NotInheritable Class Periodicite

    ''' <summary>En deçà, le praticien est à jour.</summary>
    Public Const MoisMinimum As Integer = 6

    ''' <summary>Au-delà, le praticien est à revoir en priorité.</summary>
    Public Const MoisMaximum As Integer = 8

    Private Sub New()
    End Sub

    ''' <summary>Situation d'un praticien selon la date de sa dernière visite validée.</summary>
    Public Shared Function Evaluer(dateDerniereVisite As Date?, aujourdhui As Date) As EtatPeriodicite
        If Not dateDerniereVisite.HasValue Then Return EtatPeriodicite.JamaisVisite
        Dim derniere = dateDerniereVisite.Value.Date
        If derniere > aujourdhui.Date.AddMonths(-MoisMinimum) Then Return EtatPeriodicite.AJour
        If derniere >= aujourdhui.Date.AddMonths(-MoisMaximum) Then Return EtatPeriodicite.ARevoirBientot
        Return EtatPeriodicite.ARevoir
    End Function

    ''' <summary>Date à partir de laquelle la prochaine visite est attendue (dernière visite + 6 mois).</summary>
    Public Shared Function ProchaineVisiteConseillee(dateDerniereVisite As Date?) As Date?
        Return dateDerniereVisite?.Date.AddMonths(MoisMinimum)
    End Function

End Class
