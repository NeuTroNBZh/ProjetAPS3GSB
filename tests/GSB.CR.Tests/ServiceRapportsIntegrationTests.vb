Imports GSB.CR.Donnees
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Parcours complet d'un compte-rendu sur la base réelle : brouillon, validation, refus du retour
''' en brouillon, suppression d'un brouillon. Les données créées sont supprimées. Exclus de la CI.
''' </summary>
<TestClass>
<TestCategory("Integration")>
<DoNotParallelize>
Public Class ServiceRapportsIntegrationTests

    Private _connexion As ConnexionOracle
    Private _service As ServiceRapports
    Private ReadOnly _visiteur As New UtilisateurConnecte(New Collaborateur() With {.Matricule = "a17"},
                                                          New Affectation() With {.Profil = Profil.Visiteur})
    Private ReadOnly _crees As New List(Of Integer)

    <TestInitialize>
    Public Sub Initialiser()
        Dim config = ConfigurationOracle.Charger()
        If Not config.EstComplete Then Assert.Inconclusive("appsettings.Local.json absent ou incomplet.")
        _connexion = New ConnexionOracle(config)
        _service = New ServiceRapports(New RapportDao(_connexion), New ReferentielDao(_connexion), TimeProvider.System)
    End Sub

    <TestCleanup>
    Public Sub Nettoyer()
        If _connexion Is Nothing Then Return
        Using cnx = _connexion.Ouvrir()
            For Each numero In _crees
                Using cmd As New Oracle.ManagedDataAccess.Client.OracleCommand("delete from RAPPORT_VISITE where rap_num = :n", cnx)
                    cmd.Parameters.Add("n", numero)
                    cmd.ExecuteNonQuery()
                End Using
            Next
        End Using
    End Sub

    <TestMethod>
    Public Sub Parcours_BrouillonPuisValidation()
        Dim r = _service.NouveauRapport(_visiteur)
        r.NumeroPraticien = _service.Portefeuille(_visiteur).First().Numero
        r.ProduitsPresentes.Add("TROXT21")
        Dim debut = DateTime.Now.AddMinutes(-4)

        Dim brouillon = _service.Enregistrer(_visiteur, r, valider:=False, debutSaisie:=debut)
        Assert.IsTrue(brouillon.Reussi, String.Join(" | ", brouillon.Erreurs))
        _crees.Add(brouillon.Numero.Value)
        Assert.IsTrue(_service.MesRapports(_visiteur).Any(Function(x) x.Numero = brouillon.Numero.Value AndAlso x.Etat = EtatRapport.Brouillon))

        Dim charge = _service.Charger(_visiteur, brouillon.Numero.Value)
        charge.CodeMotif = "AUTRE" : charge.PrecisionMotif = "Test d'intégration"
        charge.Bilan = "Validation de bout en bout" : charge.CoefConfiance = 2
        charge.Echantillons.Add(New EchantillonOffert() With {.DepotLegal = "TROXT21", .Quantite = 1})
        Dim valide = _service.Enregistrer(_visiteur, charge, valider:=True, debutSaisie:=debut)
        Assert.IsTrue(valide.Reussi, String.Join(" | ", valide.Erreurs))

        Dim relu = _service.Charger(_visiteur, brouillon.Numero.Value)
        Assert.AreEqual(EtatRapport.Valide, relu.Etat)
        Assert.AreEqual("Test d'intégration", relu.PrecisionMotif)
        Assert.IsNotNull(relu.DateValidation)

        Dim retour = _service.Enregistrer(_visiteur, relu, valider:=False, debutSaisie:=debut)
        Assert.IsFalse(retour.Reussi, "Un CR validé ne doit pas repasser en brouillon.")
        Assert.ThrowsExactly(Of ErreurMetierException)(Sub() _service.SupprimerBrouillon(_visiteur, relu.Numero.Value))
    End Sub

    <TestMethod>
    Public Sub SupprimerBrouillon_DisparaitDeLaListe()
        Dim r = _service.NouveauRapport(_visiteur)
        r.NumeroPraticien = _service.Portefeuille(_visiteur).First().Numero
        Dim numero = _service.Enregistrer(_visiteur, r, valider:=False, debutSaisie:=DateTime.Now).Numero.Value
        _crees.Add(numero)

        _service.SupprimerBrouillon(_visiteur, numero)

        Assert.IsFalse(_service.MesRapports(_visiteur).Any(Function(x) x.Numero = numero))
    End Sub

End Class
