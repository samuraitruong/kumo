CREATE TABLE [dbo].[Payments]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
	 [PaymentNumber] NVARCHAR(50) NOT NULL, 
    [PaymentDate] DATETIME NOT NULL, 
    [AmountReceived] DECIMAL(18, 2) NOT NULL, 
    [PaymentModeId] INT NOT NULL, 
    [ChequeNumber] NVARCHAR(100) NOT NULL, 
    [BankTypeId] INT NOT NULL, 
    [PaymentStatus] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_PaymentsID] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Payments_PaymentMode] FOREIGN KEY ([PaymentModeId]) REFERENCES [dbo].[PaymentModes] ([Id]),
	CONSTRAINT [FK_Payments_BankType] FOREIGN KEY ([BankTypeId]) REFERENCES [dbo].[BankTypes] ([Id])
)
