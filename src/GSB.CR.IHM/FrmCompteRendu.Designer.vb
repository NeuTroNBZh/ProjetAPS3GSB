<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCompteRendu
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
        pnlActions = New Panel()
        btnAnnuler = New Button()
        btnBrouillon = New Button()
        btnValider = New Button()
        pnlContenu = New Panel()
        lblSectionVisite = New Label()
        lblPraticien = New Label()
        cboPraticien = New ComboBox()
        chkRemplacant = New CheckBox()
        cboRemplacant = New ComboBox()
        btnNouveauRemplacant = New Button()
        lblDateVisite = New Label()
        dtpDateVisite = New DateTimePicker()
        lblMotif = New Label()
        cboMotif = New ComboBox()
        lblPrecision = New Label()
        txtPrecisionMotif = New TextBox()
        lblSectionProduits = New Label()
        lblProduit1 = New Label()
        cboProduit1 = New ComboBox()
        lblProduit2 = New Label()
        cboProduit2 = New ComboBox()
        lblSectionEchantillons = New Label()
        cboEchantillon = New ComboBox()
        nudQuantite = New NumericUpDown()
        btnAjouterEchantillon = New Button()
        dgvEchantillons = New DataGridView()
        colProduit = New DataGridViewTextBoxColumn()
        colQuantite = New DataGridViewTextBoxColumn()
        btnRetirerEchantillon = New Button()
        lblTotalEchantillons = New Label()
        lblSectionBilan = New Label()
        lblBilan = New Label()
        txtBilan = New TextBox()
        lblCompteur = New Label()
        lblConfiance = New Label()
        cboConfiance = New ComboBox()
        chkProchaineVisite = New CheckBox()
        dtpProchaineVisite = New DateTimePicker()
        lblObligatoire = New Label()
        lblErreurs = New Label()
        pnlEntete.SuspendLayout()
        pnlActions.SuspendLayout()
        pnlContenu.SuspendLayout()
        CType(nudQuantite, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvEchantillons, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        'pnlEntete
        '
        pnlEntete.Controls.Add(lblSousTitre)
        pnlEntete.Controls.Add(lblTitre)
        pnlEntete.Dock = DockStyle.Top
        pnlEntete.Location = New Point(0, 0)
        pnlEntete.Name = "pnlEntete"
        pnlEntete.Size = New Size(944, 68)
        pnlEntete.TabIndex = 0
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Font = New Font("Segoe UI Semibold", 16.0F)
        lblTitre.Location = New Point(22, 6)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(240, 30)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Nouveau compte-rendu"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.Size = New Size(100, 19)
        lblSousTitre.TabIndex = 1
        '
        'pnlActions
        '
        pnlActions.Controls.Add(btnAnnuler)
        pnlActions.Controls.Add(btnBrouillon)
        pnlActions.Controls.Add(btnValider)
        pnlActions.Dock = DockStyle.Bottom
        pnlActions.Location = New Point(0, 736)
        pnlActions.Name = "pnlActions"
        pnlActions.Size = New Size(944, 64)
        pnlActions.TabIndex = 2
        '
        'btnAnnuler
        '
        btnAnnuler.Location = New Point(24, 13)
        btnAnnuler.Name = "btnAnnuler"
        btnAnnuler.Size = New Size(140, 38)
        btnAnnuler.TabIndex = 0
        btnAnnuler.Text = "Fermer"
        '
        'btnBrouillon
        '
        btnBrouillon.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnBrouillon.Location = New Point(522, 13)
        btnBrouillon.Name = "btnBrouillon"
        btnBrouillon.Size = New Size(190, 38)
        btnBrouillon.TabIndex = 1
        btnBrouillon.Text = "Enregistrer le brouillon"
        '
        'btnValider
        '
        btnValider.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnValider.Location = New Point(724, 13)
        btnValider.Name = "btnValider"
        btnValider.Size = New Size(196, 38)
        btnValider.TabIndex = 2
        btnValider.Text = "Valider le compte-rendu"
        '
        'pnlContenu
        '
        pnlContenu.AutoScroll = True
        pnlContenu.Controls.Add(lblSectionVisite)
        pnlContenu.Controls.Add(lblPraticien)
        pnlContenu.Controls.Add(cboPraticien)
        pnlContenu.Controls.Add(chkRemplacant)
        pnlContenu.Controls.Add(cboRemplacant)
        pnlContenu.Controls.Add(btnNouveauRemplacant)
        pnlContenu.Controls.Add(lblDateVisite)
        pnlContenu.Controls.Add(dtpDateVisite)
        pnlContenu.Controls.Add(lblMotif)
        pnlContenu.Controls.Add(cboMotif)
        pnlContenu.Controls.Add(lblPrecision)
        pnlContenu.Controls.Add(txtPrecisionMotif)
        pnlContenu.Controls.Add(lblSectionProduits)
        pnlContenu.Controls.Add(lblProduit1)
        pnlContenu.Controls.Add(cboProduit1)
        pnlContenu.Controls.Add(lblProduit2)
        pnlContenu.Controls.Add(cboProduit2)
        pnlContenu.Controls.Add(lblSectionEchantillons)
        pnlContenu.Controls.Add(cboEchantillon)
        pnlContenu.Controls.Add(nudQuantite)
        pnlContenu.Controls.Add(btnAjouterEchantillon)
        pnlContenu.Controls.Add(dgvEchantillons)
        pnlContenu.Controls.Add(btnRetirerEchantillon)
        pnlContenu.Controls.Add(lblTotalEchantillons)
        pnlContenu.Controls.Add(lblSectionBilan)
        pnlContenu.Controls.Add(lblBilan)
        pnlContenu.Controls.Add(txtBilan)
        pnlContenu.Controls.Add(lblCompteur)
        pnlContenu.Controls.Add(lblConfiance)
        pnlContenu.Controls.Add(cboConfiance)
        pnlContenu.Controls.Add(chkProchaineVisite)
        pnlContenu.Controls.Add(dtpProchaineVisite)
        pnlContenu.Controls.Add(lblObligatoire)
        pnlContenu.Controls.Add(lblErreurs)
        pnlContenu.Dock = DockStyle.Fill
        pnlContenu.Location = New Point(0, 68)
        pnlContenu.Name = "pnlContenu"
        pnlContenu.Size = New Size(944, 668)
        pnlContenu.TabIndex = 1
        '
        'lblSectionVisite
        '
        lblSectionVisite.AutoSize = True
        lblSectionVisite.Location = New Point(24, 14)
        lblSectionVisite.Name = "lblSectionVisite"
        lblSectionVisite.Size = New Size(50, 21)
        lblSectionVisite.TabIndex = 0
        lblSectionVisite.Text = "Visite"
        '
        'lblPraticien
        '
        lblPraticien.AutoSize = True
        lblPraticien.Location = New Point(24, 44)
        lblPraticien.Name = "lblPraticien"
        lblPraticien.Size = New Size(260, 19)
        lblPraticien.TabIndex = 1
        lblPraticien.Text = "Praticien visité (titulaire du cabinet) *"
        '
        'cboPraticien
        '
        cboPraticien.DropDownStyle = ComboBoxStyle.DropDownList
        cboPraticien.Location = New Point(24, 66)
        cboPraticien.Name = "cboPraticien"
        cboPraticien.Size = New Size(430, 25)
        cboPraticien.TabIndex = 2
        '
        'chkRemplacant
        '
        chkRemplacant.AutoSize = True
        chkRemplacant.Location = New Point(24, 100)
        chkRemplacant.Name = "chkRemplacant"
        chkRemplacant.Size = New Size(300, 23)
        chkRemplacant.TabIndex = 3
        chkRemplacant.Text = "La personne rencontrée est un remplaçant"
        '
        'cboRemplacant
        '
        cboRemplacant.DropDownStyle = ComboBoxStyle.DropDownList
        cboRemplacant.Enabled = False
        cboRemplacant.Location = New Point(44, 128)
        cboRemplacant.Name = "cboRemplacant"
        cboRemplacant.Size = New Size(300, 25)
        cboRemplacant.TabIndex = 4
        '
        'btnNouveauRemplacant
        '
        btnNouveauRemplacant.Enabled = False
        btnNouveauRemplacant.Location = New Point(352, 127)
        btnNouveauRemplacant.Name = "btnNouveauRemplacant"
        btnNouveauRemplacant.Size = New Size(102, 28)
        btnNouveauRemplacant.TabIndex = 5
        btnNouveauRemplacant.Text = "Nouveau…"
        '
        'lblDateVisite
        '
        lblDateVisite.AutoSize = True
        lblDateVisite.Location = New Point(24, 168)
        lblDateVisite.Name = "lblDateVisite"
        lblDateVisite.Size = New Size(130, 19)
        lblDateVisite.TabIndex = 6
        lblDateVisite.Text = "Date de la visite *"
        '
        'dtpDateVisite
        '
        dtpDateVisite.Location = New Point(24, 190)
        dtpDateVisite.Name = "dtpDateVisite"
        dtpDateVisite.Size = New Size(260, 25)
        dtpDateVisite.TabIndex = 7
        '
        'lblMotif
        '
        lblMotif.AutoSize = True
        lblMotif.Location = New Point(24, 228)
        lblMotif.Name = "lblMotif"
        lblMotif.Size = New Size(55, 19)
        lblMotif.TabIndex = 8
        lblMotif.Text = "Motif *"
        '
        'cboMotif
        '
        cboMotif.DropDownStyle = ComboBoxStyle.DropDownList
        cboMotif.Location = New Point(24, 250)
        cboMotif.Name = "cboMotif"
        cboMotif.Size = New Size(430, 25)
        cboMotif.TabIndex = 9
        '
        'lblPrecision
        '
        lblPrecision.AutoSize = True
        lblPrecision.Location = New Point(24, 286)
        lblPrecision.Name = "lblPrecision"
        lblPrecision.Size = New Size(200, 19)
        lblPrecision.TabIndex = 10
        lblPrecision.Text = "Précision (motif « Autre »)"
        '
        'txtPrecisionMotif
        '
        txtPrecisionMotif.Enabled = False
        txtPrecisionMotif.Location = New Point(24, 308)
        txtPrecisionMotif.MaxLength = 200
        txtPrecisionMotif.Name = "txtPrecisionMotif"
        txtPrecisionMotif.Size = New Size(430, 25)
        txtPrecisionMotif.TabIndex = 11
        '
        'lblSectionProduits
        '
        lblSectionProduits.AutoSize = True
        lblSectionProduits.Location = New Point(24, 350)
        lblSectionProduits.Name = "lblSectionProduits"
        lblSectionProduits.Size = New Size(250, 21)
        lblSectionProduits.TabIndex = 12
        lblSectionProduits.Text = "Produits présentés (2 maximum)"
        '
        'lblProduit1
        '
        lblProduit1.AutoSize = True
        lblProduit1.Location = New Point(24, 384)
        lblProduit1.Name = "lblProduit1"
        lblProduit1.Size = New Size(70, 19)
        lblProduit1.TabIndex = 13
        lblProduit1.Text = "Produit 1"
        '
        'cboProduit1
        '
        cboProduit1.DropDownStyle = ComboBoxStyle.DropDownList
        cboProduit1.Location = New Point(110, 381)
        cboProduit1.Name = "cboProduit1"
        cboProduit1.Size = New Size(344, 25)
        cboProduit1.TabIndex = 14
        '
        'lblProduit2
        '
        lblProduit2.AutoSize = True
        lblProduit2.Location = New Point(24, 420)
        lblProduit2.Name = "lblProduit2"
        lblProduit2.Size = New Size(70, 19)
        lblProduit2.TabIndex = 15
        lblProduit2.Text = "Produit 2"
        '
        'cboProduit2
        '
        cboProduit2.DropDownStyle = ComboBoxStyle.DropDownList
        cboProduit2.Location = New Point(110, 417)
        cboProduit2.Name = "cboProduit2"
        cboProduit2.Size = New Size(344, 25)
        cboProduit2.TabIndex = 16
        '
        'lblSectionEchantillons
        '
        lblSectionEchantillons.AutoSize = True
        lblSectionEchantillons.Location = New Point(24, 460)
        lblSectionEchantillons.Name = "lblSectionEchantillons"
        lblSectionEchantillons.Size = New Size(170, 21)
        lblSectionEchantillons.TabIndex = 17
        lblSectionEchantillons.Text = "Échantillons offerts"
        '
        'cboEchantillon
        '
        cboEchantillon.DropDownStyle = ComboBoxStyle.DropDownList
        cboEchantillon.Location = New Point(24, 492)
        cboEchantillon.Name = "cboEchantillon"
        cboEchantillon.Size = New Size(250, 25)
        cboEchantillon.TabIndex = 18
        '
        'nudQuantite
        '
        nudQuantite.Location = New Point(282, 492)
        nudQuantite.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        nudQuantite.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        nudQuantite.Name = "nudQuantite"
        nudQuantite.Size = New Size(70, 25)
        nudQuantite.TabIndex = 19
        nudQuantite.TextAlign = HorizontalAlignment.Right
        nudQuantite.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'btnAjouterEchantillon
        '
        btnAjouterEchantillon.Location = New Point(360, 491)
        btnAjouterEchantillon.Name = "btnAjouterEchantillon"
        btnAjouterEchantillon.Size = New Size(94, 28)
        btnAjouterEchantillon.TabIndex = 20
        btnAjouterEchantillon.Text = "Ajouter"
        '
        'dgvEchantillons
        '
        dgvEchantillons.AllowUserToAddRows = False
        dgvEchantillons.AllowUserToDeleteRows = False
        dgvEchantillons.AllowUserToResizeRows = False
        dgvEchantillons.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvEchantillons.BackgroundColor = Color.White
        dgvEchantillons.Columns.AddRange(New DataGridViewColumn() {colProduit, colQuantite})
        dgvEchantillons.Location = New Point(24, 526)
        dgvEchantillons.MultiSelect = False
        dgvEchantillons.Name = "dgvEchantillons"
        dgvEchantillons.ReadOnly = True
        dgvEchantillons.RowHeadersVisible = False
        dgvEchantillons.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvEchantillons.Size = New Size(430, 110)
        dgvEchantillons.TabIndex = 21
        '
        'colProduit
        '
        colProduit.FillWeight = 75.0F
        colProduit.HeaderText = "Produit"
        colProduit.Name = "colProduit"
        colProduit.ReadOnly = True
        '
        'colQuantite
        '
        colQuantite.FillWeight = 25.0F
        colQuantite.HeaderText = "Quantité"
        colQuantite.Name = "colQuantite"
        colQuantite.ReadOnly = True
        '
        'btnRetirerEchantillon
        '
        btnRetirerEchantillon.Location = New Point(24, 642)
        btnRetirerEchantillon.Name = "btnRetirerEchantillon"
        btnRetirerEchantillon.Size = New Size(120, 28)
        btnRetirerEchantillon.TabIndex = 22
        btnRetirerEchantillon.Text = "Retirer"
        '
        'lblTotalEchantillons
        '
        lblTotalEchantillons.Location = New Point(154, 646)
        lblTotalEchantillons.Name = "lblTotalEchantillons"
        lblTotalEchantillons.Size = New Size(300, 20)
        lblTotalEchantillons.TabIndex = 23
        lblTotalEchantillons.TextAlign = ContentAlignment.MiddleRight
        '
        'lblSectionBilan
        '
        lblSectionBilan.AutoSize = True
        lblSectionBilan.Location = New Point(490, 14)
        lblSectionBilan.Name = "lblSectionBilan"
        lblSectionBilan.Size = New Size(45, 21)
        lblSectionBilan.TabIndex = 24
        lblSectionBilan.Text = "Bilan"
        '
        'lblBilan
        '
        lblBilan.AutoSize = True
        lblBilan.Location = New Point(490, 44)
        lblBilan.Name = "lblBilan"
        lblBilan.Size = New Size(140, 19)
        lblBilan.TabIndex = 25
        lblBilan.Text = "Bilan de la visite *"
        '
        'txtBilan
        '
        txtBilan.AcceptsReturn = True
        txtBilan.Location = New Point(490, 66)
        txtBilan.Multiline = True
        txtBilan.Name = "txtBilan"
        txtBilan.ScrollBars = ScrollBars.Vertical
        txtBilan.Size = New Size(430, 210)
        txtBilan.TabIndex = 26
        '
        'lblCompteur
        '
        lblCompteur.Location = New Point(490, 280)
        lblCompteur.Name = "lblCompteur"
        lblCompteur.Size = New Size(430, 18)
        lblCompteur.TabIndex = 27
        lblCompteur.TextAlign = ContentAlignment.MiddleRight
        '
        'lblConfiance
        '
        lblConfiance.AutoSize = True
        lblConfiance.Location = New Point(490, 306)
        lblConfiance.Name = "lblConfiance"
        lblConfiance.Size = New Size(330, 19)
        lblConfiance.TabIndex = 28
        lblConfiance.Text = "Confiance du praticien dans les produits GSB *"
        '
        'cboConfiance
        '
        cboConfiance.DropDownStyle = ComboBoxStyle.DropDownList
        cboConfiance.Location = New Point(490, 328)
        cboConfiance.Name = "cboConfiance"
        cboConfiance.Size = New Size(430, 25)
        cboConfiance.TabIndex = 29
        '
        'chkProchaineVisite
        '
        chkProchaineVisite.AutoSize = True
        chkProchaineVisite.Location = New Point(490, 372)
        chkProchaineVisite.Name = "chkProchaineVisite"
        chkProchaineVisite.Size = New Size(200, 23)
        chkProchaineVisite.TabIndex = 30
        chkProchaineVisite.Text = "Prochaine visite prévue le"
        '
        'dtpProchaineVisite
        '
        dtpProchaineVisite.Enabled = False
        dtpProchaineVisite.Location = New Point(700, 370)
        dtpProchaineVisite.Name = "dtpProchaineVisite"
        dtpProchaineVisite.Size = New Size(220, 25)
        dtpProchaineVisite.TabIndex = 31
        '
        'lblObligatoire
        '
        lblObligatoire.Location = New Point(490, 408)
        lblObligatoire.Name = "lblObligatoire"
        lblObligatoire.Size = New Size(430, 38)
        lblObligatoire.TabIndex = 32
        lblObligatoire.Text = "* Obligatoire pour valider. Un brouillon n'exige que le praticien et la date de visite."
        '
        'lblErreurs
        '
        lblErreurs.Location = New Point(490, 452)
        lblErreurs.Name = "lblErreurs"
        lblErreurs.Size = New Size(430, 218)
        lblErreurs.TabIndex = 33
        '
        'FrmCompteRendu
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(944, 800)
        Controls.Add(pnlContenu)
        Controls.Add(pnlActions)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(760, 520)
        Name = "FrmCompteRendu"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Compte-rendu de visite"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        pnlActions.ResumeLayout(False)
        pnlContenu.ResumeLayout(False)
        pnlContenu.PerformLayout()
        CType(nudQuantite, System.ComponentModel.ISupportInitialize).EndInit()
        CType(dgvEchantillons, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents pnlActions As Panel
    Friend WithEvents btnAnnuler As Button
    Friend WithEvents btnBrouillon As Button
    Friend WithEvents btnValider As Button
    Friend WithEvents pnlContenu As Panel
    Friend WithEvents lblSectionVisite As Label
    Friend WithEvents lblPraticien As Label
    Friend WithEvents cboPraticien As ComboBox
    Friend WithEvents chkRemplacant As CheckBox
    Friend WithEvents cboRemplacant As ComboBox
    Friend WithEvents btnNouveauRemplacant As Button
    Friend WithEvents lblDateVisite As Label
    Friend WithEvents dtpDateVisite As DateTimePicker
    Friend WithEvents lblMotif As Label
    Friend WithEvents cboMotif As ComboBox
    Friend WithEvents lblPrecision As Label
    Friend WithEvents txtPrecisionMotif As TextBox
    Friend WithEvents lblSectionProduits As Label
    Friend WithEvents lblProduit1 As Label
    Friend WithEvents cboProduit1 As ComboBox
    Friend WithEvents lblProduit2 As Label
    Friend WithEvents cboProduit2 As ComboBox
    Friend WithEvents lblSectionEchantillons As Label
    Friend WithEvents cboEchantillon As ComboBox
    Friend WithEvents nudQuantite As NumericUpDown
    Friend WithEvents btnAjouterEchantillon As Button
    Friend WithEvents dgvEchantillons As DataGridView
    Friend WithEvents colProduit As DataGridViewTextBoxColumn
    Friend WithEvents colQuantite As DataGridViewTextBoxColumn
    Friend WithEvents btnRetirerEchantillon As Button
    Friend WithEvents lblTotalEchantillons As Label
    Friend WithEvents lblSectionBilan As Label
    Friend WithEvents lblBilan As Label
    Friend WithEvents txtBilan As TextBox
    Friend WithEvents lblCompteur As Label
    Friend WithEvents lblConfiance As Label
    Friend WithEvents cboConfiance As ComboBox
    Friend WithEvents chkProchaineVisite As CheckBox
    Friend WithEvents dtpProchaineVisite As DateTimePicker
    Friend WithEvents lblObligatoire As Label
    Friend WithEvents lblErreurs As Label

End Class
