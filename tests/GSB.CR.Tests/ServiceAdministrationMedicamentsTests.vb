Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>Référentiel médicaments de l'administration : composition, interactions, posologie (EX-73).</summary>
<TestClass>
Public Class ServiceAdministrationMedicamentsTests

    Private _dao As FauxAdministrationDao
    Private _service As ServiceAdministration

    Private Shared ReadOnly Admin As New UtilisateurConnecte(New Collaborateur() With {.Matricule = "adm1"},
                                                             New Affectation() With {.Profil = Profil.Administrateur})
    Private Shared ReadOnly Visiteur As New UtilisateurConnecte(New Collaborateur() With {.Matricule = "a131"},
                                                                New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "AQU"})

    <TestInitialize>
    Public Sub Initialiser()
        _dao = New FauxAdministrationDao()
        _service = New ServiceAdministration(_dao, New FauxReferentielDao(), New FauxConsultationDao(), New HorlogeFixe(#2026-09-28#))
    End Sub

    <TestMethod>
    Public Sub Acces_ReserveAuxAdministrateurs()
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.AjouterComposant(Visiteur, "NOVEL26", "IBUP", 400, "mg"))
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.Composants(Visiteur))
    End Sub

    <TestMethod>
    Public Sub FicheMedicament_Inconnu_Erreur()
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.FicheMedicament(Admin, "INCONNU"))
        Assert.IsNotNull(_service.FicheMedicament(Admin, "NOVEL26"))
    End Sub

    ' --- Composition --------------------------------------------------------

    <TestMethod>
    Public Sub AjouterComposant_Valide_QuantiteArrondieEtUniteNettoyee()
        Dim r = _service.AjouterComposant(Admin, "NOVEL26", "IBUP", 400.12345D, "  mg ")

        Assert.IsTrue(r.Reussi, String.Join(" | ", r.Erreurs))
        Assert.AreEqual("composition NOVEL26 IBUP 400.123 mg", _dao.Operations.Single())
    End Sub

    <TestMethod>
    Public Sub AjouterComposant_Incomplet_TroisErreurs()
        Dim r = _service.AjouterComposant(Admin, "NOVEL26", Nothing, 0, " ")

        Assert.HasCount(3, r.Erreurs, String.Join(" | ", r.Erreurs))
        Assert.IsEmpty(_dao.Operations)
    End Sub

    <TestMethod>
    Public Sub AjouterComposant_DejaPresent_MessageExplicite()
        _dao.DoublonAuProchainAppel = True

        Dim r = _service.AjouterComposant(Admin, "NOVEL26", "IBUP", 400, "mg")

        StringAssert.Contains(r.Erreurs.Single(), "déjà dans la composition")
    End Sub

    <TestMethod>
    Public Sub RetirerComposant_ParCode()
        _service.RetirerComposant(Admin, "NOVEL26", New LigneComposition() With {.CodeComposant = "IBUP", .Composant = "Ibuprofène"})

        Assert.AreEqual("retirer composition NOVEL26 IBUP", _dao.Operations.Single())
    End Sub

    <TestMethod>
    Public Sub CreerComposant_CodeNormalise()
        Dim r = _service.CreerComposant(Admin, " ketp ", "Kétoprofène")

        Assert.IsTrue(r.Reussi, String.Join(" | ", r.Erreurs))
        Assert.AreEqual("composant KETP Kétoprofène", _dao.Operations.Single())
    End Sub

    <TestMethod>
    <DataRow("K", "Kétoprofène")>
    <DataRow("KET-P", "Kétoprofène")>
    <DataRow("KETP", "")>
    Public Sub CreerComposant_Invalide_Refuse(code As String, libelle As String)
        Assert.IsFalse(_service.CreerComposant(Admin, code, libelle).Reussi)
        Assert.IsEmpty(_dao.Operations)
    End Sub

    ' --- Dosages -------------------------------------------------------------

    <TestMethod>
    <DataRow("500", "mg", "500MG")>
    <DataRow("1", "g", "1G")>
    <DataRow("0.5", "%", "0V5PC")>
    <DataRow("2.5", "ml", "2V5ML")>
    Public Sub CodeDosage_LisibleEtCompatibleAvecLeJeuDEssai(quantite As String, unite As String, attendu As String)
        Assert.AreEqual(attendu, ServiceAdministration.CodeDosage(Decimal.Parse(quantite, Globalization.CultureInfo.InvariantCulture), unite))
    End Sub

    <TestMethod>
    Public Sub CreerDosage_Valide()
        Dim r = _service.CreerDosage(Admin, 200, "mg")

        Assert.IsTrue(r.Reussi, String.Join(" | ", r.Erreurs))
        Assert.AreEqual("dosage 200MG 200 mg", _dao.Operations.Single())
    End Sub

    <TestMethod>
    Public Sub CreerDosage_Existant_MessageExplicite()
        _dao.DoublonAuProchainAppel = True

        StringAssert.Contains(_service.CreerDosage(Admin, 500, "mg").Erreurs.Single(), "existe déjà")
    End Sub

    ' --- Interactions ---------------------------------------------------------

    <TestMethod>
    Public Sub AjouterInteraction_DansLesDeuxSens()
        _service.AjouterInteraction(Admin, "NOVEL26", "TROXT21", perturbeLAutre:=True, description:="Risque hémorragique.")
        _service.AjouterInteraction(Admin, "NOVEL26", "APATOUX22", perturbeLAutre:=False, description:=Nothing)

        CollectionAssert.AreEqual({"interaction NOVEL26>TROXT21 Risque hémorragique.", "interaction APATOUX22>NOVEL26 "}, _dao.Operations)
    End Sub

    <TestMethod>
    Public Sub AjouterInteraction_AvecLuiMemeOuSansAutre_Refuse()
        StringAssert.Contains(_service.AjouterInteraction(Admin, "NOVEL26", "NOVEL26", True, Nothing).Erreurs.Single(), "lui-même")
        Assert.IsFalse(_service.AjouterInteraction(Admin, "NOVEL26", Nothing, True, Nothing).Reussi)
        Assert.IsEmpty(_dao.Operations)
    End Sub

    <TestMethod>
    Public Sub AjouterInteraction_DescriptionTropLongue_Refuse()
        Assert.IsFalse(_service.AjouterInteraction(Admin, "NOVEL26", "TROXT21", True, New String("x"c, 501)).Reussi)
    End Sub

    <TestMethod>
    Public Sub RetirerInteraction_RespecteLeSens()
        _service.RetirerInteraction(Admin, "NOVEL26", New InteractionMedicamenteuse() With {.DepotLegalAutre = "TROXT21", .EstPerturbateur = True})
        _service.RetirerInteraction(Admin, "NOVEL26", New InteractionMedicamenteuse() With {.DepotLegalAutre = "DOLRIL7", .EstPerturbateur = False})

        CollectionAssert.AreEqual({"retirer interaction NOVEL26>TROXT21", "retirer interaction DOLRIL7>NOVEL26"}, _dao.Operations)
    End Sub

    ' --- Posologie --------------------------------------------------------------

    <TestMethod>
    Public Sub AjouterPosologie_Valide()
        Dim r = _service.AjouterPosologie(Admin, "NOVEL26", "ADU", "CP", "400MG", " 1 comprimé 3 fois par jour ")

        Assert.IsTrue(r.Reussi, String.Join(" | ", r.Erreurs))
        Assert.AreEqual("posologie NOVEL26 ADU CP 400MG 1 comprimé 3 fois par jour", _dao.Operations.Single())
    End Sub

    <TestMethod>
    Public Sub AjouterPosologie_Incomplete_QuatreErreurs()
        Dim r = _service.AjouterPosologie(Admin, "NOVEL26", Nothing, Nothing, Nothing, "")

        Assert.HasCount(4, r.Erreurs, String.Join(" | ", r.Erreurs))
    End Sub

    <TestMethod>
    Public Sub AjouterPosologie_DejaDefinie_MessageExplicite()
        _dao.DoublonAuProchainAppel = True

        StringAssert.Contains(_service.AjouterPosologie(Admin, "NOVEL26", "ADU", "CP", "400MG", "1 cp").Erreurs.Single(), "existe déjà")
    End Sub

    <TestMethod>
    Public Sub RetirerPosologie_ParCodes()
        _service.RetirerPosologie(Admin, "NOVEL26", New Posologie() With {.CodeTypeIndividu = "ADU", .CodePresentation = "CP", .CodeDosage = "400MG"})

        Assert.AreEqual("retirer posologie NOVEL26 ADU CP 400MG", _dao.Operations.Single())
    End Sub

End Class
