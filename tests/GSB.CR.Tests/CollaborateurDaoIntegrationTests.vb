Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Tests d'intégration sur la base réelle (jeu d'essai bdd/05_jeu_essai.sql).
''' Exclus de la CI (catégorie Integration).
''' </summary>
<TestClass>
<TestCategory("Integration")>
Public Class CollaborateurDaoIntegrationTests

    Private _dao As CollaborateurDao

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _dao = New CollaborateurDao(New ConnexionOracle(config))
    End Sub

    <TestMethod>
    Public Sub TrouverParLogin_VisiteurExistant_RenvoieSesDonnees()
        Dim c = _dao.TrouverParLogin("LVillechalane")   ' casse différente volontairement

        Assert.IsNotNull(c)
        Assert.AreEqual("a131", c.Matricule)
        Assert.AreEqual("Villechalane", c.Nom)
        StringAssert.StartsWith(c.MotDePasseHache, "PBKDF2-SHA256$")
        Assert.IsFalse(c.EstParti(Date.Today))
    End Sub

    <TestMethod>
    Public Sub TrouverParLogin_Inconnu_RenvoieNothing()
        Assert.IsNull(_dao.TrouverParLogin("personne"))
    End Sub

    <TestMethod>
    Public Sub TrouverAffectationEnCours_Visiteur_RegionEtSecteur()
        Dim a = _dao.TrouverAffectationEnCours("a131")

        Assert.IsNotNull(a)
        Assert.AreEqual(Profil.Visiteur, a.Profil)
        Assert.AreEqual("AQU", a.CodeRegion)
        Assert.AreEqual("O", a.CodeSecteur)
    End Sub

    <TestMethod>
    Public Sub TrouverAffectationEnCours_Responsable_SecteurSansRegion()
        Dim a = _dao.TrouverAffectationEnCours("r01")

        Assert.AreEqual(Profil.Responsable, a.Profil)
        Assert.IsNull(a.CodeRegion)
        Assert.AreEqual("E", a.CodeSecteur)
    End Sub

    <TestMethod>
    Public Sub TrouverAffectationEnCours_CollaborateurParti_RenvoieNothing()
        Assert.IsNull(_dao.TrouverAffectationEnCours("c14"))
    End Sub

    <TestMethod>
    Public Sub EnregistrerEchecPuisSucces_CompteurIncrementePuisRemisAZero()
        ' Compte d'un collaborateur parti : sans impact sur les autres tests
        Dim nb = _dao.EnregistrerEchec("c14", maxEchecs:=99)
        Assert.IsGreaterThanOrEqualTo(1, nb)

        _dao.EnregistrerSucces("c14")

        Assert.AreEqual(0, _dao.TrouverParLogin("fdaburon").NbEchecsConnexion)
    End Sub

End Class
