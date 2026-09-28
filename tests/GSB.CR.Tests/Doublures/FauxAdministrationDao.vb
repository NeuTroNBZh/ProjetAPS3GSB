Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>Administration en mémoire : trois comptes (admin, visiteur a131, délégué d01) et un parti (c14).</summary>
Public Class FauxAdministrationDao
    Implements IAdministrationDao

    Public ReadOnly Fiches As New List(Of FicheCollaborateur)
    Public ReadOnly Operations As New List(Of String)
    Public Property DernierMotDePasseHache As String
    Public Property DoublonAuProchainAppel As Boolean

    Public Sub New()
        Fiches.Add(Fiche("adm1", "admin", Profil.Administrateur, Nothing, #2020-01-01#))
        Fiches.Add(Fiche("a131", "lvillechalane", Profil.Visiteur, "AQU", #2019-12-21#, portefeuille:=2))
        Fiches.Add(Fiche("d01", "cbedos", Profil.Delegue, "AQU", #2018-01-01#))
        Dim parti = Fiche("c14", "fdaburon", Profil.Visiteur, Nothing, #2024-01-08#)
        parti.AffectationEnCours = Nothing
        parti.Collaborateur.DateDepart = #2025-06-30#
        Fiches.Add(parti)
    End Sub

    Private Shared Function Fiche(matricule As String, login As String, profil As Profil, region As String, debut As Date,
                                  Optional portefeuille As Integer = 0) As FicheCollaborateur
        Return New FicheCollaborateur() With {
            .Collaborateur = New Collaborateur() With {.Matricule = matricule, .Login = login, .Nom = matricule, .Prenom = "X", .DateEmbauche = debut},
            .AffectationEnCours = New Affectation() With {.Profil = profil, .CodeRegion = region, .DateDebut = debut},
            .TaillePortefeuille = portefeuille}
    End Function

    Private Sub Ecrire(operation As String)
        If DoublonAuProchainAppel Then
            DoublonAuProchainAppel = False
            Throw New AccesDonneesException("doublon", 1, New Exception())
        End If
        Operations.Add(operation)
    End Sub

    Public Function ListerRegions() As List(Of Region) Implements IAdministrationDao.ListerRegions
        Return New List(Of Region) From {New Region() With {.Code = "AQU", .Nom = "Aquitaine", .CodeSecteur = "O"}}
    End Function

    Public Function ListerSecteurs() As List(Of Secteur) Implements IAdministrationDao.ListerSecteurs
        Return New List(Of Secteur) From {New Secteur() With {.Code = "O", .Libelle = "Ouest"}}
    End Function

    Public Function ListerCollaborateurs() As List(Of FicheCollaborateur) Implements IAdministrationDao.ListerCollaborateurs
        Return Fiches.ToList()
    End Function

    Public Function ListerAffectations(matricule As String) As List(Of Affectation) Implements IAdministrationDao.ListerAffectations
        Return New List(Of Affectation)
    End Function

    Public Sub CreerCollaborateur(collaborateur As Collaborateur, motDePasseHache As String, affectation As Affectation) Implements IAdministrationDao.CreerCollaborateur
        Ecrire($"creer {collaborateur.Matricule}")
        DernierMotDePasseHache = motDePasseHache
    End Sub

    Public Sub ModifierCollaborateur(collaborateur As Collaborateur) Implements IAdministrationDao.ModifierCollaborateur
        Ecrire($"modifier {collaborateur.Matricule}")
    End Sub

    Public Sub ChangerAffectation(matricule As String, nouvelle As Affectation, dateEffet As Date) Implements IAdministrationDao.ChangerAffectation
        Ecrire($"affectation {matricule} {ProfilCodes.VersCode(nouvelle.Profil)} {dateEffet:yyyy-MM-dd}")
    End Sub

    Public Sub EnregistrerDepart(matricule As String, dateDepart As Date) Implements IAdministrationDao.EnregistrerDepart
        Ecrire($"depart {matricule}")
    End Sub

    Public Sub ReinitialiserMotDePasse(matricule As String, motDePasseHache As String) Implements IAdministrationDao.ReinitialiserMotDePasse
        Ecrire($"reinit {matricule}")
        DernierMotDePasseHache = motDePasseHache
    End Sub

    Public Sub DefinirVerrouillage(matricule As String, verrouille As Boolean) Implements IAdministrationDao.DefinirVerrouillage
        Ecrire($"verrou {matricule} {verrouille}")
    End Sub

    Public Function ListerPraticiensSansVisiteur() As List(Of PraticienResume) Implements IAdministrationDao.ListerPraticiensSansVisiteur
        Return New List(Of PraticienResume)
    End Function

    Public Sub AttribuerPraticiens(numeros As IEnumerable(Of Integer), matricule As String, dateEffet As Date) Implements IAdministrationDao.AttribuerPraticiens
        Ecrire($"attribuer {String.Join(",", numeros)} {matricule}")
    End Sub

    Public Function TransfererPortefeuille(deMatricule As String, versMatricule As String, dateEffet As Date) As Integer Implements IAdministrationDao.TransfererPortefeuille
        Ecrire($"transferer {deMatricule} {versMatricule}")
        Return Fiches.Single(Function(f) f.Collaborateur.Matricule = deMatricule).TaillePortefeuille
    End Function

    Public Sub ModifierPraticien(praticien As Praticien) Implements IAdministrationDao.ModifierPraticien
        Ecrire($"praticien {praticien.Numero}")
    End Sub

    Public Sub ModifierMedicament(depotLegal As String, prixEchantillon As Decimal, actif As Boolean) Implements IAdministrationDao.ModifierMedicament
        Ecrire($"medicament {depotLegal} {prixEchantillon} {actif}")
    End Sub

    Public Sub CreerMotif(code As String, libelle As String) Implements IAdministrationDao.CreerMotif
        Ecrire($"motif {code}")
    End Sub

    Public Sub ModifierMotif(code As String, libelle As String, actif As Boolean) Implements IAdministrationDao.ModifierMotif
        Ecrire($"motif {code} {actif}")
    End Sub

    Private Shared Function Elements(ParamArray codes As String()) As List(Of ElementReferentiel)
        Return codes.Select(Function(c) New ElementReferentiel() With {.Code = c, .Libelle = c}).ToList()
    End Function

    Public Function ListerComposants() As List(Of ElementReferentiel) Implements IAdministrationDao.ListerComposants
        Return Elements("IBUP", "PARA")
    End Function

    Public Function ListerTypesIndividu() As List(Of ElementReferentiel) Implements IAdministrationDao.ListerTypesIndividu
        Return Elements("ADU", "ENF")
    End Function

    Public Function ListerPresentations() As List(Of ElementReferentiel) Implements IAdministrationDao.ListerPresentations
        Return Elements("CP", "SIR")
    End Function

    Public Function ListerDosages() As List(Of ElementReferentiel) Implements IAdministrationDao.ListerDosages
        Return Elements("400MG", "500MG")
    End Function

    Public Sub CreerComposant(code As String, libelle As String) Implements IAdministrationDao.CreerComposant
        Ecrire($"composant {code} {libelle}")
    End Sub

    Public Sub CreerDosage(code As String, quantite As Decimal, unite As String) Implements IAdministrationDao.CreerDosage
        Ecrire($"dosage {code} {quantite.ToString(Globalization.CultureInfo.InvariantCulture)} {unite}")
    End Sub

    Public Sub AjouterComposition(depotLegal As String, codeComposant As String, quantite As Decimal, unite As String) Implements IAdministrationDao.AjouterComposition
        Ecrire($"composition {depotLegal} {codeComposant} {quantite.ToString(Globalization.CultureInfo.InvariantCulture)} {unite}")
    End Sub

    Public Sub RetirerComposition(depotLegal As String, codeComposant As String) Implements IAdministrationDao.RetirerComposition
        Ecrire($"retirer composition {depotLegal} {codeComposant}")
    End Sub

    Public Sub AjouterInteraction(perturbateur As String, perturbe As String, description As String) Implements IAdministrationDao.AjouterInteraction
        Ecrire($"interaction {perturbateur}>{perturbe} {description}")
    End Sub

    Public Sub RetirerInteraction(perturbateur As String, perturbe As String) Implements IAdministrationDao.RetirerInteraction
        Ecrire($"retirer interaction {perturbateur}>{perturbe}")
    End Sub

    Public Sub AjouterPosologie(depotLegal As String, codeTypeIndividu As String, codePresentation As String, codeDosage As String, texte As String) Implements IAdministrationDao.AjouterPosologie
        Ecrire($"posologie {depotLegal} {codeTypeIndividu} {codePresentation} {codeDosage} {texte}")
    End Sub

    Public Sub RetirerPosologie(depotLegal As String, codeTypeIndividu As String, codePresentation As String, codeDosage As String) Implements IAdministrationDao.RetirerPosologie
        Ecrire($"retirer posologie {depotLegal} {codeTypeIndividu} {codePresentation} {codeDosage}")
    End Sub

    Public Function ListerJournal(debut As Date, fin As Date, texte As String, echecsSeulement As Boolean, maximum As Integer) As List(Of EntreeJournal) Implements IAdministrationDao.ListerJournal
        Return New List(Of EntreeJournal)
    End Function

End Class
