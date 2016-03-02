CREATE TABLE [dbo].[BizExTrackers]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
	[VenderId] INT NOT NULL, 
    [ExpenseName] NVARCHAR(500) NOT NULL, 
    [ExpenseCategoryId] INT NOT NULL, 
    [PaymentDate] DATETIME NOT NULL, 
    [PaymentModeId] INT NOT NULL, 
    [InvoiceNumber] NVARCHAR(50) NOT NULL, 
    [PaidBy] INT NOT NULL, 
    [PaidByCompany] BIT NOT NULL, 
    [ChequeNumber] NVARCHAR(100) NOT NULL, 
    [Amount] DECIMAL(18, 2) NOT NULL, 
    CONSTRAINT [PK_BizExTrackersID] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_BizExTrackers_PaymentMode] FOREIGN KEY ([PaymentModeId]) REFERENCES [dbo].[PaymentModes] ([Id]),
	CONSTRAINT [FK_BizExTrackers_Employee] FOREIGN KEY ([PaidBy]) REFERENCES [dbo].[Employee] ([Id]),
	CONSTRAINT [FK_BizExTrackers_ExpenseCategory] FOREIGN KEY ([ExpenseCategoryId]) REFERENCES [dbo].[ExpenseCategories] ([Id]),
	CONSTRAINT [FK_BizExTrackers_Vender] FOREIGN KEY ([VenderId]) REFERENCES [dbo].[Vendors] ([Id])
)
