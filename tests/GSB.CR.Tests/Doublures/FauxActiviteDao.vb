Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>DAO d'activité en mémoire : renvoie une synthèse fixe et mémorise la période demandée.</summary>
Public Class FauxActiviteDao
    Implements IActiviteDao

    Public Property DerniereDemande As (Matricule As String, Debut As Date, Fin As Date)

    Public Function ChargerSynthese(matricule As String, debut As Date, fin As Date) As SyntheseActivite Implements IActiviteDao.ChargerSynthese
        DerniereDemande = (matricule, debut, fin)
        Return New SyntheseActivite() With {
            .Debut = debut, .Fin = fin, .NbVisites = 2,
            .ParMois = New List(Of VisitesDuMois) From {New VisitesDuMois() With {.Mois = New Date(fin.Year, fin.Month, 1), .Nombre = 2}}}
    End Function

End Class
