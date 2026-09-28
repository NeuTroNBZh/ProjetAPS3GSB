<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmConnexion
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
        pnlMarque = New Panel()
        lblVersion = New Label()
        lblApplication = New Label()
        lblLaboratoire = New Label()
        lblLogo = New Label()
        lblTitre = New Label()
        lblSousTitre = New Label()
        lblIdentifiant = New Label()
        txtIdentifiant = New TextBox()
        lblMotDePasse = New Label()
        txtMotDePasse = New TextBox()
        chkAfficherMotDePasse = New CheckBox()
        btnConnexion = New Button()
        lblErreur = New Label()
        pnlMarque.SuspendLayout()
        SuspendLayout()
        '
        'pnlMarque
        '
        pnlMarque.Controls.Add(lblVersion)
        pnlMarque.Controls.Add(lblApplication)
        pnlMarque.Controls.Add(lblLaboratoire)
        pnlMarque.Controls.Add(lblLogo)
        pnlMarque.Dock = DockStyle.Left
        pnlMarque.Location = New Point(0, 0)
        pnlMarque.Name = "pnlMarque"
        pnlMarque.Size = New Size(300, 440)
        pnlMarque.TabIndex = 0
        '
        'lblLogo
        '
        lblLogo.AutoSize = True
        lblLogo.Font = New Font("Segoe UI", 48.0F, FontStyle.Bold)
        lblLogo.Location = New Point(36, 110)
        lblLogo.Name = "lblLogo"
        lblLogo.Size = New Size(160, 86)
        lblLogo.TabIndex = 0
        lblLogo.Text = "GSB"
        '
        'lblLaboratoire
        '
        lblLaboratoire.AutoSize = True
        lblLaboratoire.Font = New Font("Segoe UI", 12.0F)
        lblLaboratoire.Location = New Point(42, 196)
        lblLaboratoire.Name = "lblLaboratoire"
        lblLaboratoire.Size = New Size(170, 21)
        lblLaboratoire.TabIndex = 1
        lblLaboratoire.Text = "Galaxy Swiss Bourdin"
        '
        'lblApplication
        '
        lblApplication.AutoSize = True
        lblApplication.Font = New Font("Segoe UI Light", 11.0F)
        lblApplication.Location = New Point(42, 226)
        lblApplication.Name = "lblApplication"
        lblApplication.Size = New Size(190, 20)
        lblApplication.TabIndex = 2
        lblApplication.Text = "Comptes-rendus de visite"
        '
        'lblVersion
        '
        lblVersion.AutoSize = True
        lblVersion.Font = New Font("Segoe UI", 8.5F)
        lblVersion.Location = New Point(42, 400)
        lblVersion.Name = "lblVersion"
        lblVersion.Size = New Size(60, 15)
        lblVersion.TabIndex = 3
        lblVersion.Text = "Version"
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Location = New Point(350, 56)
        lblTitre.Name = "lblTitre"
        lblTitre.Size = New Size(120, 32)
        lblTitre.TabIndex = 1
        lblTitre.Text = "Connexion"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(352, 96)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.Size = New Size(260, 20)
        lblSousTitre.TabIndex = 2
        lblSousTitre.Text = "Identifiez-vous pour accéder à vos comptes-rendus."
        '
        'lblIdentifiant
        '
        lblIdentifiant.AutoSize = True
        lblIdentifiant.Location = New Point(352, 146)
        lblIdentifiant.Name = "lblIdentifiant"
        lblIdentifiant.Size = New Size(75, 19)
        lblIdentifiant.TabIndex = 3
        lblIdentifiant.Text = "Identifiant"
        '
        'txtIdentifiant
        '
        txtIdentifiant.Location = New Point(352, 168)
        txtIdentifiant.MaxLength = 30
        txtIdentifiant.Name = "txtIdentifiant"
        txtIdentifiant.Size = New Size(330, 25)
        txtIdentifiant.TabIndex = 4
        '
        'lblMotDePasse
        '
        lblMotDePasse.AutoSize = True
        lblMotDePasse.Location = New Point(352, 212)
        lblMotDePasse.Name = "lblMotDePasse"
        lblMotDePasse.Size = New Size(95, 19)
        lblMotDePasse.TabIndex = 5
        lblMotDePasse.Text = "Mot de passe"
        '
        'txtMotDePasse
        '
        txtMotDePasse.Location = New Point(352, 234)
        txtMotDePasse.MaxLength = 100
        txtMotDePasse.Name = "txtMotDePasse"
        txtMotDePasse.Size = New Size(330, 25)
        txtMotDePasse.TabIndex = 6
        txtMotDePasse.UseSystemPasswordChar = True
        '
        'chkAfficherMotDePasse
        '
        chkAfficherMotDePasse.AutoSize = True
        chkAfficherMotDePasse.Location = New Point(352, 266)
        chkAfficherMotDePasse.Name = "chkAfficherMotDePasse"
        chkAfficherMotDePasse.Size = New Size(180, 23)
        chkAfficherMotDePasse.TabIndex = 7
        chkAfficherMotDePasse.Text = "Afficher le mot de passe"
        '
        'btnConnexion
        '
        btnConnexion.Location = New Point(352, 304)
        btnConnexion.Name = "btnConnexion"
        btnConnexion.Size = New Size(330, 40)
        btnConnexion.TabIndex = 8
        btnConnexion.Text = "Se connecter"
        '
        'lblErreur
        '
        lblErreur.Location = New Point(352, 352)
        lblErreur.Name = "lblErreur"
        lblErreur.Size = New Size(330, 60)
        lblErreur.TabIndex = 9
        '
        'FrmConnexion
        '
        AcceptButton = btnConnexion
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(730, 440)
        Controls.Add(lblErreur)
        Controls.Add(btnConnexion)
        Controls.Add(chkAfficherMotDePasse)
        Controls.Add(txtMotDePasse)
        Controls.Add(lblMotDePasse)
        Controls.Add(txtIdentifiant)
        Controls.Add(lblIdentifiant)
        Controls.Add(lblSousTitre)
        Controls.Add(lblTitre)
        Controls.Add(pnlMarque)
        Font = New Font("Segoe UI", 10.0F)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        Name = "FrmConnexion"
        StartPosition = FormStartPosition.CenterScreen
        Text = "GSB - Comptes-rendus de visite"
        pnlMarque.ResumeLayout(False)
        pnlMarque.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents pnlMarque As Panel
    Friend WithEvents lblLogo As Label
    Friend WithEvents lblLaboratoire As Label
    Friend WithEvents lblApplication As Label
    Friend WithEvents lblVersion As Label
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents lblIdentifiant As Label
    Friend WithEvents txtIdentifiant As TextBox
    Friend WithEvents lblMotDePasse As Label
    Friend WithEvents txtMotDePasse As TextBox
    Friend WithEvents chkAfficherMotDePasse As CheckBox
    Friend WithEvents btnConnexion As Button
    Friend WithEvents lblErreur As Label

End Class
