Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ServiceEchantillonsTests

    Private Shared ReadOnly Aujourdhui As Date = #2026-09-28#
    Private Shared ReadOnly Septembre As Date = #2026-09-01#

    Private _dao As FauxEchantillonDao
    Private _service As ServiceEchantillons

    Private Shared ReadOnly Delegue As New UtilisateurConnecte(
        New Collaborateur() With {.Matricule = "d01"},
        New Affectation() With {.Profil = Profil.Delegue, .CodeRegion = "AQU", .NomRegion = "Aquitaine"})
    Private Shared ReadOnly Responsable As New UtilisateurConnecte(
        New Collaborateur() With {.Matricule = "r02"},
        New Affectation() With {.Profil = Profil.Responsable, .CodeSecteur = "O", .LibelleSecteur = "Ouest"})
    Private Shared ReadOnly Visiteur As New UtilisateurConnecte(
        New Collaborateur() With {.Matricule = "a131"},
        New Affectation() With {.Profil = Profil.Visiteur, .CodeRegion = "AQU"})

    <TestInitialize>
    Public Sub Initialiser()
        _dao = New FauxEchantillonDao()
        _service = New ServiceEchantillons(_dao, New FauxEquipeDao(), New FauxReferentielDao(), New HorlogeFixe(Aujourdhui))
    End Sub

    <TestMethod>
    Public Sub EX33_Delegue_EnregistreUneDotationPourSaRegion()
        Dim erreurs = _service.EnregistrerDotation(Delegue, "a17", "NOVEL26", Septembre, 10)

        Assert.IsEmpty(erreurs)
        Assert.AreEqual((10, "d01"), _dao.Dotations(("a17", "NOVEL26", Septembre)))
    End Sub

    <TestMethod>
    Public Sub EX33_Responsable_ConsulteMaisNeSaisitPas()
        Assert.IsFalse(ServiceEchantillons.PeutSaisir(Responsable))
        Assert.HasCount(2, _service.Stock(Responsable, Septembre, seulementLesEcarts:=False))

        Dim erreurs = _service.EnregistrerDotation(Responsable, "a17", "NOVEL26", Septembre, 10)

        StringAssert.Contains(erreurs.Single(), "délégué")
        Assert.IsEmpty(_dao.Dotations)
    End Sub

    <TestMethod>
    Public Sub Visiteur_AucunAcces()
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.Stock(Visiteur, Septembre, False))
    End Sub

    <TestMethod>
    Public Sub EX33_VisiteurHorsRegion_Refuse()
        Dim erreurs = _service.EnregistrerDotation(Delegue, "b13", "NOVEL26", Septembre, 10)

        StringAssert.Contains(erreurs.Single(), "votre région")
    End Sub

    <TestMethod>
    <DataRow(0)>
    <DataRow(-5)>
    <DataRow(1_000_000)>
    Public Sub EX33_QuantiteInvalide_Refusee(quantite As Integer)
        Assert.IsNotEmpty(_service.EnregistrerDotation(Delegue, "a17", "NOVEL26", Septembre, quantite))
        Assert.IsEmpty(_dao.Dotations)
    End Sub

    <TestMethod>
    <DataRow("2026-11-01")>
    <DataRow("2025-08-01")>
    Public Sub EX33_MoisHorsFenetre_Refuse(mois As String)
        Dim erreurs = _service.EnregistrerDotation(Delegue, "a17", "NOVEL26", Date.Parse(mois, Globalization.CultureInfo.InvariantCulture), 5)

        StringAssert.Contains(erreurs.Single(), "mois")
    End Sub

    <TestMethod>
    Public Sub EX33_MoisProchain_Accepte()
        Assert.IsEmpty(_service.EnregistrerDotation(Delegue, "a17", "NOVEL26", #2026-10-01#, 5))
    End Sub

    <TestMethod>
    Public Sub MoisProposes_DuMoisProchainADouzeMoisEnArriere()
        Dim mois = _service.MoisProposes()

        Assert.AreEqual(#2026-10-01#, mois.First())
        Assert.AreEqual(#2025-09-01#, mois.Last())
        Assert.HasCount(14, mois)
    End Sub

    <TestMethod>
    Public Sub EX34_Stock_SeulementLesEcarts()
        Dim ecarts = _service.Stock(Delegue, Septembre, seulementLesEcarts:=True)

        Assert.AreEqual("a17", ecarts.Single().Matricule)
    End Sub

    <TestMethod>
    Public Sub SupprimerDotation_Existante_PuisInexistante()
        _service.EnregistrerDotation(Delegue, "a17", "NOVEL26", Septembre, 3)

        Assert.IsEmpty(_service.SupprimerDotation(Delegue, "a17", "NOVEL26", Septembre))
        Assert.IsNotEmpty(_service.SupprimerDotation(Delegue, "a17", "NOVEL26", Septembre))
    End Sub

End Class
