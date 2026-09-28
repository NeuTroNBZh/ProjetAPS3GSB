Imports GSB.CR.Modeles

''' <summary>
''' Consultation des fiches praticiens et médicaments (EX-21, EX-22).
''' Toutes les méthodes lèvent <see cref="AccesDonneesException"/> en cas de problème de base.
''' </summary>
Public Interface IConsultationDao

    ''' <summary>
    ''' Praticiens dont le nom, le prénom ou la ville contient <paramref name="texte"/> (tous si vide),
    ''' limités au portefeuille de <paramref name="matriculePortefeuille"/> s'il est renseigné.
    ''' </summary>
    Function RechercherPraticiens(texte As String, matriculePortefeuille As String,
                                  inclureInactifs As Boolean, maximum As Integer) As List(Of PraticienResume)

    ''' <summary>Fiche complète d'un praticien. Nothing s'il n'existe pas.</summary>
    Function ChargerFichePraticien(numero As Integer) As FichePraticien

    ''' <summary>Fiche complète d'un médicament. Nothing s'il n'existe pas.</summary>
    Function ChargerFicheMedicament(depotLegal As String) As FicheMedicament

End Interface
