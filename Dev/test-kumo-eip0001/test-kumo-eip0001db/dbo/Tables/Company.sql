CREATE TABLE [dbo].[Company]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(250) NOT NULL, 
    [DatabaseName] NVARCHAR(50) NOT NULL, 
    [DatabaseServer] NVARCHAR(150) NOT NULL, 
    [DatabasePassword] NVARCHAR(150) NOT NULL, 
    [Status] NVARCHAR(50) NOT NULL, 
    [DatabaseUser] NVARCHAR(50) NOT NULL 
)
