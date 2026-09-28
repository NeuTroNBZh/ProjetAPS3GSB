Imports GSB.CR.Donnees

''' <summary>
''' Point d'entrée de la couche Métier pour l'IHM : crée les services branchés sur la base Oracle
''' à partir de la configuration (appsettings.json + appsettings.Local.json).
''' L'IHM n'a ainsi jamais besoin de connaître la couche Données.
''' </summary>
Public Class FabriqueServices

    Private ReadOnly _connexion As ConnexionOracle

    Private Sub New(connexion As ConnexionOracle)
        _connexion = connexion
    End Sub

    ''' <summary>
    ''' Crée la fabrique à partir des fichiers de configuration du dossier de l'application.
    ''' </summary>
    ''' <exception cref="InvalidOperationException">Si la configuration Oracle est incomplète.</exception>
    Public Shared Function DepuisConfiguration() As FabriqueServices
        Return New FabriqueServices(New ConnexionOracle(ConfigurationOracle.Charger()))
    End Function

    Public Function Authentification() As ServiceAuthentification
        Return New ServiceAuthentification(New CollaborateurDao(_connexion), TimeProvider.System)
    End Function

    Public Function Rapports() As ServiceRapports
        Return New ServiceRapports(New RapportDao(_connexion), New ReferentielDao(_connexion), TimeProvider.System)
    End Function

End Class
