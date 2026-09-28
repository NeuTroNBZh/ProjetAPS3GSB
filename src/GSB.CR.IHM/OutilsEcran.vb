''' <summary>
''' Construction par code des barres de boutons et des grilles aux couleurs GSB, partagée par les écrans
''' d'administration (FrmAdministration, FrmDetailsMedicament).
''' </summary>
Public Module OutilsEcran

    Public Function AjouterBouton(barre As FlowLayoutPanel, texte As String, action As EventHandler,
                                  Optional principal As Boolean = False) As Button
        Dim b As New Button() With {.Text = texte, .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowOnly,
                                    .MinimumSize = New Size(0, 36), .Padding = New Padding(10, 0, 10, 0), .Margin = New Padding(0, 0, 8, 6)}
        If principal Then Theme.StyliserBoutonPrincipal(b) Else Theme.StyliserBoutonSecondaire(b)
        AddHandler b.Click, action
        barre.Controls.Add(b)
        Return b
    End Function

    Public Function AjouterEtiquette(barre As FlowLayoutPanel, texte As String) As Label
        Dim l As New Label() With {.Text = texte, .AutoSize = True, .ForeColor = Theme.BleuFonce, .Margin = New Padding(8, 9, 4, 0)}
        barre.Controls.Add(l)
        Return l
    End Function

    ''' <summary>Grille en lecture seule, sélection par ligne, colonnes proportionnelles (poids).</summary>
    Public Sub PreparerGrille(grille As DataGridView, colonnes As IEnumerable(Of (Titre As String, Poids As Integer)),
                              Optional selectionMultiple As Boolean = False)
        grille.ReadOnly = True
        grille.AllowUserToAddRows = False
        grille.AllowUserToDeleteRows = False
        grille.AllowUserToResizeRows = False
        grille.RowHeadersVisible = False
        grille.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grille.MultiSelect = selectionMultiple
        grille.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Theme.StyliserGrille(grille)
        For Each c In colonnes
            grille.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = c.Titre, .FillWeight = c.Poids,
                                                                     .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
    End Sub

    ''' <summary>Objet (Tag) de la ligne sélectionnée, ou Nothing.</summary>
    Public Function Selection(Of T As Class)(grille As DataGridView) As T
        Return If(grille.SelectedRows.Count = 0, Nothing, TryCast(grille.SelectedRows(0).Tag, T))
    End Function

End Module
