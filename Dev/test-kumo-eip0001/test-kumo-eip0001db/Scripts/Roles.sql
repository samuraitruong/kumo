print N'Configuring Roles'

merge into [dbo].[AspNetRoles] as Target
using (values
	(1, N'Company Admin'),
	(2, N'User')
) as Source ([Id], [Name])
on Target.[Id] = Source.[Id]

when matched then
update set [Name] = Source.[Name]

when not matched by target then
insert ([Id], [Name])
values ([Id], [Name]);
GO