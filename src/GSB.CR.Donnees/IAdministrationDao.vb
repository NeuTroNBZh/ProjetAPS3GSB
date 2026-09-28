Imports GSB.CR.Modeles

''' <summary>
''' Administration (EX-70 à EX-74) : comptes, affectations, portefeuilles, référentiels et journal.
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base
''' (avec le numéro d'erreur Oracle, ex. doublon de matricule ou de login).
''' </summary>
Public Interface IAdministrationDao

    ' --- Organisation
    Function ListerRegions() As List(Of Region)
    Function ListerSecteurs() As List(Of Secteur)

    ' --- Collaborateurs (EX-70, EX-71)

    ''' <summary>Tous les collaborateurs (partis compris) avec affectation en cours et taille du portefeuille.</summary>
    Function ListerCollaborateurs() As List(Of FicheCollaborateur)

    ''' <summary>Historique des affectations, de la plus récente à la plus ancienne.</summary>
    Function ListerAffectations(matricule As String) As List(Of Affectation)

    ''' <summary>Crée le compte (mot de passe à changer) et sa première affectation, en une transaction.</summary>
    Sub CreerCollaborateur(collaborateur As Collaborateur, motDePasseHache As String, affectation As Affectation)

    ''' <summary>Met à jour l'identité, les coordonnées et le login.</summary>
    Sub ModifierCollaborateur(collaborateur As Collaborateur)

    ''' <summary>Clôt l'affectation en cours la veille de <paramref name="dateEffet"/> et ouvre la nouvelle.</summary>
    Sub ChangerAffectation(matricule As String, nouvelle As Affectation, dateEffet As Date)

    ''' <summary>Départ : date de départ, affectation et portefeuille clos à cette date (une transaction).</summary>
    Sub EnregistrerDepart(matricule As String, dateDepart As Date)

    ''' <summary>Nouveau mot de passe provisoire (à changer), compte déverrouillé.</summary>
    Sub ReinitialiserMotDePasse(matricule As String, motDePasseHache As String)

    ''' <summary>Verrouille ou déverrouille le compte ; remet le compteur d'échecs à zéro.</summary>
    Sub DefinirVerrouillage(matricule As String, verrouille As Boolean)

    ' --- Portefeuilles (EX-72)

    ''' <summary>Praticiens actifs qu'aucun visiteur ne suit actuellement.</summary>
    Function ListerPraticiensSansVisiteur() As List(Of PraticienResume)

    ''' <summary>
    ''' Confie des praticiens à un visiteur à partir de <paramref name="dateEffet"/> : le suivi en cours est clos
    ''' la veille (ou remplacé s'il commençait le même jour). Une transaction.
    ''' </summary>
    Sub AttribuerPraticiens(numeros As IEnumerable(Of Integer), matricule As String, dateEffet As Date)

    ''' <summary>Transfère tout le portefeuille d'un visiteur à un autre ; renvoie le nombre de praticiens transférés.</summary>
    Function TransfererPortefeuille(deMatricule As String, versMatricule As String, dateEffet As Date) As Integer

    ' --- Référentiels (EX-73)
    Sub ModifierPraticien(praticien As Praticien)
    Sub ModifierMedicament(depotLegal As String, prixEchantillon As Decimal, actif As Boolean)

    ''' <summary>Crée un motif, placé juste avant « Autre » qui reste en dernier.</summary>
    Sub CreerMotif(code As String, libelle As String)
    Sub ModifierMotif(code As String, libelle As String, actif As Boolean)

    ' --- Médicaments : composition, interactions, posologie (EX-73)

    Function ListerComposants() As List(Of ElementReferentiel)
    Function ListerTypesIndividu() As List(Of ElementReferentiel)
    Function ListerPresentations() As List(Of ElementReferentiel)

    ''' <summary>Dosages, libellé lisible (« 500 mg »), par unité puis quantité.</summary>
    Function ListerDosages() As List(Of ElementReferentiel)

    Sub CreerComposant(code As String, libelle As String)
    Sub CreerDosage(code As String, quantite As Decimal, unite As String)

    Sub AjouterComposition(depotLegal As String, codeComposant As String, quantite As Decimal, unite As String)
    Sub RetirerComposition(depotLegal As String, codeComposant As String)

    ''' <summary>Enregistre que <paramref name="perturbateur"/> perturbe l'effet de <paramref name="perturbe"/>.</summary>
    Sub AjouterInteraction(perturbateur As String, perturbe As String, description As String)
    Sub RetirerInteraction(perturbateur As String, perturbe As String)

    Sub AjouterPosologie(depotLegal As String, codeTypeIndividu As String, codePresentation As String, codeDosage As String, texte As String)
    Sub RetirerPosologie(depotLegal As String, codeTypeIndividu As String, codePresentation As String, codeDosage As String)

    ' --- Journal (EX-74)

    ''' <summary>Tentatives de connexion entre deux dates (incluses), les plus récentes d'abord.</summary>
    Function ListerJournal(debut As Date, fin As Date, texte As String, echecsSeulement As Boolean, maximum As Integer) As List(Of EntreeJournal)

End Interface
