Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ServiceActiviteTests

    Private Shared ReadOnly Aujourdhui As Date = #2026-09-28#

    Private _dao As FauxActiviteDao
    Private _service As ServiceActivite

    <TestInitialize>
    Public Sub Initialiser()
        _dao = New FauxActiviteDao()
        _service = New ServiceActivite(_dao, New FauxConsultationDao(), New HorlogeFixe(Aujourdhui))
    End Sub

    Private Shared Function Utilisateur(profil As Profil) As UtilisateurConnecte
        Return New UtilisateurConnecte(New Collaborateur() With {.Matricule = "a131"}, New Affectation() With {.Profil = profil})
    End Function

    ' --- Périodes et synthèse ------------------------------------------------

    <TestMethod>
    Public Sub PeriodesPredefinies_BornesCorrectes()
        Dim periodes = _service.PeriodesPredefinies()

        Assert.AreEqual("Les 3 derniers mois", periodes(0).Libelle, "Période par défaut")
        Assert.AreEqual(#2026-06-29#, periodes(0).Debut)
        Dim moisDernier = periodes.Single(Function(p) p.Libelle = "Le mois dernier")
        Assert.AreEqual(#2026-08-01#, moisDernier.Debut)
        Assert.AreEqual(#2026-08-31#, moisDernier.Fin)
        Assert.IsTrue(periodes.All(Function(p) p.Fin <= Aujourdhui AndAlso p.Debut >= _service.DebutConsultable))
    End Sub

    <TestMethod>
    Public Sub MaSynthese_FinFutureRameneeAAujourdhui()
        _service.MaSynthese(Utilisateur(Profil.Visiteur), #2026-09-01#, #2026-12-31#)

        Assert.AreEqual(Aujourdhui, _dao.DerniereDemande.Fin)
        Assert.AreEqual("a131", _dao.DerniereDemande.Matricule)
    End Sub

    <TestMethod>
    Public Sub MaSynthese_DebutApresFin_Refuse()
        Assert.ThrowsExactly(Of ErreurMetierException)(
            Function() _service.MaSynthese(Utilisateur(Profil.Visiteur), #2026-09-10#, #2026-09-01#))
    End Sub

    <TestMethod>
    Public Sub EX20_MaSynthese_PlusDeTroisAns_Refuse()
        Dim ex = Assert.ThrowsExactly(Of ErreurMetierException)(
            Function() _service.MaSynthese(Utilisateur(Profil.Visiteur), #2023-01-01#, #2026-09-28#))
        StringAssert.Contains(ex.Message, "trois dernières années")
    End Sub

    <TestMethod>
    Public Sub MaSynthese_TousLesMoisDeLaPeriodeSontPresents()
        Dim s = _service.MaSynthese(Utilisateur(Profil.Visiteur), #2026-07-15#, #2026-09-28#)

        CollectionAssert.AreEqual({#2026-07-01#, #2026-08-01#, #2026-09-01#}, s.ParMois.Select(Function(m) m.Mois).ToList())
        CollectionAssert.AreEqual({0, 0, 2}, s.ParMois.Select(Function(m) m.Nombre).ToList())
    End Sub

    <TestMethod>
    Public Sub CompleterMois_PeriodeSurDeuxAnnees()
        Dim mois = ServiceActivite.CompleterMois({New VisitesDuMois() With {.Mois = #2026-01-01#, .Nombre = 4}}, #2025-11-20#, #2026-02-03#)

        Assert.HasCount(4, mois)
        Assert.AreEqual(4, mois(2).Nombre)
    End Sub

    <TestMethod>
    Public Sub Responsable_PasDActivitePersonnelle()
        Assert.ThrowsExactly(Of ErreurMetierException)(
            Function() _service.MaSynthese(Utilisateur(Profil.Responsable), #2026-09-01#, #2026-09-28#))
    End Sub

    ' --- Praticiens à revoir ----------------------------------------------------

    Private Shared Function P(nom As String, derniere As Date?, Optional prochaine As Date? = Nothing) As PraticienResume
        Return New PraticienResume() With {.Nom = nom, .DateDerniereVisite = derniere, .DateProchainePrevue = prochaine}
    End Function

    <TestMethod>
    Public Sub EX24_Prioriser_OrdreDUrgence()
        Dim liste = ServiceActivite.Prioriser({
            P("AJour", #2026-09-10#),
            P("Bientot", #2026-02-15#),
            P("Retard", #2025-11-12#),
            P("TresRetard", #2025-01-05#),
            P("Jamais", Nothing),
            P("PrevueDepassee", #2026-08-01#, #2026-09-15#)}, Aujourdhui)

        CollectionAssert.AreEqual({"TresRetard", "Retard", "Jamais", "PrevueDepassee", "Bientot"},
                                  liste.Select(Function(x) x.Praticien.Nom).ToList())
        Assert.IsTrue(liste.Single(Function(x) x.Praticien.Nom = "PrevueDepassee").ProchainePrevueDepassee)
        Assert.AreEqual(320, liste.Single(Function(x) x.Praticien.Nom = "Retard").JoursDepuisDerniereVisite)
        Assert.IsNull(liste.Single(Function(x) x.Praticien.Nom = "Jamais").JoursDepuisDerniereVisite)
    End Sub

    <TestMethod>
    Public Sub EX24_Prioriser_PraticienAJourSansRetard_Exclu()
        Dim liste = ServiceActivite.Prioriser({P("AJour", #2026-09-10#, #2026-12-01#)}, Aujourdhui)

        Assert.IsEmpty(liste)
    End Sub

End Class
