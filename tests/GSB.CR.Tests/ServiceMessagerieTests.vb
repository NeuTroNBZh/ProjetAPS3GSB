Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ServiceMessagerieTests

    Private _dao As FauxMessagerieDao
    Private _service As ServiceMessagerie

    Private Shared ReadOnly Delegue As New UtilisateurConnecte(New Collaborateur() With {.Matricule = "d01"},
        New Affectation() With {.Profil = Profil.Delegue, .CodeRegion = "AQU", .CodeSecteur = "O"})
    Private Shared ReadOnly Visiteur As New UtilisateurConnecte(New Collaborateur() With {.Matricule = "a17"},
        New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "AQU", .CodeSecteur = "O"})
    Private Shared ReadOnly Etranger As New UtilisateurConnecte(New Collaborateur() With {.Matricule = "b19"},
        New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "ALS", .CodeSecteur = "E"})

    <TestInitialize>
    Public Sub Initialiser()
        _dao = New FauxMessagerieDao()
        _service = New ServiceMessagerie(_dao)
    End Sub

    <TestMethod>
    Public Sub Envoyer_SansDoublonNiExpediteur()
        Dim erreurs = _service.Envoyer(Delegue, "  Réunion  ", "Jeudi 14 h", {"a131", "a17", "a131", "d01"})

        Assert.IsEmpty(erreurs)
        Dim m = _dao.Messages.Single()
        Assert.AreEqual("Réunion", m.Objet)
        CollectionAssert.AreEquivalent({"a131", "a17"}, m.Destinataires.Select(Function(d) d.Matricule).ToList())
    End Sub

    <TestMethod>
    Public Sub Envoyer_ChampsManquants_TroisErreurs()
        Dim erreurs = _service.Envoyer(Delegue, " ", "", {})

        Assert.HasCount(3, erreurs)
        Assert.IsEmpty(_dao.Messages)
    End Sub

    <TestMethod>
    Public Sub Envoyer_ObjetTropLong_Refuse()
        Assert.IsNotEmpty(_service.Envoyer(Delegue, New String("x"c, 101), "Contenu", {"a17"}))
    End Sub

    <TestMethod>
    Public Sub Envoyer_DestinataireHorsPoste_Refuse()
        Dim erreurs = _service.Envoyer(Delegue, "Objet", "Contenu", {"a17", "c14"})

        StringAssert.Contains(erreurs.Single(), "plus en poste")
        Assert.IsEmpty(_dao.Messages)
    End Sub

    <TestMethod>
    Public Sub Lire_ParUnDestinataire_MarqueLu()
        _service.Envoyer(Delegue, "Objet", "Contenu", {"a17"})
        Assert.AreEqual(1, _service.NombreNonLus(Visiteur))

        _service.Lire(Visiteur, 1)

        Assert.AreEqual(0, _service.NombreNonLus(Visiteur))
    End Sub

    <TestMethod>
    Public Sub Lire_ParLExpediteur_AutoriseSansMarquer()
        _service.Envoyer(Delegue, "Objet", "Contenu", {"a17"})

        _service.Lire(Delegue, 1)

        Assert.AreEqual(1, _service.NombreNonLus(Visiteur))
    End Sub

    <TestMethod>
    Public Sub Lire_ParUnTiers_Refuse()
        _service.Envoyer(Delegue, "Objet", "Contenu", {"a17"})

        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.Lire(Etranger, 1))
    End Sub

    <TestMethod>
    Public Sub Annuaire_SansLUtilisateur()
        Assert.IsFalse(_service.Annuaire(Delegue).Any(Function(c) c.Matricule = "d01"))
    End Sub

    <TestMethod>
    Public Sub Groupes_RegionEtSecteurDeLUtilisateurEnPremier()
        Dim groupes = ServiceMessagerie.Groupes(_dao.ListerAnnuaire(), Etranger)

        Assert.AreEqual("Région Alsace-Lorraine", groupes(0).Libelle)
        Assert.AreEqual("Secteur Est", groupes(1).Libelle)
        Dim ouest = groupes.Single(Function(g) g.Libelle = "Secteur Ouest")
        CollectionAssert.AreEquivalent({"d01", "a131", "a17", "r02"}, ouest.Matricules, "Le responsable fait partie du secteur")
        CollectionAssert.AreEquivalent({"d01", "a131", "a17"}, groupes.Single(Function(g) g.Libelle = "Région Aquitaine").Matricules)
    End Sub

    <TestMethod>
    <DataRow("Réunion", "RE : Réunion")>
    <DataRow("RE : Réunion", "RE : Réunion")>
    Public Sub ObjetReponse_PrefixeUneSeuleFois(objet As String, attendu As String)
        Assert.AreEqual(attendu, ServiceMessagerie.ObjetReponse(objet))
    End Sub

End Class
