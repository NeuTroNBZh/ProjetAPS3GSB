Imports GSB.CR.Metier

<TestClass>
Public Class HacheurMotDePasseTests

    ''' <summary>Hachage de « Gsb2026! » tel que produit pour le jeu d'essai (bdd/05_jeu_essai.sql).</summary>
    Private Const HacheJeuEssai As String =
        "PBKDF2-SHA256$100000$HZbeCl2b2eme0nWe9SIoug==$nw2KtNekqqAMPZrtQuvW7rjm2FzaAarjrR/rnytaeC4="

    <TestMethod>
    Public Sub Verifier_HacheDuJeuEssai_EstCompatible()
        Assert.IsTrue(HacheurMotDePasse.Verifier("Gsb2026!", HacheJeuEssai))
    End Sub

    <TestMethod>
    Public Sub Hacher_PuisVerifier_MemeMotDePasse_Vrai()
        Dim hache = HacheurMotDePasse.Hacher("Secret#2026")

        StringAssert.StartsWith(hache, "PBKDF2-SHA256$100000$")
        Assert.IsTrue(HacheurMotDePasse.Verifier("Secret#2026", hache))
    End Sub

    <TestMethod>
    Public Sub Verifier_MauvaisMotDePasse_Faux()
        Assert.IsFalse(HacheurMotDePasse.Verifier("gsb2026!", HacheJeuEssai))
    End Sub

    <TestMethod>
    Public Sub Hacher_DeuxFois_ResultatsDifferents_GraceAuSel()
        Assert.AreNotEqual(HacheurMotDePasse.Hacher("abc"), HacheurMotDePasse.Hacher("abc"))
    End Sub

    <TestMethod>
    <DataRow("")>
    <DataRow("motdepasseenclair")>
    <DataRow("MD5$1$abc$def")>
    <DataRow("PBKDF2-SHA256$pasunnombre$AAAA$AAAA")>
    <DataRow("PBKDF2-SHA256$100000$pas-du-base64!$AAAA")>
    Public Sub Verifier_FormatInvalide_Faux(hache As String)
        Assert.IsFalse(HacheurMotDePasse.Verifier("Gsb2026!", hache))
    End Sub

    <TestMethod>
    Public Sub Verifier_MotDePasseNothing_Faux()
        Assert.IsFalse(HacheurMotDePasse.Verifier(Nothing, HacheJeuEssai))
    End Sub

End Class
