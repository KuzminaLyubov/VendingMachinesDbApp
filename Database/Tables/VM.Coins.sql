﻿CREATE TABLE [VM].[Coins]
(
	[Id] INT NOT NULL CONSTRAINT [PK_Coins] PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL, 
	[Denomination] money NOT NULL
)
