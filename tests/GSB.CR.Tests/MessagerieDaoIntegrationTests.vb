Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>Tests d'intégration de la messagerie sur le jeu d'essai. Les messages créés sont supprimés. Exclus de la CI.</summary>
<TestClass>
<TestCategory("Integration")>
<DoNotParallelize>
Public Class MessagerieDaoIntegrationTests

    Private _connexion As ConnexionOracle
    Private _dao As MessagerieDao
    Private ReadOnly _crees As New List(Of Integer)

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _connexion = New ConnexionOracle(config)
        _dao = New MessagerieDao(_connexion)
    End Sub

    <TestCleanup>
    Public Sub Nettoyer()
        If _connexion Is Nothing Then Return
        Using cnx = _connexion.Ouvrir()
            For Each id In _crees
                Using cmd As New Oracle.ManagedDataAccess.Client.OracleCommand("delete from MESSAGE where msg_id = :id", cnx)
                    cmd.Parameters.Add("id", id)
                    cmd.ExecuteNonQuery()
                End Using
            Next
        End Using
    End Sub

    <TestMethod>
    Public Sub Annuaire_TousLesCollaborateursEnPoste()
        Dim annuaire = _dao.ListerAnnuaire()

        Assert.HasCount(13, annuaire, "14 comptes dont F. Daburon, parti")
        Dim lemoine = annuaire.Single(Function(c) c.Matricule = "r01")
        Assert.AreEqual(Profil.Responsable, lemoine.Profil)
        Assert.AreEqual("E", lemoine.CodeSecteur)
        Assert.AreEqual("O", annuaire.Single(Function(c) c.Matricule = "a131").CodeSecteur)
    End Sub

    <TestMethod>
    Public Sub JeuDEssai_BoiteDeDeniseBunisset_UnMessageNonLu()
        Dim recus = _dao.ListerRecus("b25")

        Assert.AreEqual("Lancement NOVELIX", recus.Single().Objet)
        Assert.IsFalse(recus.Single().Lu)
        Assert.AreEqual("Claire Lemoine", recus.Single().Correspondant)
    End Sub

    <TestMethod>
    Public Sub JeuDEssai_EnvoyesDeClaireLemoine_SuiviDeLecture()
        Dim envoye = _dao.ListerEnvoyes("r01").Single()

        Assert.AreEqual(3, envoye.NbDestinataires)
        Assert.AreEqual(2, envoye.NbLus)
    End Sub

    <TestMethod>
    Public Sub Envoyer_PuisLire_PuisCompter()
        Dim avant = _dao.CompterNonLus("a17")
        Dim id = _dao.Envoyer("d01", "Test d'intégration", "Contenu du message", {"a17", "a131"})
        _crees.Add(id)

        Assert.AreEqual(avant + 1, _dao.CompterNonLus("a17"))
        Dim message = _dao.Charger(id)
        Assert.AreEqual("Christian Bedos", message.Expediteur)
        Assert.HasCount(2, message.Destinataires)
        Assert.IsTrue(message.Destinataires.All(Function(d) Not d.DateLecture.HasValue))

        _dao.MarquerLu(id, "a17")
        _dao.MarquerLu(id, "a17")   ' sans effet la deuxième fois

        Assert.AreEqual(avant, _dao.CompterNonLus("a17"))
        Assert.IsTrue(_dao.Charger(id).Destinataires.Single(Function(d) d.Matricule = "a17").DateLecture.HasValue)
        Assert.AreEqual(1, _dao.ListerEnvoyes("d01").Single(Function(m) m.Id = id).NbLus)
    End Sub

    <TestMethod>
    Public Sub Envoyer_DestinataireInconnu_RienNEstEnregistre()
        Dim avant = _dao.ListerEnvoyes("d01").Count

        Assert.ThrowsExactly(Of AccesDonneesException)(Function() _dao.Envoyer("d01", "Objet", "Contenu", {"a17", "inconnu"}))
        Assert.HasCount(avant, _dao.ListerEnvoyes("d01"), "La transaction doit être annulée.")
    End Sub

End Class
