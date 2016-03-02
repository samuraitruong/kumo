CREATE TABLE [dbo].[Calendar]
(
	[Id]          INT            IDENTITY (1, 1) NOT NULL, 
    [JobScope] NVARCHAR(255) NOT NULL, 
    [Priority] NVARCHAR(50) NOT NULL, 
    [StartDate] DATETIME NOT NULL, 
    [DueDate] DATETIME NOT NULL, 
    [CompletedDate] DATETIME NULL, 
    [AssignedTo] NVARCHAR(128) NOT NULL, 
    [CompletedPercent] DECIMAL(5, 2) NULL DEFAULT 0.0, 
    [Status] NVARCHAR(50) NOT NULL,	
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Calendar_AspNetUser] FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[AspNetUsers] ([Id])  ON DELETE CASCADE ON UPDATE CASCADE
)
