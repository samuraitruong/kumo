CREATE TABLE [dbo].[Vendors]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_VendersID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
