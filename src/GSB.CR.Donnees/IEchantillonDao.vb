Imports GSB.CR.Modeles

''' <summary>
''' Dotations d'échantillons et contrôle de stock (EX-33, EX-34).
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base.
''' </summary>
Public Interface IEchantillonDao

    ''' <summary>
    ''' Pour chaque visiteur du périmètre et chaque produit : quantités attribuées (dotations) et distribuées
    ''' (CR validés) sur le mois indiqué (premier jour du mois).
    ''' </summary>
    Function Stock(perimetre As Perimetre, mois As Date) As List(Of LigneStock)

    ''' <summary>
    ''' Crée ou remplace la dotation d'un visiteur pour un produit et un mois.
    ''' Le déclencheur refuse la saisie si <paramref name="saisiPar"/> n'est pas délégué, responsable ou administrateur.
    ''' </summary>
    Sub EnregistrerDotation(matricule As String, depotLegal As String, mois As Date, quantite As Integer, saisiPar As String)

    ''' <summary>Supprime une dotation. Renvoie Faux si elle n'existait pas.</summary>
    Function SupprimerDotation(matricule As String, depotLegal As String, mois As Date) As Boolean

End Interface
