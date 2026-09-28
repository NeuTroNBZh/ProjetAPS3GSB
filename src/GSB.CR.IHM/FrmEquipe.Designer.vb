<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmEquipe
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
        tabVisiteurs = New TabPage()
        dgvVisiteurs = New DataGridView()
        pnlVisiteursHaut = New Panel()
        btnDetailVisiteur = New Button()
        lblVisiteurs = New Label()
        tabRapports = New TabPage()
        dgvRapports = New DataGridView()
        pnlRapportsHaut = New Panel()
        btnConsulter = New Button()
        lblNbRapports = New Label()
        txtRecherche = New TextBox()
        cboVisiteur = New ComboBox()
        lblFiltreVisiteur = New Label()
        tabARevoir = New TabPage()
        dgvARevoir = New DataGridView()
        lblARevoir = New Label()
        pnlActions = New Panel()
        btnFermer = New Button()
        btnExporter = New Button()
        pnlEntete.SuspendLayout()
        pnlPeriode.SuspendLayout()
        tabOnglets.SuspendLayout()
        tabSynthese.SuspendLayout()
        tabVisiteurs.SuspendLayout()
        CType(dgvVisiteurs, System.ComponentModel.ISupportInitialize).BeginInit()
        pnlVisiteursHaut.SuspendLayout()
        tabRapports.SuspendLayout()
        CType(dgvRapports, System.ComponentModel.ISupportInitialize).BeginInit()
        pnlRapportsHaut.SuspendLayout()
        tabARevoir.SuspendLayout()
        CType(dgvARevoir, System.ComponentModel.ISupportInitialize).BeginInit()
        pnlActions.SuspendLayout()
        SuspendLayout()
        '
        'pnlEntete
        '
        pnlEntete.Controls.Add(lblSousTitre)
        pnlEntete.Controls.Add(lblTitre)
        pnlEntete.Dock = DockStyle.Top
        pnlEntete.Name = "pnlEntete"
        pnlEntete.Size = New Size(1180, 68)
        pnlEntete.TabIndex = 0
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Font = New Font("Segoe UI Semibold", 16.0F)
        lblTitre.Location = New Point(22, 6)
        lblTitre.Name = "lblTitre"
        lblTitre.TabIndex = 0
        lblTitre.Text = "Mon équipe"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.TabIndex = 1
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
        pnlPeriode.Name = "pnlPeriode"
        pnlPeriode.Size = New Size(1180, 58)
        pnlPeriode.TabIndex = 1
        '
        'lblPeriode
        '
        lblPeriode.AutoSize = True
        lblPeriode.Location = New Point(24, 20)
        lblPeriode.Name = "lblPeriode"
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
        tabOnglets.Controls.Add(tabVisiteurs)
        tabOnglets.Controls.Add(tabRapports)
        tabOnglets.Controls.Add(tabARevoir)
        tabOnglets.Dock = DockStyle.Fill
        tabOnglets.Name = "tabOnglets"
        tabOnglets.Padding = New Point(16, 6)
        tabOnglets.SelectedIndex = 0
        tabOnglets.TabIndex = 2
        '
        'tabSynthese
        '
        tabSynthese.Controls.Add(pnlSynthese)
        tabSynthese.Name = "tabSynthese"
        tabSynthese.TabIndex = 0
        tabSynthese.Text = "Synthèse"
        '
        'pnlSynthese
        '
        pnlSynthese.Dock = DockStyle.Fill
        pnlSynthese.Name = "pnlSynthese"
        pnlSynthese.TabIndex = 0
        '
        'tabVisiteurs
        '
        tabVisiteurs.Controls.Add(dgvVisiteurs)
        tabVisiteurs.Controls.Add(pnlVisiteursHaut)
        tabVisiteurs.Name = "tabVisiteurs"
        tabVisiteurs.Padding = New Padding(18, 0, 18, 12)
        tabVisiteurs.TabIndex = 1
        tabVisiteurs.Text = "Visiteurs"
        '
        'pnlVisiteursHaut
        '
        pnlVisiteursHaut.Controls.Add(btnDetailVisiteur)
        pnlVisiteursHaut.Controls.Add(lblVisiteurs)
        pnlVisiteursHaut.Dock = DockStyle.Top
        pnlVisiteursHaut.Name = "pnlVisiteursHaut"
        pnlVisiteursHaut.Size = New Size(1136, 60)
        pnlVisiteursHaut.TabIndex = 0
        '
        'lblVisiteurs
        '
        lblVisiteurs.Location = New Point(0, 10)
        lblVisiteurs.Name = "lblVisiteurs"
        lblVisiteurs.Size = New Size(820, 42)
        lblVisiteurs.TabIndex = 0
        lblVisiteurs.Text = "Activité de chaque membre sur la période (comptes-rendus validés). Double-cliquez pour le détail."
        '
        'btnDetailVisiteur
        '
        btnDetailVisiteur.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnDetailVisiteur.Location = New Point(890, 12)
        btnDetailVisiteur.Name = "btnDetailVisiteur"
        btnDetailVisiteur.Size = New Size(246, 36)
        btnDetailVisiteur.TabIndex = 1
        btnDetailVisiteur.Text = "Voir son activité"
        '
        'dgvVisiteurs
        '
        dgvVisiteurs.AllowUserToAddRows = False
        dgvVisiteurs.AllowUserToDeleteRows = False
        dgvVisiteurs.AllowUserToResizeRows = False
        dgvVisiteurs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvVisiteurs.Dock = DockStyle.Fill
        dgvVisiteurs.MultiSelect = False
        dgvVisiteurs.Name = "dgvVisiteurs"
        dgvVisiteurs.ReadOnly = True
        dgvVisiteurs.RowHeadersVisible = False
        dgvVisiteurs.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvVisiteurs.TabIndex = 1
        '
        'tabRapports
        '
        tabRapports.Controls.Add(dgvRapports)
        tabRapports.Controls.Add(pnlRapportsHaut)
        tabRapports.Name = "tabRapports"
        tabRapports.Padding = New Padding(18, 0, 18, 12)
        tabRapports.TabIndex = 2
        tabRapports.Text = "Comptes-rendus"
        '
        'pnlRapportsHaut
        '
        pnlRapportsHaut.Controls.Add(btnConsulter)
        pnlRapportsHaut.Controls.Add(lblNbRapports)
        pnlRapportsHaut.Controls.Add(txtRecherche)
        pnlRapportsHaut.Controls.Add(cboVisiteur)
        pnlRapportsHaut.Controls.Add(lblFiltreVisiteur)
        pnlRapportsHaut.Dock = DockStyle.Top
        pnlRapportsHaut.Name = "pnlRapportsHaut"
        pnlRapportsHaut.Size = New Size(1136, 60)
        pnlRapportsHaut.TabIndex = 0
        '
        'lblFiltreVisiteur
        '
        lblFiltreVisiteur.AutoSize = True
        lblFiltreVisiteur.Location = New Point(0, 20)
        lblFiltreVisiteur.Name = "lblFiltreVisiteur"
        lblFiltreVisiteur.TabIndex = 0
        lblFiltreVisiteur.Text = "Visiteur"
        '
        'cboVisiteur
        '
        cboVisiteur.DropDownStyle = ComboBoxStyle.DropDownList
        cboVisiteur.Location = New Point(66, 17)
        cboVisiteur.Name = "cboVisiteur"
        cboVisiteur.Size = New Size(250, 25)
        cboVisiteur.TabIndex = 1
        '
        'txtRecherche
        '
        txtRecherche.Location = New Point(332, 17)
        txtRecherche.Name = "txtRecherche"
        txtRecherche.PlaceholderText = "Praticien ou ville…"
        txtRecherche.Size = New Size(240, 25)
        txtRecherche.TabIndex = 2
        '
        'lblNbRapports
        '
        lblNbRapports.Location = New Point(590, 20)
        lblNbRapports.Name = "lblNbRapports"
        lblNbRapports.Size = New Size(280, 19)
        lblNbRapports.TabIndex = 3
        '
        'btnConsulter
        '
        btnConsulter.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnConsulter.Location = New Point(890, 12)
        btnConsulter.Name = "btnConsulter"
        btnConsulter.Size = New Size(246, 36)
        btnConsulter.TabIndex = 4
        btnConsulter.Text = "Consulter le compte-rendu"
        '
        'dgvRapports
        '
        dgvRapports.AllowUserToAddRows = False
        dgvRapports.AllowUserToDeleteRows = False
        dgvRapports.AllowUserToResizeRows = False
        dgvRapports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvRapports.Dock = DockStyle.Fill
        dgvRapports.MultiSelect = False
        dgvRapports.Name = "dgvRapports"
        dgvRapports.ReadOnly = True
        dgvRapports.RowHeadersVisible = False
        dgvRapports.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRapports.TabIndex = 1
        '
        'tabARevoir
        '
        tabARevoir.Controls.Add(dgvARevoir)
        tabARevoir.Controls.Add(lblARevoir)
        tabARevoir.Name = "tabARevoir"
        tabARevoir.Padding = New Padding(18, 0, 18, 12)
        tabARevoir.TabIndex = 3
        tabARevoir.Text = "Praticiens à revoir"
        '
        'lblARevoir
        '
        lblARevoir.Dock = DockStyle.Top
        lblARevoir.Name = "lblARevoir"
        lblARevoir.Padding = New Padding(0, 12, 0, 0)
        lblARevoir.Size = New Size(1136, 52)
        lblARevoir.TabIndex = 0
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
        pnlActions.Name = "pnlActions"
        pnlActions.Size = New Size(1180, 60)
        pnlActions.TabIndex = 3
        '
        'btnFermer
        '
        btnFermer.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnFermer.DialogResult = DialogResult.Cancel
        btnFermer.Location = New Point(1036, 11)
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
        'FrmEquipe
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnFermer
        ClientSize = New Size(1180, 760)
        Controls.Add(tabOnglets)
        Controls.Add(pnlActions)
        Controls.Add(pnlPeriode)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(960, 580)
        Name = "FrmEquipe"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Mon équipe"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        pnlPeriode.ResumeLayout(False)
        pnlPeriode.PerformLayout()
        tabOnglets.ResumeLayout(False)
        tabSynthese.ResumeLayout(False)
        tabVisiteurs.ResumeLayout(False)
        CType(dgvVisiteurs, System.ComponentModel.ISupportInitialize).EndInit()
        pnlVisiteursHaut.ResumeLayout(False)
        tabRapports.ResumeLayout(False)
        CType(dgvRapports, System.ComponentModel.ISupportInitialize).EndInit()
        pnlRapportsHaut.ResumeLayout(False)
        pnlRapportsHaut.PerformLayout()
        tabARevoir.ResumeLayout(False)
        CType(dgvARevoir, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents tabVisiteurs As TabPage
    Friend WithEvents pnlVisiteursHaut As Panel
    Friend WithEvents lblVisiteurs As Label
    Friend WithEvents btnDetailVisiteur As Button
    Friend WithEvents dgvVisiteurs As DataGridView
    Friend WithEvents tabRapports As TabPage
    Friend WithEvents pnlRapportsHaut As Panel
    Friend WithEvents lblFiltreVisiteur As Label
    Friend WithEvents cboVisiteur As ComboBox
    Friend WithEvents txtRecherche As TextBox
    Friend WithEvents lblNbRapports As Label
    Friend WithEvents btnConsulter As Button
    Friend WithEvents dgvRapports As DataGridView
    Friend WithEvents tabARevoir As TabPage
    Friend WithEvents lblARevoir As Label
    Friend WithEvents dgvARevoir As DataGridView
    Friend WithEvents pnlActions As Panel
    Friend WithEvents btnFermer As Button
    Friend WithEvents btnExporter As Button

End Class
