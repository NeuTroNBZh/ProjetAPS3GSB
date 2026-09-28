Imports System.ComponentModel
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Menu principal après connexion : affiche les modules accessibles selon le profil (EX-02).
''' </summary>
Public Class FrmAccueil

    Private ReadOnly _fabrique As FabriqueServices
    Private ReadOnly _service As ServiceAuthentification
    Private ReadOnly _utilisateur As UtilisateurConnecte

    ''' <summary>Vrai si l'utilisateur a demandé à se déconnecter (retour à l'écran de connexion).</summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Deconnexion As Boolean

    Public Sub New(fabrique As FabriqueServices, service As ServiceAuthentification, utilisateur As UtilisateurConnecte)
        InitializeComponent()
        _fabrique = fabrique
        _service = service
        _utilisateur = utilisateur
        AppliquerTheme()
        AfficherUtilisateur()
        CreerTuilesModules()
    End Sub

    Private Sub AppliquerTheme()
        BackColor = Theme.Blanc
        pnlEntete.BackColor = Theme.BleuGsb
        For Each lbl In {lblApplication, lblUtilisateur, lblRattachement}
            lbl.ForeColor = Theme.Blanc
        Next
        lblBienvenue.Font = Theme.PoliceTitre
        lblBienvenue.ForeColor = Theme.BleuFonce
        lblPied.BackColor = Theme.BleuClair
        lblPied.ForeColor = Theme.TexteGris
        For Each btn In {btnMotDePasse, btnDeconnexion}
            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderColor = Theme.Blanc
            btn.FlatAppearance.MouseOverBackColor = Theme.BleuFonce
            btn.ForeColor = Theme.Blanc
            btn.BackColor = Theme.BleuGsb
            btn.Cursor = Cursors.Hand
        Next
    End Sub

    Private Sub AfficherUtilisateur()
        Dim c = _utilisateur.Collaborateur
        lblApplication.Text = "GSB · Comptes-rendus"
        lblUtilisateur.Text = c.NomComplet
        lblRattachement.Text = $"{LibellesModules.LibelleProfil(_utilisateur.Profil)} · {_utilisateur.Affectation.LibelleRattachement}"
        lblBienvenue.Text = $"Bonjour {c.Prenom}"
        lblPied.Text = If(c.DerniereConnexion.HasValue,
                          $"Dernière connexion : {c.DerniereConnexion.Value:dddd d MMMM yyyy à HH\hmm}",
                          "Première connexion")
    End Sub

    Private Sub CreerTuilesModules()
        flpModules.Controls.Clear()
        For Each m In Autorisations.ModulesAccessibles(_utilisateur.Profil)
            Dim tuile As New TuileModule(m)
            AddHandler tuile.Click, AddressOf Tuile_Click
            flpModules.Controls.Add(tuile)
        Next
    End Sub

    Private Sub Tuile_Click(sender As Object, e As EventArgs)
        Dim m = CType(sender, TuileModule).ModuleAssocie
        Try
            If OuvrirModule(m) Then Return
        Catch ex As ErreurMetierException
            MessageBox.Show(Me, ex.Message, "GSB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End Try
        ' Les autres modules seront branchés au fur et à mesure de leur développement
        MessageBox.Show(Me, $"Le module « {LibellesModules.Titre(m)} » sera disponible dans une prochaine version.",
                        "GSB", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>Ouvre la fenêtre du module ; renvoie Faux si le module n'est pas encore disponible.</summary>
    Private Function OuvrirModule(m As ModuleApplication) As Boolean
        Select Case m
            Case ModuleApplication.MesComptesRendus
                Using frm As New FrmMesComptesRendus(_fabrique.Rapports(), _utilisateur)
                    frm.ShowDialog(Me)
                End Using
                Return True
            Case ModuleApplication.MonActivite
                Using frm As New FrmMonActivite(_fabrique.Activite(), _fabrique.Rapports(), _utilisateur)
                    frm.ShowDialog(Me)
                End Using
                Return True
            Case ModuleApplication.Praticiens
                Using frm As New FrmPraticiens(_fabrique.Consultation(), _utilisateur)
                    frm.ShowDialog(Me)
                End Using
                Return True
            Case ModuleApplication.Medicaments
                Using frm As New FrmMedicaments(_fabrique.Consultation(), _utilisateur)
                    frm.ShowDialog(Me)
                End Using
                Return True
            Case ModuleApplication.ActiviteRegion, ModuleApplication.ActiviteSecteur
                Using frm As New FrmEquipe(_fabrique.Equipe(), _fabrique.Activite(), _utilisateur)
                    frm.ShowDialog(Me)
                End Using
                Return True
            Case ModuleApplication.Echantillons
                Using frm As New FrmEchantillons(_fabrique.Echantillons(), _utilisateur)
                    frm.ShowDialog(Me)
                End Using
                Return True
        End Select
        Return False
    End Function

    Private Sub btnMotDePasse_Click(sender As Object, e As EventArgs) Handles btnMotDePasse.Click
        Using frm As New FrmChangementMotDePasse(_service, _utilisateur, obligatoire:=False)
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnDeconnexion_Click(sender As Object, e As EventArgs) Handles btnDeconnexion.Click
        Deconnexion = True
        Close()
    End Sub

End Class
