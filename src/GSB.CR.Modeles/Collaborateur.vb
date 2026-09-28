''' <summary>
''' Collaborateur de GSB (visiteur, délégué, responsable ou administrateur) — table COLLABORATEUR.
''' </summary>
Public Class Collaborateur

    ''' <summary>Matricule dans l'entreprise (identifiant).</summary>
    Public Property Matricule As String = ""

    Public Property Nom As String = ""

    Public Property Prenom As String = ""

    Public Property Adresse As String

    Public Property CodePostal As String

    Public Property Ville As String

    Public Property Telephone As String

    Public Property Email As String

    Public Property DateEmbauche As Date

    ''' <summary>Date de départ de l'entreprise (Nothing si toujours présent).</summary>
    Public Property DateDepart As Date?

    ''' <summary>Identifiant de connexion.</summary>
    Public Property Login As String = ""

    ''' <summary>Mot de passe haché (format PBKDF2-SHA256$iterations$sel$cle).</summary>
    Public Property MotDePasseHache As String = ""

    ''' <summary>Vrai si le collaborateur doit changer son mot de passe à la prochaine connexion.</summary>
    Public Property MotDePasseAChanger As Boolean

    ''' <summary>Nombre d'échecs de connexion consécutifs.</summary>
    Public Property NbEchecsConnexion As Integer

    ''' <summary>Vrai si le compte est verrouillé (trop d'échecs ou décision de l'administrateur).</summary>
    Public Property Verrouille As Boolean

    Public Property DerniereConnexion As DateTime?

    ''' <summary>Prénom et nom, pour l'affichage.</summary>
    Public ReadOnly Property NomComplet As String
        Get
            Return $"{Prenom} {Nom}"
        End Get
    End Property

    ''' <summary>Indique si le collaborateur a quitté l'entreprise à la date donnée.</summary>
    Public Function EstParti(aLaDate As Date) As Boolean
        Return DateDepart.HasValue AndAlso DateDepart.Value.Date <= aLaDate.Date
    End Function

End Class
