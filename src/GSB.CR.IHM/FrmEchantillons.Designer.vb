<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmEchantillons
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
        chkEcarts = New CheckBox()
        cboMois = New ComboBox()
        lblMois = New Label()
        pnlSaisie = New Panel()
        lblMessage = New Label()
        btnSupprimer = New Button()
        btnEnregistrer = New Button()
        nudQuantite = New NumericUpDown()
        lblQuantite = New Label()
        cboProduit = New ComboBox()
        lblProduit = New Label()
        cboVisiteurSaisie = New ComboBox()
        lblVisiteurSaisie = New Label()
        lblAideSaisie = New Label()
        lblSectionSaisie = New Label()
        pnlGrille = New Panel()
        dgvStock = New DataGridView()
        pnlActions = New Panel()
        lblLegende = New Label()
        btnFermer = New Button()
        pnlEntete.SuspendLayout()
        pnlFiltres.SuspendLayout()
        pnlSaisie.SuspendLayout()
        CType(nudQuantite, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvStock, System.ComponentModel.ISupportInitialize).BeginInit()
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
        lblTitre.Text = "Échantillons"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.TabIndex = 1
        '
        'pnlFiltres
        '
        pnlFiltres.Controls.Add(lblNombre)
        pnlFiltres.Controls.Add(chkEcarts)
        pnlFiltres.Controls.Add(cboMois)
        pnlFiltres.Controls.Add(lblMois)
        pnlFiltres.Dock = DockStyle.Top
        pnlFiltres.Name = "pnlFiltres"
        pnlFiltres.Size = New Size(1180, 56)
        pnlFiltres.TabIndex = 1
        '
        'lblMois
        '
        lblMois.AutoSize = True
        lblMois.Location = New Point(24, 19)
        lblMois.Name = "lblMois"
        lblMois.TabIndex = 0
        lblMois.Text = "Mois"
        '
        'cboMois
        '
        cboMois.DropDownStyle = ComboBoxStyle.DropDownList
        cboMois.Location = New Point(74, 16)
        cboMois.Name = "cboMois"
        cboMois.Size = New Size(200, 25)
        cboMois.TabIndex = 1
        '
        'chkEcarts
        '
        chkEcarts.AutoSize = True
        chkEcarts.Location = New Point(300, 18)
        chkEcarts.Name = "chkEcarts"
        chkEcarts.TabIndex = 2
        chkEcarts.Text = "Seulement les dépassements"
        '
        'lblNombre
        '
        lblNombre.Location = New Point(540, 19)
        lblNombre.Name = "lblNombre"
        lblNombre.Size = New Size(260, 19)
        lblNombre.TabIndex = 3
        '
        'pnlSaisie
        '
        pnlSaisie.Controls.Add(lblMessage)
        pnlSaisie.Controls.Add(btnSupprimer)
        pnlSaisie.Controls.Add(btnEnregistrer)
        pnlSaisie.Controls.Add(nudQuantite)
        pnlSaisie.Controls.Add(lblQuantite)
        pnlSaisie.Controls.Add(cboProduit)
        pnlSaisie.Controls.Add(lblProduit)
        pnlSaisie.Controls.Add(cboVisiteurSaisie)
        pnlSaisie.Controls.Add(lblVisiteurSaisie)
        pnlSaisie.Controls.Add(lblAideSaisie)
        pnlSaisie.Controls.Add(lblSectionSaisie)
        pnlSaisie.Dock = DockStyle.Right
        pnlSaisie.Name = "pnlSaisie"
        pnlSaisie.Size = New Size(360, 520)
        pnlSaisie.TabIndex = 3
        '
        'lblSectionSaisie
        '
        lblSectionSaisie.AutoSize = True
        lblSectionSaisie.Location = New Point(16, 6)
        lblSectionSaisie.Name = "lblSectionSaisie"
        lblSectionSaisie.TabIndex = 0
        lblSectionSaisie.Text = "Attribuer des échantillons"
        '
        'lblAideSaisie
        '
        lblAideSaisie.Location = New Point(16, 34)
        lblAideSaisie.Name = "lblAideSaisie"
        lblAideSaisie.Size = New Size(320, 58)
        lblAideSaisie.TabIndex = 1
        lblAideSaisie.Text = "Dotation du mois choisi. Une nouvelle saisie pour le même visiteur et le même produit remplace la précédente."
        '
        'lblVisiteurSaisie
        '
        lblVisiteurSaisie.AutoSize = True
        lblVisiteurSaisie.Location = New Point(16, 100)
        lblVisiteurSaisie.Name = "lblVisiteurSaisie"
        lblVisiteurSaisie.TabIndex = 2
        lblVisiteurSaisie.Text = "Visiteur"
        '
        'cboVisiteurSaisie
        '
        cboVisiteurSaisie.DropDownStyle = ComboBoxStyle.DropDownList
        cboVisiteurSaisie.Location = New Point(16, 122)
        cboVisiteurSaisie.Name = "cboVisiteurSaisie"
        cboVisiteurSaisie.Size = New Size(320, 25)
        cboVisiteurSaisie.TabIndex = 3
        '
        'lblProduit
        '
        lblProduit.AutoSize = True
        lblProduit.Location = New Point(16, 160)
        lblProduit.Name = "lblProduit"
        lblProduit.TabIndex = 4
        lblProduit.Text = "Produit"
        '
        'cboProduit
        '
        cboProduit.DropDownStyle = ComboBoxStyle.DropDownList
        cboProduit.Location = New Point(16, 182)
        cboProduit.Name = "cboProduit"
        cboProduit.Size = New Size(320, 25)
        cboProduit.TabIndex = 5
        '
        'lblQuantite
        '
        lblQuantite.AutoSize = True
        lblQuantite.Location = New Point(16, 220)
        lblQuantite.Name = "lblQuantite"
        lblQuantite.TabIndex = 6
        lblQuantite.Text = "Quantité attribuée"
        '
        'nudQuantite
        '
        nudQuantite.Location = New Point(16, 242)
        nudQuantite.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        nudQuantite.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        nudQuantite.Name = "nudQuantite"
        nudQuantite.Size = New Size(120, 25)
        nudQuantite.TabIndex = 7
        nudQuantite.TextAlign = HorizontalAlignment.Right
        nudQuantite.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'btnEnregistrer
        '
        btnEnregistrer.Location = New Point(16, 286)
        btnEnregistrer.Name = "btnEnregistrer"
        btnEnregistrer.Size = New Size(320, 38)
        btnEnregistrer.TabIndex = 8
        btnEnregistrer.Text = "Enregistrer la dotation"
        '
        'btnSupprimer
        '
        btnSupprimer.Location = New Point(16, 332)
        btnSupprimer.Name = "btnSupprimer"
        btnSupprimer.Size = New Size(320, 34)
        btnSupprimer.TabIndex = 9
        btnSupprimer.Text = "Supprimer la dotation"
        '
        'lblMessage
        '
        lblMessage.Location = New Point(16, 378)
        lblMessage.Name = "lblMessage"
        lblMessage.Size = New Size(320, 120)
        lblMessage.TabIndex = 10
        '
        'pnlGrille
        '
        pnlGrille.Controls.Add(dgvStock)
        pnlGrille.Dock = DockStyle.Fill
        pnlGrille.Name = "pnlGrille"
        pnlGrille.Padding = New Padding(24, 0, 16, 12)
        pnlGrille.TabIndex = 2
        '
        'dgvStock
        '
        dgvStock.AllowUserToAddRows = False
        dgvStock.AllowUserToDeleteRows = False
        dgvStock.AllowUserToResizeRows = False
        dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvStock.Dock = DockStyle.Fill
        dgvStock.MultiSelect = False
        dgvStock.Name = "dgvStock"
        dgvStock.ReadOnly = True
        dgvStock.RowHeadersVisible = False
        dgvStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvStock.TabIndex = 2
        '
        'pnlActions
        '
        pnlActions.Controls.Add(lblLegende)
        pnlActions.Controls.Add(btnFermer)
        pnlActions.Dock = DockStyle.Bottom
        pnlActions.Name = "pnlActions"
        pnlActions.Size = New Size(1180, 60)
        pnlActions.TabIndex = 4
        '
        'lblLegende
        '
        lblLegende.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblLegende.Location = New Point(24, 20)
        lblLegende.Name = "lblLegende"
        lblLegende.Size = New Size(980, 20)
        lblLegende.TabIndex = 1
        lblLegende.Text = "Distribué = échantillons des comptes-rendus validés. Écart négatif : plus d'échantillons distribués qu'attribués (à justifier)."
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
        'FrmEchantillons
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnFermer
        ClientSize = New Size(1180, 720)
        Controls.Add(pnlGrille)
        Controls.Add(pnlSaisie)
        Controls.Add(pnlActions)
        Controls.Add(pnlFiltres)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(960, 560)
        Name = "FrmEchantillons"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Échantillons"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        pnlFiltres.ResumeLayout(False)
        pnlFiltres.PerformLayout()
        pnlSaisie.ResumeLayout(False)
        pnlSaisie.PerformLayout()
        CType(nudQuantite, System.ComponentModel.ISupportInitialize).EndInit()
        CType(dgvStock, System.ComponentModel.ISupportInitialize).EndInit()
        pnlActions.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents pnlFiltres As Panel
    Friend WithEvents lblMois As Label
    Friend WithEvents cboMois As ComboBox
    Friend WithEvents chkEcarts As CheckBox
    Friend WithEvents lblNombre As Label
    Friend WithEvents pnlSaisie As Panel
    Friend WithEvents lblSectionSaisie As Label
    Friend WithEvents lblAideSaisie As Label
    Friend WithEvents lblVisiteurSaisie As Label
    Friend WithEvents cboVisiteurSaisie As ComboBox
    Friend WithEvents lblProduit As Label
    Friend WithEvents cboProduit As ComboBox
    Friend WithEvents lblQuantite As Label
    Friend WithEvents nudQuantite As NumericUpDown
    Friend WithEvents btnEnregistrer As Button
    Friend WithEvents btnSupprimer As Button
    Friend WithEvents lblMessage As Label
    Friend WithEvents pnlGrille As Panel
    Friend WithEvents dgvStock As DataGridView
    Friend WithEvents pnlActions As Panel
    Friend WithEvents lblLegende As Label
    Friend WithEvents btnFermer As Button

End Class
