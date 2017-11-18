CREATE TABLE [VM].[SoldProducts]
(
	[Id] INT NOT NULL CONSTRAINT [PK_SoldProducts] PRIMARY KEY IDENTITY(1,1),  
    [ProductId] CHAR(4) NOT NULL, 
    [TerminalId] INT NOT NULL,
    [Date] DATE NOT NULL, 

	 CONSTRAINT [FK_SoldProducts_Products] FOREIGN KEY ([ProductId]) REFERENCES [VM].[Products] ([Id]), 
	CONSTRAINT [FK_SoldProducts_Terminals] FOREIGN KEY ([TerminalId]) REFERENCES [VM].[Terminals] ([Id])
)
