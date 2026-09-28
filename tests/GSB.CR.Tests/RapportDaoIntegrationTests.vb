Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Tests d'intégration des comptes-rendus et référentiels sur la base réelle (jeu d'essai).
''' Les rapports créés sont supprimés à la fin de chaque test. Exclus de la CI.
''' </summary>
<TestClass>
<TestCategory("Integration")>
<DoNotParallelize>
Public Class RapportDaoIntegrationTests

    Private _connexion As ConnexionOracle
    Private _rapports As RapportDao
    Private _referentiel As ReferentielDao
    Private ReadOnly _crees As New List(Of Integer)

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _connexion = New ConnexionOracle(config)
        _rapports = New RapportDao(_connexion)
        _referentiel = New ReferentielDao(_connexion)
    End Sub

    <TestCleanup>
    Public Sub Nettoyer()
        If _connexion Is Nothing Then Return
        Using cnx = _connexion.Ouvrir()
            For Each numero In _crees
                Using cmd As New Oracle.ManagedDataAccess.Client.OracleCommand("delete from RAPPORT_VISITE where rap_num = :n", cnx)
                    cmd.Parameters.Add("n", numero)
                    cmd.ExecuteNonQuery()
                End Using
            Next
        End Using
    End Sub

    Private Function NouveauBrouillon() As RapportVisite
        Dim r As New RapportVisite() With {
            .MatriculeAuteur = "a131", .NumeroPraticien = 2, .DateVisite = Date.Today.AddDays(-1),
            .CodeMotif = "PERIO", .Etat = EtatRapport.Brouillon}
        r.ProduitsPresentes.AddRange({"CLAZER6", "AMOPIL7"})
        r.Echantillons.Add(New EchantillonOffert() With {.DepotLegal = "CLAZER6", .Quantite = 3})
        r.Echantillons.Add(New EchantillonOffert() With {.DepotLegal = "INSXT5", .Quantite = 1})
        Return r
    End Function

    <TestMethod>
    Public Sub CreerPuisCharger_RapportCompletRelu()
        Dim numero = _rapports.Creer(NouveauBrouillon())
        _crees.Add(numero)

        Dim r = _rapports.Charger(numero)

        Assert.AreEqual("a131", r.MatriculeAuteur)
        Assert.AreEqual(2, r.NumeroPraticien)
        Assert.AreEqual(EtatRapport.Brouillon, r.Etat)
        CollectionAssert.AreEqual({"CLAZER6", "AMOPIL7"}, r.ProduitsPresentes)
        Assert.AreEqual(4, r.TotalEchantillons)
        Assert.IsNotNull(r.DateSaisie)
        Assert.IsNull(r.DateValidation)
    End Sub

    <TestMethod>
    Public Sub Modifier_ValiderEtRemplacerLesLignes()
        Dim numero = _rapports.Creer(NouveauBrouillon())
        _crees.Add(numero)
        Dim r = _rapports.Charger(numero)

        r.Bilan = "Bilan de test"
        r.CoefConfiance = 4
        r.Etat = EtatRapport.Valide
        r.NumeroRemplacant = 5
        r.ProduitsPresentes = New List(Of String) From {"NOVEL26"}
        r.Echantillons = New List(Of EchantillonOffert) From {New EchantillonOffert() With {.DepotLegal = "NOVEL26", .Quantite = 2}}
        _rapports.Modifier(r)

        Dim relu = _rapports.Charger(numero)
        Assert.AreEqual(EtatRapport.Valide, relu.Etat)
        Assert.IsNotNull(relu.DateValidation)
        Assert.IsNotNull(relu.DateModification)
        Assert.AreEqual(5, relu.NumeroRemplacant)
        StringAssert.StartsWith(relu.NomRemplacant, "Petit")
        CollectionAssert.AreEqual({"NOVEL26"}, relu.ProduitsPresentes)
        Assert.AreEqual(2, relu.TotalEchantillons)
    End Sub

    <TestMethod>
    Public Sub SupprimerBrouillon_ValideRefuse_BrouillonSupprime()
        Dim brouillon = _rapports.Creer(NouveauBrouillon())
        _crees.Add(brouillon)
        Dim valide = NouveauBrouillon()
        valide.Bilan = "ok" : valide.CoefConfiance = 3 : valide.Etat = EtatRapport.Valide
        Dim numValide = _rapports.Creer(valide)
        _crees.Add(numValide)

        Assert.IsTrue(_rapports.SupprimerBrouillon(brouillon))
        Assert.IsNull(_rapports.Charger(brouillon))
        Assert.IsFalse(_rapports.SupprimerBrouillon(numValide))
    End Sub

    <TestMethod>
    Public Sub Creer_TroisProduitsPresentes_TransactionAnnulee()
        Dim r = NouveauBrouillon()
        r.ProduitsPresentes.Add("NOVEL26")   ' 3e produit : refusé par la base

        Assert.ThrowsExactly(Of AccesDonneesException)(Function() _rapports.Creer(r))
        ' Aucun rapport partiel ne doit rester
        Assert.IsFalse(_rapports.ListerParPerimetre(Perimetre.DuCollaborateur("a131"), Date.Today.AddDays(-2), True).Any(Function(x) x.Praticien.StartsWith("Durand") AndAlso x.DateVisite = Date.Today.AddDays(-1)))
    End Sub

    <TestMethod>
    Public Sub Creer_DateFuture_RefuseeParLaBase()
        Dim r = NouveauBrouillon()
        r.DateVisite = Date.Today.AddDays(3)

        Assert.ThrowsExactly(Of AccesDonneesException)(Function() _rapports.Creer(r))
    End Sub

    <TestMethod>
    Public Sub ListerParAuteur_TroisAns_ExclutLesVieuxRapports()
        Dim liste = _rapports.ListerParPerimetre(Perimetre.DuCollaborateur("a131"), Date.Today.AddYears(-3), True)

        Assert.IsTrue(liste.Any(Function(x) x.Numero = 5))
        Assert.IsFalse(liste.Any(Function(x) x.Numero = 1), "Le CR de 2022 ne doit plus apparaître.")
        Assert.AreEqual("Petit Camille", liste.Single(Function(x) x.Numero = 5).Remplacant)
    End Sub

    <TestMethod>
    Public Sub AjouterSessionSaisie_EnregistreLaSession()
        Dim numero = _rapports.Creer(NouveauBrouillon())
        _crees.Add(numero)

        _rapports.AjouterSessionSaisie(numero, "a131", DateTime.Now.AddMinutes(-5), DateTime.Now)
    End Sub

    <TestMethod>
    Public Sub Referentiels_PortefeuilleMotifsMedicaments()
        Dim portefeuille = _referentiel.ListerPortefeuille("a131")
        CollectionAssert.AreEquivalent({1, 2}, portefeuille.Select(Function(p) p.Numero.Value).ToList())

        Assert.AreEqual("AUTRE", _referentiel.ListerMotifs().Last().Code)
        Assert.HasCount(11, _referentiel.ListerMedicaments())
        Assert.IsTrue(_referentiel.RechercherPraticiens("pet", 50).Any(Function(p) p.Nom = "Petit"))
        Assert.AreEqual("Durand", _referentiel.TrouverPraticien(2).Nom)
        Assert.HasCount(5, _referentiel.ListerTypesPraticien())
    End Sub

    <TestMethod>
    Public Sub CreerPraticien_RemplacantSansAdresse()
        Dim numero = _referentiel.CreerPraticien(New Praticien() With {.Nom = "Test", .Prenom = "Remplaçant", .CodeType = "MV"})
        Try
            Dim p = _referentiel.TrouverPraticien(numero)
            Assert.AreEqual("Test", p.Nom)
            Assert.IsNull(p.Ville)
        Finally
            Using cnx = _connexion.Ouvrir()
                Using cmd As New Oracle.ManagedDataAccess.Client.OracleCommand("delete from PRATICIEN where pra_num = :n", cnx)
                    cmd.Parameters.Add("n", numero)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Try
    End Sub

End Class
