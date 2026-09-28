Imports GSB.CR.Modeles

''' <summary>
''' Lecture et écriture des comptes-rendus de visite.
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base.
''' </summary>
Public Interface IRapportDao

    ''' <summary>
    ''' Rapports des membres du périmètre visités depuis <paramref name="depuis"/>, du plus récent au plus ancien.
    ''' Les brouillons (travail personnel en cours) ne sont inclus que sur demande.
    ''' </summary>
    Function ListerParPerimetre(perimetre As Perimetre, depuis As Date, inclureBrouillons As Boolean) As List(Of RapportResume)

    ''' <summary>Rapport complet (produits présentés et échantillons compris). Nothing s'il n'existe pas.</summary>
    Function Charger(numero As Integer) As RapportVisite

    ''' <summary>Enregistre un nouveau rapport avec ses produits et échantillons (une seule transaction).</summary>
    Function Creer(rapport As RapportVisite) As Integer

    ''' <summary>Met à jour un rapport existant et remplace ses produits et échantillons (une seule transaction).</summary>
    Sub Modifier(rapport As RapportVisite)

    ''' <summary>Supprime un rapport s'il est en brouillon. Renvoie Faux si rien n'a été supprimé.</summary>
    Function SupprimerBrouillon(numero As Integer) As Boolean

    ''' <summary>Trace une session de saisie (temps passé sur le formulaire).</summary>
    Sub AjouterSessionSaisie(numero As Integer, matricule As String, debut As DateTime, fin As DateTime)

End Interface
