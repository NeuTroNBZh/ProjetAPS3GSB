Imports GSB.CR.Donnees

''' <summary>Tests d'intégration de la synthèse d'activité sur le jeu d'essai. Exclus de la CI.</summary>
<TestClass>
<TestCategory("Integration")>
Public Class ActiviteDaoIntegrationTests

    Private _dao As ActiviteDao

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _dao = New ActiviteDao(New ConnexionOracle(config))
    End Sub

    <TestMethod>
    Public Sub Synthese_2026_LouisVillechalane()
        ' CR validés 2026 de a131 : n°3 (Durand), n°4 et n°5 (Martin, remplaçante vue au n°5)
        Dim s = _dao.ChargerSynthese("a131", #2026-01-01#, #2026-09-28#)

        Assert.AreEqual(3, s.NbVisites)
        Assert.AreEqual(2, s.NbPraticiens)
        Assert.AreEqual(1, s.NbVisitesRemplacant)
        Assert.AreEqual(3.33D, s.ConfianceMoyenne)
        Assert.AreEqual(13, s.NbEchantillons, "3 CLAZER + 5 NOVELIX + 2 APATOUX + 1 EQUILAR + 2 NOVELIX")
        Assert.AreEqual(7, s.Echantillons.Single(Function(e) e.DepotLegal = "NOVEL26").Quantite)
        Assert.AreEqual(3 * 2.6D + 7 * 2.1D + 2 * 0.9D + 1.3D, s.CoutEchantillons)
        Assert.AreEqual(2, s.ParMotif.Single(Function(m) m.Libelle.StartsWith("Nouveauté")).Nombre)
        Assert.HasCount(3, s.ParMois, "février, mai et septembre")
        Assert.AreEqual("NOVELIX", s.ProduitsPresentes.First().Libelle)
        Assert.IsNotNull(s.TempsSaisieMoyen)
    End Sub

    <TestMethod>
    Public Sub Synthese_PeriodeSansVisite_ZerosEtListesVides()
        Dim s = _dao.ChargerSynthese("a131", #2024-01-01#, #2024-12-31#)

        Assert.AreEqual(0, s.NbVisites)
        Assert.IsNull(s.ConfianceMoyenne)
        Assert.AreEqual(0D, s.CoutEchantillons)
        Assert.IsEmpty(s.ParMois)
        Assert.IsNull(s.TempsSaisieMoyen)
    End Sub

    <TestMethod>
    Public Sub Synthese_CompteLesBrouillonsEnAttente()
        Assert.AreEqual(1, _dao.ChargerSynthese("a17", #2026-01-01#, #2026-09-28#).NbBrouillons)
    End Sub

End Class
