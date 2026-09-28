Imports GSB.CR.Metier
Imports GSB.CR.Modeles

' Onglet Portefeuilles (EX-72) : un praticien n'est suivi que par un seul visiteur à la fois.
Partial Public Class FrmAdministration

    Private cboVisiteurPtf As ComboBox
    Private btnConfier, btnReattribuer, btnTransfererTout As Button

    ''' <summary>Visiteur ou délégué proposé dans les listes.</summary>
    Private NotInheritable Class ChoixVisiteur
        Public Sub New(fiche As FicheCollaborateur)
            Me.Fiche = fiche
        End Sub
        Public ReadOnly Property Fiche As FicheCollaborateur
        Public Overrides Function ToString() As String
            Dim f = Fiche
            Return $"{f.Collaborateur.Nom} {f.Collaborateur.Prenom} — {f.AffectationEnCours.NomRegion} ({f.TaillePortefeuille})"
        End Function
    End Class

    Private Sub PreparerPortefeuilles()
        AjouterEtiquette(flpBarrePortefeuilles, "Visiteur")
        cboVisiteurPtf = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Width = Echelle(340), .Margin = New Padding(0, 4, 16, 0)}
        AddHandler cboVisiteurPtf.SelectedIndexChanged, Async Sub(s, e) Await ChargerPortefeuille()
        flpBarrePortefeuilles.Controls.Add(cboVisiteurPtf)
        flpBarrePortefeuilles.SetFlowBreak(cboVisiteurPtf, True)
        btnConfier = AjouterBouton(flpBarrePortefeuilles, "Confier les praticiens sélectionnés à ce visiteur", AddressOf Confier, principal:=True)
        btnReattribuer = AjouterBouton(flpBarrePortefeuilles, "Réattribuer le praticien sélectionné…", AddressOf Reattribuer)
        btnTransfererTout = AjouterBouton(flpBarrePortefeuilles, "Transférer tout le portefeuille…", AddressOf TransfererTout)

        PreparerGrille(dgvPortefeuille, {("Praticien", 34), ("Ville", 22), ("Dernière visite", 20), ("Périodicité", 24)})
        PreparerGrille(dgvSansVisiteur, {("Praticien", 32), ("Type", 26), ("Ville", 22), ("Dernière visite", 20)}, selectionMultiple:=True)
        AddHandler dgvPortefeuille.SelectionChanged, Sub(s, e) MettreAJourBoutonsPortefeuille()
        AddHandler dgvSansVisiteur.SelectionChanged, Sub(s, e) MettreAJourBoutonsPortefeuille()
        ' À son premier affichage, la grille sélectionne d'office la première ligne : aucun praticien ne doit être pré-choisi
        AddHandler dgvSansVisiteur.VisibleChanged, Sub(s, e) If dgvSansVisiteur.Visible AndAlso Not dgvSansVisiteur.Focused Then dgvSansVisiteur.ClearSelection()
        MettreAJourBoutonsPortefeuille()
    End Sub

    Private ReadOnly Property VisiteurChoisi As FicheCollaborateur
        Get
            Return TryCast(cboVisiteurPtf.SelectedItem, ChoixVisiteur)?.Fiche
        End Get
    End Property

    ''' <summary>Visiteurs et délégués en poste (les seuls à pouvoir avoir un portefeuille).</summary>
    Private Function VisiteursEnPoste() As IEnumerable(Of FicheCollaborateur)
        Return _fiches.Where(Function(f) f.AffectationEnCours IsNot Nothing AndAlso
                                         (f.AffectationEnCours.Profil = Profil.Visiteur OrElse f.AffectationEnCours.Profil = Profil.Delegue))
    End Function

    Private Sub RemplirVisiteurs()
        Dim precedent = VisiteurChoisi?.Collaborateur.Matricule
        cboVisiteurPtf.Items.Clear()
        For Each f In VisiteursEnPoste()
            Dim choix As New ChoixVisiteur(f)
            cboVisiteurPtf.Items.Add(choix)
            If f.Collaborateur.Matricule = precedent Then cboVisiteurPtf.SelectedItem = choix
        Next
        If cboVisiteurPtf.SelectedIndex < 0 AndAlso cboVisiteurPtf.Items.Count > 0 Then cboVisiteurPtf.SelectedIndex = 0
    End Sub

    Private Async Function ChargerPortefeuille() As Task
        Dim v = VisiteurChoisi
        If v Is Nothing Then Return
        Try
            Dim liste = Await Task.Run(Function() _service.PortefeuilleDe(_utilisateur, v.Collaborateur.Matricule))
            If IsDisposed OrElse VisiteurChoisi IsNot v Then Return
            dgvPortefeuille.Rows.Clear()
            For Each p In liste
                Dim etat = Periodicite.Evaluer(p.DateDerniereVisite, _service.Aujourdhui)
                Dim i = dgvPortefeuille.Rows.Add(p.NomComplet, p.Ville, p.DateDerniereVisite?.ToString("dd/MM/yyyy"), AffichagePeriodicite.Libelle(etat))
                dgvPortefeuille.Rows(i).Tag = p
            Next
            lblPortefeuille.Text = $"Portefeuille de {v.Collaborateur.Prenom} {v.Collaborateur.Nom} ({liste.Count})"
            MettreAJourBoutonsPortefeuille()
        Catch ex As ErreurMetierException
            lblPortefeuille.Text = ex.Message
        End Try
    End Function

    Private Async Function ChargerSansVisiteur() As Task
        Dim liste = Await Task.Run(Function() _service.PraticiensSansVisiteur(_utilisateur))
        If IsDisposed Then Return
        dgvSansVisiteur.Rows.Clear()
        For Each p In liste
            Dim i = dgvSansVisiteur.Rows.Add(p.NomComplet, p.LibelleType, p.Ville, p.DateDerniereVisite?.ToString("dd/MM/yyyy"))
            dgvSansVisiteur.Rows(i).Tag = p
        Next
        dgvSansVisiteur.ClearSelection()
        lblSansVisiteur.Text = $"Praticiens sans visiteur ({liste.Count})"
        tabPortefeuilles.Text = If(liste.Count > 0, $"Portefeuilles ({liste.Count} sans visiteur)", "Portefeuilles")
        MettreAJourBoutonsPortefeuille()
    End Function

    Private Sub MettreAJourBoutonsPortefeuille()
        If btnConfier Is Nothing Then Return
        btnConfier.Enabled = VisiteurChoisi IsNot Nothing AndAlso dgvSansVisiteur.SelectedRows.Count > 0
        btnReattribuer.Enabled = dgvPortefeuille.SelectedRows.Count > 0
        btnTransfererTout.Enabled = dgvPortefeuille.Rows.Count > 0
    End Sub

    Private Async Function RafraichirPortefeuilles() As Task
        Await ChargerCollaborateurs()   ' met aussi à jour les tailles de portefeuille dans la liste des visiteurs
        Await ChargerPortefeuille()
        Await ChargerSansVisiteur()
    End Function

    Private Async Sub Confier(sender As Object, e As EventArgs)
        Dim v = VisiteurChoisi
        If v Is Nothing Then Return
        Dim numeros = dgvSansVisiteur.SelectedRows.Cast(Of DataGridViewRow)().Select(Function(r) DirectCast(r.Tag, PraticienResume).Numero).ToList()
        Await Operer(Function() _service.AttribuerPraticiens(_utilisateur, numeros, v.Collaborateur.Matricule), AddressOf RafraichirPortefeuilles)
    End Sub

    ''' <summary>Demande le visiteur destinataire (autre que le visiteur affiché).</summary>
    Private Function ChoisirDestinataire(titre As String, sousTitre As String, action As Func(Of FicheCollaborateur, ResultatOperation)) As FrmFormulaire
        Dim source = VisiteurChoisi
        Dim frm As New FrmFormulaire(titre, sousTitre, "Transférer")
        Dim autres = VisiteursEnPoste().Where(Function(f) f.Collaborateur.Matricule <> source.Collaborateur.Matricule).
                                       Select(Function(f) New ChoixVisiteur(f)).ToList()
        ' Même région proposée en premier
        Dim premier = autres.FirstOrDefault(Function(c) c.Fiche.AffectationEnCours.CodeRegion = source.AffectationEnCours.CodeRegion)
        frm.AjouterListe("vers", "Nouveau visiteur", autres, premier)
        frm.Validation = Function() Task.Run(Function() action(frm.Selection(Of ChoixVisiteur)("vers")?.Fiche))
        Return frm
    End Function

    Private Async Sub Reattribuer(sender As Object, e As EventArgs)
        Dim p = Selection(Of PraticienResume)(dgvPortefeuille)
        If p Is Nothing OrElse VisiteurChoisi Is Nothing Then Return
        Dim frm = ChoisirDestinataire($"Réattribuer {p.NomComplet}", "Le suivi par le visiteur actuel est clos ; l'historique des visites est conservé.",
                                      Function(cible) _service.AttribuerPraticiens(_utilisateur, {p.Numero}, cible?.Collaborateur.Matricule))
        Await OuvrirFormulaire(frm, AddressOf RafraichirPortefeuilles)
    End Sub

    Private Async Sub TransfererTout(sender As Object, e As EventArgs)
        Dim source = VisiteurChoisi
        If source Is Nothing Then Return
        Dim frm = ChoisirDestinataire($"Transférer le portefeuille de {source.Collaborateur.Prenom} {source.Collaborateur.Nom}",
                                      $"Ses {dgvPortefeuille.Rows.Count} praticien(s) seront suivis par le visiteur choisi à partir d'aujourd'hui.",
                                      Function(cible) _service.TransfererPortefeuille(_utilisateur, source.Collaborateur.Matricule, cible?.Collaborateur.Matricule))
        Await OuvrirFormulaire(frm, AddressOf RafraichirPortefeuilles)
    End Sub

End Class
