CREATE TABLE [dbo].[TaskTracker] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TaskName]        NVARCHAR (100) NOT NULL,
    [Priority]    NVARCHAR (50)  NOT NULL,
    [CompletedPercent]    DECIMAL(5, 2)  NOT NULL DEFAULT 0.0,
    [StartDate]   DATETIME       NOT NULL,
    [DueDate]     DATETIME       NOT NULL,
    [CompletedDate]  DATETIME       NULL,
    [AssignedTo]  NVARCHAR (128) NOT NULL,
    [Status]      NVARCHAR (20)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Task_User] FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

