CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId]    NVARCHAR (150)  NOT NULL,
    [ContextKey]     NVARCHAR (300)  NOT NULL,
    [Model]          VARBINARY (MAX) NOT NULL,
    [ProductVersion] NVARCHAR (32)   NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC, [ContextKey] ASC)
);
CREATE TABLE [dbo].[Account] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (100) NOT NULL,
    [Name]     NVARCHAR (255) NOT NULL,
    [Created]  DATETIME       NOT NULL,
    [Modified] DATETIME       NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);

CREATE TABLE [dbo].[Announcements] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Title]         NVARCHAR (1000) NOT NULL,
    [Body]          NVARCHAR (MAX)  NOT NULL,
    [Created]       DATETIME        NOT NULL,
    [Modified]      DATETIME        NULL,
    [PublishedDate] DATETIME        NULL,
    CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRole] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);


CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC)
);


CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);


CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
);

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);


CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);


CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    [Company]              NVARCHAR (150) NULL,
    [Firstname]            NVARCHAR (50)  NULL,
    [Lastname]             NVARCHAR (50)  NULL,
    [Status]               NVARCHAR (50)  NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);


CREATE TABLE [dbo].[Company] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (250) NOT NULL,
    [DatabaseName]     NVARCHAR (50)  NOT NULL,
    [DatabaseServer]   NVARCHAR (150) NOT NULL,
    [DatabasePassword] NVARCHAR (150) NOT NULL,
    [Status]           NVARCHAR (50)  NOT NULL,
    [DatabaseUser]     NVARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Customer]
(
	[Id] INT NOT NULL IDENTITY (1, 1), 
    [Website] NVARCHAR(100) NOT NULL, 
    [FaxNumber] NVARCHAR(100) NOT NULL, 
    [CompanyAddress] NVARCHAR(255) NOT NULL, 
    [Phone] NVARCHAR(50) NULL, 
    [AssignedTo] INT NOT NULL, 
    [CustomerId] NVARCHAR(50) NOT NULL, 
    [CompanyName] NVARCHAR(255) NOT NULL, 
    [IndustryId] INT NOT NULL, 
    [CompanyEmailAddress] NVARCHAR(255) NOT NULL, 
    [Source] NVARCHAR(50) NOT NULL, 
    [LeadType] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
)


CREATE TABLE [dbo].[Department] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DepartmentID] PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Employee] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Firstname]     NVARCHAR (100) NOT NULL,
    [Lastname]      NVARCHAR (100) NOT NULL,
    [Address]       NVARCHAR (255) NULL,
    [Phone]         NVARCHAR (50)  NULL,
    [EmployeeId]    NVARCHAR (50)  NOT NULL,
    [Fullname]      NVARCHAR (100) NOT NULL,
    [DateOfBirth]   DATETIME       NOT NULL,
    [JobTitle]      NVARCHAR (100) NOT NULL,
    [DepartmentID]  INT            NOT NULL,
    [LineManager]   INT            NULL,
    [Status]        NVARCHAR (20)  NOT NULL,
    [EffectiveDate] DATETIME       NOT NULL,
    [DirectPhone]   NVARCHAR (50)  NOT NULL,
    [Email]         NVARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

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
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[TaskTracker] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [TaskName]         NVARCHAR (100) NOT NULL,
    [Priority]         NVARCHAR (50)  NOT NULL,
    [CompletedPercent] DECIMAL (5, 2) NOT NULL,
    [StartDate]        DATETIME       NOT NULL,
    [DueDate]          DATETIME       NOT NULL,
    [CompletedDate]    DATETIME       NULL,
    [AssignedTo]       NVARCHAR (128) NOT NULL,
    [Status]           NVARCHAR (20)  NOT NULL,
    [Description]      NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Components] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Document]
(
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
	[Extension] nvarchar(10),	
	[ItemType] INT NOT NULL,
	[ParentId] INT,
    [Created]  DATETIME       NULL,
    [CreatedBy]  NVARCHAR (128) NULL,
    [Uuid] NVARCHAR(20) NULL, 
    [FileSize] BIGINT NULL, 
    [DocumentLibraryId] INT NOT NULL, 
    [BlobUrl] NVARCHAR(2000) NOT NULL, 
	PRIMARY KEY CLUSTERED ([Id] ASC),
);

CREATE TABLE [dbo].[DocumentLibrary]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [Name] NVARCHAR(255) NOT NULL, 
    [Description] NVARCHAR(500) NULL,
	BlobContainer nvarchar(250) NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [dbo].[Calendar](
	[Id] INT IDENTITY(1,1) NOT NULL,
	[JobScope] NVARCHAR(255) NOT NULL,
	[Priority] NVARCHAR(50) NOT NULL,
	[StartDate] DATETIME NOT NULL,
	[DueDate] DATETIME NOT NULL,
	[CompletedDate] DATETIME NULL,
	[AssignedTo] NVARCHAR(128) NOT NULL,
	[CompletedPercent] DECIMAL(5, 2) NULL,
	[Status] NVARCHAR(50) NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [dbo].[Industry]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Name] NVARCHAR (500) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [dbo].[SystemActions]
(
	[Id] INT NOT NULL, 
    [Name] NVARCHAR(255) NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [dbo].[UserActions]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [ActionId] INT NOT NULL, 
    [ComponentId] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC)
)



ALTER TABLE [dbo].[Calendar] 
	ADD  DEFAULT ((0.0)) FOR [CompletedPercent]

ALTER TABLE [dbo].[Customer]
    ADD DEFAULT getDate() FOR [CustomerId];

ALTER TABLE [dbo].[Employee]
    ADD DEFAULT (getdate()) FOR [EmployeeId];


ALTER TABLE [dbo].[TaskTracker]
    ADD DEFAULT 0.0 FOR [CompletedPercent];

ALTER TABLE [dbo].[AspNetUserClaims] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

ALTER TABLE [dbo].[AspNetUserLogins] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

ALTER TABLE [dbo].[AspNetUserRoles] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;

ALTER TABLE [dbo].[AspNetUserRoles] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

ALTER TABLE [dbo].[Customer] WITH NOCHECK
    ADD CONSTRAINT [FK_Customer_ToTable] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id]);

ALTER TABLE [dbo].[Employee] WITH NOCHECK
    ADD CONSTRAINT [FK_Employee_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([Id]);

ALTER TABLE [dbo].[Employee] WITH NOCHECK
    ADD CONSTRAINT [FK_Employee_Manager] FOREIGN KEY ([LineManager]) REFERENCES [dbo].[Employee] ([Id]);

ALTER TABLE [dbo].[IssueTracker] WITH NOCHECK
    ADD CONSTRAINT [FK_Issue_User] FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE [dbo].[TaskTracker] WITH NOCHECK
    ADD CONSTRAINT [FK_Task_User] FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_DocumentLibrary] FOREIGN KEY([DocumentLibraryId])
REFERENCES [dbo].[DocumentLibrary] ([Id])

ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_Parent] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Document] ([Id])

ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_User] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE SET NULL

ALTER TABLE [dbo].[Calendar]  WITH CHECK ADD  CONSTRAINT [FK_Calendar_AspNetUser] FOREIGN KEY([AssignedTo])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([Id])

ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_ToTable] FOREIGN KEY([AssignedTo])
REFERENCES [dbo].[Employee] ([Id])

ALTER TABLE [dbo].[UserActions]  WITH CHECK ADD  CONSTRAINT [FK_UserActions_Action] FOREIGN KEY([ActionId])
REFERENCES [dbo].[SystemActions] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[UserActions]  WITH CHECK ADD  CONSTRAINT [FK_UserActions_Component] FOREIGN KEY([ComponentId])
REFERENCES [dbo].[Components] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[UserActions]  WITH CHECK ADD  CONSTRAINT [FK_UserActions_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END


IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '70961cbd-fc35-48cc-bd36-a225567f8024')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('70961cbd-fc35-48cc-bd36-a225567f8024')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '46618021-5521-44f2-97e3-566162a37af9')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('46618021-5521-44f2-97e3-566162a37af9')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'afc33621-003a-4d37-926c-540e22105e83')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('afc33621-003a-4d37-926c-540e22105e83')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'a69d30ff-806a-4b8c-b76e-9e4787450ace')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('a69d30ff-806a-4b8c-b76e-9e4787450ace')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '2fd3a674-2745-4c2c-9258-8e01d3e8eddc')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('2fd3a674-2745-4c2c-9258-8e01d3e8eddc')

ALTER TABLE [dbo].[AspNetUserClaims] WITH CHECK CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId];

ALTER TABLE [dbo].[AspNetUserLogins] WITH CHECK CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUser_UserId];

ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId];

ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId];

ALTER TABLE [dbo].[Customer] WITH CHECK CHECK CONSTRAINT [FK_Customer_ToTable];

ALTER TABLE [dbo].[Employee] WITH CHECK CHECK CONSTRAINT [FK_Employee_Department];

ALTER TABLE [dbo].[Employee] WITH CHECK CHECK CONSTRAINT [FK_Employee_Manager];

ALTER TABLE [dbo].[IssueTracker] WITH CHECK CHECK CONSTRAINT [FK_Issue_User];

ALTER TABLE [dbo].[TaskTracker] WITH CHECK CHECK CONSTRAINT [FK_Task_User];

ALTER TABLE [dbo].[ReportDocument] CHECK CONSTRAINT [FK_ReportDocument_User]

ALTER TABLE [dbo].[ReportDocument] CHECK CONSTRAINT [FK_ReportDocument_Parent]

ALTER TABLE [dbo].[Calendar] CHECK CONSTRAINT [FK_Calendar_AspNetUser]

ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Industry]

ALTER TABLE [dbo].[UserActions] CHECK CONSTRAINT [FK_UserActions_Action]

ALTER TABLE [dbo].[UserActions] CHECK CONSTRAINT [FK_UserActions_Component]

ALTER TABLE [dbo].[UserActions] CHECK CONSTRAINT [FK_UserActions_User]

ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_DocumentLibrary]

ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_Parent]

ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_User]