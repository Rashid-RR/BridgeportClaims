CREATE TABLE [dbo].[AcctPayable]
(
[PaymentID] [int] NOT NULL IDENTITY(1, 1),
[CheckNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CheckDate] [date] NOT NULL,
[AmountPaid] [money] NOT NULL,
[ClaimID] [int] NOT NULL,
[InvoiceID] [int] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfAcctPayableCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfAcctPayableUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL,
[ETLRowID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[AcctPayable] ADD CONSTRAINT [pkAcctPayable] PRIMARY KEY CLUSTERED  ([PaymentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxAcctPayableClaimIDIncludeAll] ON [dbo].[AcctPayable] ([ClaimID]) INCLUDE ([AmountPaid], [CheckDate], [CheckNumber], [CreatedOnUTC], [InvoiceID], [PaymentID], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AcctPayable] ADD CONSTRAINT [fkAcctPayableClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[AcctPayable] ADD CONSTRAINT [fkAcctPayableInvoiceIDInvoiceInvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [dbo].[Invoice] ([InvoiceID])
GO
