
GO
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId]    NVARCHAR (150)  NOT NULL,
    [ContextKey]     NVARCHAR (300)  NOT NULL,
    [Model]          VARBINARY (MAX) NOT NULL,
    [ProductVersion] NVARCHAR (32)   NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC, [ContextKey] ASC)
);


GO
PRINT N'Creating [dbo].[Account]...';


GO
CREATE TABLE [dbo].[Account] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (100) NOT NULL,
    [Name]     NVARCHAR (255) NOT NULL,
    [Created]  DATETIME       NOT NULL,
    [Modified] DATETIME       NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);


GO
PRINT N'Creating [dbo].[Announcements]...';


GO
CREATE TABLE [dbo].[Announcements] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Title]         NVARCHAR (1000) NOT NULL,
    [Body]          NVARCHAR (MAX)  NOT NULL,
    [Created]       DATETIME        NOT NULL,
    [Modified]      DATETIME        NULL,
    [PublishedDate] DATETIME        NULL,
    CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[AspNetRoles]...';


GO
CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRole] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[AspNetRoles].[RoleNameIndex]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);


GO
PRINT N'Creating [dbo].[AspNetUserClaims]...';


GO
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[AspNetUserClaims].[IX_UserId]...';


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);


GO
PRINT N'Creating [dbo].[AspNetUserLogins]...';


GO
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC)
);


GO
PRINT N'Creating [dbo].[AspNetUserLogins].[IX_UserId]...';


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);


GO
PRINT N'Creating [dbo].[AspNetUserRoles]...';


GO
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
);


GO
PRINT N'Creating [dbo].[AspNetUserRoles].[IX_UserId]...';


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);


GO
PRINT N'Creating [dbo].[AspNetUserRoles].[IX_RoleId]...';


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);


GO
PRINT N'Creating [dbo].[AspNetUsers]...';


GO
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


GO
PRINT N'Creating [dbo].[AspNetUsers].[UserNameIndex]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);


GO
PRINT N'Creating [dbo].[Company]...';


GO
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


GO
PRINT N'Creating [dbo].[Customer]...';


GO
CREATE TABLE [dbo].[Customer] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Firstname]  NVARCHAR (100) NOT NULL,
    [Lastname]   NVARCHAR (100) NOT NULL,
    [Address]    NVARCHAR (255) NULL,
    [Phone]      NVARCHAR (50)  NULL,
    [EmployeeId] INT            NOT NULL,
    [CustomerId] NVARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Department]...';


GO
CREATE TABLE [dbo].[Department] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DepartmentID] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Employee]...';


GO
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


GO
PRINT N'Creating [dbo].[IssueTracker]...';


GO
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


GO
PRINT N'Creating [dbo].[TaskTracker]...';


GO
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


GO
PRINT N'Creating unnamed constraint on [dbo].[Customer]...';


GO
ALTER TABLE [dbo].[Customer]
    ADD DEFAULT getDate() FOR [CustomerId];


GO
PRINT N'Creating unnamed constraint on [dbo].[Employee]...';


GO
ALTER TABLE [dbo].[Employee]
    ADD DEFAULT (getdate()) FOR [EmployeeId];


GO
PRINT N'Creating unnamed constraint on [dbo].[TaskTracker]...';


GO
ALTER TABLE [dbo].[TaskTracker]
    ADD DEFAULT 0.0 FOR [CompletedPercent];


GO
PRINT N'Creating [dbo].[FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]...';


GO
ALTER TABLE [dbo].[AspNetUserClaims] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.AspNetUserLogins_dbo.AspNetUser_UserId]...';


GO
ALTER TABLE [dbo].[AspNetUserLogins] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]...';


GO
ALTER TABLE [dbo].[AspNetUserRoles] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]...';


GO
ALTER TABLE [dbo].[AspNetUserRoles] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Customer_ToTable]...';


GO
ALTER TABLE [dbo].[Customer] WITH NOCHECK
    ADD CONSTRAINT [FK_Customer_ToTable] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Employee_Department]...';


GO
ALTER TABLE [dbo].[Employee] WITH NOCHECK
    ADD CONSTRAINT [FK_Employee_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Employee_Manager]...';


GO
ALTER TABLE [dbo].[Employee] WITH NOCHECK
    ADD CONSTRAINT [FK_Employee_Manager] FOREIGN KEY ([LineManager]) REFERENCES [dbo].[Employee] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Issue_User]...';


GO
ALTER TABLE [dbo].[IssueTracker] WITH NOCHECK
    ADD CONSTRAINT [FK_Issue_User] FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Task_User]...';


GO
ALTER TABLE [dbo].[TaskTracker] WITH NOCHECK
    ADD CONSTRAINT [FK_Task_User] FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
-- Refactoring step to update target server with deployed transaction logs

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
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

GO

GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/*:r .\Roles.sql	
GO*/		
GO

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[AspNetUserClaims] WITH CHECK CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId];

ALTER TABLE [dbo].[AspNetUserLogins] WITH CHECK CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUser_UserId];

ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId];

ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId];

ALTER TABLE [dbo].[Customer] WITH CHECK CHECK CONSTRAINT [FK_Customer_ToTable];

ALTER TABLE [dbo].[Employee] WITH CHECK CHECK CONSTRAINT [FK_Employee_Department];

ALTER TABLE [dbo].[Employee] WITH CHECK CHECK CONSTRAINT [FK_Employee_Manager];

ALTER TABLE [dbo].[IssueTracker] WITH CHECK CHECK CONSTRAINT [FK_Issue_User];

ALTER TABLE [dbo].[TaskTracker] WITH CHECK CHECK CONSTRAINT [FK_Task_User];


GO