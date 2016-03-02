CREATE TABLE [dbo].[PaymentFrequencies]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PaymentFrequenciesID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
