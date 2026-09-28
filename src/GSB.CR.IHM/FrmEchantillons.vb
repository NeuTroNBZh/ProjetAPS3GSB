Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Échantillons de l'équipe : contrôle de stock attribué / distribué du mois (EX-34) et, pour le délégué,
''' saisie des dotations mensuelles (EX-33). Le responsable consulte sans pouvoir modifier.
''' </summary>
Public Class FrmEchantillons

    Private ReadOnly _service As ServiceEchantillons
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private ReadOnly _peutSaisir As Boolean
    Private _chargement As Boolean = True

    Public Sub New(service As ServiceEchantillons, utilisateur As UtilisateurConnecte)
        InitializeComponent()
        _service = service
        _utilisateur = utilisateur
        _peutSaisir = ServiceEchantillons.PeutSaisir(utilisateur)

        Dim perimetre = Perimetres.DeLEquipe(utilisateur)
        lblSousTitre.Text = $"{perimetre.Libelle} · " & If(_peutSaisir, "dotations et contrôle de stock", "contrôle de stock (consultation)")

        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        pnlSaisie.BackColor = Theme.BleuClair
        pnlSaisie.Padding = New Padding(0, 0, 0, 0)
        Theme.StyliserSection(lblSectionSaisie)
        For Each l In {lblMois, lblVisiteurSaisie, lblProduit, lblQuantite}
            l.ForeColor = Theme.BleuFonce
        Next
        For Each l In {lblNombre, lblAideSaisie, lblLegende}
            l.ForeColor = Theme.TexteGris
        Next
        Theme.StyliserBoutonPrincipal(btnEnregistrer)
        Theme.StyliserBoutonSecondaire(btnSupprimer)
        Theme.StyliserBoutonSecondaire(btnFermer)
        Theme.StyliserGrille(dgvStock)
        For Each c In {("Visiteur", "Visiteur", 22), ("Produit", "Produit", 22), ("Attribue", "Attribué", 11),
                       ("Distribue", "Distribué", 11), ("Ecart", "Écart", 10), ("Situation", "Situation", 20)}
            dgvStock.Columns.Add(New DataGridViewTextBoxColumn() With {
                .Name = c.Item1, .HeaderText = c.Item2, .FillWeight = c.Item3, .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
        For Each nom In {"Attribue", "Distribue", "Ecart"}
            dgvStock.Columns(nom).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Next

        ' Le responsable consulte seulement : pas de panneau de saisie
        pnlSaisie.Visible = _peutSaisir
        For Each m In _service.MoisProposes()
            cboMois.Items.Add(New Periode(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m.ToString("MMMM yyyy", CultureInfo.CurrentCulture)), m, m))
        Next
        cboMois.SelectedIndex = 1   ' mois en cours (le premier est le mois prochain)
    End Sub

    Private ReadOnly Property MoisChoisi As Date
        Get
            Return DirectCast(cboMois.SelectedItem, Periode).Debut
        End Get
    End Property

    Private Async Sub FrmEchantillons_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If _peutSaisir Then
                Dim membres = Await Task.Run(Function() _service.Membres(_utilisateur))
                Dim produits = Await Task.Run(Function() _service.Medicaments())
                If IsDisposed Then Return
                cboVisiteurSaisie.Items.AddRange(membres.ToArray())
                cboProduit.Items.AddRange(produits.ToArray())
                If cboVisiteurSaisie.Items.Count > 0 Then cboVisiteurSaisie.SelectedIndex = 0
                If cboProduit.Items.Count > 0 Then cboProduit.SelectedIndex = 0
            End If
            _chargement = False
            Await ChargerStock()
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Async Sub Filtres_Change(sender As Object, e As EventArgs) Handles cboMois.SelectedIndexChanged, chkEcarts.CheckedChanged
        If _chargement Then Return
        Await ChargerStock()
    End Sub

    Private Async Function ChargerStock() As Task
        Dim mois = MoisChoisi, ecarts = chkEcarts.Checked
        UseWaitCursor = True
        Try
            Dim lignes = Await Task.Run(Function() _service.Stock(_utilisateur, mois, ecarts))
            If IsDisposed Then Return
            AfficherStock(lignes)
        Catch ex As ErreurMetierException
            lblNombre.Text = ex.Message
        Finally
            UseWaitCursor = False
        End Try
    End Function

    Private Sub AfficherStock(lignes As IReadOnlyList(Of LigneStock))
        dgvStock.Rows.Clear()
        For Each l In lignes
            Dim situation = If(l.EnDepassement, $"Dépassement de {-l.Ecart}",
                               If(l.Attribue = 0, "Aucune dotation", If(l.Distribue = 0, "Rien distribué", "Conforme")))
            Dim i = dgvStock.Rows.Add(l.Visiteur, l.Produit, l.Attribue, l.Distribue, l.Ecart, situation)
            Dim ligne = dgvStock.Rows(i)
            ligne.Tag = l
            If l.EnDepassement Then
                ' Couleur d'alerte + libellé explicite (jamais la couleur seule)
                For Each nom In {"Ecart", "Situation"}
                    ligne.Cells(nom).Style.ForeColor = Theme.Erreur
                    ligne.Cells(nom).Style.SelectionForeColor = Theme.Erreur
                    ligne.Cells(nom).Style.BackColor = Color.FromArgb(253, 236, 234)
                    ligne.Cells(nom).Style.SelectionBackColor = Color.FromArgb(249, 214, 210)
                    ligne.Cells(nom).Style.Font = New Font(dgvStock.Font, FontStyle.Bold)
                Next
            End If
        Next
        Dim nbDepassements = lignes.Where(Function(l) l.EnDepassement).Count()
        lblNombre.Text = $"{lignes.Count} ligne(s)" & If(nbDepassements > 0, $" · {nbDepassements} dépassement(s)", "")
        lblNombre.ForeColor = If(nbDepassements > 0, Theme.Erreur, Theme.TexteGris)
    End Sub

    ' ------------------------------------------------------------------
    ' Saisie des dotations (délégué)
    ' ------------------------------------------------------------------

    ''' <summary>Une ligne sélectionnée pré-remplit la saisie (correction d'une dotation existante).</summary>
    Private Sub dgvStock_SelectionChanged(sender As Object, e As EventArgs) Handles dgvStock.SelectionChanged
        If Not _peutSaisir OrElse dgvStock.SelectedRows.Count = 0 Then Return
        Dim l = TryCast(dgvStock.SelectedRows(0).Tag, LigneStock)
        If l Is Nothing Then Return
        For Each m As MembreEquipe In cboVisiteurSaisie.Items
            If m.Matricule = l.Matricule Then cboVisiteurSaisie.SelectedItem = m
        Next
        For Each p As Medicament In cboProduit.Items
            If p.DepotLegal = l.DepotLegal Then cboProduit.SelectedItem = p
        Next
        If l.Attribue > 0 Then nudQuantite.Value = Math.Min(l.Attribue, nudQuantite.Maximum)
    End Sub

    Private Async Sub btnEnregistrer_Click(sender As Object, e As EventArgs) Handles btnEnregistrer.Click
        Dim membre = TryCast(cboVisiteurSaisie.SelectedItem, MembreEquipe)
        Dim produit = TryCast(cboProduit.SelectedItem, Medicament)
        Dim mois = MoisChoisi, quantite = CInt(nudQuantite.Value)
        Await Executer(Function() _service.EnregistrerDotation(_utilisateur, membre?.Matricule, produit?.DepotLegal, mois, quantite),
                       $"Dotation enregistrée : {quantite} × {produit?.NomCommercial} pour {membre?.NomComplet}.")
    End Sub

    Private Async Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        Dim membre = TryCast(cboVisiteurSaisie.SelectedItem, MembreEquipe)
        Dim produit = TryCast(cboProduit.SelectedItem, Medicament)
        If membre Is Nothing OrElse produit Is Nothing Then Return
        Dim reponse = MessageBox.Show(Me, $"Supprimer la dotation de {produit.NomCommercial} pour {membre.NomComplet} ({cboMois.Text}) ?",
                                      "GSB", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        If reponse <> DialogResult.Yes Then Return
        Dim mois = MoisChoisi
        Await Executer(Function() _service.SupprimerDotation(_utilisateur, membre.Matricule, produit.DepotLegal, mois),
                       "Dotation supprimée.")
    End Sub

    Private Async Function Executer(action As Func(Of IReadOnlyList(Of String)), messageSucces As String) As Task
        btnEnregistrer.Enabled = False
        btnSupprimer.Enabled = False
        Try
            Dim erreurs = Await Task.Run(action)
            If IsDisposed Then Return
            If erreurs.Count > 0 Then
                lblMessage.ForeColor = Theme.Erreur
                lblMessage.Text = Theme.EnPuces(erreurs)
            Else
                lblMessage.ForeColor = Color.FromArgb(46, 125, 50)
                lblMessage.Text = messageSucces
                Await ChargerStock()
            End If
        Catch ex As ErreurMetierException
            lblMessage.ForeColor = Theme.Erreur
            lblMessage.Text = ex.Message
        Finally
            btnEnregistrer.Enabled = True
            btnSupprimer.Enabled = True
        End Try
    End Function

End Class
