Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>
''' Module « Mon activité » : synthèse de l'activité du collaborateur sur une période (EX-23)
''' et praticiens de son portefeuille à revoir (EX-24).
''' </summary>
Public Class ServiceActivite

    Private ReadOnly _activite As IActiviteDao
    Private ReadOnly _consultation As IConsultationDao
    Private ReadOnly _horloge As TimeProvider

    Public Sub New(activite As IActiviteDao, consultation As IConsultationDao, horloge As TimeProvider)
        ArgumentNullException.ThrowIfNull(activite)
        ArgumentNullException.ThrowIfNull(consultation)
        ArgumentNullException.ThrowIfNull(horloge)
        _activite = activite
        _consultation = consultation
        _horloge = horloge
    End Sub

    Public ReadOnly Property Aujourdhui As Date
        Get
            Return _horloge.GetLocalNow().Date
        End Get
    End Property

    ''' <summary>Date la plus ancienne consultable (3 ans, EX-20).</summary>
    Public ReadOnly Property DebutConsultable As Date
        Get
            Return Aujourdhui.AddYears(-ServiceRapports.AnneesConsultation)
        End Get
    End Property

    ''' <summary>Périodes proposées à l'utilisateur, la première étant la période par défaut.</summary>
    Public Function PeriodesPredefinies() As IReadOnlyList(Of Periode)
        Dim j = Aujourdhui
        Dim debutMois As New Date(j.Year, j.Month, 1)
        Return {
            New Periode("Les 3 derniers mois", j.AddMonths(-3).AddDays(1), j),
            New Periode("Ce mois-ci", debutMois, j),
            New Periode("Le mois dernier", debutMois.AddMonths(-1), debutMois.AddDays(-1)),
            New Periode("Depuis le 1er janvier", New Date(j.Year, 1, 1), j),
            New Periode("Les 12 derniers mois", j.AddMonths(-12).AddDays(1), j),
            New Periode("Les 3 dernières années", DebutConsultable, j)}
    End Function

    ''' <summary>
    ''' Synthèse de l'activité de l'utilisateur entre deux dates (incluses).
    ''' Une fin dans le futur est ramenée à aujourd'hui.
    ''' </summary>
    ''' <exception cref="ErreurMetierException">Période invalide ou trop ancienne, ou serveur indisponible.</exception>
    Public Function MaSynthese(utilisateur As UtilisateurConnecte, debut As Date, fin As Date) As SyntheseActivite
        VerifierAcces(utilisateur)
        Dim periode = Perimetres.ControlerPeriode(debut, fin, Aujourdhui)

        Dim synthese = Appeler(Function() _activite.ChargerSynthese(Perimetre.DuCollaborateur(utilisateur.Matricule), periode.Debut, periode.Fin))
        synthese.ParMois = CompleterMois(synthese.ParMois, periode.Debut, periode.Fin)
        Return synthese
    End Function

    ''' <summary>
    ''' Renvoie un élément par mois de la période, dans l'ordre, avec zéro pour les mois sans visite
    ''' (un graphique ne doit pas « sauter » les mois creux).
    ''' </summary>
    Public Shared Function CompleterMois(mois As IEnumerable(Of VisitesDuMois), debut As Date, fin As Date) As List(Of VisitesDuMois)
        Dim connus = mois.ToDictionary(Function(m) New Date(m.Mois.Year, m.Mois.Month, 1), Function(m) m.Nombre)
        Dim resultat As New List(Of VisitesDuMois)
        Dim courant As New Date(debut.Year, debut.Month, 1)
        While courant <= fin
            Dim nombre As Integer
            connus.TryGetValue(courant, nombre)
            resultat.Add(New VisitesDuMois() With {.Mois = courant, .Nombre = nombre})
            courant = courant.AddMonths(1)
        End While
        Return resultat
    End Function

    ''' <summary>
    ''' Praticiens du portefeuille à planifier, du plus urgent au moins urgent (EX-24) :
    ''' 1. plus de 8 mois sans visite ; 2. jamais visités ; 3. prochaine visite prévue dépassée ;
    ''' 4. entre 6 et 8 mois. Les praticiens à jour sans visite prévue dépassée sont exclus.
    ''' </summary>
    Public Function PraticiensARevoir(utilisateur As UtilisateurConnecte) As IReadOnlyList(Of PraticienARevoir)
        VerifierAcces(utilisateur)
        Dim portefeuille = Appeler(Function() _consultation.RechercherPraticiens(Nothing, Perimetre.DuCollaborateur(utilisateur.Matricule), False, ServiceConsultation.MaxResultats))
        Return Prioriser(portefeuille, Aujourdhui)
    End Function

    ''' <summary>Calcul de la liste « à revoir » (séparé pour être testé sans base).</summary>
    Public Shared Function Prioriser(praticiens As IEnumerable(Of PraticienResume), aujourdhui As Date) As List(Of PraticienARevoir)
        Dim resultat As New List(Of PraticienARevoir)
        For Each p In praticiens
            Dim etat = Periodicite.Evaluer(p.DateDerniereVisite, aujourdhui)
            Dim depassee = p.DateProchainePrevue.HasValue AndAlso p.DateProchainePrevue.Value.Date < aujourdhui.Date
            Dim priorite As Integer
            Select Case etat
                Case EtatPeriodicite.ARevoir : priorite = 1
                Case EtatPeriodicite.JamaisVisite : priorite = 2
                Case EtatPeriodicite.ARevoirBientot : priorite = If(depassee, 3, 4)
                Case Else : priorite = If(depassee, 3, 0)
            End Select
            If priorite = 0 Then Continue For

            resultat.Add(New PraticienARevoir() With {
                .Praticien = p, .Etat = etat, .ProchainePrevueDepassee = depassee, .Priorite = priorite,
                .JoursDepuisDerniereVisite = If(p.DateDerniereVisite.HasValue,
                                                (aujourdhui.Date - p.DateDerniereVisite.Value.Date).Days, CType(Nothing, Integer?))})
        Next
        ' Même priorité : la visite la plus ancienne d'abord
        Return resultat.OrderBy(Function(x) x.Priorite).
                        ThenBy(Function(x) If(x.Praticien.DateDerniereVisite, Date.MinValue)).
                        ThenBy(Function(x) x.Praticien.NomComplet).ToList()
    End Function

    Private Shared Sub VerifierAcces(utilisateur As UtilisateurConnecte)
        ArgumentNullException.ThrowIfNull(utilisateur)
        If Not Autorisations.PeutAcceder(utilisateur.Profil, ModuleApplication.MonActivite) Then
            Throw New ErreurMetierException("Votre profil ne permet pas de consulter une activité personnelle.")
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
