''' <summary>Posologie selon le type d'individu, la présentation et le dosage — table PRESCRIRE.</summary>
Public Class Posologie

    Public Property CodeTypeIndividu As String = ""

    Public Property CodePresentation As String = ""

    Public Property CodeDosage As String = ""

    Public Property TypeIndividu As String = ""

    Public Property Presentation As String = ""

    ''' <summary>Dosage lisible (ex. « 500 mg »).</summary>
    Public Property Dosage As String = ""

    Public Property Texte As String = ""

End Class
