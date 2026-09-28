Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class AutorisationsTests

    <TestMethod>
    Public Sub Visiteur_SaisitSesCR_MaisNeGerePasLesEchantillons()
        Assert.IsTrue(Autorisations.PeutAcceder(Profil.Visiteur, ModuleApplication.MesComptesRendus))
        Assert.IsFalse(Autorisations.PeutAcceder(Profil.Visiteur, ModuleApplication.Echantillons))
        Assert.IsFalse(Autorisations.PeutAcceder(Profil.Visiteur, ModuleApplication.ActiviteRegion))
    End Sub

    <TestMethod>
    Public Sub Delegue_EstAussiVisiteur_EtVoitSaRegion()
        For Each m In Autorisations.ModulesAccessibles(Profil.Visiteur)
            Assert.IsTrue(Autorisations.PeutAcceder(Profil.Delegue, m), $"Le délégué doit avoir {m}.")
        Next
        Assert.IsTrue(Autorisations.PeutAcceder(Profil.Delegue, ModuleApplication.ActiviteRegion))
        Assert.IsTrue(Autorisations.PeutAcceder(Profil.Delegue, ModuleApplication.Echantillons))
    End Sub

    <TestMethod>
    Public Sub Responsable_VoitSonSecteur_MaisNeSaisitPasDeCR()
        Assert.IsTrue(Autorisations.PeutAcceder(Profil.Responsable, ModuleApplication.ActiviteSecteur))
        Assert.IsFalse(Autorisations.PeutAcceder(Profil.Responsable, ModuleApplication.MesComptesRendus))
    End Sub

    <TestMethod>
    Public Sub SeulAdministrateur_AccedeAlAdministration()
        Assert.IsTrue(Autorisations.PeutAcceder(Profil.Administrateur, ModuleApplication.Administration))
        For Each p In {Profil.Visiteur, Profil.Delegue, Profil.Responsable}
            Assert.IsFalse(Autorisations.PeutAcceder(p, ModuleApplication.Administration))
        Next
    End Sub

End Class
