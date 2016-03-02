CREATE TABLE [dbo].[Customer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Website] NVARCHAR(100) NOT NULL, 
    [FaxNumber] NVARCHAR(100) NOT NULL, 
    [CompanyAddress] NVARCHAR(255) NOT NULL, 
    [Phone] NVARCHAR(50) NULL, 
    [AssignedTo] INT NOT NULL, 
    [CustomerId] NVARCHAR(50) NOT NULL DEFAULT getDate(), 
    [CompanyName] NVARCHAR(255) NOT NULL, 
    [IndustryId] INT NOT NULL, 
    [CompanyEmailAddress] NVARCHAR(255) NOT NULL, 
    [SourceTypeId] INT NOT NULL, 
    [LeadTypeId] INT NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Customer_ToTable] FOREIGN KEY ([AssignedTo]) REFERENCES [Employee]([Id]),
	CONSTRAINT [FK_Customer_Industry] FOREIGN KEY ([IndustryId]) REFERENCES [Industry]([Id]),
	CONSTRAINT [FK_Customer_SourceType] FOREIGN KEY ([SourceTypeId]) REFERENCES [SourceTypes]([Id]),
	CONSTRAINT [FK_Customer_LeadType] FOREIGN KEY ([LeadTypeId]) REFERENCES [LeadTypes]([Id])
)