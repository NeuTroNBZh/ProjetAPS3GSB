Imports GSB.CR.Modeles

''' <summary>
''' Accès aux collaborateurs et à leur authentification.
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base.
''' </summary>
Public Interface ICollaborateurDao

    ''' <summary>Recherche un collaborateur par son login (insensible à la casse). Nothing si inconnu.</summary>
    Function TrouverParLogin(login As String) As Collaborateur

    ''' <summary>Affectation en cours du collaborateur. Nothing s'il n'en a pas (ou s'il est parti).</summary>
    Function TrouverAffectationEnCours(matricule As String) As Affectation

    ''' <summary>
    ''' Incrémente le compteur d'échecs et verrouille le compte s'il atteint <paramref name="maxEchecs"/>.
    ''' Renvoie le nouveau nombre d'échecs.
    ''' </summary>
    Function EnregistrerEchec(matricule As String, maxEchecs As Integer) As Integer

    ''' <summary>Remet le compteur d'échecs à zéro et mémorise la date de connexion.</summary>
    Sub EnregistrerSucces(matricule As String)

    ''' <summary>Remplace le mot de passe (déjà haché) et lève l'obligation de le changer.</summary>
    Sub ChangerMotDePasse(matricule As String, motDePasseHache As String)

    ''' <summary>Trace une tentative de connexion dans JOURNAL_CONNEXION.</summary>
    Sub Journaliser(login As String, matricule As String, succes As Boolean)

End Interface
