Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Consultation des médicaments (EX-22) : recherche, filtre par famille, fiche avec effets,
''' contre-indications, composition, interactions et posologie.
''' </summary>
Public Class FrmMedicaments

    Private Const ToutesFamilles As String = "Toutes les familles"

    Private ReadOnly _service As ServiceConsultation
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private _medicaments As IReadOnlyList(Of Medicament) = Array.Empty(Of Medicament)()
    Private _depotAffiche As String
    Private _demandeFiche As Integer
    Private _remplissage As Boolean

    Public Sub New(service As ServiceConsultation, utilisateur As UtilisateurConnecte)
        InitializeComponent()
        _service = service
        _utilisateur = utilisateur

        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        lblRecherche.ForeColor = Theme.BleuFonce
        lblNombre.ForeColor = Theme.TexteGris
        Theme.StyliserBoutonSecondaire(btnFermer)
        Theme.StyliserGrille(dgvMedicaments)
        For Each c In {("Nom", "Nom commercial", 30), ("Famille", "Famille", 40), ("Depot", "Dépôt légal", 18), ("Prix", "Prix éch.", 12)}
            dgvMedicaments.Columns.Add(New DataGridViewTextBoxColumn() With {
                .Name = c.Item1, .HeaderText = c.Item2, .FillWeight = c.Item3, .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
        dgvMedicaments.Columns("Prix").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        pnlFiche.AfficherMessage("Sélectionnez un médicament pour afficher sa fiche.")
    End Sub

    Private Async Sub FrmMedicaments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UseWaitCursor = True
        Try
            _medicaments = Await Task.Run(Function() _service.Medicaments(_utilisateur))
            If IsDisposed Then Return
            cboFamille.Items.Add(ToutesFamilles)
            cboFamille.Items.AddRange(_medicaments.Select(Function(m) m.LibelleFamille).Distinct().OrderBy(Function(f) f).ToArray())
            cboFamille.SelectedIndex = 0   ' déclenche l'affichage
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            UseWaitCursor = False
        End Try
    End Sub

    Private Sub Filtre_Change(sender As Object, e As EventArgs) Handles txtRecherche.TextChanged, cboFamille.SelectedIndexChanged, chkRetires.CheckedChanged
        Dim texte = txtRecherche.Text.Trim()
        Dim famille = TryCast(cboFamille.SelectedItem, String)
        Dim visibles = _medicaments.Where(Function(m)
                                              If Not m.Actif AndAlso Not chkRetires.Checked Then Return False
                                              If famille IsNot Nothing AndAlso famille <> ToutesFamilles AndAlso m.LibelleFamille <> famille Then Return False
                                              Return texte.Length = 0 OrElse
                                                     m.NomCommercial.Contains(texte, StringComparison.CurrentCultureIgnoreCase) OrElse
                                                     m.DepotLegal.Contains(texte, StringComparison.CurrentCultureIgnoreCase)
                                          End Function).ToList()

        Dim depotSelectionne = _depotAffiche
        _remplissage = True
        dgvMedicaments.Rows.Clear()
        For Each m In visibles
            Dim i = dgvMedicaments.Rows.Add(m.NomCommercial, m.LibelleFamille, m.DepotLegal, m.PrixEchantillon.ToString("C", CultureInfo.CurrentCulture))
            dgvMedicaments.Rows(i).Tag = m
            If Not m.Actif Then dgvMedicaments.Rows(i).DefaultCellStyle.ForeColor = Theme.TexteGris
            If m.DepotLegal = depotSelectionne Then dgvMedicaments.Rows(i).Selected = True
        Next
        If dgvMedicaments.SelectedRows.Count = 0 AndAlso dgvMedicaments.Rows.Count > 0 Then dgvMedicaments.Rows(0).Selected = True
        _remplissage = False
        ChargerFicheSelection()
        lblNombre.Text = $"{visibles.Count} médicament(s)"
        If visibles.Count = 0 Then pnlFiche.AfficherMessage("Aucun médicament ne correspond à la recherche.")
    End Sub

    Private Sub dgvMedicaments_SelectionChanged(sender As Object, e As EventArgs) Handles dgvMedicaments.SelectionChanged
        If Not _remplissage Then ChargerFicheSelection()
    End Sub

    Private Async Sub ChargerFicheSelection()
        If dgvMedicaments.SelectedRows.Count = 0 Then Return
        Dim m = TryCast(dgvMedicaments.SelectedRows(0).Tag, Medicament)
        If m Is Nothing OrElse m.DepotLegal = _depotAffiche Then Return

        _demandeFiche += 1
        Dim demande = _demandeFiche
        Try
            Dim fiche = Await Task.Run(Function() _service.FicheMedicament(_utilisateur, m.DepotLegal))
            If IsDisposed OrElse demande <> _demandeFiche Then Return
            _depotAffiche = m.DepotLegal
            AfficherFiche(fiche)
        Catch ex As ErreurMetierException
            pnlFiche.AfficherMessage(ex.Message)
        End Try
    End Sub

    Private Sub AfficherFiche(fiche As FicheMedicament)
        Dim m = fiche.Medicament
        pnlFiche.SuspendLayout()
        pnlFiche.Vider()
        pnlFiche.AjouterTitre(m.NomCommercial)
        pnlFiche.AjouterSousTitre(m.LibelleFamille)
        If m.Actif Then
            pnlFiche.AjouterBadge("Commercialisé", Color.FromArgb(232, 245, 233), Color.FromArgb(46, 125, 50))
        Else
            pnlFiche.AjouterBadge("Retiré", Color.FromArgb(236, 239, 243), Theme.TexteGris)
        End If

        pnlFiche.AjouterSection("Informations")
        pnlFiche.AjouterInformation("Dépôt légal", m.DepotLegal)
        pnlFiche.AjouterInformation("Commercialisé depuis le", fiche.DateCommercialisation?.ToString("dd/MM/yyyy"))
        pnlFiche.AjouterInformation("Prix d'un échantillon", m.PrixEchantillon.ToString("C", CultureInfo.CurrentCulture))

        pnlFiche.AjouterSection("Effets thérapeutiques")
        pnlFiche.AjouterTexte(If(fiche.Effets, "Non renseignés."))

        pnlFiche.AjouterSection("Contre-indications")
        pnlFiche.AjouterTexte(If(fiche.ContreIndications, "Non renseignées."), Theme.Erreur)

        pnlFiche.AjouterSection("Composition")
        If fiche.Composition.Count = 0 Then
            pnlFiche.AjouterTexte("Non renseignée.", Theme.TexteGris, italique:=True)
        Else
            pnlFiche.AjouterTableau({("Composant", 60.0F), ("Quantité", 40.0F)},
                fiche.Composition.Select(Function(c) New Object() {c.Composant, $"{c.Quantite:0.###} {c.Unite}"}))
        End If

        pnlFiche.AjouterSection("Interactions médicamenteuses")
        If fiche.Interactions.Count = 0 Then
            pnlFiche.AjouterTexte("Aucune interaction connue.", Theme.TexteGris, italique:=True)
        Else
            pnlFiche.AjouterTableau({("Interaction", 42.0F), ("Effet", 58.0F)},
                fiche.Interactions.Select(Function(i) New Object() {
                    If(i.EstPerturbateur, $"{m.NomCommercial} perturbe {i.NomAutre}", $"{i.NomAutre} perturbe {m.NomCommercial}"),
                    i.Description}))
        End If

        pnlFiche.AjouterSection("Posologie")
        If fiche.Posologies.Count = 0 Then
            pnlFiche.AjouterTexte("Non renseignée.", Theme.TexteGris, italique:=True)
        Else
            pnlFiche.AjouterTableau({("Patient", 18.0F), ("Présentation", 24.0F), ("Dosage", 14.0F), ("Posologie", 44.0F)},
                fiche.Posologies.Select(Function(p) New Object() {p.TypeIndividu, p.Presentation, p.Dosage, p.Texte}))
        End If
        pnlFiche.ResumeLayout()
        pnlFiche.AutoScrollPosition = New Point(0, 0)
    End Sub

End Class
