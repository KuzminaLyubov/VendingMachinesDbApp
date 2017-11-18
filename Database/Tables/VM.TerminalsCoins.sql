CREATE TABLE [VM].[TerminalsCoins]
(
	[CoinId] int NOT NULL , 
    [TerminalId] INT NOT NULL,
	[ReturnedQuantity] INT NOT NULL,
	[LoadedQuantity] INT NOT NULL, 
    CONSTRAINT [PK_TerminalsCoins] PRIMARY KEY CLUSTERED 
	(
		[CoinId] ASC,
		[TerminalId] ASC
	),
    CONSTRAINT [FK_TerminalsCoins_Coins] FOREIGN KEY ([CoinId]) REFERENCES [VM].[Coins] ([Id]),
	CONSTRAINT [FK_TerminalsCoins_Terminals] FOREIGN KEY ([TerminalId]) REFERENCES [VM].[Terminals] ([Id])
)

