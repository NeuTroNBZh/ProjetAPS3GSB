Imports System.ComponentModel
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Page d'accueil de l'application : uniquement la zone d'identification (EX-01).
''' Renvoie DialogResult.OK quand l'utilisateur est accepté.
''' </summary>
Public Class FrmConnexion

    Private ReadOnly _service As ServiceAuthentification

    ''' <summary>Utilisateur connecté (renseigné quand la fenêtre renvoie OK).</summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Utilisateur As UtilisateurConnecte

    ''' <summary>Vrai si l'utilisateur doit changer son mot de passe avant d'entrer (EX-07).</summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ChangementMotDePasseRequis As Boolean

    Public Sub New(service As ServiceAuthentification)
        InitializeComponent()
        _service = service
        AppliquerTheme()
    End Sub

    Private Sub AppliquerTheme()
        BackColor = Theme.Blanc
        pnlMarque.BackColor = Theme.BleuGsb
        For Each lbl In {lblLogo, lblLaboratoire, lblApplication, lblVersion}
            lbl.ForeColor = Theme.Blanc
        Next
        lblVersion.Text = $"Version {Application.ProductVersion.Split("+"c)(0)}"
        lblTitre.Font = Theme.PoliceTitre
        lblTitre.ForeColor = Theme.BleuGsb
        lblSousTitre.ForeColor = Theme.TexteGris
        lblIdentifiant.ForeColor = Theme.BleuFonce
        lblMotDePasse.ForeColor = Theme.BleuFonce
        chkAfficherMotDePasse.ForeColor = Theme.TexteGris
        lblErreur.ForeColor = Theme.Erreur
        Theme.StyliserBoutonPrincipal(btnConnexion)
    End Sub

    Private Sub FrmConnexion_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtIdentifiant.Focus()
    End Sub

    Private Sub chkAfficherMotDePasse_CheckedChanged(sender As Object, e As EventArgs) Handles chkAfficherMotDePasse.CheckedChanged
        txtMotDePasse.UseSystemPasswordChar = Not chkAfficherMotDePasse.Checked
    End Sub

    Private Async Sub btnConnexion_Click(sender As Object, e As EventArgs) Handles btnConnexion.Click
        Dim login = txtIdentifiant.Text
        Dim motDePasse = txtMotDePasse.Text

        BasculerAttente(True)
        ' Appel base + calcul PBKDF2 hors du thread de l'interface pour qu'elle reste réactive
        Dim resultat = Await Task.Run(Function() _service.Connecter(login, motDePasse))
        BasculerAttente(False)

        If resultat.EstAcceptee Then
            Utilisateur = resultat.Utilisateur
            ChangementMotDePasseRequis = resultat.Statut = StatutConnexion.ChangementMotDePasseRequis
            DialogResult = DialogResult.OK
            Close()
            Return
        End If

        lblErreur.Text = resultat.Message
        txtMotDePasse.Clear()
        If String.IsNullOrWhiteSpace(txtIdentifiant.Text) Then txtIdentifiant.Focus() Else txtMotDePasse.Focus()
    End Sub

    Private Sub BasculerAttente(enCours As Boolean)
        btnConnexion.Enabled = Not enCours
        txtIdentifiant.Enabled = Not enCours
        txtMotDePasse.Enabled = Not enCours
        btnConnexion.Text = If(enCours, "Connexion en cours…", "Se connecter")
        UseWaitCursor = enCours
        If enCours Then lblErreur.Text = ""
    End Sub

End Class
