Imports System.ComponentModel
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Création de la fiche d'un remplaçant rencontré lors d'une visite (EX-16, EX-28).
''' Renvoie DialogResult.OK et le praticien créé.
''' </summary>
Public Class FrmNouveauRemplacant

    Private ReadOnly _service As ServiceRapports

    ''' <summary>Praticien créé (renseigné quand la fenêtre renvoie OK).</summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Praticien As Praticien

    Public Sub New(service As ServiceRapports)
        InitializeComponent()
        _service = service
        BackColor = Theme.Blanc
        lblTitre.Font = Theme.PoliceTitre
        lblTitre.ForeColor = Theme.BleuGsb
        lblInformation.ForeColor = Theme.TexteGris
        lblErreurs.ForeColor = Theme.Erreur
        For Each l In {lblNom, lblPrenom, lblType, lblTelephone, lblEmail}
            l.ForeColor = Theme.BleuFonce
        Next
        Theme.StyliserBoutonPrincipal(btnValider)
        Theme.StyliserBoutonSecondaire(btnAnnuler)
    End Sub

    Private Async Sub FrmNouveauRemplacant_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim types = Await Task.Run(Function() _service.TypesPraticien())
            cboType.Items.AddRange(types.ToArray())
            ' Un remplaçant est le plus souvent un médecin de ville
            Dim parDefaut = types.FirstOrDefault(Function(t) t.Code = "MV")
            If parDefaut IsNot Nothing Then cboType.SelectedItem = parDefaut
        Catch ex As ErreurMetierException
            lblErreurs.Text = ex.Message
            btnValider.Enabled = False
        End Try
    End Sub

    Private Async Sub btnValider_Click(sender As Object, e As EventArgs) Handles btnValider.Click
        Dim nouveau As New Praticien() With {
            .Nom = txtNom.Text,
            .Prenom = txtPrenom.Text,
            .CodeType = If(TryCast(cboType.SelectedItem, TypePraticien)?.Code, ""),
            .Telephone = If(String.IsNullOrWhiteSpace(txtTelephone.Text), Nothing, txtTelephone.Text.Trim()),
            .Email = If(String.IsNullOrWhiteSpace(txtEmail.Text), Nothing, txtEmail.Text.Trim())
        }

        btnValider.Enabled = False
        Try
            Dim erreurs = Await Task.Run(Function() _service.CreerRemplacant(nouveau))
            If erreurs.Count > 0 Then
                lblErreurs.Text = Theme.EnPuces(erreurs)
                Return
            End If
            Praticien = nouveau
            DialogResult = DialogResult.OK
            Close()
        Catch ex As ErreurMetierException
            lblErreurs.Text = ex.Message
        Finally
            btnValider.Enabled = True
        End Try
    End Sub

End Class
