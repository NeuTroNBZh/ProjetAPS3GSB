Imports GSB.CR.Modeles

''' <summary>Libellés des profils, communs à l'affichage et aux exports.</summary>
Public Module LibellesProfils

    Public Function Libelle(p As Profil) As String
        Select Case p
            Case Profil.Visiteur : Return "Visiteur médical"
            Case Profil.Delegue : Return "Délégué régional"
            Case Profil.Responsable : Return "Responsable de secteur"
            Case Else : Return "Administrateur"
        End Select
    End Function

End Module
