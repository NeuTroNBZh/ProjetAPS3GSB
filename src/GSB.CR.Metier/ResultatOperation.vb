''' <summary>Résultat d'une opération d'administration : erreurs éventuelles et message pour l'utilisateur.</summary>
Public Class ResultatOperation

    Private Sub New(erreurs As IReadOnlyList(Of String), message As String, motDePasse As String)
        Me.Erreurs = erreurs
        Me.Message = message
        MotDePasseProvisoire = motDePasse
    End Sub

    Public ReadOnly Property Erreurs As IReadOnlyList(Of String)

    ''' <summary>Message de confirmation (vide en cas d'échec).</summary>
    Public ReadOnly Property Message As String

    ''' <summary>Mot de passe provisoire à communiquer (création ou réinitialisation), sinon Nothing.</summary>
    Public ReadOnly Property MotDePasseProvisoire As String

    Public ReadOnly Property Reussi As Boolean
        Get
            Return Erreurs.Count = 0
        End Get
    End Property

    Friend Shared Function Succes(message As String, Optional motDePasse As String = Nothing) As ResultatOperation
        Return New ResultatOperation(Array.Empty(Of String)(), message, motDePasse)
    End Function

    Friend Shared Function Echec(erreurs As IEnumerable(Of String)) As ResultatOperation
        Return New ResultatOperation(erreurs.ToList(), "", Nothing)
    End Function

    Friend Shared Function Echec(erreur As String) As ResultatOperation
        Return New ResultatOperation({erreur}, "", Nothing)
    End Function

End Class
