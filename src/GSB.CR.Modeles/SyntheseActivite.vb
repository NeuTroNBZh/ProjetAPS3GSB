''' <summary>
''' Synthèse de l'activité d'un collaborateur sur une période (EX-23).
''' Seuls les comptes-rendus validés sont comptés.
''' </summary>
Public Class SyntheseActivite

    Public Property Debut As Date

    Public Property Fin As Date

    Public Property NbVisites As Integer

    ''' <summary>Nombre de praticiens (titulaires) différents visités.</summary>
    Public Property NbPraticiens As Integer

    ''' <summary>Visites où un remplaçant a été rencontré.</summary>
    Public Property NbVisitesRemplacant As Integer

    ''' <summary>Coefficient de confiance moyen (1 à 5), Nothing sans visite.</summary>
    Public Property ConfianceMoyenne As Decimal?

    Public Property NbEchantillons As Integer

    Public Property CoutEchantillons As Decimal

    ''' <summary>Temps de saisie moyen par compte-rendu, en secondes (Nothing si non mesuré).</summary>
    Public Property TempsSaisieMoyen As Decimal?

    Public Property TempsSaisieTotal As Decimal

    ''' <summary>Brouillons en attente de validation (toutes dates confondues).</summary>
    Public Property NbBrouillons As Integer

    Public Property ParMotif As New List(Of Repartition)

    ''' <summary>Visites par mois, chaque mois de la période présent (zéro compris).</summary>
    Public Property ParMois As New List(Of VisitesDuMois)

    Public Property ProduitsPresentes As New List(Of Repartition)

    Public Property Echantillons As New List(Of EchantillonsDistribues)

End Class
