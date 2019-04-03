create tablespace tbseventos datafile 'C:\app\Erik\oradata\produ\dtfappeventos.dbf' size 1024m extent management local segment space management auto;

create user eve01 identified by eve01 default tablespace tbseventos temporary tablespace temp account unlock;

grant connect, resource to eve01;
grant alter any index to eve01;
grant alter any sequence to eve01;
grant alter any table to eve01;
grant alter any trigger to eve01;
grant alter any procedure to eve01;
grant create any index to eve01;
grant create any sequence to eve01;
grant create any synonym to eve01;
grant create any table to eve01;
grant create any trigger to eve01;
grant create any view to eve01;
grant create procedure to eve01;
grant create public synonym to eve01;
grant create trigger to eve01;
grant create view to eve01;
grant delete any table to eve01;
grant drop any index to eve01;
grant drop any sequence to eve01;
grant drop any table to eve01;
grant drop any trigger to eve01;
grant drop any view to eve01;
grant insert any table to eve01;
grant query rewrite to eve01;
grant select any table to eve01;
grant unlimited tablespace to eve01;

grant execute any procedure to eve01;


select * from dba_profiles where resource_name like 'PASSWORD_LIFE_TIME';

alter profile default limit password_life_time unlimited;