Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>Équipes en mémoire : Aquitaine (a131, a17, délégué d01).</summary>
Public Class FauxEquipeDao
    Implements IEquipeDao

    Public Property DernierPerimetre As Perimetre

    Public Function ListerMembres(perimetre As Perimetre) As List(Of MembreEquipe) Implements IEquipeDao.ListerMembres
        DernierPerimetre = perimetre
        If perimetre.Code <> "AQU" AndAlso perimetre.Code <> "O" Then Return New List(Of MembreEquipe)
        Return New List(Of MembreEquipe) From {
            New MembreEquipe() With {.Matricule = "d01", .Nom = "Bedos", .Prenom = "Christian", .Profil = Profil.Delegue},
            New MembreEquipe() With {.Matricule = "a131", .Nom = "Villechalane", .Prenom = "Louis", .Profil = Profil.Visiteur},
            New MembreEquipe() With {.Matricule = "a17", .Nom = "Andre", .Prenom = "David", .Profil = Profil.Visiteur}}
    End Function

End Class
