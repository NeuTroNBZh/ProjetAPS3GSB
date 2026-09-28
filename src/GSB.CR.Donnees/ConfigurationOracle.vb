Imports Microsoft.Extensions.Configuration
Imports Oracle.ManagedDataAccess.Client

''' <summary>
''' Paramètres de connexion au serveur Oracle 19c.
''' Lus depuis appsettings.json (serveur) et appsettings.Local.json (identifiants, non versionné).
''' </summary>
Public Class ConfigurationOracle

    Private Const NomFichierPublic As String = "appsettings.json"
    Private Const NomFichierLocal As String = "appsettings.Local.json"

    ''' <summary>Adresse du serveur Oracle.</summary>
    Public Property Hote As String = ""

    ''' <summary>Port du listener Oracle (1521 par défaut).</summary>
    Public Property Port As Integer = 1521

    ''' <summary>Nom de service Oracle (ex. FREEPDB1).</summary>
    Public Property Service As String = ""

    ''' <summary>Utilisateur (schéma) Oracle de l'application.</summary>
    Public Property Utilisateur As String = ""

    ''' <summary>Mot de passe de l'utilisateur Oracle.</summary>
    Public Property MotDePasse As String = ""

    ''' <summary>
    ''' Charge la configuration depuis le dossier indiqué (par défaut celui de l'exécutable).
    ''' </summary>
    Public Shared Function Charger(Optional dossier As String = Nothing) As ConfigurationOracle
        Dim racine = New ConfigurationBuilder() _
            .SetBasePath(If(dossier, AppContext.BaseDirectory)) _
            .AddJsonFile(NomFichierPublic, optional:=True) _
            .AddJsonFile(NomFichierLocal, optional:=True) _
            .Build()

        Dim config As New ConfigurationOracle()
        racine.GetSection("Oracle").Bind(config)
        Return config
    End Function

    ''' <summary>Indique si tous les paramètres nécessaires à la connexion sont renseignés.</summary>
    Public ReadOnly Property EstComplete As Boolean
        Get
            Return Not String.IsNullOrWhiteSpace(Hote) AndAlso
                   Not String.IsNullOrWhiteSpace(Service) AndAlso
                   Not String.IsNullOrWhiteSpace(Utilisateur) AndAlso
                   Not String.IsNullOrWhiteSpace(MotDePasse) AndAlso
                   Port > 0
        End Get
    End Property

    ''' <summary>Construit la chaîne de connexion ODP.NET (format EZConnect).</summary>
    ''' <exception cref="InvalidOperationException">Si la configuration est incomplète.</exception>
    Public Function ChaineDeConnexion() As String
        If Not EstComplete Then
            Throw New InvalidOperationException(
                $"Configuration Oracle incomplète : renseigner Hote, Port, Service, Utilisateur et MotDePasse (voir {NomFichierLocal}).")
        End If

        Dim constructeur As New OracleConnectionStringBuilder() With {
            .DataSource = $"{Hote}:{Port}/{Service}",
            .UserID = Utilisateur,
            .Password = MotDePasse
        }
        Return constructeur.ConnectionString
    End Function

End Class
