Imports GSB.CR.Modeles
Imports Oracle.ManagedDataAccess.Client

''' <summary>
''' Filtre SQL commun « ce collaborateur appartient au périmètre ».
''' Région ou secteur : visiteurs et délégués qui y sont affectés actuellement (vue V_AFFECTATION_EN_COURS).
''' Le code du périmètre est lié au paramètre <c>:cle</c>.
''' </summary>
Friend Module SqlPerimetre

    ''' <summary>Condition SQL sur la colonne de matricule <paramref name="colonne"/>.</summary>
    Public Function Condition(colonne As String, perimetre As Perimetre) As String
        ArgumentNullException.ThrowIfNull(perimetre)
        Select Case perimetre.Type
            Case TypePerimetre.Collaborateur
                Return $"{colonne} = :cle"
            Case TypePerimetre.Region
                Return $"{colonne} in (select col_matricule from V_AFFECTATION_EN_COURS
                                        where pro_code in ('VIS', 'DEL') and reg_code = :cle)"
            Case Else
                Return $"{colonne} in (select col_matricule from V_AFFECTATION_EN_COURS
                                        where pro_code in ('VIS', 'DEL') and sec_code = :cle)"
        End Select
    End Function

    ''' <summary>Ajoute le paramètre <c>:cle</c> (code du périmètre).</summary>
    Public Sub Lier(cmd As OracleCommand, perimetre As Perimetre)
        cmd.Parameters.Add("cle", OracleDbType.Varchar2).Value = perimetre.Code
    End Sub

End Module
