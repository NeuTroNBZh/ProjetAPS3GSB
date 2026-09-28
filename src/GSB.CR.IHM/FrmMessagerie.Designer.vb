<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMessagerie
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
        pnlBarre = New Panel()
        lblNonLus = New Label()
        btnRepondre = New Button()
        btnNouveau = New Button()
        scPrincipal = New SplitContainer()
        tabBoites = New TabControl()
        tabRecus = New TabPage()
        dgvRecus = New DataGridView()
        tabEnvoyes = New TabPage()
        dgvEnvoyes = New DataGridView()
        pnlMessage = New PanneauFiche()
        pnlActions = New Panel()
        btnFermer = New Button()
        pnlEntete.SuspendLayout()
        pnlBarre.SuspendLayout()
        CType(scPrincipal, System.ComponentModel.ISupportInitialize).BeginInit()
        scPrincipal.Panel1.SuspendLayout()
        scPrincipal.Panel2.SuspendLayout()
        scPrincipal.SuspendLayout()
        tabBoites.SuspendLayout()
        tabRecus.SuspendLayout()
        CType(dgvRecus, System.ComponentModel.ISupportInitialize).BeginInit()
        tabEnvoyes.SuspendLayout()
        CType(dgvEnvoyes, System.ComponentModel.ISupportInitialize).BeginInit()
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
        lblTitre.Text = "Messagerie"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.TabIndex = 1
        lblSousTitre.Text = "Messages entre collaborateurs, à une personne ou à un groupe"
        '
        'pnlBarre
        '
        pnlBarre.Controls.Add(lblNonLus)
        pnlBarre.Controls.Add(btnRepondre)
        pnlBarre.Controls.Add(btnNouveau)
        pnlBarre.Dock = DockStyle.Top
        pnlBarre.Name = "pnlBarre"
        pnlBarre.Size = New Size(1180, 62)
        pnlBarre.TabIndex = 1
        '
        'btnNouveau
        '
        btnNouveau.Location = New Point(24, 12)
        btnNouveau.Name = "btnNouveau"
        btnNouveau.Size = New Size(200, 38)
        btnNouveau.TabIndex = 0
        btnNouveau.Text = "Nouveau message"
        '
        'btnRepondre
        '
        btnRepondre.Location = New Point(236, 12)
        btnRepondre.Name = "btnRepondre"
        btnRepondre.Size = New Size(140, 38)
        btnRepondre.TabIndex = 1
        btnRepondre.Text = "Répondre"
        '
        'lblNonLus
        '
        lblNonLus.Location = New Point(396, 21)
        lblNonLus.Name = "lblNonLus"
        lblNonLus.Size = New Size(400, 20)
        lblNonLus.TabIndex = 2
        '
        'scPrincipal
        '
        scPrincipal.Dock = DockStyle.Fill
        scPrincipal.Name = "scPrincipal"
        '
        'scPrincipal.Panel1
        '
        scPrincipal.Panel1.Controls.Add(tabBoites)
        scPrincipal.Panel1.Padding = New Padding(24, 0, 6, 12)
        '
        'scPrincipal.Panel2
        '
        scPrincipal.Panel2.Controls.Add(pnlMessage)
        scPrincipal.Panel2.Padding = New Padding(6, 30, 24, 12)
        scPrincipal.Size = New Size(1180, 540)
        scPrincipal.SplitterDistance = 560
        scPrincipal.TabIndex = 2
        '
        'tabBoites
        '
        tabBoites.Controls.Add(tabRecus)
        tabBoites.Controls.Add(tabEnvoyes)
        tabBoites.Dock = DockStyle.Fill
        tabBoites.Name = "tabBoites"
        tabBoites.Padding = New Point(16, 6)
        tabBoites.SelectedIndex = 0
        tabBoites.TabIndex = 0
        '
        'tabRecus
        '
        tabRecus.Controls.Add(dgvRecus)
        tabRecus.Name = "tabRecus"
        tabRecus.TabIndex = 0
        tabRecus.Text = "Reçus"
        '
        'dgvRecus
        '
        dgvRecus.AllowUserToAddRows = False
        dgvRecus.AllowUserToDeleteRows = False
        dgvRecus.AllowUserToResizeRows = False
        dgvRecus.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvRecus.Dock = DockStyle.Fill
        dgvRecus.MultiSelect = False
        dgvRecus.Name = "dgvRecus"
        dgvRecus.ReadOnly = True
        dgvRecus.RowHeadersVisible = False
        dgvRecus.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRecus.TabIndex = 0
        '
        'tabEnvoyes
        '
        tabEnvoyes.Controls.Add(dgvEnvoyes)
        tabEnvoyes.Name = "tabEnvoyes"
        tabEnvoyes.TabIndex = 1
        tabEnvoyes.Text = "Envoyés"
        '
        'dgvEnvoyes
        '
        dgvEnvoyes.AllowUserToAddRows = False
        dgvEnvoyes.AllowUserToDeleteRows = False
        dgvEnvoyes.AllowUserToResizeRows = False
        dgvEnvoyes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvEnvoyes.Dock = DockStyle.Fill
        dgvEnvoyes.MultiSelect = False
        dgvEnvoyes.Name = "dgvEnvoyes"
        dgvEnvoyes.ReadOnly = True
        dgvEnvoyes.RowHeadersVisible = False
        dgvEnvoyes.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvEnvoyes.TabIndex = 0
        '
        'pnlMessage
        '
        pnlMessage.BorderStyle = BorderStyle.FixedSingle
        pnlMessage.Dock = DockStyle.Fill
        pnlMessage.Name = "pnlMessage"
        pnlMessage.TabIndex = 0
        '
        'pnlActions
        '
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
        'FrmMessagerie
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnFermer
        ClientSize = New Size(1180, 730)
        Controls.Add(scPrincipal)
        Controls.Add(pnlActions)
        Controls.Add(pnlBarre)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(900, 520)
        Name = "FrmMessagerie"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Messagerie"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        pnlBarre.ResumeLayout(False)
        scPrincipal.Panel1.ResumeLayout(False)
        scPrincipal.Panel2.ResumeLayout(False)
        CType(scPrincipal, System.ComponentModel.ISupportInitialize).EndInit()
        scPrincipal.ResumeLayout(False)
        tabBoites.ResumeLayout(False)
        tabRecus.ResumeLayout(False)
        CType(dgvRecus, System.ComponentModel.ISupportInitialize).EndInit()
        tabEnvoyes.ResumeLayout(False)
        CType(dgvEnvoyes, System.ComponentModel.ISupportInitialize).EndInit()
        pnlActions.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents pnlBarre As Panel
    Friend WithEvents btnNouveau As Button
    Friend WithEvents btnRepondre As Button
    Friend WithEvents lblNonLus As Label
    Friend WithEvents scPrincipal As SplitContainer
    Friend WithEvents tabBoites As TabControl
    Friend WithEvents tabRecus As TabPage
    Friend WithEvents dgvRecus As DataGridView
    Friend WithEvents tabEnvoyes As TabPage
    Friend WithEvents dgvEnvoyes As DataGridView
    Friend WithEvents pnlMessage As PanneauFiche
    Friend WithEvents pnlActions As Panel
    Friend WithEvents btnFermer As Button

End Class
