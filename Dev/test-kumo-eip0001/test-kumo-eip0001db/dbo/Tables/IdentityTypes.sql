CREATE TABLE [dbo].[IdentityTypes]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_IdentityTypesID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
