CREATE TABLE [dbo].[RevenueTrackers]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [CustomerId] INT NOT NULL, 
    [ContactId] INT NOT NULL, 
    [InvoiceId] INT NOT NULL, 
    [PaymentId] INT NOT NULL, 
    CONSTRAINT [PK_RevenueTrackersID] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_RevenueTrackers_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id]),
	CONSTRAINT [FK_RevenueTrackers_Contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([Id]),
	CONSTRAINT [FK_RevenueTrackers_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoices] ([Id]),
	CONSTRAINT [FK_RevenueTrackers_Payment] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[Payments] ([Id]),
)
