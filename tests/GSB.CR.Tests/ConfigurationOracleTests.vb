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

    <TestMethod>
    Public Sub InstructionSchema_SansSchema_RenvoieRien()
        Dim config As New ConfigurationOracle()

        Assert.AreEqual("", config.InstructionSchema())
    End Sub

    <TestMethod>
    Public Sub InstructionSchema_SchemaValide_PasseSurLeSchema()
        Dim config As New ConfigurationOracle() With {.Schema = "gsb"}

        Assert.AreEqual("ALTER SESSION SET CURRENT_SCHEMA = GSB", config.InstructionSchema())
    End Sub

    <DataTestMethod>
    <DataRow("GSB; DROP TABLE RAPPORT_VISITE")>
    <DataRow("GSB--")>
    <DataRow("1GSB")>
    <DataRow("""GSB""")>
    Public Sub InstructionSchema_NomInvalide_LeveUneErreur(schema As String)
        Dim config As New ConfigurationOracle() With {.Schema = schema}

        Assert.ThrowsExactly(Of InvalidOperationException)(Function() config.InstructionSchema())
    End Sub

    <TestMethod>
    Public Sub Charger_LitLeSchema()
        Dim dossier = Directory.CreateTempSubdirectory("gsbcr").FullName
        Try
            File.WriteAllText(Path.Combine(dossier, "appsettings.json"),
                "{""Oracle"":{""Hote"":""srv"",""Service"":""SVC"",""Schema"":""GSB""}}")

            Dim config = ConfigurationOracle.Charger(dossier)

            Assert.AreEqual("GSB", config.Schema)
        Finally
            Directory.Delete(dossier, recursive:=True)
        End Try
    End Sub

    ''' <summary>
    ''' Test d'intégration : connexion en précisant le schéma (cas du compte applicatif GSB_APP).
    ''' Avec le compte GSB, passer sur son propre schéma doit fonctionner à l'identique.
    ''' </summary>
    <TestMethod>
    <TestCategory("Integration")>
    Public Sub Ouvrir_AvecSchema_LitLesTablesDuSchema()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then
            Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        End If
        config.Schema = "GSB"

        Using cnx = New ConnexionOracle(config).Ouvrir()
            Using cmd = cnx.CreateCommand()
                cmd.CommandText = "select sys_context('USERENV', 'CURRENT_SCHEMA') from dual"
                Assert.AreEqual("GSB", CStr(cmd.ExecuteScalar()))
                cmd.CommandText = "select count(*) from MOTIF"
                Assert.IsGreaterThan(0, Convert.ToInt32(cmd.ExecuteScalar()))
            End Using
        End Using
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

        Assert.IsFalse(String.IsNullOrWhiteSpace(version))
    End Sub

End Class
