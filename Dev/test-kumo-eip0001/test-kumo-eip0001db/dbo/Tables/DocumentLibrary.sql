CREATE TABLE [dbo].[DocumentLibrary]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(255) NOT NULL, 
    [Description] NVARCHAR(500) NULL,
	BlobContainer nvarchar(250) NOT NULL
)
