-- =====================================================================
-- GSB-CR : compte Oracle utilisé par les postes (à exécuter en SYSTEM)
--   sql SYSTEM/<mdp>@//<serveur>:1521/<service> @bdd/06_compte_applicatif.sql "<mot_de_passe_GSB_APP>"
--
-- Les postes ne se connectent pas avec le propriétaire du schéma (GSB),
-- mais avec GSB_APP, qui peut seulement lire et écrire les données :
-- aucun droit de créer, modifier ou supprimer des tables.
-- L'application passe sur le schéma GSB au moyen du paramètre « Schema »
-- de appsettings.json (ALTER SESSION SET CURRENT_SCHEMA).
--
-- Relançable sans risque : crée le compte s'il n'existe pas, puis
-- (re)donne les droits sur toutes les tables et vues de GSB. À relancer
-- après chaque mise à jour qui ajoute une table ou une vue.
-- =====================================================================
whenever sqlerror exit failure
set feedback off
set verify off
set define on

define mdp_app = '&1'

prompt == Compte GSB_APP
declare
    nb number;
begin
    select count(*) into nb from dba_users where username = 'GSB_APP';
    if nb = 0 then
        execute immediate 'create user GSB_APP identified by "&mdp_app" '
                       || 'default tablespace USERS temporary tablespace TEMP';
    end if;
    execute immediate 'grant create session to GSB_APP';
end;
/

prompt == Droits sur les données de GSB
begin
    for t in (select table_name from dba_tables where owner = 'GSB') loop
        execute immediate 'grant select, insert, update, delete on GSB."' || t.table_name || '" to GSB_APP';
    end loop;
    for v in (select view_name from dba_views where owner = 'GSB') loop
        execute immediate 'grant select on GSB."' || v.view_name || '" to GSB_APP';
    end loop;
end;
/
prompt == Compte applicatif prêt
