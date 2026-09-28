-- =====================================================================
-- GSB-CR : installation de PRODUCTION (à exécuter connecté en GSB)
--   sql GSB/<mdp>@//<serveur>:1521/<service> @bdd/installer_production.sql "<hash>"
-- <hash> : mot de passe provisoire de l'administrateur, haché avec
--          scripts/hacher-mot-de-passe.ps1 (jamais le mot de passe en clair).
--
-- Crée tables, vues, déclencheurs et référentiels, puis un seul compte :
-- l'administrateur (login « admin »), qui devra changer son mot de passe
-- à la première connexion. Aucune donnée de test n'est chargée.
-- Refuse de s'exécuter si le schéma contient déjà les tables (rien n'est
-- supprimé : pour une base existante, utiliser les scripts de migration).
-- =====================================================================
whenever sqlerror exit failure rollback
set feedback off
set verify off
set define on

define hash_admin = '&1'

prompt == Vérification : le schéma doit être vide
declare
    nb number;
begin
    select count(*) into nb from user_tables where table_name = 'COLLABORATEUR';
    if nb > 0 then
        raise_application_error(-20001,
            'Le schéma contient déjà GSB-CR : installation annulée, aucune donnée modifiée.');
    end if;
    if '&hash_admin' not like 'PBKDF2-SHA256$%$%$%' then
        raise_application_error(-20002,
            'Paramètre invalide : fournir le mot de passe haché par scripts/hacher-mot-de-passe.ps1.');
    end if;
end;
/

set define off
prompt == Tables
@@01_tables.sql
prompt == Vues
@@02_vues.sql
prompt == Déclencheurs
@@03_triggers.sql
prompt == Référentiels
@@04_referentiels.sql

set define on
prompt == Compte administrateur (mot de passe provisoire à changer)
insert into COLLABORATEUR (col_matricule, col_nom, col_prenom, col_date_embauche, col_login, col_mdp, col_mdp_a_changer)
values ('adm1', 'Administrateur', 'GSB', trunc(sysdate), 'admin', '&hash_admin', 'O');
insert into AFFECTATION (col_matricule, pro_code, reg_code, sec_code, aff_date_debut, aff_date_fin)
values ('adm1', 'ADM', null, null, trunc(sysdate), null);
commit;
set define off

prompt == Recompilation des vues
begin
    for v in (select object_name from user_objects where object_type = 'VIEW' and status <> 'VALID') loop
        execute immediate 'alter view "' || v.object_name || '" compile';
    end loop;
end;
/
prompt == Installation terminée : connectez-vous avec le login « admin » et le mot de passe provisoire.
