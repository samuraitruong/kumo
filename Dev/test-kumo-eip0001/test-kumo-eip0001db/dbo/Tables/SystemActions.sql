﻿CREATE TABLE [dbo].[SystemActions]
(
	[Id] INT NOT NULL, 
    [Name] NVARCHAR(255) NOT NULL,
	CONSTRAINT [PK_SystemActions] PRIMARY KEY CLUSTERED ([Id] ASC)
)
