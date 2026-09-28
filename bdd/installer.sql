-- =====================================================================
-- GSB-CR : installation complète du schéma (à exécuter connecté en GSB)
--   sql GSB/<mdp>@//100.109.217.110:1521/FREEPDB1 @bdd/installer.sql
-- Supprime tout puis recrée tables, vues, déclencheurs et jeu d'essai.
-- =====================================================================
whenever sqlerror exit failure
set feedback off
set define off

prompt == Suppression des objets existants
@@99_suppression.sql
prompt == Tables
@@01_tables.sql
prompt == Vues
@@02_vues.sql
prompt == Déclencheurs
@@03_triggers.sql
prompt == Jeu d'essai
@@04_jeu_essai.sql
prompt == Recompilation des vues
begin
    for v in (select object_name from user_objects where object_type = 'VIEW' and status <> 'VALID') loop
        execute immediate 'alter view "' || v.object_name || '" compile';
    end loop;
end;
/
prompt == Installation terminée
