-- =====================================================================
-- GSB-CR : création du schéma applicatif GSB
-- À exécuter en SYSTEM sur la PDB (FREEPDB1).
-- Usage (SQLcl) : @bdd/00_creation_utilisateur.sql <mot_de_passe>
-- =====================================================================
whenever sqlerror exit failure

define mdp = '&1'

create user GSB identified by "&mdp"
    default tablespace USERS
    temporary tablespace TEMP
    quota unlimited on USERS;

grant create session,
      create table,
      create view,
      create sequence,
      create trigger,
      create procedure
   to GSB;
