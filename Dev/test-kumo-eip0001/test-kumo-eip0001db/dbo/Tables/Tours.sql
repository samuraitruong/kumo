CREATE TABLE [dbo].[Tours]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    [StartDate] DATETIME NOT NULL, 
    [EndDate] DATETIME NOT NULL, 
    [Location] NVARCHAR(500) NOT NULL, 
    [AssignTo] INT NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_ToursID] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Tours_Employee] FOREIGN KEY ([AssignTo]) REFERENCES [Employee]([Id])
)
