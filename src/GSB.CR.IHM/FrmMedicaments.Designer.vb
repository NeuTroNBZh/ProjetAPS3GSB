<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMedicaments
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
        pnlFiltres = New Panel()
        lblNombre = New Label()
        chkRetires = New CheckBox()
        cboFamille = New ComboBox()
        txtRecherche = New TextBox()
        lblRecherche = New Label()
        scPrincipal = New SplitContainer()
        dgvMedicaments = New DataGridView()
        pnlFiche = New PanneauFiche()
        pnlActions = New Panel()
        btnFermer = New Button()
        pnlEntete.SuspendLayout()
        pnlFiltres.SuspendLayout()
        CType(scPrincipal, System.ComponentModel.ISupportInitialize).BeginInit()
        scPrincipal.Panel1.SuspendLayout()
        scPrincipal.Panel2.SuspendLayout()
        scPrincipal.SuspendLayout()
        CType(dgvMedicaments, System.ComponentModel.ISupportInitialize).BeginInit()
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
        pnlEntete.Size = New Size(1200, 68)
        pnlEntete.TabIndex = 0
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Font = New Font("Segoe UI Semibold", 16.0F)
        lblTitre.Location = New Point(22, 6)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(110, 30)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Médicaments"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.Size = New Size(400, 19)
        lblSousTitre.TabIndex = 1
        lblSousTitre.Text = "Composition, effets, contre-indications, interactions et posologie"
        '
        'pnlFiltres
        '
        pnlFiltres.Controls.Add(lblNombre)
        pnlFiltres.Controls.Add(chkRetires)
        pnlFiltres.Controls.Add(cboFamille)
        pnlFiltres.Controls.Add(txtRecherche)
        pnlFiltres.Controls.Add(lblRecherche)
        pnlFiltres.Dock = DockStyle.Top
        pnlFiltres.Location = New Point(0, 68)
        pnlFiltres.Name = "pnlFiltres"
        pnlFiltres.Size = New Size(1200, 56)
        pnlFiltres.TabIndex = 1
        '
        'lblRecherche
        '
        lblRecherche.AutoSize = True
        lblRecherche.Location = New Point(24, 19)
        lblRecherche.Name = "lblRecherche"
        lblRecherche.Size = New Size(80, 19)
        lblRecherche.TabIndex = 0
        lblRecherche.Text = "Rechercher"
        '
        'txtRecherche
        '
        txtRecherche.Location = New Point(112, 16)
        txtRecherche.Name = "txtRecherche"
        txtRecherche.PlaceholderText = "Nom commercial ou dépôt légal…"
        txtRecherche.Size = New Size(280, 25)
        txtRecherche.TabIndex = 1
        '
        'cboFamille
        '
        cboFamille.DropDownStyle = ComboBoxStyle.DropDownList
        cboFamille.Location = New Point(410, 16)
        cboFamille.Name = "cboFamille"
        cboFamille.Size = New Size(330, 25)
        cboFamille.TabIndex = 2
        '
        'chkRetires
        '
        chkRetires.AutoSize = True
        chkRetires.Location = New Point(760, 18)
        chkRetires.Name = "chkRetires"
        chkRetires.Size = New Size(210, 23)
        chkRetires.TabIndex = 3
        chkRetires.Text = "Inclure les médicaments retirés"
        '
        'lblNombre
        '
        lblNombre.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblNombre.Location = New Point(1000, 19)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(176, 19)
        lblNombre.TabIndex = 4
        lblNombre.TextAlign = ContentAlignment.MiddleRight
        '
        'scPrincipal
        '
        scPrincipal.Dock = DockStyle.Fill
        scPrincipal.Location = New Point(0, 124)
        scPrincipal.Name = "scPrincipal"
        '
        'scPrincipal.Panel1
        '
        scPrincipal.Panel1.Controls.Add(dgvMedicaments)
        scPrincipal.Panel1.Padding = New Padding(24, 0, 6, 12)
        '
        'scPrincipal.Panel2
        '
        scPrincipal.Panel2.Controls.Add(pnlFiche)
        scPrincipal.Panel2.Padding = New Padding(6, 0, 24, 12)
        scPrincipal.Size = New Size(1200, 540)
        scPrincipal.SplitterDistance = 560
        scPrincipal.TabIndex = 2
        '
        'dgvMedicaments
        '
        dgvMedicaments.AllowUserToAddRows = False
        dgvMedicaments.AllowUserToDeleteRows = False
        dgvMedicaments.AllowUserToResizeRows = False
        dgvMedicaments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvMedicaments.Dock = DockStyle.Fill
        dgvMedicaments.MultiSelect = False
        dgvMedicaments.Name = "dgvMedicaments"
        dgvMedicaments.ReadOnly = True
        dgvMedicaments.RowHeadersVisible = False
        dgvMedicaments.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvMedicaments.TabIndex = 0
        '
        'pnlFiche
        '
        pnlFiche.BorderStyle = BorderStyle.FixedSingle
        pnlFiche.Dock = DockStyle.Fill
        pnlFiche.Name = "pnlFiche"
        pnlFiche.TabIndex = 0
        '
        'pnlActions
        '
        pnlActions.Controls.Add(btnFermer)
        pnlActions.Dock = DockStyle.Bottom
        pnlActions.Location = New Point(0, 664)
        pnlActions.Name = "pnlActions"
        pnlActions.Size = New Size(1200, 60)
        pnlActions.TabIndex = 3
        '
        'btnFermer
        '
        btnFermer.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnFermer.DialogResult = DialogResult.Cancel
        btnFermer.Location = New Point(1056, 11)
        btnFermer.Name = "btnFermer"
        btnFermer.Size = New Size(120, 38)
        btnFermer.TabIndex = 0
        btnFermer.Text = "Fermer"
        '
        'FrmMedicaments
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnFermer
        ClientSize = New Size(1200, 724)
        Controls.Add(scPrincipal)
        Controls.Add(pnlActions)
        Controls.Add(pnlFiltres)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(900, 520)
        Name = "FrmMedicaments"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Médicaments"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        pnlFiltres.ResumeLayout(False)
        pnlFiltres.PerformLayout()
        scPrincipal.Panel1.ResumeLayout(False)
        scPrincipal.Panel2.ResumeLayout(False)
        CType(scPrincipal, System.ComponentModel.ISupportInitialize).EndInit()
        scPrincipal.ResumeLayout(False)
        CType(dgvMedicaments, System.ComponentModel.ISupportInitialize).EndInit()
        pnlActions.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents pnlFiltres As Panel
    Friend WithEvents lblRecherche As Label
    Friend WithEvents txtRecherche As TextBox
    Friend WithEvents cboFamille As ComboBox
    Friend WithEvents chkRetires As CheckBox
    Friend WithEvents lblNombre As Label
    Friend WithEvents scPrincipal As SplitContainer
    Friend WithEvents dgvMedicaments As DataGridView
    Friend WithEvents pnlFiche As PanneauFiche
    Friend WithEvents pnlActions As Panel
    Friend WithEvents btnFermer As Button

End Class
