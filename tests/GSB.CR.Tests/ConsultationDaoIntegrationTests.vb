Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>Tests d'intégration des fiches praticiens et médicaments sur le jeu d'essai. Exclus de la CI.</summary>
<TestClass>
<TestCategory("Integration")>
Public Class ConsultationDaoIntegrationTests

    Private _dao As ConsultationDao

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _dao = New ConsultationDao(New ConnexionOracle(config))
    End Sub

    <TestMethod>
    Public Sub Rechercher_ParNomOuVille_InsensibleALaCasse()
        Assert.IsTrue(_dao.RechercherPraticiens("MART", Nothing, False, 100).Any(Function(p) p.Nom = "Martin"))
        Dim bordelais = _dao.RechercherPraticiens("bordeaux", Nothing, False, 100)
        Assert.IsTrue(bordelais.All(Function(p) p.Ville = "Bordeaux"))
        Assert.HasCount(2, bordelais, "Martin et Durand (Fontaine est inactif).")
    End Sub

    <TestMethod>
    Public Sub Rechercher_InactifsSurDemande()
        Assert.IsFalse(_dao.RechercherPraticiens("fontaine", Nothing, False, 100).Any())
        Assert.IsFalse(_dao.RechercherPraticiens("fontaine", Nothing, True, 100).Single().Actif)
    End Sub

    <TestMethod>
    Public Sub Rechercher_PortefeuilleDuVisiteur()
        Dim liste = _dao.RechercherPraticiens("", "a131", False, 100)

        CollectionAssert.AreEquivalent({1, 2}, liste.Select(Function(p) p.Numero).ToList())
        Assert.IsTrue(liste.All(Function(p) p.NomVisiteur = "Louis Villechalane"))
        Assert.AreEqual(#2026-09-10#, liste.Single(Function(p) p.Numero = 1).DateDerniereVisite)
    End Sub

    <TestMethod>
    Public Sub FichePraticien_SpecialitesSuiviEtHistorique()
        Dim fiche = _dao.ChargerFichePraticien(8)   ' Simon : repris par Luc Bioret après le départ de F. Daburon

        Assert.AreEqual("Simon", fiche.Praticien.Nom)
        Assert.AreEqual("Luc Bioret", fiche.NomVisiteur)
        Assert.AreEqual("MGE", fiche.Specialites.Single().Code)
        Assert.HasCount(2, fiche.Visites)
        Assert.AreEqual("François Daburon", fiche.Visites.Last().Visiteur, "L'historique garde les visites de l'ancien visiteur.")
    End Sub

    <TestMethod>
    Public Sub FichePraticien_RemplacantGardeSonHistorique()
        Dim fiche = _dao.ChargerFichePraticien(5)   ' Camille Petit, vue en remplacement du Dr Martin

        Dim visite = fiche.Visites.Single()
        Assert.IsTrue(visite.VuCommeRemplacant)
        Assert.AreEqual("Martin Hélène", visite.PraticienLie)
        Assert.IsNull(fiche.DateDerniereVisite, "Les visites comme remplaçant ne comptent pas dans sa périodicité de titulaire.")
    End Sub

    <TestMethod>
    Public Sub FichePraticien_TitulaireVoitLeRemplacantRencontre()
        Dim visite = _dao.ChargerFichePraticien(1).Visites.First()

        Assert.IsFalse(visite.VuCommeRemplacant)
        Assert.AreEqual("Petit Camille", visite.PraticienLie)
    End Sub

    <TestMethod>
    Public Sub FichePraticien_Inexistant_RenvoieNothing()
        Assert.IsNull(_dao.ChargerFichePraticien(999999))
    End Sub

    <TestMethod>
    Public Sub FicheMedicament_CompositionInteractionsPosologie()
        Dim fiche = _dao.ChargerFicheMedicament("DOLRIL7")

        Assert.AreEqual("DOLORIL", fiche.Medicament.NomCommercial)
        Assert.HasCount(3, fiche.Composition)
        Dim interaction = fiche.Interactions.Single()
        Assert.AreEqual("TROXADET", interaction.NomAutre)
        Assert.IsTrue(interaction.EstPerturbateur)
        Assert.AreEqual("500 mg", fiche.Posologies.Single().Dosage)
    End Sub

    <TestMethod>
    Public Sub FicheMedicament_InteractionsDansLesDeuxSens()
        Dim fiche = _dao.ChargerFicheMedicament("TROXT21")

        Assert.HasCount(2, fiche.Interactions)
        Assert.IsTrue(fiche.Interactions.All(Function(i) Not i.EstPerturbateur), "TROXADET est perturbé par DOLORIL et NOVELIX.")
    End Sub

    <TestMethod>
    Public Sub FicheMedicament_PosologieDuPlusAgeAuPlusJeune()
        Dim types = _dao.ChargerFicheMedicament("AMOPIL7").Posologies.Select(Function(p) p.TypeIndividu).ToList()

        CollectionAssert.AreEqual({"Adulte", "Enfant", "Nourrisson"}, types)
    End Sub

End Class
