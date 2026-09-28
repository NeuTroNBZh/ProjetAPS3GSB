# Questions fréquentes et messages d'erreur

[← Sommaire](README.md)

## Connexion

| Message ou situation | Cause | Que faire |
|---|---|---|
| « Identifiant ou mot de passe incorrect. » | Login inconnu ou mot de passe faux (le message est volontairement le même dans les deux cas). | Vérifier la saisie (majuscules, clavier). Après 5 erreurs, le compte est verrouillé. |
| « Votre compte est verrouillé. Contactez l'administrateur. » | 5 erreurs consécutives, ou verrouillage par l'administrateur. | Demander le déverrouillage ou une réinitialisation du mot de passe à l'administrateur. |
| « Ce compte n'est plus actif. Contactez l'administrateur. » | Départ enregistré, ou aucune affectation en cours. | Contacter l'administrateur. |
| « Le serveur est indisponible. Réessayez dans quelques instants. » | Le poste n'atteint pas le serveur de base de données (réseau, VPN, serveur arrêté). | Vérifier la connexion réseau ou le VPN, puis réessayer ; sinon prévenir le service informatique. |
| Mot de passe oublié | — | L'administrateur génère un mot de passe provisoire, à changer à la connexion suivante. |
| Le nouveau mot de passe est refusé | Il ne respecte pas la règle. | Au moins 8 caractères, dont une majuscule, une minuscule, un chiffre et un caractère spécial. |

## Comptes-rendus

| Question | Réponse |
|---|---|
| Le praticien que j'ai vu n'est pas dans la liste. | La liste ne propose que **votre portefeuille**. S'il s'agit d'un remplaçant, choisissez le titulaire du cabinet puis cochez « La personne rencontrée est un remplaçant ». Sinon, demandez à l'administrateur de vous confier le praticien. |
| Je ne peux pas choisir une date de visite future. | C'est normal : on ne saisit que des visites déjà faites. Pour planifier, utilisez « Prochaine visite prévue le ». |
| « Valider » refuse mon compte-rendu. | Les erreurs sont listées en rouge : motif, bilan et confiance sont obligatoires pour valider ; la précision est obligatoire pour le motif « Autre » ; deux produits présentés au maximum ; quantités d'échantillons positives. Vous pouvez aussi « Enregistrer le brouillon » et terminer plus tard. |
| Mon compte-rendu n'apparaît pas dans « Mon activité ». | Seuls les comptes-rendus **validés** sont comptés. Vérifiez aussi la période choisie. |
| Je veux supprimer un compte-rendu validé. | Impossible : seul un brouillon se supprime. Un compte-rendu validé reste modifiable. |
| Je ne vois pas mes comptes-rendus anciens. | La consultation couvre les **trois dernières années**. |
| Je ne peux pas modifier le compte-rendu d'un collègue. | Chacun ne modifie que ses propres comptes-rendus ; délégués et responsables les consultent en lecture seule. |

## Export et référentiels

| Question | Réponse |
|---|---|
| « Exporter en CSV » est grisé. | Affichez d'abord une synthèse (choisissez la période puis « Afficher »). |
| « Impossible d'écrire le fichier ». | Le fichier est ouvert dans Excel ou le dossier est protégé : fermez-le, ou enregistrez ailleurs. |
| Les accents s'affichent mal dans le fichier. | Ouvrez-le avec Excel (double-clic) ; un autre tableur doit l'importer en UTF-8 avec le point-virgule comme séparateur. |
| « Ce composant figure déjà dans la composition ». | Retirez la ligne existante puis ajoutez-la avec la nouvelle quantité. |
| « Ce dosage existe déjà ». | Il est déjà dans la liste « Dosage » de la posologie : choisissez-le. |

## Échantillons

| Question | Réponse |
|---|---|
| Un « Dépassement » apparaît en rouge. | Le visiteur a déclaré plus d'échantillons distribués que ce qui lui a été attribué ce mois-là. Vérifier ses comptes-rendus ou corriger la dotation. |
| Je ne peux pas attribuer d'échantillons à un visiteur. | Seul le délégué régional attribue, et uniquement aux visiteurs de sa région. |
