Imports GSB.CR.Donnees
Imports GSB.CR.Modeles

''' <summary>Messagerie en mémoire.</summary>
Public Class FauxMessagerieDao
    Implements IMessagerieDao

    Public ReadOnly Messages As New List(Of MessageDetaille)
    Private _prochainId As Integer = 1

    Public Function ListerAnnuaire() As List(Of MembreEquipe) Implements IMessagerieDao.ListerAnnuaire
        Return New List(Of MembreEquipe) From {
            New MembreEquipe() With {.Matricule = "d01", .Nom = "Bedos", .Prenom = "Christian", .Profil = Profil.Delegue, .CodeRegion = "AQU", .NomRegion = "Aquitaine", .CodeSecteur = "O", .LibelleSecteur = "Ouest"},
            New MembreEquipe() With {.Matricule = "a131", .Nom = "Villechalane", .Prenom = "Louis", .Profil = Profil.Visiteur, .CodeRegion = "AQU", .NomRegion = "Aquitaine", .CodeSecteur = "O", .LibelleSecteur = "Ouest"},
            New MembreEquipe() With {.Matricule = "a17", .Nom = "Andre", .Prenom = "David", .Profil = Profil.Visiteur, .CodeRegion = "AQU", .NomRegion = "Aquitaine", .CodeSecteur = "O", .LibelleSecteur = "Ouest"},
            New MembreEquipe() With {.Matricule = "r02", .Nom = "Garnier", .Prenom = "Paul", .Profil = Profil.Responsable, .CodeSecteur = "O", .LibelleSecteur = "Ouest"},
            New MembreEquipe() With {.Matricule = "b19", .Nom = "Bunisset", .Prenom = "Francis", .Profil = Profil.Visiteur, .CodeRegion = "ALS", .NomRegion = "Alsace-Lorraine", .CodeSecteur = "E", .LibelleSecteur = "Est"}}
    End Function

    Public Function ListerRecus(matricule As String) As List(Of MessageResume) Implements IMessagerieDao.ListerRecus
        Return Messages.Where(Function(m) m.Destinataires.Any(Function(d) d.Matricule = matricule)).
            Select(Function(m) New MessageResume() With {.Id = m.Id, .Objet = m.Objet,
                .Lu = m.Destinataires.Single(Function(d) d.Matricule = matricule).DateLecture.HasValue}).ToList()
    End Function

    Public Function ListerEnvoyes(matricule As String) As List(Of MessageResume) Implements IMessagerieDao.ListerEnvoyes
        Return Messages.Where(Function(m) m.MatriculeExpediteur = matricule).
            Select(Function(m) New MessageResume() With {.Id = m.Id, .Objet = m.Objet, .NbDestinataires = m.Destinataires.Count}).ToList()
    End Function

    Public Function Charger(id As Integer) As MessageDetaille Implements IMessagerieDao.Charger
        Return Messages.FirstOrDefault(Function(m) m.Id = id)
    End Function

    Public Function Envoyer(expediteur As String, objet As String, contenu As String, destinataires As IEnumerable(Of String)) As Integer Implements IMessagerieDao.Envoyer
        Dim m As New MessageDetaille() With {.Id = _prochainId, .MatriculeExpediteur = expediteur, .Objet = objet, .Contenu = contenu,
                                             .Destinataires = destinataires.Select(Function(d) New DestinataireMessage() With {.Matricule = d}).ToList()}
        _prochainId += 1
        Messages.Add(m)
        Return m.Id
    End Function

    Public Sub MarquerLu(id As Integer, matricule As String) Implements IMessagerieDao.MarquerLu
        Dim d = Charger(id).Destinataires.Single(Function(x) x.Matricule = matricule)
        If Not d.DateLecture.HasValue Then d.DateLecture = DateTime.Now
    End Sub

    Public Function CompterNonLus(matricule As String) As Integer Implements IMessagerieDao.CompterNonLus
        Return ListerRecus(matricule).Where(Function(m) Not m.Lu).Count()
    End Function

End Class
