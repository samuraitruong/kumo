print N'Configuring Components'

merge into [dbo].[Components] as Target
using (values
	(1, N'HR'),
	(2, N'CRM'),
	(3, N'CCS'),
	(4, N'ERP'),
	(5, N'DMS'),
	(6, N'MIS')
) as Source ([Id], [Name])
on Target.[Id] = Source.[Id]

when matched then
update set [Name] = Source.[Name]

when not matched by target then
insert ([Id], [Name])
values ([Id], [Name]);