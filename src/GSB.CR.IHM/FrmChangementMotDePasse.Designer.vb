<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmChangementMotDePasse
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
        lblActuel = New Label()
        txtActuel = New TextBox()
        lblNouveau = New Label()
        txtNouveau = New TextBox()
        lblConfirmation = New Label()
        txtConfirmation = New TextBox()
        lblRegles = New Label()
        lblErreurs = New Label()
        btnValider = New Button()
        btnAnnuler = New Button()
        SuspendLayout()
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(30, 24)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(250, 32)
        lblTitre.TabIndex = 0
        lblTitre.Text = "Changer mon mot de passe"
        '
        'lblInformation
        '
        lblInformation.Location = New Point(32, 66)
        lblInformation.Name = "lblInformation"
        lblInformation.Size = New Size(420, 40)
        lblInformation.TabIndex = 1
        '
        'lblActuel
        '
        lblActuel.AutoSize = True
        lblActuel.Location = New Point(32, 112)
        lblActuel.Name = "lblActuel"
        lblActuel.Size = New Size(150, 19)
        lblActuel.TabIndex = 2
        lblActuel.Text = "Mot de passe actuel"
        '
        'txtActuel
        '
        txtActuel.Location = New Point(32, 134)
        txtActuel.Name = "txtActuel"
        txtActuel.Size = New Size(420, 25)
        txtActuel.TabIndex = 3
        txtActuel.UseSystemPasswordChar = True
        '
        'lblNouveau
        '
        lblNouveau.AutoSize = True
        lblNouveau.Location = New Point(32, 172)
        lblNouveau.Name = "lblNouveau"
        lblNouveau.Size = New Size(160, 19)
        lblNouveau.TabIndex = 4
        lblNouveau.Text = "Nouveau mot de passe"
        '
        'txtNouveau
        '
        txtNouveau.Location = New Point(32, 194)
        txtNouveau.Name = "txtNouveau"
        txtNouveau.Size = New Size(420, 25)
        txtNouveau.TabIndex = 5
        txtNouveau.UseSystemPasswordChar = True
        '
        'lblConfirmation
        '
        lblConfirmation.AutoSize = True
        lblConfirmation.Location = New Point(32, 232)
        lblConfirmation.Name = "lblConfirmation"
        lblConfirmation.Size = New Size(260, 19)
        lblConfirmation.TabIndex = 6
        lblConfirmation.Text = "Confirmation du nouveau mot de passe"
        '
        'txtConfirmation
        '
        txtConfirmation.Location = New Point(32, 254)
        txtConfirmation.Name = "txtConfirmation"
        txtConfirmation.Size = New Size(420, 25)
        txtConfirmation.TabIndex = 7
        txtConfirmation.UseSystemPasswordChar = True
        '
        'lblRegles
        '
        lblRegles.Location = New Point(32, 288)
        lblRegles.Name = "lblRegles"
        lblRegles.Size = New Size(420, 38)
        lblRegles.TabIndex = 8
        '
        'lblErreurs
        '
        lblErreurs.Location = New Point(32, 330)
        lblErreurs.Name = "lblErreurs"
        lblErreurs.Size = New Size(420, 80)
        lblErreurs.TabIndex = 9
        '
        'btnValider
        '
        btnValider.Location = New Point(252, 420)
        btnValider.Name = "btnValider"
        btnValider.Size = New Size(200, 38)
        btnValider.TabIndex = 10
        btnValider.Text = "Valider"
        '
        'btnAnnuler
        '
        btnAnnuler.DialogResult = DialogResult.Cancel
        btnAnnuler.Location = New Point(32, 420)
        btnAnnuler.Name = "btnAnnuler"
        btnAnnuler.Size = New Size(200, 38)
        btnAnnuler.TabIndex = 11
        btnAnnuler.Text = "Annuler"
        '
        'FrmChangementMotDePasse
        '
        AcceptButton = btnValider
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnAnnuler
        ClientSize = New Size(486, 484)
        Controls.Add(btnAnnuler)
        Controls.Add(btnValider)
        Controls.Add(lblErreurs)
        Controls.Add(lblRegles)
        Controls.Add(txtConfirmation)
        Controls.Add(lblConfirmation)
        Controls.Add(txtNouveau)
        Controls.Add(lblNouveau)
        Controls.Add(txtActuel)
        Controls.Add(lblActuel)
        Controls.Add(lblInformation)
        Controls.Add(lblTitre)
        Font = New Font("Segoe UI", 10.0F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "FrmChangementMotDePasse"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterScreen
        Text = "GSB - Mot de passe"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitre As Label
    Friend WithEvents lblInformation As Label
    Friend WithEvents lblActuel As Label
    Friend WithEvents txtActuel As TextBox
    Friend WithEvents lblNouveau As Label
    Friend WithEvents txtNouveau As TextBox
    Friend WithEvents lblConfirmation As Label
    Friend WithEvents txtConfirmation As TextBox
    Friend WithEvents lblRegles As Label
    Friend WithEvents lblErreurs As Label
    Friend WithEvents btnValider As Button
    Friend WithEvents btnAnnuler As Button

End Class
