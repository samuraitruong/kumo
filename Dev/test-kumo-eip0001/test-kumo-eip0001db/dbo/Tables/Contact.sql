CREATE TABLE [dbo].[Contact]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomerId] INT NOT NULL, 
    [RoleId] INT NOT NULL, 
    [FirstName] NVARCHAR(255) NOT NULL, 
    [LastName] NVARCHAR(255) NOT NULL, 
    [FullName] NVARCHAR(500) NOT NULL, 
    [JobTitle] NVARCHAR(500) NOT NULL, 
    [EmailAddress] NVARCHAR(255) NOT NULL, 
    [MobileNumber] NVARCHAR(50) NOT NULL, 
    [OfficeNumber] NVARCHAR(50) NOT NULL, 
    [FaxNumber] NVARCHAR(50) NOT NULL, 
	CONSTRAINT [FK_Contact_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]),
	CONSTRAINT [FK_Contact_Role] FOREIGN KEY ([RoleId]) REFERENCES [ContactRoles]([Id]),
)
