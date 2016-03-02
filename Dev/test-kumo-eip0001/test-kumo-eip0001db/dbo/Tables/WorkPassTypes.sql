CREATE TABLE [dbo].[WorkPassTypes]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_WorkPassTypesID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
