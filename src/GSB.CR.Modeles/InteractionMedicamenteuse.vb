''' <summary>Interaction avec un autre médicament — table INTERAGIR, vue depuis un médicament donné.</summary>
Public Class InteractionMedicamenteuse

    Public Property DepotLegalAutre As String = ""

    Public Property NomAutre As String = ""

    ''' <summary>Vrai si le médicament consulté perturbe l'autre ; faux s'il est perturbé par l'autre.</summary>
    Public Property EstPerturbateur As Boolean

    Public Property Description As String

End Class
