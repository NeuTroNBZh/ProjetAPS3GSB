Imports System.IO
Imports GSB.CR.Donnees

<TestClass>
Public Class ConfigurationOracleTests

    <TestMethod>
    Public Sub ChaineDeConnexion_ConfigComplete_ContientServeurEtUtilisateur()
        Dim config As New ConfigurationOracle() With {
            .Hote = "100.109.217.110", .Port = 1521, .Service = "ORCLPDB1",
            .Utilisateur = "GSB", .MotDePasse = "secret"}

        Dim chaine = config.ChaineDeConnexion()

        StringAssert.Contains(chaine, "100.109.217.110:1521/ORCLPDB1")
        StringAssert.Contains(chaine, "GSB")
    End Sub

    <TestMethod>
    Public Sub ChaineDeConnexion_SansMotDePasse_LeveUneErreur()
        Dim config As New ConfigurationOracle() With {
            .Hote = "100.109.217.110", .Service = "ORCLPDB1", .Utilisateur = "GSB"}

        Assert.IsFalse(config.EstComplete)
        Assert.ThrowsExactly(Of InvalidOperationException)(Function() config.ChaineDeConnexion())
    End Sub

    <TestMethod>
    Public Sub Charger_FusionneFichierPublicEtFichierLocal()
        Dim dossier = Directory.CreateTempSubdirectory("gsbcr").FullName
        Try
            File.WriteAllText(Path.Combine(dossier, "appsettings.json"),
                "{""Oracle"":{""Hote"":""srv"",""Port"":1522,""Service"":""SVC""}}")
            File.WriteAllText(Path.Combine(dossier, "appsettings.Local.json"),
                "{""Oracle"":{""Utilisateur"":""u"",""MotDePasse"":""p""}}")

            Dim config = ConfigurationOracle.Charger(dossier)

            Assert.AreEqual("srv", config.Hote)
            Assert.AreEqual(1522, config.Port)
            Assert.AreEqual("u", config.Utilisateur)
            Assert.IsTrue(config.EstComplete)
        Finally
            Directory.Delete(dossier, recursive:=True)
        End Try
    End Sub

    ''' <summary>
    ''' Test d'intégration : nécessite appsettings.Local.json et l'accès au serveur.
    ''' Exclu de la CI (catégorie Integration).
    ''' </summary>
    <TestMethod>
    <TestCategory("Integration")>
    Public Sub Tester_ConnexionAuServeurReel()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then
            Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        End If

        Dim version = New ConnexionOracle(config).Tester()

        StringAssert.StartsWith(version, "19")
    End Sub

End Class
