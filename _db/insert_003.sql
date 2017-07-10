select [DESCRIPTION], MIN([CREATED]) as [CREATED], [USER_ID]
into #desc_temp
from TIME_LOG
group by [DESCRIPTION], [USER_ID]

insert into ASSIGNMENT
select NEWID(), [DESCRIPTION], [CREATED], null, [USER_ID]
from #desc_temp;

go