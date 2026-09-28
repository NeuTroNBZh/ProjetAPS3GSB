Imports GSB.CR.Modeles

<TestClass>
Public Class ProfilCodesTests

    <TestMethod>
    <DataRow("VIS", Profil.Visiteur)>
    <DataRow("DEL", Profil.Delegue)>
    <DataRow("RES", Profil.Responsable)>
    <DataRow("ADM", Profil.Administrateur)>
    Public Sub DepuisCode_EtVersCode_SontReciproques(code As String, attendu As Profil)
        Assert.AreEqual(attendu, ProfilCodes.DepuisCode(code))
        Assert.AreEqual(code, ProfilCodes.VersCode(attendu))
    End Sub

    <TestMethod>
    Public Sub DepuisCode_Inconnu_LeveUneErreur()
        Assert.ThrowsExactly(Of ArgumentException)(Function() ProfilCodes.DepuisCode("XXX"))
    End Sub

End Class
