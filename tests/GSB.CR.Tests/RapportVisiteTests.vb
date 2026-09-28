Imports GSB.CR.Modeles

<TestClass>
Public Class RapportVisiteTests

    <TestMethod>
    Public Sub Nouveau_SansNumero_EstNouveauEnBrouillon()
        Dim r As New RapportVisite()

        Assert.IsTrue(r.EstNouveau)
        Assert.AreEqual(EtatRapport.Brouillon, r.Etat)
        Assert.AreEqual(0, r.TotalEchantillons)
    End Sub

    <TestMethod>
    Public Sub TotalEchantillons_SommeDesQuantites()
        Dim r As New RapportVisite() With {.Numero = 4}
        r.Echantillons.Add(New EchantillonOffert() With {.DepotLegal = "NOVEL26", .Quantite = 5})
        r.Echantillons.Add(New EchantillonOffert() With {.DepotLegal = "APATOUX22", .Quantite = 2})

        Assert.IsFalse(r.EstNouveau)
        Assert.AreEqual(7, r.TotalEchantillons)
    End Sub

    <TestMethod>
    Public Sub Motif_Autre_EstReconnu()
        Assert.IsTrue(New Motif() With {.Code = "AUTRE"}.EstAutre)
        Assert.IsFalse(New Motif() With {.Code = "PERIO"}.EstAutre)
    End Sub

    <TestMethod>
    Public Sub Praticien_ToString_NomPrenomEtVille()
        Dim p As New Praticien() With {.Nom = "Martin", .Prenom = "Hélène", .Ville = "Bordeaux"}
        Assert.AreEqual("Martin Hélène — Bordeaux", p.ToString())
        p.Ville = Nothing
        Assert.AreEqual("Martin Hélène", p.ToString())
    End Sub

End Class
