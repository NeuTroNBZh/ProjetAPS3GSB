Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Tests d'intégration du référentiel médicaments (EX-73) sur la base réelle : composition, interactions,
''' posologie, composants et dosages. Chaque test supprime ce qu'il a créé. Exclus de la CI.
''' </summary>
<TestClass>
<TestCategory("Integration")>
<DoNotParallelize>
Public Class MedicamentsDaoIntegrationTests

    Private Const Medicament As String = "NOVEL26"
    Private _connexion As ConnexionOracle
    Private _dao As AdministrationDao
    Private _consultation As ConsultationDao

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _connexion = New ConnexionOracle(config)
        _dao = New AdministrationDao(_connexion)
        _consultation = New ConsultationDao(_connexion)
        Nettoyer()
    End Sub

    <TestCleanup>
    Public Sub Nettoyer()
        If _connexion Is Nothing Then Return
        Executer("delete from CONSTITUER where cmp_code = 'TSTC'")
        Executer("delete from PRESCRIRE where dos_code = 'TST9MG'")
        Executer("delete from INTERAGIR where med_perturbateur = 'NOVEL26' and med_perturbe = 'DOLRIL7'")
        Executer("delete from COMPOSANT where cmp_code = 'TSTC'")
        Executer("delete from DOSAGE where dos_code = 'TST9MG'")
    End Sub

    Private Sub Executer(sql As String)
        Using cnx = _connexion.Ouvrir()
            Using cmd As New Oracle.ManagedDataAccess.Client.OracleCommand(sql, cnx)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    <TestMethod>
    Public Sub Listes_JeuDEssai()
        Assert.HasCount(12, _dao.ListerComposants())
        Assert.AreEqual("ADU", _dao.ListerTypesIndividu().First().Code, "Du plus âgé au plus jeune")
        Assert.IsNotEmpty(_dao.ListerPresentations())
        CollectionAssert.Contains(_dao.ListerDosages().Select(Function(d) d.Libelle).ToList(), "500 mg")
    End Sub

    <TestMethod>
    Public Sub Composition_AjoutPuisRetrait()
        _dao.CreerComposant("TSTC", "Composant de test")
        _dao.AjouterComposition(Medicament, "TSTC", 12.5D, "mg")

        Dim ligne = _consultation.ChargerFicheMedicament(Medicament).Composition.Single(Function(c) c.CodeComposant = "TSTC")
        Assert.AreEqual(12.5D, ligne.Quantite)
        Assert.AreEqual("Composant de test", ligne.Composant)

        _dao.RetirerComposition(Medicament, "TSTC")
        Assert.IsFalse(_consultation.ChargerFicheMedicament(Medicament).Composition.Any(Function(c) c.CodeComposant = "TSTC"))
    End Sub

    <TestMethod>
    Public Sub Composition_Doublon_Detecte()
        _dao.CreerComposant("TSTC", "Composant de test")
        _dao.AjouterComposition(Medicament, "TSTC", 1, "mg")

        Dim ex = Assert.ThrowsExactly(Of AccesDonneesException)(Sub() _dao.AjouterComposition(Medicament, "TSTC", 2, "mg"))
        Assert.IsTrue(ex.EstDoublon)
    End Sub

    <TestMethod>
    Public Sub Interaction_VisibleDepuisLesDeuxMedicaments()
        _dao.AjouterInteraction(Medicament, "DOLRIL7", "Interaction de test")

        Dim depuisNovel = _consultation.ChargerFicheMedicament(Medicament).Interactions.Single(Function(i) i.DepotLegalAutre = "DOLRIL7")
        Dim depuisDolril = _consultation.ChargerFicheMedicament("DOLRIL7").Interactions.Single(Function(i) i.DepotLegalAutre = Medicament)
        Assert.IsTrue(depuisNovel.EstPerturbateur)
        Assert.IsFalse(depuisDolril.EstPerturbateur)

        _dao.RetirerInteraction(Medicament, "DOLRIL7")
        Assert.IsFalse(_consultation.ChargerFicheMedicament(Medicament).Interactions.Any(Function(i) i.DepotLegalAutre = "DOLRIL7"))
    End Sub

    <TestMethod>
    Public Sub Posologie_AvecNouveauDosage_AjoutPuisRetrait()
        _dao.CreerDosage("TST9MG", 9, "mg")
        _dao.AjouterPosologie(Medicament, "ADU", "CP", "TST9MG", "Posologie de test")

        Dim p = _consultation.ChargerFicheMedicament(Medicament).Posologies.Single(Function(x) x.CodeDosage = "TST9MG")
        Assert.AreEqual("9 mg", p.Dosage)
        Assert.AreEqual("ADU", p.CodeTypeIndividu)

        _dao.RetirerPosologie(Medicament, "ADU", "CP", "TST9MG")
        Assert.IsFalse(_consultation.ChargerFicheMedicament(Medicament).Posologies.Any(Function(x) x.CodeDosage = "TST9MG"))
    End Sub

End Class
