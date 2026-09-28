Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Point d'entrée : connexion → (changement de mot de passe) → menu principal,
''' en boucle tant que l'utilisateur se déconnecte au lieu de quitter.
''' </summary>
Friend Module Program

    <STAThread()>
    Friend Sub Main(args As String())
        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        Dim fabrique As FabriqueServices
        Try
            fabrique = FabriqueServices.DepuisConfiguration()
        Catch ex As InvalidOperationException
            MessageBox.Show(ex.Message, "GSB - Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        Dim authentification = fabrique.Authentification()

        Do
            Dim utilisateur As UtilisateurConnecte
            Dim changementRequis As Boolean

            Using connexion As New FrmConnexion(authentification)
                If connexion.ShowDialog() <> DialogResult.OK Then Return
                utilisateur = connexion.Utilisateur
                changementRequis = connexion.ChangementMotDePasseRequis
            End Using

            If changementRequis Then
                Using changement As New FrmChangementMotDePasse(authentification, utilisateur, obligatoire:=True)
                    If changement.ShowDialog() <> DialogResult.OK Then Continue Do
                End Using
            End If

            Using accueil As New FrmAccueil(fabrique, authentification, utilisateur)
                accueil.ShowDialog()
                If Not accueil.Deconnexion Then Return
            End Using
        Loop
    End Sub

End Module
