Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ServiceEquipeTests

    Private Shared ReadOnly Aujourdhui As Date = #2026-09-28#

    Private _activite As FauxActiviteDao
    Private _rapports As FauxRapportDao
    Private _equipe As FauxEquipeDao
    Private _consultation As FauxConsultationDao
    Private _service As ServiceEquipe

    <TestInitialize>
    Public Sub Initialiser()
        _activite = New FauxActiviteDao()
        _rapports = New FauxRapportDao()
        _equipe = New FauxEquipeDao()
        _consultation = New FauxConsultationDao()
        _service = New ServiceEquipe(_activite, _rapports, _consultation, _equipe, New HorlogeFixe(Aujourdhui))
    End Sub

    Private Shared ReadOnly Delegue As New UtilisateurConnecte(
        New Collaborateur() With {.Matricule = "d01"},
        New Affectation() With {.Profil = Profil.Delegue, .CodeRegion = "AQU", .NomRegion = "Aquitaine", .CodeSecteur = "O"})
    Private Shared ReadOnly Responsable As New UtilisateurConnecte(
        New Collaborateur() With {.Matricule = "r02"},
        New Affectation() With {.Profil = Profil.Responsable, .CodeSecteur = "O", .LibelleSecteur = "Ouest"})
    Private Shared ReadOnly Visiteur As New UtilisateurConnecte(
        New Collaborateur() With {.Matricule = "a131"},
        New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "AQU"})

    Private Function RapportDe(auteur As String, etat As EtatRapport) As Integer
        Dim n = _rapports.Creer(New RapportVisite() With {.MatriculeAuteur = auteur, .NumeroPraticien = 1,
                                                          .DateVisite = Aujourdhui.AddDays(-3), .Etat = etat})
        Return n
    End Function

    <TestMethod>
    Public Sub Perimetre_DelegueSaRegion_ResponsableSonSecteur()
        Assert.AreEqual(TypePerimetre.Region, _service.PerimetreDe(Delegue).Type)
        Assert.AreEqual("AQU", _service.PerimetreDe(Delegue).Code)
        Assert.AreEqual(TypePerimetre.Secteur, _service.PerimetreDe(Responsable).Type)
        Assert.AreEqual("Secteur Ouest", _service.PerimetreDe(Responsable).Libelle)
    End Sub

    <TestMethod>
    Public Sub Visiteur_PasDEquipe()
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.PerimetreDe(Visiteur))
    End Sub

    <TestMethod>
    Public Sub Synthese_SurLePerimetreEtMoisCompletes()
        Dim s = _service.Synthese(Delegue, #2026-07-01#, #2026-09-28#)

        Assert.AreEqual(TypePerimetre.Region, _activite.DernierPerimetre.Type)
        Assert.HasCount(3, s.ParMois)
    End Sub

    <TestMethod>
    Public Sub Synthese_PeriodeTropAncienne_Refusee()
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.Synthese(Delegue, #2020-01-01#, #2026-09-28#))
    End Sub

    <TestMethod>
    Public Sub SyntheseMembre_HorsEquipe_Refusee()
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.SyntheseMembre(Delegue, "b13", #2026-07-01#, #2026-09-28#))
        _service.SyntheseMembre(Delegue, "a17", #2026-07-01#, #2026-09-28#)
        Assert.AreEqual(TypePerimetre.Collaborateur, _activite.DernierPerimetre.Type)
        Assert.AreEqual("a17", _activite.DernierPerimetre.Code)
    End Sub

    <TestMethod>
    Public Sub EX32_ChargerRapport_ValideDUnMembre_Accepte()
        Dim n = RapportDe("a17", EtatRapport.Valide)

        Assert.AreEqual("a17", _service.ChargerRapport(Delegue, n).MatriculeAuteur)
    End Sub

    <TestMethod>
    Public Sub EX32_ChargerRapport_BrouillonDUnMembre_Refuse()
        Dim n = RapportDe("a17", EtatRapport.Brouillon)

        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.ChargerRapport(Delegue, n))
    End Sub

    <TestMethod>
    Public Sub EX32_ChargerRapport_HorsEquipe_Refuse()
        Dim n = RapportDe("b13", EtatRapport.Valide)

        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.ChargerRapport(Delegue, n))
    End Sub

    <TestMethod>
    Public Sub EX35_PraticiensARevoir_SurLePerimetre()
        _service.PraticiensARevoir(Delegue)

        Assert.AreEqual(TypePerimetre.Region, _consultation.DernierPerimetre.Type)
    End Sub

End Class
