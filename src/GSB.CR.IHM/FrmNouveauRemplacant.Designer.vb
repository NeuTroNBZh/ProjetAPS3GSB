<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmNouveauRemplacant
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
        lblInformation = New Label()
        lblNom = New Label()
        txtNom = New TextBox()
        lblPrenom = New Label()
        txtPrenom = New TextBox()
        lblType = New Label()
        cboType = New ComboBox()
        lblTelephone = New Label()
        txtTelephone = New TextBox()
        lblEmail = New Label()
        txtEmail = New TextBox()
        lblErreurs = New Label()
        btnAnnuler = New Button()
        btnValider = New Button()
        SuspendLayout()
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(28, 20)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(200, 32)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Nouveau remplaçant"
        '
        'lblInformation
        '
        lblInformation.Location = New Point(30, 60)
        lblInformation.Name = "lblInformation"
        lblInformation.Size = New Size(400, 40)
        lblInformation.TabIndex = 1
        lblInformation.Text = "Sa fiche sera conservée pour suivre son historique, même s'il s'installe ailleurs."
        '
        'lblNom
        '
        lblNom.AutoSize = True
        lblNom.Location = New Point(30, 108)
        lblNom.Name = "lblNom"
        lblNom.Size = New Size(50, 19)
        lblNom.TabIndex = 2
        lblNom.Text = "Nom *"
        '
        'txtNom
        '
        txtNom.Location = New Point(30, 130)
        txtNom.MaxLength = 50
        txtNom.Name = "txtNom"
        txtNom.Size = New Size(195, 25)
        txtNom.TabIndex = 3
        '
        'lblPrenom
        '
        lblPrenom.AutoSize = True
        lblPrenom.Location = New Point(235, 108)
        lblPrenom.Name = "lblPrenom"
        lblPrenom.Size = New Size(70, 19)
        lblPrenom.TabIndex = 4
        lblPrenom.Text = "Prénom *"
        '
        'txtPrenom
        '
        txtPrenom.Location = New Point(235, 130)
        txtPrenom.MaxLength = 50
        txtPrenom.Name = "txtPrenom"
        txtPrenom.Size = New Size(195, 25)
        txtPrenom.TabIndex = 5
        '
        'lblType
        '
        lblType.AutoSize = True
        lblType.Location = New Point(30, 166)
        lblType.Name = "lblType"
        lblType.Size = New Size(50, 19)
        lblType.TabIndex = 6
        lblType.Text = "Type *"
        '
        'cboType
        '
        cboType.DropDownStyle = ComboBoxStyle.DropDownList
        cboType.Location = New Point(30, 188)
        cboType.Name = "cboType"
        cboType.Size = New Size(400, 25)
        cboType.TabIndex = 7
        '
        'lblTelephone
        '
        lblTelephone.AutoSize = True
        lblTelephone.Location = New Point(30, 224)
        lblTelephone.Name = "lblTelephone"
        lblTelephone.Size = New Size(80, 19)
        lblTelephone.TabIndex = 8
        lblTelephone.Text = "Téléphone"
        '
        'txtTelephone
        '
        txtTelephone.Location = New Point(30, 246)
        txtTelephone.MaxLength = 20
        txtTelephone.Name = "txtTelephone"
        txtTelephone.Size = New Size(195, 25)
        txtTelephone.TabIndex = 9
        '
        'lblEmail
        '
        lblEmail.AutoSize = True
        lblEmail.Location = New Point(235, 224)
        lblEmail.Name = "lblEmail"
        lblEmail.Size = New Size(50, 19)
        lblEmail.TabIndex = 10
        lblEmail.Text = "E-mail"
        '
        'txtEmail
        '
        txtEmail.Location = New Point(235, 246)
        txtEmail.MaxLength = 100
        txtEmail.Name = "txtEmail"
        txtEmail.Size = New Size(195, 25)
        txtEmail.TabIndex = 11
        '
        'lblErreurs
        '
        lblErreurs.Location = New Point(30, 282)
        lblErreurs.Name = "lblErreurs"
        lblErreurs.Size = New Size(400, 56)
        lblErreurs.TabIndex = 12
        '
        'btnAnnuler
        '
        btnAnnuler.DialogResult = DialogResult.Cancel
        btnAnnuler.Location = New Point(30, 346)
        btnAnnuler.Name = "btnAnnuler"
        btnAnnuler.Size = New Size(190, 38)
        btnAnnuler.TabIndex = 14
        btnAnnuler.Text = "Annuler"
        '
        'btnValider
        '
        btnValider.Location = New Point(240, 346)
        btnValider.Name = "btnValider"
        btnValider.Size = New Size(190, 38)
        btnValider.TabIndex = 13
        btnValider.Text = "Créer la fiche"
        '
        'FrmNouveauRemplacant
        '
        AcceptButton = btnValider
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnAnnuler
        ClientSize = New Size(462, 408)
        Controls.Add(btnValider)
        Controls.Add(btnAnnuler)
        Controls.Add(lblErreurs)
        Controls.Add(txtEmail)
        Controls.Add(lblEmail)
        Controls.Add(txtTelephone)
        Controls.Add(lblTelephone)
        Controls.Add(cboType)
        Controls.Add(lblType)
        Controls.Add(txtPrenom)
        Controls.Add(lblPrenom)
        Controls.Add(txtNom)
        Controls.Add(lblNom)
        Controls.Add(lblInformation)
        Controls.Add(lblTitre)
        Font = New Font("Segoe UI", 10.0F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "FrmNouveauRemplacant"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Remplaçant"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitre As Label
    Friend WithEvents lblInformation As Label
    Friend WithEvents lblNom As Label
    Friend WithEvents txtNom As TextBox
    Friend WithEvents lblPrenom As Label
    Friend WithEvents txtPrenom As TextBox
    Friend WithEvents lblType As Label
    Friend WithEvents cboType As ComboBox
    Friend WithEvents lblTelephone As Label
    Friend WithEvents txtTelephone As TextBox
    Friend WithEvents lblEmail As Label
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents lblErreurs As Label
    Friend WithEvents btnAnnuler As Button
    Friend WithEvents btnValider As Button

End Class
