Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Tests d'intégration des modules Délégué / Responsable sur le jeu d'essai
''' (équipes, activité, CR, praticiens, stock et dotations). Exclus de la CI.
''' </summary>
<TestClass>
<TestCategory("Integration")>
<DoNotParallelize>
Public Class EquipeIntegrationTests

    Private Shared ReadOnly Aquitaine As Perimetre = Perimetre.DeLaRegion("AQU", "Aquitaine")
    Private Shared ReadOnly SecteurEst As Perimetre = Perimetre.DuSecteur("E", "Est")

    Private _connexion As ConnexionOracle

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _connexion = New ConnexionOracle(config)
    End Sub

    <TestMethod>
    Public Sub Membres_RegionEtSecteur()
        Dim dao As New EquipeDao(_connexion)

        CollectionAssert.AreEquivalent({"d01", "a131", "a17"}, dao.ListerMembres(Aquitaine).Select(Function(m) m.Matricule).ToList())
        Dim est = dao.ListerMembres(SecteurEst)
        CollectionAssert.AreEquivalent({"d03", "b19", "b25"}, est.Select(Function(m) m.Matricule).ToList())
        Assert.AreEqual(Profil.Delegue, est.Single(Function(m) m.Matricule = "d03").Profil)
    End Sub

    <TestMethod>
    Public Sub ActiviteParMembre_TousLesMembresMemeSansVisite()
        Dim liste = New ActiviteDao(_connexion).ActiviteParMembre(Aquitaine, #2026-01-01#, #2026-09-28#)

        Assert.HasCount(3, liste)
        Assert.AreEqual(3, liste.Single(Function(m) m.Matricule = "a131").NbVisites)
        Dim andre = liste.Single(Function(m) m.Matricule = "a17")
        Assert.AreEqual(1, andre.NbVisites, "Le brouillon n'est pas compté")
        Assert.AreEqual(1, andre.NbBrouillons)
        Assert.AreEqual(1, liste.Single(Function(m) m.Matricule = "d01").NbVisites)
    End Sub

    <TestMethod>
    Public Sub SyntheseRegion_AdditionneLesMembres()
        Dim s = New ActiviteDao(_connexion).ChargerSynthese(Aquitaine, #2026-01-01#, #2026-09-28#)

        Assert.AreEqual(5, s.NbVisites)
        Assert.AreEqual(1, s.NbBrouillons)
    End Sub

    <TestMethod>
    Public Sub RapportsEquipe_SansLesBrouillons()
        Dim liste = New RapportDao(_connexion).ListerParPerimetre(Aquitaine, #2023-09-28#, inclureBrouillons:=False)

        Assert.IsFalse(liste.Any(Function(r) r.Etat = EtatRapport.Brouillon))
        Assert.IsTrue(liste.Any(Function(r) r.Auteur = "Bedos Christian"))
        Assert.IsTrue(liste.Any(Function(r) r.MatriculeAuteur = "a17"))
    End Sub

    <TestMethod>
    Public Sub PraticiensDeLaRegion()
        Dim liste = New ConsultationDao(_connexion).RechercherPraticiens("", Aquitaine, False, 500)

        CollectionAssert.AreEquivalent({1, 2, 3, 4, 17}, liste.Select(Function(p) p.Numero).ToList())
    End Sub

    <TestMethod>
    Public Sub Stock_AnomalieVolontaireDuJeuDEssai()
        Dim ligne = New EchantillonDao(_connexion).Stock(SecteurEst, #2026-07-01#).
                        Single(Function(l) l.Matricule = "b25" AndAlso l.DepotLegal = "EQUILARX6")

        Assert.AreEqual(8, ligne.Attribue)
        Assert.AreEqual(10, ligne.Distribue)
        Assert.IsTrue(ligne.EnDepassement)
    End Sub

    <TestMethod>
    Public Sub Dotation_CreationRemplacementSuppression()
        Dim dao As New EchantillonDao(_connexion)
        Dim mois = #2026-10-01#
        Try
            dao.EnregistrerDotation("a17", "NOVEL26", mois, 4, "d01")
            Assert.AreEqual(4, dao.Stock(Aquitaine, mois).Single(Function(l) l.Matricule = "a17").Attribue)

            dao.EnregistrerDotation("a17", "NOVEL26", mois, 6, "d01")   ' même visiteur, produit et mois : remplacement
            Assert.AreEqual(6, dao.Stock(Aquitaine, mois).Single(Function(l) l.Matricule = "a17").Attribue)
        Finally
            Assert.IsTrue(dao.SupprimerDotation("a17", "NOVEL26", mois))
        End Try
        Assert.IsFalse(dao.Stock(Aquitaine, mois).Any())
    End Sub

    <TestMethod>
    Public Sub Dotation_SaisieParUnVisiteur_RefuseeParLaBase()
        Dim dao As New EchantillonDao(_connexion)

        Assert.ThrowsExactly(Of AccesDonneesException)(Sub() dao.EnregistrerDotation("a17", "NOVEL26", #2026-10-01#, 4, "a131"))
    End Sub

End Class
