Imports System.Drawing.Drawing2D

''' <summary>
''' Histogramme d'une seule série (ex. visites par mois), dessiné en GDI+ :
''' barres de 24 px maximum au sommet arrondi et à la base droite, 2 px d'écart minimum entre barres,
''' grille fine et discrète, graduations rondes, étiquette uniquement sur la valeur maximale,
''' valeur exacte au survol de toute la colonne. Les textes n'utilisent jamais la couleur de la série.
''' </summary>
Public Class GraphiqueBarres
    Inherits Control

    Private Shared ReadOnly CouleurGrille As Color = Color.FromArgb(227, 231, 236)
    Private Shared ReadOnly CouleurBase As Color = Color.FromArgb(190, 197, 206)
    Private Shared ReadOnly EncreTexte As Color = Color.FromArgb(31, 41, 51)

    Private _donnees As IReadOnlyList(Of (Libelle As String, Valeur As Integer, Detail As String)) =
        Array.Empty(Of (String, Integer, String))()
    Private _survol As Integer = -1
    Private ReadOnly _infobulle As New ToolTip() With {.InitialDelay = 0, .ReshowDelay = 0}
    Private _zones As New List(Of Rectangle)
    Private _policeGras As Font
    Private _policeItalique As Font

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        BackColor = Theme.Blanc
        Font = New Font("Segoe UI", 9.0F)
        Height = 230
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
        MyBase.OnFontChanged(e)
        _policeGras?.Dispose()
        _policeItalique?.Dispose()
        _policeGras = New Font(Font, FontStyle.Bold)
        _policeItalique = New Font(Font, FontStyle.Italic)
    End Sub

    ''' <summary>Message affiché quand toutes les valeurs sont nulles.</summary>
    <System.ComponentModel.DefaultValue("Aucune donnée sur la période.")>
    Public Property MessageVide As String = "Aucune donnée sur la période."

    ''' <summary>
    ''' Données à afficher : libellé d'axe, valeur, et texte de l'infobulle (ex. « septembre 2026 : 2 visites »).
    ''' </summary>
    Public Sub DefinirDonnees(donnees As IEnumerable(Of (Libelle As String, Valeur As Integer, Detail As String)))
        _donnees = donnees.ToList()
        _survol = -1
        Invalidate()
    End Sub

    Private Function Echelle(pixels As Single) As Single
        Return pixels * DeviceDpi / 96.0F
    End Function

    ''' <summary>Maximum de l'axe arrondi à une valeur « ronde » et pas des graduations.</summary>
    Private Shared Function Graduations(maximum As Integer) As (Haut As Integer, Pas As Integer)
        If maximum <= 4 Then Return (Math.Max(maximum, 1), 1)
        Dim brut = maximum / 4.0
        Dim puissance = Math.Pow(10, Math.Floor(Math.Log10(brut)))
        Dim pas = CInt({1, 2, 5, 10}.Select(Function(m) m * puissance).First(Function(v) v >= brut))
        Return (CInt(Math.Ceiling(maximum / CDbl(pas))) * pas, pas)
    End Function

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        _zones = New List(Of Rectangle)

        If _donnees.Count = 0 OrElse _donnees.All(Function(d) d.Valeur = 0) Then
            TextRenderer.DrawText(g, MessageVide, _policeItalique, ClientRectangle, Theme.TexteGris,
                                  TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            Return
        End If

        Dim axe = Graduations(_donnees.Max(Function(d) d.Valeur))
        Dim largeurAxe = TextRenderer.MeasureText(axe.Haut.ToString("N0"), Font).Width + Echelle(8)
        Dim hauteurTexte = TextRenderer.MeasureText("Ag", Font).Height
        Dim zone As New RectangleF(largeurAxe, hauteurTexte + Echelle(6), Width - largeurAxe - Echelle(8),
                                   Height - hauteurTexte * 2 - Echelle(14))
        If zone.Width < 20 OrElse zone.Height < 20 Then Return

        ' Grille horizontale fine + graduations
        Using stylo As New Pen(CouleurGrille, 1), styloBase As New Pen(CouleurBase, 1)
            For v = 0 To axe.Haut Step axe.Pas
                Dim y = zone.Bottom - CSng(v / axe.Haut) * zone.Height
                g.DrawLine(If(v = 0, styloBase, stylo), zone.Left, y, zone.Right, y)
                TextRenderer.DrawText(g, v.ToString("N0"), Font,
                                      New Rectangle(0, CInt(y - hauteurTexte / 2), CInt(largeurAxe - Echelle(6)), hauteurTexte),
                                      Theme.TexteGris, TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)
            Next
        End Using

        ' Barres
        Dim bande = zone.Width / _donnees.Count
        Dim largeurBarre = Math.Max(2, Math.Min(Echelle(24), bande - Echelle(2)))
        Dim rayon = Math.Min(Echelle(4), largeurBarre / 2)
        ' Étiquette sélective : le maximum, seulement s'il est unique (sinon l'axe suffit)
        Dim valeurMax = _donnees.Max(Function(d) d.Valeur)
        Dim iMax = If(_donnees.Where(Function(d) d.Valeur = valeurMax).Count() = 1,
                      Enumerable.Range(0, _donnees.Count).First(Function(i) _donnees(i).Valeur = valeurMax), -1)

        ' Un libellé d'axe sur combien, pour éviter les chevauchements
        Dim largeurLibelle = _donnees.Max(Function(d) TextRenderer.MeasureText(d.Libelle, Font).Width) + Echelle(6)
        Dim frequence = Math.Max(1, CInt(Math.Ceiling(largeurLibelle / bande)))

        For i = 0 To _donnees.Count - 1
            Dim d = _donnees(i)
            Dim centre = zone.Left + bande * (i + 0.5F)
            _zones.Add(Rectangle.Round(New RectangleF(zone.Left + bande * i, zone.Top, bande, zone.Height + hauteurTexte)))

            If d.Valeur > 0 Then
                Dim h = CSng(d.Valeur / axe.Haut) * zone.Height
                Dim barre As New RectangleF(centre - largeurBarre / 2, zone.Bottom - h, largeurBarre, h)
                Using pinceau As New SolidBrush(If(i = _survol, Theme.BleuFonce, Theme.BleuGsb))
                    g.FillPath(pinceau, SommetArrondi(barre, Math.Min(rayon, h)))
                End Using
                ' Étiquette sélective : seulement le maximum (le reste est dans l'infobulle et le tableau)
                If i = iMax Then
                    TextRenderer.DrawText(g, d.Valeur.ToString("N0"), _policeGras,
                                          New Rectangle(CInt(centre - bande / 2), CInt(barre.Top - hauteurTexte - Echelle(2)), CInt(bande), hauteurTexte),
                                          EncreTexte, TextFormatFlags.HorizontalCenter Or TextFormatFlags.Bottom)
                End If
            End If

            If i Mod frequence = 0 Then
                TextRenderer.DrawText(g, d.Libelle, Font,
                                      New Rectangle(CInt(centre - largeurLibelle / 2), CInt(zone.Bottom + Echelle(4)), CInt(largeurLibelle), hauteurTexte),
                                      Theme.TexteGris, TextFormatFlags.HorizontalCenter)
            End If
        Next
    End Sub

    ''' <summary>Rectangle au sommet arrondi et à la base droite (la barre part de la ligne de base).</summary>
    Private Shared Function SommetArrondi(r As RectangleF, rayon As Single) As GraphicsPath
        Dim chemin As New GraphicsPath()
        If rayon <= 0.5F Then
            chemin.AddRectangle(r)
            Return chemin
        End If
        Dim d = rayon * 2
        chemin.AddLine(r.Left, r.Bottom, r.Left, r.Top + rayon)
        chemin.AddArc(r.Left, r.Top, d, d, 180, 90)
        chemin.AddArc(r.Right - d, r.Top, d, d, 270, 90)
        chemin.AddLine(r.Right, r.Top + rayon, r.Right, r.Bottom)
        chemin.CloseFigure()
        Return chemin
    End Function

    ' --- Survol : toute la colonne est la zone de détection -----------------------

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        Dim i = _zones.FindIndex(Function(z) z.Contains(e.Location))
        If i = _survol Then Return
        _survol = i
        Invalidate()
        If i >= 0 AndAlso i < _donnees.Count Then
            _infobulle.Show(_donnees(i).Detail, Me, e.X + CInt(Echelle(12)), e.Y - CInt(Echelle(8)))
        Else
            _infobulle.Hide(Me)
        End If
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        _survol = -1
        _infobulle.Hide(Me)
        Invalidate()
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            _infobulle.Dispose()
            _policeGras?.Dispose()
            _policeItalique?.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

End Class
