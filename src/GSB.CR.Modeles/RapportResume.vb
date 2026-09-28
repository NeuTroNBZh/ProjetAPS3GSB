''' <summary>Ligne de la liste des comptes-rendus (vue V_RAPPORT_DETAIL).</summary>
Public Class RapportResume

    Public Property Numero As Integer

    Public Property DateVisite As Date

    Public Property Praticien As String = ""

    Public Property Ville As String

    ''' <summary>Nom du remplaçant rencontré, sinon Nothing.</summary>
    Public Property Remplacant As String

    ''' <summary>Libellé du motif (ou la précision pour « Autre »).</summary>
    Public Property Motif As String

    Public Property Etat As EtatRapport

    Public Property DateSaisie As DateTime

    Public Property DateModification As DateTime?

End Class
