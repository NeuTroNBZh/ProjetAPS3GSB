Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Liste des comptes-rendus du collaborateur connecté sur les 3 dernières années (EX-20),
''' avec création, ouverture et suppression des brouillons (EX-29).
''' </summary>
Public Class FrmMesComptesRendus

    Private Const FiltreTous As Integer = 0
    Private Const FiltreBrouillons As Integer = 1
    Private Const FiltreValides As Integer = 2

    Private ReadOnly _service As ServiceRapports
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private _rapports As IReadOnlyList(Of RapportResume) = Array.Empty(Of RapportResume)()

    Public Sub New(service As ServiceRapports, utilisateur As UtilisateurConnecte)
        InitializeComponent()
        _service = service
        _utilisateur = utilisateur
        AppliquerTheme()
        CreerColonnes()
        cboEtat.Items.AddRange({"Tous les comptes-rendus", "Brouillons à terminer", "Comptes-rendus validés"})
        cboEtat.SelectedIndex = FiltreTous
    End Sub

    Private Sub AppliquerTheme()
        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        lblAfficher.ForeColor = Theme.BleuFonce
        lblRecherche.ForeColor = Theme.BleuFonce
        lblNombre.ForeColor = Theme.TexteGris
        lblStatut.ForeColor = Theme.BleuFonce
        Theme.StyliserBoutonPrincipal(btnNouveau)
        For Each b In {btnOuvrir, btnSupprimer, btnFermer}
            Theme.StyliserBoutonSecondaire(b)
        Next
        Theme.StyliserGrille(dgvRapports)
    End Sub

    Private Sub CreerColonnes()
        Dim colonnes = {
            ("Numero", "N°", 6), ("DateVisite", "Visite le", 10), ("Praticien", "Praticien", 18), ("Ville", "Ville", 12),
            ("Remplacant", "Remplaçant vu", 14), ("Motif", "Motif", 18), ("Etat", "État", 9), ("Saisie", "Saisi le", 13)}
        For Each c In colonnes
            dgvRapports.Columns.Add(New DataGridViewTextBoxColumn() With {
                .Name = c.Item1, .HeaderText = c.Item2, .FillWeight = c.Item3, .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
    End Sub

    Private Async Sub FrmMesComptesRendus_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await Recharger(Nothing)
    End Sub

    ''' <summary>Relit la liste depuis la base et resélectionne éventuellement un rapport.</summary>
    Private Async Function Recharger(numeroASelectionner As Integer?) As Task
        UseWaitCursor = True
        Try
            _rapports = Await Task.Run(Function() _service.MesRapports(_utilisateur))
            If IsDisposed Then Return
            Afficher(numeroASelectionner)
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            UseWaitCursor = False
        End Try
    End Function

    Private Sub Afficher(Optional numeroASelectionner As Integer? = Nothing)
        Dim recherche = txtRecherche.Text.Trim()
        Dim visibles = _rapports.Where(Function(r)
                                           Select Case cboEtat.SelectedIndex
                                               Case FiltreBrouillons : If r.Etat <> EtatRapport.Brouillon Then Return False
                                               Case FiltreValides : If r.Etat <> EtatRapport.Valide Then Return False
                                           End Select
                                           Return recherche.Length = 0 OrElse
                                                  r.Praticien.Contains(recherche, StringComparison.CurrentCultureIgnoreCase) OrElse
                                                  If(r.Ville, "").Contains(recherche, StringComparison.CurrentCultureIgnoreCase)
                                       End Function).ToList()

        dgvRapports.Rows.Clear()
        For Each r In visibles
            Dim i = dgvRapports.Rows.Add(r.Numero, r.DateVisite.ToString("dd/MM/yyyy"), r.Praticien, r.Ville,
                                         r.Remplacant, r.Motif, If(r.Etat = EtatRapport.Valide, "Validé", "Brouillon"),
                                         r.DateSaisie.ToString("dd/MM/yyyy HH:mm"))
            dgvRapports.Rows(i).Tag = r
            If r.Etat = EtatRapport.Brouillon Then
                dgvRapports.Rows(i).Cells("Etat").Style.ForeColor = Theme.Erreur
                dgvRapports.Rows(i).Cells("Etat").Style.Font = New Font(dgvRapports.Font, FontStyle.Bold)
            End If
            If numeroASelectionner.HasValue AndAlso r.Numero = numeroASelectionner.Value Then
                dgvRapports.Rows(i).Selected = True
                dgvRapports.FirstDisplayedScrollingRowIndex = i
            End If
        Next

        Dim nbBrouillons = _rapports.Where(Function(r) r.Etat = EtatRapport.Brouillon).Count()
        lblNombre.Text = $"{visibles.Count} compte(s)-rendu(s)" & If(nbBrouillons > 0, $" · {nbBrouillons} brouillon(s) à terminer", "")
        MettreAJourBoutons()
    End Sub

    Private ReadOnly Property Selection As RapportResume
        Get
            Return If(dgvRapports.SelectedRows.Count = 0, Nothing, TryCast(dgvRapports.SelectedRows(0).Tag, RapportResume))
        End Get
    End Property

    Private Sub MettreAJourBoutons()
        btnOuvrir.Enabled = Selection IsNot Nothing
        btnSupprimer.Enabled = Selection IsNot Nothing AndAlso Selection.Etat = EtatRapport.Brouillon
    End Sub

    Private Sub Filtre_Change(sender As Object, e As EventArgs) Handles cboEtat.SelectedIndexChanged, txtRecherche.TextChanged
        Afficher()
    End Sub

    Private Sub dgvRapports_SelectionChanged(sender As Object, e As EventArgs) Handles dgvRapports.SelectionChanged
        MettreAJourBoutons()
    End Sub

    Private Sub dgvRapports_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRapports.CellDoubleClick
        If e.RowIndex >= 0 Then OuvrirRapport(Selection?.Numero)
    End Sub

    Private Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        OuvrirRapport(Nothing)
    End Sub

    Private Sub btnOuvrir_Click(sender As Object, e As EventArgs) Handles btnOuvrir.Click
        If Selection IsNot Nothing Then OuvrirRapport(Selection.Numero)
    End Sub

    Private Async Sub OuvrirRapport(numero As Integer?)
        Using frm As New FrmCompteRendu(_service, _utilisateur, numero)
            If frm.ShowDialog(Me) <> DialogResult.OK Then Return
            lblStatut.Text = $"Compte-rendu n° {frm.NumeroEnregistre} enregistré " &
                             If(frm.EnregistreValide, "et validé.", "en brouillon.")
            Await Recharger(frm.NumeroEnregistre)
        End Using
    End Sub

    Private Async Sub btnSupprimer_Click(sender As Object, e As EventArgs) Handles btnSupprimer.Click
        Dim r = Selection
        If r Is Nothing OrElse r.Etat <> EtatRapport.Brouillon Then Return
        Dim reponse = MessageBox.Show(Me, $"Supprimer définitivement le brouillon du {r.DateVisite:dd/MM/yyyy} ({r.Praticien}) ?",
                                      "GSB", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        If reponse <> DialogResult.Yes Then Return
        Try
            Await Task.Run(Sub() _service.SupprimerBrouillon(_utilisateur, r.Numero))
            lblStatut.Text = $"Brouillon n° {r.Numero} supprimé."
            Await Recharger(Nothing)
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

End Class
