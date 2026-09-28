Imports System.Text
Imports GSB.CR.Modeles

''' <summary>
''' Règles de saisie d'un compte-rendu (EX-11 à EX-19, EX-27).
''' Un brouillon n'exige que le praticien et la date ; un CR validé doit être complet.
''' </summary>
Public NotInheritable Class ValidateurRapport

    Public Const MaxProduitsPresentes As Integer = 2
    Public Const MaxQuantiteEchantillon As Integer = 9999
    Public Const MaxOctetsBilan As Integer = 4000
    Public Const MaxLongueurPrecision As Integer = 200

    Private Sub New()
    End Sub

    ''' <summary>Renvoie la liste des erreurs (vide si le rapport peut être enregistré dans son état).</summary>
    ''' <param name="aujourdhui">Date du jour (la visite ne peut pas être postérieure).</param>
    Public Shared Function Verifier(rapport As RapportVisite, aujourdhui As Date) As IReadOnlyList(Of String)
        ArgumentNullException.ThrowIfNull(rapport)
        Dim erreurs As New List(Of String)
        Dim valider = rapport.Etat = EtatRapport.Valide

        ' --- Toujours obligatoires (même en brouillon)
        If Not rapport.NumeroPraticien.HasValue Then erreurs.Add("Choisissez le praticien visité.")
        If Not rapport.DateVisite.HasValue Then
            erreurs.Add("Indiquez la date de la visite.")
        ElseIf rapport.DateVisite.Value.Date > aujourdhui.Date Then
            erreurs.Add("La date de visite ne peut pas être dans le futur.")
        End If

        ' --- Remplaçant
        If rapport.NumeroRemplacant.HasValue AndAlso rapport.NumeroRemplacant = rapport.NumeroPraticien Then
            erreurs.Add("Le remplaçant doit être différent du praticien titulaire.")
        End If

        ' --- Motif
        If rapport.CodeMotif = Motif.CodeAutre Then
            If String.IsNullOrWhiteSpace(rapport.PrecisionMotif) Then
                erreurs.Add("Précisez le motif « Autre ».")
            ElseIf rapport.PrecisionMotif.Trim().Length > MaxLongueurPrecision Then
                erreurs.Add($"La précision du motif ne doit pas dépasser {MaxLongueurPrecision} caractères.")
            End If
        End If

        ' --- Produits présentés
        If rapport.ProduitsPresentes.Count > MaxProduitsPresentes Then
            erreurs.Add($"Une visite comporte au plus {MaxProduitsPresentes} produits présentés.")
        End If
        If rapport.ProduitsPresentes.Distinct().Count() <> rapport.ProduitsPresentes.Count Then
            erreurs.Add("Le même produit ne peut pas être présenté deux fois.")
        End If

        ' --- Échantillons
        If rapport.Echantillons.GroupBy(Function(e) e.DepotLegal).Any(Function(g) g.Count() > 1) Then
            erreurs.Add("Un même produit apparaît plusieurs fois dans les échantillons.")
        End If
        For Each e In rapport.Echantillons
            If e.Quantite <= 0 Then
                erreurs.Add($"La quantité d'échantillons de {Nom(e)} doit être supérieure à zéro.")
            ElseIf e.Quantite > MaxQuantiteEchantillon Then
                erreurs.Add($"La quantité d'échantillons de {Nom(e)} ne peut pas dépasser {MaxQuantiteEchantillon}.")
            End If
        Next

        ' --- Bilan, confiance, prochaine visite
        If rapport.Bilan IsNot Nothing AndAlso Encoding.UTF8.GetByteCount(rapport.Bilan) > MaxOctetsBilan Then
            erreurs.Add("Le bilan est trop long.")
        End If
        If rapport.CoefConfiance.HasValue AndAlso (rapport.CoefConfiance < 1 OrElse rapport.CoefConfiance > 5) Then
            erreurs.Add("Le coefficient de confiance doit être compris entre 1 et 5.")
        End If
        If rapport.DateProchaineVisite.HasValue AndAlso rapport.DateVisite.HasValue AndAlso
           rapport.DateProchaineVisite.Value.Date <= rapport.DateVisite.Value.Date Then
            erreurs.Add("La prochaine visite doit être postérieure à la date de visite.")
        End If

        ' --- Obligatoires pour valider (EX-11)
        If valider Then
            If String.IsNullOrEmpty(rapport.CodeMotif) Then erreurs.Add("Choisissez le motif de la visite.")
            If String.IsNullOrWhiteSpace(rapport.Bilan) Then erreurs.Add("Rédigez le bilan de la visite.")
            If Not rapport.CoefConfiance.HasValue Then erreurs.Add("Indiquez le coefficient de confiance du praticien.")
        End If

        Return erreurs
    End Function

    Private Shared Function Nom(e As EchantillonOffert) As String
        Return If(String.IsNullOrEmpty(e.NomCommercial), e.DepotLegal, e.NomCommercial)
    End Function

End Class
