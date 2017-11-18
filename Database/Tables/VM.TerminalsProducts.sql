CREATE TABLE [VM].[TerminalsProducts]
(
	[ProductId] CHAR(4) NOT NULL , 
    [TerminalId] INT NOT NULL,
	[Quantity] INT NOT NULL,
	 CONSTRAINT [PK_TerminalsProducts] PRIMARY KEY CLUSTERED 
	(
		[ProductId] ASC,
		[TerminalId] ASC
	),
    CONSTRAINT [FK_TerminalsProducts_Products] FOREIGN KEY ([ProductId]) REFERENCES [VM].[Products] ([Id]),
	CONSTRAINT [FK_TerminalsProducts_Terminals] FOREIGN KEY ([TerminalId]) REFERENCES [VM].[Terminals] ([Id])
)

