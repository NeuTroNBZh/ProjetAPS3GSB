-- =====================================================================
-- GSB-CR : jeu d'essai (données fictives de test, développement uniquement)
-- Nécessite les référentiels (04_referentiels.sql).
-- Mot de passe de TOUS les comptes de test : voir docs/comptes-test.md
-- Hachage : PBKDF2-SHA256, 100 000 itérations, sel 16 octets, clé 32 octets
-- Format  : PBKDF2-SHA256$<iterations>$<sel base64>$<cle base64>
-- =====================================================================
whenever sqlerror exit failure
set define off

-- ---------------------------------------------------------------------
-- Collaborateurs et affectations
-- ---------------------------------------------------------------------
-- matricule, nom, prénom, adresse, cp, ville, tél, email, embauche, départ, login, mdp, mdp_a_changer
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('adm1', 'Système', 'Admin', null, null, null, null, 'admin@gsb.fr', date '2020-01-01', null, 'admin', 'PBKDF2-SHA256$100000$dYrERnNnOZrb6qONO7/PXA==$wrrDu/Yc0mXrGvC0XUrKavsaRFgl0Oh0mxSC1+8Vhbc=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('r01', 'Lemoine', 'Claire', '12 rue des Vosges', '67000', 'Strasbourg', '0388001122', 'claire.lemoine@gsb.fr', date '2008-09-01', null, 'clemoine', 'PBKDF2-SHA256$100000$CQPQzqnR+IxoNiuaXuV/Uw==$Wi4bMXHGbq7qC0fwa0lEAX/TIAvV6h4kfzl8FAzOppU=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('r02', 'Garnier', 'Paul', '4 quai de la Fosse', '44000', 'Nantes', '0240112233', 'paul.garnier@gsb.fr', date '2010-02-15', null, 'pgarnier', 'PBKDF2-SHA256$100000$HAKU2MT9sbFuUvyniFwKNQ==$3aoEcjbRQsmHxEMFnnOXgOOfQ09b0hdp9CZiTp+htkk=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('r03', 'Moreau', 'Sophie', '8 place du Théâtre', '59000', 'Lille', '0320445566', 'sophie.moreau@gsb.fr', date '2012-05-02', null, 'smoreau', 'PBKDF2-SHA256$100000$47T6DkAtDLE6Td7w32bgiA==$XEQDcXXjTK77bHe3Cq9BNX6GQRGI8L1UFyZuUJAblJo=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('d01', 'Bedos', 'Christian', '1 cours de l''Intendance', '33000', 'Bordeaux', '0556102030', 'christian.bedos@gsb.fr', date '2005-03-01', null, 'cbedos', 'PBKDF2-SHA256$100000$dG0+gsuCG9uQWdTEJDifhg==$v2WwL7l6u0JMqG24myWYmddvSvSY/lJ8v4Ej/TuiD6k=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('d02', 'Cacheux', 'Bernard', '22 rue Jeanne d''Arc', '76000', 'Rouen', '0235203040', 'bernard.cacheux@gsb.fr', date '2001-10-01', null, 'bcacheux', 'PBKDF2-SHA256$100000$2u35R50iwcZbazM6vtqoMg==$7VK1ARlsjqyLqCaPjJBlSbXtujMD85nmD4RUWA6ryQQ=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('d03', 'Cadic', 'Eric', '5 rue Serpenoise', '57000', 'Metz', '0387304050', 'eric.cadic@gsb.fr', date '2009-06-15', null, 'ecadic', 'PBKDF2-SHA256$100000$NtlYbWn6Dc09Xd2Le/+1Mw==$ZvjpWfc+uOHfDYXI4+vtgG3pspLgaOITi5qEdFnA/vY=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('a131', 'Villechalane', 'Louis', '8 rue des Charmes', '33700', 'Mérignac', '0556405060', 'louis.villechalane@gsb.fr', date '2019-12-21', null, 'lvillechalane', 'PBKDF2-SHA256$100000$HZbeCl2b2eme0nWe9SIoug==$nw2KtNekqqAMPZrtQuvW7rjm2FzaAarjrR/rnytaeC4=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('a17', 'Andre', 'David', '1 rue Petit', '33600', 'Pessac', '0556506070', 'david.andre@gsb.fr', date '2016-11-23', null, 'dandre', 'PBKDF2-SHA256$100000$4SN3axuuiVkhOMmVMsJgTg==$7z8MFq5l5G9uuVA6861LJRKgzyg18EIs9/WLWGkwyQc=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('b13', 'Bentot', 'Pascal', '11 allée des Cerises', '14000', 'Caen', '0231607080', 'pascal.bentot@gsb.fr', date '1987-03-01', null, 'pbentot', 'PBKDF2-SHA256$100000$lQXCO8+wpVHnrZark6hwdA==$DwPFhFGzzALKoC/q8L4DrRXj5IuaFLqzRjMy9gS9ZN8=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('b16', 'Bioret', 'Luc', '1 avenue Gambetta', '76600', 'Le Havre', '0235708090', 'luc.bioret@gsb.fr', date '2024-02-05', null, 'lbioret', 'PBKDF2-SHA256$100000$WaEqyRfeh0uC0+m7vlA+Fg==$85b5SbrPY8bE6PmTuziHpv7MNRiqTbYrRcI110AAiDc=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('b19', 'Bunisset', 'Francis', '10 rue des Perles', '68100', 'Mulhouse', '0389809010', 'francis.bunisset@gsb.fr', date '2021-01-04', null, 'fbunisset', 'PBKDF2-SHA256$100000$5RwK1j4xWS5c2L6DGnqYaw==$p6Zc9kCFJJKM3HkYRlxQlAxBe1EYaYpuS2afWPzLIns=', 'N');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('b25', 'Bunisset', 'Denise', '23 rue Manin', '68000', 'Colmar', '0389901020', 'denise.bunisset@gsb.fr', date '2023-09-11', null, 'dbunisset', 'PBKDF2-SHA256$100000$+BWVNSWk5sFraWc07QVz2A==$RiA0u0IW22/F+H/q4M7RrlLGJtjhekZ5wzm7fF2nn0M=', 'O');
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_adresse, col_cp, col_ville, col_telephone, col_email, col_date_embauche, col_date_depart, col_login, col_mdp, col_mdp_a_changer)
values ('c14', 'Daburon', 'François', '13 rue de Chanzy', '27000', 'Évreux', '0232112233', null, date '2024-01-08', date '2025-06-30', 'fdaburon', 'PBKDF2-SHA256$100000$TtciZfWclUEp4e6xxcc8ug==$9SIDOxdm3wLaMp/BVIRJLb1NAlmpy+SvGYfmDzQ9oDQ=', 'N');

-- Affectations (historique)
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('adm1', 'ADM', null,  null, date '2020-01-01', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('r01',  'VIS', 'ALS', null, date '2008-09-01', date '2014-12-31');
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('r01',  'RES', null,  'E',  date '2015-01-01', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('r02',  'RES', null,  'O',  date '2010-02-15', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('r03',  'RES', null,  'N',  date '2012-05-02', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('d01',  'VIS', 'AQU', null, date '2005-03-01', date '2017-12-31');
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('d01',  'DEL', 'AQU', null, date '2018-01-01', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('d02',  'DEL', 'NOR', null, date '2001-10-01', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('d03',  'DEL', 'ALS', null, date '2009-06-15', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('a131', 'VIS', 'AQU', null, date '2019-12-21', null);
-- David Andre : Aquitaine -> Bretagne -> retour en Aquitaine (même région deux fois)
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('a17',  'VIS', 'AQU', null, date '2016-11-23', date '2019-08-31');
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('a17',  'VIS', 'BRE', null, date '2019-09-01', date '2022-08-31');
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('a17',  'VIS', 'AQU', null, date '2022-09-01', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('b13',  'VIS', 'NOR', null, date '1987-03-01', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('b16',  'VIS', 'NOR', null, date '2024-02-05', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('b19',  'VIS', 'ALS', null, date '2021-01-04', null);
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('b25',  'VIS', 'ALS', null, date '2023-09-11', null);
-- François Daburon : parti de l'entreprise (turn-over)
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin) values ('c14',  'VIS', 'NOR', null, date '2024-01-08', date '2025-06-30');

-- ---------------------------------------------------------------------
-- Praticiens (fiches, spécialités, portefeuilles)
-- ---------------------------------------------------------------------
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (1,  'Martin',   'Hélène',  '15 cours Victor Hugo',      '33000', 'Bordeaux',   '0556111111', null, 420.50, 'MV', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (2,  'Durand',   'Pierre',  'CHU Pellegrin, place Amélie Raba-Léon', '33000', 'Bordeaux', '0556222222', null, 812.00, 'MH', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (3,  'Lefèvre',  'Anne',    '3 avenue de la Marne',      '33700', 'Mérignac',   '0556333333', null, 150.00, 'PO', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (4,  'Roux',     'Julien',  '27 avenue Jean Jaurès',     '33600', 'Pessac',     '0556444444', null, 305.75, 'MV', 'O');
-- Remplaçant : pas de cabinet propre
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (5,  'Petit',    'Camille', null, null, null, '0611555555', null, null, 'MV', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (6,  'Leroy',    'Jacques', '40 rue du Gros-Horloge',    '76000', 'Rouen',      '0235666666', null, 510.00, 'MV', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (7,  'Moreau',   'Isabelle','CHU, avenue de la Côte de Nacre', '14000', 'Caen', '0231777777', null, 690.25, 'MH', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (8,  'Simon',    'Marc',    '9 rue de Paris',            '76600', 'Le Havre',   '0235888888', null, 275.00, 'MV', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (9,  'Laurent',  'Nathalie','2 place Sepmanville',       '27000', 'Évreux',     '0232999999', null, 60.00,  'PS', 'O');
-- Remplaçant
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (10, 'Michel',   'Thomas',  null, null, null, '0622101010', null, null, 'MV', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (11, 'Garcia',   'Sophie',  '6 rue du Dôme',             '67000', 'Strasbourg', '0388111111', null, 380.00, 'MV', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (12, 'Muller',   'Hans',    'Hôpital civil, 1 place de l''Hôpital', '67000', 'Strasbourg', '0388121212', null, 745.00, 'MH', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (13, 'Schmitt',  'Claire',  '18 Grand''Rue',             '68000', 'Colmar',     '0389131313', null, 120.00, 'PO', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (14, 'Weber',    'Paul',    '31 rue du Sauvage',         '68100', 'Mulhouse',   '0389141414', null, 330.00, 'MV', 'O');
-- Praticien parti à la retraite (inactif)
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (15, 'Fontaine', 'Gérard',  '7 rue Sainte-Catherine',    '33000', 'Bordeaux',   '0556151515', null, 200.00, 'MV', 'N');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (16, 'Bernard',  'Louise',  '12 rue des Clercs',         '57000', 'Metz',       '0387161616', null, 410.00, 'MV', 'O');
insert into PRATICIEN (pra_num, pra_nom, pra_prenom, pra_adresse, pra_cp, pra_ville, pra_telephone, pra_email, pra_coef_notoriete, typ_code, pra_actif)
values (17, 'Blanc',    'Olivier', '45 boulevard de la Plage',  '33120', 'Arcachon',   '0556171717', null, 295.00, 'MV', 'O');

insert into POSSEDER values (1,  'MGE', 'Doctorat en médecine', 2.50);
insert into POSSEDER values (2,  'CAR', 'DES de cardiologie', 4.80);
insert into POSSEDER values (3,  'PHA', 'Doctorat en pharmacie', 1.20);
insert into POSSEDER values (4,  'MGE', 'Doctorat en médecine', 2.10);
insert into POSSEDER values (4,  'PED', 'DIU de pédiatrie', 3.40);
insert into POSSEDER values (5,  'MGE', 'Doctorat en médecine', 1.00);
insert into POSSEDER values (6,  'MGE', 'Doctorat en médecine', 3.10);
insert into POSSEDER values (7,  'PNE', 'DES de pneumologie', 4.50);
insert into POSSEDER values (8,  'MGE', 'Doctorat en médecine', 2.00);
insert into POSSEDER values (9,  'INF', 'Diplôme d''État infirmier', 0.80);
insert into POSSEDER values (10, 'MGE', 'Doctorat en médecine', 1.00);
insert into POSSEDER values (11, 'DER', 'DES de dermatologie', 3.90);
insert into POSSEDER values (12, 'PSY', 'DES de psychiatrie', 4.20);
insert into POSSEDER values (13, 'PHA', 'Doctorat en pharmacie', 1.50);
insert into POSSEDER values (14, 'MGE', 'Doctorat en médecine', 2.70);
insert into POSSEDER values (14, 'RHU', 'DIU de rhumatologie', 3.00);
insert into POSSEDER values (15, 'MGE', 'Doctorat en médecine', 2.20);
insert into POSSEDER values (16, 'MGE', 'Doctorat en médecine', 2.90);
insert into POSSEDER values (17, 'MGE', 'Doctorat en médecine', 2.40);

-- Portefeuilles
insert into PORTEFEUILLE values (1,  'a131', date '2019-12-21', null);
insert into PORTEFEUILLE values (2,  'a131', date '2019-12-21', null);
insert into PORTEFEUILLE values (15, 'a131', date '2019-12-21', date '2024-06-30');
insert into PORTEFEUILLE values (3,  'a17',  date '2022-09-01', null);
insert into PORTEFEUILLE values (4,  'a17',  date '2022-09-01', null);
insert into PORTEFEUILLE values (17, 'd01',  date '2018-01-01', null);
insert into PORTEFEUILLE values (6,  'b13',  date '1995-04-01', null);
insert into PORTEFEUILLE values (7,  'b13',  date '2003-09-01', null);
-- Transfert de portefeuille après le départ de François Daburon
insert into PORTEFEUILLE values (8,  'c14',  date '2024-01-15', date '2025-06-30');
insert into PORTEFEUILLE values (8,  'b16',  date '2025-07-01', null);
insert into PORTEFEUILLE values (9,  'b16',  date '2024-02-05', null);
insert into PORTEFEUILLE values (11, 'b19',  date '2021-01-04', null);
insert into PORTEFEUILLE values (12, 'b19',  date '2021-01-04', null);
insert into PORTEFEUILLE values (13, 'b25',  date '2023-09-11', null);
insert into PORTEFEUILLE values (14, 'b25',  date '2023-09-11', null);
insert into PORTEFEUILLE values (16, 'd03',  date '2009-06-15', null);

-- rap_num, auteur, praticien, remplaçant, date visite, motif, précision, bilan, confiance, prochaine visite, état, date saisie
-- CR de plus de 3 ans (ne doit plus apparaître dans la consultation « 3 années précédentes »)
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (1,  'a131', 1,  null, date '2022-05-10', 'PERIO',  null, 'Praticienne attentive, satisfaite de la gamme antibiotique.', 4, null, 'V', timestamp '2022-05-10 18:30:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (2,  'a131', 1,  null, date '2025-11-12', 'PERIO',  null, 'Bon accueil. Prescrit régulièrement AMOPIL. Rappel posologie DOLORIL.', 4, date '2026-05-15', 'V', timestamp '2025-11-12 19:05:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (3,  'a131', 2,  null, date '2026-02-03', 'NOUV',   null, 'Présentation du nouveau conditionnement CLAZER. Intéressé pour le service.', 3, null, 'V', timestamp '2026-02-04 08:15:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (4,  'a131', 1,  null, date '2026-05-18', 'PERIO',  null, 'Présentation de NOVELIX, accueil favorable. Échantillons de dépannage laissés.', 4, date '2026-11-20', 'V', timestamp '2026-05-18 20:10:00');
-- Visite chez le Dr Martin mais remplaçante vue (Camille Petit) : prochaine visite rapprochée
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (5,  'a131', 1,  5,    date '2026-09-10', 'NOUV',   null, 'Vu la remplaçante. Présentation rapide de NOVELIX, à revoir avec la titulaire.', 3, date '2026-10-15', 'V', timestamp '2026-09-10 18:45:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (6,  'a17',  3,  null, date '2026-06-22', 'REMONT', null, 'Baisse des ventes de TROXADET dans la zone. Pharmacienne peu convaincue.', 2, date '2026-09-30', 'V', timestamp '2026-06-23 09:00:00');
-- Brouillon (incomplet : pas encore de bilan ni de coefficient)
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (7,  'a17',  4,  null, date '2026-09-25', 'SOLLIC', null, null, null, null, 'B', timestamp '2026-09-25 19:20:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (8,  'b13',  6,  null, date '2026-04-14', 'PERIO',  null, 'Médecin fidèle depuis 20 ans. Très bonne connaissance des produits.', 5, date '2026-10-14', 'V', timestamp '2026-04-14 17:50:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (9,  'b13',  7,  null, date '2026-07-08', 'AUTRE',  'Présentation au staff mensuel du service de pneumologie', 'Présentation devant 8 médecins du service. Questions nombreuses sur AMOXAR.', 4, null, 'V', timestamp '2026-07-09 08:30:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (10, 'b13',  6,  10,   date '2026-09-16', 'PERIO',  null, 'Remplaçant présent (Dr Michel). Rappel de la gamme, à revoir avec le titulaire.', 3, date '2026-11-02', 'V', timestamp '2026-09-16 18:00:00');
-- CR d'un collaborateur parti de l'entreprise (conservé)
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (11, 'c14',  8,  null, date '2025-03-11', 'PERIO',  null, 'Accueil froid, préfère les génériques.', 2, null, 'V', timestamp '2025-03-12 10:00:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (12, 'b16',  8,  null, date '2026-08-19', 'REMONT', null, 'Reprise de contact après changement de visiteur. Plus ouvert que prévu.', 3, date '2027-02-19', 'V', timestamp '2026-08-19 19:30:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (13, 'b19',  11, null, date '2026-06-02', 'NOUV',   null, 'Lancement NOVELIX : très intéressée pour ses patients arthrosiques.', 4, date '2026-12-02', 'V', timestamp '2026-06-02 18:10:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (14, 'b25',  13, null, date '2026-07-15', 'PERIO',  null, 'Pharmacie très active en saison des allergies. Stock EQUILAR à suivre.', 5, date '2027-01-15', 'V', timestamp '2026-07-15 17:40:00');
-- Le délégué fait lui aussi des visites
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (15, 'd01',  17, null, date '2026-08-28', 'PERIO',  null, 'Visite estivale, patientèle touristique. Demande de documentation patient.', 4, date '2027-02-26', 'V', timestamp '2026-08-28 19:00:00');
insert into RAPPORT_VISITE (rap_num, col_matricule, pra_num, pra_num_remplacant, rap_date_visite, mot_code, rap_motif_autre, rap_bilan, rap_coef_confiance, rap_date_prochaine_visite, rap_etat, rap_date_saisie)
values (16, 'd03',  16, null, date '2026-09-02', 'NOUV',   null, 'Présentation NOVELIX. Souhaite un retour d''expérience de confrères.', 3, date '2026-12-01', 'V', timestamp '2026-09-02 18:20:00');

-- Produits présentés (2 maximum)
insert into PRESENTER values (1,  'AMOPIL7',  1);
insert into PRESENTER values (2,  'AMOPIL7',  1);
insert into PRESENTER values (2,  'DOLRIL7',  2);
insert into PRESENTER values (3,  'CLAZER6',  1);
insert into PRESENTER values (4,  'NOVEL26',  1);
insert into PRESENTER values (4,  'AMOPIL7',  2);
insert into PRESENTER values (5,  'NOVEL26',  1);
insert into PRESENTER values (6,  'TROXT21',  1);
insert into PRESENTER values (8,  'AMOX45',   1);
insert into PRESENTER values (8,  'BACTIG10', 2);
insert into PRESENTER values (9,  'AMOX45',   1);
insert into PRESENTER values (10, 'AMOX45',   1);
insert into PRESENTER values (11, 'DOLRIL7',  1);
insert into PRESENTER values (12, 'DOLRIL7',  1);
insert into PRESENTER values (13, 'NOVEL26',  1);
insert into PRESENTER values (14, 'EQUILARX6',1);
insert into PRESENTER values (14, 'INSXT5',   2);
insert into PRESENTER values (15, 'APATOUX22',1);
insert into PRESENTER values (16, 'NOVEL26',  1);

-- Échantillons offerts (à l'unité, pas forcément les produits présentés)
insert into OFFRIR values (2,  'AMOPIL7',   4);
insert into OFFRIR values (2,  'DOLRIL7',   2);
insert into OFFRIR values (3,  'CLAZER6',   3);
insert into OFFRIR values (4,  'NOVEL26',   5);
insert into OFFRIR values (4,  'APATOUX22', 2);
insert into OFFRIR values (4,  'EQUILARX6', 1);
insert into OFFRIR values (5,  'NOVEL26',   2);
insert into OFFRIR values (8,  'AMOX45',    3);
insert into OFFRIR values (8,  'BACTIG10',  3);
insert into OFFRIR values (8,  'INSXT5',    2);
insert into OFFRIR values (8,  'LIDOXY23',  1);
insert into OFFRIR values (12, 'DOLRIL7',   4);
insert into OFFRIR values (13, 'NOVEL26',   6);
insert into OFFRIR values (14, 'EQUILARX6', 10);
insert into OFFRIR values (15, 'APATOUX22', 3);
insert into OFFRIR values (16, 'NOVEL26',   2);

-- Sessions de saisie (temps passé à saisir les CR)
insert into SESSION_SAISIE (rap_num, col_matricule, ses_debut, ses_fin) values (2,  'a131', timestamp '2025-11-12 19:05:00', timestamp '2025-11-12 19:13:30');
insert into SESSION_SAISIE (rap_num, col_matricule, ses_debut, ses_fin) values (4,  'a131', timestamp '2026-05-18 20:10:00', timestamp '2026-05-18 20:17:00');
insert into SESSION_SAISIE (rap_num, col_matricule, ses_debut, ses_fin) values (4,  'a131', timestamp '2026-05-19 08:02:00', timestamp '2026-05-19 08:05:10');
insert into SESSION_SAISIE (rap_num, col_matricule, ses_debut, ses_fin) values (5,  'a131', timestamp '2026-09-10 18:45:00', timestamp '2026-09-10 18:51:40');
insert into SESSION_SAISIE (rap_num, col_matricule, ses_debut, ses_fin) values (7,  'a17',  timestamp '2026-09-25 19:20:00', timestamp '2026-09-25 19:22:30');
insert into SESSION_SAISIE (rap_num, col_matricule, ses_debut, ses_fin) values (8,  'b13',  timestamp '2026-04-14 17:50:00', timestamp '2026-04-14 18:04:00');
insert into SESSION_SAISIE (rap_num, col_matricule, ses_debut, ses_fin) values (13, 'b19',  timestamp '2026-06-02 18:10:00', timestamp '2026-06-02 18:16:20');

-- Modification d'un CR après saisie (tolérée : seule la dernière version compte)
update RAPPORT_VISITE
   set rap_bilan = rap_bilan || ' Complément : souhaite une documentation patient sur NOVELIX.'
 where rap_num = 4;

-- ---------------------------------------------------------------------
-- Dotations d'échantillons (saisies par les délégués)
-- ---------------------------------------------------------------------
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('a131', 'AMOPIL7', date '2025-11-01', 5, 'd01');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('a131', 'DOLRIL7', date '2025-11-01', 3, 'd01');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('a131', 'CLAZER6', date '2026-02-01', 3, 'd01');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('a131', 'NOVEL26',   date '2026-05-01', 10, 'd01');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('a131', 'APATOUX22', date '2026-05-01', 5,  'd01');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('a131', 'EQUILARX6', date '2026-05-01', 5,  'd01');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('a131', 'NOVEL26',   date '2026-09-01', 10, 'd01');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('b13',  'AMOX45',    date '2026-04-01', 5,  'd02');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('b13',  'BACTIG10',  date '2026-04-01', 3,  'd02');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('b13',  'INSXT5',    date '2026-04-01', 2,  'd02');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('b13',  'LIDOXY23',  date '2026-04-01', 2,  'd02');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('b19',  'NOVEL26',   date '2026-06-01', 10, 'd03');
-- Anomalie volontaire pour démontrer le contrôle de stock : 8 attribués, 10 distribués
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('b25',  'EQUILARX6', date '2026-07-01', 8,  'd03');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('b16', 'DOLRIL7', date '2026-08-01', 5, 'd02');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('d01', 'APATOUX22', date '2026-08-01', 5, 'd01');
insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values ('d03', 'NOVEL26', date '2026-09-01', 5, 'd03');

-- ---------------------------------------------------------------------
-- Messagerie
-- ---------------------------------------------------------------------
insert into MESSAGE (msg_id, col_expediteur, msg_objet, msg_contenu, msg_date_envoi)
values (1, 'r01', 'Lancement NOVELIX', 'Bonjour à tous, NOVELIX est disponible depuis mai. Merci de le présenter en priorité lors de vos prochaines visites.', timestamp '2026-05-02 09:00:00');
insert into MESSAGE_DESTINATAIRE values (1, 'd03', timestamp '2026-05-02 10:12:00');
insert into MESSAGE_DESTINATAIRE values (1, 'b19', timestamp '2026-05-03 08:40:00');
insert into MESSAGE_DESTINATAIRE values (1, 'b25', null);
insert into MESSAGE (msg_id, col_expediteur, msg_objet, msg_contenu, msg_date_envoi)
values (2, 'd01', 'Réunion bilan de septembre', 'Réunion bilan mensuelle le 30/09 à 14h à Bordeaux.', timestamp '2026-09-15 11:00:00');
insert into MESSAGE_DESTINATAIRE values (2, 'a131', null);
insert into MESSAGE_DESTINATAIRE values (2, 'a17',  timestamp '2026-09-15 18:00:00');

-- ---------------------------------------------------------------------
-- Journal de connexion
-- ---------------------------------------------------------------------
insert into JOURNAL_CONNEXION (jco_login, col_matricule, jco_date, jco_succes) values ('lvillechalane', 'a131', timestamp '2026-09-10 18:44:12', 'O');
insert into JOURNAL_CONNEXION (jco_login, col_matricule, jco_date, jco_succes) values ('pbentot',       'b13',  timestamp '2026-09-16 17:58:03', 'N');
insert into JOURNAL_CONNEXION (jco_login, col_matricule, jco_date, jco_succes) values ('pbentot',       'b13',  timestamp '2026-09-16 17:58:40', 'O');
insert into JOURNAL_CONNEXION (jco_login, col_matricule, jco_date, jco_succes) values ('inconnu',       null,   timestamp '2026-09-20 22:14:55', 'N');

-- Recaler les identités après les insertions avec identifiants explicites
alter table PRATICIEN      modify pra_num generated by default as identity (start with limit value);
alter table RAPPORT_VISITE modify rap_num generated by default as identity (start with limit value);
alter table MESSAGE        modify msg_id  generated by default as identity (start with limit value);

commit;
