CREATE TABLE [VM].[ProductPrices]
(
	[Id] INT NOT NULL CONSTRAINT [PK_ProductPrices] PRIMARY KEY IDENTITY(1,1), 
	[ProductId] CHAR(4) NOT NULL,
    [Date] DATE NOT NULL, 
    [SellingPrice] MONEY NOT NULL, 
    [PurchasePrice] MONEY NOT NULL, 
	

	 CONSTRAINT [FK_ProductPrices_Products] FOREIGN KEY ([ProductId]) REFERENCES [VM].[Products] ([Id]) 
    
)
