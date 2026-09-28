Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Consultation des fiches praticiens (EX-21) et médicaments (EX-22), ouverte à tous les profils.
''' Les erreurs de base de données sont transformées en <see cref="ErreurMetierException"/>.
''' </summary>
Public Class ServiceConsultation

    ''' <summary>Nombre maximal de praticiens renvoyés par une recherche.</summary>
    Public Const MaxResultats As Integer = 500

    Private ReadOnly _consultation As IConsultationDao
    Private ReadOnly _referentiel As IReferentielDao
    Private ReadOnly _horloge As TimeProvider

    Public Sub New(consultation As IConsultationDao, referentiel As IReferentielDao, horloge As TimeProvider)
        ArgumentNullException.ThrowIfNull(consultation)
        ArgumentNullException.ThrowIfNull(referentiel)
        ArgumentNullException.ThrowIfNull(horloge)
        _consultation = consultation
        _referentiel = referentiel
        _horloge = horloge
    End Sub

    Public ReadOnly Property Aujourdhui As Date
        Get
            Return _horloge.GetLocalNow().Date
        End Get
    End Property

    ''' <summary>Vrai si le profil a un portefeuille de praticiens (visiteur ou délégué).</summary>
    Public Shared Function APortefeuille(profil As Profil) As Boolean
        Return profil = Profil.Visiteur OrElse profil = Profil.Delegue
    End Function

    ''' <summary>
    ''' Recherche des praticiens. <paramref name="seulementMonPortefeuille"/> n'a d'effet que pour
    ''' un visiteur ou un délégué.
    ''' </summary>
    Public Function RechercherPraticiens(utilisateur As UtilisateurConnecte, texte As String,
                                         seulementMonPortefeuille As Boolean, inclureInactifs As Boolean) As IReadOnlyList(Of PraticienResume)
        VerifierAcces(utilisateur, ModuleApplication.Praticiens)
        Dim matricule = If(seulementMonPortefeuille AndAlso APortefeuille(utilisateur.Profil), utilisateur.Matricule, Nothing)
        Return Appeler(Function() _consultation.RechercherPraticiens(texte, matricule, inclureInactifs, MaxResultats))
    End Function

    Public Function FichePraticien(utilisateur As UtilisateurConnecte, numero As Integer) As FichePraticien
        VerifierAcces(utilisateur, ModuleApplication.Praticiens)
        Dim fiche = Appeler(Function() _consultation.ChargerFichePraticien(numero))
        If fiche Is Nothing Then Throw New ErreurMetierException("Ce praticien n'existe plus.")
        Return fiche
    End Function

    ''' <summary>Tous les médicaments (commercialisés ou retirés), triés par nom commercial.</summary>
    Public Function Medicaments(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of Medicament)
        VerifierAcces(utilisateur, ModuleApplication.Medicaments)
        Return Appeler(Function() _referentiel.ListerMedicaments())
    End Function

    Public Function FicheMedicament(utilisateur As UtilisateurConnecte, depotLegal As String) As FicheMedicament
        VerifierAcces(utilisateur, ModuleApplication.Medicaments)
        Dim fiche = Appeler(Function() _consultation.ChargerFicheMedicament(depotLegal))
        If fiche Is Nothing Then Throw New ErreurMetierException("Ce médicament n'existe plus.")
        Return fiche
    End Function

    ''' <summary>Situation de périodicité d'un praticien à la date du jour.</summary>
    Public Function Periodicite(dateDerniereVisite As Date?) As EtatPeriodicite
        Return Metier.Periodicite.Evaluer(dateDerniereVisite, Aujourdhui)
    End Function

    Private Shared Sub VerifierAcces(utilisateur As UtilisateurConnecte, moduleDemande As ModuleApplication)
        ArgumentNullException.ThrowIfNull(utilisateur)
        If Not Autorisations.PeutAcceder(utilisateur.Profil, moduleDemande) Then
            Throw New ErreurMetierException("Votre profil ne permet pas d'accéder à ces informations.")
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
