<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAccueil
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
        btnDeconnexion = New Button()
        btnMotDePasse = New Button()
        lblRattachement = New Label()
        lblUtilisateur = New Label()
        lblApplication = New Label()
        lblBienvenue = New Label()
        flpModules = New FlowLayoutPanel()
        lblPied = New Label()
        pnlEntete.SuspendLayout()
        SuspendLayout()
        '
        'pnlEntete
        '
        pnlEntete.Controls.Add(btnDeconnexion)
        pnlEntete.Controls.Add(btnMotDePasse)
        pnlEntete.Controls.Add(lblRattachement)
        pnlEntete.Controls.Add(lblUtilisateur)
        pnlEntete.Controls.Add(lblApplication)
        pnlEntete.Dock = DockStyle.Top
        pnlEntete.Location = New Point(0, 0)
        pnlEntete.Name = "pnlEntete"
        pnlEntete.Size = New Size(900, 90)
        pnlEntete.TabIndex = 0
        '
        'lblApplication
        '
        lblApplication.AutoSize = True
        lblApplication.Font = New Font("Segoe UI", 20.0F, FontStyle.Bold)
        lblApplication.Location = New Point(24, 22)
        lblApplication.Name = "lblApplication"
        lblApplication.Size = New Size(100, 37)
        lblApplication.TabIndex = 0
        lblApplication.Text = "GSB"
        '
        'lblUtilisateur
        '
        lblUtilisateur.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblUtilisateur.Font = New Font("Segoe UI Semibold", 11.0F)
        lblUtilisateur.Location = New Point(340, 18)
        lblUtilisateur.Name = "lblUtilisateur"
        lblUtilisateur.Size = New Size(300, 22)
        lblUtilisateur.TabIndex = 1
        lblUtilisateur.TextAlign = ContentAlignment.MiddleRight
        '
        'lblRattachement
        '
        lblRattachement.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblRattachement.Location = New Point(340, 44)
        lblRattachement.Name = "lblRattachement"
        lblRattachement.Size = New Size(300, 22)
        lblRattachement.TabIndex = 2
        lblRattachement.TextAlign = ContentAlignment.MiddleRight
        '
        'btnMotDePasse
        '
        btnMotDePasse.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnMotDePasse.Location = New Point(656, 26)
        btnMotDePasse.Name = "btnMotDePasse"
        btnMotDePasse.Size = New Size(110, 36)
        btnMotDePasse.TabIndex = 3
        btnMotDePasse.Text = "Mot de passe"
        '
        'btnDeconnexion
        '
        btnDeconnexion.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnDeconnexion.Location = New Point(774, 26)
        btnDeconnexion.Name = "btnDeconnexion"
        btnDeconnexion.Size = New Size(110, 36)
        btnDeconnexion.TabIndex = 4
        btnDeconnexion.Text = "Déconnexion"
        '
        'lblBienvenue
        '
        lblBienvenue.AutoSize = True
        lblBienvenue.Location = New Point(30, 110)
        lblBienvenue.Name = "lblBienvenue"
        lblBienvenue.Size = New Size(120, 32)
        lblBienvenue.TabIndex = 1
        lblBienvenue.Text = "Bonjour"
        '
        'flpModules
        '
        flpModules.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        flpModules.AutoScroll = True
        flpModules.Location = New Point(24, 160)
        flpModules.Name = "flpModules"
        flpModules.Size = New Size(852, 350)
        flpModules.TabIndex = 2
        '
        'lblPied
        '
        lblPied.Dock = DockStyle.Bottom
        lblPied.Location = New Point(0, 526)
        lblPied.Name = "lblPied"
        lblPied.Padding = New Padding(24, 0, 0, 0)
        lblPied.Size = New Size(900, 34)
        lblPied.TabIndex = 3
        lblPied.TextAlign = ContentAlignment.MiddleLeft
        '
        'FrmAccueil
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 560)
        Controls.Add(flpModules)
        Controls.Add(lblBienvenue)
        Controls.Add(lblPied)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(760, 480)
        Name = "FrmAccueil"
        StartPosition = FormStartPosition.CenterScreen
        Text = "GSB - Comptes-rendus de visite"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblApplication As Label
    Friend WithEvents lblUtilisateur As Label
    Friend WithEvents lblRattachement As Label
    Friend WithEvents btnMotDePasse As Button
    Friend WithEvents btnDeconnexion As Button
    Friend WithEvents lblBienvenue As Label
    Friend WithEvents flpModules As FlowLayoutPanel
    Friend WithEvents lblPied As Label

End Class
