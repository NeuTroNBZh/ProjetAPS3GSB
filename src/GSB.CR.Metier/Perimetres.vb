Imports GSB.CR.Modeles

''' <summary>Périmètre d'équipe d'un utilisateur selon son profil et son affectation.</summary>
Public NotInheritable Class Perimetres

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Délégué : sa région. Responsable : son secteur.
    ''' </summary>
    ''' <exception cref="ErreurMetierException">Profil sans équipe, ou affectation incomplète.</exception>
    Public Shared Function DeLEquipe(utilisateur As UtilisateurConnecte) As Perimetre
        ArgumentNullException.ThrowIfNull(utilisateur)
        Dim a = utilisateur.Affectation
        Select Case utilisateur.Profil
            Case Profil.Delegue
                If String.IsNullOrEmpty(a.CodeRegion) Then Throw New ErreurMetierException("Aucune région n'est associée à votre affectation.")
                Return Perimetre.DeLaRegion(a.CodeRegion, a.NomRegion)
            Case Profil.Responsable
                If String.IsNullOrEmpty(a.CodeSecteur) Then Throw New ErreurMetierException("Aucun secteur n'est associé à votre affectation.")
                Return Perimetre.DuSecteur(a.CodeSecteur, a.LibelleSecteur)
            Case Else
                Throw New ErreurMetierException("Votre profil n'a pas d'équipe à suivre.")
        End Select
    End Function

    ''' <summary>
    ''' Contrôle une période d'analyse : début avant fin, pas plus de 3 ans en arrière (EX-20).
    ''' Renvoie la période avec une fin future ramenée à aujourd'hui.
    ''' </summary>
    Public Shared Function ControlerPeriode(debut As Date, fin As Date, aujourdhui As Date) As (Debut As Date, Fin As Date)
        debut = debut.Date
        fin = If(fin.Date > aujourdhui.Date, aujourdhui.Date, fin.Date)
        Dim limite = aujourdhui.Date.AddYears(-ServiceRapports.AnneesConsultation)
        If debut > fin Then Throw New ErreurMetierException("La date de début doit précéder la date de fin.")
        If debut < limite Then
            Throw New ErreurMetierException($"La consultation est limitée aux trois dernières années (depuis le {limite:dd/MM/yyyy}).")
        End If
        Return (debut, fin)
    End Function

End Class
