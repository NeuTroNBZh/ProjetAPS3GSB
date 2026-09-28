Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>Référentiels en mémoire pour tester la couche Métier sans base de données.</summary>
Public Class FauxReferentielDao
    Implements IReferentielDao

    Public ReadOnly Praticiens As New List(Of Praticien)
    Public ReadOnly Portefeuilles As New Dictionary(Of String, List(Of Integer))
    Private _prochainNumero As Integer = 1000

    Public Function ListerPortefeuille(matricule As String) As List(Of Praticien) Implements IReferentielDao.ListerPortefeuille
        Dim nums As List(Of Integer) = Nothing
        If Not Portefeuilles.TryGetValue(matricule, nums) Then Return New List(Of Praticien)
        Return Praticiens.Where(Function(p) nums.Contains(p.Numero.Value)).ToList()
    End Function

    Public Function RechercherPraticiens(debut As String, maximum As Integer) As List(Of Praticien) Implements IReferentielDao.RechercherPraticiens
        Return Praticiens.Where(Function(p) p.Nom.StartsWith(debut, StringComparison.OrdinalIgnoreCase)).Take(maximum).ToList()
    End Function

    Public Function TrouverPraticien(numero As Integer) As Praticien Implements IReferentielDao.TrouverPraticien
        Return Praticiens.FirstOrDefault(Function(p) p.Numero.GetValueOrDefault() = numero)
    End Function

    Public Function CreerPraticien(praticien As Praticien) As Integer Implements IReferentielDao.CreerPraticien
        _prochainNumero += 1
        praticien.Numero = _prochainNumero
        Praticiens.Add(praticien)
        Return _prochainNumero
    End Function

    Public Function ListerTypesPraticien() As List(Of TypePraticien) Implements IReferentielDao.ListerTypesPraticien
        Return New List(Of TypePraticien) From {New TypePraticien() With {.Code = "MV", .Libelle = "Médecin de ville"}}
    End Function

    Public Function ListerMedicaments() As List(Of Medicament) Implements IReferentielDao.ListerMedicaments
        Return New List(Of Medicament) From {New Medicament() With {.DepotLegal = "NOVEL26", .NomCommercial = "NOVELIX"}}
    End Function

    Public Function ListerMotifs() As List(Of Motif) Implements IReferentielDao.ListerMotifs
        Return New List(Of Motif) From {New Motif() With {.Code = "PERIO", .Libelle = "Périodicité"},
                                        New Motif() With {.Code = "AUTRE", .Libelle = "Autre"}}
    End Function

End Class
