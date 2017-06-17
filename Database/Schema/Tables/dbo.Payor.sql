CREATE TABLE [dbo].[Payor]
(
[PayorID] [int] NOT NULL IDENTITY(1, 1),
[BillToName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BillToAddress1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BillToAddress2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BillToCity] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BillToStateID] [int] NULL,
[BillToPostalCode] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AlternatePhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FaxNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Notes] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Contact] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPayorCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPayorUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Payor] ADD CONSTRAINT [pkPayor] PRIMARY KEY CLUSTERED  ([PayorID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPayorBillToStateID] ON [dbo].[Payor] ([BillToStateID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payor] ADD CONSTRAINT [fkPayorBillToStateIDUsStateStateID] FOREIGN KEY ([BillToStateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
