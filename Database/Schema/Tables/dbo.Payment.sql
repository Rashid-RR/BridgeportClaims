CREATE TABLE [dbo].[Payment]
(
[PaymentID] [int] NOT NULL IDENTITY(1, 1),
[CheckNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CheckDate] [datetime2] NOT NULL,
[AmountPaid] [money] NOT NULL,
[InvoiceNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ClaimID] [int] NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPaymentCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPaymentUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = PAGE
)
GO
ALTER TABLE [dbo].[Payment] ADD CONSTRAINT [pkPayment] PRIMARY KEY CLUSTERED  ([PaymentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPaymentClaimIDIncludeAll] ON [dbo].[Payment] ([ClaimID]) INCLUDE ([AmountPaid], [CheckDate], [CheckNumber], [CreatedOn], [DataVersion], [InvoiceNumber], [PaymentID], [UpdatedOn]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payment] ADD CONSTRAINT [fkPaymentClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
