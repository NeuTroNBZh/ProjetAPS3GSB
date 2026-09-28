''' <summary>Conversion entre l'énumération <see cref="Profil"/> et les codes stockés en base.</summary>
Public Module ProfilCodes

    ''' <summary>Convertit un code base (VIS, DEL, RES, ADM) en <see cref="Profil"/>.</summary>
    ''' <exception cref="ArgumentException">Si le code est inconnu.</exception>
    Public Function DepuisCode(code As String) As Profil
        Select Case code
            Case "VIS" : Return Profil.Visiteur
            Case "DEL" : Return Profil.Delegue
            Case "RES" : Return Profil.Responsable
            Case "ADM" : Return Profil.Administrateur
            Case Else : Throw New ArgumentException($"Code profil inconnu : « {code} ».", NameOf(code))
        End Select
    End Function

    ''' <summary>Renvoie le code base correspondant au profil.</summary>
    Public Function VersCode(profil As Profil) As String
        Select Case profil
            Case Profil.Visiteur : Return "VIS"
            Case Profil.Delegue : Return "DEL"
            Case Profil.Responsable : Return "RES"
            Case Else : Return "ADM"
        End Select
    End Function

End Module
