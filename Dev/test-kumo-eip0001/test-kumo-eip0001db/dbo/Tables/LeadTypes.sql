CREATE TABLE [dbo].[LeadTypes]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_LeadTypesID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
