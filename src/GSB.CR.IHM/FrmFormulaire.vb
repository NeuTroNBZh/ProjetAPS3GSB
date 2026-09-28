Imports System.ComponentModel
Imports GSB.CR.Metier

''' <summary>
''' Formulaire de saisie générique aux couleurs GSB : on déclare des champs (texte, liste, date, nombre,
''' case à cocher) puis une validation qui appelle le service. La fenêtre affiche les erreurs renvoyées
''' et ne se ferme (DialogResult.OK) qu'en cas de succès. Utilisé par les dialogues d'administration.
''' </summary>
Public Class FrmFormulaire

    ''' <summary>Largeur des champs, mise à l'échelle de l'écran (les contrôles créés par code ne le sont pas automatiquement).</summary>
    Private ReadOnly Property LargeurChamp As Integer
        Get
            Return CInt(500 * DeviceDpi / 96.0)
        End Get
    End Property

    Private ReadOnly _champs As New Dictionary(Of String, Control)
    Private ReadOnly _libelles As New Dictionary(Of String, Label)

    ''' <summary>Appelée au clic sur le bouton de validation ; renvoie le résultat du service.</summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Validation As Func(Of Task(Of ResultatOperation))

    ''' <summary>Résultat de la validation réussie (message, mot de passe provisoire…).</summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Resultat As ResultatOperation

    Public Sub New(titre As String, Optional sousTitre As String = "", Optional texteBouton As String = "Enregistrer")
        InitializeComponent()
        Text = $"GSB - {titre}"
        lblTitre.Text = titre
        lblSousTitre.Text = sousTitre
        btnValider.Text = texteBouton
        BackColor = Theme.Blanc
        lblTitre.Font = New Font("Segoe UI Semibold", 15.0F)
        lblTitre.ForeColor = Theme.BleuGsb
        lblSousTitre.ForeColor = Theme.TexteGris
        lblErreurs.ForeColor = Theme.Erreur
        pnlBoutons.BackColor = Theme.BleuClair
        Theme.StyliserBoutonPrincipal(btnValider)
        Theme.StyliserBoutonSecondaire(btnAnnuler)
        If String.IsNullOrEmpty(sousTitre) Then lblSousTitre.Visible = False
    End Sub

    ' ------------------------------------------------------------------
    ' Déclaration des champs
    ' ------------------------------------------------------------------

    Private Sub Ajouter(cle As String, libelle As String, controle As Control)
        Dim etiquette As New Label() With {.Text = libelle, .AutoSize = True, .ForeColor = Theme.BleuFonce, .Margin = New Padding(0, 8, 0, 2)}
        controle.Width = LargeurChamp
        controle.Margin = New Padding(0, 0, 0, 2)
        flpChamps.Controls.Add(etiquette)
        flpChamps.Controls.Add(controle)
        _libelles(cle) = etiquette
        _champs(cle) = controle
    End Sub

    Public Function AjouterTexte(cle As String, libelle As String, valeur As String, Optional longueurMax As Integer = 100,
                                 Optional lectureSeule As Boolean = False) As TextBox
        Dim t As New TextBox() With {.Text = If(valeur, ""), .MaxLength = longueurMax, .ReadOnly = lectureSeule}
        Ajouter(cle, libelle, t)
        Return t
    End Function

    Public Function AjouterListe(cle As String, libelle As String, elements As IEnumerable, Optional selection As Object = Nothing) As ComboBox
        Dim c As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        For Each e In elements
            c.Items.Add(e)
        Next
        If selection IsNot Nothing Then
            c.SelectedItem = selection
        ElseIf c.Items.Count > 0 Then
            c.SelectedIndex = 0
        End If
        Ajouter(cle, libelle, c)
        Return c
    End Function

    Public Function AjouterDate(cle As String, libelle As String, valeur As Date) As DateTimePicker
        Dim d As New DateTimePicker() With {.Format = DateTimePickerFormat.Long, .Value = valeur}
        Ajouter(cle, libelle, d)
        Return d
    End Function

    Public Function AjouterNombre(cle As String, libelle As String, valeur As Decimal, minimum As Decimal, maximum As Decimal,
                                  Optional decimales As Integer = 0) As NumericUpDown
        Dim n As New NumericUpDown() With {.Minimum = minimum, .Maximum = maximum, .DecimalPlaces = decimales,
                                           .Value = Math.Min(Math.Max(valeur, minimum), maximum), .TextAlign = HorizontalAlignment.Right,
                                           .ThousandsSeparator = True}
        Ajouter(cle, libelle, n)
        n.Width = CInt(180 * DeviceDpi / 96.0)
        Return n
    End Function

    Public Function AjouterCase(cle As String, libelle As String, valeur As Boolean) As CheckBox
        Dim c As New CheckBox() With {.Text = libelle, .Checked = valeur, .AutoSize = True, .ForeColor = Theme.BleuFonce}
        c.Margin = New Padding(0, 10, 0, 2)
        flpChamps.Controls.Add(c)
        _champs(cle) = c
        Return c
    End Function

    ''' <summary>Paragraphe d'information (grisé) au milieu des champs.</summary>
    Public Sub AjouterNote(texte As String)
        flpChamps.Controls.Add(New Label() With {.Text = texte, .AutoSize = True, .MaximumSize = New Size(LargeurChamp, 0),
                                                 .ForeColor = Theme.TexteGris, .Margin = New Padding(0, 10, 0, 2),
                                                 .Font = New Font("Segoe UI", 9.0F, FontStyle.Italic)})
    End Sub

    ''' <summary>Affiche ou masque un champ et son libellé.</summary>
    Public Sub Afficher(cle As String, visible As Boolean)
        _champs(cle).Visible = visible
        Dim etiquette As Label = Nothing
        If _libelles.TryGetValue(cle, etiquette) Then etiquette.Visible = visible
    End Sub

    ' ------------------------------------------------------------------
    ' Lecture des valeurs
    ' ------------------------------------------------------------------

    Public Function Texte(cle As String) As String
        Return DirectCast(_champs(cle), TextBox).Text
    End Function

    Public Function Selection(Of T As Class)(cle As String) As T
        Return TryCast(DirectCast(_champs(cle), ComboBox).SelectedItem, T)
    End Function

    Public Function DateChoisie(cle As String) As Date
        Return DirectCast(_champs(cle), DateTimePicker).Value.Date
    End Function

    Public Function Nombre(cle As String) As Decimal
        Return DirectCast(_champs(cle), NumericUpDown).Value
    End Function

    Public Function Coche(cle As String) As Boolean
        Return DirectCast(_champs(cle), CheckBox).Checked
    End Function

    ' ------------------------------------------------------------------

    Private Sub FrmFormulaire_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ' Sous-titre sur autant de lignes que nécessaire
        If lblSousTitre.Visible Then
            lblSousTitre.Height = lblSousTitre.GetPreferredSize(New Size(lblSousTitre.Width, 0)).Height
        End If
        ' Hauteur adaptée au nombre de champs (sans dépasser l'écran)
        Dim hauteurChamps = flpChamps.Controls.Cast(Of Control)().Where(Function(c) c.Visible).
            Sum(Function(c) c.Height + c.Margin.Vertical) + flpChamps.Padding.Vertical
        Dim voulu = lblTitre.Height + If(lblSousTitre.Visible, lblSousTitre.Height, 0) + hauteurChamps + lblErreurs.Height + pnlBoutons.Height + 10
        ClientSize = New Size(ClientSize.Width, Math.Min(voulu, Screen.FromControl(Me).WorkingArea.Height - 80))
        CenterToParent()
        flpChamps.Controls.OfType(Of Control)().FirstOrDefault(Function(c) Not TypeOf c Is Label AndAlso c.Enabled AndAlso Not (TypeOf c Is TextBox AndAlso DirectCast(c, TextBox).ReadOnly))?.Focus()
    End Sub

    Private Async Sub btnValider_Click(sender As Object, e As EventArgs) Handles btnValider.Click
        If Validation Is Nothing Then
            DialogResult = DialogResult.OK
            Close()
            Return
        End If
        btnValider.Enabled = False
        UseWaitCursor = True
        Try
            Dim r = Await Validation.Invoke()
            If IsDisposed Then Return
            If Not r.Reussi Then
                lblErreurs.Text = Theme.EnPuces(r.Erreurs)
                Return
            End If
            Resultat = r
            DialogResult = DialogResult.OK
            Close()
        Catch ex As ErreurMetierException
            lblErreurs.Text = ex.Message
        Finally
            UseWaitCursor = False
            If Not IsDisposed Then btnValider.Enabled = True
        End Try
    End Sub

End Class
