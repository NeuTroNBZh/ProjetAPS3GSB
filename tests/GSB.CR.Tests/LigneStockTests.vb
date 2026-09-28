Imports GSB.CR.Modeles

<TestClass>
Public Class LigneStockTests

    <TestMethod>
    Public Sub Ecart_AttribueMoinsDistribue()
        Dim l As New LigneStock() With {.Attribue = 8, .Distribue = 10}

        Assert.AreEqual(-2, l.Ecart)
        Assert.IsTrue(l.EnDepassement)
    End Sub

    <TestMethod>
    Public Sub Ecart_PositifOuNul_PasDeDepassement()
        Assert.IsFalse(New LigneStock() With {.Attribue = 10, .Distribue = 10}.EnDepassement)
        Assert.IsFalse(New LigneStock() With {.Attribue = 10, .Distribue = 3}.EnDepassement)
    End Sub

    <TestMethod>
    Public Sub Perimetre_Libelles()
        Assert.AreEqual("Région Aquitaine", Perimetre.DeLaRegion("AQU", "Aquitaine").Libelle)
        Assert.AreEqual("Secteur Est", Perimetre.DuSecteur("E", "Est").Libelle)
        Assert.AreEqual(TypePerimetre.Collaborateur, Perimetre.DuCollaborateur("a131").Type)
    End Sub

End Class
