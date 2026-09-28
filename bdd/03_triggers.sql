-- =====================================================================
-- GSB-CR : déclencheurs (règles qu'une contrainte CHECK ne peut pas porter)
-- Codes d'erreur applicatifs : -20001 à -20099 (repris dans la couche Données)
-- =====================================================================
whenever sqlerror exit failure

-- Rapport de visite :
--  * la date de visite ne peut pas être dans le futur        (-20001)
--  * l'auteur et la date de saisie ne peuvent pas être modifiés (-20002)
--  * horodatage automatique des modifications et de la validation
create or replace trigger TRG_RAPPORT_VISITE_BIU
before insert or update on RAPPORT_VISITE
for each row
begin
    if :new.rap_date_visite > sysdate then
        raise_application_error(-20001, 'La date de visite ne peut pas être dans le futur.');
    end if;

    if inserting then
        -- La date de saisie vaut systimestamp par défaut (l'application ne la fournit pas ;
        -- seule une reprise de données / le jeu d'essai peut la préciser).
        :new.rap_date_saisie := nvl(:new.rap_date_saisie, systimestamp);
        :new.rap_date_modif  := null;
        if :new.rap_etat = 'V' then
            :new.rap_date_validation := nvl(:new.rap_date_validation, :new.rap_date_saisie);
        else
            :new.rap_date_validation := null;
        end if;
    else
        if :new.col_matricule <> :old.col_matricule then
            raise_application_error(-20002, 'L''auteur d''un compte-rendu ne peut pas être modifié.');
        end if;
        :new.rap_date_saisie := :old.rap_date_saisie;
        :new.rap_date_modif  := systimestamp;
        if :old.rap_etat = 'B' and :new.rap_etat = 'V' then
            :new.rap_date_validation := systimestamp;
        elsif :new.rap_etat = 'B' then
            :new.rap_date_validation := null;
        end if;
    end if;
end;
/

-- Dotation : le saisissant doit être le délégué (ou un responsable / admin) en cours (-20003)
create or replace trigger TRG_DOTATION_BI
before insert or update on DOTATION
for each row
declare
    v_nb number;
begin
    select count(*)
      into v_nb
      from AFFECTATION
     where col_matricule = :new.col_matricule_saisie
       and aff_date_fin is null
       and pro_code in ('DEL', 'RES', 'ADM');

    if v_nb = 0 then
        raise_application_error(-20003, 'Seul un délégué régional peut attribuer des échantillons.');
    end if;
end;
/

-- Praticien : mise à jour automatique de la date de dernière modification
create or replace trigger TRG_PRATICIEN_BU
before update on PRATICIEN
for each row
begin
    :new.pra_date_maj := sysdate;
end;
/
