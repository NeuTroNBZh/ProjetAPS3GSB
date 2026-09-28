Imports GSB.CR.Modeles

<TestClass>
Public Class CollaborateurTests

    <TestMethod>
    Public Sub EstParti_SansDateDepart_Faux()
        Dim c As New Collaborateur()
        Assert.IsFalse(c.EstParti(#2026-09-28#))
    End Sub

    <TestMethod>
    Public Sub EstParti_DateDepartPassee_Vrai()
        Dim c As New Collaborateur() With {.DateDepart = #2025-06-30#}
        Assert.IsTrue(c.EstParti(#2026-09-28#))
    End Sub

    <TestMethod>
    Public Sub EstParti_DateDepartFuture_Faux()
        Dim c As New Collaborateur() With {.DateDepart = #2026-12-31#}
        Assert.IsFalse(c.EstParti(#2026-09-28#))
    End Sub

    <TestMethod>
    Public Sub NomComplet_PrenomPuisNom()
        Dim c As New Collaborateur() With {.Prenom = "Louis", .Nom = "Villechalane"}
        Assert.AreEqual("Louis Villechalane", c.NomComplet)
    End Sub

End Class
