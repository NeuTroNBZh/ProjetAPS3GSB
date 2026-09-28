Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Rédaction d'un message (EX-50) : destinataires choisis dans l'annuaire (avec recherche) ou par groupe
''' (région, secteur), objet et texte. Renvoie DialogResult.OK une fois le message envoyé.
''' </summary>
Public Class FrmNouveauMessage

    ''' <summary>Élément affiché dans la liste des destinataires.</summary>
    Private NotInheritable Class ElementAnnuaire
        Public Sub New(membre As MembreEquipe)
            Me.Membre = membre
        End Sub

        Public ReadOnly Property Membre As MembreEquipe

        Public Overrides Function ToString() As String
            Dim rattachement = If(Membre.NomRegion, If(Membre.LibelleSecteur Is Nothing, "siège", $"secteur {Membre.LibelleSecteur}"))
            Return $"{Membre.Nom} {Membre.Prenom} — {LibellesModules.LibelleProfil(Membre.Profil).ToLower()}, {rattachement}"
        End Function
    End Class

    Private ReadOnly _service As ServiceMessagerie
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private ReadOnly _selection As New HashSet(Of String)
    Private _annuaire As IReadOnlyList(Of MembreEquipe) = Array.Empty(Of MembreEquipe)()
    Private _remplissage As Boolean

    Public Sub New(service As ServiceMessagerie, utilisateur As UtilisateurConnecte,
                   Optional destinataires As IEnumerable(Of String) = Nothing,
                   Optional objet As String = "", Optional contenu As String = "")
        InitializeComponent()
        _service = service
        _utilisateur = utilisateur
        If destinataires IsNot Nothing Then _selection.UnionWith(destinataires)
        txtObjet.Text = objet
        txtContenu.Text = contenu
        lblTitre.Text = If(String.IsNullOrEmpty(objet), "Nouveau message", "Répondre")

        BackColor = Theme.Blanc
        lblTitre.Font = Theme.PoliceTitre
        lblTitre.ForeColor = Theme.BleuGsb
        For Each l In {lblDestinataires, lblGroupe, lblObjet, lblContenu}
            l.ForeColor = Theme.BleuFonce
        Next
        lblNbDestinataires.ForeColor = Theme.TexteGris
        lblErreurs.ForeColor = Theme.Erreur
        Theme.StyliserBoutonPrincipal(btnEnvoyer)
        Theme.StyliserBoutonSecondaire(btnAnnuler)
        Theme.StyliserBoutonSecondaire(btnAjouterGroupe)
        Theme.StyliserBoutonSecondaire(btnToutDecocher)
    End Sub

    Private Async Sub FrmNouveauMessage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            _annuaire = Await Task.Run(Function() _service.Annuaire(_utilisateur))
            If IsDisposed Then Return
            cboGroupe.Items.AddRange(ServiceMessagerie.Groupes(_annuaire, _utilisateur).ToArray())
            If cboGroupe.Items.Count > 0 Then cboGroupe.SelectedIndex = 0
            AfficherAnnuaire()
            If _selection.Count > 0 Then txtContenu.Focus() : txtContenu.Select(0, 0) Else txtFiltre.Focus()
        Catch ex As ErreurMetierException
            lblErreurs.Text = ex.Message
            btnEnvoyer.Enabled = False
        End Try
    End Sub

    ''' <summary>Affiche l'annuaire filtré en conservant les cases cochées (même masquées par le filtre).</summary>
    Private Sub AfficherAnnuaire()
        Dim filtre = txtFiltre.Text.Trim()
        _remplissage = True
        clbDestinataires.BeginUpdate()
        clbDestinataires.Items.Clear()
        For Each m In _annuaire
            Dim element As New ElementAnnuaire(m)
            If filtre.Length > 0 AndAlso Not element.ToString().Contains(filtre, StringComparison.CurrentCultureIgnoreCase) Then Continue For
            clbDestinataires.Items.Add(element, _selection.Contains(m.Matricule))
        Next
        clbDestinataires.EndUpdate()
        _remplissage = False
        MettreAJourCompteur()
    End Sub

    Private Sub MettreAJourCompteur()
        lblNbDestinataires.Text = If(_selection.Count = 0, "aucun destinataire", $"{_selection.Count} destinataire(s)")
    End Sub

    Private Sub txtFiltre_TextChanged(sender As Object, e As EventArgs) Handles txtFiltre.TextChanged
        AfficherAnnuaire()
    End Sub

    Private Sub clbDestinataires_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbDestinataires.ItemCheck
        If _remplissage Then Return
        Dim matricule = DirectCast(clbDestinataires.Items(e.Index), ElementAnnuaire).Membre.Matricule
        If e.NewValue = CheckState.Checked Then _selection.Add(matricule) Else _selection.Remove(matricule)
        lblNbDestinataires.Text = If(_selection.Count = 0, "aucun destinataire", $"{_selection.Count} destinataire(s)")
    End Sub

    Private Sub btnAjouterGroupe_Click(sender As Object, e As EventArgs) Handles btnAjouterGroupe.Click
        Dim groupe = TryCast(cboGroupe.SelectedItem, GroupeDestinataires)
        If groupe Is Nothing Then Return
        _selection.UnionWith(groupe.Matricules.Where(Function(m) m <> _utilisateur.Matricule))
        AfficherAnnuaire()
    End Sub

    Private Sub btnToutDecocher_Click(sender As Object, e As EventArgs) Handles btnToutDecocher.Click
        _selection.Clear()
        AfficherAnnuaire()
    End Sub

    Private Async Sub btnEnvoyer_Click(sender As Object, e As EventArgs) Handles btnEnvoyer.Click
        Dim objet = txtObjet.Text, contenu = txtContenu.Text, destinataires = _selection.ToList()
        btnEnvoyer.Enabled = False
        Try
            Dim erreurs = Await Task.Run(Function() _service.Envoyer(_utilisateur, objet, contenu, destinataires))
            If erreurs.Count > 0 Then
                lblErreurs.Text = Theme.EnPuces(erreurs)
                Return
            End If
            DialogResult = DialogResult.OK
            Close()
        Catch ex As ErreurMetierException
            lblErreurs.Text = ex.Message
        Finally
            btnEnvoyer.Enabled = True
        End Try
    End Sub

End Class
