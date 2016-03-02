CREATE TABLE [dbo].[SalaryTrackers]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
	[EmployeeId] INT NOT NULL, 
    [PaymentDate] DATETIME NOT NULL, 
    [BasicSalary] DECIMAL(18, 2) NOT NULL, 
    [Commission] DECIMAL(18, 2) NOT NULL, 
    [TotalNumberOfHours] FLOAT NOT NULL, 
    [CPFContribution] DECIMAL(18, 2) NOT NULL, 
    [WorkerLevy] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_SalaryTrackersID] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_SalaryTrackers_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id])
)
