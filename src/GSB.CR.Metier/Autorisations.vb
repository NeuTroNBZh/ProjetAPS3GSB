Imports GSB.CR.Modeles

''' <summary>
''' Droits d'accès aux modules selon le profil (EX-02).
''' Le délégué est aussi un visiteur : il a les modules du visiteur en plus des siens.
''' </summary>
Public NotInheritable Class Autorisations

    Private Sub New()
    End Sub

    ''' <summary>Modules accessibles pour un profil, dans l'ordre d'affichage du menu.</summary>
    Public Shared Function ModulesAccessibles(profil As Profil) As IReadOnlyList(Of ModuleApplication)
        Select Case profil
            Case Profil.Visiteur
                Return {ModuleApplication.MesComptesRendus, ModuleApplication.Praticiens, ModuleApplication.Medicaments,
                        ModuleApplication.MonActivite, ModuleApplication.Messagerie}
            Case Profil.Delegue
                Return {ModuleApplication.MesComptesRendus, ModuleApplication.Praticiens, ModuleApplication.Medicaments,
                        ModuleApplication.MonActivite, ModuleApplication.ActiviteRegion, ModuleApplication.Echantillons,
                        ModuleApplication.Messagerie}
            Case Profil.Responsable
                Return {ModuleApplication.ActiviteSecteur, ModuleApplication.Praticiens, ModuleApplication.Medicaments,
                        ModuleApplication.Echantillons, ModuleApplication.Messagerie}
            Case Else
                Return {ModuleApplication.Administration, ModuleApplication.Praticiens, ModuleApplication.Medicaments,
                        ModuleApplication.Messagerie}
        End Select
    End Function

    ''' <summary>Indique si un profil a accès à un module.</summary>
    Public Shared Function PeutAcceder(profil As Profil, moduleDemande As ModuleApplication) As Boolean
        Return ModulesAccessibles(profil).Contains(moduleDemande)
    End Function

End Class
