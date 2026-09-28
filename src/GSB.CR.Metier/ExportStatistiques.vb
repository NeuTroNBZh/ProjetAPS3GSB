Imports System.Globalization
Imports System.Text
Imports System.Text.RegularExpressions
Imports GSB.CR.Modeles

''' <summary>
''' Export des statistiques d'activité au format CSV (EX-51), lisible directement par Excel en français :
''' séparateur point-virgule, virgule décimale, dates jj/mm/aaaa. Le fichier est à enregistrer en UTF-8
''' avec BOM (<see cref="Encodage"/>) pour que les accents s'affichent correctement.
''' </summary>
Public Module ExportStatistiques

    Private Const Separateur As Char = ";"c
    Private ReadOnly Francais As CultureInfo = CultureInfo.GetCultureInfo("fr-FR")

    ''' <summary>Encodage du fichier : UTF-8 avec BOM (reconnu par Excel).</summary>
    Public ReadOnly Property Encodage As Encoding = New UTF8Encoding(encoderShouldEmitUTF8Identifier:=True)

    ''' <summary>
    ''' Contenu CSV d'une synthèse : en-tête (périmètre, période, date d'export), indicateurs, visites par mois,
    ''' motifs, produits présentés, échantillons et, pour une équipe, l'activité de chaque membre.
    ''' </summary>
    ''' <param name="perimetre">« Mon activité », « Région Aquitaine »…</param>
    ''' <param name="membres">Activité par membre (région ou secteur) ; Nothing pour un collaborateur seul.</param>
    Public Function Generer(perimetre As String, s As SyntheseActivite, membres As IEnumerable(Of ActiviteMembre), exporteLe As DateTime) As String
        ArgumentNullException.ThrowIfNull(s)
        Dim sb As New StringBuilder()

        Ecrire(sb, "GSB-CR - statistiques d'activité (comptes-rendus validés)")
        Ecrire(sb, "Périmètre", perimetre)
        Ecrire(sb, "Période", $"du {Jour(s.Debut)} au {Jour(s.Fin)}")
        Ecrire(sb, "Exporté le", exporteLe.ToString("dd/MM/yyyy HH:mm", Francais))

        Ecrire(sb)
        Ecrire(sb, "Indicateur", "Valeur")
        Ecrire(sb, "Visites (comptes-rendus validés)", s.NbVisites)
        Ecrire(sb, "dont avec un remplaçant", s.NbVisitesRemplacant)
        Ecrire(sb, "Praticiens vus", s.NbPraticiens)
        Ecrire(sb, "Confiance moyenne (sur 5)", s.ConfianceMoyenne?.ToString("0.0", Francais))
        Ecrire(sb, "Échantillons distribués", s.NbEchantillons)
        Ecrire(sb, "Coût des échantillons (€)", Montant(s.CoutEchantillons))
        Ecrire(sb, "Temps moyen de saisie (secondes)", s.TempsSaisieMoyen?.ToString("0", Francais))
        Ecrire(sb, "Temps total de saisie (secondes)", s.TempsSaisieTotal.ToString("0", Francais))
        Ecrire(sb, "Brouillons à terminer (toutes dates)", s.NbBrouillons)

        Ecrire(sb)
        Ecrire(sb, "Visites par mois")
        Ecrire(sb, "Mois", "Visites")
        For Each m In s.ParMois
            Ecrire(sb, m.Mois.ToString("MM/yyyy", Francais), m.Nombre)
        Next

        Ecrire(sb)
        Ecrire(sb, "Visites par motif")
        Ecrire(sb, "Motif", "Visites")
        For Each m In s.ParMotif
            Ecrire(sb, m.Libelle, m.Nombre)
        Next

        Ecrire(sb)
        Ecrire(sb, "Produits présentés")
        Ecrire(sb, "Produit", "Présentations")
        For Each p In s.ProduitsPresentes
            Ecrire(sb, p.Libelle, p.Nombre)
        Next

        Ecrire(sb)
        Ecrire(sb, "Échantillons distribués")
        Ecrire(sb, "Dépôt légal", "Produit", "Quantité", "Coût (€)")
        For Each e In s.Echantillons
            Ecrire(sb, e.DepotLegal, e.NomCommercial, e.Quantite, Montant(e.Cout))
        Next

        If membres IsNot Nothing Then
            Ecrire(sb)
            Ecrire(sb, "Activité par visiteur")
            Ecrire(sb, "Visiteur", "Profil", "Région", "Visites", "Praticiens vus", "Confiance moyenne", "Échantillons", "Coût (€)",
                  "Brouillons", "Dernière visite")
            For Each m In membres
                Ecrire(sb, m.NomComplet, LibellesProfils.Libelle(m.Profil), m.NomRegion, m.NbVisites, m.NbPraticiens,
                      m.ConfianceMoyenne?.ToString("0.0", Francais), m.NbEchantillons, Montant(m.CoutEchantillons),
                      m.NbBrouillons, If(m.DateDerniereVisite.HasValue, Jour(m.DateDerniereVisite.Value), Nothing))
            Next
        End If
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Valeur d'une cellule : entre guillemets si elle contient un séparateur, un guillemet ou un retour à la ligne ;
    ''' précédée d'une apostrophe si elle commence par = + - @ (évite qu'Excel l'interprète comme une formule).
    ''' </summary>
    Public Function Champ(valeur As Object) As String
        If valeur Is Nothing Then Return ""
        Dim texte = If(TypeOf valeur Is IFormattable, DirectCast(valeur, IFormattable).ToString(Nothing, Francais), valeur.ToString())
        Dim nombre As Decimal
        If texte.Length > 0 AndAlso "=+-@".Contains(texte(0)) AndAlso Not Decimal.TryParse(texte, NumberStyles.Number, Francais, nombre) Then
            texte = "'" & texte
        End If
        If texte.IndexOfAny({Separateur, """"c, ControlChars.Cr, ControlChars.Lf}) >= 0 Then
            texte = """" & texte.Replace("""", """""") & """"
        End If
        Return texte
    End Function

    ''' <summary>Nom de fichier proposé : « GSB-activite-Mon-activite-2026-07-01-au-2026-09-28.csv ».</summary>
    Public Function NomDeFichier(perimetre As String, debut As Date, fin As Date) As String
        Dim sansAccents = New String(perimetre.Normalize(NormalizationForm.FormD).
                                     Where(Function(c) CharUnicodeInfo.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark).ToArray())
        Dim nom = Regex.Replace(sansAccents, "[^A-Za-z0-9]+", "-").Trim("-"c)
        Return $"GSB-activite-{nom}-{debut:yyyy-MM-dd}-au-{fin:yyyy-MM-dd}.csv"
    End Function

    Private Sub Ecrire(sb As StringBuilder, ParamArray valeurs As Object())
        sb.Append(String.Join(Separateur, valeurs.Select(AddressOf Champ))).Append(vbCrLf)
    End Sub

    Private Function Jour(d As Date) As String
        Return d.ToString("dd/MM/yyyy", Francais)
    End Function

    Private Function Montant(m As Decimal) As String
        Return m.ToString("0.00", Francais)
    End Function

End Module
