<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMonActivite
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        pnlEntete = New Panel()
        lblSousTitre = New Label()
        lblTitre = New Label()
        pnlPeriode = New Panel()
        lblPeriode = New Label()
        cboPeriode = New ComboBox()
        lblDu = New Label()
        dtpDebut = New DateTimePicker()
        lblAu = New Label()
        dtpFin = New DateTimePicker()
        btnAfficher = New Button()
        tabOnglets = New TabControl()
        tabSynthese = New TabPage()
        pnlSynthese = New PanneauFiche()
        tabARevoir = New TabPage()
        dgvARevoir = New DataGridView()
        pnlARevoirHaut = New Panel()
        btnNouveauCR = New Button()
        lblARevoir = New Label()
        pnlActions = New Panel()
        btnFermer = New Button()
        btnExporter = New Button()
        pnlEntete.SuspendLayout()
        pnlPeriode.SuspendLayout()
        tabOnglets.SuspendLayout()
        tabSynthese.SuspendLayout()
        tabARevoir.SuspendLayout()
        CType(dgvARevoir, System.ComponentModel.ISupportInitialize).BeginInit()
        pnlARevoirHaut.SuspendLayout()
        pnlActions.SuspendLayout()
        SuspendLayout()
        '
        'pnlEntete
        '
        pnlEntete.Controls.Add(lblSousTitre)
        pnlEntete.Controls.Add(lblTitre)
        pnlEntete.Dock = DockStyle.Top
        pnlEntete.Location = New Point(0, 0)
        pnlEntete.Name = "pnlEntete"
        pnlEntete.Size = New Size(1100, 68)
        pnlEntete.TabIndex = 0
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Font = New Font("Segoe UI Semibold", 16.0F)
        lblTitre.Location = New Point(22, 6)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(140, 30)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Mon activité"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.Size = New Size(400, 19)
        lblSousTitre.TabIndex = 1
        lblSousTitre.Text = "Synthèse des comptes-rendus validés et praticiens à revoir"
        '
        'pnlPeriode
        '
        pnlPeriode.Controls.Add(btnAfficher)
        pnlPeriode.Controls.Add(dtpFin)
        pnlPeriode.Controls.Add(lblAu)
        pnlPeriode.Controls.Add(dtpDebut)
        pnlPeriode.Controls.Add(lblDu)
        pnlPeriode.Controls.Add(cboPeriode)
        pnlPeriode.Controls.Add(lblPeriode)
        pnlPeriode.Dock = DockStyle.Top
        pnlPeriode.Location = New Point(0, 68)
        pnlPeriode.Name = "pnlPeriode"
        pnlPeriode.Size = New Size(1100, 58)
        pnlPeriode.TabIndex = 1
        '
        'lblPeriode
        '
        lblPeriode.AutoSize = True
        lblPeriode.Location = New Point(24, 20)
        lblPeriode.Name = "lblPeriode"
        lblPeriode.Size = New Size(60, 19)
        lblPeriode.TabIndex = 0
        lblPeriode.Text = "Période"
        '
        'cboPeriode
        '
        cboPeriode.DropDownStyle = ComboBoxStyle.DropDownList
        cboPeriode.Location = New Point(92, 17)
        cboPeriode.Name = "cboPeriode"
        cboPeriode.Size = New Size(220, 25)
        cboPeriode.TabIndex = 1
        '
        'lblDu
        '
        lblDu.AutoSize = True
        lblDu.Location = New Point(332, 20)
        lblDu.Name = "lblDu"
        lblDu.Size = New Size(25, 19)
        lblDu.TabIndex = 2
        lblDu.Text = "du"
        '
        'dtpDebut
        '
        dtpDebut.Format = DateTimePickerFormat.Short
        dtpDebut.Location = New Point(362, 17)
        dtpDebut.Name = "dtpDebut"
        dtpDebut.Size = New Size(130, 25)
        dtpDebut.TabIndex = 3
        '
        'lblAu
        '
        lblAu.AutoSize = True
        lblAu.Location = New Point(502, 20)
        lblAu.Name = "lblAu"
        lblAu.Size = New Size(25, 19)
        lblAu.TabIndex = 4
        lblAu.Text = "au"
        '
        'dtpFin
        '
        dtpFin.Format = DateTimePickerFormat.Short
        dtpFin.Location = New Point(532, 17)
        dtpFin.Name = "dtpFin"
        dtpFin.Size = New Size(130, 25)
        dtpFin.TabIndex = 5
        '
        'btnAfficher
        '
        btnAfficher.Location = New Point(680, 12)
        btnAfficher.Name = "btnAfficher"
        btnAfficher.Size = New Size(120, 34)
        btnAfficher.TabIndex = 6
        btnAfficher.Text = "Afficher"
        '
        'tabOnglets
        '
        tabOnglets.Controls.Add(tabSynthese)
        tabOnglets.Controls.Add(tabARevoir)
        tabOnglets.Dock = DockStyle.Fill
        tabOnglets.Location = New Point(0, 126)
        tabOnglets.Name = "tabOnglets"
        tabOnglets.Padding = New Point(16, 6)
        tabOnglets.SelectedIndex = 0
        tabOnglets.Size = New Size(1100, 550)
        tabOnglets.TabIndex = 2
        '
        'tabSynthese
        '
        tabSynthese.Controls.Add(pnlSynthese)
        tabSynthese.Location = New Point(4, 34)
        tabSynthese.Name = "tabSynthese"
        tabSynthese.Size = New Size(1092, 512)
        tabSynthese.TabIndex = 0
        tabSynthese.Text = "Synthèse"
        '
        'pnlSynthese
        '
        pnlSynthese.Dock = DockStyle.Fill
        pnlSynthese.Name = "pnlSynthese"
        pnlSynthese.TabIndex = 0
        '
        'tabARevoir
        '
        tabARevoir.Controls.Add(dgvARevoir)
        tabARevoir.Controls.Add(pnlARevoirHaut)
        tabARevoir.Location = New Point(4, 34)
        tabARevoir.Name = "tabARevoir"
        tabARevoir.Padding = New Padding(18, 0, 18, 12)
        tabARevoir.Size = New Size(1092, 512)
        tabARevoir.TabIndex = 1
        tabARevoir.Text = "Praticiens à revoir"
        '
        'pnlARevoirHaut
        '
        pnlARevoirHaut.Controls.Add(btnNouveauCR)
        pnlARevoirHaut.Controls.Add(lblARevoir)
        pnlARevoirHaut.Dock = DockStyle.Top
        pnlARevoirHaut.Name = "pnlARevoirHaut"
        pnlARevoirHaut.Size = New Size(1056, 60)
        pnlARevoirHaut.TabIndex = 0
        '
        'lblARevoir
        '
        lblARevoir.Location = New Point(0, 10)
        lblARevoir.Name = "lblARevoir"
        lblARevoir.Size = New Size(760, 42)
        lblARevoir.TabIndex = 0
        '
        'btnNouveauCR
        '
        btnNouveauCR.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnNouveauCR.Location = New Point(810, 12)
        btnNouveauCR.Name = "btnNouveauCR"
        btnNouveauCR.Size = New Size(246, 36)
        btnNouveauCR.TabIndex = 1
        btnNouveauCR.Text = "Saisir un compte-rendu"
        '
        'dgvARevoir
        '
        dgvARevoir.AllowUserToAddRows = False
        dgvARevoir.AllowUserToDeleteRows = False
        dgvARevoir.AllowUserToResizeRows = False
        dgvARevoir.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvARevoir.Dock = DockStyle.Fill
        dgvARevoir.MultiSelect = False
        dgvARevoir.Name = "dgvARevoir"
        dgvARevoir.ReadOnly = True
        dgvARevoir.RowHeadersVisible = False
        dgvARevoir.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvARevoir.TabIndex = 1
        '
        'pnlActions
        '
        pnlActions.Controls.Add(btnExporter)
        pnlActions.Controls.Add(btnFermer)
        pnlActions.Dock = DockStyle.Bottom
        pnlActions.Location = New Point(0, 676)
        pnlActions.Name = "pnlActions"
        pnlActions.Size = New Size(1100, 60)
        pnlActions.TabIndex = 3
        '
        'btnFermer
        '
        btnFermer.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnFermer.DialogResult = DialogResult.Cancel
        btnFermer.Location = New Point(956, 11)
        btnFermer.Name = "btnFermer"
        btnFermer.Size = New Size(120, 38)
        btnFermer.TabIndex = 0
        btnFermer.Text = "Fermer"
        '
        'btnExporter
        '
        btnExporter.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        btnExporter.Enabled = False
        btnExporter.Location = New Point(24, 11)
        btnExporter.Name = "btnExporter"
        btnExporter.Size = New Size(180, 38)
        btnExporter.TabIndex = 1
        btnExporter.Text = "Exporter en CSV"
        '
        'FrmMonActivite
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnFermer
        ClientSize = New Size(1100, 736)
        Controls.Add(tabOnglets)
        Controls.Add(pnlActions)
        Controls.Add(pnlPeriode)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(900, 560)
        Name = "FrmMonActivite"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Mon activité"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        pnlPeriode.ResumeLayout(False)
        pnlPeriode.PerformLayout()
        tabOnglets.ResumeLayout(False)
        tabSynthese.ResumeLayout(False)
        tabARevoir.ResumeLayout(False)
        CType(dgvARevoir, System.ComponentModel.ISupportInitialize).EndInit()
        pnlARevoirHaut.ResumeLayout(False)
        pnlActions.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents pnlPeriode As Panel
    Friend WithEvents lblPeriode As Label
    Friend WithEvents cboPeriode As ComboBox
    Friend WithEvents lblDu As Label
    Friend WithEvents dtpDebut As DateTimePicker
    Friend WithEvents lblAu As Label
    Friend WithEvents dtpFin As DateTimePicker
    Friend WithEvents btnAfficher As Button
    Friend WithEvents tabOnglets As TabControl
    Friend WithEvents tabSynthese As TabPage
    Friend WithEvents pnlSynthese As PanneauFiche
    Friend WithEvents tabARevoir As TabPage
    Friend WithEvents pnlARevoirHaut As Panel
    Friend WithEvents lblARevoir As Label
    Friend WithEvents btnNouveauCR As Button
    Friend WithEvents dgvARevoir As DataGridView
    Friend WithEvents pnlActions As Panel
    Friend WithEvents btnFermer As Button
    Friend WithEvents btnExporter As Button

End Class
