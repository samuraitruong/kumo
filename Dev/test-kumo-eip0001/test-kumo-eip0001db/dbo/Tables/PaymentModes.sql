CREATE TABLE [dbo].[PaymentModes]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PaymentModesID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
