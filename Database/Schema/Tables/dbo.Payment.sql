CREATE TABLE [dbo].[Payment]
(
[PaymentID] [int] NOT NULL IDENTITY(1, 1),
[CheckNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CheckDate] [date] NOT NULL,
[AmountPaid] [money] NOT NULL,
[ClaimID] [int] NOT NULL,
[InvoiceID] [int] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPaymentCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPaymentUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Payment] ADD CONSTRAINT [pkPayment] PRIMARY KEY CLUSTERED  ([PaymentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPaymentClaimIDIncludeAll] ON [dbo].[Payment] ([ClaimID]) INCLUDE ([AmountPaid], [CheckDate], [CheckNumber], [CreatedOnUTC], [InvoiceID], [PaymentID], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payment] ADD CONSTRAINT [fkPaymentClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Payment] ADD CONSTRAINT [fkPaymentInvoiceIDInvoiceInvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [dbo].[Invoice] ([InvoiceID])
GO
