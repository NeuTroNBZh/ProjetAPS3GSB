''' <summary>Praticien du portefeuille à planifier, avec la raison et la priorité (EX-24).</summary>
Public Class PraticienARevoir

    Public Property Praticien As New PraticienResume()

    Public Property Etat As EtatPeriodicite

    ''' <summary>Vrai si la prochaine visite prévue lors du dernier CR est dépassée.</summary>
    Public Property ProchainePrevueDepassee As Boolean

    ''' <summary>Jours écoulés depuis la dernière visite (Nothing si jamais visité).</summary>
    Public Property JoursDepuisDerniereVisite As Integer?

    ''' <summary>1 = le plus urgent.</summary>
    Public Property Priorite As Integer

End Class
