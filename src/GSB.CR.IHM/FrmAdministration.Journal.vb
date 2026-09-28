Imports GSB.CR.Metier
Imports GSB.CR.Modeles

' Onglet Journal des connexions (EX-74).
Partial Public Class FrmAdministration

    Private dtpJournalDu, dtpJournalAu As DateTimePicker
    Private txtJournalLogin As TextBox
    Private chkJournalEchecs As CheckBox
    Private lblJournalNombre As Label

    Private Sub PreparerJournal()
        AjouterEtiquette(flpBarreJournal, "Du")
        dtpJournalDu = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Width = Echelle(150), .Value = Date.Today.AddDays(-30)}
        AjouterEtiquette(flpBarreJournal, "au")
        flpBarreJournal.Controls.Add(dtpJournalDu)
        flpBarreJournal.Controls.SetChildIndex(dtpJournalDu, 1)
        dtpJournalAu = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Width = Echelle(150), .Value = Date.Today}
        flpBarreJournal.Controls.Add(dtpJournalAu)
        AjouterEtiquette(flpBarreJournal, "Login")
        txtJournalLogin = New TextBox() With {.Width = Echelle(180), .Margin = New Padding(0, 4, 12, 0)}
        flpBarreJournal.Controls.Add(txtJournalLogin)
        chkJournalEchecs = New CheckBox() With {.Text = "Échecs seulement", .AutoSize = True, .Margin = New Padding(0, 6, 12, 0)}
        flpBarreJournal.Controls.Add(chkJournalEchecs)
        AjouterBouton(flpBarreJournal, "Afficher", Async Sub(s, e) Await ChargerJournal(), principal:=True)
        lblJournalNombre = New Label() With {.AutoSize = True, .ForeColor = Theme.TexteGris, .Margin = New Padding(8, 9, 0, 0)}
        flpBarreJournal.Controls.Add(lblJournalNombre)

        PreparerGrille(dgvJournal, {("Date et heure", 22), ("Login saisi", 24), ("Collaborateur", 30), ("Résultat", 24)})
    End Sub

    Private Async Function ChargerJournal() As Task
        Dim du = dtpJournalDu.Value.Date, au = dtpJournalAu.Value.Date, login = txtJournalLogin.Text, echecs = chkJournalEchecs.Checked
        Try
            Dim liste = Await Task.Run(Function() _service.Journal(_utilisateur, du, au, login, echecs))
            If IsDisposed Then Return
            dgvJournal.Rows.Clear()
            For Each j In liste
                Dim i = dgvJournal.Rows.Add(j.DateHeure.ToString("dd/MM/yyyy HH:mm:ss"), j.LoginSaisi,
                                            If(j.NomCollaborateur, "(login inconnu)"), If(j.Succes, "Connexion réussie", "Échec"))
                If Not j.Succes Then
                    Dim cellule = dgvJournal.Rows(i).Cells(3)
                    cellule.Style.ForeColor = Theme.Erreur
                    cellule.Style.SelectionForeColor = Theme.Erreur
                    cellule.Style.Font = New Font(dgvJournal.Font, FontStyle.Bold)
                End If
            Next
            Dim nbEchecs = liste.Where(Function(j) Not j.Succes).Count()
            lblJournalNombre.Text = $"{liste.Count} tentative(s), dont {nbEchecs} échec(s)" &
                                    If(liste.Count >= ServiceAdministration.MaxLignesJournal, " — affichage limité, affinez la période", "")
        Catch ex As ErreurMetierException
            lblJournalNombre.Text = ex.Message
        End Try
    End Function

End Class
