-- =====================================================================
-- GSB-CR : suppression de TOUS les objets du schéma GSB (remise à zéro)
-- À exécuter connecté en GSB. Irréversible : toutes les données sont perdues.
-- =====================================================================
begin
    for v in (select view_name from user_views) loop
        execute immediate 'drop view "' || v.view_name || '"';
    end loop;
    for t in (select table_name from user_tables where table_name not like 'BIN$%') loop
        execute immediate 'drop table "' || t.table_name || '" cascade constraints purge';
    end loop;
    for s in (select sequence_name from user_sequences where sequence_name not like 'ISEQ$$%') loop
        execute immediate 'drop sequence "' || s.sequence_name || '"';
    end loop;
end;
/
