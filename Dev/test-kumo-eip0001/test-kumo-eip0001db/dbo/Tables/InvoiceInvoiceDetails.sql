CREATE TABLE [dbo].[InvoiceInvoiceDetails]
(
	[InvoiceId] INT NOT NULL,
    [InvoiceDetailId] INT NOT NULL,
    CONSTRAINT [PK_InvoiceInvoiceDetails] PRIMARY KEY CLUSTERED ([InvoiceId] ASC, [InvoiceDetailId] ASC),
    CONSTRAINT [FK_InvoiceInvoiceDetails_InvoiceDetail] FOREIGN KEY ([InvoiceDetailId]) REFERENCES [dbo].[InvoiceDetails] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InvoiceInvoiceDetails_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoices] ([Id]) ON DELETE CASCADE
)
