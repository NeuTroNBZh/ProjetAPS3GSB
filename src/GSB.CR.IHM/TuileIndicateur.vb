''' <summary>
''' Tuile d'indicateur : libellé, valeur mise en avant et complément facultatif.
''' Taille automatique (s'adapte à la mise à l'échelle de l'écran et à la longueur des textes).
''' </summary>
Public Class TuileIndicateur
    Inherits Panel

    Public Sub New(libelle As String, valeur As String, Optional complement As String = Nothing)
        AutoSize = True
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        Margin = New Padding(0, 0, 10, 10)
        Padding = New Padding(16, 10, 14, 10)
        BackColor = Theme.BleuClair

        Dim pile As New FlowLayoutPanel() With {
            .FlowDirection = FlowDirection.TopDown, .WrapContents = False, .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink, .Margin = Padding.Empty, .Padding = Padding.Empty,
            .BackColor = Color.Transparent, .Dock = DockStyle.Fill}
        pile.Controls.Add(Texte(libelle, New Font("Segoe UI", 9.5F), Theme.BleuFonce))
        pile.Controls.Add(Texte(valeur, New Font("Segoe UI Semibold", 20.0F), Color.FromArgb(31, 41, 51)))
        pile.Controls.Add(Texte(If(complement, " "), New Font("Segoe UI", 9.0F), Theme.TexteGris))
        Controls.Add(pile)

        ' Largeur minimale commune pour aligner les tuiles
        MinimumSize = New Size(CInt(190 * DeviceDpi / 96.0), 0)
    End Sub

    Private Shared Function Texte(contenu As String, police As Font, couleur As Color) As Label
        Return New Label() With {.Text = contenu, .Font = police, .ForeColor = couleur, .AutoSize = True,
                                 .Margin = Padding.Empty, .BackColor = Color.Transparent}
    End Function

    ''' <summary>Filet bleu à gauche, aux couleurs GSB.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Using pinceau As New SolidBrush(Theme.BleuGsb)
            e.Graphics.FillRectangle(pinceau, 0, 0, 4, Height)
        End Using
    End Sub

End Class
