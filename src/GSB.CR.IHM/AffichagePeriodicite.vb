Imports GSB.CR.Modeles

''' <summary>Libellés et couleurs des états de périodicité des visites.</summary>
Public Module AffichagePeriodicite

    Public Function Libelle(etat As EtatPeriodicite) As String
        Select Case etat
            Case EtatPeriodicite.AJour : Return "À jour"
            Case EtatPeriodicite.ARevoirBientot : Return "À revoir bientôt"
            Case EtatPeriodicite.ARevoir : Return "À revoir"
            Case Else : Return "Jamais visité"
        End Select
    End Function

    ''' <summary>Couleurs (fond, texte) du badge d'un état.</summary>
    Public Function Couleurs(etat As EtatPeriodicite) As (Fond As Color, Encre As Color)
        Select Case etat
            Case EtatPeriodicite.AJour : Return (Color.FromArgb(232, 245, 233), Color.FromArgb(46, 125, 50))
            Case EtatPeriodicite.ARevoirBientot : Return (Color.FromArgb(255, 244, 219), Color.FromArgb(138, 90, 0))
            Case EtatPeriodicite.ARevoir : Return (Color.FromArgb(253, 236, 234), Color.FromArgb(179, 38, 30))
            Case Else : Return (Color.FromArgb(236, 239, 243), Theme.TexteGris)
        End Select
    End Function

End Module
