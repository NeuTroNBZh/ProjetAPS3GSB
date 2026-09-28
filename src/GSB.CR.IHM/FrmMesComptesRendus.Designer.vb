<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMesComptesRendus
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
        lblAfficher = New Label()
        cboEtat = New ComboBox()
        lblRecherche = New Label()
        txtRecherche = New TextBox()
        lblNombre = New Label()
        dgvRapports = New DataGridView()
        pnlActions = New Panel()
        lblStatut = New Label()
        btnFermer = New Button()
        btnSupprimer = New Button()
        btnOuvrir = New Button()
        btnNouveau = New Button()
        pnlEntete.SuspendLayout()
        CType(dgvRapports, System.ComponentModel.ISupportInitialize).BeginInit()
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
        pnlEntete.Size = New Size(1080, 68)
        pnlEntete.TabIndex = 0
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Font = New Font("Segoe UI Semibold", 16.0F)
        lblTitre.Location = New Point(22, 6)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(210, 30)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Mes comptes-rendus"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.Size = New Size(250, 19)
        lblSousTitre.TabIndex = 1
        lblSousTitre.Text = "Visites des trois dernières années"
        '
        'lblAfficher
        '
        lblAfficher.AutoSize = True
        lblAfficher.Location = New Point(24, 88)
        lblAfficher.Name = "lblAfficher"
        lblAfficher.Size = New Size(60, 19)
        lblAfficher.TabIndex = 1
        lblAfficher.Text = "Afficher"
        '
        'cboEtat
        '
        cboEtat.DropDownStyle = ComboBoxStyle.DropDownList
        cboEtat.Location = New Point(92, 85)
        cboEtat.Name = "cboEtat"
        cboEtat.Size = New Size(230, 25)
        cboEtat.TabIndex = 2
        '
        'lblRecherche
        '
        lblRecherche.AutoSize = True
        lblRecherche.Location = New Point(346, 88)
        lblRecherche.Name = "lblRecherche"
        lblRecherche.Size = New Size(70, 19)
        lblRecherche.TabIndex = 3
        lblRecherche.Text = "Praticien"
        '
        'txtRecherche
        '
        txtRecherche.Location = New Point(420, 85)
        txtRecherche.Name = "txtRecherche"
        txtRecherche.PlaceholderText = "Nom ou ville…"
        txtRecherche.Size = New Size(260, 25)
        txtRecherche.TabIndex = 4
        '
        'lblNombre
        '
        lblNombre.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblNombre.Location = New Point(760, 88)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(296, 19)
        lblNombre.TabIndex = 5
        lblNombre.TextAlign = ContentAlignment.MiddleRight
        '
        'dgvRapports
        '
        dgvRapports.AllowUserToAddRows = False
        dgvRapports.AllowUserToDeleteRows = False
        dgvRapports.AllowUserToResizeRows = False
        dgvRapports.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvRapports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvRapports.Location = New Point(24, 124)
        dgvRapports.MultiSelect = False
        dgvRapports.Name = "dgvRapports"
        dgvRapports.ReadOnly = True
        dgvRapports.RowHeadersVisible = False
        dgvRapports.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRapports.Size = New Size(1032, 440)
        dgvRapports.TabIndex = 6
        '
        'pnlActions
        '
        pnlActions.Controls.Add(lblStatut)
        pnlActions.Controls.Add(btnFermer)
        pnlActions.Controls.Add(btnSupprimer)
        pnlActions.Controls.Add(btnOuvrir)
        pnlActions.Controls.Add(btnNouveau)
        pnlActions.Dock = DockStyle.Bottom
        pnlActions.Location = New Point(0, 580)
        pnlActions.Name = "pnlActions"
        pnlActions.Size = New Size(1080, 64)
        pnlActions.TabIndex = 7
        '
        'btnNouveau
        '
        btnNouveau.Location = New Point(24, 13)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(210, 38)
        btnNouveau.TabIndex = 0
        btnNouveau.Text = "Nouveau compte-rendu"
        '
        'btnOuvrir
        '
        btnOuvrir.Location = New Point(246, 13)
        btnOuvrir.Name = "btnOuvrir"
        btnOuvrir.Size = New Size(120, 38)
        btnOuvrir.TabIndex = 1
        btnOuvrir.Text = "Ouvrir"
        '
        'btnSupprimer
        '
        btnSupprimer.Location = New Point(378, 13)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(190, 38)
        btnSupprimer.TabIndex = 2
        btnSupprimer.Text = "Supprimer le brouillon"
        '
        'lblStatut
        '
        lblStatut.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblStatut.Location = New Point(584, 22)
        lblStatut.Name = "lblStatut"
        lblStatut.Size = New Size(330, 20)
        lblStatut.TabIndex = 3
        lblStatut.TextAlign = ContentAlignment.MiddleRight
        '
        'btnFermer
        '
        btnFermer.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnFermer.DialogResult = DialogResult.Cancel
        btnFermer.Location = New Point(936, 13)
        btnFermer.Name = "btnFermer"
        btnFermer.Size = New Size(120, 38)
        btnFermer.TabIndex = 4
        btnFermer.Text = "Fermer"
        '
        'FrmMesComptesRendus
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnFermer
        ClientSize = New Size(1080, 644)
        Controls.Add(dgvRapports)
        Controls.Add(lblNombre)
        Controls.Add(txtRecherche)
        Controls.Add(lblRecherche)
        Controls.Add(cboEtat)
        Controls.Add(lblAfficher)
        Controls.Add(pnlActions)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(820, 480)
        Name = "FrmMesComptesRendus"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Mes comptes-rendus"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        CType(dgvRapports, System.ComponentModel.ISupportInitialize).EndInit()
        pnlActions.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents lblAfficher As Label
    Friend WithEvents cboEtat As ComboBox
    Friend WithEvents lblRecherche As Label
    Friend WithEvents txtRecherche As TextBox
    Friend WithEvents lblNombre As Label
    Friend WithEvents dgvRapports As DataGridView
    Friend WithEvents pnlActions As Panel
    Friend WithEvents btnNouveau As Button
    Friend WithEvents btnOuvrir As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents lblStatut As Label
    Friend WithEvents btnFermer As Button

End Class
