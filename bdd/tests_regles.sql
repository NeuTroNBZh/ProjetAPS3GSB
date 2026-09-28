-- =====================================================================
-- GSB-CR : tests des règles de gestion portées par la base
-- Chaque cas tente une opération INTERDITE : elle doit être refusée.
-- Aucune donnée n'est modifiée (rollback final).
-- =====================================================================
set serveroutput on
set feedback off
declare
    v_ok  pls_integer := 0;
    v_ko  pls_integer := 0;

    procedure doit_echouer(p_nom varchar2, p_sql varchar2) is
    begin
        savepoint sp;
        execute immediate p_sql;
        rollback to sp;
        v_ko := v_ko + 1;
        dbms_output.put_line('KO  ' || p_nom || ' : accepté alors que ça devait être refusé');
    exception when others then
        rollback to sp;
        v_ok := v_ok + 1;
        dbms_output.put_line('OK  ' || p_nom || '  [' || substr(sqlerrm, 1, 70) || ']');
    end;
begin
    doit_echouer('EX-13 3e produit présenté',
        'insert into PRESENTER values (2, ''CLAZER6'', 3)');
    doit_echouer('EX-13 2 produits au même rang',
        'insert into PRESENTER values (3, ''AMOPIL7'', 1)');
    doit_echouer('EX-14 échantillon quantité 0',
        'insert into OFFRIR values (3, ''AMOPIL7'', 0)');
    doit_echouer('EX-11 CR validé sans bilan',
        'insert into RAPPORT_VISITE (col_matricule, pra_num, rap_date_visite, mot_code, rap_coef_confiance, rap_etat) values (''a131'', 1, date ''2026-09-01'', ''PERIO'', 3, ''V'')');
    doit_echouer('EX-11 CR sans praticien',
        'insert into RAPPORT_VISITE (col_matricule, rap_date_visite, rap_etat) values (''a131'', date ''2026-09-01'', ''B'')');
    doit_echouer('EX-10 CR sans auteur',
        'insert into RAPPORT_VISITE (pra_num, rap_date_visite, rap_etat) values (1, date ''2026-09-01'', ''B'')');
    doit_echouer('EX-12 motif AUTRE sans précision',
        'insert into RAPPORT_VISITE (col_matricule, pra_num, rap_date_visite, mot_code, rap_etat) values (''a131'', 1, date ''2026-09-01'', ''AUTRE'', ''B'')');
    doit_echouer('EX-12 précision avec un motif standard',
        'insert into RAPPORT_VISITE (col_matricule, pra_num, rap_date_visite, mot_code, rap_motif_autre, rap_etat) values (''a131'', 1, date ''2026-09-01'', ''PERIO'', ''x'', ''B'')');
    doit_echouer('EX-15 coefficient de confiance hors 1..5',
        'update RAPPORT_VISITE set rap_coef_confiance = 7 where rap_num = 2');
    doit_echouer('EX-16 remplaçant = titulaire',
        'update RAPPORT_VISITE set pra_num_remplacant = pra_num where rap_num = 2');
    doit_echouer('EX-19 prochaine visite avant la visite',
        'update RAPPORT_VISITE set rap_date_prochaine_visite = date ''2020-01-01'' where rap_num = 2');
    doit_echouer('Date de visite dans le futur',
        'insert into RAPPORT_VISITE (col_matricule, pra_num, rap_date_visite, rap_etat) values (''a131'', 1, sysdate + 5, ''B'')');
    doit_echouer('Changer l''auteur d''un CR',
        'update RAPPORT_VISITE set col_matricule = ''a17'' where rap_num = 2');
    doit_echouer('Portefeuille : 2 visiteurs pour 1 praticien',
        'insert into PORTEFEUILLE values (1, ''a17'', date ''2026-01-01'', null)');
    doit_echouer('2 affectations en cours pour 1 collaborateur',
        'insert into AFFECTATION (col_matricule, pro_code, reg_code, aff_date_debut) values (''a131'', ''VIS'', ''BRE'', date ''2026-01-01'')');
    doit_echouer('2 délégués en cours pour 1 région',
        'insert into AFFECTATION (col_matricule, pro_code, reg_code, aff_date_debut) values (''c14'', ''DEL'', ''AQU'', date ''2026-01-01'')');
    doit_echouer('Responsable rattaché à une région',
        'insert into AFFECTATION (col_matricule, pro_code, reg_code, aff_date_debut, aff_date_fin) values (''r02'', ''RES'', ''BRE'', date ''2000-01-01'', date ''2000-12-31'')');
    doit_echouer('EX-33 dotation saisie par un visiteur',
        'insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values (''a17'', ''AMOPIL7'', date ''2026-09-01'', 5, ''a131'')');
    doit_echouer('Dotation pas au 1er du mois',
        'insert into DOTATION (col_matricule, med_depot_legal, dot_mois, dot_quantite, col_matricule_saisie) values (''a17'', ''AMOPIL7'', date ''2026-09-15'', 5, ''d01'')');
    doit_echouer('Login en double',
        'insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_date_embauche, col_login, col_mdp) values (''z1'', ''X'', ''Y'', sysdate, ''admin'', ''h'')');
    doit_echouer('Interaction d''un médicament avec lui-même',
        'insert into INTERAGIR values (''AMOPIL7'', ''AMOPIL7'', null)');

    dbms_output.put_line('---');
    dbms_output.put_line('Règles vérifiées : ' || v_ok || ' OK, ' || v_ko || ' KO');
    rollback;
end;
/
