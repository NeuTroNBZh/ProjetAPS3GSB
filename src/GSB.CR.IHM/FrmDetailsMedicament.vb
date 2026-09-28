Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Composition, interactions et posologie d'un médicament (EX-73), ouvert depuis l'onglet Référentiels
''' de l'administration. Chaque ajout passe par un formulaire contrôlé par <see cref="ServiceAdministration"/>.
''' </summary>
Public Class FrmDetailsMedicament

    Private ReadOnly _service As ServiceAdministration
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private ReadOnly _medicament As Medicament
    Private btnRetirerComposant, btnRetirerInteraction, btnRetirerPosologie As Button

    Public Sub New(service As ServiceAdministration, utilisateur As UtilisateurConnecte, medicament As Medicament)
        InitializeComponent()
        _service = service
        _utilisateur = utilisateur
        _medicament = medicament

        lblTitre.Text = medicament.NomCommercial
        lblSousTitre.Text = $"Dépôt légal {medicament.DepotLegal} · {medicament.LibelleFamille} · composition, interactions et posologie"
        Text = $"GSB - {medicament.NomCommercial}"
        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        Theme.StyliserBoutonSecondaire(btnFermer)
        For Each t In {tabComposition, tabInteractions, tabPosologie}
            t.BackColor = Theme.Blanc
        Next

        AjouterBouton(flpBarreComposition, "Ajouter un composant", AddressOf AjouterComposant, principal:=True)
        btnRetirerComposant = AjouterBouton(flpBarreComposition, "Retirer", AddressOf RetirerComposant)
        AjouterBouton(flpBarreComposition, "Nouveau composant…", AddressOf NouveauComposant)
        PreparerGrille(dgvComposition, {("Composant", 60), ("Quantité", 20), ("Unité", 20)})

        AjouterBouton(flpBarreInteractions, "Ajouter une interaction", AddressOf AjouterInteraction, principal:=True)
        btnRetirerInteraction = AjouterBouton(flpBarreInteractions, "Retirer", AddressOf RetirerInteraction)
        PreparerGrille(dgvInteractions, {("Autre médicament", 22), ("Sens", 30), ("Description", 48)})

        AjouterBouton(flpBarrePosologie, "Ajouter une posologie", AddressOf AjouterPosologie, principal:=True)
        btnRetirerPosologie = AjouterBouton(flpBarrePosologie, "Retirer", AddressOf RetirerPosologie)
        AjouterBouton(flpBarrePosologie, "Nouveau dosage…", AddressOf NouveauDosage)
        PreparerGrille(dgvPosologie, {("Type d'individu", 18), ("Présentation", 22), ("Dosage", 12), ("Posologie", 48)})

        For Each g In {dgvComposition, dgvInteractions, dgvPosologie}
            AddHandler g.SelectionChanged, Sub(s, e) MettreAJourBoutons()
        Next
        MettreAJourBoutons()
    End Sub

    Private Async Sub FrmDetailsMedicament_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await Charger()
    End Sub

    Private Sub MettreAJourBoutons()
        If btnRetirerComposant Is Nothing Then Return
        btnRetirerComposant.Enabled = dgvComposition.SelectedRows.Count > 0
        btnRetirerInteraction.Enabled = dgvInteractions.SelectedRows.Count > 0
        btnRetirerPosologie.Enabled = dgvPosologie.SelectedRows.Count > 0
    End Sub

    ''' <summary>Recharge la fiche et remplit les trois grilles.</summary>
    Private Async Function Charger() As Task
        Try
            Dim fiche = Await Task.Run(Function() _service.FicheMedicament(_utilisateur, _medicament.DepotLegal))
            If IsDisposed Then Return
            Dim culture = CultureInfo.CurrentCulture

            dgvComposition.Rows.Clear()
            For Each c In fiche.Composition
                dgvComposition.Rows(dgvComposition.Rows.Add(c.Composant, c.Quantite.ToString("0.###", culture), c.Unite)).Tag = c
            Next
            dgvInteractions.Rows.Clear()
            For Each i In fiche.Interactions
                ' La colonne « Sens » se lit avec l'autre médicament pour sujet
                Dim sens = If(i.EstPerturbateur, $"voit son effet perturbé par {_medicament.NomCommercial}", $"perturbe l'effet de {_medicament.NomCommercial}")
                dgvInteractions.Rows(dgvInteractions.Rows.Add(i.NomAutre, sens, i.Description)).Tag = i
            Next
            dgvPosologie.Rows.Clear()
            For Each p In fiche.Posologies
                dgvPosologie.Rows(dgvPosologie.Rows.Add(p.TypeIndividu, p.Presentation, p.Dosage, p.Texte)).Tag = p
            Next

            tabComposition.Text = $"Composition ({fiche.Composition.Count})"
            tabInteractions.Text = $"Interactions ({fiche.Interactions.Count})"
            tabPosologie.Text = $"Posologie ({fiche.Posologies.Count})"
            MettreAJourBoutons()
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Function

    ' ------------------------------------------------------------------
    ' Opérations communes
    ' ------------------------------------------------------------------

    ''' <summary>Ouvre un formulaire ; après validation réussie, affiche le message et recharge la fiche.</summary>
    Private Async Function OuvrirFormulaire(frm As FrmFormulaire) As Task
        Using frm
            If frm.ShowDialog(Me) <> DialogResult.OK Then Return
            AfficherSucces(frm.Resultat.Message)
        End Using
        Await Charger()
    End Function

    Private Async Function Retirer(question As String, operation As Func(Of ResultatOperation)) As Task
        If MessageBox.Show(Me, question, "GSB", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        Try
            Dim r = Await Task.Run(operation)
            If IsDisposed Then Return
            If Not r.Reussi Then
                MessageBox.Show(Me, String.Join(Environment.NewLine, r.Erreurs), "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            AfficherSucces(r.Message)
            Await Charger()
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Function

    Private Sub AfficherSucces(message As String)
        lblStatut.ForeColor = Color.FromArgb(46, 125, 50)
        lblStatut.Text = message
    End Sub

    ''' <summary>Charge une liste de référence ; affiche l'erreur et renvoie Nothing en cas de problème.</summary>
    Private Async Function Lire(Of T)(lecture As Func(Of T)) As Task(Of T)
        Try
            Return Await Task.Run(lecture)
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End Try
    End Function

    ' ------------------------------------------------------------------
    ' Composition
    ' ------------------------------------------------------------------

    Private Async Sub AjouterComposant(sender As Object, e As EventArgs)
        Dim composants = Await Lire(Function() _service.Composants(_utilisateur))
        If composants Is Nothing Then Return
        Dim frm As New FrmFormulaire("Ajouter un composant", $"Composition de {_medicament.NomCommercial}. Composant absent de la liste : « Nouveau composant… ».")
        frm.AjouterListe("composant", "Composant *", composants)
        frm.AjouterNombre("quantite", "Quantité *", 0D, 0D, 9_999_999.999D, decimales:=3)
        frm.AjouterTexte("unite", "Unité * (mg, g, ml, %…)", "mg", 10)
        frm.Validation = Function() Task.Run(Function() _service.AjouterComposant(_utilisateur, _medicament.DepotLegal,
                                                 frm.Selection(Of ElementReferentiel)("composant")?.Code, frm.Nombre("quantite"), frm.Texte("unite")))
        Await OuvrirFormulaire(frm)
    End Sub

    Private Async Sub RetirerComposant(sender As Object, e As EventArgs)
        Dim c = Selection(Of LigneComposition)(dgvComposition)
        If c Is Nothing Then Return
        Await Retirer($"Retirer {c.Composant} de la composition de {_medicament.NomCommercial} ?",
                      Function() _service.RetirerComposant(_utilisateur, _medicament.DepotLegal, c))
    End Sub

    Private Async Sub NouveauComposant(sender As Object, e As EventArgs)
        Dim frm As New FrmFormulaire("Nouveau composant", "Il pourra ensuite être ajouté à la composition de n'importe quel médicament.", "Créer")
        frm.AjouterTexte("code", "Code (2 à 4 lettres ou chiffres) *", "", 4)
        frm.AjouterTexte("libelle", "Nom *", "", 60)
        frm.Validation = Function() Task.Run(Function() _service.CreerComposant(_utilisateur, frm.Texte("code"), frm.Texte("libelle")))
        Await OuvrirFormulaire(frm)
    End Sub

    ' ------------------------------------------------------------------
    ' Interactions
    ' ------------------------------------------------------------------

    Private Async Sub AjouterInteraction(sender As Object, e As EventArgs)
        Dim medicaments = Await Lire(Function() _service.Medicaments(_utilisateur))
        If medicaments Is Nothing Then Return
        Dim perturbe = $"{_medicament.NomCommercial} perturbe l'effet de l'autre médicament"
        Dim estPerturbe = $"{_medicament.NomCommercial} voit son effet perturbé par l'autre médicament"
        Dim frm As New FrmFormulaire("Ajouter une interaction", $"Interaction médicamenteuse de {_medicament.NomCommercial}.")
        frm.AjouterListe("autre", "Autre médicament *", medicaments.Where(Function(m) m.DepotLegal <> _medicament.DepotLegal).ToList())
        frm.AjouterListe("sens", "Sens *", {perturbe, estPerturbe})
        frm.AjouterTexte("description", "Description (effet, précautions)", "", 500)
        frm.Validation = Function() Task.Run(Function() _service.AjouterInteraction(_utilisateur, _medicament.DepotLegal,
                                                 frm.Selection(Of Medicament)("autre")?.DepotLegal, frm.Selection(Of String)("sens") = perturbe,
                                                 frm.Texte("description")))
        Await OuvrirFormulaire(frm)
    End Sub

    Private Async Sub RetirerInteraction(sender As Object, e As EventArgs)
        Dim i = Selection(Of InteractionMedicamenteuse)(dgvInteractions)
        If i Is Nothing Then Return
        Await Retirer($"Retirer l'interaction entre {_medicament.NomCommercial} et {i.NomAutre} ?",
                      Function() _service.RetirerInteraction(_utilisateur, _medicament.DepotLegal, i))
    End Sub

    ' ------------------------------------------------------------------
    ' Posologie
    ' ------------------------------------------------------------------

    Private Async Sub AjouterPosologie(sender As Object, e As EventArgs)
        Dim types = Await Lire(Function() _service.TypesIndividu(_utilisateur))
        Dim presentations = Await Lire(Function() _service.Presentations(_utilisateur))
        Dim dosages = Await Lire(Function() _service.Dosages(_utilisateur))
        If types Is Nothing OrElse presentations Is Nothing OrElse dosages Is Nothing Then Return
        Dim frm As New FrmFormulaire("Ajouter une posologie", $"Posologie de {_medicament.NomCommercial}. Dosage absent de la liste : « Nouveau dosage… ».")
        frm.AjouterListe("individu", "Type d'individu *", types)
        frm.AjouterListe("presentation", "Présentation *", presentations)
        frm.AjouterListe("dosage", "Dosage *", dosages)
        frm.AjouterTexte("texte", "Posologie * (ex. 1 comprimé 3 fois par jour)", "", 200)
        frm.Validation = Function() Task.Run(Function() _service.AjouterPosologie(_utilisateur, _medicament.DepotLegal,
                                                 frm.Selection(Of ElementReferentiel)("individu")?.Code,
                                                 frm.Selection(Of ElementReferentiel)("presentation")?.Code,
                                                 frm.Selection(Of ElementReferentiel)("dosage")?.Code, frm.Texte("texte")))
        Await OuvrirFormulaire(frm)
    End Sub

    Private Async Sub RetirerPosologie(sender As Object, e As EventArgs)
        Dim p = Selection(Of Posologie)(dgvPosologie)
        If p Is Nothing Then Return
        Await Retirer($"Retirer la posologie « {p.TypeIndividu}, {p.Presentation}, {p.Dosage} » ?",
                      Function() _service.RetirerPosologie(_utilisateur, _medicament.DepotLegal, p))
    End Sub

    Private Async Sub NouveauDosage(sender As Object, e As EventArgs)
        Dim frm As New FrmFormulaire("Nouveau dosage", "Il pourra ensuite être choisi pour la posologie de n'importe quel médicament.", "Créer")
        frm.AjouterNombre("quantite", "Quantité *", 0D, 0D, 9_999_999.999D, decimales:=3)
        frm.AjouterTexte("unite", "Unité * (mg, g, ml, %…)", "mg", 10)
        frm.Validation = Function() Task.Run(Function() _service.CreerDosage(_utilisateur, frm.Nombre("quantite"), frm.Texte("unite")))
        Await OuvrirFormulaire(frm)
    End Sub

End Class
