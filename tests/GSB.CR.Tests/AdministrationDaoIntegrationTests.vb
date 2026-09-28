Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Tests d'intégration de l'administration sur la base réelle. Chaque test travaille sur des données
''' de test qu'il supprime ou remet en état à la fin. Exclus de la CI.
''' </summary>
<TestClass>
<TestCategory("Integration")>
<DoNotParallelize>
Public Class AdministrationDaoIntegrationTests

    Private Const Matricule As String = "tst1"
    Private _connexion As ConnexionOracle
    Private _dao As AdministrationDao

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _connexion = New ConnexionOracle(config)
        _dao = New AdministrationDao(_connexion)
        Nettoyer()
    End Sub

    <TestCleanup>
    Public Sub Nettoyer()
        If _connexion Is Nothing Then Return
        Executer("delete from PORTEFEUILLE where col_matricule = 'tst1' or col_matricule = 'tst2'")
        Executer("delete from AFFECTATION where col_matricule in ('tst1', 'tst2')")
        Executer("delete from COLLABORATEUR where col_matricule in ('tst1', 'tst2')")
        Executer("delete from MOTIF where mot_code = 'TSTMOT'")
        Executer("update MOTIF set mot_ordre = 9 where mot_code = 'AUTRE'")
    End Sub

    Private Sub Executer(sql As String)
        Using cnx = _connexion.Ouvrir()
            Using cmd As New Oracle.ManagedDataAccess.Client.OracleCommand(sql, cnx)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub CreerTest(matricule As String, login As String, region As String, Optional embauche As Date? = Nothing)
        _dao.CreerCollaborateur(
            New Collaborateur() With {.Matricule = matricule, .Nom = "Test", .Prenom = matricule, .Login = login,
                                      .DateEmbauche = If(embauche, #2026-01-05#)},
            "PBKDF2-SHA256$1$AAAA$AAAA",
            New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = region})
    End Sub

    <TestMethod>
    Public Sub Organisation_RegionsEtSecteurs()
        Assert.HasCount(11, _dao.ListerRegions())
        Assert.HasCount(5, _dao.ListerSecteurs())
    End Sub

    <TestMethod>
    Public Sub Collaborateurs_JeuDEssai()
        Dim liste = _dao.ListerCollaborateurs()

        Dim daburon = liste.Single(Function(c) c.Collaborateur.Matricule = "c14")
        Assert.IsNull(daburon.AffectationEnCours, "Parti : plus d'affectation en cours")
        Assert.AreEqual(2, liste.Single(Function(c) c.Collaborateur.Matricule = "a131").TaillePortefeuille)
        Assert.HasCount(3, _dao.ListerAffectations("a17"), "Aquitaine, Bretagne, Aquitaine")
    End Sub

    <TestMethod>
    Public Sub CycleDeVie_CreationAffectationMotDePasseDepart()
        CreerTest(Matricule, "test.cycle", "AQU")
        Dim fiche = _dao.ListerCollaborateurs().Single(Function(c) c.Collaborateur.Matricule = Matricule)
        Assert.IsTrue(fiche.Collaborateur.MotDePasseAChanger)
        Assert.AreEqual("AQU", fiche.AffectationEnCours.CodeRegion)

        _dao.ChangerAffectation(Matricule, New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "BRE"}, #2026-06-01#)
        Dim historique = _dao.ListerAffectations(Matricule)
        Assert.HasCount(2, historique)
        Assert.AreEqual(#2026-05-31#, historique(1).DateFin)
        Assert.AreEqual("BRE", historique(0).CodeRegion)

        _dao.DefinirVerrouillage(Matricule, True)
        Assert.IsTrue(_dao.ListerCollaborateurs().Single(Function(c) c.Collaborateur.Matricule = Matricule).Collaborateur.Verrouille)
        _dao.ReinitialiserMotDePasse(Matricule, "PBKDF2-SHA256$1$BBBB$BBBB")
        Assert.IsFalse(_dao.ListerCollaborateurs().Single(Function(c) c.Collaborateur.Matricule = Matricule).Collaborateur.Verrouille,
                       "La réinitialisation déverrouille le compte")

        _dao.EnregistrerDepart(Matricule, #2026-09-01#)
        fiche = _dao.ListerCollaborateurs().Single(Function(c) c.Collaborateur.Matricule = Matricule)
        Assert.AreEqual(#2026-09-01#, fiche.Collaborateur.DateDepart)
        Assert.IsNull(fiche.AffectationEnCours)
    End Sub

    <TestMethod>
    Public Sub Creation_LoginEnDouble_Doublon()
        Dim ex = Assert.ThrowsExactly(Of AccesDonneesException)(Sub() CreerTest(Matricule, "lvillechalane", "AQU"))

        Assert.IsTrue(ex.EstDoublon)
        Assert.IsFalse(_dao.ListerCollaborateurs().Any(Function(c) c.Collaborateur.Matricule = Matricule), "Transaction annulée")
    End Sub

    <TestMethod>
    Public Sub Affectation_SecondDelegueDansUneRegion_Doublon()
        CreerTest(Matricule, "test.delegue", "AQU")

        Dim ex = Assert.ThrowsExactly(Of AccesDonneesException)(
            Sub() _dao.ChangerAffectation(Matricule, New Affectation() With {.Profil = Profil.Delegue, .CodeRegion = "AQU"}, #2026-06-01#))

        Assert.IsTrue(ex.EstDoublon, "Christian Bedos est déjà délégué d'Aquitaine")
        Assert.HasCount(1, _dao.ListerAffectations(Matricule), "Transaction annulée")
    End Sub

    <TestMethod>
    Public Sub Portefeuille_AttributionPuisTransfert()
        CreerTest(Matricule, "test.ptf1", "AQU")
        CreerTest("tst2", "test.ptf2", "AQU")
        Dim sansVisiteur = _dao.ListerPraticiensSansVisiteur()
        Assert.IsTrue(sansVisiteur.Any(Function(p) p.Nom = "Petit"), "Les remplaçants n'ont pas de visiteur")
        Dim numero = sansVisiteur.First().Numero

        _dao.AttribuerPraticiens({numero}, Matricule, #2026-09-01#)
        _dao.AttribuerPraticiens({numero}, Matricule, #2026-09-01#)   ' même jour : remplacement, pas de doublon
        Assert.IsFalse(_dao.ListerPraticiensSansVisiteur().Any(Function(p) p.Numero = numero))

        Assert.AreEqual(1, _dao.TransfererPortefeuille(Matricule, "tst2", #2026-09-15#))
        Dim fiches = _dao.ListerCollaborateurs()
        Assert.AreEqual(0, fiches.Single(Function(c) c.Collaborateur.Matricule = Matricule).TaillePortefeuille)
        Assert.AreEqual(1, fiches.Single(Function(c) c.Collaborateur.Matricule = "tst2").TaillePortefeuille)
    End Sub

    <TestMethod>
    Public Sub Motif_CreePlaceAvantAutre()
        _dao.CreerMotif("TSTMOT", "Motif de test")
        Dim motifs = New ReferentielDao(_connexion).ListerMotifs()

        Assert.AreEqual("AUTRE", motifs.Last().Code, "« Autre » reste en dernier")
        Assert.AreEqual("TSTMOT", motifs(motifs.Count - 2).Code)
        _dao.ModifierMotif("TSTMOT", "Motif renommé", False)
        Assert.IsFalse(New ReferentielDao(_connexion).ListerMotifs().Single(Function(m) m.Code = "TSTMOT").Actif)
    End Sub

    <TestMethod>
    Public Sub Medicament_ModifierPuisRestaurer()
        Try
            _dao.ModifierMedicament("INSXT5", 0.75D, False)
            Dim m = New ReferentielDao(_connexion).ListerMedicaments().Single(Function(x) x.DepotLegal = "INSXT5")
            Assert.AreEqual(0.75D, m.PrixEchantillon)
            Assert.IsFalse(m.Actif)
        Finally
            _dao.ModifierMedicament("INSXT5", 0.7D, True)
        End Try
    End Sub

    <TestMethod>
    Public Sub Praticien_ModifierPuisRestaurer()
        Dim referentiel As New ReferentielDao(_connexion)
        Dim origine = referentiel.TrouverPraticien(9)
        Try
            Dim modifie = referentiel.TrouverPraticien(9)
            modifie.Telephone = "0102030405"
            modifie.Actif = False
            _dao.ModifierPraticien(modifie)
            Dim relu = referentiel.TrouverPraticien(9)
            Assert.AreEqual("0102030405", relu.Telephone)
            Assert.IsFalse(relu.Actif)
        Finally
            _dao.ModifierPraticien(origine)
        End Try
    End Sub

    <TestMethod>
    Public Sub Journal_FiltresEchecsEtLogin()
        Dim echecs = _dao.ListerJournal(#2026-09-01#, #2026-09-30#, "", echecsSeulement:=True, maximum:=1000)

        Assert.IsTrue(echecs.All(Function(e) Not e.Succes))
        Dim inconnu = echecs.Single(Function(e) e.LoginSaisi = "inconnu")
        Assert.IsNull(inconnu.NomCollaborateur)
        Assert.IsTrue(_dao.ListerJournal(#2026-09-01#, #2026-09-30#, "bentot", False, 1000).All(Function(e) e.LoginSaisi = "pbentot"))
    End Sub

End Class
