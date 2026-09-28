Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Messagerie interne (EX-50) : boîte de réception (non lus en gras), messages envoyés avec suivi de lecture,
''' lecture, nouveau message et réponse.
''' </summary>
Public Class FrmMessagerie

    Private ReadOnly _service As ServiceMessagerie
    Private ReadOnly _utilisateur As UtilisateurConnecte
    Private _messageAffiche As MessageDetaille
    Private _demande As Integer
    Private _remplissage As Boolean

    Public Sub New(service As ServiceMessagerie, utilisateur As UtilisateurConnecte)
        InitializeComponent()
        _service = service
        _utilisateur = utilisateur

        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        lblNonLus.ForeColor = Theme.BleuFonce
        Theme.StyliserBoutonPrincipal(btnNouveau)
        Theme.StyliserBoutonSecondaire(btnRepondre)
        Theme.StyliserBoutonSecondaire(btnFermer)
        For Each g In {dgvRecus, dgvEnvoyes}
            Theme.StyliserGrille(g)
        Next
        tabRecus.BackColor = Theme.Blanc
        tabEnvoyes.BackColor = Theme.Blanc
        Colonnes(dgvRecus, {("De", 34), ("Objet", 44), ("Date", 22)})
        Colonnes(dgvEnvoyes, {("À", 36), ("Objet", 34), ("Lu par", 12), ("Date", 18)})
        btnRepondre.Enabled = False
        pnlMessage.AfficherMessage("Sélectionnez un message pour le lire.")
    End Sub

    Private Shared Sub Colonnes(grille As DataGridView, colonnes As IEnumerable(Of (Titre As String, Poids As Integer)))
        For Each c In colonnes
            grille.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = c.Titre, .FillWeight = c.Poids, .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
    End Sub

    Private Async Sub FrmMessagerie_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await Recharger()
    End Sub

    Private Async Function Recharger() As Task
        UseWaitCursor = True
        Try
            Dim recus = Await Task.Run(Function() _service.BoiteDeReception(_utilisateur))
            Dim envoyes = Await Task.Run(Function() _service.MessagesEnvoyes(_utilisateur))
            If IsDisposed Then Return
            AfficherListes(recus, envoyes)
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            UseWaitCursor = False
        End Try
    End Function

    Private Sub AfficherListes(recus As IReadOnlyList(Of MessageResume), envoyes As IReadOnlyList(Of MessageResume))
        Dim gras As New Font(dgvRecus.Font, FontStyle.Bold)
        _remplissage = True
        dgvRecus.Rows.Clear()
        For Each m In recus
            Dim ligne = dgvRecus.Rows(dgvRecus.Rows.Add(m.Correspondant, m.Objet, Horodatage(m.DateEnvoi)))
            ligne.Tag = m
            ' Non lu : en gras (le texte « non lu » est aussi dans le compteur, jamais la mise en forme seule)
            If Not m.Lu Then ligne.DefaultCellStyle.Font = gras
        Next
        dgvEnvoyes.Rows.Clear()
        For Each m In envoyes
            Dim ligne = dgvEnvoyes.Rows(dgvEnvoyes.Rows.Add(m.Correspondant, m.Objet, $"{m.NbLus} / {m.NbDestinataires}", Horodatage(m.DateEnvoi)))
            ligne.Tag = m
        Next
        dgvRecus.ClearSelection()
        dgvEnvoyes.ClearSelection()
        _remplissage = False

        Dim nonLus = recus.Where(Function(m) Not m.Lu).Count()
        tabRecus.Text = If(nonLus > 0, $"Reçus ({nonLus} non lu{If(nonLus > 1, "s", "")})", "Reçus")
        tabEnvoyes.Text = $"Envoyés ({envoyes.Count})"
        lblNonLus.Text = If(nonLus = 0, "Aucun message non lu.", $"{nonLus} message(s) non lu(s).")
    End Sub

    Private Shared Function Horodatage(d As DateTime) As String
        Return If(d.Date = Date.Today, d.ToString("HH:mm"), d.ToString("dd/MM/yyyy HH:mm"))
    End Function

    Private Sub Selection_Change(sender As Object, e As EventArgs) Handles dgvRecus.SelectionChanged, dgvEnvoyes.SelectionChanged
        If _remplissage Then Return
        Dim grille = DirectCast(sender, DataGridView)
        If grille.SelectedRows.Count = 0 Then Return
        Dim m = TryCast(grille.SelectedRows(0).Tag, MessageResume)
        If m IsNot Nothing Then Ouvrir(m, grille.SelectedRows(0))
    End Sub

    Private Async Sub Ouvrir(apercu As MessageResume, ligne As DataGridViewRow)
        _demande += 1
        Dim demande = _demande
        Try
            Dim message = Await Task.Run(Function() _service.Lire(_utilisateur, apercu.Id))
            If IsDisposed OrElse demande <> _demande Then Return
            _messageAffiche = message
            AfficherMessage(message)
            ' Le message reçu est désormais lu : on retire le gras et on met à jour les compteurs
            If ligne.DataGridView Is dgvRecus AndAlso Not apercu.Lu Then
                apercu.Lu = True
                ligne.DefaultCellStyle.Font = dgvRecus.Font
                Dim nonLus = dgvRecus.Rows.Cast(Of DataGridViewRow)().Where(Function(r) Not DirectCast(r.Tag, MessageResume).Lu).Count()
                tabRecus.Text = If(nonLus > 0, $"Reçus ({nonLus} non lu{If(nonLus > 1, "s", "")})", "Reçus")
                lblNonLus.Text = If(nonLus = 0, "Aucun message non lu.", $"{nonLus} message(s) non lu(s).")
            End If
        Catch ex As ErreurMetierException
            pnlMessage.AfficherMessage(ex.Message)
        End Try
    End Sub

    Private Sub AfficherMessage(m As MessageDetaille)
        Dim culture = CultureInfo.CurrentCulture
        Dim recu = m.MatriculeExpediteur <> _utilisateur.Matricule
        pnlMessage.SuspendLayout()
        pnlMessage.Vider()
        pnlMessage.AjouterTitre(m.Objet)
        pnlMessage.AjouterSousTitre($"Le {m.DateEnvoi.ToString("dddd d MMMM yyyy à HH:mm", culture)}")
        pnlMessage.AjouterInformation("De", m.Expediteur)
        pnlMessage.AjouterInformation("À", String.Join(", ", m.Destinataires.Select(Function(d) d.NomComplet)))
        pnlMessage.AjouterSection("Message")
        pnlMessage.AjouterTexte(m.Contenu)
        If Not recu Then
            ' Suivi de lecture pour l'expéditeur
            pnlMessage.AjouterSection($"Lecture ({m.Destinataires.Where(Function(d) d.DateLecture.HasValue).Count()} / {m.Destinataires.Count})")
            pnlMessage.AjouterTableau({("Destinataire", 60.0F), ("Lu le", 40.0F)},
                m.Destinataires.Select(Function(d) New Object() {d.NomComplet, If(d.DateLecture.HasValue, d.DateLecture.Value.ToString("dd/MM/yyyy HH:mm"), "non lu")}))
        End If
        pnlMessage.ResumeLayout()
        pnlMessage.AutoScrollPosition = New Point(0, 0)
        btnRepondre.Enabled = recu
    End Sub

    Private Async Sub btnNouveau_Click(sender As Object, e As EventArgs) Handles btnNouveau.Click
        Await Ecrire(Nothing, "", "")
    End Sub

    Private Async Sub btnRepondre_Click(sender As Object, e As EventArgs) Handles btnRepondre.Click
        If _messageAffiche Is Nothing Then Return
        Dim citation = $"{Environment.NewLine}{Environment.NewLine}--- Le {_messageAffiche.DateEnvoi:dd/MM/yyyy HH:mm}, {_messageAffiche.Expediteur} a écrit :{Environment.NewLine}{_messageAffiche.Contenu}"
        Await Ecrire({_messageAffiche.MatriculeExpediteur}, ServiceMessagerie.ObjetReponse(_messageAffiche.Objet), citation)
    End Sub

    Private Async Function Ecrire(destinataires As IEnumerable(Of String), objet As String, contenu As String) As Task
        Using frm As New FrmNouveauMessage(_service, _utilisateur, destinataires, objet, contenu)
            If frm.ShowDialog(Me) <> DialogResult.OK Then Return
        End Using
        tabBoites.SelectedTab = tabEnvoyes
        Await Recharger()
    End Function

End Class
