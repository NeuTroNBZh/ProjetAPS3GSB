Imports System.ComponentModel
Imports GSB.CR.Metier

''' <summary>
''' Tuile cliquable du menu principal : titre du module en gras et description en dessous.
''' </summary>
Public Class TuileModule
    Inherits Panel

    Private ReadOnly _lblTitre As New Label()
    Private ReadOnly _lblDescription As New Label()

    ''' <summary>Module ouvert par la tuile.</summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ModuleAssocie As ModuleApplication

    Public Sub New(moduleAssocie As ModuleApplication)
        Me.ModuleAssocie = moduleAssocie
        Size = New Size(290, 110)
        Margin = New Padding(8)
        Padding = New Padding(16, 14, 12, 10)
        BackColor = Theme.BleuClair
        Cursor = Cursors.Hand
        BorderStyle = BorderStyle.None

        _lblTitre.Text = LibellesModules.Titre(moduleAssocie)
        _lblTitre.Font = New Font("Segoe UI Semibold", 12.0F)
        _lblTitre.ForeColor = Theme.BleuFonce
        _lblTitre.Dock = DockStyle.Top
        _lblTitre.Height = 36

        _lblDescription.Text = LibellesModules.Description(moduleAssocie)
        _lblDescription.Font = New Font("Segoe UI", 9.5F)
        _lblDescription.ForeColor = Theme.TexteGris
        _lblDescription.Dock = DockStyle.Fill

        Controls.Add(_lblDescription)
        Controls.Add(_lblTitre)

        ' Toute la surface de la tuile est cliquable et réagit au survol
        For Each c As Control In {_lblTitre, _lblDescription}
            c.Cursor = Cursors.Hand
            AddHandler c.Click, Sub(s, e) OnClick(e)
            AddHandler c.MouseEnter, Sub(s, e) Survoler(True)
            AddHandler c.MouseLeave, Sub(s, e) Survoler(ClientRectangle.Contains(PointToClient(MousePosition)))
        Next
        AddHandler MouseEnter, Sub(s, e) Survoler(True)
        AddHandler MouseLeave, Sub(s, e) Survoler(ClientRectangle.Contains(PointToClient(MousePosition)))
    End Sub

    ''' <summary>Remplace la description ; <paramref name="alerte"/> la met en évidence (ex. messages non lus).</summary>
    Public Sub DefinirDescription(texte As String, alerte As Boolean)
        _lblDescription.Text = texte
        _lblDescription.ForeColor = If(alerte, Theme.Erreur, Theme.TexteGris)
        _lblDescription.Font = New Font("Segoe UI", 9.5F, If(alerte, FontStyle.Bold, FontStyle.Regular))
    End Sub

    Private Sub Survoler(actif As Boolean)
        BackColor = If(actif, Theme.BleuSurvol, Theme.BleuClair)
    End Sub

    ''' <summary>Bordure bleue et bandeau gauche aux couleurs GSB.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Using stylo As New Pen(Theme.BleuGsb)
            e.Graphics.DrawRectangle(stylo, 0, 0, Width - 1, Height - 1)
        End Using
        Using pinceau As New SolidBrush(Theme.BleuGsb)
            e.Graphics.FillRectangle(pinceau, 0, 0, 5, Height)
        End Using
    End Sub

End Class
