Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Module « Mon activité » : synthèse de l'activité sur une période (EX-23)
''' et praticiens du portefeuille à revoir (EX-24), avec saisie directe d'un compte-rendu.
''' </summary>
Public Class FrmMonActivite

    Private Const Personnalisee As String = "Période personnalisée"

    Private ReadOnly _activite As ServiceActivite
    Private ReadOnly _rapports As ServiceRapports
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private _reglagePeriode As Boolean
    Private _synthese As SyntheseActivite

    Public Sub New(activite As ServiceActivite, rapports As ServiceRapports, utilisateur As UtilisateurConnecte)
        InitializeComponent()
        _activite = activite
        _rapports = rapports
        _utilisateur = utilisateur

        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        For Each l In {lblPeriode, lblDu, lblAu}
            l.ForeColor = Theme.BleuFonce
        Next
        lblARevoir.ForeColor = Theme.TexteGris
        Theme.StyliserBoutonPrincipal(btnAfficher)
        Theme.StyliserBoutonPrincipal(btnNouveauCR)
        Theme.StyliserBoutonSecondaire(btnFermer)
        Theme.StyliserBoutonSecondaire(btnExporter)
        Theme.StyliserGrille(dgvARevoir)
        tabSynthese.BackColor = Theme.Blanc
        tabARevoir.BackColor = Theme.Blanc

        RenduARevoir.CreerColonnes(dgvARevoir, avecSuivi:=False)

        ' Périodes : bornes limitées à la profondeur de consultation (3 ans) et à aujourd'hui
        dtpDebut.MinDate = _activite.DebutConsultable
        dtpFin.MinDate = _activite.DebutConsultable
        Dim finDuJour = _activite.Aujourdhui.AddDays(1).AddSeconds(-1)
        dtpDebut.MaxDate = finDuJour
        dtpFin.MaxDate = finDuJour
        cboPeriode.Items.AddRange(_activite.PeriodesPredefinies().ToArray())
        cboPeriode.Items.Add(Personnalisee)
        cboPeriode.SelectedIndex = 0
    End Sub

    Private Async Sub FrmMonActivite_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await ChargerSynthese()
        Await ChargerARevoir()
    End Sub

    ' ------------------------------------------------------------------
    ' Période
    ' ------------------------------------------------------------------

    Private Sub cboPeriode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPeriode.SelectedIndexChanged
        Dim p = TryCast(cboPeriode.SelectedItem, Periode)
        If p Is Nothing Then Return
        _reglagePeriode = True
        dtpDebut.Value = p.Debut
        dtpFin.Value = p.Fin
        _reglagePeriode = False
        If IsHandleCreated Then btnAfficher.PerformClick()
    End Sub

    Private Sub Dates_ValueChanged(sender As Object, e As EventArgs) Handles dtpDebut.ValueChanged, dtpFin.ValueChanged
        ' Une date modifiée à la main passe en « période personnalisée »
        If Not _reglagePeriode Then cboPeriode.SelectedItem = Personnalisee
    End Sub

    Private Async Sub btnAfficher_Click(sender As Object, e As EventArgs) Handles btnAfficher.Click
        Await ChargerSynthese()
    End Sub

    ' ------------------------------------------------------------------
    ' Synthèse
    ' ------------------------------------------------------------------

    Private Async Function ChargerSynthese() As Task
        Dim debut = dtpDebut.Value.Date, fin = dtpFin.Value.Date
        btnAfficher.Enabled = False
        UseWaitCursor = True
        Try
            Dim synthese = Await Task.Run(Function() _activite.MaSynthese(_utilisateur, debut, fin))
            If IsDisposed Then Return
            AfficherSynthese(synthese)
            _synthese = synthese
        Catch ex As ErreurMetierException
            _synthese = Nothing
            pnlSynthese.AfficherMessage(ex.Message)
        Finally
            UseWaitCursor = False
            btnAfficher.Enabled = True
            btnExporter.Enabled = _synthese IsNot Nothing
        End Try
    End Function

    ''' <summary>Export CSV de la synthèse affichée (EX-51).</summary>
    Private Sub btnExporter_Click(sender As Object, e As EventArgs) Handles btnExporter.Click
        If _synthese IsNot Nothing Then EnregistrementExport.Exporter(Me, "Mon activité", _synthese)
    End Sub

    Private Sub AfficherSynthese(s As SyntheseActivite)
        RenduSynthese.Afficher(pnlSynthese, s, "Mon activité")
    End Sub

    ' ------------------------------------------------------------------
    ' Praticiens à revoir
    ' ------------------------------------------------------------------

    Private Async Function ChargerARevoir() As Task
        Try
            Dim liste = Await Task.Run(Function() _activite.PraticiensARevoir(_utilisateur))
            If IsDisposed Then Return
            AfficherARevoir(liste)
        Catch ex As ErreurMetierException
            lblARevoir.Text = ex.Message
        End Try
    End Function

    Private Sub AfficherARevoir(liste As IReadOnlyList(Of PraticienARevoir))
        RenduARevoir.Remplir(dgvARevoir, liste, avecSuivi:=False)
        tabARevoir.Text = $"Praticiens à revoir ({liste.Count})"
        lblARevoir.Text = If(liste.Count = 0,
            "Tous les praticiens de votre portefeuille sont à jour. Bravo !",
            $"{liste.Count} praticien(s) de votre portefeuille à planifier, du plus urgent au moins urgent. " &
            $"Règle : une visite tous les {Periodicite.MoisMinimum} à {Periodicite.MoisMaximum} mois.")
        btnNouveauCR.Enabled = liste.Count > 0
    End Sub

    Private Sub dgvARevoir_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvARevoir.CellDoubleClick
        If e.RowIndex >= 0 Then btnNouveauCR.PerformClick()
    End Sub

    Private Async Sub btnNouveauCR_Click(sender As Object, e As EventArgs) Handles btnNouveauCR.Click
        If dgvARevoir.SelectedRows.Count = 0 Then Return
        Dim x = TryCast(dgvARevoir.SelectedRows(0).Tag, PraticienARevoir)
        If x Is Nothing Then Return

        Using frm As New FrmCompteRendu(_rapports, _utilisateur, Nothing, x.Praticien.Numero)
            If frm.ShowDialog(Me) <> DialogResult.OK Then Return
        End Using
        ' Le nouveau CR peut changer la synthèse et la liste à revoir
        Await ChargerARevoir()
        Await ChargerSynthese()
    End Sub

End Class
