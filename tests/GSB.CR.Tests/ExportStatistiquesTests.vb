Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ExportStatistiquesTests

    Private Shared ReadOnly ExporteLe As New DateTime(2026, 9, 28, 15, 30, 0)

    Private Shared Function Synthese() As SyntheseActivite
        Return New SyntheseActivite() With {
            .Debut = #2026-07-01#, .Fin = #2026-09-28#,
            .NbVisites = 4, .NbPraticiens = 2, .NbVisitesRemplacant = 1, .ConfianceMoyenne = 3.5D,
            .NbEchantillons = 19, .CoutEchantillons = 33.8D, .TempsSaisieMoyen = 507, .TempsSaisieTotal = 1520, .NbBrouillons = 0,
            .ParMois = New List(Of VisitesDuMois) From {
                New VisitesDuMois() With {.Mois = #2026-07-01#, .Nombre = 0},
                New VisitesDuMois() With {.Mois = #2026-08-01#, .Nombre = 1},
                New VisitesDuMois() With {.Mois = #2026-09-01#, .Nombre = 3}},
            .ParMotif = New List(Of Repartition) From {New Repartition() With {.Libelle = "Périodicité", .Nombre = 4}},
            .ProduitsPresentes = New List(Of Repartition) From {New Repartition() With {.Libelle = "AMOPIL", .Nombre = 2}},
            .Echantillons = New List(Of EchantillonsDistribues) From {
                New EchantillonsDistribues() With {.DepotLegal = "AMOPIL7", .NomCommercial = "AMOPIL", .Quantite = 19, .Cout = 33.8D}}}
    End Function

    Private Shared Function Lignes(csv As String) As String()
        Return csv.Split({vbCrLf}, StringSplitOptions.None)
    End Function

    <TestMethod>
    Public Sub Generer_EnteteAvecPerimetrePeriodeEtDate()
        Dim l = Lignes(ExportStatistiques.Generer("Mon activité", Synthese(), Nothing, ExporteLe))

        CollectionAssert.Contains(l, "Périmètre;Mon activité")
        CollectionAssert.Contains(l, "Période;du 01/07/2026 au 28/09/2026")
        CollectionAssert.Contains(l, "Exporté le;28/09/2026 15:30")
    End Sub

    <TestMethod>
    Public Sub Generer_IndicateursAuFormatFrancais()
        Dim l = Lignes(ExportStatistiques.Generer("Mon activité", Synthese(), Nothing, ExporteLe))

        CollectionAssert.Contains(l, "Visites (comptes-rendus validés);4")
        CollectionAssert.Contains(l, "Confiance moyenne (sur 5);3,5")
        CollectionAssert.Contains(l, "Coût des échantillons (€);33,80")
        CollectionAssert.Contains(l, "Temps moyen de saisie (secondes);507")
    End Sub

    <TestMethod>
    Public Sub Generer_TousLesMoisDeLaPeriode_ZeroCompris()
        Dim l = Lignes(ExportStatistiques.Generer("Mon activité", Synthese(), Nothing, ExporteLe))

        CollectionAssert.Contains(l, "07/2026;0")
        CollectionAssert.Contains(l, "09/2026;3")
        CollectionAssert.Contains(l, "AMOPIL7;AMOPIL;19;33,80")
    End Sub

    <TestMethod>
    Public Sub Generer_SansVisite_ConfianceVide()
        Dim s = Synthese()
        s.ConfianceMoyenne = Nothing
        s.TempsSaisieMoyen = Nothing

        Dim l = Lignes(ExportStatistiques.Generer("Mon activité", s, Nothing, ExporteLe))

        CollectionAssert.Contains(l, "Confiance moyenne (sur 5);")
        CollectionAssert.Contains(l, "Temps moyen de saisie (secondes);")
    End Sub

    <TestMethod>
    Public Sub Generer_AvecEquipe_AjouteUneLigneParMembre()
        Dim membres = {New ActiviteMembre() With {
            .NomComplet = "Louis Villechalane", .Profil = Profil.Visiteur, .NomRegion = "Aquitaine",
            .NbVisites = 4, .NbPraticiens = 2, .ConfianceMoyenne = 3.5D, .NbEchantillons = 19, .CoutEchantillons = 33.8D,
            .NbBrouillons = 1, .DateDerniereVisite = #2026-09-10#}}

        Dim l = Lignes(ExportStatistiques.Generer("Région Aquitaine", Synthese(), membres, ExporteLe))

        CollectionAssert.Contains(l, "Activité par visiteur")
        CollectionAssert.Contains(l, "Louis Villechalane;Visiteur médical;Aquitaine;4;2;3,5;19;33,80;1;10/09/2026")
    End Sub

    <TestMethod>
    Public Sub Generer_SansEquipe_PasDeSectionVisiteurs()
        Dim csv = ExportStatistiques.Generer("Mon activité", Synthese(), Nothing, ExporteLe)

        Assert.DoesNotContain("Activité par visiteur", csv)
    End Sub

    <TestMethod>
    <DataRow("Simple", "Simple")>
    <DataRow("Avec ; point-virgule", """Avec ; point-virgule""")>
    <DataRow("Avec ""guillemets""", """Avec """"guillemets""""""")>
    <DataRow("=1+1", "'=1+1")>
    Public Sub Champ_ProtegeLesCaracteresSpeciaux(valeur As String, attendu As String)
        Assert.AreEqual(attendu, ExportStatistiques.Champ(valeur))
    End Sub

    <TestMethod>
    Public Sub NomDeFichier_SansCaracteresInterdits()
        Dim nom = ExportStatistiques.NomDeFichier("Région Alsace-Lorraine", #2026-07-01#, #2026-09-28#)

        Assert.AreEqual("GSB-activite-Region-Alsace-Lorraine-2026-07-01-au-2026-09-28.csv", nom)
    End Sub

End Class
