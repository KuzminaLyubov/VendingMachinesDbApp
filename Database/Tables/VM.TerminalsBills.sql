CREATE TABLE [VM].[TerminalsBills]
(
	[BillId] int NOT NULL , 
    [TerminalId] INT NOT NULL,
	[InsertedQuantity] INT NOT NULL,
	 CONSTRAINT [PK_TerminalsBills] PRIMARY KEY CLUSTERED 
	(
		[BillId] ASC,
		[TerminalId] ASC
	),
    CONSTRAINT [FK_TerminalsBills_Bills] FOREIGN KEY ([BillId]) REFERENCES [VM].[Bills] ([Id]),
	CONSTRAINT [FK_TerminalsBills_Terminals] FOREIGN KEY ([TerminalId]) REFERENCES [VM].[Terminals] ([Id])
)

