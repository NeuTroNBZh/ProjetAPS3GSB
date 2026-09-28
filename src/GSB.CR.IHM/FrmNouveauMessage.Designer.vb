<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmNouveauMessage
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
        lblTitre = New Label()
        lblDestinataires = New Label()
        txtFiltre = New TextBox()
        clbDestinataires = New CheckedListBox()
        lblGroupe = New Label()
        cboGroupe = New ComboBox()
        btnAjouterGroupe = New Button()
        btnToutDecocher = New Button()
        lblNbDestinataires = New Label()
        lblObjet = New Label()
        txtObjet = New TextBox()
        lblContenu = New Label()
        txtContenu = New TextBox()
        lblErreurs = New Label()
        btnAnnuler = New Button()
        btnEnvoyer = New Button()
        SuspendLayout()
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(24, 16)
        lblTitre.Name = "lblTitre"
        lblTitre.TabIndex = 0
        lblTitre.Text = "Nouveau message"
        '
        'lblDestinataires
        '
        lblDestinataires.AutoSize = True
        lblDestinataires.Location = New Point(24, 64)
        lblDestinataires.Name = "lblDestinataires"
        lblDestinataires.TabIndex = 1
        lblDestinataires.Text = "Destinataires"
        '
        'txtFiltre
        '
        txtFiltre.Location = New Point(24, 88)
        txtFiltre.Name = "txtFiltre"
        txtFiltre.PlaceholderText = "Rechercher un collaborateur…"
        txtFiltre.Size = New Size(360, 25)
        txtFiltre.TabIndex = 2
        '
        'clbDestinataires
        '
        clbDestinataires.CheckOnClick = True
        clbDestinataires.IntegralHeight = False
        clbDestinataires.Location = New Point(24, 120)
        clbDestinataires.Name = "clbDestinataires"
        clbDestinataires.Size = New Size(360, 300)
        clbDestinataires.TabIndex = 3
        '
        'lblGroupe
        '
        lblGroupe.AutoSize = True
        lblGroupe.Location = New Point(24, 432)
        lblGroupe.Name = "lblGroupe"
        lblGroupe.TabIndex = 4
        lblGroupe.Text = "Ajouter tout un groupe"
        '
        'cboGroupe
        '
        cboGroupe.DropDownStyle = ComboBoxStyle.DropDownList
        cboGroupe.Location = New Point(24, 456)
        cboGroupe.Name = "cboGroupe"
        cboGroupe.Size = New Size(250, 25)
        cboGroupe.TabIndex = 5
        '
        'btnAjouterGroupe
        '
        btnAjouterGroupe.Location = New Point(282, 455)
        btnAjouterGroupe.Name = "btnAjouterGroupe"
        btnAjouterGroupe.Size = New Size(102, 28)
        btnAjouterGroupe.TabIndex = 6
        btnAjouterGroupe.Text = "Ajouter"
        '
        'btnToutDecocher
        '
        btnToutDecocher.Location = New Point(24, 494)
        btnToutDecocher.Name = "btnToutDecocher"
        btnToutDecocher.Size = New Size(150, 28)
        btnToutDecocher.TabIndex = 7
        btnToutDecocher.Text = "Tout décocher"
        '
        'lblNbDestinataires
        '
        lblNbDestinataires.Location = New Point(184, 498)
        lblNbDestinataires.Name = "lblNbDestinataires"
        lblNbDestinataires.Size = New Size(200, 20)
        lblNbDestinataires.TabIndex = 8
        lblNbDestinataires.TextAlign = ContentAlignment.MiddleRight
        '
        'lblObjet
        '
        lblObjet.AutoSize = True
        lblObjet.Location = New Point(408, 64)
        lblObjet.Name = "lblObjet"
        lblObjet.TabIndex = 9
        lblObjet.Text = "Objet"
        '
        'txtObjet
        '
        txtObjet.Location = New Point(408, 88)
        txtObjet.MaxLength = 100
        txtObjet.Name = "txtObjet"
        txtObjet.Size = New Size(500, 25)
        txtObjet.TabIndex = 10
        '
        'lblContenu
        '
        lblContenu.AutoSize = True
        lblContenu.Location = New Point(408, 124)
        lblContenu.Name = "lblContenu"
        lblContenu.TabIndex = 11
        lblContenu.Text = "Message"
        '
        'txtContenu
        '
        txtContenu.AcceptsReturn = True
        txtContenu.Location = New Point(408, 148)
        txtContenu.Multiline = True
        txtContenu.Name = "txtContenu"
        txtContenu.ScrollBars = ScrollBars.Vertical
        txtContenu.Size = New Size(500, 330)
        txtContenu.TabIndex = 12
        '
        'lblErreurs
        '
        lblErreurs.Location = New Point(408, 486)
        lblErreurs.Name = "lblErreurs"
        lblErreurs.Size = New Size(500, 44)
        lblErreurs.TabIndex = 13
        '
        'btnAnnuler
        '
        btnAnnuler.DialogResult = DialogResult.Cancel
        btnAnnuler.Location = New Point(24, 544)
        btnAnnuler.Name = "btnAnnuler"
        btnAnnuler.Size = New Size(160, 38)
        btnAnnuler.TabIndex = 15
        btnAnnuler.Text = "Annuler"
        '
        'btnEnvoyer
        '
        btnEnvoyer.Location = New Point(748, 544)
        btnEnvoyer.Name = "btnEnvoyer"
        btnEnvoyer.Size = New Size(160, 38)
        btnEnvoyer.TabIndex = 14
        btnEnvoyer.Text = "Envoyer"
        '
        'FrmNouveauMessage
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnAnnuler
        ClientSize = New Size(932, 604)
        Controls.Add(btnEnvoyer)
        Controls.Add(btnAnnuler)
        Controls.Add(lblErreurs)
        Controls.Add(txtContenu)
        Controls.Add(lblContenu)
        Controls.Add(txtObjet)
        Controls.Add(lblObjet)
        Controls.Add(lblNbDestinataires)
        Controls.Add(btnToutDecocher)
        Controls.Add(btnAjouterGroupe)
        Controls.Add(cboGroupe)
        Controls.Add(lblGroupe)
        Controls.Add(clbDestinataires)
        Controls.Add(txtFiltre)
        Controls.Add(lblDestinataires)
        Controls.Add(lblTitre)
        Font = New Font("Segoe UI", 10.0F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "FrmNouveauMessage"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Nouveau message"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitre As Label
    Friend WithEvents lblDestinataires As Label
    Friend WithEvents txtFiltre As TextBox
    Friend WithEvents clbDestinataires As CheckedListBox
    Friend WithEvents lblGroupe As Label
    Friend WithEvents cboGroupe As ComboBox
    Friend WithEvents btnAjouterGroupe As Button
    Friend WithEvents btnToutDecocher As Button
    Friend WithEvents lblNbDestinataires As Label
    Friend WithEvents lblObjet As Label
    Friend WithEvents txtObjet As TextBox
    Friend WithEvents lblContenu As Label
    Friend WithEvents txtContenu As TextBox
    Friend WithEvents lblErreurs As Label
    Friend WithEvents btnAnnuler As Button
    Friend WithEvents btnEnvoyer As Button

End Class
