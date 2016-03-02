CREATE TABLE [dbo].[Products]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    [UnitPrice] DECIMAL(18, 2) NOT NULL, 
    CONSTRAINT [PK_ProductsID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
