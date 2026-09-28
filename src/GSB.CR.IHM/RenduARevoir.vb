Imports GSB.CR.Modeles

''' <summary>Tableau des praticiens à revoir (visiteur ou équipe), avec la situation en couleur.</summary>
Public Module RenduARevoir

    Private Const PrevueDepassee As String = "Visite prévue dépassée"

    ''' <param name="avecSuivi">Ajoute la colonne « Suivi par » (vue d'équipe).</param>
    Public Sub CreerColonnes(grille As DataGridView, avecSuivi As Boolean)
        Dim colonnes As New List(Of (String, String, Integer)) From {
            ("Situation", "Situation", 16), ("Praticien", "Praticien", 20), ("Ville", "Ville", 13)}
        If avecSuivi Then colonnes.Add(("Suivi", "Suivi par", 16))
        colonnes.AddRange({("Derniere", "Dernière visite", 12), ("Depuis", "Il y a", 8), ("Prevue", "Prochaine prévue", 12), ("Tel", "Téléphone", 12)})
        For Each c In colonnes
            grille.Columns.Add(New DataGridViewTextBoxColumn() With {
                .Name = c.Item1, .HeaderText = c.Item2, .FillWeight = c.Item3, .SortMode = DataGridViewColumnSortMode.NotSortable})
        Next
    End Sub

    Public Sub Remplir(grille As DataGridView, liste As IEnumerable(Of PraticienARevoir), avecSuivi As Boolean)
        grille.Rows.Clear()
        For Each x In liste
            Dim depassee = x.ProchainePrevueDepassee AndAlso x.Etat <> EtatPeriodicite.ARevoir AndAlso x.Etat <> EtatPeriodicite.JamaisVisite
            Dim valeurs As New List(Of Object) From {If(depassee, PrevueDepassee, AffichagePeriodicite.Libelle(x.Etat)),
                                                     x.Praticien.NomComplet, x.Praticien.Ville}
            If avecSuivi Then valeurs.Add(x.Praticien.NomVisiteur)
            valeurs.AddRange({x.Praticien.DateDerniereVisite?.ToString("dd/MM/yyyy"),
                              If(x.JoursDepuisDerniereVisite.HasValue, $"{x.JoursDepuisDerniereVisite} j", ""),
                              x.Praticien.DateProchainePrevue?.ToString("dd/MM/yyyy"), x.Praticien.Telephone})
            Dim ligne = grille.Rows(grille.Rows.Add(valeurs.ToArray()))
            ligne.Tag = x
            Dim couleurs = AffichagePeriodicite.Couleurs(If(depassee, EtatPeriodicite.ARevoirBientot, x.Etat))
            ligne.Cells("Situation").Style.BackColor = couleurs.Fond
            ligne.Cells("Situation").Style.ForeColor = couleurs.Encre
            ligne.Cells("Situation").Style.SelectionBackColor = couleurs.Fond
            ligne.Cells("Situation").Style.SelectionForeColor = couleurs.Encre
            ligne.Cells("Situation").Style.Font = New Font(grille.Font, FontStyle.Bold)
        Next
    End Sub

End Module
