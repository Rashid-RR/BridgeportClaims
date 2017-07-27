CREATE TABLE [dbo].[Payment]
(
[PaymentID] [int] NOT NULL IDENTITY(1, 1),
[CheckNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmountPaid] [money] NOT NULL,
[DateScanned] [date] NULL,
[PrescriptionID] [int] NULL,
[ClaimID] [int] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Payment] ADD CONSTRAINT [pkPayment] PRIMARY KEY CLUSTERED  ([PaymentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payment] ADD CONSTRAINT [fkPaymentClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Payment] ADD CONSTRAINT [fkPaymentPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
