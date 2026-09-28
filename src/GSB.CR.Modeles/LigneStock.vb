''' <summary>
''' Contrôle de stock d'un produit pour un visiteur et un mois (EX-34) :
''' échantillons attribués (dotation) et distribués (CR validés).
''' </summary>
Public Class LigneStock

    Public Property Matricule As String = ""

    Public Property Visiteur As String = ""

    Public Property DepotLegal As String = ""

    Public Property Produit As String = ""

    ''' <summary>Premier jour du mois.</summary>
    Public Property Mois As Date

    Public Property Attribue As Integer

    Public Property Distribue As Integer

    ''' <summary>Attribué moins distribué : négatif = plus d'échantillons distribués que reçus.</summary>
    Public ReadOnly Property Ecart As Integer
        Get
            Return Attribue - Distribue
        End Get
    End Property

    Public ReadOnly Property EnDepassement As Boolean
        Get
            Return Ecart < 0
        End Get
    End Property

End Class
