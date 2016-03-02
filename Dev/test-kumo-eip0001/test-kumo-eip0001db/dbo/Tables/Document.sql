CREATE TABLE [dbo].[Document]
(
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
	[Extension] nvarchar(10),	
	[ItemType] INT NOT NULL,
	[ParentId] INT,
    [Created]  DATETIME       NULL DEFAULT GETDATE(),
    [CreatedBy]  NVARCHAR (128) NULL
    PRIMARY KEY CLUSTERED ([Id] ASC),
    [Uuid] NVARCHAR(20) NOT NULL, 
    [FileSize] BIGINT NULL, 
	
    [DocumentLibraryId] INT NOT NULL, 
    [BlobUrl] NVARCHAR(2000) NULL, 
    [Modified] DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_Document_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE SET NULL,	
    CONSTRAINT [FK_Document_Parent] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Document] ([Id]) , 
    CONSTRAINT [FK_Document_DocumentLibrary] FOREIGN KEY (DocumentLibraryId) REFERENCES DocumentLibrary(Id)
);

GO

CREATE UNIQUE INDEX [IX_Document_UUID] ON [dbo].[Document] (Uuid)
GO

CREATE UNIQUE INDEX [IX_Document_Name] ON [dbo].[Document] (Name,ParentId, DocumentLibraryId)
