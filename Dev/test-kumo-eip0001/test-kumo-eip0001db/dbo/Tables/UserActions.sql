CREATE TABLE [dbo].[UserActions]
(
	[Id] INT NOT NULL IDENTITY(1, 1), 
    [UserId] NVARCHAR(128) NOT NULL, 
    [ActionId] INT NOT NULL, 
    [ComponentId] INT NOT NULL,
	CONSTRAINT [PK_UserActions] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UserActions_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_UserActions_Action] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[SystemActions] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_UserActions_Component] FOREIGN KEY ([ComponentId]) REFERENCES [dbo].[Components] ([Id]) ON DELETE CASCADE,		
)
