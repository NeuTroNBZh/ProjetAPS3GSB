<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmFormulaire
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
        lblSousTitre = New Label()
        flpChamps = New FlowLayoutPanel()
        lblErreurs = New Label()
        pnlBoutons = New Panel()
        btnAnnuler = New Button()
        btnValider = New Button()
        pnlBoutons.SuspendLayout()
        SuspendLayout()
        '
        'lblTitre
        '
        lblTitre.Dock = DockStyle.Top
        lblTitre.Name = "lblTitre"
        lblTitre.Padding = New Padding(22, 16, 22, 0)
        lblTitre.Size = New Size(560, 52)
        lblTitre.TabIndex = 0
        '
        'lblSousTitre
        '
        lblSousTitre.Dock = DockStyle.Top
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.Padding = New Padding(24, 0, 22, 6)
        lblSousTitre.Size = New Size(560, 30)
        lblSousTitre.TabIndex = 1
        '
        'flpChamps
        '
        flpChamps.AutoScroll = True
        flpChamps.Dock = DockStyle.Fill
        flpChamps.FlowDirection = FlowDirection.TopDown
        flpChamps.Name = "flpChamps"
        flpChamps.Padding = New Padding(22, 4, 22, 4)
        flpChamps.TabIndex = 2
        flpChamps.WrapContents = False
        '
        'lblErreurs
        '
        lblErreurs.Dock = DockStyle.Bottom
        lblErreurs.Name = "lblErreurs"
        lblErreurs.Padding = New Padding(24, 4, 22, 4)
        lblErreurs.Size = New Size(560, 70)
        lblErreurs.TabIndex = 3
        '
        'pnlBoutons
        '
        pnlBoutons.Controls.Add(btnAnnuler)
        pnlBoutons.Controls.Add(btnValider)
        pnlBoutons.Dock = DockStyle.Bottom
        pnlBoutons.Name = "pnlBoutons"
        pnlBoutons.Size = New Size(560, 60)
        pnlBoutons.TabIndex = 4
        '
        'btnAnnuler
        '
        btnAnnuler.DialogResult = DialogResult.Cancel
        btnAnnuler.Location = New Point(24, 11)
        btnAnnuler.Name = "btnAnnuler"
        btnAnnuler.Size = New Size(160, 38)
        btnAnnuler.TabIndex = 1
        btnAnnuler.Text = "Annuler"
        '
        'btnValider
        '
        btnValider.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnValider.Location = New Point(336, 11)
        btnValider.Name = "btnValider"
        btnValider.Size = New Size(200, 38)
        btnValider.TabIndex = 0
        btnValider.Text = "Enregistrer"
        '
        'FrmFormulaire
        '
        AcceptButton = btnValider
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnAnnuler
        ClientSize = New Size(560, 640)
        Controls.Add(flpChamps)
        Controls.Add(lblErreurs)
        Controls.Add(pnlBoutons)
        Controls.Add(lblSousTitre)
        Controls.Add(lblTitre)
        Font = New Font("Segoe UI", 10.0F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "FrmFormulaire"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB"
        pnlBoutons.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents flpChamps As FlowLayoutPanel
    Friend WithEvents lblErreurs As Label
    Friend WithEvents pnlBoutons As Panel
    Friend WithEvents btnAnnuler As Button
    Friend WithEvents btnValider As Button

End Class
