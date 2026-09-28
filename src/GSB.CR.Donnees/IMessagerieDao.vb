Imports GSB.CR.Modeles

''' <summary>
''' Messagerie interne entre collaborateurs (EX-50).
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base.
''' </summary>
Public Interface IMessagerieDao

    ''' <summary>Collaborateurs actuellement en poste (tous profils), avec région et secteur.</summary>
    Function ListerAnnuaire() As List(Of MembreEquipe)

    ''' <summary>Messages reçus, du plus récent au plus ancien.</summary>
    Function ListerRecus(matricule As String) As List(Of MessageResume)

    ''' <summary>Messages envoyés, du plus récent au plus ancien, avec le suivi de lecture.</summary>
    Function ListerEnvoyes(matricule As String) As List(Of MessageResume)

    ''' <summary>Message complet avec ses destinataires. Nothing s'il n'existe pas.</summary>
    Function Charger(id As Integer) As MessageDetaille

    ''' <summary>Enregistre le message et ses destinataires (une seule transaction) ; renvoie son numéro.</summary>
    Function Envoyer(expediteur As String, objet As String, contenu As String, destinataires As IEnumerable(Of String)) As Integer

    ''' <summary>Note la lecture du message par ce destinataire (sans effet s'il était déjà lu).</summary>
    Sub MarquerLu(id As Integer, matricule As String)

    Function CompterNonLus(matricule As String) As Integer

End Interface
