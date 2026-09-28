Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ValidateurRapportTests

    Private Shared ReadOnly Aujourdhui As Date = #2026-09-28#

    ''' <summary>CR complet et valide, que chaque test dégrade sur un point.</summary>
    Private Shared Function RapportComplet() As RapportVisite
        Dim r As New RapportVisite() With {
            .NumeroPraticien = 1, .DateVisite = Aujourdhui.AddDays(-1), .CodeMotif = "PERIO",
            .Bilan = "Bon accueil.", .CoefConfiance = 4, .Etat = EtatRapport.Valide}
        r.ProduitsPresentes.AddRange({"NOVEL26", "AMOPIL7"})
        r.Echantillons.Add(New EchantillonOffert() With {.DepotLegal = "NOVEL26", .NomCommercial = "NOVELIX", .Quantite = 5})
        Return r
    End Function

    Private Shared Function Erreurs(r As RapportVisite) As IReadOnlyList(Of String)
        Return ValidateurRapport.Verifier(r, Aujourdhui)
    End Function

    Private Shared Sub AssertErreur(r As RapportVisite, motCle As String)
        Dim liste = Erreurs(r)
        Assert.IsTrue(liste.Any(Function(e) e.Contains(motCle)),
                      $"Erreur attendue contenant « {motCle} ». Obtenu : {String.Join(" | ", liste)}")
    End Sub

    <TestMethod>
    Public Sub RapportComplet_AucuneErreur()
        Assert.IsEmpty(Erreurs(RapportComplet()))
    End Sub

    <TestMethod>
    Public Sub Brouillon_SeulsPraticienEtDateSontExiges()
        Dim r As New RapportVisite() With {.NumeroPraticien = 1, .DateVisite = Aujourdhui, .Etat = EtatRapport.Brouillon}
        Assert.IsEmpty(Erreurs(r))
    End Sub

    <TestMethod>
    Public Sub Brouillon_SansPraticienNiDate_Refuse()
        Dim r As New RapportVisite() With {.Etat = EtatRapport.Brouillon}
        AssertErreur(r, "praticien")
        AssertErreur(r, "date de la visite")
    End Sub

    <TestMethod>
    Public Sub EX11_ValiderSansMotifBilanNiConfiance_TroisErreurs()
        Dim r = RapportComplet()
        r.CodeMotif = Nothing : r.Bilan = "   " : r.CoefConfiance = Nothing
        AssertErreur(r, "motif")
        AssertErreur(r, "bilan")
        AssertErreur(r, "confiance")
    End Sub

    <TestMethod>
    Public Sub EX27_DateDansLeFutur_Refusee()
        Dim r = RapportComplet()
        r.DateVisite = Aujourdhui.AddDays(1)
        AssertErreur(r, "futur")
    End Sub

    <TestMethod>
    Public Sub DateDuJour_Acceptee()
        Dim r = RapportComplet()
        r.DateVisite = Aujourdhui
        Assert.IsEmpty(Erreurs(r))
    End Sub

    <TestMethod>
    Public Sub EX12_MotifAutreSansPrecision_Refuse()
        Dim r = RapportComplet()
        r.CodeMotif = "AUTRE"
        AssertErreur(r, "Précisez")
        r.PrecisionMotif = "Staff du service"
        Assert.IsEmpty(Erreurs(r))
    End Sub

    <TestMethod>
    Public Sub EX12_PrecisionTropLongue_Refusee()
        Dim r = RapportComplet()
        r.CodeMotif = "AUTRE" : r.PrecisionMotif = New String("x"c, 201)
        AssertErreur(r, "200 caractères")
    End Sub

    <TestMethod>
    Public Sub EX13_TroisProduitsPresentes_Refuse()
        Dim r = RapportComplet()
        r.ProduitsPresentes.Add("CLAZER6")
        AssertErreur(r, "au plus 2")
    End Sub

    <TestMethod>
    Public Sub EX13_MemeProduitDeuxFois_Refuse()
        Dim r = RapportComplet()
        r.ProduitsPresentes(1) = "NOVEL26"
        AssertErreur(r, "deux fois")
    End Sub

    <TestMethod>
    Public Sub EX13_AucunProduitPresente_Accepte()
        Dim r = RapportComplet()
        r.ProduitsPresentes.Clear()
        Assert.IsEmpty(Erreurs(r))
    End Sub

    <TestMethod>
    <DataRow(0, "supérieure à zéro")>
    <DataRow(-3, "supérieure à zéro")>
    <DataRow(10000, "9999")>
    Public Sub EX14_QuantiteEchantillonInvalide_Refusee(quantite As Integer, motCle As String)
        Dim r = RapportComplet()
        r.Echantillons(0).Quantite = quantite
        AssertErreur(r, motCle)
    End Sub

    <TestMethod>
    Public Sub EX14_EchantillonEnDouble_Refuse()
        Dim r = RapportComplet()
        r.Echantillons.Add(New EchantillonOffert() With {.DepotLegal = "NOVEL26", .Quantite = 1})
        AssertErreur(r, "plusieurs fois")
    End Sub

    <TestMethod>
    Public Sub EX14_EchantillonsNonPresentes_Acceptes()
        Dim r = RapportComplet()
        r.Echantillons.Add(New EchantillonOffert() With {.DepotLegal = "INSXT5", .Quantite = 10})
        Assert.IsEmpty(Erreurs(r))
    End Sub

    <TestMethod>
    <DataRow(0)>
    <DataRow(6)>
    Public Sub EX15_ConfianceHorsBornes_Refusee(coef As Integer)
        Dim r = RapportComplet()
        r.CoefConfiance = coef
        AssertErreur(r, "entre 1 et 5")
    End Sub

    <TestMethod>
    Public Sub EX16_RemplacantEgalTitulaire_Refuse()
        Dim r = RapportComplet()
        r.NumeroRemplacant = r.NumeroPraticien
        AssertErreur(r, "remplaçant")
    End Sub

    <TestMethod>
    Public Sub EX19_ProchaineVisiteAvantOuLeJourDeLaVisite_Refusee()
        Dim r = RapportComplet()
        r.DateProchaineVisite = r.DateVisite
        AssertErreur(r, "prochaine visite")
        r.DateProchaineVisite = r.DateVisite.Value.AddMonths(6)
        Assert.IsEmpty(Erreurs(r))
    End Sub

    <TestMethod>
    Public Sub Bilan_TropLongEnOctets_Refuse()
        Dim r = RapportComplet()
        r.Bilan = New String("é"c, 2001)   ' 2001 caractères mais 4002 octets en UTF-8
        AssertErreur(r, "trop long")
    End Sub

End Class
