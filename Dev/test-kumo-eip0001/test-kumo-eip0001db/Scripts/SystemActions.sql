print N'Configuring SystemActions'


merge into [dbo].[SystemActions] as Target
using (values
	(10001, N'View'),
	(10002, N'Add'),
	(10003, N'Edit'),
	(10004, N'Delete')
) as Source ([Id], [Name])
on Target.[Id] = Source.[Id]

when matched then
update set [Name] = Source.[Name]

when not matched by target then
insert ([Id], [Name])
values ([Id], [Name]);
