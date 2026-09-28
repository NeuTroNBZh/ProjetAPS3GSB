Imports GSB.CR.Metier

''' <summary>Textes affichés pour chaque module dans le menu principal.</summary>
Public Module LibellesModules

    Public Function Titre(m As ModuleApplication) As String
        Select Case m
            Case ModuleApplication.MesComptesRendus : Return "Mes comptes-rendus"
            Case ModuleApplication.Praticiens : Return "Praticiens"
            Case ModuleApplication.Medicaments : Return "Médicaments"
            Case ModuleApplication.MonActivite : Return "Mon activité"
            Case ModuleApplication.ActiviteRegion : Return "Ma région"
            Case ModuleApplication.Echantillons : Return "Échantillons"
            Case ModuleApplication.ActiviteSecteur : Return "Mon secteur"
            Case ModuleApplication.Messagerie : Return "Messagerie"
            Case Else : Return "Administration"
        End Select
    End Function

    Public Function Description(m As ModuleApplication) As String
        Select Case m
            Case ModuleApplication.MesComptesRendus : Return "Saisir et consulter mes CR de visite"
            Case ModuleApplication.Praticiens : Return "Coordonnées et informations des praticiens"
            Case ModuleApplication.Medicaments : Return "Composition, effets, posologie"
            Case ModuleApplication.MonActivite : Return "Visites et statistiques sur une période"
            Case ModuleApplication.ActiviteRegion : Return "Activité des visiteurs de ma région"
            Case ModuleApplication.Echantillons : Return "Dotations et contrôle de stock"
            Case ModuleApplication.ActiviteSecteur : Return "Activité des régions de mon secteur"
            Case ModuleApplication.Messagerie : Return "Messages entre collaborateurs"
            Case Else : Return "Comptes, affectations, référentiels"
        End Select
    End Function

    Public Function LibelleProfil(p As Modeles.Profil) As String
        Return LibellesProfils.Libelle(p)
    End Function

End Module
