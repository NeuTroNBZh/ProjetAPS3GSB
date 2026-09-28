Imports GSB.CR.Modeles

''' <summary>
''' Données de référence utiles à la saisie des comptes-rendus : praticiens, médicaments, motifs.
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base.
''' </summary>
Public Interface IReferentielDao

    ''' <summary>Praticiens actifs actuellement dans le portefeuille du collaborateur, triés par nom.</summary>
    Function ListerPortefeuille(matricule As String) As List(Of Praticien)

    ''' <summary>
    ''' Praticiens actifs dont le nom ou le prénom commence par <paramref name="debut"/>
    ''' (tous si vide), limités à <paramref name="maximum"/> résultats.
    ''' </summary>
    Function RechercherPraticiens(debut As String, maximum As Integer) As List(Of Praticien)

    ''' <summary>Praticien par son numéro (même inactif). Nothing s'il n'existe pas.</summary>
    Function TrouverPraticien(numero As Integer) As Praticien

    ''' <summary>Enregistre un nouveau praticien (ex. remplaçant) et renvoie son numéro.</summary>
    Function CreerPraticien(praticien As Praticien) As Integer

    Function ListerTypesPraticien() As List(Of TypePraticien)

    ''' <summary>Tous les médicaments (actifs et inactifs), triés par nom commercial.</summary>
    Function ListerMedicaments() As List(Of Medicament)

    ''' <summary>Tous les motifs (actifs et inactifs), dans l'ordre d'affichage.</summary>
    Function ListerMotifs() As List(Of Motif)

End Interface
