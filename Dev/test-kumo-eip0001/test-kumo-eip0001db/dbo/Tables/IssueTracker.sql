CREATE TABLE [dbo].[IssueTracker] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Priority]    NVARCHAR (50)  NOT NULL,
    [Category]    NVARCHAR (50)  NOT NULL,
    [StartDate]   DATETIME       NOT NULL,
    [DueDate]     DATETIME       NOT NULL,
    [ClosedDate]  DATETIME       NULL,
    [AssignedTo]  NVARCHAR (128) NOT NULL,
    [Status]      NVARCHAR (20)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Resolution]  NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Issue_User] FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

