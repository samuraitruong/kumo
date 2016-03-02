CREATE TABLE [dbo].[Qualifications]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_QualificationsID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
