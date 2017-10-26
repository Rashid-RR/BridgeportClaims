CREATE TABLE [dbo].[ClaimPayment]
(
[ClaimPaymentID] [int] NOT NULL IDENTITY(500000000, 1),
[CheckNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmountPaid] [money] NOT NULL,
[DatePosted] [date] NULL,
[ClaimID] [int] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimPaymentCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimPaymentUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ClaimPayment] ADD CONSTRAINT [pkClaimPayment] PRIMARY KEY CLUSTERED  ([ClaimPaymentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPaymentClaimIDIncludeAll] ON [dbo].[ClaimPayment] ([ClaimID]) INCLUDE ([AmountPaid], [CheckNumber], [ClaimPaymentID], [CreatedOnUTC], [DatePosted], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClaimPayment] ADD CONSTRAINT [fkClaimPaymentClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
