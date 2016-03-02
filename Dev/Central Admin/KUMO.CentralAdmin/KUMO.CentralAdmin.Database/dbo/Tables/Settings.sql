CREATE TABLE [dbo].[Settings]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Key] NVARCHAR(50) NOT NULL, 
    [StringValue] NVARCHAR(50) NULL, 
    [NumberValue] INT NULL, 
    [Category] NVARCHAR(250) NOT NULL DEFAULT 'Web Setting', 
    [Type] NVARCHAR(50) NOT NULL DEFAULT 'String', 
    [Description] NVARCHAR(500) NULL
)
