CREATE TABLE [dbo].[Account]
(
	[Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UserName]    NVARCHAR (100) NOT NULL,
    [Name]      NVARCHAR (255) NOT NULL,
    [Created]   DATETIME       NOT NULL,
    [Modified]  DATETIME       NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
)
