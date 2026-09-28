Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>DAO de consultation en mémoire : mémorise les paramètres reçus.</summary>
Public Class FauxConsultationDao
    Implements IConsultationDao

    Public Property DernierMatricule As String = "(non appelé)"
    Public Property DernierInclureInactifs As Boolean
    Public Property DernierPerimetre As Perimetre
    Public Property EnPanne As Boolean

    Private Sub VerifierPanne()
        If EnPanne Then Throw New AccesDonneesException("Panne simulée", New Exception())
    End Sub

    Public Function RechercherPraticiens(texte As String, perimetre As Perimetre,
                                         inclureInactifs As Boolean, maximum As Integer) As List(Of PraticienResume) Implements IConsultationDao.RechercherPraticiens
        VerifierPanne()
        DernierMatricule = perimetre?.Code
        DernierPerimetre = perimetre
        DernierInclureInactifs = inclureInactifs
        Return New List(Of PraticienResume) From {New PraticienResume() With {.Numero = 1, .Nom = "Martin"}}
    End Function

    Public Function ChargerFichePraticien(numero As Integer) As FichePraticien Implements IConsultationDao.ChargerFichePraticien
        VerifierPanne()
        Return If(numero = 1, New FichePraticien() With {.Praticien = New Praticien() With {.Numero = 1, .Nom = "Martin"}}, Nothing)
    End Function

    Public Function ChargerFicheMedicament(depotLegal As String) As FicheMedicament Implements IConsultationDao.ChargerFicheMedicament
        VerifierPanne()
        Return If(depotLegal = "NOVEL26", New FicheMedicament() With {.Medicament = New Medicament() With {.DepotLegal = "NOVEL26"}}, Nothing)
    End Function

End Class
