CREATE TABLE [dbo].[InvoiceDetails]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [ProductId] INT NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [UnitPrice] DECIMAL(18, 2) NOT NULL, 
    [Amount] DECIMAL(18, 2) NOT NULL, 
    CONSTRAINT [PK_InvoiceDetailsID] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_InvoiceDetails_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE,
)
