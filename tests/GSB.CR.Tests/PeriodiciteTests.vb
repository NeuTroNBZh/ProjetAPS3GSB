Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class PeriodiciteTests

    Private Shared ReadOnly Aujourdhui As Date = #2026-09-28#

    <TestMethod>
    Public Sub JamaisVisite()
        Assert.AreEqual(EtatPeriodicite.JamaisVisite, Periodicite.Evaluer(Nothing, Aujourdhui))
    End Sub

    <TestMethod>
    <DataRow("2026-09-10", EtatPeriodicite.AJour)>
    <DataRow("2026-03-29", EtatPeriodicite.AJour)>
    <DataRow("2026-03-28", EtatPeriodicite.ARevoirBientot)>
    <DataRow("2026-01-28", EtatPeriodicite.ARevoirBientot)>
    <DataRow("2026-01-27", EtatPeriodicite.ARevoir)>
    <DataRow("2022-05-10", EtatPeriodicite.ARevoir)>
    Public Sub Evaluer_BornesDe6Et8Mois(derniere As String, attendu As EtatPeriodicite)
        Assert.AreEqual(attendu, Periodicite.Evaluer(Date.Parse(derniere, Globalization.CultureInfo.InvariantCulture), Aujourdhui))
    End Sub

    <TestMethod>
    Public Sub ProchaineVisiteConseillee_SixMoisApres()
        Assert.AreEqual(#2027-03-10#, Periodicite.ProchaineVisiteConseillee(#2026-09-10#))
        Assert.IsNull(Periodicite.ProchaineVisiteConseillee(Nothing))
    End Sub

End Class
