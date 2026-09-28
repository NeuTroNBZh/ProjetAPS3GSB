Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Suivi d'une équipe : région pour le délégué (EX-31, EX-32, EX-35), secteur pour le responsable (EX-40, EX-41).
''' Seuls les comptes-rendus validés des membres actuels de l'équipe sont visibles, en lecture seule.
''' </summary>
Public Class ServiceEquipe

    Private ReadOnly _activite As IActiviteDao
    Private ReadOnly _rapports As IRapportDao
    Private ReadOnly _consultation As IConsultationDao
    Private ReadOnly _equipe As IEquipeDao
    Private ReadOnly _horloge As TimeProvider

    Public Sub New(activite As IActiviteDao, rapports As IRapportDao, consultation As IConsultationDao,
                   equipe As IEquipeDao, horloge As TimeProvider)
        ArgumentNullException.ThrowIfNull(activite)
        ArgumentNullException.ThrowIfNull(rapports)
        ArgumentNullException.ThrowIfNull(consultation)
        ArgumentNullException.ThrowIfNull(equipe)
        ArgumentNullException.ThrowIfNull(horloge)
        _activite = activite
        _rapports = rapports
        _consultation = consultation
        _equipe = equipe
        _horloge = horloge
    End Sub

    Public ReadOnly Property Aujourdhui As Date
        Get
            Return _horloge.GetLocalNow().Date
        End Get
    End Property

    ''' <summary>Périmètre suivi par l'utilisateur (vérifie aussi ses droits).</summary>
    Public Function PerimetreDe(utilisateur As UtilisateurConnecte) As Perimetre
        ArgumentNullException.ThrowIfNull(utilisateur)
        If Not Autorisations.PeutAcceder(utilisateur.Profil, ModuleApplication.ActiviteRegion) AndAlso
           Not Autorisations.PeutAcceder(utilisateur.Profil, ModuleApplication.ActiviteSecteur) Then
            Throw New ErreurMetierException("Votre profil ne permet pas de suivre une équipe.")
        End If
        Return Perimetres.DeLEquipe(utilisateur)
    End Function

    Public Function Membres(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of MembreEquipe)
        Dim p = PerimetreDe(utilisateur)
        Return Appeler(Function() _equipe.ListerMembres(p))
    End Function

    ''' <summary>Synthèse de toute l'équipe sur la période.</summary>
    Public Function Synthese(utilisateur As UtilisateurConnecte, debut As Date, fin As Date) As SyntheseActivite
        Dim p = PerimetreDe(utilisateur)
        Dim periode = Perimetres.ControlerPeriode(debut, fin, Aujourdhui)
        Dim s = Appeler(Function() _activite.ChargerSynthese(p, periode.Debut, periode.Fin))
        s.ParMois = ServiceActivite.CompleterMois(s.ParMois, periode.Debut, periode.Fin)
        Return s
    End Function

    ''' <summary>Activité de chaque membre sur la période.</summary>
    Public Function ActiviteParMembre(utilisateur As UtilisateurConnecte, debut As Date, fin As Date) As IReadOnlyList(Of ActiviteMembre)
        Dim p = PerimetreDe(utilisateur)
        Dim periode = Perimetres.ControlerPeriode(debut, fin, Aujourdhui)
        Return Appeler(Function() _activite.ActiviteParMembre(p, periode.Debut, periode.Fin))
    End Function

    ''' <summary>Synthèse d'un membre de l'équipe (refusée pour un collaborateur hors équipe).</summary>
    Public Function SyntheseMembre(utilisateur As UtilisateurConnecte, matricule As String, debut As Date, fin As Date) As SyntheseActivite
        Dim membre = VerifierMembre(utilisateur, matricule)
        Dim periode = Perimetres.ControlerPeriode(debut, fin, Aujourdhui)
        Dim s = Appeler(Function() _activite.ChargerSynthese(Perimetre.DuCollaborateur(membre.Matricule, membre.NomComplet), periode.Debut, periode.Fin))
        s.ParMois = ServiceActivite.CompleterMois(s.ParMois, periode.Debut, periode.Fin)
        Return s
    End Function

    ''' <summary>CR validés de l'équipe sur les 3 dernières années (les brouillons restent privés).</summary>
    Public Function RapportsEquipe(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of RapportResume)
        Dim p = PerimetreDe(utilisateur)
        Return Appeler(Function() _rapports.ListerParPerimetre(p, Aujourdhui.AddYears(-ServiceRapports.AnneesConsultation), inclureBrouillons:=False))
    End Function

    ''' <summary>Lecture d'un CR validé d'un membre de l'équipe.</summary>
    Public Function ChargerRapport(utilisateur As UtilisateurConnecte, numero As Integer) As RapportVisite
        Dim p = PerimetreDe(utilisateur)
        Dim rapport = Appeler(Function() _rapports.Charger(numero))
        If rapport Is Nothing OrElse rapport.Etat <> EtatRapport.Valide Then
            Throw New ErreurMetierException("Ce compte-rendu n'est pas consultable.")
        End If
        Dim membres = Appeler(Function() _equipe.ListerMembres(p))
        If Not membres.Any(Function(m) m.Matricule = rapport.MatriculeAuteur) Then
            Throw New ErreurMetierException("Ce compte-rendu n'appartient pas à votre équipe.")
        End If
        Return rapport
    End Function

    ''' <summary>Praticiens des portefeuilles de l'équipe à revoir, du plus urgent au moins urgent (EX-35).</summary>
    Public Function PraticiensARevoir(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of PraticienARevoir)
        Dim p = PerimetreDe(utilisateur)
        Dim praticiens = Appeler(Function() _consultation.RechercherPraticiens(Nothing, p, False, ServiceConsultation.MaxResultats))
        Return ServiceActivite.Prioriser(praticiens, Aujourdhui)
    End Function

    Private Function VerifierMembre(utilisateur As UtilisateurConnecte, matricule As String) As MembreEquipe
        Dim membre = Membres(utilisateur).FirstOrDefault(Function(m) m.Matricule = matricule)
        If membre Is Nothing Then Throw New ErreurMetierException("Ce collaborateur ne fait pas partie de votre équipe.")
        Return membre
    End Function

    Private Shared Function Appeler(Of T)(appel As Func(Of T)) As T
        Try
            Return appel()
        Catch ex As AccesDonneesException
            Throw New ErreurMetierException(ServiceRapports.MessageServeur, ex)
        End Try
    End Function

End Class
