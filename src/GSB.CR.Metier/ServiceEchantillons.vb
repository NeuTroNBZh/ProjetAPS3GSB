Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Échantillons de l'équipe : dotations mensuelles saisies par le délégué (EX-33) et
''' contrôle de stock attribué / distribué (EX-34), consultable aussi par le responsable.
''' </summary>
Public Class ServiceEchantillons

    ''' <summary>Quantité maximale d'une dotation (colonne NUMBER(6)).</summary>
    Public Const MaxQuantite As Integer = 999_999

    ''' <summary>Nombre de mois passés sur lesquels une dotation peut encore être saisie ou corrigée.</summary>
    Public Const MoisPassesModifiables As Integer = 12

    Private ReadOnly _echantillons As IEchantillonDao
    Private ReadOnly _equipe As IEquipeDao
    Private ReadOnly _referentiel As IReferentielDao
    Private ReadOnly _horloge As TimeProvider

    Public Sub New(echantillons As IEchantillonDao, equipe As IEquipeDao, referentiel As IReferentielDao, horloge As TimeProvider)
        ArgumentNullException.ThrowIfNull(echantillons)
        ArgumentNullException.ThrowIfNull(equipe)
        ArgumentNullException.ThrowIfNull(referentiel)
        ArgumentNullException.ThrowIfNull(horloge)
        _echantillons = echantillons
        _equipe = equipe
        _referentiel = referentiel
        _horloge = horloge
    End Sub

    Private ReadOnly Property MoisCourant As Date
        Get
            Dim j = _horloge.GetLocalNow().Date
            Return New Date(j.Year, j.Month, 1)
        End Get
    End Property

    ''' <summary>Seul le délégué régional enregistre les attributions (CDC) ; le responsable consulte.</summary>
    Public Shared Function PeutSaisir(utilisateur As UtilisateurConnecte) As Boolean
        Return utilisateur IsNot Nothing AndAlso utilisateur.Profil = Profil.Delegue
    End Function

    ''' <summary>Mois proposés, du plus récent (mois prochain) au plus ancien (12 mois en arrière).</summary>
    Public Function MoisProposes() As IReadOnlyList(Of Date)
        Return Enumerable.Range(-1, MoisPassesModifiables + 2).Select(Function(i) MoisCourant.AddMonths(-i)).ToList()
    End Function

    Public Function Membres(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of MembreEquipe)
        Dim p = PerimetreDe(utilisateur)
        Return Appeler(Function() _equipe.ListerMembres(p))
    End Function

    ''' <summary>Médicaments commercialisés, pour la saisie des dotations.</summary>
    Public Function Medicaments() As IReadOnlyList(Of Medicament)
        Return Appeler(Function() _referentiel.ListerMedicaments()).Where(Function(m) m.Actif).ToList()
    End Function

    ''' <summary>Contrôle de stock du mois pour l'équipe ; les dépassements en premier si demandé.</summary>
    Public Function Stock(utilisateur As UtilisateurConnecte, mois As Date, seulementLesEcarts As Boolean) As IReadOnlyList(Of LigneStock)
        Dim p = PerimetreDe(utilisateur)
        Dim lignes = Appeler(Function() _echantillons.Stock(p, mois))
        Return If(seulementLesEcarts, lignes.Where(Function(l) l.EnDepassement).ToList(), lignes)
    End Function

    ''' <summary>Crée ou remplace une dotation. Renvoie les erreurs (vide si enregistrée).</summary>
    Public Function EnregistrerDotation(utilisateur As UtilisateurConnecte, matricule As String, depotLegal As String,
                                        mois As Date, quantite As Integer) As IReadOnlyList(Of String)
        Dim erreurs = ControlerSaisie(utilisateur, matricule, mois)
        If String.IsNullOrEmpty(depotLegal) Then erreurs.Add("Choisissez le produit.")
        If quantite <= 0 Then erreurs.Add("La quantité attribuée doit être supérieure à zéro.")
        If quantite > MaxQuantite Then erreurs.Add($"La quantité attribuée ne peut pas dépasser {MaxQuantite:N0}.")
        If erreurs.Count > 0 Then Return erreurs

        Try
            _echantillons.EnregistrerDotation(matricule, depotLegal, mois, quantite, utilisateur.Matricule)
        Catch ex As AccesDonneesException
            Return {ServiceRapports.MessageServeur}
        End Try
        Return erreurs
    End Function

    ''' <summary>Supprime une dotation. Renvoie les erreurs (vide si supprimée).</summary>
    Public Function SupprimerDotation(utilisateur As UtilisateurConnecte, matricule As String, depotLegal As String,
                                      mois As Date) As IReadOnlyList(Of String)
        Dim erreurs = ControlerSaisie(utilisateur, matricule, mois)
        If erreurs.Count > 0 Then Return erreurs
        Try
            If Not _echantillons.SupprimerDotation(matricule, depotLegal, mois) Then Return {"Aucune dotation à supprimer."}
        Catch ex As AccesDonneesException
            Return {ServiceRapports.MessageServeur}
        End Try
        Return erreurs
    End Function

    ''' <summary>Règles communes : délégué, visiteur de sa région, mois modifiable.</summary>
    Private Function ControlerSaisie(utilisateur As UtilisateurConnecte, matricule As String, mois As Date) As List(Of String)
        Dim erreurs As New List(Of String)
        If Not PeutSaisir(utilisateur) Then
            erreurs.Add("Seul le délégué régional peut attribuer des échantillons.")
            Return erreurs
        End If
        If String.IsNullOrEmpty(matricule) OrElse Not Membres(utilisateur).Any(Function(m) m.Matricule = matricule) Then
            erreurs.Add("Choisissez un visiteur de votre région.")
        End If
        Dim premier = New Date(mois.Year, mois.Month, 1)
        If premier > MoisCourant.AddMonths(1) OrElse premier < MoisCourant.AddMonths(-MoisPassesModifiables) Then
            erreurs.Add($"Les dotations se saisissent du mois prochain jusqu'à {MoisPassesModifiables} mois en arrière.")
        End If
        Return erreurs
    End Function

    Private Shared Function PerimetreDe(utilisateur As UtilisateurConnecte) As Perimetre
        ArgumentNullException.ThrowIfNull(utilisateur)
        If Not Autorisations.PeutAcceder(utilisateur.Profil, ModuleApplication.Echantillons) Then
            Throw New ErreurMetierException("Votre profil ne permet pas de gérer les échantillons.")
        End If
        Return Perimetres.DeLEquipe(utilisateur)
    End Function

    Private Shared Function Appeler(Of T)(appel As Func(Of T)) As T
        Try
            Return appel()
        Catch ex As AccesDonneesException
            Throw New ErreurMetierException(ServiceRapports.MessageServeur, ex)
        End Try
    End Function

End Class
