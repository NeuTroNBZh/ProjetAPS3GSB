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
        Theme.StyliserGrille(dgvARevoir)
        tabSynthese.BackColor = Theme.Blanc
        tabARevoir.BackColor = Theme.Blanc

        For Each c In {("Situation", "Situation", 16), ("Praticien", "Praticien", 22), ("Ville", "Ville", 14),
                       ("Derniere", "Dernière visite", 13), ("Depuis", "Il y a", 10), ("Prevue", "Prochaine prévue", 13), ("Tel", "Téléphone", 12)}
            dgvARevoir.Columns.Add(New DataGridViewTextBoxColumn() With {
                .Name = c.Item1, .HeaderText = c.Item2, .FillWeight = c.Item3, .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next

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
        Catch ex As ErreurMetierException
            pnlSynthese.AfficherMessage(ex.Message)
        Finally
            UseWaitCursor = False
            btnAfficher.Enabled = True
        End Try
    End Function

    Private Sub AfficherSynthese(s As SyntheseActivite)
        Dim culture = CultureInfo.CurrentCulture
        pnlSynthese.SuspendLayout()
        pnlSynthese.Vider()
        pnlSynthese.AjouterTitre($"Du {s.Debut:dd/MM/yyyy} au {s.Fin:dd/MM/yyyy}")
        pnlSynthese.AjouterSousTitre("Seuls les comptes-rendus validés sont comptés.")

        pnlSynthese.AjouterIndicateurs({
            New TuileIndicateur("Visites", s.NbVisites.ToString("N0", culture),
                                If(s.NbVisitesRemplacant > 0, $"dont {s.NbVisitesRemplacant} avec un remplaçant", Nothing)),
            New TuileIndicateur("Praticiens vus", s.NbPraticiens.ToString("N0", culture)),
            New TuileIndicateur("Confiance moyenne", If(s.ConfianceMoyenne.HasValue, s.ConfianceMoyenne.Value.ToString("0.0", culture), "—"), "sur 5"),
            New TuileIndicateur("Échantillons distribués", s.NbEchantillons.ToString("N0", culture), $"coût : {s.CoutEchantillons.ToString("C", culture)}"),
            New TuileIndicateur("Temps moyen de saisie", Duree(s.TempsSaisieMoyen), $"total : {Duree(s.TempsSaisieTotal)}"),
            New TuileIndicateur("Brouillons à terminer", s.NbBrouillons.ToString("N0", culture), "toutes dates confondues")})

        pnlSynthese.AjouterSection("Visites par mois")
        Dim graphique As New GraphiqueBarres() With {.MessageVide = "Aucune visite validée sur la période."}
        graphique.DefinirDonnees(s.ParMois.Select(Function(m) (
            m.Mois.ToString("MMM yy", culture),
            m.Nombre,
            $"{m.Mois.ToString("MMMM yyyy", culture)} : {m.Nombre} visite(s)")))
        pnlSynthese.AjouterControle(graphique)
        If s.NbVisites > 0 Then
            ' Vue tableau du graphique (lecture exacte des valeurs)
            pnlSynthese.AjouterTableau({("Mois", 60.0F), ("Visites", 40.0F)},
                s.ParMois.Where(Function(m) m.Nombre > 0).Select(Function(m) New Object() {m.Mois.ToString("MMMM yyyy", culture), m.Nombre}),
                hauteurMax:=160)
        End If

        pnlSynthese.AjouterSection("Motifs des visites")
        If s.ParMotif.Count = 0 Then
            pnlSynthese.AjouterTexte("Aucune visite.", Theme.TexteGris, italique:=True)
        Else
            pnlSynthese.AjouterTableau({("Motif", 60.0F), ("Visites", 20.0F), ("Part", 20.0F)},
                s.ParMotif.Select(Function(m) New Object() {m.Libelle, m.Nombre, (m.Nombre / CDbl(s.NbVisites)).ToString("P0", culture)}))
        End If

        pnlSynthese.AjouterSection("Produits présentés")
        If s.ProduitsPresentes.Count = 0 Then
            pnlSynthese.AjouterTexte("Aucun produit présenté.", Theme.TexteGris, italique:=True)
        Else
            pnlSynthese.AjouterTableau({("Produit", 60.0F), ("Présentations", 40.0F)},
                s.ProduitsPresentes.Select(Function(p) New Object() {p.Libelle, p.Nombre}))
        End If

        pnlSynthese.AjouterSection("Échantillons distribués")
        If s.Echantillons.Count = 0 Then
            pnlSynthese.AjouterTexte("Aucun échantillon distribué.", Theme.TexteGris, italique:=True)
        Else
            pnlSynthese.AjouterTableau({("Produit", 50.0F), ("Quantité", 25.0F), ("Coût", 25.0F)},
                s.Echantillons.Select(Function(x) New Object() {x.NomCommercial, x.Quantite, x.Cout.ToString("C", culture)}))
        End If
        pnlSynthese.ResumeLayout()
        pnlSynthese.AutoScrollPosition = New Point(0, 0)
    End Sub

    ''' <summary>Durée lisible : « 6 min 40 s ».</summary>
    Private Shared Function Duree(secondes As Decimal?) As String
        If Not secondes.HasValue Then Return "—"
        Dim t = TimeSpan.FromSeconds(CDbl(secondes.Value))
        If t.TotalHours >= 1 Then Return $"{CInt(Math.Floor(t.TotalHours))} h {t.Minutes:00}"
        If t.TotalMinutes >= 1 Then Return $"{t.Minutes} min {t.Seconds:00} s"
        Return $"{t.Seconds} s"
    End Function

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
        dgvARevoir.Rows.Clear()
        For Each x In liste
            Dim situation = If(x.ProchainePrevueDepassee AndAlso x.Etat <> EtatPeriodicite.ARevoir AndAlso x.Etat <> EtatPeriodicite.JamaisVisite,
                               "Visite prévue dépassée", AffichagePeriodicite.Libelle(x.Etat))
            Dim i = dgvARevoir.Rows.Add(situation, x.Praticien.NomComplet, x.Praticien.Ville,
                                        x.Praticien.DateDerniereVisite?.ToString("dd/MM/yyyy"),
                                        If(x.JoursDepuisDerniereVisite.HasValue, $"{x.JoursDepuisDerniereVisite} j", ""),
                                        x.Praticien.DateProchainePrevue?.ToString("dd/MM/yyyy"), x.Praticien.Telephone)
            Dim ligne = dgvARevoir.Rows(i)
            ligne.Tag = x
            Dim couleurs = AffichagePeriodicite.Couleurs(If(situation = "Visite prévue dépassée", EtatPeriodicite.ARevoirBientot, x.Etat))
            ligne.Cells("Situation").Style.BackColor = couleurs.Fond
            ligne.Cells("Situation").Style.ForeColor = couleurs.Encre
            ligne.Cells("Situation").Style.Font = New Font(dgvARevoir.Font, FontStyle.Bold)
        Next
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
