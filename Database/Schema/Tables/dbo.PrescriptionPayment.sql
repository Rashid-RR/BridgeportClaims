CREATE TABLE [dbo].[PrescriptionPayment]
(
[PrescriptionPaymentID] [int] NOT NULL IDENTITY(1, 1),
[CheckNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmountPaid] [money] NOT NULL,
[DatePosted] [date] NOT NULL,
[PrescriptionID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DocumentID] [int] NULL,
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
CREATE NONCLUSTERED INDEX [idxPrescriptionPaymentAmountPaid] ON [dbo].[PrescriptionPayment] ([AmountPaid]) INCLUDE ([DatePosted], [PrescriptionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionPaymentCheckNumber] ON [dbo].[PrescriptionPayment] ([CheckNumber]) INCLUDE ([AmountPaid], [PrescriptionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionPaymentDatePostedIncludeAll] ON [dbo].[PrescriptionPayment] ([DatePosted]) INCLUDE ([AmountPaid], [CheckNumber], [CreatedOnUTC], [DataVersion], [DocumentID], [ModifiedByUserID], [PrescriptionID], [PrescriptionPaymentID], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionPaymentDocumentID] ON [dbo].[PrescriptionPayment] ([DocumentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionPaymentPrescriptionIDUserIDIncludeAll] ON [dbo].[PrescriptionPayment] ([PrescriptionID], [ModifiedByUserID]) INCLUDE ([AmountPaid], [CheckNumber], [CreatedOnUTC], [DatePosted], [DocumentID], [PrescriptionPaymentID], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionPayment] ADD CONSTRAINT [fkPrescriptionPaymentDocumentIDDocumentDocumentID] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[Document] ([DocumentID])
GO
ALTER TABLE [dbo].[PrescriptionPayment] ADD CONSTRAINT [fkPrescriptionPaymentModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[PrescriptionPayment] ADD CONSTRAINT [fkPrescriptionPaymentPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
