# Référence des classes — GSB-CR

> Fichier généré par `scripts/generer-reference.ps1` à partir des commentaires de documentation du code. Ne pas modifier à la main.

Voir [le guide technique](README.md) pour l'architecture et les choix de conception.

## GSB.CR.Modeles

Entités du domaine, sans dépendance (utilisées par toutes les couches). 42 types documentés.

| Type | Rôle |
|---|---|
| [ActiviteMembre](#activitemembre) | Activité d'un membre de l'équipe sur une période (CR validés). |
| [Affectation](#affectation) | Affectation d'un collaborateur : profil et rattachement (région ou secteur) — table AFFECTATION. |
| [Collaborateur](#collaborateur) | Collaborateur de GSB (visiteur, délégué, responsable ou administrateur) — table COLLABORATEUR. |
| [DestinataireMessage](#destinatairemessage) | Destinataire d'un message et date à laquelle il l'a lu. |
| [EchantillonOffert](#echantillonoffert) | Échantillons d'un médicament laissés lors d'une visite — table OFFRIR. |
| [EchantillonsDistribues](#echantillonsdistribues) | Échantillons d'un produit distribués sur une période, et leur coût. |
| [ElementReferentiel](#elementreferentiel) | Élément d'une petite table de référence (composant, type d'individu, présentation, dosage) : un code et un libellé affiché dans les listes. |
| [EntreeJournal](#entreejournal) | Tentative de connexion — table JOURNAL_CONNEXION (EX-74). |
| [EtatPeriodicite](#etatperiodicite) | Situation d'un praticien au regard de la périodicité des visites (tous les 6 à 8 mois). |
| [EtatRapport](#etatrapport) | État d'un compte-rendu de visite (colonne rap_etat : B / V). |
| [FicheCollaborateur](#fichecollaborateur) | Collaborateur vu par l'administrateur : identité, état du compte et affectation en cours. |
| [FicheMedicament](#fichemedicament) | Fiche détaillée d'un médicament (EX-22). |
| [FichePraticien](#fichepraticien) | Fiche détaillée d'un praticien (EX-21) : identité, spécialités, suivi et historique des visites. |
| [GroupeDestinataires](#groupedestinataires) | Groupe de destinataires (tous les membres d'une région ou d'un secteur). |
| [InteractionMedicamenteuse](#interactionmedicamenteuse) | Interaction avec un autre médicament — table INTERAGIR, vue depuis un médicament donné. |
| [LigneComposition](#lignecomposition) | Composant d'un médicament et sa quantité — table CONSTITUER. |
| [LigneStock](#lignestock) | Contrôle de stock d'un produit pour un visiteur et un mois (EX-34) : échantillons attribués (dotation) et distribués (CR validés). |
| [Medicament](#medicament) | Médicament du laboratoire — table MEDICAMENT (vue simplifiée pour la saisie des CR). |
| [MembreEquipe](#membreequipe) | Visiteur ou délégué membre d'un périmètre (région ou secteur). |
| [MessageDetaille](#messagedetaille) | Message complet — tables MESSAGE et MESSAGE_DESTINATAIRE. |
| [MessageResume](#messageresume) | Ligne d'une boîte de réception ou de la liste des messages envoyés. |
| [Motif](#motif) | Motif de visite standardisé — table MOTIF. |
| [Perimetre](#perimetre) | Périmètre d'analyse : un collaborateur, une région ou un secteur. Pour une région ou un secteur, les membres sont les visiteurs et délégués qui y sont affectés actuellement. |
| [Periode](#periode) | Période d'analyse de l'activité (bornes incluses). |
| [Posologie](#posologie) | Posologie selon le type d'individu, la présentation et le dosage — table PRESCRIRE. |
| [Praticien](#praticien) | Praticien visité (médecin, pharmacien, personnel de santé…) — table PRATICIEN. |
| [PraticienARevoir](#praticienarevoir) | Praticien du portefeuille à planifier, avec la raison et la priorité (EX-24). |
| [PraticienResume](#praticienresume) | Ligne de la liste des praticiens : identité, suivi et dernière visite. |
| [Profil](#profil) | Profil applicatif d'un collaborateur (table PROFIL). Détermine le module ouvert après connexion. |
| [ProfilCodes](#profilcodes) | Conversion entre l'énumération `Profil` et les codes stockés en base. |
| [RapportResume](#rapportresume) | Ligne de la liste des comptes-rendus (vue V_RAPPORT_DETAIL). |
| [RapportVisite](#rapportvisite) | Compte-rendu de visite complet — tables RAPPORT_VISITE, PRESENTER et OFFRIR. |
| [Region](#region) | Région commerciale — table REGION. |
| [Repartition](#repartition) | Effectif d'une catégorie (motif, produit…) dans une synthèse d'activité. |
| [Secteur](#secteur) | Secteur géographique — table SECTEUR. |
| [SpecialitePraticien](#specialitepraticien) | Spécialité d'un praticien avec son diplôme le plus haut — table POSSEDER. |
| [SyntheseActivite](#syntheseactivite) | Synthèse de l'activité d'un collaborateur sur une période (EX-23). Seuls les comptes-rendus validés sont comptés. |
| [TypePerimetre](#typeperimetre) | Étendue d'une analyse d'activité. |
| [TypePraticien](#typepraticien) | Type de praticien (médecin de ville, hospitalier, pharmacien…) — table TYPE_PRATICIEN. |
| [UtilisateurConnecte](#utilisateurconnecte) | Utilisateur authentifié pour la session en cours : le collaborateur et son affectation actuelle. |
| [VisitePraticien](#visitepraticien) | Visite validée figurant dans l'historique d'un praticien. |
| [VisitesDuMois](#visitesdumois) | Nombre de visites validées d'un mois. |

### ActiviteMembre

Activité d'un membre de l'équipe sur une période (CR validés).

| Membre | Description |
|---|---|
| `NbBrouillons` | Brouillons en attente (toutes dates confondues). |

### Affectation

Affectation d'un collaborateur : profil et rattachement (région ou secteur) — table AFFECTATION.

| Membre | Description |
|---|---|
| `CodeRegion` | Région de rattachement (visiteur et délégué), sinon Nothing. |
| `CodeSecteur` | Secteur (celui du responsable, ou celui de la région). |
| `DateFin` | Date de fin (Nothing pour l'affectation en cours). |
| `LibelleRattachement` | Libellé du rattachement pour l'affichage (« Région Aquitaine », « Secteur Est »…). |

### Collaborateur

Collaborateur de GSB (visiteur, délégué, responsable ou administrateur) — table COLLABORATEUR.

| Membre | Description |
|---|---|
| `Matricule` | Matricule dans l'entreprise (identifiant). |
| `DateDepart` | Date de départ de l'entreprise (Nothing si toujours présent). |
| `Login` | Identifiant de connexion. |
| `MotDePasseHache` | Mot de passe haché (format PBKDF2-SHA256$iterations$sel$cle). |
| `MotDePasseAChanger` | Vrai si le collaborateur doit changer son mot de passe à la prochaine connexion. |
| `NbEchecsConnexion` | Nombre d'échecs de connexion consécutifs. |
| `Verrouille` | Vrai si le compte est verrouillé (trop d'échecs ou décision de l'administrateur). |
| `NomComplet` | Prénom et nom, pour l'affichage. |
| `EstParti(DateTime)` | Indique si le collaborateur a quitté l'entreprise à la date donnée. |

### DestinataireMessage

Destinataire d'un message et date à laquelle il l'a lu.

| Membre | Description |
|---|---|
| `DateLecture` | Nothing tant que le message n'a pas été ouvert. |

### EchantillonOffert

Échantillons d'un médicament laissés lors d'une visite — table OFFRIR.

| Membre | Description |
|---|---|
| `NomCommercial` | Nom commercial (affichage). |
| `Quantite` | Nombre d'échantillons, à l'unité près. |

### EchantillonsDistribues

Échantillons d'un produit distribués sur une période, et leur coût.

### ElementReferentiel

Élément d'une petite table de référence (composant, type d'individu, présentation, dosage) : un code et un libellé affiché dans les listes.

### EntreeJournal

Tentative de connexion — table JOURNAL_CONNEXION (EX-74).

| Membre | Description |
|---|---|
| `LoginSaisi` | Identifiant saisi (même s'il ne correspond à aucun compte). |
| `NomCollaborateur` | Collaborateur correspondant, Nothing si le login est inconnu. |

### EtatPeriodicite

Situation d'un praticien au regard de la périodicité des visites (tous les 6 à 8 mois).

| Membre | Description |
|---|---|
| `AJour` | Visité il y a moins de 6 mois. |
| `ARevoirBientot` | Visité il y a 6 à 8 mois : à planifier. |
| `ARevoir` | Visité il y a plus de 8 mois. |
| `JamaisVisite` | Aucune visite validée. |

### EtatRapport

État d'un compte-rendu de visite (colonne rap_etat : B / V).

| Membre | Description |
|---|---|
| `Brouillon` | Enregistré mais incomplet ou non définitif ; exclu des statistiques. |
| `Valide` | Définitif et complet ; pris en compte dans les statistiques. |

### FicheCollaborateur

Collaborateur vu par l'administrateur : identité, état du compte et affectation en cours.

| Membre | Description |
|---|---|
| `AffectationEnCours` | Affectation en cours (Nothing si le collaborateur est parti ou sans affectation). |
| `TaillePortefeuille` | Nombre de praticiens actuellement dans son portefeuille. |

### FicheMedicament

Fiche détaillée d'un médicament (EX-22).

### FichePraticien

Fiche détaillée d'un praticien (EX-21) : identité, spécialités, suivi et historique des visites.

| Membre | Description |
|---|---|
| `NomVisiteur` | Visiteur qui suit actuellement le praticien, sinon Nothing. |
| `Visites` | Visites validées, de la plus récente à la plus ancienne (titulaire ou remplaçant). |

### GroupeDestinataires

Groupe de destinataires (tous les membres d'une région ou d'un secteur).

### InteractionMedicamenteuse

Interaction avec un autre médicament — table INTERAGIR, vue depuis un médicament donné.

| Membre | Description |
|---|---|
| `EstPerturbateur` | Vrai si le médicament consulté perturbe l'autre ; faux s'il est perturbé par l'autre. |

### LigneComposition

Composant d'un médicament et sa quantité — table CONSTITUER.

### LigneStock

Contrôle de stock d'un produit pour un visiteur et un mois (EX-34) : échantillons attribués (dotation) et distribués (CR validés).

| Membre | Description |
|---|---|
| `Mois` | Premier jour du mois. |
| `Ecart` | Attribué moins distribué : négatif = plus d'échantillons distribués que reçus. |

### Medicament

Médicament du laboratoire — table MEDICAMENT (vue simplifiée pour la saisie des CR).

| Membre | Description |
|---|---|
| `DepotLegal` | Numéro de dépôt légal (identifiant). |

### MembreEquipe

Visiteur ou délégué membre d'un périmètre (région ou secteur).

### MessageDetaille

Message complet — tables MESSAGE et MESSAGE_DESTINATAIRE.

### MessageResume

Ligne d'une boîte de réception ou de la liste des messages envoyés.

| Membre | Description |
|---|---|
| `Correspondant` | Expéditeur (message reçu) ou liste abrégée des destinataires (message envoyé). |
| `Lu` | Message reçu : vrai si déjà ouvert. |
| `NbDestinataires` | Message envoyé : nombre de destinataires et nombre de lectures. |

### Motif

Motif de visite standardisé — table MOTIF.

| Membre | Description |
|---|---|
| `CodeAutre` | Code du motif « Autre », qui exige une précision libre. |

### Perimetre

Périmètre d'analyse : un collaborateur, une région ou un secteur. Pour une région ou un secteur, les membres sont les visiteurs et délégués qui y sont affectés actuellement.

| Membre | Description |
|---|---|
| `Code` | Matricule, code région ou code secteur selon le type. |

### Periode

Période d'analyse de l'activité (bornes incluses).

### Posologie

Posologie selon le type d'individu, la présentation et le dosage — table PRESCRIRE.

| Membre | Description |
|---|---|
| `Dosage` | Dosage lisible (ex. « 500 mg »). |

### Praticien

Praticien visité (médecin, pharmacien, personnel de santé…) — table PRATICIEN.

| Membre | Description |
|---|---|
| `Numero` | Numéro du praticien (Nothing tant qu'il n'est pas enregistré). |
| `CodeType` | Code du type de praticien (MV, MH, PO, PH, PS). |
| `CoefNotoriete` | Coefficient de notoriété (donnée achetée). |
| `NomComplet` | Nom puis prénom, pour les listes triées. |
| `ToString()` | Libellé affiché dans les listes déroulantes (nom, prénom et ville). |

### PraticienARevoir

Praticien du portefeuille à planifier, avec la raison et la priorité (EX-24).

| Membre | Description |
|---|---|
| `ProchainePrevueDepassee` | Vrai si la prochaine visite prévue lors du dernier CR est dépassée. |
| `JoursDepuisDerniereVisite` | Jours écoulés depuis la dernière visite (Nothing si jamais visité). |
| `Priorite` | 1 = le plus urgent. |

### PraticienResume

Ligne de la liste des praticiens : identité, suivi et dernière visite.

| Membre | Description |
|---|---|
| `MatriculeVisiteur` | Visiteur qui suit actuellement le praticien (portefeuille), sinon Nothing. |
| `DateDerniereVisite` | Date de la dernière visite validée (en tant que titulaire). |
| `DateProchainePrevue` | Prochaine visite prévue lors de la dernière visite. |

### Profil

Profil applicatif d'un collaborateur (table PROFIL). Détermine le module ouvert après connexion.

### ProfilCodes

Conversion entre l'énumération `Profil` et les codes stockés en base.

| Membre | Description |
|---|---|
| `DepuisCode(String)` | Convertit un code base (VIS, DEL, RES, ADM) en `Profil`. |
| `VersCode(Profil)` | Renvoie le code base correspondant au profil. |

### RapportResume

Ligne de la liste des comptes-rendus (vue V_RAPPORT_DETAIL).

| Membre | Description |
|---|---|
| `MatriculeAuteur` | Auteur du compte-rendu (utile pour les CR d'une équipe). |
| `Remplacant` | Nom du remplaçant rencontré, sinon Nothing. |
| `Motif` | Libellé du motif (ou la précision pour « Autre »). |

### RapportVisite

Compte-rendu de visite complet — tables RAPPORT_VISITE, PRESENTER et OFFRIR.

| Membre | Description |
|---|---|
| `Numero` | Numéro du rapport (Nothing pour un nouveau rapport). |
| `MatriculeAuteur` | Matricule de l'auteur (imposé par l'application : le collaborateur connecté). |
| `NumeroPraticien` | Praticien titulaire du cabinet, pour lequel la visite est faite. |
| `NomPraticien` | Nom du praticien titulaire (affichage). |
| `NumeroRemplacant` | Remplaçant réellement rencontré (Nothing si c'est le titulaire). |
| `NomRemplacant` | Nom du remplaçant (affichage). |
| `LibelleMotif` | Libellé du motif (affichage, renseigné à la lecture). |
| `PrecisionMotif` | Précision libre, uniquement pour le motif « Autre ». |
| `CoefConfiance` | Confiance du praticien dans les produits GSB, de 1 (faible) à 5 (forte). |
| `ProduitsPresentes` | Dépôts légaux des produits présentés, dans l'ordre (2 au maximum). |
| `NomsProduitsPresentes` | Noms commerciaux des produits présentés, dans l'ordre (affichage, renseignés à la lecture). |
| `Echantillons` | Échantillons offerts (indépendants des produits présentés). |
| `TotalEchantillons` | Nombre total d'échantillons offerts. |

### Region

Région commerciale — table REGION.

### Repartition

Effectif d'une catégorie (motif, produit…) dans une synthèse d'activité.

### Secteur

Secteur géographique — table SECTEUR.

### SpecialitePraticien

Spécialité d'un praticien avec son diplôme le plus haut — table POSSEDER.

| Membre | Description |
|---|---|
| `CoefPrescription` | Coefficient de prescription dans la spécialité (donnée achetée). |

### SyntheseActivite

Synthèse de l'activité d'un collaborateur sur une période (EX-23). Seuls les comptes-rendus validés sont comptés.

| Membre | Description |
|---|---|
| `NbPraticiens` | Nombre de praticiens (titulaires) différents visités. |
| `NbVisitesRemplacant` | Visites où un remplaçant a été rencontré. |
| `ConfianceMoyenne` | Coefficient de confiance moyen (1 à 5), Nothing sans visite. |
| `TempsSaisieMoyen` | Temps de saisie moyen par compte-rendu, en secondes (Nothing si non mesuré). |
| `NbBrouillons` | Brouillons en attente de validation (toutes dates confondues). |
| `ParMois` | Visites par mois, chaque mois de la période présent (zéro compris). |

### TypePerimetre

Étendue d'une analyse d'activité.

| Membre | Description |
|---|---|
| `Collaborateur` | Un seul collaborateur (« Mon activité »). |
| `Region` | Les visiteurs et le délégué d'une région (module Délégué). |
| `Secteur` | Les visiteurs et délégués de toutes les régions d'un secteur (module Responsable). |

### TypePraticien

Type de praticien (médecin de ville, hospitalier, pharmacien…) — table TYPE_PRATICIEN.

### UtilisateurConnecte

Utilisateur authentifié pour la session en cours : le collaborateur et son affectation actuelle.

### VisitePraticien

Visite validée figurant dans l'historique d'un praticien.

| Membre | Description |
|---|---|
| `Visiteur` | Collaborateur ayant effectué la visite. |
| `VuCommeRemplacant` | Vrai si le praticien a été vu en tant que remplaçant d'un autre praticien. |
| `PraticienLie` | Praticien lié : le titulaire remplacé (si `VuCommeRemplacant`), ou le remplaçant rencontré à la place du praticien. Nothing sinon. |

### VisitesDuMois

Nombre de visites validées d'un mois.

| Membre | Description |
|---|---|
| `Mois` | Premier jour du mois. |

## GSB.CR.Donnees

Accès à Oracle : configuration, connexions, DAO et leurs interfaces. 23 types documentés.

| Type | Rôle |
|---|---|
| [AccesDonneesException](#accesdonneesexception) | Erreur d'accès à la base de données (serveur injoignable, requête refusée…). Masque les types Oracle aux couches supérieures. |
| [ActiviteDao](#activitedao) | Implémentation Oracle de `IActiviteDao`. |
| [AdministrationDao](#administrationdao) | Implémentation Oracle de `IAdministrationDao`. |
| [CollaborateurDao](#collaborateurdao) | Implémentation Oracle de `ICollaborateurDao` (tables COLLABORATEUR, JOURNAL_CONNEXION, vue V_AFFECTATION_EN_COURS). Requêtes paramétrées uniquement. |
| [ConfigurationOracle](#configurationoracle) | Paramètres de connexion au serveur Oracle 19c. Lus depuis appsettings.json (serveur) et appsettings.Local.json (identifiants, non versionné). |
| [ConnexionOracle](#connexionoracle) | Fabrique de connexions Oracle pour la couche Données. Chaque appel renvoie une connexion ouverte à utiliser dans un bloc Using. |
| [ConsultationDao](#consultationdao) | Implémentation Oracle de `IConsultationDao`. |
| [DaoOracle](#daooracle) | Base commune des DAO Oracle : ouverture de connexion, transactions, traduction des erreurs Oracle en `AccesDonneesException` et lecture des valeurs nullables. |
| [EchantillonDao](#echantillondao) | Implémentation Oracle de `IEchantillonDao` (table DOTATION, vue V_STOCK_ECHANTILLON). |
| [EquipeDao](#equipedao) | Implémentation Oracle de `IEquipeDao` (vue V_AFFECTATION_EN_COURS). |
| [IActiviteDao](#iactivitedao) | Statistiques d'activité d'un collaborateur, d'une région ou d'un secteur (EX-23, EX-31, EX-40). Seuls les comptes-rendus validés sont comptés. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base. |
| [IAdministrationDao](#iadministrationdao) | Administration (EX-70 à EX-74) : comptes, affectations, portefeuilles, référentiels et journal. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base (avec le numéro d'erreur Oracle, ex. doublon de matricule ou de login). |
| [ICollaborateurDao](#icollaborateurdao) | Accès aux collaborateurs et à leur authentification. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base. |
| [IConsultationDao](#iconsultationdao) | Consultation des fiches praticiens et médicaments (EX-21, EX-22). Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base. |
| [IEchantillonDao](#iechantillondao) | Dotations d'échantillons et contrôle de stock (EX-33, EX-34). Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base. |
| [IEquipeDao](#iequipedao) | Composition des équipes (région, secteur). Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base. |
| [IMessagerieDao](#imessageriedao) | Messagerie interne entre collaborateurs (EX-50). Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base. |
| [IRapportDao](#irapportdao) | Lecture et écriture des comptes-rendus de visite. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base. |
| [IReferentielDao](#ireferentieldao) | Données de référence utiles à la saisie des comptes-rendus : praticiens, médicaments, motifs. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base. |
| [MessagerieDao](#messageriedao) | Implémentation Oracle de `IMessagerieDao` (tables MESSAGE et MESSAGE_DESTINATAIRE). |
| [RapportDao](#rapportdao) | Implémentation Oracle de `IRapportDao` (tables RAPPORT_VISITE, PRESENTER, OFFRIR, SESSION_SAISIE et vue V_RAPPORT_DETAIL). |
| [ReferentielDao](#referentieldao) | Implémentation Oracle de `IReferentielDao`. |
| [SqlPerimetre](#sqlperimetre) | Filtre SQL commun « ce collaborateur appartient au périmètre ». Région ou secteur : visiteurs et délégués qui y sont affectés actuellement (vue V_AFFECTATION_EN_COURS). Le code du périmètre est lié au paramètre `:cle`. |

### AccesDonneesException

Erreur d'accès à la base de données (serveur injoignable, requête refusée…). Masque les types Oracle aux couches supérieures.

| Membre | Description |
|---|---|
| `CodeOracle` | Numéro de l'erreur Oracle (ex. 1 = valeur unique en double, 2290 = contrainte CHECK), 0 si inconnu. |
| `EstDoublon` | Vrai si l'erreur vient d'une contrainte d'unicité (ORA-00001). |

### ActiviteDao

Implémentation Oracle de `IActiviteDao`.

### AdministrationDao

Implémentation Oracle de `IAdministrationDao`.

| Membre | Description |
|---|---|
| `Attribuer(OracleConnection, Int32, String, DateTime)` | Un seul visiteur en cours par praticien : le suivi en cours est clos la veille, ou supprimé s'il commençait le jour même (correction d'une attribution), puis le nouveau suivi est ouvert. |

### CollaborateurDao

Implémentation Oracle de `ICollaborateurDao` (tables COLLABORATEUR, JOURNAL_CONNEXION, vue V_AFFECTATION_EN_COURS). Requêtes paramétrées uniquement.

### ConfigurationOracle

Paramètres de connexion au serveur Oracle 19c. Lus depuis appsettings.json (serveur) et appsettings.Local.json (identifiants, non versionné).

| Membre | Description |
|---|---|
| `Hote` | Adresse du serveur Oracle. |
| `Port` | Port du listener Oracle (1521 par défaut). |
| `Service` | Nom de service Oracle (ex. FREEPDB1). |
| `Utilisateur` | Utilisateur (schéma) Oracle de l'application. |
| `MotDePasse` | Mot de passe de l'utilisateur Oracle. |
| `Schema` | Schéma des tables, si l'utilisateur n'en est pas le propriétaire (facultatif). En production, les postes se connectent avec le compte applicatif GSB_APP et travaillent sur le schéma GSB. |
| `Charger(String)` | Charge la configuration depuis le dossier indiqué (par défaut celui de l'exécutable). |
| `EstComplete` | Indique si tous les paramètres nécessaires à la connexion sont renseignés. |
| `ChaineDeConnexion()` | Construit la chaîne de connexion ODP.NET (format EZConnect). |
| `InstructionSchema()` | Instruction à exécuter après l'ouverture d'une connexion pour travailler sur `Schema`, ou chaîne vide si aucun schéma n'est configuré. Un nom d'objet ne se passe pas en paramètre SQL : il est donc contrôlé (identifiant Oracle simple) avant d'être inséré dans l'instruction. |

### ConnexionOracle

Fabrique de connexions Oracle pour la couche Données. Chaque appel renvoie une connexion ouverte à utiliser dans un bloc Using.

| Membre | Description |
|---|---|
| `New(ConfigurationOracle)` | Crée la fabrique à partir d'une configuration Oracle. |
| `Ouvrir()` | Ouvre et renvoie une nouvelle connexion, placée sur le schéma configuré le cas échéant. |
| `Tester()` | Vérifie que la base répond. Renvoie la version du serveur Oracle. |

### ConsultationDao

Implémentation Oracle de `IConsultationDao`.

| Membre | Description |
|---|---|
| `SousRequeteDerniereVisite` | Dernière visite validée de chaque praticien (en tant que titulaire) et prochaine visite prévue. |

### DaoOracle

Base commune des DAO Oracle : ouverture de connexion, transactions, traduction des erreurs Oracle en `AccesDonneesException` et lecture des valeurs nullables.

| Membre | Description |
|---|---|
| `Executer``1(String, OracleConnection, ``0})` | Ouvre une connexion, exécute le traitement et traduit les erreurs Oracle. |
| `ExecuterTransaction``1(String, OracleConnection, ``0})` | Exécute le traitement dans une transaction : validée s'il se termine normalement, annulée en cas d'erreur (aucune écriture partielle). |
| `ExecuterMiseAJour(String, String, Action)` | Exécute une requête de mise à jour paramétrée et renvoie le nombre de lignes touchées. |
| `Lister``1(String, String, Action, OracleDataReader, ``0})` | Exécute une requête et transforme chaque ligne avec . |
| `Lister``1(OracleConnection, String, Action, OracleDataReader, ``0})` | Variante de `OracleDataReader,``0})` sur une connexion déjà ouverte. |
| `Parametre(OracleCommand, String, OracleDbType, Object)` | Ajoute un paramètre ; Nothing est envoyé comme NULL. |
| `EntierSortie(OracleParameter)` | Lit la valeur d'un paramètre de sortie numérique (RETURNING … INTO). |

### EchantillonDao

Implémentation Oracle de `IEchantillonDao` (table DOTATION, vue V_STOCK_ECHANTILLON).

### EquipeDao

Implémentation Oracle de `IEquipeDao` (vue V_AFFECTATION_EN_COURS).

### IActiviteDao

Statistiques d'activité d'un collaborateur, d'une région ou d'un secteur (EX-23, EX-31, EX-40). Seuls les comptes-rendus validés sont comptés. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base.

| Membre | Description |
|---|---|
| `ChargerSynthese(Perimetre, DateTime, DateTime)` | Synthèse des CR validés du périmètre visités entre et (inclus). `ParMois` ne contient que les mois ayant au moins une visite. |
| `ActiviteParMembre(Perimetre, DateTime, DateTime)` | Activité de chaque membre actuel du périmètre (membres sans visite compris). |

### IAdministrationDao

Administration (EX-70 à EX-74) : comptes, affectations, portefeuilles, référentiels et journal. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base (avec le numéro d'erreur Oracle, ex. doublon de matricule ou de login).

| Membre | Description |
|---|---|
| `ListerCollaborateurs()` | Tous les collaborateurs (partis compris) avec affectation en cours et taille du portefeuille. |
| `ListerAffectations(String)` | Historique des affectations, de la plus récente à la plus ancienne. |
| `CreerCollaborateur(Collaborateur, String, Affectation)` | Crée le compte (mot de passe à changer) et sa première affectation, en une transaction. |
| `ModifierCollaborateur(Collaborateur)` | Met à jour l'identité, les coordonnées et le login. |
| `ChangerAffectation(String, Affectation, DateTime)` | Clôt l'affectation en cours la veille de et ouvre la nouvelle. |
| `EnregistrerDepart(String, DateTime)` | Départ : date de départ, affectation et portefeuille clos à cette date (une transaction). |
| `ReinitialiserMotDePasse(String, String)` | Nouveau mot de passe provisoire (à changer), compte déverrouillé. |
| `DefinirVerrouillage(String, Boolean)` | Verrouille ou déverrouille le compte ; remet le compteur d'échecs à zéro. |
| `ListerPraticiensSansVisiteur()` | Praticiens actifs qu'aucun visiteur ne suit actuellement. |
| `AttribuerPraticiens(IEnumerable, String, DateTime)` | Confie des praticiens à un visiteur à partir de : le suivi en cours est clos la veille (ou remplacé s'il commençait le même jour). Une transaction. |
| `TransfererPortefeuille(String, String, DateTime)` | Transfère tout le portefeuille d'un visiteur à un autre ; renvoie le nombre de praticiens transférés. |
| `CreerMotif(String, String)` | Crée un motif, placé juste avant « Autre » qui reste en dernier. |
| `ListerDosages()` | Dosages, libellé lisible (« 500 mg »), par unité puis quantité. |
| `AjouterInteraction(String, String, String)` | Enregistre que perturbe l'effet de . |
| `ListerJournal(DateTime, DateTime, String, Boolean, Int32)` | Tentatives de connexion entre deux dates (incluses), les plus récentes d'abord. |

### ICollaborateurDao

Accès aux collaborateurs et à leur authentification. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base.

| Membre | Description |
|---|---|
| `TrouverParLogin(String)` | Recherche un collaborateur par son login (insensible à la casse). Nothing si inconnu. |
| `TrouverAffectationEnCours(String)` | Affectation en cours du collaborateur. Nothing s'il n'en a pas (ou s'il est parti). |
| `EnregistrerEchec(String, Int32)` | Incrémente le compteur d'échecs et verrouille le compte s'il atteint . Renvoie le nouveau nombre d'échecs. |
| `EnregistrerSucces(String)` | Remet le compteur d'échecs à zéro et mémorise la date de connexion. |
| `ChangerMotDePasse(String, String)` | Remplace le mot de passe (déjà haché) et lève l'obligation de le changer. |
| `Journaliser(String, String, Boolean)` | Trace une tentative de connexion dans JOURNAL_CONNEXION. |

### IConsultationDao

Consultation des fiches praticiens et médicaments (EX-21, EX-22). Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base.

| Membre | Description |
|---|---|
| `RechercherPraticiens(String, Perimetre, Boolean, Int32)` | Praticiens dont le nom, le prénom ou la ville contient (tous si vide), limités aux portefeuilles des membres de s'il est renseigné. |
| `ChargerFichePraticien(Int32)` | Fiche complète d'un praticien. Nothing s'il n'existe pas. |
| `ChargerFicheMedicament(String)` | Fiche complète d'un médicament. Nothing s'il n'existe pas. |

### IEchantillonDao

Dotations d'échantillons et contrôle de stock (EX-33, EX-34). Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base.

| Membre | Description |
|---|---|
| `Stock(Perimetre, DateTime)` | Pour chaque visiteur du périmètre et chaque produit : quantités attribuées (dotations) et distribuées (CR validés) sur le mois indiqué (premier jour du mois). |
| `EnregistrerDotation(String, String, DateTime, Int32, String)` | Crée ou remplace la dotation d'un visiteur pour un produit et un mois. Le déclencheur refuse la saisie si n'est pas délégué, responsable ou administrateur. |
| `SupprimerDotation(String, String, DateTime)` | Supprime une dotation. Renvoie Faux si elle n'existait pas. |

### IEquipeDao

Composition des équipes (région, secteur). Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base.

| Membre | Description |
|---|---|
| `ListerMembres(Perimetre)` | Visiteurs et délégués actuellement affectés au périmètre, triés par région puis par nom. |

### IMessagerieDao

Messagerie interne entre collaborateurs (EX-50). Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base.

| Membre | Description |
|---|---|
| `ListerAnnuaire()` | Collaborateurs actuellement en poste (tous profils), avec région et secteur. |
| `ListerRecus(String)` | Messages reçus, du plus récent au plus ancien. |
| `ListerEnvoyes(String)` | Messages envoyés, du plus récent au plus ancien, avec le suivi de lecture. |
| `Charger(Int32)` | Message complet avec ses destinataires. Nothing s'il n'existe pas. |
| `Envoyer(String, String, String, IEnumerable)` | Enregistre le message et ses destinataires (une seule transaction) ; renvoie son numéro. |
| `MarquerLu(Int32, String)` | Note la lecture du message par ce destinataire (sans effet s'il était déjà lu). |

### IRapportDao

Lecture et écriture des comptes-rendus de visite. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base.

| Membre | Description |
|---|---|
| `ListerParPerimetre(Perimetre, DateTime, Boolean)` | Rapports des membres du périmètre visités depuis , du plus récent au plus ancien. Les brouillons (travail personnel en cours) ne sont inclus que sur demande. |
| `Charger(Int32)` | Rapport complet (produits présentés et échantillons compris). Nothing s'il n'existe pas. |
| `Creer(RapportVisite)` | Enregistre un nouveau rapport avec ses produits et échantillons (une seule transaction). |
| `Modifier(RapportVisite)` | Met à jour un rapport existant et remplace ses produits et échantillons (une seule transaction). |
| `SupprimerBrouillon(Int32)` | Supprime un rapport s'il est en brouillon. Renvoie Faux si rien n'a été supprimé. |
| `AjouterSessionSaisie(Int32, String, DateTime, DateTime)` | Trace une session de saisie (temps passé sur le formulaire). |

### IReferentielDao

Données de référence utiles à la saisie des comptes-rendus : praticiens, médicaments, motifs. Toutes les méthodes lèvent `AccesDonneesException` en cas de problème de base.

| Membre | Description |
|---|---|
| `ListerPortefeuille(String)` | Praticiens actifs actuellement dans le portefeuille du collaborateur, triés par nom. |
| `RechercherPraticiens(String, Int32)` | Praticiens actifs dont le nom ou le prénom commence par (tous si vide), limités à résultats. |
| `TrouverPraticien(Int32)` | Praticien par son numéro (même inactif). Nothing s'il n'existe pas. |
| `CreerPraticien(Praticien)` | Enregistre un nouveau praticien (ex. remplaçant) et renvoie son numéro. |
| `ListerMedicaments()` | Tous les médicaments (actifs et inactifs), triés par nom commercial. |
| `ListerMotifs()` | Tous les motifs (actifs et inactifs), dans l'ordre d'affichage. |

### MessagerieDao

Implémentation Oracle de `IMessagerieDao` (tables MESSAGE et MESSAGE_DESTINATAIRE).

### RapportDao

Implémentation Oracle de `IRapportDao` (tables RAPPORT_VISITE, PRESENTER, OFFRIR, SESSION_SAISIE et vue V_RAPPORT_DETAIL).

| Membre | Description |
|---|---|
| `EcrireLignes(OracleConnection, Int32, RapportVisite)` | Insère les produits présentés (ordre 1, 2) et les échantillons du rapport. |

### ReferentielDao

Implémentation Oracle de `IReferentielDao`.

### SqlPerimetre

Filtre SQL commun « ce collaborateur appartient au périmètre ». Région ou secteur : visiteurs et délégués qui y sont affectés actuellement (vue V_AFFECTATION_EN_COURS). Le code du périmètre est lié au paramètre `:cle`.

| Membre | Description |
|---|---|
| `Condition(String, Perimetre)` | Condition SQL sur la colonne de matricule . |
| `Lier(OracleCommand, Perimetre)` | Ajoute le paramètre `:cle` (code du périmètre). |

## GSB.CR.Metier

Règles de gestion, droits et services appelés par les écrans. 24 types documentés.

| Type | Rôle |
|---|---|
| [Autorisations](#autorisations) | Droits d'accès aux modules selon le profil (EX-02). Le délégué est aussi un visiteur : il a les modules du visiteur en plus des siens. |
| [ErreurMetierException](#erreurmetierexception) | Erreur destinée à l'utilisateur : action refusée par une règle de gestion ou serveur indisponible. Le message est affichable tel quel. |
| [ExportStatistiques](#exportstatistiques) | Export des statistiques d'activité au format CSV (EX-51), lisible directement par Excel en français : séparateur point-virgule, virgule décimale, dates jj/mm/aaaa. Le fichier est à enregistrer en UTF-8 avec BOM (`Encodage`) pour que les accents s'affichent correctement. |
| [FabriqueServices](#fabriqueservices) | Point d'entrée de la couche Métier pour l'IHM : crée les services branchés sur la base Oracle à partir de la configuration (appsettings.json + appsettings.Local.json). L'IHM n'a ainsi jamais besoin de connaître la couche Données. |
| [GenerateurMotDePasse](#generateurmotdepasse) | Génère des mots de passe provisoires robustes (conformes à `PolitiqueMotDePasse`), sans caractères ambigus à la lecture (O/0, l/1/I) puisqu'ils sont communiqués au collaborateur. |
| [HacheurMotDePasse](#hacheurmotdepasse) | Hachage et vérification des mots de passe (PBKDF2-SHA256). Format stocké : `PBKDF2-SHA256$<iterations>$<sel base64>$<cle base64>` (identique à celui du jeu d'essai, voir docs/modele-donnees.md). |
| [LibellesProfils](#libellesprofils) | Libellés des profils, communs à l'affichage et aux exports. |
| [ModuleApplication](#moduleapplication) | Fonctionnalités de l'application, affichées dans le menu principal selon le profil. |
| [Perimetres](#perimetres) | Périmètre d'équipe d'un utilisateur selon son profil et son affectation. |
| [Periodicite](#periodicite) | Règle de périodicité des visites : chaque praticien doit être revu tous les 6 à 8 mois (motif principal des visites selon les visiteurs, EX-24). |
| [PolitiqueMotDePasse](#politiquemotdepasse) | Règles de robustesse d'un nouveau mot de passe (EX-07). |
| [ResultatConnexion](#resultatconnexion) | Résultat d'une tentative de connexion : statut, message à afficher et utilisateur si accepté. |
| [ResultatEnregistrement](#resultatenregistrement) | Résultat de l'enregistrement d'un compte-rendu : numéro attribué ou liste des erreurs. |
| [ResultatOperation](#resultatoperation) | Résultat d'une opération d'administration : erreurs éventuelles et message pour l'utilisateur. |
| [ServiceActivite](#serviceactivite) | Module « Mon activité » : synthèse de l'activité du collaborateur sur une période (EX-23) et praticiens de son portefeuille à revoir (EX-24). |
| [ServiceAdministration](#serviceadministration) | Module Administration (EX-70 à EX-74), réservé au profil administrateur : comptes, affectations, portefeuilles, référentiels et journal des connexions. |
| [ServiceAuthentification](#serviceauthentification) | Authentification des collaborateurs (EX-01, EX-05 à EX-09). |
| [ServiceConsultation](#serviceconsultation) | Consultation des fiches praticiens (EX-21) et médicaments (EX-22), ouverte à tous les profils. Les erreurs de base de données sont transformées en `ErreurMetierException`. |
| [ServiceEchantillons](#serviceechantillons) | Échantillons de l'équipe : dotations mensuelles saisies par le délégué (EX-33) et contrôle de stock attribué / distribué (EX-34), consultable aussi par le responsable. |
| [ServiceEquipe](#serviceequipe) | Suivi d'une équipe : région pour le délégué (EX-31, EX-32, EX-35), secteur pour le responsable (EX-40, EX-41). Seuls les comptes-rendus validés des membres actuels de l'équipe sont visibles, en lecture seule. |
| [ServiceMessagerie](#servicemessagerie) | Messagerie interne (EX-50) : envoi à une personne ou à un groupe (région, secteur), boîte de réception avec état lu / non lu, messages envoyés avec suivi de lecture. |
| [ServiceRapports](#servicerapports) | Module « Mes comptes-rendus » : consultation, saisie, modification et suppression des CR du collaborateur connecté (EX-10 à EX-29). Les erreurs de base de données sont transformées en `ErreurMetierException`. |
| [StatutConnexion](#statutconnexion) | Issue d'une tentative de connexion. |
| [ValidateurRapport](#validateurrapport) | Règles de saisie d'un compte-rendu (EX-11 à EX-19, EX-27). Un brouillon n'exige que le praticien et la date ; un CR validé doit être complet. |

### Autorisations

Droits d'accès aux modules selon le profil (EX-02). Le délégué est aussi un visiteur : il a les modules du visiteur en plus des siens.

| Membre | Description |
|---|---|
| `ModulesAccessibles(Profil)` | Modules accessibles pour un profil, dans l'ordre d'affichage du menu. |
| `PeutAcceder(Profil, ModuleApplication)` | Indique si un profil a accès à un module. |

### ErreurMetierException

Erreur destinée à l'utilisateur : action refusée par une règle de gestion ou serveur indisponible. Le message est affichable tel quel.

### ExportStatistiques

Export des statistiques d'activité au format CSV (EX-51), lisible directement par Excel en français : séparateur point-virgule, virgule décimale, dates jj/mm/aaaa. Le fichier est à enregistrer en UTF-8 avec BOM (`Encodage`) pour que les accents s'affichent correctement.

| Membre | Description |
|---|---|
| `Encodage` | Encodage du fichier : UTF-8 avec BOM (reconnu par Excel). |
| `Generer(String, SyntheseActivite, IEnumerable, DateTime)` | Contenu CSV d'une synthèse : en-tête (périmètre, période, date d'export), indicateurs, visites par mois, motifs, produits présentés, échantillons et, pour une équipe, l'activité de chaque membre. |
| `Champ(Object)` | Valeur d'une cellule : entre guillemets si elle contient un séparateur, un guillemet ou un retour à la ligne ; précédée d'une apostrophe si elle commence par = + - @ (évite qu'Excel l'interprète comme une formule). |
| `NomDeFichier(String, DateTime, DateTime)` | Nom de fichier proposé : « GSB-activite-Mon-activite-2026-07-01-au-2026-09-28.csv ». |

### FabriqueServices

Point d'entrée de la couche Métier pour l'IHM : crée les services branchés sur la base Oracle à partir de la configuration (appsettings.json + appsettings.Local.json). L'IHM n'a ainsi jamais besoin de connaître la couche Données.

| Membre | Description |
|---|---|
| `DepuisConfiguration()` | Crée la fabrique à partir des fichiers de configuration du dossier de l'application. |

### GenerateurMotDePasse

Génère des mots de passe provisoires robustes (conformes à `PolitiqueMotDePasse`), sans caractères ambigus à la lecture (O/0, l/1/I) puisqu'ils sont communiqués au collaborateur.

### HacheurMotDePasse

Hachage et vérification des mots de passe (PBKDF2-SHA256). Format stocké : `PBKDF2-SHA256$<iterations>$<sel base64>$<cle base64>` (identique à celui du jeu d'essai, voir docs/modele-donnees.md).

| Membre | Description |
|---|---|
| `Hacher(String)` | Hache un mot de passe avec un sel aléatoire. |
| `Verifier(String, String)` | Vérifie un mot de passe contre sa valeur hachée. Renvoie Faux si le format est invalide. La comparaison se fait en temps constant. |

### LibellesProfils

Libellés des profils, communs à l'affichage et aux exports.

### ModuleApplication

Fonctionnalités de l'application, affichées dans le menu principal selon le profil.

| Membre | Description |
|---|---|
| `MesComptesRendus` | Saisie et consultation de ses comptes-rendus (EX-10 à EX-20). |
| `Praticiens` | Fiches praticiens (EX-21). |
| `Medicaments` | Fiches médicaments (EX-22). |
| `MonActivite` | Synthèse de sa propre activité (EX-23). |
| `ActiviteRegion` | Activité et CR des visiteurs de la région (EX-31, EX-32). |
| `Echantillons` | Dotations d'échantillons et contrôle de stock (EX-33, EX-34). |
| `ActiviteSecteur` | Activité et CR des visiteurs du secteur (EX-40, EX-41). |
| `Messagerie` | Messagerie interne (EX-50). |
| `Administration` | Gestion des comptes, affectations, portefeuilles et référentiels (EX-70 à EX-74). |

### Perimetres

Périmètre d'équipe d'un utilisateur selon son profil et son affectation.

| Membre | Description |
|---|---|
| `DeLEquipe(UtilisateurConnecte)` | Délégué : sa région. Responsable : son secteur. |
| `ControlerPeriode(DateTime, DateTime, DateTime)` | Contrôle une période d'analyse : début avant fin, pas plus de 3 ans en arrière (EX-20). Renvoie la période avec une fin future ramenée à aujourd'hui. |

### Periodicite

Règle de périodicité des visites : chaque praticien doit être revu tous les 6 à 8 mois (motif principal des visites selon les visiteurs, EX-24).

| Membre | Description |
|---|---|
| `MoisMinimum` | En deçà, le praticien est à jour. |
| `MoisMaximum` | Au-delà, le praticien est à revoir en priorité. |
| `Evaluer(Nullable, DateTime)` | Situation d'un praticien selon la date de sa dernière visite validée. |
| `ProchaineVisiteConseillee(Nullable)` | Date à partir de laquelle la prochaine visite est attendue (dernière visite + 6 mois). |

### PolitiqueMotDePasse

Règles de robustesse d'un nouveau mot de passe (EX-07).

| Membre | Description |
|---|---|
| `LongueurMinimale` | Longueur minimale d'un mot de passe. |
| `Description` | Texte des règles, à afficher à l'utilisateur. |
| `Verifier(String)` | Renvoie la liste des règles non respectées (vide si le mot de passe est valide). |

### ResultatConnexion

Résultat d'une tentative de connexion : statut, message à afficher et utilisateur si accepté.

| Membre | Description |
|---|---|
| `Message` | Message destiné à l'utilisateur (vide si la connexion est réussie). |
| `Utilisateur` | Utilisateur connecté (Nothing si la connexion est refusée). |
| `EstAcceptee` | Vrai si l'utilisateur peut entrer (éventuellement après changement de mot de passe). |

### ResultatEnregistrement

Résultat de l'enregistrement d'un compte-rendu : numéro attribué ou liste des erreurs.

| Membre | Description |
|---|---|
| `Numero` | Numéro du rapport enregistré (Nothing en cas d'échec). |
| `Erreurs` | Erreurs à corriger (vide si l'enregistrement a réussi). |

### ResultatOperation

Résultat d'une opération d'administration : erreurs éventuelles et message pour l'utilisateur.

| Membre | Description |
|---|---|
| `Message` | Message de confirmation (vide en cas d'échec). |
| `MotDePasseProvisoire` | Mot de passe provisoire à communiquer (création ou réinitialisation), sinon Nothing. |

### ServiceActivite

Module « Mon activité » : synthèse de l'activité du collaborateur sur une période (EX-23) et praticiens de son portefeuille à revoir (EX-24).

| Membre | Description |
|---|---|
| `DebutConsultable` | Date la plus ancienne consultable (3 ans, EX-20). |
| `PeriodesPredefinies()` | Périodes proposées à l'utilisateur, la première étant la période par défaut. |
| `MaSynthese(UtilisateurConnecte, DateTime, DateTime)` | Synthèse de l'activité de l'utilisateur entre deux dates (incluses). Une fin dans le futur est ramenée à aujourd'hui. |
| `CompleterMois(IEnumerable, DateTime, DateTime)` | Renvoie un élément par mois de la période, dans l'ordre, avec zéro pour les mois sans visite (un graphique ne doit pas « sauter » les mois creux). |
| `PraticiensARevoir(UtilisateurConnecte)` | Praticiens du portefeuille à planifier, du plus urgent au moins urgent (EX-24) : 1. plus de 8 mois sans visite ; 2. jamais visités ; 3. prochaine visite prévue dépassée ; 4. entre 6 et 8 mois. Les praticiens à jour sans visite prévue dépassée sont exclus. |
| `Prioriser(IEnumerable, DateTime)` | Calcul de la liste « à revoir » (séparé pour être testé sans base). |

### ServiceAdministration

Module Administration (EX-70 à EX-74), réservé au profil administrateur : comptes, affectations, portefeuilles, référentiels et journal des connexions.

| Membre | Description |
|---|---|
| `CreerCollaborateur(UtilisateurConnecte, Collaborateur, Affectation)` | Crée un compte avec un mot de passe provisoire (à changer à la première connexion). |
| `ChangerAffectation(UtilisateurConnecte, String, Affectation, DateTime)` | Nouvelle affectation à partir de ; l'historique est conservé (EX-71). |
| `EnregistrerDepart(UtilisateurConnecte, String, DateTime)` | Départ de l'entreprise : le compte ne permet plus de se connecter, ses CR sont conservés et ses praticiens deviennent « sans visiteur » (à réattribuer). |
| `ReinitialiserMotDePasse(UtilisateurConnecte, String)` | Nouveau mot de passe provisoire ; le compte est déverrouillé. |
| `AttribuerPraticiens(UtilisateurConnecte, IEnumerable, String)` | Confie des praticiens à un visiteur ou un délégué en poste, à partir d'aujourd'hui. |
| `EnregistrerPraticien(UtilisateurConnecte, Praticien)` | Crée (numéro vide) ou modifie un praticien. |
| `FicheMedicament(UtilisateurConnecte, String)` | Fiche complète d'un médicament (composition, interactions, posologies avec leurs codes). |
| `CreerComposant(UtilisateurConnecte, String, String)` | Nouveau composant : code de 2 à 4 lettres ou chiffres (mis en majuscules), libellé obligatoire. |
| `CodeDosage(Decimal, String)` | Code d'un dosage calculé à partir de sa valeur, comme dans le jeu d'essai : « 500MG », « 1G » ; la virgule devient V et le signe % devient PC (« 0V5PC » pour 0,5 %). |
| `AjouterInteraction(UtilisateurConnecte, String, String, Boolean, String)` | Interaction entre le médicament et un autre ; indique le sens (vrai : le médicament perturbe l'effet de l'autre ; faux : il est perturbé par l'autre). |
| `ControlerAffectation(Affectation)` | Visiteur / délégué : une région ; responsable : un secteur ; administrateur : rien. |
| `VerifierVisiteurEnPoste(String)` | Renvoie la fiche si le collaborateur est un visiteur ou un délégué en poste, sinon Nothing. |
| `Executer(Action, String, String, String)` | Exécute une écriture et traduit les erreurs de base en messages compréhensibles. |

### ServiceAuthentification

Authentification des collaborateurs (EX-01, EX-05 à EX-09).

| Membre | Description |
|---|---|
| `MaxEchecs` | Nombre d'échecs consécutifs entraînant le verrouillage du compte (EX-06). |
| `New(ICollaborateurDao, TimeProvider)` |  |
| `Connecter(String, String)` | Tente de connecter un collaborateur. Ne lève pas d'exception : tout est dans le résultat. |
| `ChangerMotDePasse(UtilisateurConnecte, String, String, String)` | Change le mot de passe de l'utilisateur connecté (EX-07). Renvoie la liste des erreurs (vide si le changement a réussi). |

### ServiceConsultation

Consultation des fiches praticiens (EX-21) et médicaments (EX-22), ouverte à tous les profils. Les erreurs de base de données sont transformées en `ErreurMetierException`.

| Membre | Description |
|---|---|
| `MaxResultats` | Nombre maximal de praticiens renvoyés par une recherche. |
| `APortefeuille(Profil)` | Vrai si le profil a un portefeuille de praticiens (visiteur ou délégué). |
| `RechercherPraticiens(UtilisateurConnecte, String, Boolean, Boolean)` | Recherche des praticiens. n'a d'effet que pour un visiteur ou un délégué. |
| `Medicaments(UtilisateurConnecte)` | Tous les médicaments (commercialisés ou retirés), triés par nom commercial. |
| `Periodicite(Nullable)` | Situation de périodicité d'un praticien à la date du jour. |

### ServiceEchantillons

Échantillons de l'équipe : dotations mensuelles saisies par le délégué (EX-33) et contrôle de stock attribué / distribué (EX-34), consultable aussi par le responsable.

| Membre | Description |
|---|---|
| `MaxQuantite` | Quantité maximale d'une dotation (colonne NUMBER(6)). |
| `MoisPassesModifiables` | Nombre de mois passés sur lesquels une dotation peut encore être saisie ou corrigée. |
| `PeutSaisir(UtilisateurConnecte)` | Seul le délégué régional enregistre les attributions (CDC) ; le responsable consulte. |
| `MoisProposes()` | Mois proposés, du plus récent (mois prochain) au plus ancien (12 mois en arrière). |
| `Medicaments()` | Médicaments commercialisés, pour la saisie des dotations. |
| `Stock(UtilisateurConnecte, DateTime, Boolean)` | Contrôle de stock du mois pour l'équipe ; les dépassements en premier si demandé. |
| `EnregistrerDotation(UtilisateurConnecte, String, String, DateTime, Int32)` | Crée ou remplace une dotation. Renvoie les erreurs (vide si enregistrée). |
| `SupprimerDotation(UtilisateurConnecte, String, String, DateTime)` | Supprime une dotation. Renvoie les erreurs (vide si supprimée). |
| `ControlerSaisie(UtilisateurConnecte, String, DateTime)` | Règles communes : délégué, visiteur de sa région, mois modifiable. |

### ServiceEquipe

Suivi d'une équipe : région pour le délégué (EX-31, EX-32, EX-35), secteur pour le responsable (EX-40, EX-41). Seuls les comptes-rendus validés des membres actuels de l'équipe sont visibles, en lecture seule.

| Membre | Description |
|---|---|
| `PerimetreDe(UtilisateurConnecte)` | Périmètre suivi par l'utilisateur (vérifie aussi ses droits). |
| `Synthese(UtilisateurConnecte, DateTime, DateTime)` | Synthèse de toute l'équipe sur la période. |
| `ActiviteParMembre(UtilisateurConnecte, DateTime, DateTime)` | Activité de chaque membre sur la période. |
| `SyntheseMembre(UtilisateurConnecte, String, DateTime, DateTime)` | Synthèse d'un membre de l'équipe (refusée pour un collaborateur hors équipe). |
| `RapportsEquipe(UtilisateurConnecte)` | CR validés de l'équipe sur les 3 dernières années (les brouillons restent privés). |
| `ChargerRapport(UtilisateurConnecte, Int32)` | Lecture d'un CR validé d'un membre de l'équipe. |
| `PraticiensARevoir(UtilisateurConnecte)` | Praticiens des portefeuilles de l'équipe à revoir, du plus urgent au moins urgent (EX-35). |

### ServiceMessagerie

Messagerie interne (EX-50) : envoi à une personne ou à un groupe (région, secteur), boîte de réception avec état lu / non lu, messages envoyés avec suivi de lecture.

| Membre | Description |
|---|---|
| `Lire(UtilisateurConnecte, Int32)` | Ouvre un message : réservé à l'expéditeur et aux destinataires. Pour un destinataire, la lecture est enregistrée. |
| `Annuaire(UtilisateurConnecte)` | Collaborateurs à qui écrire (l'utilisateur lui-même exclu). |
| `Groupes(IEnumerable, UtilisateurConnecte)` | Groupes de destinataires : chaque région (visiteurs et délégué) et chaque secteur (tous ses membres, responsable compris). La région et le secteur de l'utilisateur sont proposés en premier. |
| `Envoyer(UtilisateurConnecte, String, String, IEnumerable)` | Envoie un message. Renvoie les erreurs (vide si envoyé). |
| `ObjetReponse(String)` | Objet d'une réponse : « RE : » ajouté une seule fois. |

### ServiceRapports

Module « Mes comptes-rendus » : consultation, saisie, modification et suppression des CR du collaborateur connecté (EX-10 à EX-29). Les erreurs de base de données sont transformées en `ErreurMetierException`.

| Membre | Description |
|---|---|
| `AnneesConsultation` | Profondeur de consultation de ses propres CR (EX-20). |
| `MaxResultatsRecherche` | Nombre maximal de praticiens renvoyés par une recherche. |
| `Maintenant` | Date et heure actuelles (utilisées pour la fin des sessions de saisie). |
| `MesRapports(UtilisateurConnecte)` | CR de l'utilisateur sur les 3 dernières années, du plus récent au plus ancien (EX-20). |
| `NouveauRapport(UtilisateurConnecte)` | Nouveau CR vide, daté du jour, au nom de l'utilisateur. |
| `Charger(UtilisateurConnecte, Int32)` | Charge un CR de l'utilisateur. Refuse les CR d'un autre auteur. |
| `Enregistrer(UtilisateurConnecte, RapportVisite, Boolean, DateTime)` | Enregistre le CR en brouillon ou validé. L'auteur est toujours l'utilisateur connecté (EX-10), un CR validé ne peut pas repasser en brouillon (EX-29) et le temps de saisie est tracé (EX-26). |
| `SupprimerBrouillon(UtilisateurConnecte, Int32)` | Supprime un brouillon de l'utilisateur (EX-29). Un CR validé ne peut pas être supprimé. |
| `Portefeuille(UtilisateurConnecte)` | Praticiens du portefeuille de l'utilisateur (EX-28). |
| `Medicaments()` | Tous les médicaments ; l'IHM ne propose que les actifs (plus ceux déjà présents dans le CR). |
| `Motifs()` | Tous les motifs ; l'IHM ne propose que les actifs (plus celui déjà présent dans le CR). |
| `CreerRemplacant(Praticien)` | Crée la fiche d'un remplaçant rencontré lors d'une visite (EX-16, EX-28). Renvoie la liste des erreurs, vide en cas de succès (le numéro est alors renseigné). |
| `Normaliser(RapportVisite)` | Nettoie la saisie : espaces superflus, champs vides, précision réservée au motif « Autre ». |
| `Appeler``1(Func)` | Exécute un appel à la base en traduisant les erreurs techniques. |

### StatutConnexion

Issue d'une tentative de connexion.

| Membre | Description |
|---|---|
| `Reussie` | Connexion acceptée. |
| `ChangementMotDePasseRequis` | Connexion acceptée mais le mot de passe doit être changé avant d'aller plus loin (EX-07). |
| `IdentifiantsInvalides` | Login inconnu ou mot de passe faux (message volontairement identique). |
| `CompteVerrouille` | Compte verrouillé (EX-06). |
| `CompteInactif` | Collaborateur parti de l'entreprise ou sans affectation (EX-09). |
| `ServeurIndisponible` | Base de données injoignable. |

### ValidateurRapport

Règles de saisie d'un compte-rendu (EX-11 à EX-19, EX-27). Un brouillon n'exige que le praticien et la date ; un CR validé doit être complet.

| Membre | Description |
|---|---|
| `Verifier(RapportVisite, DateTime)` | Renvoie la liste des erreurs (vide si le rapport peut être enregistré dans son état). |

## GSB.CR.IHM

Écrans WinForms, composants visuels et point d'entrée. 32 types documentés.

| Type | Rôle |
|---|---|
| [AffichagePeriodicite](#affichageperiodicite) | Libellés et couleurs des états de périodicité des visites. |
| [ChoixProfil](#choixprofil) | Profil proposé dans les listes (libellé lisible). |
| [ChoixVisiteur](#choixvisiteur) | Visiteur ou délégué proposé dans les listes. |
| [ElementAnnuaire](#elementannuaire) | Élément affiché dans la liste des destinataires. |
| [EnregistrementExport](#enregistrementexport) | Enregistrement d'une synthèse d'activité en CSV (EX-51) : choix du fichier, écriture, message de confirmation. Utilisé par « Mon activité », « Ma région » et « Mon secteur ». |
| [FrmAccueil](#frmaccueil) | Menu principal après connexion : affiche les modules accessibles selon le profil (EX-02). |
| [FrmAdministration](#frmadministration) | Module Administration (EX-70 à EX-74), réservé à l'administrateur. Ce fichier contient la partie commune et l'onglet Collaborateurs ; les autres onglets sont dans FrmAdministration.Portefeuilles.vb, FrmAdministration.Referentiels.vb et FrmAdministration.Journal.vb. |
| [FrmChangementMotDePasse](#frmchangementmotdepasse) | Changement du mot de passe (EX-07) : obligatoire à la première connexion, ou à la demande. |
| [FrmCompteRendu](#frmcompterendu) | Saisie ou modification d'un compte-rendu de visite (EX-10 à EX-19, EX-25 à EX-29). Renvoie DialogResult.OK si le compte-rendu a été enregistré. |
| [FrmConnexion](#frmconnexion) | Page d'accueil de l'application : uniquement la zone d'identification (EX-01). Renvoie DialogResult.OK quand l'utilisateur est accepté. |
| [FrmDetailsMedicament](#frmdetailsmedicament) | Composition, interactions et posologie d'un médicament (EX-73), ouvert depuis l'onglet Référentiels de l'administration. Chaque ajout passe par un formulaire contrôlé par `ServiceAdministration`. |
| [FrmEchantillons](#frmechantillons) | Échantillons de l'équipe : contrôle de stock attribué / distribué du mois (EX-34) et, pour le délégué, saisie des dotations mensuelles (EX-33). Le responsable consulte sans pouvoir modifier. |
| [FrmEquipe](#frmequipe) | Suivi d'équipe : « Ma région » pour le délégué (EX-31, EX-32, EX-35), « Mon secteur » pour le responsable (EX-40, EX-41). Synthèse, activité par visiteur, comptes-rendus validés en lecture seule, praticiens à revoir. |
| [FrmFiche](#frmfiche) | Fenêtre de consultation générique : en-tête bleu et fiche remplie en arrière-plan par l'appelant. Sert à lire un compte-rendu d'un membre de l'équipe (EX-32, EX-41) ou la synthèse d'un membre (EX-31). |
| [FrmFormulaire](#frmformulaire) | Formulaire de saisie générique aux couleurs GSB : on déclare des champs (texte, liste, date, nombre, case à cocher) puis une validation qui appelle le service. La fenêtre affiche les erreurs renvoyées et ne se ferme (DialogResult.OK) qu'en cas de succès. Utilisé par les dialogues d'administration. |
| [FrmMedicaments](#frmmedicaments) | Consultation des médicaments (EX-22) : recherche, filtre par famille, fiche avec effets, contre-indications, composition, interactions et posologie. |
| [FrmMesComptesRendus](#frmmescomptesrendus) | Liste des comptes-rendus du collaborateur connecté sur les 3 dernières années (EX-20), avec création, ouverture et suppression des brouillons (EX-29). |
| [FrmMessagerie](#frmmessagerie) | Messagerie interne (EX-50) : boîte de réception (non lus en gras), messages envoyés avec suivi de lecture, lecture, nouveau message et réponse. |
| [FrmMonActivite](#frmmonactivite) | Module « Mon activité » : synthèse de l'activité sur une période (EX-23) et praticiens du portefeuille à revoir (EX-24), avec saisie directe d'un compte-rendu. |
| [FrmNouveauMessage](#frmnouveaumessage) | Rédaction d'un message (EX-50) : destinataires choisis dans l'annuaire (avec recherche) ou par groupe (région, secteur), objet et texte. Renvoie DialogResult.OK une fois le message envoyé. |
| [FrmNouveauRemplacant](#frmnouveauremplacant) | Création de la fiche d'un remplaçant rencontré lors d'une visite (EX-16, EX-28). Renvoie DialogResult.OK et le praticien créé. |
| [FrmPraticiens](#frmpraticiens) | Consultation des praticiens (EX-21) : recherche, fiche détaillée, périodicité des visites et historique (y compris les visites où le praticien était remplaçant). |
| [GraphiqueBarres](#graphiquebarres) | Histogramme d'une seule série (ex. visites par mois), dessiné en GDI+ : barres de 24 px maximum au sommet arrondi et à la base droite, 2 px d'écart minimum entre barres, grille fine et discrète, graduations rondes, étiquette uniquement sur la valeur maximale, valeur exacte au survol de toute la colonne. Les textes n'utilisent jamais la couleur de la série. |
| [LibellesModules](#libellesmodules) | Textes affichés pour chaque module dans le menu principal. |
| [OutilsEcran](#outilsecran) | Construction par code des barres de boutons et des grilles aux couleurs GSB, partagée par les écrans d'administration (FrmAdministration, FrmDetailsMedicament). |
| [PanneauFiche](#panneaufiche) | Panneau de fiche détaillée aux couleurs GSB : empile verticalement titres, badges, sections, textes et tableaux, en ajustant leur largeur à celle du panneau. Défile si le contenu est long. |
| [Program](#program) | Point d'entrée : connexion → (changement de mot de passe) → menu principal, en boucle tant que l'utilisateur se déconnecte au lieu de quitter. |
| [RenduARevoir](#renduarevoir) | Tableau des praticiens à revoir (visiteur ou équipe), avec la situation en couleur. |
| [RenduSynthese](#rendusynthese) | Affichage d'une synthèse d'activité dans un `PanneauFiche` : indicateurs, visites par mois (graphique + tableau), motifs, produits présentés, échantillons. Utilisé pour un visiteur, un membre d'équipe ou une équipe entière. |
| [Theme](#theme) | Charte graphique GSB : bleu et blanc, couleurs du logo du laboratoire (CDC « Ergonomie »). Toutes les fenêtres utilisent ces couleurs et polices pour garder une unité visuelle. |
| [TuileIndicateur](#tuileindicateur) | Tuile d'indicateur : libellé, valeur mise en avant et complément facultatif. Taille automatique (s'adapte à la mise à l'échelle de l'écran et à la longueur des textes). |
| [TuileModule](#tuilemodule) | Tuile cliquable du menu principal : titre du module en gras et description en dessous. |

### AffichagePeriodicite

Libellés et couleurs des états de périodicité des visites.

| Membre | Description |
|---|---|
| `Couleurs(EtatPeriodicite)` | Couleurs (fond, texte) du badge d'un état. |

### ChoixProfil

Profil proposé dans les listes (libellé lisible).

### ChoixVisiteur

Visiteur ou délégué proposé dans les listes.

### ElementAnnuaire

Élément affiché dans la liste des destinataires.

### EnregistrementExport

Enregistrement d'une synthèse d'activité en CSV (EX-51) : choix du fichier, écriture, message de confirmation. Utilisé par « Mon activité », « Ma région » et « Mon secteur ».

### FrmAccueil

Menu principal après connexion : affiche les modules accessibles selon le profil (EX-02).

| Membre | Description |
|---|---|
| `Deconnexion` | Vrai si l'utilisateur a demandé à se déconnecter (retour à l'écran de connexion). |
| `OuvrirModule(ModuleApplication)` | Ouvre la fenêtre du module ; renvoie Faux si le module n'est pas encore disponible. |
| `ActualiserMessagesNonLus()` | Affiche le nombre de messages non lus sur la tuile Messagerie. |

### FrmAdministration

Module Administration (EX-70 à EX-74), réservé à l'administrateur. Ce fichier contient la partie commune et l'onglet Collaborateurs ; les autres onglets sont dans FrmAdministration.Portefeuilles.vb, FrmAdministration.Referentiels.vb et FrmAdministration.Journal.vb.

| Membre | Description |
|---|---|
| `VisiteursEnPoste()` | Visiteurs et délégués en poste (les seuls à pouvoir avoir un portefeuille). |
| `ChoisirDestinataire(String, String, FicheCollaborateur, ResultatOperation})` | Demande le visiteur destinataire (autre que le visiteur affiché). |
| `FormulairePraticien(Praticien)` | Formulaire praticien (création si n'a pas de numéro). |
| `DetailsMedicament(Object, EventArgs)` | Composition, interactions et posologie du médicament sélectionné (EX-73). |
| `Echelle(Int32)` | Mise à l'échelle de l'écran des dimensions des contrôles créés par code. |
| `Operer(Func, Func)` | Exécute une opération du service en arrière-plan, affiche le résultat (message ou erreurs), communique le mot de passe provisoire éventuel, puis rafraîchit l'affichage. |
| `Afficher(ResultatOperation)` | Affiche le résultat d'une opération (formulaire ou action directe). |
| `CommuniquerMotDePasse(String)` | Le mot de passe provisoire n'est montré qu'une fois (il n'est stocké que haché). |
| `OuvrirFormulaire(FrmFormulaire, Func)` | Ouvre un formulaire ; après validation réussie, affiche le résultat et rafraîchit. |
| `AjouterChampsAffectation(FrmFormulaire, Affectation)` | Champs profil / région / secteur, la région ou le secteur n'apparaissant que si nécessaire. |

### FrmChangementMotDePasse

Changement du mot de passe (EX-07) : obligatoire à la première connexion, ou à la demande.

| Membre | Description |
|---|---|
| `New(ServiceAuthentification, UtilisateurConnecte, Boolean)` |  |

### FrmCompteRendu

Saisie ou modification d'un compte-rendu de visite (EX-10 à EX-19, EX-25 à EX-29). Renvoie DialogResult.OK si le compte-rendu a été enregistré.

| Membre | Description |
|---|---|
| `AucunProduit` | Élément « aucun produit » des listes de produits présentés. |
| `NumeroEnregistre` | Numéro du compte-rendu enregistré (renseigné quand la fenêtre renvoie OK). |
| `EnregistreValide` | Vrai si le compte-rendu enregistré est validé (faux : brouillon). |
| `New(ServiceRapports, UtilisateurConnecte, Nullable, Nullable)` |  |
| `ChargerDonnees()` | Lit en arrière-plan le rapport et toutes les listes nécessaires au formulaire. |
| `LireFormulaire()` | Recopie la saisie du formulaire dans le rapport. |

### FrmConnexion

Page d'accueil de l'application : uniquement la zone d'identification (EX-01). Renvoie DialogResult.OK quand l'utilisateur est accepté.

| Membre | Description |
|---|---|
| `Utilisateur` | Utilisateur connecté (renseigné quand la fenêtre renvoie OK). |
| `ChangementMotDePasseRequis` | Vrai si l'utilisateur doit changer son mot de passe avant d'entrer (EX-07). |

### FrmDetailsMedicament

Composition, interactions et posologie d'un médicament (EX-73), ouvert depuis l'onglet Référentiels de l'administration. Chaque ajout passe par un formulaire contrôlé par `ServiceAdministration`.

| Membre | Description |
|---|---|
| `Charger()` | Recharge la fiche et remplit les trois grilles. |
| `OuvrirFormulaire(FrmFormulaire)` | Ouvre un formulaire ; après validation réussie, affiche le message et recharge la fiche. |
| `Lire``1(Func)` | Charge une liste de référence ; affiche l'erreur et renvoie Nothing en cas de problème. |

### FrmEchantillons

Échantillons de l'équipe : contrôle de stock attribué / distribué du mois (EX-34) et, pour le délégué, saisie des dotations mensuelles (EX-33). Le responsable consulte sans pouvoir modifier.

| Membre | Description |
|---|---|
| `dgvStock_SelectionChanged(Object, EventArgs)` | Une ligne sélectionnée pré-remplit la saisie (correction d'une dotation existante). |

### FrmEquipe

Suivi d'équipe : « Ma région » pour le délégué (EX-31, EX-32, EX-35), « Mon secteur » pour le responsable (EX-40, EX-41). Synthèse, activité par visiteur, comptes-rendus validés en lecture seule, praticiens à revoir.

| Membre | Description |
|---|---|
| `New(ServiceEquipe, ServiceActivite, UtilisateurConnecte)` |  |
| `btnExporter_Click(Object, EventArgs)` | Export CSV de la synthèse et de l'activité par visiteur affichées (EX-51). |

### FrmFiche

Fenêtre de consultation générique : en-tête bleu et fiche remplie en arrière-plan par l'appelant. Sert à lire un compte-rendu d'un membre de l'équipe (EX-32, EX-41) ou la synthèse d'un membre (EX-31).

| Membre | Description |
|---|---|
| `New(String, String, PanneauFiche, Task})` |  |
| `CompteRendu(ServiceEquipe, UtilisateurConnecte, Int32, String)` | Lecture seule d'un compte-rendu validé d'un membre de l'équipe. |
| `SyntheseMembre(ServiceEquipe, UtilisateurConnecte, ActiviteMembre, DateTime, DateTime)` | Synthèse de l'activité d'un membre de l'équipe sur une période. |

### FrmFormulaire

Formulaire de saisie générique aux couleurs GSB : on déclare des champs (texte, liste, date, nombre, case à cocher) puis une validation qui appelle le service. La fenêtre affiche les erreurs renvoyées et ne se ferme (DialogResult.OK) qu'en cas de succès. Utilisé par les dialogues d'administration.

| Membre | Description |
|---|---|
| `LargeurChamp` | Largeur des champs, mise à l'échelle de l'écran (les contrôles créés par code ne le sont pas automatiquement). |
| `Validation` | Appelée au clic sur le bouton de validation ; renvoie le résultat du service. |
| `Resultat` | Résultat de la validation réussie (message, mot de passe provisoire…). |
| `AjouterNote(String)` | Paragraphe d'information (grisé) au milieu des champs. |
| `Afficher(String, Boolean)` | Affiche ou masque un champ et son libellé. |

### FrmMedicaments

Consultation des médicaments (EX-22) : recherche, filtre par famille, fiche avec effets, contre-indications, composition, interactions et posologie.

### FrmMesComptesRendus

Liste des comptes-rendus du collaborateur connecté sur les 3 dernières années (EX-20), avec création, ouverture et suppression des brouillons (EX-29).

| Membre | Description |
|---|---|
| `Recharger(Nullable)` | Relit la liste depuis la base et resélectionne éventuellement un rapport. |

### FrmMessagerie

Messagerie interne (EX-50) : boîte de réception (non lus en gras), messages envoyés avec suivi de lecture, lecture, nouveau message et réponse.

### FrmMonActivite

Module « Mon activité » : synthèse de l'activité sur une période (EX-23) et praticiens du portefeuille à revoir (EX-24), avec saisie directe d'un compte-rendu.

| Membre | Description |
|---|---|
| `btnExporter_Click(Object, EventArgs)` | Export CSV de la synthèse affichée (EX-51). |

### FrmNouveauMessage

Rédaction d'un message (EX-50) : destinataires choisis dans l'annuaire (avec recherche) ou par groupe (région, secteur), objet et texte. Renvoie DialogResult.OK une fois le message envoyé.

| Membre | Description |
|---|---|
| `AfficherAnnuaire()` | Affiche l'annuaire filtré en conservant les cases cochées (même masquées par le filtre). |

### FrmNouveauRemplacant

Création de la fiche d'un remplaçant rencontré lors d'une visite (EX-16, EX-28). Renvoie DialogResult.OK et le praticien créé.

| Membre | Description |
|---|---|
| `Praticien` | Praticien créé (renseigné quand la fenêtre renvoie OK). |

### FrmPraticiens

Consultation des praticiens (EX-21) : recherche, fiche détaillée, périodicité des visites et historique (y compris les visites où le praticien était remplaçant).

### GraphiqueBarres

Histogramme d'une seule série (ex. visites par mois), dessiné en GDI+ : barres de 24 px maximum au sommet arrondi et à la base droite, 2 px d'écart minimum entre barres, grille fine et discrète, graduations rondes, étiquette uniquement sur la valeur maximale, valeur exacte au survol de toute la colonne. Les textes n'utilisent jamais la couleur de la série.

| Membre | Description |
|---|---|
| `MessageVide` | Message affiché quand toutes les valeurs sont nulles. |
| `DefinirDonnees(String, Int32, String}})` | Données à afficher : libellé d'axe, valeur, et texte de l'infobulle (ex. « septembre 2026 : 2 visites »). |
| `Graduations(Int32)` | Maximum de l'axe arrondi à une valeur « ronde » et pas des graduations. |
| `SommetArrondi(RectangleF, Single)` | Rectangle au sommet arrondi et à la base droite (la barre part de la ligne de base). |

### LibellesModules

Textes affichés pour chaque module dans le menu principal.

### OutilsEcran

Construction par code des barres de boutons et des grilles aux couleurs GSB, partagée par les écrans d'administration (FrmAdministration, FrmDetailsMedicament).

| Membre | Description |
|---|---|
| `PreparerGrille(DataGridView, String, Int32}}, Boolean)` | Grille en lecture seule, sélection par ligne, colonnes proportionnelles (poids). |
| `Selection``1(DataGridView)` | Objet (Tag) de la ligne sélectionnée, ou Nothing. |

### PanneauFiche

Panneau de fiche détaillée aux couleurs GSB : empile verticalement titres, badges, sections, textes et tableaux, en ajustant leur largeur à celle du panneau. Défile si le contenu est long.

| Membre | Description |
|---|---|
| `LargeurUtile` | Largeur utile d'un élément (panneau moins marges et barre de défilement). |
| `Vider()` | Efface la fiche affichée. |
| `AfficherMessage(String)` | Affiche un message centré quand aucune fiche n'est sélectionnée. |
| `AjouterBadge(String, Color, Color)` | Étiquette colorée (état, périodicité…). |
| `AjouterTexte(String, Color, Boolean)` | Paragraphe ; les lignes vides ou Nothing sont ignorées. |
| `AjouterInformation(String, String)` | Couple « libellé : valeur » ; ignoré si la valeur est vide. |
| `AjouterTableau(String, Single}[], IEnumerable, Int32)` | Tableau en lecture seule. : (titre, poids relatif). |
| `AjouterControle(Control)` | Ajoute un contrôle (graphique…) qui occupe toute la largeur du panneau. |
| `AjouterIndicateurs(IEnumerable)` | Ajoute une rangée de tuiles d'indicateurs, passant à la ligne si besoin. |
| `OnResize(EventArgs)` | Réajuste la largeur des éléments quand le panneau change de taille. |

### Program

Point d'entrée : connexion → (changement de mot de passe) → menu principal, en boucle tant que l'utilisateur se déconnecte au lieu de quitter.

### RenduARevoir

Tableau des praticiens à revoir (visiteur ou équipe), avec la situation en couleur.

| Membre | Description |
|---|---|
| `CreerColonnes(DataGridView, Boolean)` |  |

### RenduSynthese

Affichage d'une synthèse d'activité dans un `PanneauFiche` : indicateurs, visites par mois (graphique + tableau), motifs, produits présentés, échantillons. Utilisé pour un visiteur, un membre d'équipe ou une équipe entière.

| Membre | Description |
|---|---|
| `Duree(Nullable)` | Durée lisible : « 6 min 40 s ». |

### Theme

Charte graphique GSB : bleu et blanc, couleurs du logo du laboratoire (CDC « Ergonomie »). Toutes les fenêtres utilisent ces couleurs et polices pour garder une unité visuelle.

| Membre | Description |
|---|---|
| `StyliserBoutonPrincipal(Button)` | Bouton d'action principale : fond bleu, texte blanc. |
| `StyliserBoutonSecondaire(Button)` | Bouton secondaire : fond blanc, bordure et texte bleus. |
| `StyliserSection(Label)` | Titre de section dans un formulaire : bleu, semi-gras. |
| `StyliserEntete(Panel, Label[])` | Bandeau d'en-tête bleu avec textes blancs. |
| `StyliserGrille(DataGridView)` | Grille de données aux couleurs GSB (en-têtes bleus, lignes alternées). |
| `EnPuces(IEnumerable)` | Affiche une liste d'erreurs sous forme de puces. |

### TuileIndicateur

Tuile d'indicateur : libellé, valeur mise en avant et complément facultatif. Taille automatique (s'adapte à la mise à l'échelle de l'écran et à la longueur des textes).

| Membre | Description |
|---|---|
| `OnPaint(PaintEventArgs)` | Filet bleu à gauche, aux couleurs GSB. |

### TuileModule

Tuile cliquable du menu principal : titre du module en gras et description en dessous.

| Membre | Description |
|---|---|
| `ModuleAssocie` | Module ouvert par la tuile. |
| `DefinirDescription(String, Boolean)` | Remplace la description ; la met en évidence (ex. messages non lus). |
| `OnPaint(PaintEventArgs)` | Bordure bleue et bandeau gauche aux couleurs GSB. |

