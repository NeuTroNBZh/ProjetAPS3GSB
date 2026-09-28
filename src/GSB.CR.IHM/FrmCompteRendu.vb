Imports System.ComponentModel
Imports System.Text
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Saisie ou modification d'un compte-rendu de visite (EX-10 à EX-19, EX-25 à EX-29).
''' Renvoie DialogResult.OK si le compte-rendu a été enregistré.
''' </summary>
Public Class FrmCompteRendu

    Private Shared ReadOnly LibellesConfiance As String() = {
        "(à renseigner)", "1 — Très réticent", "2 — Réticent", "3 — Neutre", "4 — Confiant", "5 — Très confiant"}

    ''' <summary>Élément « aucun produit » des listes de produits présentés.</summary>
    Private Shared ReadOnly AucunProduit As New Medicament() With {.DepotLegal = "", .NomCommercial = "(aucun)"}

    Private ReadOnly _service As ServiceRapports
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private ReadOnly _numero As Integer?
    Private ReadOnly _debutSaisie As DateTime

    Private _rapport As RapportVisite
    Private _echantillons As New List(Of EchantillonOffert)
    Private _valideEnBase As Boolean
    Private _chargement As Boolean = True
    Private _modifie As Boolean

    ''' <summary>Numéro du compte-rendu enregistré (renseigné quand la fenêtre renvoie OK).</summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property NumeroEnregistre As Integer?

    ''' <summary>Vrai si le compte-rendu enregistré est validé (faux : brouillon).</summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property EnregistreValide As Boolean

    ''' <param name="numero">Numéro du CR à modifier, ou Nothing pour un nouveau CR.</param>
    Public Sub New(service As ServiceRapports, utilisateur As UtilisateurConnecte, numero As Integer?)
        InitializeComponent()
        _service = service
        _utilisateur = utilisateur
        _numero = numero
        _debutSaisie = service.Maintenant
        AppliquerTheme()
    End Sub

    Private Sub AppliquerTheme()
        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        For Each s In {lblSectionVisite, lblSectionProduits, lblSectionEchantillons, lblSectionBilan}
            Theme.StyliserSection(s)
        Next
        For Each l In {lblPraticien, lblDateVisite, lblMotif, lblPrecision, lblProduit1, lblProduit2, lblBilan, lblConfiance}
            l.ForeColor = Theme.BleuFonce
        Next
        For Each l In {lblCompteur, lblObligatoire, lblTotalEchantillons}
            l.ForeColor = Theme.TexteGris
        Next
        lblObligatoire.Font = New Font("Segoe UI", 9.0F, FontStyle.Italic)
        lblErreurs.ForeColor = Theme.Erreur
        Theme.StyliserBoutonPrincipal(btnValider)
        Theme.StyliserBoutonSecondaire(btnBrouillon)
        Theme.StyliserBoutonSecondaire(btnAnnuler)
        Theme.StyliserBoutonSecondaire(btnAjouterEchantillon)
        Theme.StyliserBoutonSecondaire(btnRetirerEchantillon)
        Theme.StyliserBoutonSecondaire(btnNouveauRemplacant)
        Theme.StyliserGrille(dgvEchantillons)
        dtpDateVisite.Format = DateTimePickerFormat.Long
        dtpProchaineVisite.Format = DateTimePickerFormat.Long
        cboConfiance.Items.AddRange(LibellesConfiance)
    End Sub

    ' ------------------------------------------------------------------
    ' Chargement
    ' ------------------------------------------------------------------

    Private Async Sub FrmCompteRendu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pnlContenu.Enabled = False
        pnlActions.Enabled = False
        UseWaitCursor = True
        Try
            Dim donnees = Await Task.Run(Function() ChargerDonnees())
            Remplir(donnees.Rapport, donnees.Portefeuille, donnees.Praticiens, donnees.Motifs, donnees.Medicaments)
            pnlContenu.Enabled = True
            pnlActions.Enabled = True
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Close()
        Finally
            UseWaitCursor = False
            _chargement = False
        End Try
    End Sub

    ''' <summary>Lit en arrière-plan le rapport et toutes les listes nécessaires au formulaire.</summary>
    Private Function ChargerDonnees() As (Rapport As RapportVisite, Portefeuille As List(Of Praticien), Praticiens As List(Of Praticien),
                                          Motifs As IReadOnlyList(Of Motif), Medicaments As IReadOnlyList(Of Medicament))
        Dim rapport = If(_numero.HasValue, _service.Charger(_utilisateur, _numero.Value), _service.NouveauRapport(_utilisateur))
        Dim portefeuille = _service.Portefeuille(_utilisateur).ToList()
        Dim praticiens = _service.RechercherPraticiens("").ToList()

        ' Un ancien CR peut concerner un praticien sorti du portefeuille ou devenu inactif
        For Each numero In {rapport.NumeroPraticien, rapport.NumeroRemplacant}
            If numero.HasValue AndAlso Not portefeuille.Concat(praticiens).Any(Function(p) Nullable.Equals(p.Numero, numero)) Then
                Dim p = _service.TrouverPraticien(numero.Value)
                If p IsNot Nothing Then praticiens.Add(p)
            End If
        Next
        If rapport.NumeroPraticien.HasValue AndAlso Not portefeuille.Any(Function(p) Nullable.Equals(p.Numero, rapport.NumeroPraticien)) Then
            Dim titulaire = praticiens.FirstOrDefault(Function(p) Nullable.Equals(p.Numero, rapport.NumeroPraticien))
            If titulaire IsNot Nothing Then portefeuille.Insert(0, titulaire)
        End If

        Return (rapport, portefeuille, praticiens, _service.Motifs(), _service.Medicaments())
    End Function

    Private Sub Remplir(rapport As RapportVisite, portefeuille As List(Of Praticien), praticiens As List(Of Praticien),
                        motifs As IReadOnlyList(Of Motif), medicaments As IReadOnlyList(Of Medicament))
        _rapport = rapport
        _valideEnBase = Not rapport.EstNouveau AndAlso rapport.Etat = EtatRapport.Valide
        _echantillons = rapport.Echantillons.Select(Function(x) New EchantillonOffert() With {
            .DepotLegal = x.DepotLegal, .NomCommercial = x.NomCommercial, .Quantite = x.Quantite}).ToList()

        ' Listes : éléments actifs + ceux déjà utilisés par ce CR (même s'ils sont devenus inactifs)
        Dim utilises = rapport.ProduitsPresentes.Concat(rapport.Echantillons.Select(Function(x) x.DepotLegal)).ToHashSet()
        Dim produits = medicaments.Where(Function(m) m.Actif OrElse utilises.Contains(m.DepotLegal)).ToArray()

        cboPraticien.Items.AddRange(portefeuille.ToArray())
        cboRemplacant.Items.AddRange(praticiens.OrderBy(Function(p) p.NomComplet).ToArray())
        cboMotif.Items.AddRange(motifs.Where(Function(m) m.Actif OrElse m.Code = rapport.CodeMotif).ToArray())
        For Each cbo In {cboProduit1, cboProduit2}
            cbo.Items.Add(AucunProduit)
            cbo.Items.AddRange(produits)
        Next
        cboEchantillon.Items.AddRange(medicaments.Where(Function(m) m.Actif).ToArray())
        If cboEchantillon.Items.Count > 0 Then cboEchantillon.SelectedIndex = 0

        ' Valeurs du CR
        Selectionner(cboPraticien, Function(p As Praticien) p.Numero = rapport.NumeroPraticien)
        chkRemplacant.Checked = rapport.NumeroRemplacant.HasValue
        Selectionner(cboRemplacant, Function(p As Praticien) p.Numero = rapport.NumeroRemplacant)
        dtpDateVisite.MaxDate = _service.Maintenant.Date.AddDays(1).AddSeconds(-1)   ' pas de visite dans le futur (EX-27)
        dtpDateVisite.Value = If(rapport.DateVisite, _service.Maintenant.Date)
        Selectionner(cboMotif, Function(m As Motif) m.Code = rapport.CodeMotif)
        txtPrecisionMotif.Text = rapport.PrecisionMotif
        cboProduit1.SelectedIndex = 0
        cboProduit2.SelectedIndex = 0
        If rapport.ProduitsPresentes.Count > 0 Then Selectionner(cboProduit1, Function(m As Medicament) m.DepotLegal = rapport.ProduitsPresentes(0))
        If rapport.ProduitsPresentes.Count > 1 Then Selectionner(cboProduit2, Function(m As Medicament) m.DepotLegal = rapport.ProduitsPresentes(1))
        txtBilan.Text = rapport.Bilan
        cboConfiance.SelectedIndex = If(rapport.CoefConfiance, 0)
        chkProchaineVisite.Checked = rapport.DateProchaineVisite.HasValue
        dtpProchaineVisite.Value = If(rapport.DateProchaineVisite, dtpDateVisite.Value.AddMonths(6))
        AfficherEchantillons()
        MettreAJourEntete()
        MettreAJourEtatControles()
        MettreAJourCompteur()
    End Sub

    Private Shared Sub Selectionner(Of T)(cbo As ComboBox, critere As Func(Of T, Boolean?))
        For i = 0 To cbo.Items.Count - 1
            If critere(DirectCast(cbo.Items(i), T)).GetValueOrDefault() Then
                cbo.SelectedIndex = i
                Return
            End If
        Next
    End Sub

    Private Sub MettreAJourEntete()
        If _rapport.EstNouveau Then
            lblTitre.Text = "Nouveau compte-rendu"
            lblSousTitre.Text = $"Saisi par {_utilisateur.Collaborateur.NomComplet}"
        Else
            lblTitre.Text = $"Compte-rendu n° {_rapport.Numero} — {If(_valideEnBase, "validé", "brouillon")}"
            Dim details As New StringBuilder($"Saisi le {_rapport.DateSaisie:dd/MM/yyyy à HH\hmm}")
            If _rapport.DateModification.HasValue Then details.Append($" · modifié le {_rapport.DateModification:dd/MM/yyyy à HH\hmm}")
            If _rapport.DateValidation.HasValue Then details.Append($" · validé le {_rapport.DateValidation:dd/MM/yyyy à HH\hmm}")
            lblSousTitre.Text = details.ToString()
        End If
        ' Un CR validé reste modifiable mais ne repasse pas en brouillon (EX-29)
        btnBrouillon.Visible = Not _valideEnBase
        btnValider.Text = If(_valideEnBase, "Enregistrer les modifications", "Valider le compte-rendu")
    End Sub

    ' ------------------------------------------------------------------
    ' Interactions
    ' ------------------------------------------------------------------

    Private Sub MettreAJourEtatControles()
        cboRemplacant.Enabled = chkRemplacant.Checked
        btnNouveauRemplacant.Enabled = chkRemplacant.Checked
        Dim motif = TryCast(cboMotif.SelectedItem, Motif)
        txtPrecisionMotif.Enabled = motif IsNot Nothing AndAlso motif.EstAutre
        dtpProchaineVisite.Enabled = chkProchaineVisite.Checked
        btnRetirerEchantillon.Enabled = dgvEchantillons.SelectedRows.Count > 0
    End Sub

    Private Sub Champ_Modifie(sender As Object, e As EventArgs) Handles _
            cboPraticien.SelectedIndexChanged, chkRemplacant.CheckedChanged, cboRemplacant.SelectedIndexChanged,
            dtpDateVisite.ValueChanged, cboMotif.SelectedIndexChanged, txtPrecisionMotif.TextChanged,
            cboProduit1.SelectedIndexChanged, cboProduit2.SelectedIndexChanged, txtBilan.TextChanged,
            cboConfiance.SelectedIndexChanged, chkProchaineVisite.CheckedChanged, dtpProchaineVisite.ValueChanged
        If _chargement Then Return
        _modifie = True
        lblErreurs.Text = ""
        MettreAJourEtatControles()
    End Sub

    Private Sub txtBilan_TextChanged(sender As Object, e As EventArgs) Handles txtBilan.TextChanged
        MettreAJourCompteur()
    End Sub

    Private Sub MettreAJourCompteur()
        Dim octets = Encoding.UTF8.GetByteCount(txtBilan.Text)
        lblCompteur.Text = $"{txtBilan.Text.Length} caractères"
        lblCompteur.ForeColor = If(octets > ValidateurRapport.MaxOctetsBilan, Theme.Erreur, Theme.TexteGris)
        If octets > ValidateurRapport.MaxOctetsBilan Then lblCompteur.Text &= " — trop long"
    End Sub

    Private Sub dgvEchantillons_SelectionChanged(sender As Object, e As EventArgs) Handles dgvEchantillons.SelectionChanged
        btnRetirerEchantillon.Enabled = dgvEchantillons.SelectedRows.Count > 0
    End Sub

    Private Sub btnAjouterEchantillon_Click(sender As Object, e As EventArgs) Handles btnAjouterEchantillon.Click
        Dim produit = TryCast(cboEchantillon.SelectedItem, Medicament)
        If produit Is Nothing Then Return
        Dim quantite = CInt(nudQuantite.Value)

        ' Un produit déjà présent voit sa quantité augmenter plutôt que d'apparaître deux fois
        Dim existant = _echantillons.FirstOrDefault(Function(x) x.DepotLegal = produit.DepotLegal)
        If existant Is Nothing Then
            _echantillons.Add(New EchantillonOffert() With {.DepotLegal = produit.DepotLegal, .NomCommercial = produit.NomCommercial, .Quantite = quantite})
        Else
            existant.Quantite = Math.Min(existant.Quantite + quantite, ValidateurRapport.MaxQuantiteEchantillon)
        End If
        nudQuantite.Value = 1
        _modifie = True
        AfficherEchantillons()
    End Sub

    Private Sub btnRetirerEchantillon_Click(sender As Object, e As EventArgs) Handles btnRetirerEchantillon.Click
        If dgvEchantillons.SelectedRows.Count = 0 Then Return
        _echantillons.RemoveAt(dgvEchantillons.SelectedRows(0).Index)
        _modifie = True
        AfficherEchantillons()
    End Sub

    Private Sub AfficherEchantillons()
        dgvEchantillons.Rows.Clear()
        For Each x In _echantillons
            dgvEchantillons.Rows.Add(x.NomCommercial, x.Quantite)
        Next
        Dim total = _echantillons.Sum(Function(x) x.Quantite)
        lblTotalEchantillons.Text = If(total = 0, "Aucun échantillon", $"{total} échantillon(s), {_echantillons.Count} produit(s)")
        MettreAJourEtatControles()
    End Sub

    Private Sub btnNouveauRemplacant_Click(sender As Object, e As EventArgs) Handles btnNouveauRemplacant.Click
        Using frm As New FrmNouveauRemplacant(_service)
            If frm.ShowDialog(Me) <> DialogResult.OK Then Return
            cboRemplacant.Items.Add(frm.Praticien)
            cboRemplacant.SelectedItem = frm.Praticien
        End Using
    End Sub

    ' ------------------------------------------------------------------
    ' Enregistrement
    ' ------------------------------------------------------------------

    ''' <summary>Recopie la saisie du formulaire dans le rapport.</summary>
    Private Sub LireFormulaire()
        _rapport.NumeroPraticien = TryCast(cboPraticien.SelectedItem, Praticien)?.Numero
        _rapport.NumeroRemplacant = If(chkRemplacant.Checked, TryCast(cboRemplacant.SelectedItem, Praticien)?.Numero, Nothing)
        _rapport.DateVisite = dtpDateVisite.Value.Date
        _rapport.CodeMotif = TryCast(cboMotif.SelectedItem, Motif)?.Code
        _rapport.PrecisionMotif = txtPrecisionMotif.Text
        _rapport.ProduitsPresentes = {cboProduit1, cboProduit2}.
            Select(Function(c) TryCast(c.SelectedItem, Medicament)).
            Where(Function(m) m IsNot Nothing AndAlso m IsNot AucunProduit).
            Select(Function(m) m.DepotLegal).ToList()
        _rapport.Echantillons = _echantillons.ToList()
        _rapport.Bilan = txtBilan.Text
        _rapport.CoefConfiance = If(cboConfiance.SelectedIndex > 0, cboConfiance.SelectedIndex, CType(Nothing, Integer?))
        _rapport.DateProchaineVisite = If(chkProchaineVisite.Checked, dtpProchaineVisite.Value.Date, CType(Nothing, Date?))
    End Sub

    Private Sub btnBrouillon_Click(sender As Object, e As EventArgs) Handles btnBrouillon.Click
        Enregistrer(valider:=False)
    End Sub

    Private Sub btnValider_Click(sender As Object, e As EventArgs) Handles btnValider.Click
        Enregistrer(valider:=True)
    End Sub

    Private Async Sub Enregistrer(valider As Boolean)
        If chkRemplacant.Checked AndAlso cboRemplacant.SelectedItem Is Nothing Then
            lblErreurs.Text = Theme.EnPuces({"Choisissez le remplaçant rencontré, ou décochez la case « remplaçant »."})
            Return
        End If
        LireFormulaire()

        pnlActions.Enabled = False
        pnlContenu.Enabled = False
        UseWaitCursor = True
        Dim resultat = Await Task.Run(Function() _service.Enregistrer(_utilisateur, _rapport, valider, _debutSaisie))
        UseWaitCursor = False
        pnlActions.Enabled = True
        pnlContenu.Enabled = True

        If Not resultat.Reussi Then
            lblErreurs.Text = Theme.EnPuces(resultat.Erreurs)
            Return
        End If
        _modifie = False
        NumeroEnregistre = resultat.Numero
        EnregistreValide = valider
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnAnnuler_Click(sender As Object, e As EventArgs) Handles btnAnnuler.Click
        Close()
    End Sub

    Private Sub FrmCompteRendu_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult = DialogResult.OK OrElse Not _modifie Then Return
        Dim reponse = MessageBox.Show(Me, "Les modifications non enregistrées seront perdues. Fermer quand même ?",
                                      "GSB", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If reponse = DialogResult.No Then e.Cancel = True
    End Sub

End Class
