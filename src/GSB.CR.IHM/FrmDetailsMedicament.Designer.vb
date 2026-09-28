<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmDetailsMedicament
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
        tabOnglets = New TabControl()
        tabComposition = New TabPage()
        dgvComposition = New DataGridView()
        flpBarreComposition = New FlowLayoutPanel()
        tabInteractions = New TabPage()
        dgvInteractions = New DataGridView()
        flpBarreInteractions = New FlowLayoutPanel()
        tabPosologie = New TabPage()
        dgvPosologie = New DataGridView()
        flpBarrePosologie = New FlowLayoutPanel()
        pnlActions = New Panel()
        lblStatut = New Label()
        btnFermer = New Button()
        pnlEntete.SuspendLayout()
        tabOnglets.SuspendLayout()
        tabComposition.SuspendLayout()
        CType(dgvComposition, System.ComponentModel.ISupportInitialize).BeginInit()
        tabInteractions.SuspendLayout()
        CType(dgvInteractions, System.ComponentModel.ISupportInitialize).BeginInit()
        tabPosologie.SuspendLayout()
        CType(dgvPosologie, System.ComponentModel.ISupportInitialize).BeginInit()
        pnlActions.SuspendLayout()
        SuspendLayout()
        '
        'pnlEntete
        '
        pnlEntete.Controls.Add(lblSousTitre)
        pnlEntete.Controls.Add(lblTitre)
        pnlEntete.Dock = DockStyle.Top
        pnlEntete.Name = "pnlEntete"
        pnlEntete.Size = New Size(1000, 68)
        pnlEntete.TabIndex = 0
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Font = New Font("Segoe UI Semibold", 16.0F)
        lblTitre.Location = New Point(22, 6)
        lblTitre.Name = "lblTitre"
        lblTitre.TabIndex = 0
        lblTitre.Text = "Médicament"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.TabIndex = 1
        lblSousTitre.Text = "Composition, interactions et posologie"
        '
        'tabOnglets
        '
        tabOnglets.Controls.Add(tabComposition)
        tabOnglets.Controls.Add(tabInteractions)
        tabOnglets.Controls.Add(tabPosologie)
        tabOnglets.Dock = DockStyle.Fill
        tabOnglets.Name = "tabOnglets"
        tabOnglets.Padding = New Point(16, 6)
        tabOnglets.TabIndex = 1
        '
        'tabComposition
        '
        tabComposition.Controls.Add(dgvComposition)
        tabComposition.Controls.Add(flpBarreComposition)
        tabComposition.Name = "tabComposition"
        tabComposition.Padding = New Padding(12, 0, 12, 12)
        tabComposition.Text = "Composition"
        '
        'flpBarreComposition
        '
        flpBarreComposition.Dock = DockStyle.Top
        flpBarreComposition.Name = "flpBarreComposition"
        flpBarreComposition.Padding = New Padding(0, 10, 0, 6)
        flpBarreComposition.Size = New Size(960, 58)
        '
        'dgvComposition
        '
        dgvComposition.Dock = DockStyle.Fill
        dgvComposition.Name = "dgvComposition"
        '
        'tabInteractions
        '
        tabInteractions.Controls.Add(dgvInteractions)
        tabInteractions.Controls.Add(flpBarreInteractions)
        tabInteractions.Name = "tabInteractions"
        tabInteractions.Padding = New Padding(12, 0, 12, 12)
        tabInteractions.Text = "Interactions"
        '
        'flpBarreInteractions
        '
        flpBarreInteractions.Dock = DockStyle.Top
        flpBarreInteractions.Name = "flpBarreInteractions"
        flpBarreInteractions.Padding = New Padding(0, 10, 0, 6)
        flpBarreInteractions.Size = New Size(960, 58)
        '
        'dgvInteractions
        '
        dgvInteractions.Dock = DockStyle.Fill
        dgvInteractions.Name = "dgvInteractions"
        '
        'tabPosologie
        '
        tabPosologie.Controls.Add(dgvPosologie)
        tabPosologie.Controls.Add(flpBarrePosologie)
        tabPosologie.Name = "tabPosologie"
        tabPosologie.Padding = New Padding(12, 0, 12, 12)
        tabPosologie.Text = "Posologie"
        '
        'flpBarrePosologie
        '
        flpBarrePosologie.Dock = DockStyle.Top
        flpBarrePosologie.Name = "flpBarrePosologie"
        flpBarrePosologie.Padding = New Padding(0, 10, 0, 6)
        flpBarrePosologie.Size = New Size(960, 58)
        '
        'dgvPosologie
        '
        dgvPosologie.Dock = DockStyle.Fill
        dgvPosologie.Name = "dgvPosologie"
        '
        'pnlActions
        '
        pnlActions.Controls.Add(lblStatut)
        pnlActions.Controls.Add(btnFermer)
        pnlActions.Dock = DockStyle.Bottom
        pnlActions.Name = "pnlActions"
        pnlActions.Size = New Size(1000, 60)
        pnlActions.TabIndex = 2
        '
        'lblStatut
        '
        lblStatut.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblStatut.Location = New Point(24, 12)
        lblStatut.Name = "lblStatut"
        lblStatut.Size = New Size(800, 36)
        lblStatut.TextAlign = ContentAlignment.MiddleLeft
        '
        'btnFermer
        '
        btnFermer.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnFermer.DialogResult = DialogResult.Cancel
        btnFermer.Location = New Point(856, 11)
        btnFermer.Name = "btnFermer"
        btnFermer.Size = New Size(120, 38)
        btnFermer.TabIndex = 0
        btnFermer.Text = "Fermer"
        '
        'FrmDetailsMedicament
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnFermer
        ClientSize = New Size(1000, 620)
        Controls.Add(tabOnglets)
        Controls.Add(pnlActions)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(820, 480)
        Name = "FrmDetailsMedicament"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Médicament"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        tabOnglets.ResumeLayout(False)
        tabComposition.ResumeLayout(False)
        CType(dgvComposition, System.ComponentModel.ISupportInitialize).EndInit()
        tabInteractions.ResumeLayout(False)
        CType(dgvInteractions, System.ComponentModel.ISupportInitialize).EndInit()
        tabPosologie.ResumeLayout(False)
        CType(dgvPosologie, System.ComponentModel.ISupportInitialize).EndInit()
        pnlActions.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents tabOnglets As TabControl
    Friend WithEvents tabComposition As TabPage
    Friend WithEvents flpBarreComposition As FlowLayoutPanel
    Friend WithEvents dgvComposition As DataGridView
    Friend WithEvents tabInteractions As TabPage
    Friend WithEvents flpBarreInteractions As FlowLayoutPanel
    Friend WithEvents dgvInteractions As DataGridView
    Friend WithEvents tabPosologie As TabPage
    Friend WithEvents flpBarrePosologie As FlowLayoutPanel
    Friend WithEvents dgvPosologie As DataGridView
    Friend WithEvents pnlActions As Panel
    Friend WithEvents lblStatut As Label
    Friend WithEvents btnFermer As Button

End Class
