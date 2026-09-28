''' <summary>
''' Charte graphique GSB : bleu et blanc, couleurs du logo du laboratoire (CDC « Ergonomie »).
''' Toutes les fenêtres utilisent ces couleurs et polices pour garder une unité visuelle.
''' </summary>
Public Module Theme

    Public ReadOnly BleuGsb As Color = Color.FromArgb(0, 82, 155)
    Public ReadOnly BleuFonce As Color = Color.FromArgb(0, 56, 108)
    Public ReadOnly BleuClair As Color = Color.FromArgb(232, 241, 250)
    Public ReadOnly BleuSurvol As Color = Color.FromArgb(206, 225, 245)
    Public ReadOnly Blanc As Color = Color.White
    Public ReadOnly TexteGris As Color = Color.FromArgb(90, 98, 110)
    Public ReadOnly Erreur As Color = Color.FromArgb(190, 30, 45)

    Public ReadOnly PoliceNormale As New Font("Segoe UI", 10.0F)
    Public ReadOnly PoliceTitre As New Font("Segoe UI Semibold", 18.0F)
    Public ReadOnly PoliceSousTitre As New Font("Segoe UI", 11.0F)

    ''' <summary>Bouton d'action principale : fond bleu, texte blanc.</summary>
    Public Sub StyliserBoutonPrincipal(bouton As Button)
        bouton.FlatStyle = FlatStyle.Flat
        bouton.FlatAppearance.BorderSize = 0
        bouton.FlatAppearance.MouseOverBackColor = BleuFonce
        bouton.BackColor = BleuGsb
        bouton.ForeColor = Blanc
        bouton.Font = New Font("Segoe UI Semibold", 10.5F)
        bouton.Cursor = Cursors.Hand
    End Sub

    ''' <summary>Bouton secondaire : fond blanc, bordure et texte bleus.</summary>
    Public Sub StyliserBoutonSecondaire(bouton As Button)
        bouton.FlatStyle = FlatStyle.Flat
        bouton.FlatAppearance.BorderColor = BleuGsb
        bouton.FlatAppearance.MouseOverBackColor = BleuClair
        bouton.BackColor = Blanc
        bouton.ForeColor = BleuGsb
        bouton.Font = New Font("Segoe UI", 10.0F)
        bouton.Cursor = Cursors.Hand
    End Sub

End Module
