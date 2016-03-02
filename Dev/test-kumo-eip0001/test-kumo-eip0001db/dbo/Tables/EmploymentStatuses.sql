CREATE TABLE [dbo].[EmploymentStatuses]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EmploymentStatusID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
