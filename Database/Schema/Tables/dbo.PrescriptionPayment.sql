CREATE TABLE [dbo].[PrescriptionPayment]
(
[PrescriptionPaymentID] [int] NOT NULL IDENTITY(1, 1),
[CheckNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmountPaid] [money] NOT NULL,
[DatePosted] [date] NULL,
[PrescriptionID] [int] NOT NULL,
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionPaymentCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionPaymentUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[PrescriptionPayment] ADD CONSTRAINT [pkPrescriptionPaymentID] PRIMARY KEY CLUSTERED  ([PrescriptionPaymentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionPayment] ADD CONSTRAINT [fkPrescriptionPaymentPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
ALTER TABLE [dbo].[PrescriptionPayment] ADD CONSTRAINT [fkPrescriptionPaymentUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
