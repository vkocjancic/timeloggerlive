insert into CD_ACCOUNT_TYPE values ('PRSN', 'Personal account', default, '2017-05-01', null);
insert into CD_ACCOUNT_TYPE values ('BSNS', 'Business account', default, '2017-05-01', null);

insert into CD_BILLING_OPTION values ('PYRS', 'Personal - yearly subscription', 'Y', 10, default, '2017-05-01', null, 1);
insert into CD_BILLING_OPTION values ('BYRS', 'Business - yearly subscription', 'Y', 100, default, '2017-05-01', null, 2);

insert into [USER] values (NEWID(), 'admin', 'wIt/IiPbfWwCexSqAHcOUpok2iLiHO0pHxRG5PTcaH74aYcEheLppgUe8yPlH3guNBZW8SzZTjQLUguSre26jg==..N2Zw09W02Jc=', getdate() );