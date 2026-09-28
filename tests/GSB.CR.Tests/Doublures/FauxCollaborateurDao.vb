Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>DAO en mémoire pour tester la couche Métier sans base de données.</summary>
Public Class FauxCollaborateurDao
    Implements ICollaborateurDao

    Public ReadOnly Collaborateurs As New Dictionary(Of String, Collaborateur)(StringComparer.OrdinalIgnoreCase)
    Public ReadOnly Affectations As New Dictionary(Of String, Affectation)
    Public ReadOnly Journal As New List(Of (Login As String, Matricule As String, Succes As Boolean))

    ''' <summary>Si vrai, toutes les méthodes simulent une panne de la base.</summary>
    Public Property EnPanne As Boolean

    Public Sub Ajouter(collaborateur As Collaborateur, affectation As Affectation)
        Collaborateurs(collaborateur.Login) = collaborateur
        If affectation IsNot Nothing Then Affectations(collaborateur.Matricule) = affectation
    End Sub

    Private Function Par(matricule As String) As Collaborateur
        Return Collaborateurs.Values.Single(Function(c) c.Matricule = matricule)
    End Function

    Private Sub VerifierPanne()
        If EnPanne Then Throw New AccesDonneesException("Panne simulée", New Exception())
    End Sub

    Public Function TrouverParLogin(login As String) As Collaborateur Implements ICollaborateurDao.TrouverParLogin
        VerifierPanne()
        Dim c As Collaborateur = Nothing
        Return If(Collaborateurs.TryGetValue(login, c), c, Nothing)
    End Function

    Public Function TrouverAffectationEnCours(matricule As String) As Affectation Implements ICollaborateurDao.TrouverAffectationEnCours
        VerifierPanne()
        Dim a As Affectation = Nothing
        Return If(Affectations.TryGetValue(matricule, a), a, Nothing)
    End Function

    Public Function EnregistrerEchec(matricule As String, maxEchecs As Integer) As Integer Implements ICollaborateurDao.EnregistrerEchec
        VerifierPanne()
        Dim c = Par(matricule)
        c.NbEchecsConnexion += 1
        If c.NbEchecsConnexion >= maxEchecs Then c.Verrouille = True
        Return c.NbEchecsConnexion
    End Function

    Public Sub EnregistrerSucces(matricule As String) Implements ICollaborateurDao.EnregistrerSucces
        VerifierPanne()
        Dim c = Par(matricule)
        c.NbEchecsConnexion = 0
        c.DerniereConnexion = DateTime.Now
    End Sub

    Public Sub ChangerMotDePasse(matricule As String, motDePasseHache As String) Implements ICollaborateurDao.ChangerMotDePasse
        VerifierPanne()
        Dim c = Par(matricule)
        c.MotDePasseHache = motDePasseHache
        c.MotDePasseAChanger = False
    End Sub

    Public Sub Journaliser(login As String, matricule As String, succes As Boolean) Implements ICollaborateurDao.Journaliser
        VerifierPanne()
        Journal.Add((login, matricule, succes))
    End Sub

End Class
