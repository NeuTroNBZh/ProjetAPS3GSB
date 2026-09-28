Imports GSB.CR.Modeles

''' <summary>Implémentation Oracle de <see cref="IEquipeDao"/> (vue V_AFFECTATION_EN_COURS).</summary>
Public Class EquipeDao
    Inherits DaoOracle
    Implements IEquipeDao

    Public Sub New(connexion As ConnexionOracle)
        MyBase.New(connexion)
    End Sub

    Public Function ListerMembres(perimetre As Perimetre) As List(Of MembreEquipe) Implements IEquipeDao.ListerMembres
        Dim sql = $"select col_matricule, col_nom, col_prenom, pro_code, reg_code, reg_nom
                      from V_AFFECTATION_EN_COURS
                     where pro_code in ('VIS', 'DEL')
                       and {SqlPerimetre.Condition("col_matricule", perimetre)}
                     order by reg_nom, col_nom, col_prenom"
        Return Lister("Lecture de l'équipe impossible.", sql,
                      Sub(cmd) SqlPerimetre.Lier(cmd, perimetre),
                      Function(l) New MembreEquipe() With {
                          .Matricule = l.GetString(0), .Nom = l.GetString(1), .Prenom = l.GetString(2),
                          .Profil = ProfilCodes.DepuisCode(l.GetString(3)),
                          .CodeRegion = TexteOuRien(l, 4), .NomRegion = TexteOuRien(l, 5)})
    End Function

End Class
