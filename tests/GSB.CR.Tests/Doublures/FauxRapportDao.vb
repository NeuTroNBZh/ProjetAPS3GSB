Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>DAO des rapports en mémoire pour tester la couche Métier sans base de données.</summary>
Public Class FauxRapportDao
    Implements IRapportDao

    Public ReadOnly Rapports As New Dictionary(Of Integer, RapportVisite)
    Public ReadOnly Sessions As New List(Of (Numero As Integer, Matricule As String, Debut As DateTime, Fin As DateTime))
    Public Property EnPanne As Boolean
    Public Property DernierDepuis As Date
    Private _prochainNumero As Integer = 100

    Private Sub VerifierPanne()
        If EnPanne Then Throw New AccesDonneesException("Panne simulée", New Exception())
    End Sub

    Public Function ListerParAuteur(matricule As String, depuis As Date) As List(Of RapportResume) Implements IRapportDao.ListerParAuteur
        VerifierPanne()
        DernierDepuis = depuis
        Return Rapports.Values.
            Where(Function(r) r.MatriculeAuteur = matricule AndAlso r.DateVisite.Value >= depuis).
            Select(Function(r) New RapportResume() With {.Numero = r.Numero.Value, .DateVisite = r.DateVisite.Value, .Etat = r.Etat}).
            ToList()
    End Function

    Public Function Charger(numero As Integer) As RapportVisite Implements IRapportDao.Charger
        VerifierPanne()
        Dim r As RapportVisite = Nothing
        Return If(Rapports.TryGetValue(numero, r), Copier(r), Nothing)
    End Function

    Public Function Creer(rapport As RapportVisite) As Integer Implements IRapportDao.Creer
        VerifierPanne()
        _prochainNumero += 1
        Dim copie = Copier(rapport)
        copie.Numero = _prochainNumero
        Rapports(_prochainNumero) = copie
        Return _prochainNumero
    End Function

    Public Sub Modifier(rapport As RapportVisite) Implements IRapportDao.Modifier
        VerifierPanne()
        Rapports(rapport.Numero.Value) = Copier(rapport)
    End Sub

    Public Function SupprimerBrouillon(numero As Integer) As Boolean Implements IRapportDao.SupprimerBrouillon
        VerifierPanne()
        Dim r As RapportVisite = Nothing
        If Rapports.TryGetValue(numero, r) AndAlso r.Etat = EtatRapport.Brouillon Then Return Rapports.Remove(numero)
        Return False
    End Function

    Public Sub AjouterSessionSaisie(numero As Integer, matricule As String, debut As DateTime, fin As DateTime) Implements IRapportDao.AjouterSessionSaisie
        VerifierPanne()
        Sessions.Add((numero, matricule, debut, fin))
    End Sub

    ''' <summary>Copie superficielle + listes, pour simuler une relecture en base.</summary>
    Private Shared Function Copier(r As RapportVisite) As RapportVisite
        Dim c = DirectCast(r.GetType().GetMethod("MemberwiseClone", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).Invoke(r, Nothing), RapportVisite)
        c.ProduitsPresentes = New List(Of String)(r.ProduitsPresentes)
        c.Echantillons = New List(Of EchantillonOffert)(r.Echantillons)
        Return c
    End Function

End Class
