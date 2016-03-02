CREATE TABLE [dbo].[ExpenseCategories]
(
	[Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ExpenseCategoriesID] PRIMARY KEY CLUSTERED ([Id] ASC)
)
