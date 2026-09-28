''' <summary>Issue d'une tentative de connexion.</summary>
Public Enum StatutConnexion
    ''' <summary>Connexion acceptée.</summary>
    Reussie
    ''' <summary>Connexion acceptée mais le mot de passe doit être changé avant d'aller plus loin (EX-07).</summary>
    ChangementMotDePasseRequis
    ''' <summary>Login inconnu ou mot de passe faux (message volontairement identique).</summary>
    IdentifiantsInvalides
    ''' <summary>Compte verrouillé (EX-06).</summary>
    CompteVerrouille
    ''' <summary>Collaborateur parti de l'entreprise ou sans affectation (EX-09).</summary>
    CompteInactif
    ''' <summary>Base de données injoignable.</summary>
    ServeurIndisponible
End Enum
