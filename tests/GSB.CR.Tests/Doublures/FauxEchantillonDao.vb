Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>Dotations en mémoire.</summary>
Public Class FauxEchantillonDao
    Implements IEchantillonDao

    Public ReadOnly Dotations As New Dictionary(Of (String, String, Date), (Quantite As Integer, SaisiPar As String))

    Public Function Stock(perimetre As Perimetre, mois As Date) As List(Of LigneStock) Implements IEchantillonDao.Stock
        Return New List(Of LigneStock) From {
            New LigneStock() With {.Matricule = "a131", .DepotLegal = "NOVEL26", .Mois = mois, .Attribue = 10, .Distribue = 2},
            New LigneStock() With {.Matricule = "a17", .DepotLegal = "EQUILARX6", .Mois = mois, .Attribue = 8, .Distribue = 10}}
    End Function

    Public Sub EnregistrerDotation(matricule As String, depotLegal As String, mois As Date, quantite As Integer, saisiPar As String) Implements IEchantillonDao.EnregistrerDotation
        Dotations((matricule, depotLegal, mois)) = (quantite, saisiPar)
    End Sub

    Public Function SupprimerDotation(matricule As String, depotLegal As String, mois As Date) As Boolean Implements IEchantillonDao.SupprimerDotation
        Return Dotations.Remove((matricule, depotLegal, mois))
    End Function

End Class
