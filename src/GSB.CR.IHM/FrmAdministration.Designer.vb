<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAdministration
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
        tabCollaborateurs = New TabPage()
        scCollaborateurs = New SplitContainer()
        dgvCollaborateurs = New DataGridView()
        pnlFicheCollaborateur = New PanneauFiche()
        flpBarreCollaborateurs = New FlowLayoutPanel()
        tabPortefeuilles = New TabPage()
        scPortefeuilles = New SplitContainer()
        dgvPortefeuille = New DataGridView()
        lblPortefeuille = New Label()
        dgvSansVisiteur = New DataGridView()
        lblSansVisiteur = New Label()
        flpBarrePortefeuilles = New FlowLayoutPanel()
        tabReferentiels = New TabPage()
        tabRef = New TabControl()
        tabRefPraticiens = New TabPage()
        dgvPraticiens = New DataGridView()
        flpBarrePraticiens = New FlowLayoutPanel()
        tabRefMedicaments = New TabPage()
        dgvMedicaments = New DataGridView()
        flpBarreMedicaments = New FlowLayoutPanel()
        tabRefMotifs = New TabPage()
        dgvMotifs = New DataGridView()
        flpBarreMotifs = New FlowLayoutPanel()
        tabJournal = New TabPage()
        dgvJournal = New DataGridView()
        flpBarreJournal = New FlowLayoutPanel()
        pnlActions = New Panel()
        lblStatut = New Label()
        btnFermer = New Button()
        pnlEntete.SuspendLayout()
        tabOnglets.SuspendLayout()
        tabCollaborateurs.SuspendLayout()
        CType(scCollaborateurs, System.ComponentModel.ISupportInitialize).BeginInit()
        scCollaborateurs.Panel1.SuspendLayout()
        scCollaborateurs.Panel2.SuspendLayout()
        scCollaborateurs.SuspendLayout()
        tabPortefeuilles.SuspendLayout()
        CType(scPortefeuilles, System.ComponentModel.ISupportInitialize).BeginInit()
        scPortefeuilles.Panel1.SuspendLayout()
        scPortefeuilles.Panel2.SuspendLayout()
        scPortefeuilles.SuspendLayout()
        tabReferentiels.SuspendLayout()
        tabRef.SuspendLayout()
        tabRefPraticiens.SuspendLayout()
        tabRefMedicaments.SuspendLayout()
        tabRefMotifs.SuspendLayout()
        tabJournal.SuspendLayout()
        pnlActions.SuspendLayout()
        SuspendLayout()
        '
        'pnlEntete
        '
        pnlEntete.Controls.Add(lblSousTitre)
        pnlEntete.Controls.Add(lblTitre)
        pnlEntete.Dock = DockStyle.Top
        pnlEntete.Name = "pnlEntete"
        pnlEntete.Size = New Size(1240, 68)
        pnlEntete.TabIndex = 0
        '
        'lblTitre
        '
        lblTitre.AutoSize = True
        lblTitre.Font = New Font("Segoe UI Semibold", 16.0F)
        lblTitre.Location = New Point(22, 6)
        lblTitre.Name = "lblTitre"
        lblTitre.TabIndex = 0
        lblTitre.Text = "Administration"
        '
        'lblSousTitre
        '
        lblSousTitre.AutoSize = True
        lblSousTitre.Location = New Point(24, 40)
        lblSousTitre.Name = "lblSousTitre"
        lblSousTitre.TabIndex = 1
        lblSousTitre.Text = "Comptes, affectations, portefeuilles, référentiels et journal des connexions"
        '
        'tabOnglets
        '
        tabOnglets.Controls.Add(tabCollaborateurs)
        tabOnglets.Controls.Add(tabPortefeuilles)
        tabOnglets.Controls.Add(tabReferentiels)
        tabOnglets.Controls.Add(tabJournal)
        tabOnglets.Dock = DockStyle.Fill
        tabOnglets.Name = "tabOnglets"
        tabOnglets.Padding = New Point(16, 6)
        tabOnglets.TabIndex = 1
        '
        'tabCollaborateurs
        '
        tabCollaborateurs.Controls.Add(scCollaborateurs)
        tabCollaborateurs.Controls.Add(flpBarreCollaborateurs)
        tabCollaborateurs.Name = "tabCollaborateurs"
        tabCollaborateurs.Padding = New Padding(12, 0, 12, 12)
        tabCollaborateurs.Text = "Collaborateurs"
        '
        'flpBarreCollaborateurs
        '
        flpBarreCollaborateurs.Dock = DockStyle.Top
        flpBarreCollaborateurs.Name = "flpBarreCollaborateurs"
        flpBarreCollaborateurs.Padding = New Padding(0, 10, 0, 6)
        flpBarreCollaborateurs.Size = New Size(1200, 96)
        '
        'scCollaborateurs
        '
        scCollaborateurs.Dock = DockStyle.Fill
        scCollaborateurs.Name = "scCollaborateurs"
        scCollaborateurs.Panel1.Controls.Add(dgvCollaborateurs)
        scCollaborateurs.Panel2.Controls.Add(pnlFicheCollaborateur)
        scCollaborateurs.Panel2.Padding = New Padding(8, 0, 0, 0)
        scCollaborateurs.Size = New Size(1200, 460)
        scCollaborateurs.SplitterDistance = 760
        '
        'dgvCollaborateurs
        '
        dgvCollaborateurs.Dock = DockStyle.Fill
        dgvCollaborateurs.Name = "dgvCollaborateurs"
        '
        'pnlFicheCollaborateur
        '
        pnlFicheCollaborateur.BorderStyle = BorderStyle.FixedSingle
        pnlFicheCollaborateur.Dock = DockStyle.Fill
        pnlFicheCollaborateur.Name = "pnlFicheCollaborateur"
        '
        'tabPortefeuilles
        '
        tabPortefeuilles.Controls.Add(scPortefeuilles)
        tabPortefeuilles.Controls.Add(flpBarrePortefeuilles)
        tabPortefeuilles.Name = "tabPortefeuilles"
        tabPortefeuilles.Padding = New Padding(12, 0, 12, 12)
        tabPortefeuilles.Text = "Portefeuilles"
        '
        'flpBarrePortefeuilles
        '
        flpBarrePortefeuilles.Dock = DockStyle.Top
        flpBarrePortefeuilles.Name = "flpBarrePortefeuilles"
        flpBarrePortefeuilles.Padding = New Padding(0, 10, 0, 6)
        flpBarrePortefeuilles.Size = New Size(1200, 96)
        '
        'scPortefeuilles
        '
        scPortefeuilles.Dock = DockStyle.Fill
        scPortefeuilles.Name = "scPortefeuilles"
        scPortefeuilles.Panel1.Controls.Add(dgvPortefeuille)
        scPortefeuilles.Panel1.Controls.Add(lblPortefeuille)
        scPortefeuilles.Panel2.Controls.Add(dgvSansVisiteur)
        scPortefeuilles.Panel2.Controls.Add(lblSansVisiteur)
        scPortefeuilles.Panel2.Padding = New Padding(8, 0, 0, 0)
        scPortefeuilles.Size = New Size(1200, 460)
        scPortefeuilles.SplitterDistance = 600
        '
        'lblPortefeuille
        '
        lblPortefeuille.Dock = DockStyle.Top
        lblPortefeuille.Name = "lblPortefeuille"
        lblPortefeuille.Size = New Size(600, 30)
        lblPortefeuille.Text = "Portefeuille"
        '
        'dgvPortefeuille
        '
        dgvPortefeuille.Dock = DockStyle.Fill
        dgvPortefeuille.Name = "dgvPortefeuille"
        '
        'lblSansVisiteur
        '
        lblSansVisiteur.Dock = DockStyle.Top
        lblSansVisiteur.Name = "lblSansVisiteur"
        lblSansVisiteur.Size = New Size(590, 30)
        lblSansVisiteur.Text = "Praticiens sans visiteur"
        '
        'dgvSansVisiteur
        '
        dgvSansVisiteur.Dock = DockStyle.Fill
        dgvSansVisiteur.Name = "dgvSansVisiteur"
        '
        'tabReferentiels
        '
        tabReferentiels.Controls.Add(tabRef)
        tabReferentiels.Name = "tabReferentiels"
        tabReferentiels.Padding = New Padding(12, 8, 12, 12)
        tabReferentiels.Text = "Référentiels"
        '
        'tabRef
        '
        tabRef.Controls.Add(tabRefPraticiens)
        tabRef.Controls.Add(tabRefMedicaments)
        tabRef.Controls.Add(tabRefMotifs)
        tabRef.Dock = DockStyle.Fill
        tabRef.Name = "tabRef"
        '
        'tabRefPraticiens
        '
        tabRefPraticiens.Controls.Add(dgvPraticiens)
        tabRefPraticiens.Controls.Add(flpBarrePraticiens)
        tabRefPraticiens.Name = "tabRefPraticiens"
        tabRefPraticiens.Padding = New Padding(8)
        tabRefPraticiens.Text = "Praticiens"
        '
        'flpBarrePraticiens
        '
        flpBarrePraticiens.Dock = DockStyle.Top
        flpBarrePraticiens.Name = "flpBarrePraticiens"
        flpBarrePraticiens.Size = New Size(1170, 56)
        '
        'dgvPraticiens
        '
        dgvPraticiens.Dock = DockStyle.Fill
        dgvPraticiens.Name = "dgvPraticiens"
        '
        'tabRefMedicaments
        '
        tabRefMedicaments.Controls.Add(dgvMedicaments)
        tabRefMedicaments.Controls.Add(flpBarreMedicaments)
        tabRefMedicaments.Name = "tabRefMedicaments"
        tabRefMedicaments.Padding = New Padding(8)
        tabRefMedicaments.Text = "Médicaments"
        '
        'flpBarreMedicaments
        '
        flpBarreMedicaments.Dock = DockStyle.Top
        flpBarreMedicaments.Name = "flpBarreMedicaments"
        flpBarreMedicaments.Size = New Size(1170, 56)
        '
        'dgvMedicaments
        '
        dgvMedicaments.Dock = DockStyle.Fill
        dgvMedicaments.Name = "dgvMedicaments"
        '
        'tabRefMotifs
        '
        tabRefMotifs.Controls.Add(dgvMotifs)
        tabRefMotifs.Controls.Add(flpBarreMotifs)
        tabRefMotifs.Name = "tabRefMotifs"
        tabRefMotifs.Padding = New Padding(8)
        tabRefMotifs.Text = "Motifs de visite"
        '
        'flpBarreMotifs
        '
        flpBarreMotifs.Dock = DockStyle.Top
        flpBarreMotifs.Name = "flpBarreMotifs"
        flpBarreMotifs.Size = New Size(1170, 56)
        '
        'dgvMotifs
        '
        dgvMotifs.Dock = DockStyle.Fill
        dgvMotifs.Name = "dgvMotifs"
        '
        'tabJournal
        '
        tabJournal.Controls.Add(dgvJournal)
        tabJournal.Controls.Add(flpBarreJournal)
        tabJournal.Name = "tabJournal"
        tabJournal.Padding = New Padding(12, 0, 12, 12)
        tabJournal.Text = "Journal des connexions"
        '
        'flpBarreJournal
        '
        flpBarreJournal.Dock = DockStyle.Top
        flpBarreJournal.Name = "flpBarreJournal"
        flpBarreJournal.Padding = New Padding(0, 10, 0, 6)
        flpBarreJournal.Size = New Size(1200, 60)
        '
        'dgvJournal
        '
        dgvJournal.Dock = DockStyle.Fill
        dgvJournal.Name = "dgvJournal"
        '
        'pnlActions
        '
        pnlActions.Controls.Add(lblStatut)
        pnlActions.Controls.Add(btnFermer)
        pnlActions.Dock = DockStyle.Bottom
        pnlActions.Name = "pnlActions"
        pnlActions.Size = New Size(1240, 60)
        pnlActions.TabIndex = 2
        '
        'lblStatut
        '
        lblStatut.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblStatut.Location = New Point(24, 12)
        lblStatut.Name = "lblStatut"
        lblStatut.Size = New Size(1040, 36)
        lblStatut.TextAlign = ContentAlignment.MiddleLeft
        '
        'btnFermer
        '
        btnFermer.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnFermer.DialogResult = DialogResult.Cancel
        btnFermer.Location = New Point(1096, 11)
        btnFermer.Name = "btnFermer"
        btnFermer.Size = New Size(120, 38)
        btnFermer.TabIndex = 0
        btnFermer.Text = "Fermer"
        '
        'FrmAdministration
        '
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnFermer
        ClientSize = New Size(1240, 780)
        Controls.Add(tabOnglets)
        Controls.Add(pnlActions)
        Controls.Add(pnlEntete)
        Font = New Font("Segoe UI", 10.0F)
        MinimumSize = New Size(1000, 600)
        Name = "FrmAdministration"
        StartPosition = FormStartPosition.CenterParent
        Text = "GSB - Administration"
        pnlEntete.ResumeLayout(False)
        pnlEntete.PerformLayout()
        tabOnglets.ResumeLayout(False)
        tabCollaborateurs.ResumeLayout(False)
        scCollaborateurs.Panel1.ResumeLayout(False)
        scCollaborateurs.Panel2.ResumeLayout(False)
        CType(scCollaborateurs, System.ComponentModel.ISupportInitialize).EndInit()
        scCollaborateurs.ResumeLayout(False)
        tabPortefeuilles.ResumeLayout(False)
        scPortefeuilles.Panel1.ResumeLayout(False)
        scPortefeuilles.Panel2.ResumeLayout(False)
        CType(scPortefeuilles, System.ComponentModel.ISupportInitialize).EndInit()
        scPortefeuilles.ResumeLayout(False)
        tabReferentiels.ResumeLayout(False)
        tabRef.ResumeLayout(False)
        tabRefPraticiens.ResumeLayout(False)
        tabRefMedicaments.ResumeLayout(False)
        tabRefMotifs.ResumeLayout(False)
        tabJournal.ResumeLayout(False)
        pnlActions.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlEntete As Panel
    Friend WithEvents lblTitre As Label
    Friend WithEvents lblSousTitre As Label
    Friend WithEvents tabOnglets As TabControl
    Friend WithEvents tabCollaborateurs As TabPage
    Friend WithEvents flpBarreCollaborateurs As FlowLayoutPanel
    Friend WithEvents scCollaborateurs As SplitContainer
    Friend WithEvents dgvCollaborateurs As DataGridView
    Friend WithEvents pnlFicheCollaborateur As PanneauFiche
    Friend WithEvents tabPortefeuilles As TabPage
    Friend WithEvents flpBarrePortefeuilles As FlowLayoutPanel
    Friend WithEvents scPortefeuilles As SplitContainer
    Friend WithEvents lblPortefeuille As Label
    Friend WithEvents dgvPortefeuille As DataGridView
    Friend WithEvents lblSansVisiteur As Label
    Friend WithEvents dgvSansVisiteur As DataGridView
    Friend WithEvents tabReferentiels As TabPage
    Friend WithEvents tabRef As TabControl
    Friend WithEvents tabRefPraticiens As TabPage
    Friend WithEvents flpBarrePraticiens As FlowLayoutPanel
    Friend WithEvents dgvPraticiens As DataGridView
    Friend WithEvents tabRefMedicaments As TabPage
    Friend WithEvents flpBarreMedicaments As FlowLayoutPanel
    Friend WithEvents dgvMedicaments As DataGridView
    Friend WithEvents tabRefMotifs As TabPage
    Friend WithEvents flpBarreMotifs As FlowLayoutPanel
    Friend WithEvents dgvMotifs As DataGridView
    Friend WithEvents tabJournal As TabPage
    Friend WithEvents flpBarreJournal As FlowLayoutPanel
    Friend WithEvents dgvJournal As DataGridView
    Friend WithEvents pnlActions As Panel
    Friend WithEvents lblStatut As Label
    Friend WithEvents btnFermer As Button

End Class
