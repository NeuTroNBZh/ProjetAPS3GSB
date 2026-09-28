Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Changement du mot de passe (EX-07) : obligatoire à la première connexion, ou à la demande.
''' </summary>
Public Class FrmChangementMotDePasse

    Private ReadOnly _service As ServiceAuthentification
    Private ReadOnly _utilisateur As UtilisateurConnecte

    ''' <param name="obligatoire">Vrai si l'utilisateur ne peut pas entrer sans changer son mot de passe.</param>
    Public Sub New(service As ServiceAuthentification, utilisateur As UtilisateurConnecte, obligatoire As Boolean)
        InitializeComponent()
        _service = service
        _utilisateur = utilisateur

        BackColor = Theme.Blanc
        lblTitre.Font = Theme.PoliceTitre
        lblTitre.ForeColor = Theme.BleuGsb
        lblInformation.ForeColor = Theme.TexteGris
        lblRegles.ForeColor = Theme.TexteGris
        lblErreurs.ForeColor = Theme.Erreur
        For Each lbl In {lblActuel, lblNouveau, lblConfirmation}
            lbl.ForeColor = Theme.BleuFonce
        Next
        Theme.StyliserBoutonPrincipal(btnValider)
        Theme.StyliserBoutonSecondaire(btnAnnuler)

        lblRegles.Text = PolitiqueMotDePasse.Description
        If obligatoire Then
            lblInformation.Text = $"Bonjour {utilisateur.Collaborateur.Prenom}, vous devez choisir un nouveau mot de passe avant d'accéder à l'application."
            btnAnnuler.Text = "Se déconnecter"
        Else
            lblInformation.Text = "Saisissez votre mot de passe actuel puis le nouveau."
        End If
    End Sub

    Private Async Sub btnValider_Click(sender As Object, e As EventArgs) Handles btnValider.Click
        Dim ancien = txtActuel.Text, nouveau = txtNouveau.Text, confirmation = txtConfirmation.Text

        btnValider.Enabled = False
        UseWaitCursor = True
        Dim erreurs = Await Task.Run(Function() _service.ChangerMotDePasse(_utilisateur, ancien, nouveau, confirmation))
        UseWaitCursor = False
        btnValider.Enabled = True

        If erreurs.Count = 0 Then
            MessageBox.Show(Me, "Votre mot de passe a été modifié.", "Mot de passe",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            DialogResult = DialogResult.OK
            Close()
            Return
        End If

        lblErreurs.Text = String.Join(Environment.NewLine, erreurs.Select(Function(m) "• " & m))
        txtNouveau.Clear()
        txtConfirmation.Clear()
        txtNouveau.Focus()
    End Sub

End Class
