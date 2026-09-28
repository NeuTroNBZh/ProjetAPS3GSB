Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ServiceAuthentificationTests

    Private Const MotDePasse As String = "Gsb2026!"
    Private Shared ReadOnly Aujourdhui As Date = #2026-09-28#
    Private Shared _hache As String

    Private _dao As FauxCollaborateurDao
    Private _service As ServiceAuthentification

    <ClassInitialize>
    Public Shared Sub InitialiserClasse(contexte As TestContext)
        _hache = HacheurMotDePasse.Hacher(MotDePasse)   ' calculé une seule fois (PBKDF2 est volontairement lent)
    End Sub

    <TestInitialize>
    Public Sub Initialiser()
        _dao = New FauxCollaborateurDao()
        _dao.Ajouter(Collab("a131", "lvillechalane"), New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "AQU", .CodeSecteur = "O"})
        _service = New ServiceAuthentification(_dao, New HorlogeFixe(Aujourdhui))
    End Sub

    Private Shared Function Collab(matricule As String, login As String) As Collaborateur
        Return New Collaborateur() With {.Matricule = matricule, .Login = login, .Nom = "Nom", .Prenom = "Prénom",
                                         .MotDePasseHache = _hache, .DateEmbauche = #2020-01-01#}
    End Function

    ' --- Connexion -------------------------------------------------------

    <TestMethod>
    Public Sub Connecter_BonsIdentifiants_Reussie()
        Dim r = _service.Connecter("lvillechalane", MotDePasse)

        Assert.AreEqual(StatutConnexion.Reussie, r.Statut)
        Assert.IsTrue(r.EstAcceptee)
        Assert.AreEqual("a131", r.Utilisateur.Matricule)
        Assert.AreEqual(Profil.Visiteur, r.Utilisateur.Profil)
        Assert.IsTrue(_dao.Journal.Single().Succes)
    End Sub

    <TestMethod>
    Public Sub Connecter_LoginAvecEspacesEtMajuscules_Reussie()
        Assert.AreEqual(StatutConnexion.Reussie, _service.Connecter("  LVillechalane ", MotDePasse).Statut)
    End Sub

    <TestMethod>
    <DataRow("", "x")>
    <DataRow("lvillechalane", "")>
    <DataRow("   ", "x")>
    Public Sub Connecter_ChampVide_RefuseSansInterrogerLaBase(login As String, mdp As String)
        _dao.EnPanne = True   ' la base ne doit même pas être appelée

        Dim r = _service.Connecter(login, mdp)

        Assert.AreEqual(StatutConnexion.IdentifiantsInvalides, r.Statut)
        Assert.IsNull(r.Utilisateur)
    End Sub

    <TestMethod>
    Public Sub Connecter_LoginInconnu_MemeMessageQuUnMauvaisMotDePasse()
        Dim inconnu = _service.Connecter("personne", MotDePasse)
        Dim mauvais = _service.Connecter("lvillechalane", "Mauvais1!")

        Assert.AreEqual(StatutConnexion.IdentifiantsInvalides, inconnu.Statut)
        Assert.AreEqual(inconnu.Message, mauvais.Message)
        Assert.IsNull(_dao.Journal(0).Matricule)
        Assert.IsFalse(_dao.Journal(0).Succes)
    End Sub

    <TestMethod>
    Public Sub Connecter_MauvaisMotDePasse_IncrementeLesEchecs()
        _service.Connecter("lvillechalane", "Mauvais1!")

        Assert.AreEqual(1, _dao.Collaborateurs("lvillechalane").NbEchecsConnexion)
    End Sub

    <TestMethod>
    Public Sub Connecter_CinquiemeEchec_VerrouilleLeCompte()
        Dim r As ResultatConnexion = Nothing
        For i = 1 To ServiceAuthentification.MaxEchecs
            r = _service.Connecter("lvillechalane", "Mauvais1!")
        Next

        Assert.AreEqual(StatutConnexion.CompteVerrouille, r.Statut)
        Assert.IsTrue(_dao.Collaborateurs("lvillechalane").Verrouille)
    End Sub

    <TestMethod>
    Public Sub Connecter_AvantDernierEssai_PrevientDuVerrouillage()
        For i = 1 To ServiceAuthentification.MaxEchecs - 2
            _service.Connecter("lvillechalane", "Mauvais1!")
        Next
        Dim r = _service.Connecter("lvillechalane", "Mauvais1!")

        StringAssert.Contains(r.Message, "encore 1 essai")
    End Sub

    <TestMethod>
    Public Sub Connecter_CompteVerrouille_RefuseMemeAvecLeBonMotDePasse()
        _dao.Collaborateurs("lvillechalane").Verrouille = True

        Dim r = _service.Connecter("lvillechalane", MotDePasse)

        Assert.AreEqual(StatutConnexion.CompteVerrouille, r.Statut)
        Assert.IsNull(r.Utilisateur)
    End Sub

    <TestMethod>
    Public Sub Connecter_Reussie_RemetLesEchecsAZero()
        _service.Connecter("lvillechalane", "Mauvais1!")
        _service.Connecter("lvillechalane", MotDePasse)

        Assert.AreEqual(0, _dao.Collaborateurs("lvillechalane").NbEchecsConnexion)
    End Sub

    <TestMethod>
    Public Sub Connecter_CollaborateurParti_CompteInactif()
        _dao.Collaborateurs("lvillechalane").DateDepart = Aujourdhui.AddDays(-1)

        Assert.AreEqual(StatutConnexion.CompteInactif, _service.Connecter("lvillechalane", MotDePasse).Statut)
    End Sub

    <TestMethod>
    Public Sub Connecter_DepartPrevuDansLeFutur_Reussie()
        _dao.Collaborateurs("lvillechalane").DateDepart = Aujourdhui.AddDays(30)

        Assert.AreEqual(StatutConnexion.Reussie, _service.Connecter("lvillechalane", MotDePasse).Statut)
    End Sub

    <TestMethod>
    Public Sub Connecter_SansAffectation_CompteInactif()
        _dao.Affectations.Clear()

        Assert.AreEqual(StatutConnexion.CompteInactif, _service.Connecter("lvillechalane", MotDePasse).Statut)
    End Sub

    <TestMethod>
    Public Sub Connecter_MotDePasseAChanger_ChangementRequis()
        _dao.Collaborateurs("lvillechalane").MotDePasseAChanger = True

        Dim r = _service.Connecter("lvillechalane", MotDePasse)

        Assert.AreEqual(StatutConnexion.ChangementMotDePasseRequis, r.Statut)
        Assert.IsTrue(r.EstAcceptee)
        Assert.IsNotNull(r.Utilisateur)
    End Sub

    <TestMethod>
    Public Sub Connecter_BaseEnPanne_ServeurIndisponible()
        _dao.EnPanne = True

        Assert.AreEqual(StatutConnexion.ServeurIndisponible, _service.Connecter("lvillechalane", MotDePasse).Statut)
    End Sub

    ' --- Changement de mot de passe ----------------------------------------

    Private Function Connecte() As UtilisateurConnecte
        Return _service.Connecter("lvillechalane", MotDePasse).Utilisateur
    End Function

    <TestMethod>
    Public Sub ChangerMotDePasse_Valide_EnregistreLeNouveauHache()
        Dim u = Connecte()
        u.Collaborateur.MotDePasseAChanger = True

        Dim erreurs = _service.ChangerMotDePasse(u, MotDePasse, "Nouveau#2026", "Nouveau#2026")

        Assert.IsEmpty(erreurs)
        Assert.IsFalse(u.Collaborateur.MotDePasseAChanger)
        Assert.IsTrue(HacheurMotDePasse.Verifier("Nouveau#2026", _dao.Collaborateurs("lvillechalane").MotDePasseHache))
    End Sub

    <TestMethod>
    Public Sub ChangerMotDePasse_AncienIncorrect_Refuse()
        Dim erreurs = _service.ChangerMotDePasse(Connecte(), "Faux#2026", "Nouveau#2026", "Nouveau#2026")

        Assert.IsTrue(erreurs.Any(Function(e) e.Contains("actuel est incorrect")))
    End Sub

    <TestMethod>
    Public Sub ChangerMotDePasse_ConfirmationDifferente_Refuse()
        Dim erreurs = _service.ChangerMotDePasse(Connecte(), MotDePasse, "Nouveau#2026", "Nouveau#2027")

        Assert.IsTrue(erreurs.Any(Function(e) e.Contains("confirmation")))
    End Sub

    <TestMethod>
    Public Sub ChangerMotDePasse_IdentiqueAlAncien_Refuse()
        Dim erreurs = _service.ChangerMotDePasse(Connecte(), MotDePasse, MotDePasse, MotDePasse)

        Assert.IsTrue(erreurs.Any(Function(e) e.Contains("différent")))
    End Sub

    <TestMethod>
    Public Sub ChangerMotDePasse_TropFaible_NeModifiePasLaBase()
        Dim u = Connecte()
        Dim avant = _dao.Collaborateurs("lvillechalane").MotDePasseHache

        Dim erreurs = _service.ChangerMotDePasse(u, MotDePasse, "faible", "faible")

        Assert.IsNotEmpty(erreurs)
        Assert.AreEqual(avant, _dao.Collaborateurs("lvillechalane").MotDePasseHache)
    End Sub

End Class
