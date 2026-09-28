''' <summary>Résultat de l'enregistrement d'un compte-rendu : numéro attribué ou liste des erreurs.</summary>
Public Class ResultatEnregistrement

    Private Sub New(numero As Integer?, erreurs As IReadOnlyList(Of String))
        Me.Numero = numero
        Me.Erreurs = erreurs
    End Sub

    ''' <summary>Numéro du rapport enregistré (Nothing en cas d'échec).</summary>
    Public ReadOnly Property Numero As Integer?

    ''' <summary>Erreurs à corriger (vide si l'enregistrement a réussi).</summary>
    Public ReadOnly Property Erreurs As IReadOnlyList(Of String)

    Public ReadOnly Property Reussi As Boolean
        Get
            Return Erreurs.Count = 0
        End Get
    End Property

    Friend Shared Function Succes(numero As Integer) As ResultatEnregistrement
        Return New ResultatEnregistrement(numero, Array.Empty(Of String)())
    End Function

    Friend Shared Function Echec(erreurs As IReadOnlyList(Of String)) As ResultatEnregistrement
        Return New ResultatEnregistrement(Nothing, erreurs)
    End Function

    Friend Shared Function Echec(erreur As String) As ResultatEnregistrement
        Return New ResultatEnregistrement(Nothing, {erreur})
    End Function

End Class
