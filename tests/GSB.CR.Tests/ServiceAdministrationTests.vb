Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ServiceAdministrationTests

    Private Shared ReadOnly Aujourdhui As Date = #2026-09-28#

    Private _dao As FauxAdministrationDao
    Private _service As ServiceAdministration

    Private Shared ReadOnly Admin As New UtilisateurConnecte(New Collaborateur() With {.Matricule = "adm1"},
                                                             New Affectation() With {.Profil = Profil.Administrateur})
    Private Shared ReadOnly Visiteur As New UtilisateurConnecte(New Collaborateur() With {.Matricule = "a131"},
                                                                New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "AQU"})

    <TestInitialize>
    Public Sub Initialiser()
        _dao = New FauxAdministrationDao()
        _service = New ServiceAdministration(_dao, New FauxReferentielDao(), New FauxConsultationDao(), New HorlogeFixe(Aujourdhui))
    End Sub

    Private Shared Function Nouveau() As Collaborateur
        Return New Collaborateur() With {.Matricule = " A200 ", .Nom = "Martin", .Prenom = "Léa", .Login = "LMartin",
                                         .DateEmbauche = #2026-09-01#, .CodePostal = "33000", .Email = "lea.martin@gsb.fr"}
    End Function

    Private Shared ReadOnly VisiteurAquitaine As New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "AQU"}

    <TestMethod>
    Public Sub Acces_ReserveAuxAdministrateurs()
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.Collaborateurs(Visiteur))
    End Sub

    ' --- Création ------------------------------------------------------------

    <TestMethod>
    Public Sub Creer_MotDePasseProvisoireRobusteEtHache()
        Dim c = Nouveau()
        Dim r = _service.CreerCollaborateur(Admin, c, VisiteurAquitaine)

        Assert.IsTrue(r.Reussi, String.Join(" | ", r.Erreurs))
        Assert.AreEqual("a200", c.Matricule, "Matricule normalisé")
        Assert.AreEqual("lmartin", c.Login, "Login normalisé")
        Assert.IsEmpty(PolitiqueMotDePasse.Verifier(r.MotDePasseProvisoire))
        Assert.IsTrue(HacheurMotDePasse.Verifier(r.MotDePasseProvisoire, _dao.DernierMotDePasseHache), "Seul le haché est enregistré")
    End Sub

    <TestMethod>
    Public Sub Creer_ChampsInvalides_Refuse()
        Dim c As New Collaborateur() With {.Matricule = "trop-long-matricule", .Nom = "", .Prenom = "X", .Login = "a",
                                           .DateEmbauche = Aujourdhui.AddDays(3), .CodePostal = "330", .Email = "pas-un-mail"}
        Dim r = _service.CreerCollaborateur(Admin, c, New Affectation() With {.Profil = Profil.Delegue})

        Assert.IsFalse(r.Reussi)
        Assert.HasCount(7, r.Erreurs, String.Join(" | ", r.Erreurs))
        Assert.IsEmpty(_dao.Operations)
    End Sub

    <TestMethod>
    Public Sub Creer_ResponsableSansSecteur_Refuse()
        Dim r = _service.CreerCollaborateur(Admin, Nouveau(), New Affectation() With {.Profil = Profil.Responsable})

        StringAssert.Contains(r.Erreurs.Single(), "secteur")
    End Sub

    <TestMethod>
    Public Sub Creer_Doublon_MessageExplicite()
        _dao.DoublonAuProchainAppel = True

        Dim r = _service.CreerCollaborateur(Admin, Nouveau(), VisiteurAquitaine)

        StringAssert.Contains(r.Erreurs.Single(), "déjà utilisée")
        Assert.IsNull(r.MotDePasseProvisoire)
    End Sub

    ' --- Affectation, départ, compte --------------------------------------------

    <TestMethod>
    Public Sub ChangerAffectation_DateAvantAffectationActuelle_Refuse()
        Dim r = _service.ChangerAffectation(Admin, "a131", New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "BRE"}, #2019-01-01#)

        StringAssert.Contains(r.Erreurs.Single(), "commencer après")
    End Sub

    <TestMethod>
    Public Sub ChangerAffectation_Identique_Refuse()
        Dim r = _service.ChangerAffectation(Admin, "a131", VisiteurAquitaine, #2026-10-01#)

        StringAssert.Contains(r.Erreurs.Single(), "identique")
    End Sub

    <TestMethod>
    Public Sub ChangerAffectation_Promotion_Acceptee()
        Dim r = _service.ChangerAffectation(Admin, "a131", New Affectation() With {.Profil = Profil.Delegue, .CodeRegion = "BRE"}, #2026-10-01#)

        Assert.IsTrue(r.Reussi)
        Assert.AreEqual("affectation a131 DEL 2026-10-01", _dao.Operations.Single())
    End Sub

    <TestMethod>
    Public Sub Admin_NePeutPasSeRetirerSesDroitsNiPartirNiSeVerrouiller()
        Assert.IsFalse(_service.ChangerAffectation(Admin, "adm1", VisiteurAquitaine, #2026-10-01#).Reussi)
        Assert.IsFalse(_service.EnregistrerDepart(Admin, "adm1", Aujourdhui).Reussi)
        Assert.IsFalse(_service.DefinirVerrouillage(Admin, "adm1", True).Reussi)
        Assert.IsEmpty(_dao.Operations)
    End Sub

    <TestMethod>
    Public Sub Depart_SignaleLePortefeuilleAReattribuer()
        Dim r = _service.EnregistrerDepart(Admin, "a131", Aujourdhui)

        Assert.IsTrue(r.Reussi)
        StringAssert.Contains(r.Message, "2 praticien(s)")
    End Sub

    <TestMethod>
    Public Sub Depart_DejaParti_OuDateFuture_Refuse()
        Assert.IsFalse(_service.EnregistrerDepart(Admin, "c14", Aujourdhui).Reussi)
        Assert.IsFalse(_service.EnregistrerDepart(Admin, "a131", Aujourdhui.AddDays(1)).Reussi)
    End Sub

    <TestMethod>
    Public Sub Reinitialiser_NouveauMotDePasseProvisoire()
        Dim r = _service.ReinitialiserMotDePasse(Admin, "a131")

        Assert.IsTrue(HacheurMotDePasse.Verifier(r.MotDePasseProvisoire, _dao.DernierMotDePasseHache))
        Assert.IsFalse(_service.ReinitialiserMotDePasse(Admin, "c14").Reussi, "Collaborateur parti")
    End Sub

    ' --- Portefeuilles -----------------------------------------------------------

    <TestMethod>
    Public Sub Portefeuille_SeulementAUnVisiteurOuDelegueEnPoste()
        Assert.IsTrue(_service.AttribuerPraticiens(Admin, {5, 5, 10}, "d01").Reussi)
        Assert.AreEqual("attribuer 5,10 d01", _dao.Operations.Single(), "Doublons retirés")
        Assert.IsFalse(_service.AttribuerPraticiens(Admin, {5}, "c14").Reussi, "Collaborateur parti")
        Assert.IsFalse(_service.AttribuerPraticiens(Admin, {5}, "adm1").Reussi, "Administrateur")
    End Sub

    <TestMethod>
    Public Sub TransfererPortefeuille_CompteLesPraticiens()
        Dim r = _service.TransfererPortefeuille(Admin, "a131", "d01")

        StringAssert.Contains(r.Message, "2 praticien(s)")
        Assert.IsFalse(_service.TransfererPortefeuille(Admin, "d01", "d01").Reussi)
    End Sub

    ' --- Référentiels ------------------------------------------------------------

    <TestMethod>
    Public Sub Motif_CodeEnMajusculesEtAutreProtege()
        Assert.IsTrue(_service.CreerMotif(Admin, " salon ", "Salon professionnel").Reussi)
        Assert.AreEqual("motif SALON", _dao.Operations.Single())
        Assert.IsFalse(_service.CreerMotif(Admin, "AUTRE", "Doublon").Reussi)
        Assert.IsFalse(_service.ModifierMotif(Admin, "AUTRE", "Autre", actif:=False).Reussi)
    End Sub

    <TestMethod>
    <DataRow(-1.0)>
    <DataRow(1000000.0)>
    Public Sub Medicament_PrixHorsBornes_Refuse(prix As Double)
        Assert.IsFalse(_service.ModifierMedicament(Admin, "NOVEL26", CDec(prix), True).Reussi)
    End Sub

    <TestMethod>
    Public Sub Praticien_Creation_NumeroAttribue()
        Dim p As New Praticien() With {.Nom = "Nouveau", .Prenom = "Praticien", .CodeType = "MV", .CodePostal = "33000"}

        Dim r = _service.EnregistrerPraticien(Admin, p)

        Assert.IsTrue(r.Reussi)
        Assert.IsTrue(p.Numero.HasValue)
    End Sub

    <TestMethod>
    Public Sub Praticien_CodePostalInvalide_Refuse()
        Dim p As New Praticien() With {.Numero = 1, .Nom = "Martin", .Prenom = "Hélène", .CodeType = "MV", .CodePostal = "ABCDE"}

        Assert.IsFalse(_service.EnregistrerPraticien(Admin, p).Reussi)
    End Sub

End Class

<TestClass>
Public Class GenerateurMotDePasseTests

    <TestMethod>
    Public Sub Generer_ToujoursConformeALaPolitique()
        For i = 1 To 1000
            Dim mdp = GenerateurMotDePasse.Generer()
            Assert.HasCount(12, mdp)
            Assert.IsEmpty(PolitiqueMotDePasse.Verifier(mdp), mdp)
        Next
    End Sub

    <TestMethod>
    Public Sub Generer_SansCaracteresAmbigus()
        Dim tous = String.Concat(Enumerable.Range(0, 300).Select(Function(i) GenerateurMotDePasse.Generer()))
        Assert.AreEqual(-1, tous.IndexOfAny({"0"c, "O"c, "l"c, "1"c, "I"c}), "Aucun caractère ambigu attendu")
    End Sub

    <TestMethod>
    Public Sub Generer_TropCourt_Refuse()
        Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Function() GenerateurMotDePasse.Generer(6))
    End Sub

End Class
