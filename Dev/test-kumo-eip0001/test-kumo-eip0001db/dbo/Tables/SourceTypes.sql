﻿CREATE TABLE [dbo].[SourceTypes]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SourceTypesID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
