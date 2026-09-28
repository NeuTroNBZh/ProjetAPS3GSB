Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

' Onglet Référentiels (EX-73) : praticiens, médicaments (prix, statut, et composition, interactions, posologie
' dans FrmDetailsMedicament), motifs de visite.
Partial Public Class FrmAdministration

    Private txtRecherchePraticien As TextBox
    Private btnModifierPraticien, btnActiverPraticien, btnModifierMedicament, btnDetailsMedicament, btnModifierMotif As Button
    Private _types As IReadOnlyList(Of TypePraticien) = Array.Empty(Of TypePraticien)()

    Private Sub PreparerReferentiels()
        ' Praticiens
        AjouterBouton(flpBarrePraticiens, "Nouveau praticien", AddressOf NouveauPraticien, principal:=True)
        btnModifierPraticien = AjouterBouton(flpBarrePraticiens, "Modifier", AddressOf ModifierPraticien)
        btnActiverPraticien = AjouterBouton(flpBarrePraticiens, "Désactiver", AddressOf BasculerPraticien)
        AjouterEtiquette(flpBarrePraticiens, "Rechercher")
        txtRecherchePraticien = New TextBox() With {.Width = Echelle(260), .PlaceholderText = "Nom, prénom ou ville…", .Margin = New Padding(0, 4, 0, 0)}
        AddHandler txtRecherchePraticien.TextChanged, Async Sub(s, e) Await ChargerPraticiens()
        flpBarrePraticiens.Controls.Add(txtRecherchePraticien)
        PreparerGrille(dgvPraticiens, {("Praticien", 26), ("Type", 20), ("Ville", 18), ("Suivi par", 20), ("État", 16)})
        AddHandler dgvPraticiens.SelectionChanged, Sub(s, e) MettreAJourBoutonsReferentiels()
        AddHandler dgvPraticiens.CellDoubleClick, Sub(s, e) If e.RowIndex >= 0 Then btnModifierPraticien.PerformClick()

        ' Médicaments
        btnModifierMedicament = AjouterBouton(flpBarreMedicaments, "Modifier le prix ou le statut", AddressOf ModifierMedicament, principal:=True)
        btnDetailsMedicament = AjouterBouton(flpBarreMedicaments, "Composition, interactions, posologie", AddressOf DetailsMedicament)
        PreparerGrille(dgvMedicaments, {("Nom commercial", 22), ("Famille", 36), ("Dépôt légal", 16), ("Prix éch.", 12), ("Statut", 14)})
        AddHandler dgvMedicaments.SelectionChanged, Sub(s, e) MettreAJourBoutonsReferentiels()
        AddHandler dgvMedicaments.CellDoubleClick, Sub(s, e) If e.RowIndex >= 0 Then btnModifierMedicament.PerformClick()

        ' Motifs
        AjouterBouton(flpBarreMotifs, "Nouveau motif", AddressOf NouveauMotif, principal:=True)
        btnModifierMotif = AjouterBouton(flpBarreMotifs, "Modifier", AddressOf ModifierMotif)
        PreparerGrille(dgvMotifs, {("Code", 15), ("Libellé", 60), ("Statut", 25)})
        AddHandler dgvMotifs.SelectionChanged, Sub(s, e) MettreAJourBoutonsReferentiels()
        AddHandler dgvMotifs.CellDoubleClick, Sub(s, e) If e.RowIndex >= 0 Then btnModifierMotif.PerformClick()
        MettreAJourBoutonsReferentiels()
    End Sub

    Private Sub MettreAJourBoutonsReferentiels()
        If btnModifierPraticien Is Nothing Then Return
        Dim p = Selection(Of PraticienResume)(dgvPraticiens)
        btnModifierPraticien.Enabled = p IsNot Nothing
        btnActiverPraticien.Enabled = p IsNot Nothing
        btnActiverPraticien.Text = If(p IsNot Nothing AndAlso Not p.Actif, "Réactiver", "Désactiver")
        btnModifierMedicament.Enabled = dgvMedicaments.SelectedRows.Count > 0
        btnDetailsMedicament.Enabled = dgvMedicaments.SelectedRows.Count > 0
        btnModifierMotif.Enabled = dgvMotifs.SelectedRows.Count > 0
    End Sub

    Private Shared Sub MarquerInactif(ligne As DataGridViewRow, actif As Boolean)
        If Not actif Then ligne.DefaultCellStyle.ForeColor = Theme.TexteGris
    End Sub

    ' --- Praticiens ------------------------------------------------------------

    Private Async Function ChargerPraticiens() As Task
        Dim texte = txtRecherchePraticien.Text
        Dim liste = Await Task.Run(Function() _service.Praticiens(_utilisateur, texte))
        If _types.Count = 0 Then _types = Await Task.Run(Function() _service.TypesPraticien(_utilisateur))
        If IsDisposed OrElse texte <> txtRecherchePraticien.Text Then Return
        dgvPraticiens.Rows.Clear()
        For Each p In liste
            Dim i = dgvPraticiens.Rows.Add(p.NomComplet, p.LibelleType, p.Ville, p.NomVisiteur, If(p.Actif, "Actif", "Inactif"))
            dgvPraticiens.Rows(i).Tag = p
            MarquerInactif(dgvPraticiens.Rows(i), p.Actif)
        Next
        MettreAJourBoutonsReferentiels()
    End Function

    ''' <summary>Formulaire praticien (création si <paramref name="p"/> n'a pas de numéro).</summary>
    Private Function FormulairePraticien(p As Praticien) As FrmFormulaire
        Dim frm As New FrmFormulaire(If(p.Numero.HasValue, $"Modifier {p.NomComplet}", "Nouveau praticien"))
        frm.AjouterTexte("nom", "Nom *", p.Nom, 50)
        frm.AjouterTexte("prenom", "Prénom *", p.Prenom, 50)
        frm.AjouterListe("type", "Type *", _types, _types.FirstOrDefault(Function(t) t.Code = p.CodeType))
        frm.AjouterTexte("adresse", "Adresse", p.Adresse, 100)
        frm.AjouterTexte("cp", "Code postal", p.CodePostal, 5)
        frm.AjouterTexte("ville", "Ville", p.Ville, 50)
        frm.AjouterTexte("telephone", "Téléphone", p.Telephone, 20)
        frm.AjouterTexte("email", "E-mail", p.Email, 100)
        frm.AjouterTexte("notoriete", "Coefficient de notoriété (donnée achetée, facultatif)", p.CoefNotoriete?.ToString("0.##", CultureInfo.CurrentCulture), 10)
        frm.AjouterCase("actif", "Praticien actif (proposé à la saisie des comptes-rendus)", p.Actif)
        frm.Validation =
            Function()
                Dim saisie As Decimal
                Dim texteNotoriete = frm.Texte("notoriete").Trim()
                If texteNotoriete.Length > 0 AndAlso Not Decimal.TryParse(texteNotoriete, NumberStyles.Number, CultureInfo.CurrentCulture, saisie) Then
                    Return Task.FromResult(_service.EnregistrerPraticien(_utilisateur, New Praticien() With {.Nom = ""}))   ' force l'affichage des erreurs de base
                End If
                p.Nom = frm.Texte("nom") : p.Prenom = frm.Texte("prenom")
                p.CodeType = frm.Selection(Of TypePraticien)("type")?.Code
                p.Adresse = frm.Texte("adresse") : p.CodePostal = frm.Texte("cp") : p.Ville = frm.Texte("ville")
                p.Telephone = frm.Texte("telephone") : p.Email = frm.Texte("email")
                p.CoefNotoriete = If(texteNotoriete.Length = 0, CType(Nothing, Decimal?), saisie)
                p.Actif = frm.Coche("actif")
                Return Task.Run(Function() _service.EnregistrerPraticien(_utilisateur, p))
            End Function
        Return frm
    End Function

    Private Async Sub NouveauPraticien(sender As Object, e As EventArgs)
        Await OuvrirFormulaire(FormulairePraticien(New Praticien() With {.CodeType = "MV"}), AddressOf ChargerPraticiens)
    End Sub

    Private Async Sub ModifierPraticien(sender As Object, e As EventArgs)
        Dim r = Selection(Of PraticienResume)(dgvPraticiens)
        If r Is Nothing Then Return
        Try
            Dim p = Await Task.Run(Function() _service.TrouverPraticien(_utilisateur, r.Numero))
            If p Is Nothing Then Return
            Await OuvrirFormulaire(FormulairePraticien(p), AddressOf ChargerPraticiens)
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Async Sub BasculerPraticien(sender As Object, e As EventArgs)
        Dim r = Selection(Of PraticienResume)(dgvPraticiens)
        If r Is Nothing Then Return
        Await Operer(Function()
                         Dim p = _service.TrouverPraticien(_utilisateur, r.Numero)
                         p.Actif = Not p.Actif
                         Return _service.EnregistrerPraticien(_utilisateur, p)
                     End Function, AddressOf ChargerPraticiens)
    End Sub

    ' --- Médicaments ----------------------------------------------------------

    Private Async Function ChargerMedicaments() As Task
        Dim liste = Await Task.Run(Function() _service.Medicaments(_utilisateur))
        If IsDisposed Then Return
        dgvMedicaments.Rows.Clear()
        For Each m In liste
            Dim i = dgvMedicaments.Rows.Add(m.NomCommercial, m.LibelleFamille, m.DepotLegal,
                                            m.PrixEchantillon.ToString("C", CultureInfo.CurrentCulture), If(m.Actif, "Commercialisé", "Retiré"))
            dgvMedicaments.Rows(i).Tag = m
            MarquerInactif(dgvMedicaments.Rows(i), m.Actif)
        Next
        MettreAJourBoutonsReferentiels()
    End Function

    Private Async Sub ModifierMedicament(sender As Object, e As EventArgs)
        Dim m = Selection(Of Medicament)(dgvMedicaments)
        If m Is Nothing Then Return
        Dim frm As New FrmFormulaire($"{m.NomCommercial}", $"Dépôt légal {m.DepotLegal} · {m.LibelleFamille}")
        frm.AjouterNombre("prix", "Prix d'un échantillon (€)", m.PrixEchantillon, 0D, 999999.99D, decimales:=2)
        frm.AjouterCase("actif", "Commercialisé (proposé à la saisie des comptes-rendus et des dotations)", m.Actif)
        frm.Validation = Function() Task.Run(Function() _service.ModifierMedicament(_utilisateur, m.DepotLegal, frm.Nombre("prix"), frm.Coche("actif")))
        Await OuvrirFormulaire(frm, AddressOf ChargerMedicaments)
    End Sub

    ''' <summary>Composition, interactions et posologie du médicament sélectionné (EX-73).</summary>
    Private Sub DetailsMedicament(sender As Object, e As EventArgs)
        Dim m = Selection(Of Medicament)(dgvMedicaments)
        If m Is Nothing Then Return
        Using frm As New FrmDetailsMedicament(_service, _utilisateur, m)
            frm.ShowDialog(Me)
        End Using
    End Sub

    ' --- Motifs ---------------------------------------------------------------

    Private Async Function ChargerMotifs() As Task
        Dim liste = Await Task.Run(Function() _service.Motifs(_utilisateur))
        If IsDisposed Then Return
        dgvMotifs.Rows.Clear()
        For Each m In liste
            Dim i = dgvMotifs.Rows.Add(m.Code, m.Libelle, If(m.Actif, "Proposé", "Désactivé"))
            dgvMotifs.Rows(i).Tag = m
            MarquerInactif(dgvMotifs.Rows(i), m.Actif)
        Next
        MettreAJourBoutonsReferentiels()
    End Function

    Private Async Sub NouveauMotif(sender As Object, e As EventArgs)
        Dim frm As New FrmFormulaire("Nouveau motif de visite", "Il sera proposé juste avant « Autre » dans la liste des motifs.")
        frm.AjouterTexte("code", "Code (2 à 6 lettres) *", "", 6)
        frm.AjouterTexte("libelle", "Libellé *", "", 60)
        frm.Validation = Function() Task.Run(Function() _service.CreerMotif(_utilisateur, frm.Texte("code"), frm.Texte("libelle")))
        Await OuvrirFormulaire(frm, AddressOf ChargerMotifs)
    End Sub

    Private Async Sub ModifierMotif(sender As Object, e As EventArgs)
        Dim m = Selection(Of Motif)(dgvMotifs)
        If m Is Nothing Then Return
        Dim frm As New FrmFormulaire($"Motif {m.Code}", "Un motif désactivé n'est plus proposé mais reste visible dans les anciens comptes-rendus.")
        frm.AjouterTexte("libelle", "Libellé *", m.Libelle, 60)
        frm.AjouterCase("actif", "Proposé à la saisie", m.Actif)
        frm.Validation = Function() Task.Run(Function() _service.ModifierMotif(_utilisateur, m.Code, frm.Texte("libelle"), frm.Coche("actif")))
        Await OuvrirFormulaire(frm, AddressOf ChargerMotifs)
    End Sub

End Class
