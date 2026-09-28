Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Consultation des praticiens (EX-21) : recherche, fiche détaillée, périodicité des visites
''' et historique (y compris les visites où le praticien était remplaçant).
''' </summary>
Public Class FrmPraticiens

    Private ReadOnly _service As ServiceConsultation
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private _numeroAffiche As Integer?
    ' Numéro de la dernière demande de fiche : ignore les réponses arrivées dans le désordre
    Private _demandeFiche As Integer
    ' Vrai pendant le remplissage de la liste : les changements de sélection sont alors ignorés
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
        Theme.StyliserGrille(dgvPraticiens)
        CreerColonnes()

        ' Le filtre « mon portefeuille » n'a de sens que pour ceux qui ont un portefeuille
        Dim aPortefeuille = ServiceConsultation.APortefeuille(utilisateur.Profil)
        chkMonPortefeuille.Visible = aPortefeuille
        chkMonPortefeuille.Checked = aPortefeuille
        pnlFiche.AfficherMessage("Sélectionnez un praticien pour afficher sa fiche.")
    End Sub

    Private Sub CreerColonnes()
        For Each c In {("Nom", "Praticien", 26), ("Type", "Type", 18), ("Ville", "Ville", 16),
                       ("Suivi", "Suivi par", 18), ("Derniere", "Dernière visite", 12), ("Periodicite", "Périodicité", 14)}
            dgvPraticiens.Columns.Add(New DataGridViewTextBoxColumn() With {
                .Name = c.Item1, .HeaderText = c.Item2, .FillWeight = c.Item3, .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
    End Sub

    Private Async Sub FrmPraticiens_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await Rechercher()
    End Sub

    ' ------------------------------------------------------------------
    ' Liste
    ' ------------------------------------------------------------------

    Private Sub Filtre_Change(sender As Object, e As EventArgs) Handles txtRecherche.TextChanged, chkMonPortefeuille.CheckedChanged, chkInactifs.CheckedChanged
        If Not IsHandleCreated Then Return   ' réglage initial des filtres dans le constructeur
        ' Recherche différée : on attend que l'utilisateur ait fini de taper
        tmrRecherche.Stop()
        tmrRecherche.Start()
    End Sub

    Private Async Sub tmrRecherche_Tick(sender As Object, e As EventArgs) Handles tmrRecherche.Tick
        tmrRecherche.Stop()
        Await Rechercher()
    End Sub

    Private Async Function Rechercher() As Task
        Dim texte = txtRecherche.Text, portefeuille = chkMonPortefeuille.Checked, inactifs = chkInactifs.Checked
        UseWaitCursor = True
        Try
            Dim liste = Await Task.Run(Function() _service.RechercherPraticiens(_utilisateur, texte, portefeuille, inactifs))
            If IsDisposed Then Return   ' fenêtre fermée pendant la recherche
            Afficher(liste)
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            UseWaitCursor = False
        End Try
    End Function

    Private Sub Afficher(liste As IReadOnlyList(Of PraticienResume))
        Dim aReselectionner = _numeroAffiche
        _remplissage = True
        dgvPraticiens.Rows.Clear()
        For Each p In liste
            Dim etat = _service.Periodicite(p.DateDerniereVisite)
            Dim i = dgvPraticiens.Rows.Add(p.NomComplet, p.LibelleType, p.Ville, p.NomVisiteur,
                                           p.DateDerniereVisite?.ToString("dd/MM/yyyy"), AffichagePeriodicite.Libelle(etat))
            Dim ligne = dgvPraticiens.Rows(i)
            ligne.Tag = p
            Dim couleurs = AffichagePeriodicite.Couleurs(etat)
            ligne.Cells("Periodicite").Style.ForeColor = couleurs.Encre
            ligne.Cells("Periodicite").Style.Font = New Font(dgvPraticiens.Font, FontStyle.Bold)
            If Not p.Actif Then ligne.DefaultCellStyle.ForeColor = Theme.TexteGris
            If aReselectionner.HasValue AndAlso p.Numero = aReselectionner.Value Then ligne.Selected = True
        Next
        If dgvPraticiens.SelectedRows.Count = 0 AndAlso dgvPraticiens.Rows.Count > 0 Then dgvPraticiens.Rows(0).Selected = True
        _remplissage = False
        ChargerFicheSelection()

        lblNombre.Text = If(liste.Count >= ServiceConsultation.MaxResultats,
                            $"{liste.Count} premiers praticiens (affinez la recherche)",
                            $"{liste.Count} praticien(s)")
        If liste.Count = 0 Then pnlFiche.AfficherMessage("Aucun praticien ne correspond à la recherche.")
    End Sub

    ' ------------------------------------------------------------------
    ' Fiche
    ' ------------------------------------------------------------------

    Private Sub dgvPraticiens_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPraticiens.SelectionChanged
        If Not _remplissage Then ChargerFicheSelection()
    End Sub

    Private Async Sub ChargerFicheSelection()
        If dgvPraticiens.SelectedRows.Count = 0 Then Return
        Dim p = TryCast(dgvPraticiens.SelectedRows(0).Tag, PraticienResume)
        If p Is Nothing OrElse Nullable.Equals(p.Numero, _numeroAffiche) Then Return

        _demandeFiche += 1
        Dim demande = _demandeFiche
        Try
            Dim fiche = Await Task.Run(Function() _service.FichePraticien(_utilisateur, p.Numero))
            ' Fenêtre fermée, ou autre ligne sélectionnée entre-temps : réponse ignorée
            If IsDisposed OrElse demande <> _demandeFiche Then Return
            _numeroAffiche = p.Numero
            AfficherFiche(fiche)
        Catch ex As ErreurMetierException
            pnlFiche.AfficherMessage(ex.Message)
        End Try
    End Sub

    Private Sub AfficherFiche(fiche As FichePraticien)
        Dim p = fiche.Praticien
        Dim etat = _service.Periodicite(fiche.DateDerniereVisite)
        Dim couleurs = AffichagePeriodicite.Couleurs(etat)

        pnlFiche.SuspendLayout()
        pnlFiche.Vider()
        pnlFiche.AjouterTitre(p.NomComplet)
        pnlFiche.AjouterSousTitre(p.LibelleType & If(p.Actif, "", " · inactif"))
        pnlFiche.AjouterBadge(AffichagePeriodicite.Libelle(etat), couleurs.Fond, couleurs.Encre)

        pnlFiche.AjouterSection("Coordonnées")
        Dim adresse = String.Join(Environment.NewLine,
                                  {p.Adresse, String.Join(" ", {p.CodePostal, p.Ville}.Where(Function(x) Not String.IsNullOrEmpty(x)))}.
                                  Where(Function(x) Not String.IsNullOrWhiteSpace(x)))
        If adresse.Length = 0 Then
            pnlFiche.AjouterTexte("Pas de cabinet connu (remplaçant).", Theme.TexteGris, italique:=True)
        Else
            pnlFiche.AjouterTexte(adresse)
        End If
        pnlFiche.AjouterInformation("Téléphone", p.Telephone)
        pnlFiche.AjouterInformation("E-mail", p.Email)

        pnlFiche.AjouterSection("Profil")
        pnlFiche.AjouterInformation("Coefficient de notoriété", p.CoefNotoriete?.ToString("0.##", CultureInfo.CurrentCulture))
        If fiche.Specialites.Count = 0 Then
            pnlFiche.AjouterTexte("Aucune spécialité renseignée.", Theme.TexteGris, italique:=True)
        Else
            pnlFiche.AjouterTableau({("Spécialité", 34.0F), ("Diplôme", 44.0F), ("Coef. prescription", 22.0F)},
                fiche.Specialites.Select(Function(s) New Object() {s.Libelle, s.Diplome, s.CoefPrescription?.ToString("0.00")}))
        End If

        pnlFiche.AjouterSection("Suivi")
        pnlFiche.AjouterInformation("Suivi par", If(fiche.NomVisiteur, "aucun visiteur"))
        If fiche.DateDerniereVisite.HasValue Then
            Dim jours = (_service.Aujourdhui - fiche.DateDerniereVisite.Value.Date).Days
            pnlFiche.AjouterInformation("Dernière visite", $"{fiche.DateDerniereVisite:dd/MM/yyyy} (il y a {jours} jours)")
            pnlFiche.AjouterInformation("Prochaine visite prévue", fiche.DateProchainePrevue?.ToString("dd/MM/yyyy"))
            pnlFiche.AjouterInformation("Prochaine visite conseillée",
                                        $"à partir du {Periodicite.ProchaineVisiteConseillee(fiche.DateDerniereVisite):dd/MM/yyyy}")
        Else
            pnlFiche.AjouterTexte("Aucune visite validée en tant que titulaire.", Theme.TexteGris, italique:=True)
        End If

        pnlFiche.AjouterSection($"Historique des visites ({fiche.Visites.Count})")
        If fiche.Visites.Count = 0 Then
            pnlFiche.AjouterTexte("Aucune visite enregistrée.", Theme.TexteGris, italique:=True)
        Else
            pnlFiche.AjouterTableau({("Date", 19.0F), ("Visiteur", 23.0F), ("Motif", 26.0F), ("Conf.", 9.0F), ("Remarque", 23.0F)},
                fiche.Visites.Select(Function(v) New Object() {
                    v.DateVisite.ToString("dd/MM/yyyy"), v.Visiteur, v.Motif, v.CoefConfiance,
                    If(v.VuCommeRemplacant, $"en remplacement de {v.PraticienLie}",
                       If(v.PraticienLie Is Nothing, "", $"remplaçant vu : {v.PraticienLie}"))}),
                hauteurMax:=260)
        End If
        pnlFiche.ResumeLayout()
        pnlFiche.AutoScrollPosition = New Point(0, 0)
    End Sub

End Class
