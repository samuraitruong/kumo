CREATE TABLE [dbo].[Invoices]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [InvoiceNumber] NVARCHAR(50) NOT NULL, 
    [InvoiceDate] DATETIME NOT NULL, 
    [InvoiceDueDate] DATETIME NOT NULL, 
    [InvoiceAmount] DECIMAL(18, 2) NOT NULL, 
    [Memo] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_InvoicesID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
