Imports GSB.CR.Metier

<TestClass>
Public Class PolitiqueMotDePasseTests

    <TestMethod>
    <DataRow("Gsb2026!")>
    <DataRow("Visite-Medicale9")>
    Public Sub Verifier_MotDePasseRobuste_AucuneErreur(mdp As String)
        Assert.IsEmpty(PolitiqueMotDePasse.Verifier(mdp))
    End Sub

    <TestMethod>
    <DataRow("Gs2026!", "8 caractères")>
    <DataRow("gsb2026!", "majuscule")>
    <DataRow("GSB2026!", "minuscule")>
    <DataRow("Gsbgsbgsb!", "chiffre")>
    <DataRow("Gsb20266", "caractère spécial")>
    <DataRow("Gsb 2026!", "espace")>
    Public Sub Verifier_RegleNonRespectee_ErreurExplicite(mdp As String, motCle As String)
        Dim erreurs = PolitiqueMotDePasse.Verifier(mdp)

        Assert.IsTrue(erreurs.Any(Function(e) e.Contains(motCle)), $"Erreur attendue contenant « {motCle} ».")
    End Sub

    <TestMethod>
    Public Sub Verifier_Nothing_PlusieursErreurs()
        Assert.IsGreaterThanOrEqualTo(4, PolitiqueMotDePasse.Verifier(Nothing).Count)
    End Sub

End Class
