Imports GSB.CR.Metier
Imports GSB.CR.Modeles

<TestClass>
Public Class ServiceRapportsTests

    Private Shared ReadOnly Aujourdhui As Date = #2026-09-28#

    Private _rapports As FauxRapportDao
    Private _referentiel As FauxReferentielDao
    Private _service As ServiceRapports
    Private _visiteur As UtilisateurConnecte
    Private _autreVisiteur As UtilisateurConnecte

    <TestInitialize>
    Public Sub Initialiser()
        _rapports = New FauxRapportDao()
        _referentiel = New FauxReferentielDao()
        _service = New ServiceRapports(_rapports, _referentiel, New HorlogeFixe(Aujourdhui))
        _visiteur = Utilisateur("a131", Profil.Visiteur)
        _autreVisiteur = Utilisateur("a17", Profil.Visiteur)
    End Sub

    Private Shared Function Utilisateur(matricule As String, profil As Profil) As UtilisateurConnecte
        Return New UtilisateurConnecte(New Collaborateur() With {.Matricule = matricule},
                                       New Affectation() With {.Profil = profil})
    End Function

    Private Function Saisie() As RapportVisite
        Dim r = _service.NouveauRapport(_visiteur)
        r.NumeroPraticien = 1
        r.DateVisite = Aujourdhui.AddDays(-1)
        r.CodeMotif = "PERIO"
        r.Bilan = "  Bon accueil.  "
        r.CoefConfiance = 4
        Return r
    End Function

    Private ReadOnly Property Debut As DateTime
        Get
            Return Aujourdhui.AddHours(11)
        End Get
    End Property

    ' --- Création ---------------------------------------------------------

    <TestMethod>
    Public Sub NouveauRapport_DateDuJourEtAuteurConnecte()
        Dim r = _service.NouveauRapport(_visiteur)

        Assert.AreEqual(Aujourdhui, r.DateVisite)
        Assert.AreEqual("a131", r.MatriculeAuteur)
        Assert.IsTrue(r.EstNouveau)
    End Sub

    <TestMethod>
    Public Sub Enregistrer_Valide_CreeLeRapportEtTraceLaSaisie()
        Dim r = Saisie()

        Dim resultat = _service.Enregistrer(_visiteur, r, valider:=True, debutSaisie:=Debut)

        Assert.IsTrue(resultat.Reussi, String.Join(" | ", resultat.Erreurs))
        Dim enBase = _rapports.Rapports(resultat.Numero.Value)
        Assert.AreEqual(EtatRapport.Valide, enBase.Etat)
        Assert.AreEqual("Bon accueil.", enBase.Bilan, "Le bilan doit être nettoyé.")
        Assert.AreEqual(resultat.Numero, r.Numero, "Le numéro est reporté sur l'objet saisi.")
        Dim session = _rapports.Sessions.Single()
        Assert.AreEqual(Debut, session.Debut)
        Assert.AreEqual("a131", session.Matricule)
    End Sub

    <TestMethod>
    Public Sub EX10_Enregistrer_AuteurToujoursUtilisateurConnecte()
        Dim r = Saisie()
        r.MatriculeAuteur = "a17"   ' tentative de saisie au nom d'un autre

        Dim resultat = _service.Enregistrer(_visiteur, r, valider:=True, debutSaisie:=Debut)

        Assert.AreEqual("a131", _rapports.Rapports(resultat.Numero.Value).MatriculeAuteur)
    End Sub

    <TestMethod>
    Public Sub Enregistrer_Brouillon_IncompletAccepte()
        Dim r = _service.NouveauRapport(_visiteur)
        r.NumeroPraticien = 1

        Dim resultat = _service.Enregistrer(_visiteur, r, valider:=False, debutSaisie:=Debut)

        Assert.IsTrue(resultat.Reussi)
        Assert.AreEqual(EtatRapport.Brouillon, _rapports.Rapports(resultat.Numero.Value).Etat)
    End Sub

    <TestMethod>
    Public Sub Enregistrer_ValiderIncomplet_RefuseSansRienEcrire()
        Dim r = _service.NouveauRapport(_visiteur)
        r.NumeroPraticien = 1

        Dim resultat = _service.Enregistrer(_visiteur, r, valider:=True, debutSaisie:=Debut)

        Assert.IsFalse(resultat.Reussi)
        Assert.IsEmpty(_rapports.Rapports)
        Assert.IsEmpty(_rapports.Sessions)
    End Sub

    <TestMethod>
    Public Sub Enregistrer_PrecisionEffaceeSiMotifStandard()
        Dim r = Saisie()
        r.PrecisionMotif = "reste d'une saisie précédente"

        Dim resultat = _service.Enregistrer(_visiteur, r, valider:=True, debutSaisie:=Debut)

        Assert.IsNull(_rapports.Rapports(resultat.Numero.Value).PrecisionMotif)
    End Sub

    ' --- Modification ------------------------------------------------------

    <TestMethod>
    Public Sub EX18_Modifier_UnBrouillonPuisLeValider()
        Dim r = _service.NouveauRapport(_visiteur)
        r.NumeroPraticien = 1
        Dim numero = _service.Enregistrer(_visiteur, r, valider:=False, debutSaisie:=Debut).Numero.Value

        Dim charge = _service.Charger(_visiteur, numero)
        charge.CodeMotif = "PERIO" : charge.Bilan = "Complété" : charge.CoefConfiance = 3
        Dim resultat = _service.Enregistrer(_visiteur, charge, valider:=True, debutSaisie:=Debut)

        Assert.IsTrue(resultat.Reussi)
        Assert.AreEqual(numero, resultat.Numero)
        Assert.AreEqual(EtatRapport.Valide, _rapports.Rapports(numero).Etat)
        Assert.HasCount(2, _rapports.Sessions)
    End Sub

    <TestMethod>
    Public Sub EX29_ValideNePeutPasRepasserEnBrouillon()
        Dim numero = _service.Enregistrer(_visiteur, Saisie(), valider:=True, debutSaisie:=Debut).Numero.Value
        Dim charge = _service.Charger(_visiteur, numero)

        Dim resultat = _service.Enregistrer(_visiteur, charge, valider:=False, debutSaisie:=Debut)

        Assert.IsFalse(resultat.Reussi)
        StringAssert.Contains(resultat.Erreurs(0), "brouillon")
        Assert.AreEqual(EtatRapport.Valide, _rapports.Rapports(numero).Etat)
    End Sub

    <TestMethod>
    Public Sub Charger_RapportDUnAutreVisiteur_Refuse()
        Dim numero = _service.Enregistrer(_autreVisiteur, Saisie(), valider:=True, debutSaisie:=Debut).Numero.Value

        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.Charger(_visiteur, numero))
    End Sub

    <TestMethod>
    Public Sub Enregistrer_RapportDUnAutreVisiteur_Refuse()
        Dim numero = _service.Enregistrer(_autreVisiteur, Saisie(), valider:=True, debutSaisie:=Debut).Numero.Value
        Dim pirate = Saisie()
        pirate.Numero = numero

        Dim resultat = _service.Enregistrer(_visiteur, pirate, valider:=True, debutSaisie:=Debut)

        Assert.IsFalse(resultat.Reussi)
        Assert.AreEqual("a17", _rapports.Rapports(numero).MatriculeAuteur)
    End Sub

    ' --- Suppression --------------------------------------------------------

    <TestMethod>
    Public Sub EX29_SupprimerBrouillon_Accepte_Valide_Refuse()
        Dim brouillon = Saisie()
        Dim numBrouillon = _service.Enregistrer(_visiteur, brouillon, valider:=False, debutSaisie:=Debut).Numero.Value
        Dim numValide = _service.Enregistrer(_visiteur, Saisie(), valider:=True, debutSaisie:=Debut).Numero.Value

        _service.SupprimerBrouillon(_visiteur, numBrouillon)

        Assert.IsFalse(_rapports.Rapports.ContainsKey(numBrouillon))
        Assert.ThrowsExactly(Of ErreurMetierException)(Sub() _service.SupprimerBrouillon(_visiteur, numValide))
        Assert.IsTrue(_rapports.Rapports.ContainsKey(numValide))
    End Sub

    ' --- Consultation, droits, pannes -----------------------------------------

    <TestMethod>
    Public Sub EX20_MesRapports_TroisDernieresAnnees()
        _service.MesRapports(_visiteur)

        Assert.AreEqual(Aujourdhui.AddYears(-3), _rapports.DernierDepuis)
    End Sub

    <TestMethod>
    Public Sub Delegue_PeutSaisir_Responsable_Non()
        Dim delegue = Utilisateur("d01", Profil.Delegue)
        Dim responsable = Utilisateur("r01", Profil.Responsable)

        Assert.IsTrue(_service.Enregistrer(delegue, Saisie(), valider:=True, debutSaisie:=Debut).Reussi)
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.MesRapports(responsable))
    End Sub

    <TestMethod>
    Public Sub BaseEnPanne_MessageServeurSansException()
        _rapports.EnPanne = True

        Dim resultat = _service.Enregistrer(_visiteur, Saisie(), valider:=True, debutSaisie:=Debut)

        Assert.IsFalse(resultat.Reussi)
        StringAssert.Contains(resultat.Erreurs(0), "serveur")
        Assert.ThrowsExactly(Of ErreurMetierException)(Function() _service.MesRapports(_visiteur))
    End Sub

    ' --- Remplaçants --------------------------------------------------------

    <TestMethod>
    Public Sub CreerRemplacant_Valide_NumeroAttribue()
        Dim p As New Praticien() With {.Nom = " Petit ", .Prenom = "Camille", .CodeType = "MV"}

        Dim erreurs = _service.CreerRemplacant(p)

        Assert.IsEmpty(erreurs)
        Assert.IsTrue(p.Numero.HasValue)
        Assert.AreEqual("Petit", p.Nom)
    End Sub

    <TestMethod>
    Public Sub CreerRemplacant_SansNomNiType_Refuse()
        Dim erreurs = _service.CreerRemplacant(New Praticien() With {.Prenom = "Camille"})

        Assert.HasCount(2, erreurs)
        Assert.IsEmpty(_referentiel.Praticiens)
    End Sub

End Class
