Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ServiceConsultationTests

    Private _dao As FauxConsultationDao
    Private _service As ServiceConsultation

    <TestInitialize>
    Public Sub Initialiser()
        _dao = New FauxConsultationDao()
        _service = New ServiceConsultation(_dao, New FauxReferentielDao(), New HorlogeFixe(#2026-09-28#))
    End Sub

    Private Shared Function Utilisateur(profil As Profil) As UtilisateurConnecte
        Return New UtilisateurConnecte(New Collaborateur() With {.Matricule = "m1"}, New Affectation() With {.Profil = profil})
    End Function

    <TestMethod>
    <DataRow(Profil.Visiteur)>
    <DataRow(Profil.Delegue)>
    Public Sub MonPortefeuille_FiltreSurLeMatricule(profil As Profil)
        _service.RechercherPraticiens(Utilisateur(profil), "", seulementMonPortefeuille:=True, inclureInactifs:=False)

        Assert.AreEqual("m1", _dao.DernierMatricule)
    End Sub

    <TestMethod>
    <DataRow(Profil.Responsable)>
    <DataRow(Profil.Administrateur)>
    Public Sub MonPortefeuille_IgnorePourLesProfilsSansPortefeuille(profil As Profil)
        _service.RechercherPraticiens(Utilisateur(profil), "", seulementMonPortefeuille:=True, inclureInactifs:=True)

        Assert.IsNull(_dao.DernierMatricule)
        Assert.IsTrue(_dao.DernierInclureInactifs)
    End Sub

    <TestMethod>
    Public Sub TousLesProfils_AccedentAuxFiches()
        For Each p In {Profil.Visiteur, Profil.Delegue, Profil.Responsable, Profil.Administrateur}
            Assert.AreEqual("Martin", _service.FichePraticien(Utilisateur(p), 1).Praticien.Nom)
            Assert.AreEqual("NOVEL26", _service.FicheMedicament(Utilisateur(p), "NOVEL26").Medicament.DepotLegal)
        Next
    End Sub

    <TestMethod>
    Public Sub FicheInexistante_ErreurMetier()
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.FichePraticien(Utilisateur(Profil.Visiteur), 42))
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.FicheMedicament(Utilisateur(Profil.Visiteur), "XXX"))
    End Sub

    <TestMethod>
    Public Sub BaseEnPanne_ErreurMetierAvecMessageServeur()
        _dao.EnPanne = True

        Dim ex = Assert.ThrowsExactly(Of ErreurMetierException)(
            Function() _service.RechercherPraticiens(Utilisateur(Profil.Visiteur), "", False, False))
        StringAssert.Contains(ex.Message, "serveur")
    End Sub

    <TestMethod>
    Public Sub Periodicite_UtiliseLaDateDuJour()
        Assert.AreEqual(EtatPeriodicite.ARevoir, _service.Periodicite(#2025-11-12#))
        Assert.AreEqual(EtatPeriodicite.AJour, _service.Periodicite(#2026-09-10#))
    End Sub

End Class
