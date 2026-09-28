Imports GSB.CR.Donnees
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Connexion de bout en bout sur la base réelle avec les comptes du jeu d'essai (docs/comptes-test.md).
''' Exclus de la CI (catégorie Integration).
''' </summary>
<TestClass>
<TestCategory("Integration")>
Public Class AuthentificationIntegrationTests

    Private _service As ServiceAuthentification

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _service = New ServiceAuthentification(New CollaborateurDao(New ConnexionOracle(config)), TimeProvider.System)
    End Sub

    <TestMethod>
    <DataRow("lvillechalane", Profil.Visiteur)>
    <DataRow("cbedos", Profil.Delegue)>
    <DataRow("clemoine", Profil.Responsable)>
    <DataRow("admin", Profil.Administrateur)>
    Public Sub Connecter_CompteDeTest_OuvreLeBonProfil(login As String, profilAttendu As Profil)
        Dim r = _service.Connecter(login, "Gsb2026!")

        Assert.AreEqual(StatutConnexion.Reussie, r.Statut, r.Message)
        Assert.AreEqual(profilAttendu, r.Utilisateur.Profil)
    End Sub

    <TestMethod>
    Public Sub Connecter_CompteAvecMotDePasseAChanger_ChangementRequis()
        Assert.AreEqual(StatutConnexion.ChangementMotDePasseRequis, _service.Connecter("dbunisset", "Gsb2026!").Statut)
    End Sub

    <TestMethod>
    Public Sub Connecter_CollaborateurParti_CompteInactif()
        Assert.AreEqual(StatutConnexion.CompteInactif, _service.Connecter("fdaburon", "Gsb2026!").Statut)
    End Sub

End Class
