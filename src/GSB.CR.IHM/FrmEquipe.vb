Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Suivi d'équipe : « Ma région » pour le délégué (EX-31, EX-32, EX-35), « Mon secteur » pour le responsable
''' (EX-40, EX-41). Synthèse, activité par visiteur, comptes-rendus validés en lecture seule, praticiens à revoir.
''' </summary>
Public Class FrmEquipe

    Private Const Personnalisee As String = "Période personnalisée"
    Private Const TousLesVisiteurs As String = "Tous les visiteurs"

    Private ReadOnly _equipe As ServiceEquipe
    Private ReadOnly _activite As ServiceActivite
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private ReadOnly _perimetre As Perimetre
    Private _rapports As IReadOnlyList(Of RapportResume) = Array.Empty(Of RapportResume)()
    Private _reglagePeriode As Boolean

    ''' <param name="activite">Utilisé seulement pour les périodes prédéfinies (mêmes choix que « Mon activité »).</param>
    Public Sub New(equipe As ServiceEquipe, activite As ServiceActivite, utilisateur As UtilisateurConnecte)
        InitializeComponent()
        _equipe = equipe
        _activite = activite
        _utilisateur = utilisateur
        _perimetre = equipe.PerimetreDe(utilisateur)

        Dim estSecteur = _perimetre.Type = TypePerimetre.Secteur
        lblTitre.Text = If(estSecteur, "Mon secteur", "Ma région")
        lblSousTitre.Text = $"{_perimetre.Libelle} · activité des visiteurs et comptes-rendus validés"
        Text = $"GSB - {lblTitre.Text}"

        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        For Each l In {lblPeriode, lblDu, lblAu, lblFiltreVisiteur}
            l.ForeColor = Theme.BleuFonce
        Next
        For Each l In {lblVisiteurs, lblNbRapports, lblARevoir}
            l.ForeColor = Theme.TexteGris
        Next
        Theme.StyliserBoutonPrincipal(btnAfficher)
        Theme.StyliserBoutonPrincipal(btnDetailVisiteur)
        Theme.StyliserBoutonPrincipal(btnConsulter)
        Theme.StyliserBoutonSecondaire(btnFermer)
        For Each g In {dgvVisiteurs, dgvRapports, dgvARevoir}
            Theme.StyliserGrille(g)
        Next
        For Each t In {tabSynthese, tabVisiteurs, tabRapports, tabARevoir}
            t.BackColor = Theme.Blanc
        Next

        CreerColonnes(dgvVisiteurs, {("Visiteur", "Visiteur", 20)}.
            Concat(If(estSecteur, {("Region", "Région", 14)}, Array.Empty(Of (String, String, Integer))())).
            Concat({("Visites", "Visites", 9), ("Praticiens", "Praticiens vus", 11), ("Confiance", "Confiance moy.", 11),
                    ("Echantillons", "Échantillons", 11), ("Cout", "Coût", 10), ("Brouillons", "Brouillons", 9), ("Derniere", "Dernière visite", 12)}))
        CreerColonnes(dgvRapports, {("Date", "Visite le", 11), ("Visiteur", "Visiteur", 18), ("Praticien", "Praticien", 18),
                                    ("Ville", "Ville", 12), ("Remplacant", "Remplaçant vu", 14), ("Motif", "Motif", 20)})
        RenduARevoir.CreerColonnes(dgvARevoir, avecSuivi:=True)

        dtpDebut.MinDate = _activite.DebutConsultable
        dtpFin.MinDate = _activite.DebutConsultable
        Dim finDuJour = _activite.Aujourdhui.AddDays(1).AddSeconds(-1)
        dtpDebut.MaxDate = finDuJour
        dtpFin.MaxDate = finDuJour
        cboPeriode.Items.AddRange(_activite.PeriodesPredefinies().ToArray())
        cboPeriode.Items.Add(Personnalisee)
        cboPeriode.SelectedIndex = 0
    End Sub

    Private Shared Sub CreerColonnes(grille As DataGridView, colonnes As IEnumerable(Of (Nom As String, Titre As String, Poids As Integer)))
        For Each c In colonnes
            grille.Columns.Add(New DataGridViewTextBoxColumn() With {
                .Name = c.Nom, .HeaderText = c.Titre, .FillWeight = c.Poids, .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
    End Sub

    Private Async Sub FrmEquipe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await ChargerPeriode()
        Await ChargerRapports()
        Await ChargerARevoir()
    End Sub

    ' ------------------------------------------------------------------
    ' Période : synthèse et activité par visiteur
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
        If Not _reglagePeriode Then cboPeriode.SelectedItem = Personnalisee
    End Sub

    Private Async Sub btnAfficher_Click(sender As Object, e As EventArgs) Handles btnAfficher.Click
        Await ChargerPeriode()
    End Sub

    Private Async Function ChargerPeriode() As Task
        Dim debut = dtpDebut.Value.Date, fin = dtpFin.Value.Date
        btnAfficher.Enabled = False
        UseWaitCursor = True
        Try
            Dim synthese = Await Task.Run(Function() _equipe.Synthese(_utilisateur, debut, fin))
            Dim membres = Await Task.Run(Function() _equipe.ActiviteParMembre(_utilisateur, debut, fin))
            If IsDisposed Then Return
            RenduSynthese.Afficher(pnlSynthese, synthese, _perimetre.Libelle)
            AfficherMembres(membres)
        Catch ex As ErreurMetierException
            pnlSynthese.AfficherMessage(ex.Message)
        Finally
            UseWaitCursor = False
            btnAfficher.Enabled = True
        End Try
    End Function

    Private Sub AfficherMembres(membres As IReadOnlyList(Of ActiviteMembre))
        Dim culture = CultureInfo.CurrentCulture
        Dim estSecteur = _perimetre.Type = TypePerimetre.Secteur
        dgvVisiteurs.Rows.Clear()
        For Each m In membres
            Dim valeurs As New List(Of Object) From {If(m.Profil = Profil.Delegue, $"{m.NomComplet} (délégué)", m.NomComplet)}
            If estSecteur Then valeurs.Add(m.NomRegion)
            valeurs.AddRange({m.NbVisites, m.NbPraticiens, m.ConfianceMoyenne?.ToString("0.0", culture), m.NbEchantillons,
                              m.CoutEchantillons.ToString("C", culture), m.NbBrouillons, m.DateDerniereVisite?.ToString("dd/MM/yyyy")})
            Dim ligne = dgvVisiteurs.Rows(dgvVisiteurs.Rows.Add(valeurs.ToArray()))
            ligne.Tag = m
            ' Aucune visite sur la période : à signaler au délégué
            If m.NbVisites = 0 Then
                ligne.Cells("Visites").Style.ForeColor = Theme.Erreur
                ligne.Cells("Visites").Style.SelectionForeColor = Theme.Erreur
            End If
        Next
        tabVisiteurs.Text = $"Visiteurs ({membres.Count})"
        btnDetailVisiteur.Enabled = membres.Count > 0
    End Sub

    Private Sub dgvVisiteurs_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvVisiteurs.CellDoubleClick
        If e.RowIndex >= 0 Then btnDetailVisiteur.PerformClick()
    End Sub

    Private Sub btnDetailVisiteur_Click(sender As Object, e As EventArgs) Handles btnDetailVisiteur.Click
        If dgvVisiteurs.SelectedRows.Count = 0 Then Return
        Dim m = TryCast(dgvVisiteurs.SelectedRows(0).Tag, ActiviteMembre)
        If m Is Nothing Then Return
        Using frm = FrmFiche.SyntheseMembre(_equipe, _utilisateur, m, dtpDebut.Value.Date, dtpFin.Value.Date)
            frm.ShowDialog(Me)
        End Using
    End Sub

    ' ------------------------------------------------------------------
    ' Comptes-rendus de l'équipe
    ' ------------------------------------------------------------------

    Private Async Function ChargerRapports() As Task
        Try
            _rapports = Await Task.Run(Function() _equipe.RapportsEquipe(_utilisateur))
            If IsDisposed Then Return
            cboVisiteur.Items.Clear()
            cboVisiteur.Items.Add(TousLesVisiteurs)
            cboVisiteur.Items.AddRange(_rapports.Select(Function(r) r.Auteur).Distinct().OrderBy(Function(a) a).ToArray())
            cboVisiteur.SelectedIndex = 0   ' déclenche l'affichage
        Catch ex As ErreurMetierException
            lblNbRapports.Text = ex.Message
        End Try
    End Function

    Private Sub FiltreRapports_Change(sender As Object, e As EventArgs) Handles cboVisiteur.SelectedIndexChanged, txtRecherche.TextChanged
        Dim auteur = TryCast(cboVisiteur.SelectedItem, String)
        Dim texte = txtRecherche.Text.Trim()
        Dim visibles = _rapports.Where(Function(r) (auteur Is Nothing OrElse auteur = TousLesVisiteurs OrElse r.Auteur = auteur) AndAlso
                                                   (texte.Length = 0 OrElse
                                                    r.Praticien.Contains(texte, StringComparison.CurrentCultureIgnoreCase) OrElse
                                                    If(r.Ville, "").Contains(texte, StringComparison.CurrentCultureIgnoreCase))).ToList()
        dgvRapports.Rows.Clear()
        For Each r In visibles
            Dim i = dgvRapports.Rows.Add(r.DateVisite.ToString("dd/MM/yyyy"), r.Auteur, r.Praticien, r.Ville, r.Remplacant, r.Motif)
            dgvRapports.Rows(i).Tag = r
        Next
        lblNbRapports.Text = $"{visibles.Count} compte(s)-rendu(s) validé(s) sur 3 ans"
        btnConsulter.Enabled = visibles.Count > 0
        tabRapports.Text = $"Comptes-rendus ({_rapports.Count})"
    End Sub

    Private Sub dgvRapports_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRapports.CellDoubleClick
        If e.RowIndex >= 0 Then btnConsulter.PerformClick()
    End Sub

    Private Sub btnConsulter_Click(sender As Object, e As EventArgs) Handles btnConsulter.Click
        If dgvRapports.SelectedRows.Count = 0 Then Return
        Dim r = TryCast(dgvRapports.SelectedRows(0).Tag, RapportResume)
        If r Is Nothing Then Return
        Using frm = FrmFiche.CompteRendu(_equipe, _utilisateur, r.Numero, r.Auteur)
            frm.ShowDialog(Me)
        End Using
    End Sub

    ' ------------------------------------------------------------------
    ' Praticiens à revoir
    ' ------------------------------------------------------------------

    Private Async Function ChargerARevoir() As Task
        Try
            Dim liste = Await Task.Run(Function() _equipe.PraticiensARevoir(_utilisateur))
            If IsDisposed Then Return
            RenduARevoir.Remplir(dgvARevoir, liste, avecSuivi:=True)
            tabARevoir.Text = $"Praticiens à revoir ({liste.Count})"
            lblARevoir.Text = If(liste.Count = 0,
                "Tous les praticiens suivis par l'équipe sont à jour.",
                $"{liste.Count} praticien(s) suivis par l'équipe à planifier, du plus urgent au moins urgent " &
                $"(une visite tous les {Periodicite.MoisMinimum} à {Periodicite.MoisMaximum} mois).")
        Catch ex As ErreurMetierException
            lblARevoir.Text = ex.Message
        End Try
    End Function

End Class
