-- =====================================================================
-- GSB-CR : référentiels (données de base nécessaires en production)
-- Secteurs, régions, profils, types de praticiens, spécialités,
-- médicaments (familles, composition, interactions, posologies) et motifs.
-- Chargé par installer.sql (développement) et installer_production.sql.
-- =====================================================================
whenever sqlerror exit failure
set define off

-- ---------------------------------------------------------------------
-- Organisation
-- ---------------------------------------------------------------------
insert into SECTEUR values ('E', 'Est');
insert into SECTEUR values ('N', 'Nord');
insert into SECTEUR values ('O', 'Ouest');
insert into SECTEUR values ('P', 'Paris-Centre');
insert into SECTEUR values ('S', 'Sud');

insert into REGION values ('ALS', 'Alsace-Lorraine', 'E');
insert into REGION values ('BG',  'Bourgogne', 'E');
insert into REGION values ('NPC', 'Nord-Pas-de-Calais', 'N');
insert into REGION values ('NOR', 'Normandie', 'N');
insert into REGION values ('AQU', 'Aquitaine', 'O');
insert into REGION values ('BRE', 'Bretagne', 'O');
insert into REGION values ('PDL', 'Pays de la Loire', 'O');
insert into REGION values ('IDF', 'Île-de-France', 'P');
insert into REGION values ('CEN', 'Centre', 'P');
insert into REGION values ('PAC', 'Provence-Alpes-Côte d''Azur', 'S');
insert into REGION values ('RA',  'Rhône-Alpes', 'S');

insert into PROFIL values ('VIS', 'Visiteur médical');
insert into PROFIL values ('DEL', 'Délégué régional');
insert into PROFIL values ('RES', 'Responsable de secteur');
insert into PROFIL values ('ADM', 'Administrateur');

-- ---------------------------------------------------------------------
-- Praticiens
-- ---------------------------------------------------------------------
insert into TYPE_PRATICIEN values ('MV', 'Médecin de ville', 'Cabinet');
insert into TYPE_PRATICIEN values ('MH', 'Médecin hospitalier', 'Hôpital ou clinique');
insert into TYPE_PRATICIEN values ('PO', 'Pharmacien d''officine', 'Pharmacie');
insert into TYPE_PRATICIEN values ('PH', 'Pharmacien hospitalier', 'Hôpital');
insert into TYPE_PRATICIEN values ('PS', 'Personnel de santé', 'Cabinet paramédical');

insert into SPECIALITE values ('MGE', 'Médecine générale');
insert into SPECIALITE values ('CAR', 'Cardiologie');
insert into SPECIALITE values ('PED', 'Pédiatrie');
insert into SPECIALITE values ('PNE', 'Pneumologie');
insert into SPECIALITE values ('DER', 'Dermatologie');
insert into SPECIALITE values ('PSY', 'Psychiatrie');
insert into SPECIALITE values ('RHU', 'Rhumatologie');
insert into SPECIALITE values ('PHA', 'Pharmacie');
insert into SPECIALITE values ('INF', 'Soins infirmiers');

-- ---------------------------------------------------------------------
-- Produits
-- ---------------------------------------------------------------------
insert into FAMILLE values ('AAA', 'Antalgiques antipyrétiques en association');
insert into FAMILLE values ('AAC', 'Antidépresseur d''action centrale');
insert into FAMILLE values ('ABC', 'Antibiotique de la famille des céphalosporines');
insert into FAMILLE values ('ABP', 'Antibiotique de la famille des pénicillines');
insert into FAMILLE values ('AH',  'Antihistaminique');
insert into FAMILLE values ('AIN', 'Anti-inflammatoire non stéroïdien');
insert into FAMILLE values ('ALO', 'Antibiotique local');

insert into MEDICAMENT values ('AMOPIL7',   'AMOPIL',             'ABP', 'Traitement des infections à germes sensibles à l''amoxicilline.', 'Allergie aux pénicillines, mononucléose infectieuse.', 1.50, date '2015-03-01', 'O');
insert into MEDICAMENT values ('AMOX45',    'AMOXAR',             'ABP', 'Infections ORL, respiratoires et urinaires.', 'Allergie aux bêta-lactamines.', 2.20, date '2016-06-15', 'O');
insert into MEDICAMENT values ('APATOUX22', 'APATOUX Vitamine C', 'AAA', 'Traitement symptomatique de la fièvre et des douleurs légères.', 'Insuffisance hépatique, phénylcétonurie.', 0.90, date '2014-01-10', 'O');
insert into MEDICAMENT values ('BACTIG10',  'BACTIGEL',           'ALO', 'Infections cutanées superficielles.', 'Allergie à l''un des composants.', 1.80, date '2017-09-01', 'O');
insert into MEDICAMENT values ('DOLRIL7',   'DOLORIL',            'AAA', 'Douleurs d''intensité modérée à intense.', 'Insuffisance respiratoire, enfant de moins de 12 ans.', 1.10, date '2013-05-20', 'O');
insert into MEDICAMENT values ('TROXT21',   'TROXADET',           'AAC', 'Épisodes dépressifs majeurs.', 'Association aux IMAO, grossesse.', 3.40, date '2018-02-01', 'O');
insert into MEDICAMENT values ('CLAZER6',   'CLAZER',             'ABC', 'Infections respiratoires basses de l''adulte.', 'Allergie aux céphalosporines.', 2.60, date '2019-10-01', 'O');
insert into MEDICAMENT values ('EQUILARX6', 'EQUILAR',            'AH',  'Rhinite allergique, urticaire.', 'Insuffisance rénale sévère.', 1.30, date '2016-04-01', 'O');
insert into MEDICAMENT values ('INSXT5',    'INSECTIL',           'AH',  'Piqûres d''insectes, prurit.', 'Application sur une plaie ouverte.', 0.70, date '2012-07-01', 'O');
insert into MEDICAMENT values ('LIDOXY23',  'LIDOXYTRACINE',      'ALO', 'Infections cutanées douloureuses.', 'Allergie aux anesthésiques locaux.', 1.90, date '2015-11-01', 'O');
-- Nouveauté 2026 (motif « nouveauté »)
insert into MEDICAMENT values ('NOVEL26',   'NOVELIX',            'AIN', 'Douleurs articulaires et inflammatoires.', 'Ulcère gastroduodénal, grossesse (3e trimestre).', 2.10, date '2026-05-01', 'O');

insert into COMPOSANT values ('AMOX', 'Amoxicilline');
insert into COMPOSANT values ('ACSA', 'Acide ascorbique');
insert into COMPOSANT values ('PARA', 'Paracétamol');
insert into COMPOSANT values ('CAFE', 'Caféine');
insert into COMPOSANT values ('CODE', 'Codéine');
insert into COMPOSANT values ('TROX', 'Troxadétine');
insert into COMPOSANT values ('CEFU', 'Céfuroxime');
insert into COMPOSANT values ('CETI', 'Cétirizine');
insert into COMPOSANT values ('IBUP', 'Ibuprofène');
insert into COMPOSANT values ('NEOM', 'Néomycine');
insert into COMPOSANT values ('LIDO', 'Lidocaïne');
insert into COMPOSANT values ('OXYT', 'Oxytétracycline');

insert into CONSTITUER values ('AMOPIL7',   'AMOX', 500, 'mg');
insert into CONSTITUER values ('AMOX45',    'AMOX', 1,   'g');
insert into CONSTITUER values ('APATOUX22', 'PARA', 500, 'mg');
insert into CONSTITUER values ('APATOUX22', 'ACSA', 200, 'mg');
insert into CONSTITUER values ('BACTIG10',  'NEOM', 0.5, '%');
insert into CONSTITUER values ('DOLRIL7',   'PARA', 400, 'mg');
insert into CONSTITUER values ('DOLRIL7',   'CODE', 20,  'mg');
insert into CONSTITUER values ('DOLRIL7',   'CAFE', 50,  'mg');
insert into CONSTITUER values ('TROXT21',   'TROX', 20,  'mg');
insert into CONSTITUER values ('CLAZER6',   'CEFU', 250, 'mg');
insert into CONSTITUER values ('EQUILARX6', 'CETI', 10,  'mg');
insert into CONSTITUER values ('INSXT5',    'CETI', 1,   '%');
insert into CONSTITUER values ('LIDOXY23',  'LIDO', 2,   '%');
insert into CONSTITUER values ('LIDOXY23',  'OXYT', 3,   '%');
insert into CONSTITUER values ('NOVEL26',   'IBUP', 400, 'mg');

insert into INTERAGIR values ('DOLRIL7', 'TROXT21', 'Majoration de l''effet sédatif (codéine + antidépresseur).');
insert into INTERAGIR values ('NOVEL26', 'TROXT21', 'Risque hémorragique accru.');
insert into INTERAGIR values ('NOVEL26', 'APATOUX22', 'Association déconseillée sans avis médical.');

insert into PRESENTATION values ('CP',  'Comprimé');
insert into PRESENTATION values ('GEL', 'Gélule');
insert into PRESENTATION values ('SIR', 'Sirop');
insert into PRESENTATION values ('PDR', 'Poudre pour suspension buvable');
insert into PRESENTATION values ('CRM', 'Crème / pommade');

insert into DOSAGE values ('100MG', 100, 'mg');
insert into DOSAGE values ('250MG', 250, 'mg');
insert into DOSAGE values ('500MG', 500, 'mg');
insert into DOSAGE values ('1G',    1,   'g');
insert into DOSAGE values ('5ML',   5,   'ml');
insert into DOSAGE values ('2PC',   2,   '%');

insert into TYPE_INDIVIDU values ('ADU', 'Adulte');
insert into TYPE_INDIVIDU values ('JAD', 'Jeune adulte');
insert into TYPE_INDIVIDU values ('ENF', 'Enfant');
insert into TYPE_INDIVIDU values ('JEN', 'Jeune enfant');
insert into TYPE_INDIVIDU values ('NOU', 'Nourrisson');

insert into PRESCRIRE values ('AMOPIL7',   'ADU', 'GEL', '500MG', '1 gélule 3 fois par jour pendant 7 jours');
insert into PRESCRIRE values ('AMOPIL7',   'ENF', 'PDR', '250MG', '1 dose 3 fois par jour pendant 7 jours');
insert into PRESCRIRE values ('AMOPIL7',   'NOU', 'PDR', '100MG', '1 dose 2 fois par jour, adapter au poids');
insert into PRESCRIRE values ('AMOX45',    'ADU', 'CP',  '1G',    '1 comprimé 2 fois par jour');
insert into PRESCRIRE values ('APATOUX22', 'ADU', 'CP',  '500MG', '1 à 2 comprimés, 3 fois par jour maximum');
insert into PRESCRIRE values ('APATOUX22', 'JAD', 'CP',  '500MG', '1 comprimé 3 fois par jour maximum');
insert into PRESCRIRE values ('DOLRIL7',   'ADU', 'CP',  '500MG', '1 comprimé toutes les 6 heures');
insert into PRESCRIRE values ('TROXT21',   'ADU', 'CP',  '100MG', '1 comprimé par jour le matin');
insert into PRESCRIRE values ('CLAZER6',   'ADU', 'CP',  '250MG', '1 comprimé 2 fois par jour pendant 5 jours');
insert into PRESCRIRE values ('EQUILARX6', 'ADU', 'CP',  '100MG', '1 comprimé par jour');
insert into PRESCRIRE values ('EQUILARX6', 'ENF', 'SIR', '5ML',   '1 cuillère-mesure par jour');
insert into PRESCRIRE values ('INSXT5',    'ADU', 'CRM', '2PC',   '2 applications par jour');
insert into PRESCRIRE values ('INSXT5',    'JEN', 'CRM', '2PC',   '1 application par jour');
insert into PRESCRIRE values ('NOVEL26',   'ADU', 'CP',  '100MG', '1 comprimé 3 fois par jour au cours des repas');

-- ---------------------------------------------------------------------
-- Comptes-rendus de visite
-- ---------------------------------------------------------------------
insert into MOTIF values ('PERIO',  'Périodicité',                          1, 'O');
insert into MOTIF values ('NOUV',   'Nouveauté / actualisation',            2, 'O');
insert into MOTIF values ('REMONT', 'Remontage (baisse de prescription)',   3, 'O');
insert into MOTIF values ('SOLLIC', 'Sollicitation du praticien',           4, 'O');
insert into MOTIF values ('AUTRE',  'Autre (à préciser)',                   9, 'O');

commit;
