Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Module Administration (EX-70 à EX-74), réservé à l'administrateur.
''' Ce fichier contient la partie commune et l'onglet Collaborateurs ; les autres onglets sont dans
''' FrmAdministration.Portefeuilles.vb, FrmAdministration.Referentiels.vb et FrmAdministration.Journal.vb.
''' </summary>
Partial Public Class FrmAdministration

    ''' <summary>Profil proposé dans les listes (libellé lisible).</summary>
    Private NotInheritable Class ChoixProfil
        Public Sub New(profil As Profil)
            Me.Profil = profil
        End Sub
        Public ReadOnly Property Profil As Profil
        Public Overrides Function ToString() As String
            Return LibellesModules.LibelleProfil(Profil)
        End Function
    End Class

    Private ReadOnly _service As ServiceAdministration
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private _fiches As IReadOnlyList(Of FicheCollaborateur) = Array.Empty(Of FicheCollaborateur)()
    Private _regions As IReadOnlyList(Of Region) = Array.Empty(Of Region)()
    Private _secteurs As IReadOnlyList(Of Secteur) = Array.Empty(Of Secteur)()

    ' Barre de l'onglet Collaborateurs
    Private btnModifier, btnAffectation, btnMotDePasse, btnVerrou, btnDepart As Button
    Private txtFiltreCollaborateurs As TextBox
    Private chkAnciens As CheckBox

    Public Sub New(service As ServiceAdministration, utilisateur As UtilisateurConnecte)
        InitializeComponent()
        _service = service
        _utilisateur = utilisateur

        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        Theme.StyliserBoutonSecondaire(btnFermer)
        For Each t In {tabCollaborateurs, tabPortefeuilles, tabReferentiels, tabJournal, tabRefPraticiens, tabRefMedicaments, tabRefMotifs}
            t.BackColor = Theme.Blanc
        Next
        For Each l In {lblPortefeuille, lblSansVisiteur}
            Theme.StyliserSection(l)
        Next

        PreparerCollaborateurs()
        PreparerPortefeuilles()
        PreparerReferentiels()
        PreparerJournal()
    End Sub

    Private Async Sub FrmAdministration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            _regions = Await Task.Run(Function() _service.Regions(_utilisateur))
            _secteurs = Await Task.Run(Function() _service.Secteurs(_utilisateur))
            Await ChargerCollaborateurs()
            Await ChargerSansVisiteur()
            Await ChargerPraticiens()
            Await ChargerMedicaments()
            Await ChargerMotifs()
            Await ChargerJournal()
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    ' ------------------------------------------------------------------
    ' Outils communs
    ' ------------------------------------------------------------------

    ''' <summary>Mise à l'échelle de l'écran des dimensions des contrôles créés par code.</summary>
    Private Function Echelle(pixels As Integer) As Integer
        Return CInt(pixels * DeviceDpi / 96.0)
    End Function

    Private Shared Function AjouterBouton(barre As FlowLayoutPanel, texte As String, action As EventHandler,
                                          Optional principal As Boolean = False) As Button
        Dim b As New Button() With {.Text = texte, .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowOnly,
                                    .MinimumSize = New Size(0, 36), .Padding = New Padding(10, 0, 10, 0), .Margin = New Padding(0, 0, 8, 6)}
        If principal Then Theme.StyliserBoutonPrincipal(b) Else Theme.StyliserBoutonSecondaire(b)
        AddHandler b.Click, action
        barre.Controls.Add(b)
        Return b
    End Function

    Private Shared Function AjouterEtiquette(barre As FlowLayoutPanel, texte As String) As Label
        Dim l As New Label() With {.Text = texte, .AutoSize = True, .ForeColor = Theme.BleuFonce, .Margin = New Padding(8, 9, 4, 0)}
        barre.Controls.Add(l)
        Return l
    End Function

    Private Shared Sub PreparerGrille(grille As DataGridView, colonnes As IEnumerable(Of (Titre As String, Poids As Integer)),
                                      Optional selectionMultiple As Boolean = False)
        grille.ReadOnly = True
        grille.AllowUserToAddRows = False
        grille.AllowUserToDeleteRows = False
        grille.AllowUserToResizeRows = False
        grille.RowHeadersVisible = False
        grille.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grille.MultiSelect = selectionMultiple
        grille.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Theme.StyliserGrille(grille)
        For Each c In colonnes
            grille.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = c.Titre, .FillWeight = c.Poids,
                                                                     .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
    End Sub

    Private Shared Function Selection(Of T As Class)(grille As DataGridView) As T
        Return If(grille.SelectedRows.Count = 0, Nothing, TryCast(grille.SelectedRows(0).Tag, T))
    End Function

    ''' <summary>
    ''' Exécute une opération du service en arrière-plan, affiche le résultat (message ou erreurs),
    ''' communique le mot de passe provisoire éventuel, puis rafraîchit l'affichage.
    ''' </summary>
    Private Async Function Operer(operation As Func(Of ResultatOperation), rafraichir As Func(Of Task)) As Task
        UseWaitCursor = True
        Try
            Dim r = Await Task.Run(operation)
            If IsDisposed Then Return
            Afficher(r)
            If r.Reussi AndAlso rafraichir IsNot Nothing Then Await rafraichir()
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            UseWaitCursor = False
        End Try
    End Function

    ''' <summary>Affiche le résultat d'une opération (formulaire ou action directe).</summary>
    Private Sub Afficher(r As ResultatOperation)
        If Not r.Reussi Then
            MessageBox.Show(Me, String.Join(Environment.NewLine, r.Erreurs), "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        lblStatut.ForeColor = Color.FromArgb(46, 125, 50)
        lblStatut.Text = r.Message
        If r.MotDePasseProvisoire IsNot Nothing Then CommuniquerMotDePasse(r.MotDePasseProvisoire)
    End Sub

    ''' <summary>Le mot de passe provisoire n'est montré qu'une fois (il n'est stocké que haché).</summary>
    Private Sub CommuniquerMotDePasse(motDePasse As String)
        Dim copie = False
        Try
            Clipboard.SetText(motDePasse)
            copie = True
        Catch ex As Runtime.InteropServices.ExternalException
            ' Presse-papiers indisponible : le mot de passe reste affiché ci-dessous
        End Try
        MessageBox.Show(Me,
            $"Mot de passe provisoire : {motDePasse}{Environment.NewLine}{Environment.NewLine}" &
            If(copie, "Il a été copié dans le presse-papiers. ", "") &
            "Notez-le maintenant : il ne sera plus affiché. Le collaborateur devra le changer à sa première connexion.",
            "GSB - Mot de passe provisoire", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>Ouvre un formulaire ; après validation réussie, affiche le résultat et rafraîchit.</summary>
    Private Async Function OuvrirFormulaire(frm As FrmFormulaire, rafraichir As Func(Of Task)) As Task
        Using frm
            If frm.ShowDialog(Me) <> DialogResult.OK Then Return
            Afficher(frm.Resultat)
        End Using
        If rafraichir IsNot Nothing Then Await rafraichir()
    End Function

    ''' <summary>Champs profil / région / secteur, la région ou le secteur n'apparaissant que si nécessaire.</summary>
    Private Sub AjouterChampsAffectation(frm As FrmFormulaire, Optional actuelle As Affectation = Nothing)
        Dim profils = {Profil.Visiteur, Profil.Delegue, Profil.Responsable, Profil.Administrateur}.Select(Function(p) New ChoixProfil(p)).ToList()
        Dim cboProfil = frm.AjouterListe("profil", "Profil", profils,
                                         If(actuelle Is Nothing, profils(0), profils.First(Function(p) p.Profil = actuelle.Profil)))
        frm.AjouterListe("region", "Région", _regions, _regions.FirstOrDefault(Function(r) r.Code = actuelle?.CodeRegion))
        frm.AjouterListe("secteur", "Secteur", _secteurs, _secteurs.FirstOrDefault(Function(s) s.Code = actuelle?.CodeSecteur))
        Dim ajuster = Sub()
                          Dim p = DirectCast(cboProfil.SelectedItem, ChoixProfil).Profil
                          frm.Afficher("region", p = Profil.Visiteur OrElse p = Profil.Delegue)
                          frm.Afficher("secteur", p = Profil.Responsable)
                      End Sub
        AddHandler cboProfil.SelectedIndexChanged, Sub(s, e) ajuster()
        ajuster()
    End Sub

    Private Shared Function LireAffectation(frm As FrmFormulaire) As Affectation
        Dim p = frm.Selection(Of ChoixProfil)("profil").Profil
        Return New Affectation() With {
            .Profil = p,
            .CodeRegion = If(p = Profil.Visiteur OrElse p = Profil.Delegue, frm.Selection(Of Region)("region")?.Code, Nothing),
            .CodeSecteur = If(p = Profil.Responsable, frm.Selection(Of Secteur)("secteur")?.Code, Nothing)}
    End Function

    ' ------------------------------------------------------------------
    ' Onglet Collaborateurs (EX-70, EX-71)
    ' ------------------------------------------------------------------

    Private Sub PreparerCollaborateurs()
        AjouterBouton(flpBarreCollaborateurs, "Nouveau collaborateur", AddressOf NouveauCollaborateur, principal:=True)
        btnModifier = AjouterBouton(flpBarreCollaborateurs, "Modifier", AddressOf ModifierCollaborateur)
        btnAffectation = AjouterBouton(flpBarreCollaborateurs, "Changer d'affectation", AddressOf ChangerAffectation)
        btnMotDePasse = AjouterBouton(flpBarreCollaborateurs, "Réinitialiser le mot de passe", AddressOf ReinitialiserMotDePasse)
        btnVerrou = AjouterBouton(flpBarreCollaborateurs, "Verrouiller", AddressOf BasculerVerrou)
        btnDepart = AjouterBouton(flpBarreCollaborateurs, "Enregistrer le départ", AddressOf EnregistrerDepart)
        flpBarreCollaborateurs.SetFlowBreak(btnDepart, True)
        AjouterEtiquette(flpBarreCollaborateurs, "Rechercher")
        txtFiltreCollaborateurs = New TextBox() With {.Width = Echelle(260), .PlaceholderText = "Nom, login, région…", .Margin = New Padding(0, 4, 16, 0)}
        AddHandler txtFiltreCollaborateurs.TextChanged, Sub(s, e) AfficherCollaborateurs()
        flpBarreCollaborateurs.Controls.Add(txtFiltreCollaborateurs)
        chkAnciens = New CheckBox() With {.Text = "Afficher les collaborateurs partis", .AutoSize = True, .Margin = New Padding(0, 6, 0, 0)}
        AddHandler chkAnciens.CheckedChanged, Sub(s, e) AfficherCollaborateurs()
        flpBarreCollaborateurs.Controls.Add(chkAnciens)

        PreparerGrille(dgvCollaborateurs, {("Collaborateur", 14), ("Login", 11), ("Profil", 19), ("Rattachement", 13),
                                           ("Compte", 19), ("Connexion", 15), ("Ptf.", 5)})
        AddHandler dgvCollaborateurs.SelectionChanged, AddressOf CollaborateurSelectionne
        pnlFicheCollaborateur.AfficherMessage("Sélectionnez un collaborateur.")
        MettreAJourBoutonsCollaborateur()
    End Sub

    Private Async Function ChargerCollaborateurs() As Task
        _fiches = Await Task.Run(Function() _service.Collaborateurs(_utilisateur))
        If IsDisposed Then Return
        AfficherCollaborateurs()
        RemplirVisiteurs()
    End Function

    Private Shared Function EtatCompte(f As FicheCollaborateur) As String
        Dim c = f.Collaborateur
        If c.DateDepart.HasValue Then Return $"Parti le {c.DateDepart:dd/MM/yyyy}"
        If c.Verrouille Then Return "Verrouillé"
        If c.MotDePasseAChanger Then Return "Mot de passe à changer"
        Return "Actif"
    End Function

    Private Shared Function Rattachement(a As Affectation) As String
        If a Is Nothing Then Return ""
        If a.NomRegion IsNot Nothing Then Return a.NomRegion
        If a.LibelleSecteur IsNot Nothing Then Return $"Secteur {a.LibelleSecteur}"
        Return "Siège"
    End Function

    Private Sub AfficherCollaborateurs()
        Dim filtre = txtFiltreCollaborateurs.Text.Trim()
        Dim matriculeChoisi = Selection(Of FicheCollaborateur)(dgvCollaborateurs)?.Collaborateur.Matricule
        dgvCollaborateurs.Rows.Clear()
        For Each f In _fiches
            Dim c = f.Collaborateur
            If c.DateDepart.HasValue AndAlso Not chkAnciens.Checked Then Continue For
            Dim profil = If(f.AffectationEnCours Is Nothing, "—", LibellesModules.LibelleProfil(f.AffectationEnCours.Profil))
            Dim valeurs As Object() = {$"{c.Nom} {c.Prenom}", c.Login, profil, Rattachement(f.AffectationEnCours), EtatCompte(f),
                                       c.DerniereConnexion?.ToString("dd/MM/yyyy HH:mm"), f.TaillePortefeuille}
            If filtre.Length > 0 AndAlso Not valeurs.Any(Function(v) v IsNot Nothing AndAlso v.ToString().Contains(filtre, StringComparison.CurrentCultureIgnoreCase)) Then Continue For
            Dim ligne = dgvCollaborateurs.Rows(dgvCollaborateurs.Rows.Add(valeurs))
            ligne.Tag = f
            If c.DateDepart.HasValue Then ligne.DefaultCellStyle.ForeColor = Theme.TexteGris
            If c.Verrouille AndAlso Not c.DateDepart.HasValue Then
                ligne.Cells(4).Style.ForeColor = Theme.Erreur
                ligne.Cells(4).Style.SelectionForeColor = Theme.Erreur
                ligne.Cells(4).Style.Font = New Font(dgvCollaborateurs.Font, FontStyle.Bold)
            End If
            If c.Matricule = matriculeChoisi Then ligne.Selected = True
        Next
        tabCollaborateurs.Text = $"Collaborateurs ({_fiches.Where(Function(f) Not f.Collaborateur.DateDepart.HasValue).Count()})"
        MettreAJourBoutonsCollaborateur()
    End Sub

    Private Sub MettreAJourBoutonsCollaborateur()
        Dim f = Selection(Of FicheCollaborateur)(dgvCollaborateurs)
        Dim present = f IsNot Nothing AndAlso Not f.Collaborateur.DateDepart.HasValue
        btnModifier.Enabled = f IsNot Nothing
        btnAffectation.Enabled = present
        btnMotDePasse.Enabled = present
        btnVerrou.Enabled = present
        btnDepart.Enabled = present
        btnVerrou.Text = If(f IsNot Nothing AndAlso f.Collaborateur.Verrouille, "Déverrouiller", "Verrouiller")
    End Sub

    Private Async Sub CollaborateurSelectionne(sender As Object, e As EventArgs)
        MettreAJourBoutonsCollaborateur()
        Dim f = Selection(Of FicheCollaborateur)(dgvCollaborateurs)
        If f Is Nothing Then Return
        Try
            Dim historique = Await Task.Run(Function() _service.Affectations(_utilisateur, f.Collaborateur.Matricule))
            If IsDisposed OrElse Selection(Of FicheCollaborateur)(dgvCollaborateurs) IsNot f Then Return
            AfficherFicheCollaborateur(f, historique)
        Catch ex As ErreurMetierException
            pnlFicheCollaborateur.AfficherMessage(ex.Message)
        End Try
    End Sub

    Private Sub AfficherFicheCollaborateur(f As FicheCollaborateur, historique As IReadOnlyList(Of Affectation))
        Dim c = f.Collaborateur
        Dim p = pnlFicheCollaborateur
        p.SuspendLayout()
        p.Vider()
        p.AjouterTitre($"{c.Prenom} {c.Nom}")
        p.AjouterSousTitre($"Matricule {c.Matricule} · login {c.Login}")
        Dim etat = EtatCompte(f)
        If etat = "Actif" Then
            p.AjouterBadge(etat, Color.FromArgb(232, 245, 233), Color.FromArgb(46, 125, 50))
        ElseIf c.Verrouille OrElse c.DateDepart.HasValue Then
            p.AjouterBadge(etat, Color.FromArgb(253, 236, 234), Theme.Erreur)
        Else
            p.AjouterBadge(etat, Color.FromArgb(255, 244, 219), Color.FromArgb(138, 90, 0))
        End If

        p.AjouterSection("Coordonnées")
        p.AjouterTexte(String.Join(Environment.NewLine, {c.Adresse, String.Join(" ", {c.CodePostal, c.Ville}.Where(Function(x) x IsNot Nothing))}.
                                   Where(Function(x) Not String.IsNullOrWhiteSpace(x))))
        p.AjouterInformation("Téléphone", c.Telephone)
        p.AjouterInformation("E-mail", c.Email)
        p.AjouterInformation("Embauché le", c.DateEmbauche.ToString("dd/MM/yyyy"))

        p.AjouterSection("Compte")
        p.AjouterInformation("Dernière connexion", If(c.DerniereConnexion?.ToString("dd/MM/yyyy à HH:mm"), "jamais"))
        p.AjouterInformation("Échecs de connexion consécutifs", c.NbEchecsConnexion.ToString(CultureInfo.CurrentCulture))
        p.AjouterInformation("Praticiens en portefeuille", f.TaillePortefeuille.ToString(CultureInfo.CurrentCulture))

        p.AjouterSection($"Affectations ({historique.Count})")
        p.AjouterTableau({("Profil", 30.0F), ("Rattachement", 30.0F), ("Du", 20.0F), ("Au", 20.0F)},
            historique.Select(Function(a) New Object() {LibellesModules.LibelleProfil(a.Profil), Rattachement(a),
                                                         a.DateDebut.ToString("dd/MM/yyyy"), If(a.DateFin?.ToString("dd/MM/yyyy"), "en cours")}))
        p.ResumeLayout()
    End Sub

    Private Async Sub NouveauCollaborateur(sender As Object, e As EventArgs)
        Dim frm As New FrmFormulaire("Nouveau collaborateur", "Un mot de passe provisoire sera généré ; il devra être changé à la première connexion.", "Créer le compte")
        frm.AjouterTexte("matricule", "Matricule *", "", 10)
        frm.AjouterTexte("nom", "Nom *", "", 50)
        frm.AjouterTexte("prenom", "Prénom *", "", 50)
        frm.AjouterTexte("login", "Login *", "", 30)
        frm.AjouterTexte("adresse", "Adresse", "", 100)
        frm.AjouterTexte("cp", "Code postal", "", 5)
        frm.AjouterTexte("ville", "Ville", "", 50)
        frm.AjouterTexte("telephone", "Téléphone", "", 20)
        frm.AjouterTexte("email", "E-mail", "", 100)
        frm.AjouterDate("embauche", "Date d'embauche *", _service.Aujourdhui)
        AjouterChampsAffectation(frm)
        frm.Validation = Function() Task.Run(Function() _service.CreerCollaborateur(_utilisateur, LireIdentite(frm, New Collaborateur() With {
                                                                                        .Matricule = frm.Texte("matricule"), .DateEmbauche = frm.DateChoisie("embauche")}),
                                                                                    LireAffectation(frm)))
        Await OuvrirFormulaire(frm, AddressOf ChargerCollaborateurs)
    End Sub

    Private Shared Function LireIdentite(frm As FrmFormulaire, c As Collaborateur) As Collaborateur
        c.Nom = frm.Texte("nom")
        c.Prenom = frm.Texte("prenom")
        c.Login = frm.Texte("login")
        c.Adresse = frm.Texte("adresse")
        c.CodePostal = frm.Texte("cp")
        c.Ville = frm.Texte("ville")
        c.Telephone = frm.Texte("telephone")
        c.Email = frm.Texte("email")
        Return c
    End Function

    Private Async Sub ModifierCollaborateur(sender As Object, e As EventArgs)
        Dim f = Selection(Of FicheCollaborateur)(dgvCollaborateurs)
        If f Is Nothing Then Return
        Dim c = f.Collaborateur
        Dim frm As New FrmFormulaire($"Modifier {c.Prenom} {c.Nom}", "Identité, coordonnées et login. L'affectation se change avec le bouton dédié.")
        frm.AjouterTexte("matricule", "Matricule", c.Matricule, 10, lectureSeule:=True)
        frm.AjouterTexte("nom", "Nom *", c.Nom, 50)
        frm.AjouterTexte("prenom", "Prénom *", c.Prenom, 50)
        frm.AjouterTexte("login", "Login *", c.Login, 30)
        frm.AjouterTexte("adresse", "Adresse", c.Adresse, 100)
        frm.AjouterTexte("cp", "Code postal", c.CodePostal, 5)
        frm.AjouterTexte("ville", "Ville", c.Ville, 50)
        frm.AjouterTexte("telephone", "Téléphone", c.Telephone, 20)
        frm.AjouterTexte("email", "E-mail", c.Email, 100)
        frm.Validation = Function() Task.Run(Function() _service.ModifierCollaborateur(_utilisateur,
                                                 LireIdentite(frm, New Collaborateur() With {.Matricule = c.Matricule, .DateEmbauche = c.DateEmbauche})))
        Await OuvrirFormulaire(frm, AddressOf ChargerCollaborateurs)
    End Sub

    Private Async Sub ChangerAffectation(sender As Object, e As EventArgs)
        Dim f = Selection(Of FicheCollaborateur)(dgvCollaborateurs)
        If f Is Nothing OrElse f.AffectationEnCours Is Nothing Then Return
        Dim frm As New FrmFormulaire($"Affectation de {f.Collaborateur.Prenom} {f.Collaborateur.Nom}",
            $"Actuellement : {LibellesModules.LibelleProfil(f.AffectationEnCours.Profil)}, {Rattachement(f.AffectationEnCours)} depuis le {f.AffectationEnCours.DateDebut:dd/MM/yyyy}. L'historique est conservé.")
        AjouterChampsAffectation(frm, f.AffectationEnCours)
        frm.AjouterDate("effet", "À partir du", _service.Aujourdhui.AddDays(1))
        frm.AjouterNote("Changer de région ne transfère pas le portefeuille : utilisez ensuite l'onglet Portefeuilles si besoin.")
        frm.Validation = Function() Task.Run(Function() _service.ChangerAffectation(_utilisateur, f.Collaborateur.Matricule, LireAffectation(frm), frm.DateChoisie("effet")))
        Await OuvrirFormulaire(frm, AddressOf ChargerCollaborateurs)
    End Sub

    Private Async Sub ReinitialiserMotDePasse(sender As Object, e As EventArgs)
        Dim f = Selection(Of FicheCollaborateur)(dgvCollaborateurs)
        If f Is Nothing Then Return
        If MessageBox.Show(Me, $"Générer un nouveau mot de passe provisoire pour {f.Collaborateur.Prenom} {f.Collaborateur.Nom} ? L'ancien ne fonctionnera plus.",
                           "GSB", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        Await Operer(Function() _service.ReinitialiserMotDePasse(_utilisateur, f.Collaborateur.Matricule), AddressOf ChargerCollaborateurs)
    End Sub

    Private Async Sub BasculerVerrou(sender As Object, e As EventArgs)
        Dim f = Selection(Of FicheCollaborateur)(dgvCollaborateurs)
        If f Is Nothing Then Return
        Dim verrouiller = Not f.Collaborateur.Verrouille
        Await Operer(Function() _service.DefinirVerrouillage(_utilisateur, f.Collaborateur.Matricule, verrouiller), AddressOf ChargerCollaborateurs)
    End Sub

    Private Async Sub EnregistrerDepart(sender As Object, e As EventArgs)
        Dim f = Selection(Of FicheCollaborateur)(dgvCollaborateurs)
        If f Is Nothing Then Return
        Dim frm As New FrmFormulaire($"Départ de {f.Collaborateur.Prenom} {f.Collaborateur.Nom}",
                                     "Le compte ne permettra plus de se connecter. Ses comptes-rendus sont conservés.", "Enregistrer le départ")
        frm.AjouterDate("date", "Date de départ", _service.Aujourdhui)
        If f.TaillePortefeuille > 0 Then
            frm.AjouterNote($"Ses {f.TaillePortefeuille} praticien(s) deviendront « sans visiteur ». Vous pouvez d'abord transférer son portefeuille (onglet Portefeuilles).")
        End If
        frm.Validation = Function() Task.Run(Function() _service.EnregistrerDepart(_utilisateur, f.Collaborateur.Matricule, frm.DateChoisie("date")))
        Await OuvrirFormulaire(frm, Async Function()
                                        Await ChargerCollaborateurs()
                                        Await ChargerSansVisiteur()
                                    End Function)
    End Sub

End Class
