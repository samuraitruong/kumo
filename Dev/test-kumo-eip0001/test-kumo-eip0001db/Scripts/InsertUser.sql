USE [Kumo-Dev-Donot-Delete]
GO

INSERT INTO [dbo].[AspNetUsers]
           ([Id]
           ,[Email]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEndDateUtc]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[UserName]
           ,[Company]
           ,[Firstname]
           ,[Lastname]
           ,[Status])
     VALUES (
           '6c9de4ca-263a-4f9b-a767-2a805b8ed32f',
           'zyxzapr3@mailinator.com',
           0,
           'AA7xmGPq3+QNlgmal9GRt+/Q9QYr5j9+qD/qamX/nipQD8WD86Z6Fjc5ucCqxDPuXg==',
           'eb96e315-928b-4087-8889-1bd73e93923f',
		   Null,
           0,
           0
           ,NULL
           ,0
           ,0
           ,'zyxzapr3@mailinator.com'
           ,''
           ,'Alice'
           ,'Huynh'
           ,'Normal')
GO

INSERT INTO [dbo].[AspNetUserRoles]
           ([UserId]
           ,[RoleId])
     VALUES
           ('6c9de4ca-263a-4f9b-a767-2a805b8ed32f'
           ,1)
