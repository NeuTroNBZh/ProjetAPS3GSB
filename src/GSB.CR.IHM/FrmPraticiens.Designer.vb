<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmPraticiens
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
        components = New System.ComponentModel.Container()
        pnlEntete = New Panel()
        lblSousTitre = New Label()
        lblTitre = New Label()
        pnlFiltres = New Panel()
        lblNombre = New Label()
        chkInactifs = New CheckBox()
        chkMonPortefeuille = New CheckBox()
        txtRecherche = New TextBox()
        lblRecherche = New Label()
        scPrincipal = New SplitContainer()
        dgvPraticiens = New DataGridView()
        pnlFiche = New PanneauFiche()
        pnlActions = New Panel()
        btnFermer = New Button()
        tmrRecherche = New Timer(components)
        pnlEntete.SuspendLayout()
        pnlFiltres.SuspendLayout()
        CType(scPrincipal, System.ComponentModel.ISupportInitialize).BeginInit()
        scPrincipal.Panel1.SuspendLayout()
        scPrincipal.Panel2.SuspendLayout()
        scPrincipal.SuspendLayout()
        CType(dgvPraticiens, System.ComponentModel.ISupportInitialize).BeginInit()
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
        lblTitre.Text = "Praticiens"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.Size = New Size(400, 19)
        lblSousTitre.TabIndex = 1
        lblSousTitre.Text = "Coordonnées, informations et historique des visites"
        '
        'pnlFiltres
        '
        pnlFiltres.Controls.Add(lblNombre)
        pnlFiltres.Controls.Add(chkInactifs)
        pnlFiltres.Controls.Add(chkMonPortefeuille)
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
        txtRecherche.PlaceholderText = "Nom, prénom ou ville…"
        txtRecherche.Size = New Size(280, 25)
        txtRecherche.TabIndex = 1
        '
        'chkMonPortefeuille
        '
        chkMonPortefeuille.AutoSize = True
        chkMonPortefeuille.Location = New Point(414, 18)
        chkMonPortefeuille.Name = "chkMonPortefeuille"
        chkMonPortefeuille.Size = New Size(210, 23)
        chkMonPortefeuille.TabIndex = 2
        chkMonPortefeuille.Text = "Seulement mon portefeuille"
        '
        'chkInactifs
        '
        chkInactifs.AutoSize = True
        chkInactifs.Location = New Point(640, 18)
        chkInactifs.Name = "chkInactifs"
        chkInactifs.Size = New Size(210, 23)
        chkInactifs.TabIndex = 3
        chkInactifs.Text = "Inclure les praticiens inactifs"
        '
        'lblNombre
        '
        lblNombre.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblNombre.Location = New Point(876, 19)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(300, 19)
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
        scPrincipal.Panel1.Controls.Add(dgvPraticiens)
        scPrincipal.Panel1.Padding = New Padding(24, 0, 6, 12)
        '
        'scPrincipal.Panel2
        '
        scPrincipal.Panel2.Controls.Add(pnlFiche)
        scPrincipal.Panel2.Padding = New Padding(6, 0, 24, 12)
        scPrincipal.Size = New Size(1200, 540)
        scPrincipal.SplitterDistance = 590
        scPrincipal.TabIndex = 2
        '
        'dgvPraticiens
        '
        dgvPraticiens.AllowUserToAddRows = False
        dgvPraticiens.AllowUserToDeleteRows = False
        dgvPraticiens.AllowUserToResizeRows = False
        dgvPraticiens.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvPraticiens.Dock = DockStyle.Fill
        dgvPraticiens.MultiSelect = False
        dgvPraticiens.Name = "dgvPraticiens"
        dgvPraticiens.ReadOnly = True
        dgvPraticiens.RowHeadersVisible = False
        dgvPraticiens.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvPraticiens.TabIndex = 0
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
        'tmrRecherche
        '
        tmrRecherche.Interval = 350
        '
        'FrmPraticiens
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
        Name = "FrmPraticiens"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Praticiens"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        pnlFiltres.ResumeLayout(False)
        pnlFiltres.PerformLayout()
        scPrincipal.Panel1.ResumeLayout(False)
        scPrincipal.Panel2.ResumeLayout(False)
        CType(scPrincipal, System.ComponentModel.ISupportInitialize).EndInit()
        scPrincipal.ResumeLayout(False)
        CType(dgvPraticiens, System.ComponentModel.ISupportInitialize).EndInit()
        pnlActions.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents pnlFiltres As Panel
    Friend WithEvents lblRecherche As Label
    Friend WithEvents txtRecherche As TextBox
    Friend WithEvents chkMonPortefeuille As CheckBox
    Friend WithEvents chkInactifs As CheckBox
    Friend WithEvents lblNombre As Label
    Friend WithEvents scPrincipal As SplitContainer
    Friend WithEvents dgvPraticiens As DataGridView
    Friend WithEvents pnlFiche As PanneauFiche
    Friend WithEvents pnlActions As Panel
    Friend WithEvents btnFermer As Button
    Friend WithEvents tmrRecherche As Timer

End Class
