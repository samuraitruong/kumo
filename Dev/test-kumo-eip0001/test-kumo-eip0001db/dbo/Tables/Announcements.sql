CREATE TABLE [dbo].[Announcements]
(
	[Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Title]    NVARCHAR (1000) NOT NULL,
    [Body]      NVARCHAR (MAX) NOT NULL,
    [Created]   DATETIME       NOT NULL,
    [Modified]  DATETIME       NULL,
    [PublishedDate] DATETIME NULL, 
    CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED ([Id] ASC),
)
