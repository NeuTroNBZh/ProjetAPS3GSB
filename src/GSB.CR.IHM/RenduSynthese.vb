Imports System.Globalization
Imports GSB.CR.Modeles

''' <summary>
''' Affichage d'une synthèse d'activité dans un <see cref="PanneauFiche"/> : indicateurs, visites par mois
''' (graphique + tableau), motifs, produits présentés, échantillons. Utilisé pour un visiteur, un membre
''' d'équipe ou une équipe entière.
''' </summary>
Public Module RenduSynthese

    Public Sub Afficher(panneau As PanneauFiche, s As SyntheseActivite, titre As String)
        Dim culture = CultureInfo.CurrentCulture
        panneau.SuspendLayout()
        panneau.Vider()
        panneau.AjouterTitre(titre)
        panneau.AjouterSousTitre($"Du {s.Debut:dd/MM/yyyy} au {s.Fin:dd/MM/yyyy} · seuls les comptes-rendus validés sont comptés.")

        panneau.AjouterIndicateurs({
            New TuileIndicateur("Visites", s.NbVisites.ToString("N0", culture),
                                If(s.NbVisitesRemplacant > 0, $"dont {s.NbVisitesRemplacant} avec un remplaçant", Nothing)),
            New TuileIndicateur("Praticiens vus", s.NbPraticiens.ToString("N0", culture)),
            New TuileIndicateur("Confiance moyenne", If(s.ConfianceMoyenne.HasValue, s.ConfianceMoyenne.Value.ToString("0.0", culture), "—"), "sur 5"),
            New TuileIndicateur("Échantillons distribués", s.NbEchantillons.ToString("N0", culture), $"coût : {s.CoutEchantillons.ToString("C", culture)}"),
            New TuileIndicateur("Temps moyen de saisie", Duree(s.TempsSaisieMoyen), $"total : {Duree(s.TempsSaisieTotal)}"),
            New TuileIndicateur("Brouillons à terminer", s.NbBrouillons.ToString("N0", culture), "toutes dates confondues")})

        panneau.AjouterSection("Visites par mois")
        Dim graphique As New GraphiqueBarres() With {.MessageVide = "Aucune visite validée sur la période."}
        graphique.DefinirDonnees(s.ParMois.Select(Function(m) (
            m.Mois.ToString("MMM yy", culture),
            m.Nombre,
            $"{m.Mois.ToString("MMMM yyyy", culture)} : {m.Nombre} visite(s)")))
        panneau.AjouterControle(graphique)
        If s.NbVisites > 0 Then
            ' Vue tableau du graphique (lecture exacte des valeurs)
            panneau.AjouterTableau({("Mois", 60.0F), ("Visites", 40.0F)},
                s.ParMois.Where(Function(m) m.Nombre > 0).Select(Function(m) New Object() {m.Mois.ToString("MMMM yyyy", culture), m.Nombre}),
                hauteurMax:=160)
        End If

        panneau.AjouterSection("Motifs des visites")
        If s.ParMotif.Count = 0 Then
            panneau.AjouterTexte("Aucune visite.", Theme.TexteGris, italique:=True)
        Else
            panneau.AjouterTableau({("Motif", 60.0F), ("Visites", 20.0F), ("Part", 20.0F)},
                s.ParMotif.Select(Function(m) New Object() {m.Libelle, m.Nombre, (m.Nombre / CDbl(s.NbVisites)).ToString("P0", culture)}))
        End If

        panneau.AjouterSection("Produits présentés")
        If s.ProduitsPresentes.Count = 0 Then
            panneau.AjouterTexte("Aucun produit présenté.", Theme.TexteGris, italique:=True)
        Else
            panneau.AjouterTableau({("Produit", 60.0F), ("Présentations", 40.0F)},
                s.ProduitsPresentes.Select(Function(p) New Object() {p.Libelle, p.Nombre}))
        End If

        panneau.AjouterSection("Échantillons distribués")
        If s.Echantillons.Count = 0 Then
            panneau.AjouterTexte("Aucun échantillon distribué.", Theme.TexteGris, italique:=True)
        Else
            panneau.AjouterTableau({("Produit", 50.0F), ("Quantité", 25.0F), ("Coût", 25.0F)},
                s.Echantillons.Select(Function(x) New Object() {x.NomCommercial, x.Quantite, x.Cout.ToString("C", culture)}))
        End If
        panneau.ResumeLayout()
        panneau.AutoScrollPosition = New Point(0, 0)
    End Sub

    ''' <summary>Durée lisible : « 6 min 40 s ».</summary>
    Public Function Duree(secondes As Decimal?) As String
        If Not secondes.HasValue Then Return "—"
        Dim t = TimeSpan.FromSeconds(CDbl(secondes.Value))
        If t.TotalHours >= 1 Then Return $"{CInt(Math.Floor(t.TotalHours))} h {t.Minutes:00}"
        If t.TotalMinutes >= 1 Then Return $"{t.Minutes} min {t.Seconds:00} s"
        Return $"{t.Seconds} s"
    End Function

End Module
