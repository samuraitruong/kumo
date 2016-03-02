print N'Configuring Departments'

SET IDENTITY_INSERT [dbo].[Department] ON
GO

merge into [dbo].[Department] as Target
using (values
	(1, N'Finance'),
	(2, N'Human Resource'),
	(3, N'Management'),
	(4, N'Marketing'),
	(5, N'Sales')
) as Source ([Id], [Name])
on Target.[Id] = Source.[Id]

when matched then
update set [Name] = Source.[Name]

when not matched by target then
insert ([Id], [Name])
values ([Id], [Name]);

SET IDENTITY_INSERT [dbo].[Department] OFF
GO