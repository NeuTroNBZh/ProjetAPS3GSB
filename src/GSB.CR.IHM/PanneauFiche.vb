''' <summary>
''' Panneau de fiche détaillée aux couleurs GSB : empile verticalement titres, badges, sections,
''' textes et tableaux, en ajustant leur largeur à celle du panneau. Défile si le contenu est long.
''' </summary>
Public Class PanneauFiche
    Inherits FlowLayoutPanel

    Private Const MargeDroite As Integer = 28

    Public Sub New()
        FlowDirection = FlowDirection.TopDown
        WrapContents = False
        AutoScroll = True
        Padding = New Padding(18, 14, 8, 14)
        BackColor = Theme.Blanc
    End Sub

    ''' <summary>Largeur utile d'un élément (panneau moins marges et barre de défilement).</summary>
    Private ReadOnly Property LargeurUtile As Integer
        Get
            Return Math.Max(200, ClientSize.Width - Padding.Horizontal - MargeDroite)
        End Get
    End Property

    ''' <summary>Efface la fiche affichée.</summary>
    Public Sub Vider()
        SuspendLayout()
        For Each c In Controls.Cast(Of Control)().ToList()
            c.Dispose()
        Next
        Controls.Clear()
        ResumeLayout()
    End Sub

    ''' <summary>Affiche un message centré quand aucune fiche n'est sélectionnée.</summary>
    Public Sub AfficherMessage(message As String)
        Vider()
        AjouterTexte(message, Theme.TexteGris, italique:=True)
    End Sub

    Public Sub AjouterTitre(texte As String)
        Ajouter(New Label() With {.Text = texte, .Font = New Font("Segoe UI Semibold", 16.0F), .ForeColor = Theme.BleuGsb,
                                   .Margin = New Padding(0, 0, 0, 2)})
    End Sub

    Public Sub AjouterSousTitre(texte As String)
        Ajouter(New Label() With {.Text = texte, .Font = New Font("Segoe UI", 10.5F), .ForeColor = Theme.TexteGris,
                                   .Margin = New Padding(0, 0, 0, 8)})
    End Sub

    ''' <summary>Étiquette colorée (état, périodicité…).</summary>
    Public Sub AjouterBadge(texte As String, fond As Color, encre As Color)
        Dim badge As New Label() With {
            .Text = texte, .AutoSize = True, .BackColor = fond, .ForeColor = encre,
            .Font = New Font("Segoe UI Semibold", 9.5F), .Padding = New Padding(8, 3, 8, 3), .Margin = New Padding(0, 2, 0, 6)}
        Controls.Add(badge)
    End Sub

    Public Sub AjouterSection(texte As String)
        Dim section As New Label() With {.Text = texte, .Margin = New Padding(0, 14, 0, 4)}
        Theme.StyliserSection(section)
        Ajouter(section)
        ' Filet bleu sous le titre de section
        Controls.Add(New Panel() With {.Height = 1, .Width = LargeurUtile, .BackColor = Theme.BleuSurvol, .Margin = New Padding(0, 0, 0, 6)})
    End Sub

    ''' <summary>Paragraphe ; les lignes vides ou Nothing sont ignorées.</summary>
    Public Sub AjouterTexte(texte As String, Optional couleur As Color = Nothing, Optional italique As Boolean = False)
        If String.IsNullOrWhiteSpace(texte) Then Return
        Ajouter(New Label() With {
            .Text = texte, .ForeColor = If(couleur = Nothing, Color.FromArgb(31, 41, 51), couleur),
            .Font = New Font("Segoe UI", 10.0F, If(italique, FontStyle.Italic, FontStyle.Regular)),
            .Margin = New Padding(0, 0, 0, 4)})
    End Sub

    ''' <summary>Couple « libellé : valeur » ; ignoré si la valeur est vide.</summary>
    Public Sub AjouterInformation(libelle As String, valeur As String)
        If String.IsNullOrWhiteSpace(valeur) Then Return
        Dim ligne As New Label() With {.Text = $"{libelle} : {valeur}", .Margin = New Padding(0, 0, 0, 3), .ForeColor = Color.FromArgb(31, 41, 51)}
        Ajouter(ligne)
    End Sub

    ''' <summary>Tableau en lecture seule. <paramref name="colonnes"/> : (titre, poids relatif).</summary>
    Public Function AjouterTableau(colonnes As (Titre As String, Poids As Single)(), lignes As IEnumerable(Of Object()),
                                   Optional hauteurMax As Integer = 220) As DataGridView
        Dim grille As New DataGridView() With {
            .ReadOnly = True, .AllowUserToAddRows = False, .AllowUserToDeleteRows = False, .AllowUserToResizeRows = False,
            .RowHeadersVisible = False, .SelectionMode = DataGridViewSelectionMode.FullRowSelect, .MultiSelect = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, .Margin = New Padding(0, 0, 0, 6),
            .ScrollBars = ScrollBars.Vertical}
        Theme.StyliserGrille(grille)
        For Each c In colonnes
            grille.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = c.Titre, .FillWeight = c.Poids,
                                                                     .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
        For Each l In lignes
            grille.Rows.Add(l)
        Next
        grille.Width = LargeurUtile
        grille.Height = Math.Min(hauteurMax, grille.ColumnHeadersHeight + Math.Max(1, grille.Rows.Count) * grille.RowTemplate.Height + 3)
        Controls.Add(grille)
        Return grille
    End Function

    ''' <summary>Ajoute un contrôle (graphique…) qui occupe toute la largeur du panneau.</summary>
    Public Sub AjouterControle(controle As Control)
        controle.Width = LargeurUtile
        controle.Margin = New Padding(0, 0, 0, 8)
        controle.Tag = PleineLargeur
        Controls.Add(controle)
    End Sub

    ''' <summary>Ajoute une rangée de tuiles d'indicateurs, passant à la ligne si besoin.</summary>
    Public Sub AjouterIndicateurs(tuiles As IEnumerable(Of Control))
        Dim rangee As New FlowLayoutPanel() With {
            .FlowDirection = FlowDirection.LeftToRight, .WrapContents = True, .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink, .MaximumSize = New Size(LargeurUtile, 0),
            .Margin = New Padding(0, 4, 0, 4), .Tag = PleineLargeur}
        rangee.Controls.AddRange(tuiles.ToArray())
        Controls.Add(rangee)
    End Sub

    Private Const PleineLargeur As String = "pleine-largeur"

    Private Sub Ajouter(etiquette As Label)
        etiquette.AutoSize = True
        etiquette.MaximumSize = New Size(LargeurUtile, 0)
        Controls.Add(etiquette)
    End Sub

    ''' <summary>Réajuste la largeur des éléments quand le panneau change de taille.</summary>
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        SuspendLayout()
        For Each c As Control In Controls
            If TypeOf c Is Label Then
                c.MaximumSize = New Size(LargeurUtile, 0)
            ElseIf TypeOf c Is FlowLayoutPanel AndAlso Equals(c.Tag, PleineLargeur) Then
                c.MaximumSize = New Size(LargeurUtile, 0)
            ElseIf TypeOf c Is DataGridView OrElse Equals(c.Tag, PleineLargeur) OrElse (TypeOf c Is Panel AndAlso c.Height = 1) Then
                c.Width = LargeurUtile
            End If
        Next
        ResumeLayout()
    End Sub

End Class
