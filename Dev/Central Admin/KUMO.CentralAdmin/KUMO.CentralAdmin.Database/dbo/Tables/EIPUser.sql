CREATE TABLE [dbo].[EIPUsers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
	Email nvarchar(200) NOT NULL, 
    [ClientId] INT NULL, 
    [FirstName] NVARCHAR(200) NOT NULL, 
    [Lastname] NVARCHAR(200) NOT NULL, 
    [IsAdmin] BIT NOT NULL DEFAULT 0, 
	[Phone] NVARCHAR(255) NULL, 
    [CreatedDate] DATETIME NOT NULL, 
    [Company] NVARCHAR(250) NULL, 

    [Type] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_EIPUser_Client] FOREIGN KEY (ClientId) REFERENCES Clients(Id) ON DELETE CASCADE
)

GO

CREATE UNIQUE INDEX [IX_EIPUser_Column] ON [dbo].[EIPUsers] (Email)
