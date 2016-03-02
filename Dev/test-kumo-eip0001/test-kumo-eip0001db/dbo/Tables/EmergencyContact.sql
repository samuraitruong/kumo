CREATE TABLE [dbo].[EmergencyContact]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[ProfilePictureUrl] nvarchar(300) null,
	[EmployeeId] INT NOT NULL, 
	[Firstname]     NVARCHAR (100) NOT NULL,
    [Lastname]      NVARCHAR (100) NOT NULL,
	[Fullname]      NVARCHAR (100) NOT NULL,
	[Email]         NVARCHAR (50)  NOT NULL,
	[Address]       NVARCHAR (255) NULL,
	[MobileNumber] NVARCHAR(50) NOT NULL, 
    [WorkNumber] NVARCHAR(50) NOT NULL, 
    [RelationshipId] INT NOT NULL,

    CONSTRAINT [FK_EmergencyContact_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee](Id), 
    CONSTRAINT [FK_EmergencyContact_Relationship] FOREIGN KEY (RelationshipId) REFERENCES [Relationships]([Id]),

)
