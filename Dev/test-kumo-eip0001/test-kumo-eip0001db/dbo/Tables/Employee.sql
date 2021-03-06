﻿CREATE TABLE [dbo].[Employee] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Firstname]     NVARCHAR (100) NOT NULL,
    [Lastname]      NVARCHAR (100) NOT NULL,
	[Fullname]      NVARCHAR (100) NOT NULL,
	[Gender]		NVARCHAR(200) NOT NULL,
	[DateOfBirth]   DATETIME       NOT NULL,
	[IdentificationNumber] NVARCHAR(50) NOT NULL,
	[IdentityType] INT NOT NULL,

    [MobileNumber] NVARCHAR(50) NOT NULL, 
    [WorkNumber] NVARCHAR(50) NOT NULL, 
    [HomeNumber] NVARCHAR(50) NOT NULL, 
    [Email]         NVARCHAR (50)  NOT NULL,
	[Address]       NVARCHAR (255) NULL,

    [EmployeeId]    NVARCHAR (50)  DEFAULT (getdate()) NOT NULL,
	
    [JobTitleId]    INT				NOT NULL,
    [DepartmentID]  INT            NOT NULL,
    [LineManagerId]   INT            NULL,
	[EmploymentStatusId]        INT NOT NULL,
	[Current]           BIT NOT NULL DEFAULT(0),
    [EffectiveDate] DATETIME       NOT NULL,
	[EndDate] DATETIME        NULL,
	[TerminaiondDate] DATETIME NULL,
	[WorkPassTypeId] INT NOT NULL,
    [WorkPassExpiryDate] DATETIME NULL,
	

    [NationalityId] INT NULL, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [Salary] MONEY NOT NULL, 
    [PaymentFrequencyId] INT NOT NULL, 
    [PaymentModeId] INT NOT NULL, 
    [BankId] INT NOT NULL, 
    [AccountName] NVARCHAR(250) NOT NULL, 
    [MedicalIsuranceNumber] NVARCHAR(250) NOT NULL, 
    [HighestQualificationId] INT NOT NULL, 
	[EmergencyContactFirstName] NVARCHAR(255) NOT NULL,
	[EmergencyContactLastName] NVARCHAR(255) NOT NULL,
	[EmergencyContactFullName] NVARCHAR(500) NOT NULL,
	[EmergencyContactMobileNumber] NVARCHAR(50) NOT NULL,
	[EmergencyContactWorkNumber] NVARCHAR(100) NOT NULL,
	[EmergencyContactEmailAddress] NVARCHAR(255) NOT NULL,
	[EmergencyContactRelationshipId] INT NOT NULL,

    [Skills] NTEXT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employee_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([Id]),
    CONSTRAINT [FK_Employee_Manager] FOREIGN KEY ([LineManagerId]) REFERENCES [dbo].[Employee] ([Id]),
	CONSTRAINT [FK_Employee_Country] FOREIGN KEY ([NationalityId]) REFERENCES [dbo].[Nationalities] ([Id]),
	CONSTRAINT [FK_Employee_AspNetUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]), 
    CONSTRAINT [FK_Employee_IdentificationType] FOREIGN KEY (IdentityType) REFERENCES [IdentityTypes](Id), 
    CONSTRAINT [FK_Employee_JobTitle] FOREIGN KEY (JobTitleId) REFERENCES [JobTitles](Id),
	CONSTRAINT [FK_Employee_EmploymentStatus] FOREIGN KEY (EmploymentStatusId) REFERENCES [EmploymentStatuses](Id),
	CONSTRAINT [FK_Employee_Nationality] FOREIGN KEY (NationalityId) REFERENCES Nationalities(Id),
	CONSTRAINT [FK_Employee_Bank] FOREIGN KEY (BankId) REFERENCES [Banks](Id),
    CONSTRAINT [FK_Employee_PaymentFrequency] FOREIGN KEY (PaymentFrequencyId) REFERENCES [PaymentFrequencies](Id),
	CONSTRAINT [FK_Employee_PaymentMode] FOREIGN KEY ([PaymentModeId]) REFERENCES [PaymentModes](Id),
	CONSTRAINT [FK_Employee_WorkPassType] FOREIGN KEY (WorkPassTypeId) REFERENCES [WorkPassTypes](Id), 
    CONSTRAINT [FK_Employee_User] FOREIGN KEY (UserId) REFERENCES AspNetUsers([Id]),
	CONSTRAINT [FK_Employee_Qualification] FOREIGN KEY (HighestQualificationId) REFERENCES Qualifications([Id]),
	CONSTRAINT [FK_Employee_Emergency_Contact_Relationship] FOREIGN KEY ([EmergencyContactRelationshipId]) REFERENCES Relationships([Id])
);


