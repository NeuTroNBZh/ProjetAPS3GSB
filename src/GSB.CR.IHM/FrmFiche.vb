Imports System.Globalization
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Fenêtre de consultation générique : en-tête bleu et fiche remplie en arrière-plan par l'appelant.
''' Sert à lire un compte-rendu d'un membre de l'équipe (EX-32, EX-41) ou la synthèse d'un membre (EX-31).
''' </summary>
Public Class FrmFiche

    Private ReadOnly _remplir As Func(Of PanneauFiche, Task)

    ''' <param name="remplir">Traitement (asynchrone) qui charge les données et remplit la fiche.</param>
    Public Sub New(titre As String, sousTitre As String, remplir As Func(Of PanneauFiche, Task))
        InitializeComponent()
        _remplir = remplir
        Text = $"GSB - {titre}"
        lblTitre.Text = titre
        lblSousTitre.Text = sousTitre
        BackColor = Theme.Blanc
        Theme.StyliserEntete(pnlEntete, lblTitre, lblSousTitre)
        pnlActions.BackColor = Theme.BleuClair
        Theme.StyliserBoutonSecondaire(btnFermer)
        pnlFiche.AfficherMessage("Chargement…")
    End Sub

    Private Async Sub FrmFiche_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UseWaitCursor = True
        Try
            Await _remplir(pnlFiche)
        Catch ex As ErreurMetierException
            If Not IsDisposed Then pnlFiche.AfficherMessage(ex.Message)
        Finally
            UseWaitCursor = False
        End Try
    End Sub

    ' ------------------------------------------------------------------
    ' Fiches prêtes à l'emploi
    ' ------------------------------------------------------------------

    ''' <summary>Lecture seule d'un compte-rendu validé d'un membre de l'équipe.</summary>
    Public Shared Function CompteRendu(service As ServiceEquipe, utilisateur As UtilisateurConnecte, numero As Integer, auteur As String) As FrmFiche
        Return New FrmFiche($"Compte-rendu n° {numero}", $"Rédigé par {auteur} · lecture seule",
            Async Function(p)
                Dim r = Await Task.Run(Function() service.ChargerRapport(utilisateur, numero))
                AfficherCompteRendu(p, r)
            End Function)
    End Function

    ''' <summary>Synthèse de l'activité d'un membre de l'équipe sur une période.</summary>
    Public Shared Function SyntheseMembre(service As ServiceEquipe, utilisateur As UtilisateurConnecte, membre As ActiviteMembre,
                                          debut As Date, fin As Date) As FrmFiche
        Return New FrmFiche($"Activité de {membre.NomComplet}", If(membre.NomRegion Is Nothing, "", $"Région {membre.NomRegion}"),
            Async Function(p)
                Dim s = Await Task.Run(Function() service.SyntheseMembre(utilisateur, membre.Matricule, debut, fin))
                RenduSynthese.Afficher(p, s, membre.NomComplet)
            End Function)
    End Function

    Private Shared Sub AfficherCompteRendu(p As PanneauFiche, r As RapportVisite)
        Dim culture = CultureInfo.CurrentCulture
        p.SuspendLayout()
        p.Vider()
        p.AjouterTitre(r.NomPraticien)
        p.AjouterSousTitre($"Visite du {r.DateVisite:dddd d MMMM yyyy}")
        p.AjouterBadge("Validé", Color.FromArgb(232, 245, 233), Color.FromArgb(46, 125, 50))

        p.AjouterSection("Visite")
        If r.NumeroRemplacant.HasValue Then p.AjouterInformation("Personne rencontrée", $"{r.NomRemplacant} (remplaçant)")
        p.AjouterInformation("Motif", If(r.CodeMotif = Motif.CodeAutre, $"{r.LibelleMotif} : {r.PrecisionMotif}", r.LibelleMotif))
        p.AjouterInformation("Produits présentés", If(r.NomsProduitsPresentes.Count = 0, "aucun", String.Join(", ", r.NomsProduitsPresentes)))
        p.AjouterInformation("Confiance du praticien", If(r.CoefConfiance.HasValue, $"{r.CoefConfiance} sur 5", Nothing))
        p.AjouterInformation("Prochaine visite prévue", r.DateProchaineVisite?.ToString("dd/MM/yyyy"))

        p.AjouterSection("Bilan")
        p.AjouterTexte(r.Bilan)

        p.AjouterSection($"Échantillons offerts ({r.TotalEchantillons})")
        If r.Echantillons.Count = 0 Then
            p.AjouterTexte("Aucun échantillon.", Theme.TexteGris, italique:=True)
        Else
            p.AjouterTableau({("Produit", 70.0F), ("Quantité", 30.0F)},
                             r.Echantillons.Select(Function(x) New Object() {x.NomCommercial, x.Quantite}))
        End If

        p.AjouterSection("Traçabilité")
        p.AjouterInformation("Saisi le", r.DateSaisie?.ToString("dd/MM/yyyy à HH:mm", culture))
        p.AjouterInformation("Modifié le", r.DateModification?.ToString("dd/MM/yyyy à HH:mm", culture))
        p.AjouterInformation("Validé le", r.DateValidation?.ToString("dd/MM/yyyy à HH:mm", culture))
        p.ResumeLayout()
    End Sub

End Class
