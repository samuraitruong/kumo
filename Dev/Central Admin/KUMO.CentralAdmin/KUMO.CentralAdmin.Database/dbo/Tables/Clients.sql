CREATE TABLE [dbo].[Clients]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1), 
    [ClientName] NVARCHAR(255) NOT NULL, 
    [DBName] NVARCHAR(255) NOT NULL, 
    [DBUser] NVARCHAR(255) NOT NULL, 
    [DBPassword] NVARCHAR(255) NOT NULL, 
    [DBServer] NVARCHAR(255) NOT NULL, 
    [Status] NVARCHAR(50) NOT NULL, 
    [SiteUrl] NVARCHAR(500) NOT NULL, 
    [ActiveComponents] NVARCHAR(500) NULL, 
    [DeploymentLogs] NVARCHAR(MAX) NULL, 
    [DeployedDate] DATE NOT NULL
   
)
