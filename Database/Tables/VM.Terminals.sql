CREATE TABLE [VM].[Terminals]
(
	[Id] INT NOT NULL CONSTRAINT [PK_Terminals] PRIMARY KEY IDENTITY(1,1), 
	[Location] NVARCHAR(100) NOT NULL, 
    [BillsCapacity] INT NOT NULL, 
    [CoinsLoadedQty] INT NOT NULL, 
    [ProductsLoadedQty] INT NOT NULL,
  

)
