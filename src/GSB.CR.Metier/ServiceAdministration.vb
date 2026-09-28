Imports System.Text.RegularExpressions
Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Module Administration (EX-70 à EX-74), réservé au profil administrateur : comptes, affectations,
''' portefeuilles, référentiels et journal des connexions.
''' </summary>
Public Class ServiceAdministration

    Public Const MaxLignesJournal As Integer = 1000

    Private Const MessageDoublon As String =
        "Opération refusée : valeur déjà utilisée (matricule, login ou code), ou poste déjà occupé " &
        "(un seul délégué par région et un seul responsable par secteur)."

    Private Shared ReadOnly FormatMatricule As New Regex("^[a-z0-9]{1,10}$")
    Private Shared ReadOnly FormatLogin As New Regex("^[a-z0-9._-]{3,30}$")
    Private Shared ReadOnly FormatCodePostal As New Regex("^[0-9]{5}$")
    Private Shared ReadOnly FormatEmail As New Regex("^[^@\s]+@[^@\s]+\.[^@\s]+$")
    Private Shared ReadOnly FormatCodeMotif As New Regex("^[A-Z]{2,6}$")

    Private ReadOnly _admin As IAdministrationDao
    Private ReadOnly _referentiel As IReferentielDao
    Private ReadOnly _consultation As IConsultationDao
    Private ReadOnly _horloge As TimeProvider

    Public Sub New(admin As IAdministrationDao, referentiel As IReferentielDao, consultation As IConsultationDao, horloge As TimeProvider)
        ArgumentNullException.ThrowIfNull(admin)
        ArgumentNullException.ThrowIfNull(referentiel)
        ArgumentNullException.ThrowIfNull(consultation)
        ArgumentNullException.ThrowIfNull(horloge)
        _admin = admin
        _referentiel = referentiel
        _consultation = consultation
        _horloge = horloge
    End Sub

    Public ReadOnly Property Aujourdhui As Date
        Get
            Return _horloge.GetLocalNow().Date
        End Get
    End Property

    ' ------------------------------------------------------------------
    ' Lectures
    ' ------------------------------------------------------------------

    Public Function Collaborateurs(u As UtilisateurConnecte) As IReadOnlyList(Of FicheCollaborateur)
        VerifierAcces(u)
        Return Appeler(Function() _admin.ListerCollaborateurs())
    End Function

    Public Function Affectations(u As UtilisateurConnecte, matricule As String) As IReadOnlyList(Of Affectation)
        VerifierAcces(u)
        Return Appeler(Function() _admin.ListerAffectations(matricule))
    End Function

    Public Function Regions(u As UtilisateurConnecte) As IReadOnlyList(Of Region)
        VerifierAcces(u)
        Return Appeler(Function() _admin.ListerRegions())
    End Function

    Public Function Secteurs(u As UtilisateurConnecte) As IReadOnlyList(Of Secteur)
        VerifierAcces(u)
        Return Appeler(Function() _admin.ListerSecteurs())
    End Function

    ' ------------------------------------------------------------------
    ' Collaborateurs (EX-70, EX-71)
    ' ------------------------------------------------------------------

    ''' <summary>Crée un compte avec un mot de passe provisoire (à changer à la première connexion).</summary>
    Public Function CreerCollaborateur(u As UtilisateurConnecte, c As Collaborateur, affectation As Affectation) As ResultatOperation
        VerifierAcces(u)
        ArgumentNullException.ThrowIfNull(c)
        Normaliser(c)
        Dim erreurs = ControlerIdentite(c)
        If Not FormatMatricule.IsMatch(c.Matricule) Then erreurs.Add("Le matricule est obligatoire : 1 à 10 lettres minuscules ou chiffres.")
        If c.DateEmbauche.Date > Aujourdhui Then erreurs.Add("La date d'embauche ne peut pas être dans le futur.")
        erreurs.AddRange(ControlerAffectation(affectation))
        If erreurs.Count > 0 Then Return ResultatOperation.Echec(erreurs)

        Dim motDePasse = GenerateurMotDePasse.Generer()
        Return Executer(Sub() _admin.CreerCollaborateur(c, HacheurMotDePasse.Hacher(motDePasse), affectation),
                        $"Compte {c.Login} créé. Communiquez-lui son mot de passe provisoire : il devra le changer à la première connexion.",
                        motDePasse)
    End Function

    Public Function ModifierCollaborateur(u As UtilisateurConnecte, c As Collaborateur) As ResultatOperation
        VerifierAcces(u)
        ArgumentNullException.ThrowIfNull(c)
        Normaliser(c)
        Dim erreurs = ControlerIdentite(c)
        If erreurs.Count > 0 Then Return ResultatOperation.Echec(erreurs)
        Return Executer(Sub() _admin.ModifierCollaborateur(c), $"Fiche de {c.NomComplet} enregistrée.")
    End Function

    ''' <summary>Nouvelle affectation à partir de <paramref name="dateEffet"/> ; l'historique est conservé (EX-71).</summary>
    Public Function ChangerAffectation(u As UtilisateurConnecte, matricule As String, nouvelle As Affectation, dateEffet As Date) As ResultatOperation
        VerifierAcces(u)
        Dim fiche = TrouverFiche(matricule)
        Dim erreurs = ControlerAffectation(nouvelle)
        If fiche.AffectationEnCours Is Nothing Then Return ResultatOperation.Echec("Ce collaborateur n'a plus d'affectation (départ enregistré).")
        If matricule = u.Matricule AndAlso nouvelle.Profil <> Profil.Administrateur Then
            erreurs.Add("Vous ne pouvez pas retirer votre propre profil administrateur.")
        End If
        If dateEffet.Date <= fiche.AffectationEnCours.DateDebut.Date Then
            erreurs.Add($"La nouvelle affectation doit commencer après le {fiche.AffectationEnCours.DateDebut:dd/MM/yyyy} (début de l'affectation actuelle).")
        End If
        If dateEffet.Date > Aujourdhui.AddYears(1) Then erreurs.Add("La date d'effet est trop lointaine.")
        If erreurs.Count = 0 AndAlso MemeAffectation(fiche.AffectationEnCours, nouvelle) Then
            erreurs.Add("La nouvelle affectation est identique à l'actuelle.")
        End If
        If erreurs.Count > 0 Then Return ResultatOperation.Echec(erreurs)
        Return Executer(Sub() _admin.ChangerAffectation(matricule, nouvelle, dateEffet),
                        $"Nouvelle affectation de {fiche.Collaborateur.NomComplet} à partir du {dateEffet:dd/MM/yyyy}.")
    End Function

    ''' <summary>
    ''' Départ de l'entreprise : le compte ne permet plus de se connecter, ses CR sont conservés et
    ''' ses praticiens deviennent « sans visiteur » (à réattribuer).
    ''' </summary>
    Public Function EnregistrerDepart(u As UtilisateurConnecte, matricule As String, dateDepart As Date) As ResultatOperation
        VerifierAcces(u)
        If matricule = u.Matricule Then Return ResultatOperation.Echec("Vous ne pouvez pas enregistrer votre propre départ.")
        Dim fiche = TrouverFiche(matricule)
        If fiche.Collaborateur.DateDepart.HasValue Then Return ResultatOperation.Echec("Le départ de ce collaborateur est déjà enregistré.")
        Dim erreurs As New List(Of String)
        If dateDepart.Date > Aujourdhui Then erreurs.Add("La date de départ ne peut pas être dans le futur.")
        Dim minimum = If(fiche.AffectationEnCours?.DateDebut, fiche.Collaborateur.DateEmbauche).Date
        If dateDepart.Date < minimum Then erreurs.Add($"La date de départ doit être postérieure au {minimum:dd/MM/yyyy}.")
        If erreurs.Count > 0 Then Return ResultatOperation.Echec(erreurs)

        Dim suite = If(fiche.TaillePortefeuille > 0,
                       $" {fiche.TaillePortefeuille} praticien(s) de son portefeuille sont désormais sans visiteur : pensez à les réattribuer.", "")
        Return Executer(Sub() _admin.EnregistrerDepart(matricule, dateDepart), $"Départ de {fiche.Collaborateur.NomComplet} enregistré.{suite}")
    End Function

    ''' <summary>Nouveau mot de passe provisoire ; le compte est déverrouillé.</summary>
    Public Function ReinitialiserMotDePasse(u As UtilisateurConnecte, matricule As String) As ResultatOperation
        VerifierAcces(u)
        Dim fiche = TrouverFiche(matricule)
        If fiche.Collaborateur.DateDepart.HasValue Then Return ResultatOperation.Echec("Ce collaborateur a quitté l'entreprise.")
        Dim motDePasse = GenerateurMotDePasse.Generer()
        Return Executer(Sub() _admin.ReinitialiserMotDePasse(matricule, HacheurMotDePasse.Hacher(motDePasse)),
                        $"Mot de passe de {fiche.Collaborateur.NomComplet} réinitialisé ; le compte est déverrouillé.", motDePasse)
    End Function

    Public Function DefinirVerrouillage(u As UtilisateurConnecte, matricule As String, verrouille As Boolean) As ResultatOperation
        VerifierAcces(u)
        If verrouille AndAlso matricule = u.Matricule Then Return ResultatOperation.Echec("Vous ne pouvez pas verrouiller votre propre compte.")
        Dim fiche = TrouverFiche(matricule)
        Return Executer(Sub() _admin.DefinirVerrouillage(matricule, verrouille),
                        $"Compte de {fiche.Collaborateur.NomComplet} {If(verrouille, "verrouillé", "déverrouillé")}.")
    End Function

    ' ------------------------------------------------------------------
    ' Portefeuilles (EX-72)
    ' ------------------------------------------------------------------

    Public Function PraticiensSansVisiteur(u As UtilisateurConnecte) As IReadOnlyList(Of PraticienResume)
        VerifierAcces(u)
        Return Appeler(Function() _admin.ListerPraticiensSansVisiteur())
    End Function

    Public Function PortefeuilleDe(u As UtilisateurConnecte, matricule As String) As IReadOnlyList(Of PraticienResume)
        VerifierAcces(u)
        Return Appeler(Function() _consultation.RechercherPraticiens(Nothing, Perimetre.DuCollaborateur(matricule), False, ServiceConsultation.MaxResultats))
    End Function

    ''' <summary>Confie des praticiens à un visiteur ou un délégué en poste, à partir d'aujourd'hui.</summary>
    Public Function AttribuerPraticiens(u As UtilisateurConnecte, numeros As IEnumerable(Of Integer), matricule As String) As ResultatOperation
        VerifierAcces(u)
        Dim liste = If(numeros, Enumerable.Empty(Of Integer)()).Distinct().ToList()
        If liste.Count = 0 Then Return ResultatOperation.Echec("Choisissez au moins un praticien.")
        Dim cible = VerifierVisiteurEnPoste(matricule)
        If cible Is Nothing Then Return ResultatOperation.Echec("Le portefeuille doit être confié à un visiteur ou à un délégué en poste.")
        Return Executer(Sub() _admin.AttribuerPraticiens(liste, matricule, Aujourdhui),
                        $"{liste.Count} praticien(s) confié(s) à {cible.Collaborateur.NomComplet}.")
    End Function

    Public Function TransfererPortefeuille(u As UtilisateurConnecte, deMatricule As String, versMatricule As String) As ResultatOperation
        VerifierAcces(u)
        If deMatricule = versMatricule Then Return ResultatOperation.Echec("Choisissez deux collaborateurs différents.")
        Dim cible = VerifierVisiteurEnPoste(versMatricule)
        If cible Is Nothing Then Return ResultatOperation.Echec("Le portefeuille doit être confié à un visiteur ou à un délégué en poste.")
        Dim nombre As Integer
        Dim resultat = Executer(Sub() nombre = _admin.TransfererPortefeuille(deMatricule, versMatricule, Aujourdhui), "")
        If Not resultat.Reussi Then Return resultat
        Return If(nombre = 0, ResultatOperation.Echec("Ce portefeuille est vide."),
                  ResultatOperation.Succes($"{nombre} praticien(s) transféré(s) à {cible.Collaborateur.NomComplet}."))
    End Function

    ' ------------------------------------------------------------------
    ' Référentiels (EX-73)
    ' ------------------------------------------------------------------

    Public Function Praticiens(u As UtilisateurConnecte, texte As String) As IReadOnlyList(Of PraticienResume)
        VerifierAcces(u)
        Return Appeler(Function() _consultation.RechercherPraticiens(texte, Nothing, True, ServiceConsultation.MaxResultats))
    End Function

    Public Function TrouverPraticien(u As UtilisateurConnecte, numero As Integer) As Praticien
        VerifierAcces(u)
        Return Appeler(Function() _referentiel.TrouverPraticien(numero))
    End Function

    Public Function TypesPraticien(u As UtilisateurConnecte) As IReadOnlyList(Of TypePraticien)
        VerifierAcces(u)
        Return Appeler(Function() _referentiel.ListerTypesPraticien())
    End Function

    ''' <summary>Crée (numéro vide) ou modifie un praticien.</summary>
    Public Function EnregistrerPraticien(u As UtilisateurConnecte, p As Praticien) As ResultatOperation
        VerifierAcces(u)
        ArgumentNullException.ThrowIfNull(p)
        p.Nom = Nettoyer(p.Nom) : p.Prenom = Nettoyer(p.Prenom)
        p.Adresse = NettoyerOuRien(p.Adresse) : p.CodePostal = NettoyerOuRien(p.CodePostal) : p.Ville = NettoyerOuRien(p.Ville)
        p.Telephone = NettoyerOuRien(p.Telephone) : p.Email = NettoyerOuRien(p.Email)
        Dim erreurs As New List(Of String)
        If p.Nom.Length = 0 OrElse p.Nom.Length > 50 Then erreurs.Add("Le nom est obligatoire (50 caractères au plus).")
        If p.Prenom.Length = 0 OrElse p.Prenom.Length > 50 Then erreurs.Add("Le prénom est obligatoire (50 caractères au plus).")
        If String.IsNullOrEmpty(p.CodeType) Then erreurs.Add("Choisissez le type de praticien.")
        If p.CodePostal IsNot Nothing AndAlso Not FormatCodePostal.IsMatch(p.CodePostal) Then erreurs.Add("Le code postal doit comporter 5 chiffres.")
        If p.Email IsNot Nothing AndAlso Not FormatEmail.IsMatch(p.Email) Then erreurs.Add("L'adresse e-mail n'est pas valide.")
        If p.CoefNotoriete.HasValue AndAlso (p.CoefNotoriete < 0 OrElse p.CoefNotoriete > 9999.99D) Then erreurs.Add("Le coefficient de notoriété doit être compris entre 0 et 9 999,99.")
        If erreurs.Count > 0 Then Return ResultatOperation.Echec(erreurs)

        If p.Numero.HasValue Then
            Return Executer(Sub() _admin.ModifierPraticien(p), $"Praticien {p.NomComplet} enregistré.")
        End If
        Return Executer(Sub() p.Numero = _referentiel.CreerPraticien(p), $"Praticien {p.NomComplet} créé.")
    End Function

    Public Function Medicaments(u As UtilisateurConnecte) As IReadOnlyList(Of Medicament)
        VerifierAcces(u)
        Return Appeler(Function() _referentiel.ListerMedicaments())
    End Function

    Public Function ModifierMedicament(u As UtilisateurConnecte, depotLegal As String, prix As Decimal, actif As Boolean) As ResultatOperation
        VerifierAcces(u)
        If prix < 0 OrElse prix > 999_999.99D Then Return ResultatOperation.Echec("Le prix d'un échantillon doit être compris entre 0 et 999 999,99 €.")
        Return Executer(Sub() _admin.ModifierMedicament(depotLegal, Math.Round(prix, 2), actif),
                        If(actif, "Médicament enregistré.", "Médicament retiré : il n'est plus proposé à la saisie."))
    End Function

    Public Function Motifs(u As UtilisateurConnecte) As IReadOnlyList(Of Motif)
        VerifierAcces(u)
        Return Appeler(Function() _referentiel.ListerMotifs())
    End Function

    Public Function CreerMotif(u As UtilisateurConnecte, code As String, libelle As String) As ResultatOperation
        VerifierAcces(u)
        code = Nettoyer(code).ToUpperInvariant()
        libelle = Nettoyer(libelle)
        Dim erreurs As New List(Of String)
        If Not FormatCodeMotif.IsMatch(code) Then erreurs.Add("Le code est obligatoire : 2 à 6 lettres.")
        If code = Motif.CodeAutre Then erreurs.Add("Le code AUTRE est réservé.")
        If libelle.Length = 0 OrElse libelle.Length > 60 Then erreurs.Add("Le libellé est obligatoire (60 caractères au plus).")
        If erreurs.Count > 0 Then Return ResultatOperation.Echec(erreurs)
        Return Executer(Sub() _admin.CreerMotif(code, libelle), $"Motif « {libelle} » créé.")
    End Function

    Public Function ModifierMotif(u As UtilisateurConnecte, code As String, libelle As String, actif As Boolean) As ResultatOperation
        VerifierAcces(u)
        libelle = Nettoyer(libelle)
        If libelle.Length = 0 OrElse libelle.Length > 60 Then Return ResultatOperation.Echec("Le libellé est obligatoire (60 caractères au plus).")
        ' « Autre » doit toujours rester proposé (EX-12)
        If code = Motif.CodeAutre AndAlso Not actif Then Return ResultatOperation.Echec("Le motif « Autre » doit rester actif.")
        Return Executer(Sub() _admin.ModifierMotif(code, libelle, actif), $"Motif « {libelle} » enregistré.")
    End Function

    ' ------------------------------------------------------------------
    ' Journal (EX-74)
    ' ------------------------------------------------------------------

    Public Function Journal(u As UtilisateurConnecte, debut As Date, fin As Date, texte As String, echecsSeulement As Boolean) As IReadOnlyList(Of EntreeJournal)
        VerifierAcces(u)
        If debut.Date > fin.Date Then Throw New ErreurMetierException("La date de début doit précéder la date de fin.")
        Return Appeler(Function() _admin.ListerJournal(debut, fin, texte, echecsSeulement, MaxLignesJournal))
    End Function

    ' ------------------------------------------------------------------
    ' Règles communes
    ' ------------------------------------------------------------------

    Private Shared Sub Normaliser(c As Collaborateur)
        c.Matricule = Nettoyer(c.Matricule).ToLowerInvariant()
        c.Login = Nettoyer(c.Login).ToLowerInvariant()
        c.Nom = Nettoyer(c.Nom)
        c.Prenom = Nettoyer(c.Prenom)
        c.Adresse = NettoyerOuRien(c.Adresse)
        c.CodePostal = NettoyerOuRien(c.CodePostal)
        c.Ville = NettoyerOuRien(c.Ville)
        c.Telephone = NettoyerOuRien(c.Telephone)
        c.Email = NettoyerOuRien(c.Email)
    End Sub

    Private Shared Function ControlerIdentite(c As Collaborateur) As List(Of String)
        Dim erreurs As New List(Of String)
        If c.Nom.Length = 0 OrElse c.Nom.Length > 50 Then erreurs.Add("Le nom est obligatoire (50 caractères au plus).")
        If c.Prenom.Length = 0 OrElse c.Prenom.Length > 50 Then erreurs.Add("Le prénom est obligatoire (50 caractères au plus).")
        If Not FormatLogin.IsMatch(c.Login) Then erreurs.Add("Le login est obligatoire : 3 à 30 caractères parmi a-z, 0-9, point, tiret et souligné.")
        If c.CodePostal IsNot Nothing AndAlso Not FormatCodePostal.IsMatch(c.CodePostal) Then erreurs.Add("Le code postal doit comporter 5 chiffres.")
        If c.Email IsNot Nothing AndAlso Not FormatEmail.IsMatch(c.Email) Then erreurs.Add("L'adresse e-mail n'est pas valide.")
        Return erreurs
    End Function

    ''' <summary>Visiteur / délégué : une région ; responsable : un secteur ; administrateur : rien.</summary>
    Private Shared Function ControlerAffectation(a As Affectation) As List(Of String)
        Dim erreurs As New List(Of String)
        If a Is Nothing Then
            erreurs.Add("Choisissez le profil.")
        ElseIf (a.Profil = Profil.Visiteur OrElse a.Profil = Profil.Delegue) AndAlso String.IsNullOrEmpty(a.CodeRegion) Then
            erreurs.Add("Un visiteur ou un délégué doit être rattaché à une région.")
        ElseIf a.Profil = Profil.Responsable AndAlso String.IsNullOrEmpty(a.CodeSecteur) Then
            erreurs.Add("Un responsable doit être rattaché à un secteur.")
        End If
        Return erreurs
    End Function

    Private Shared Function MemeAffectation(a As Affectation, b As Affectation) As Boolean
        If a.Profil <> b.Profil Then Return False
        Select Case a.Profil
            Case Profil.Visiteur, Profil.Delegue : Return a.CodeRegion = b.CodeRegion
            Case Profil.Responsable : Return a.CodeSecteur = b.CodeSecteur
            Case Else : Return True
        End Select
    End Function

    Private Function TrouverFiche(matricule As String) As FicheCollaborateur
        Dim fiche = Appeler(Function() _admin.ListerCollaborateurs()).FirstOrDefault(Function(f) f.Collaborateur.Matricule = matricule)
        If fiche Is Nothing Then Throw New ErreurMetierException("Ce collaborateur n'existe pas.")
        Return fiche
    End Function

    ''' <summary>Renvoie la fiche si le collaborateur est un visiteur ou un délégué en poste, sinon Nothing.</summary>
    Private Function VerifierVisiteurEnPoste(matricule As String) As FicheCollaborateur
        Dim fiche = Appeler(Function() _admin.ListerCollaborateurs()).FirstOrDefault(Function(f) f.Collaborateur.Matricule = matricule)
        If fiche Is Nothing OrElse fiche.AffectationEnCours Is Nothing Then Return Nothing
        Return If(fiche.AffectationEnCours.Profil = Profil.Visiteur OrElse fiche.AffectationEnCours.Profil = Profil.Delegue, fiche, Nothing)
    End Function

    Private Shared Function Nettoyer(texte As String) As String
        Return If(texte, "").Trim()
    End Function

    Private Shared Function NettoyerOuRien(texte As String) As String
        Return If(String.IsNullOrWhiteSpace(texte), Nothing, texte.Trim())
    End Function

    ''' <summary>Exécute une écriture et traduit les erreurs de base en messages compréhensibles.</summary>
    Private Shared Function Executer(action As Action, message As String, Optional motDePasse As String = Nothing) As ResultatOperation
        Try
            action()
            Return ResultatOperation.Succes(message, motDePasse)
        Catch ex As AccesDonneesException When ex.EstDoublon
            Return ResultatOperation.Echec(MessageDoublon)
        Catch ex As AccesDonneesException
            Return ResultatOperation.Echec(ServiceRapports.MessageServeur)
        End Try
    End Function

    Private Shared Sub VerifierAcces(u As UtilisateurConnecte)
        ArgumentNullException.ThrowIfNull(u)
        If Not Autorisations.PeutAcceder(u.Profil, ModuleApplication.Administration) Then
            Throw New ErreurMetierException("L'administration est réservée aux administrateurs.")
        End If
    End Sub

    Private Shared Function Appeler(Of T)(appel As Func(Of T)) As T
        Try
            Return appel()
        Catch ex As AccesDonneesException
            Throw New ErreurMetierException(ServiceRapports.MessageServeur, ex)
        End Try
    End Function

End Class
