/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

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
GO

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

print N'Configuring Industries'

SET IDENTITY_INSERT [dbo].[Industry] ON
GO

merge into [dbo].[Industry] as Target
using (values
	(1, N'Accounting Services'),
	(2, N'Aerospace Engineering'),
	(3, N'Architecture'),
	(4, N'Arts, Music & Drama'),
	(5, N'Banking'),
	(6, N'Beauty Care'),
	(7, N'Biomedical Sciences'),
	(8, N'Chemicals'),
	(9, N'Civil/ Public Service'),
	(10, N'Clean Technology'),
	(11, N'Commodities'),
	(12, N'Construction'),
	(13, N'Consulting Services'),
	(14, N'Consumer Business'),
	(15, N'Education'),
	(16, N'Electrical'),
	(17, N'Electronics'),
	(18, N'Energy'),
	(19, N'Engineering Services'),
	(20, N'Entertainment'),
	(21, N'Fashion'),
	(22, N'Financial Services'),
	(23, N'Food & Beverage'),
	(24, N'Freight Forwarding & Logistics'),
	(25, N'Garment & Textile'),
	(26, N'Graphics Design'),
	(27, N'Government'),
	(28, N'Healthcare Services'),
	(29, N'Hospitality'),
	(30, N'Industrial Automation and Control'),
	(31, N'Infocomm Technology'),
	(32, N'Information & Communications'),
	(33, N'Insurance'),
	(34, N'Interior Design'),
	(35, N'Investment'),
	(36, N'International Organizations, Non-Profits & Social Services'),
	(37, N'Legal Services'),
	(38, N'Manufacturing'),
	(39, N'Marine & Shipbuilding'),
	(40, N'Media & Digital Entertainment'),
	(41, N'Medical'),
	(42, N'Motoring'),
	(43, N'Office Equipment & Supplies'),
	(44, N'Oil & Gas'),
	(45, N'Packaging'),
	(46, N'Pharmaceutical'),
	(47, N'Pregnancy & Parenting'),
	(48, N'Printing'),
	(49, N'Professional & Business Services'),
	(50, N'Publishing'),
	(51, N'Real Estate & Property'),
	(52, N'Recruitment'),
	(53, N'Research & Academia'),
	(54, N'Retail'),
	(55, N'Rubber & Plastic'),
	(56, N'Safety & Security'),
	(57, N'Sports'),
	(58, N'Telecommunications'),
	(59, N'Transport & Storage'),
	(60, N'Travel & Tourism'),
	(61, N'Wholesale & Trading'),
	(62, N'Other')
) as Source ([Id], [Name])
on Target.[Id] = Source.[Id]

when matched then
update set [Name] = Source.[Name]

when not matched by target then
insert ([Id], [Name])
values ([Id], [Name]);

SET IDENTITY_INSERT [dbo].[Industry] OFF
GO

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
GO