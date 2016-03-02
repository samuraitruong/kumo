CREATE TABLE [dbo].[ContactRoles]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ContactRolesID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
