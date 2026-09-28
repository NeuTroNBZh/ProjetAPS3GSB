''' <summary>
''' Périmètre d'analyse : un collaborateur, une région ou un secteur.
''' Pour une région ou un secteur, les membres sont les visiteurs et délégués qui y sont affectés actuellement.
''' </summary>
Public Class Perimetre

    Private Sub New(type As TypePerimetre, code As String, libelle As String)
        Me.Type = type
        Me.Code = code
        Me.Libelle = libelle
    End Sub

    Public ReadOnly Property Type As TypePerimetre

    ''' <summary>Matricule, code région ou code secteur selon le type.</summary>
    Public ReadOnly Property Code As String

    Public ReadOnly Property Libelle As String

    Public Shared Function DuCollaborateur(matricule As String, Optional nom As String = Nothing) As Perimetre
        Return New Perimetre(TypePerimetre.Collaborateur, matricule, If(nom, matricule))
    End Function

    Public Shared Function DeLaRegion(code As String, nom As String) As Perimetre
        Return New Perimetre(TypePerimetre.Region, code, $"Région {nom}")
    End Function

    Public Shared Function DuSecteur(code As String, libelle As String) As Perimetre
        Return New Perimetre(TypePerimetre.Secteur, code, $"Secteur {libelle}")
    End Function

    Public Overrides Function ToString() As String
        Return Libelle
    End Function

End Class
