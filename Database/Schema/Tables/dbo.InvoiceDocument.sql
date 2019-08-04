CREATE TABLE [dbo].[InvoiceDocument]
(
[InvoiceNumber] [bigint] NOT NULL,
[PrescriptionID] [int] NOT NULL,
[ClaimID] [int] NOT NULL,
[GeneratedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InvoiceDate] [date] NOT NULL CONSTRAINT [dfInvoiceDocumentInvoiceDate] DEFAULT (CONVERT([date],[dtme].[udfGetLocalDate]())),
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfInvoiceDocumentCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfInvoiceDocumentUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[InvoiceDocument] ADD CONSTRAINT [ckInvoiceDocumentLimitPrescriptionsToSix] CHECK (([dbo].[udfIsInvoiceDocumentWithinPrescriptionLimit]()=(1)))
GO
ALTER TABLE [dbo].[InvoiceDocument] ADD CONSTRAINT [pkInvoiceDocument] PRIMARY KEY CLUSTERED  ([InvoiceNumber], [PrescriptionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxInvoiceDocumentGeneratedByUserID] ON [dbo].[InvoiceDocument] ([GeneratedByUserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxInvoiceDocumentPrescriptionIDClaimIDIncludes] ON [dbo].[InvoiceDocument] ([PrescriptionID], [ClaimID]) INCLUDE ([GeneratedByUserID], [InvoiceDate], [InvoiceNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[InvoiceDocument] ADD CONSTRAINT [fkInvoiceDocumentClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[InvoiceDocument] ADD CONSTRAINT [fkInvoiceDocumentGeneratedByUserIDAspNetUsersID] FOREIGN KEY ([GeneratedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[InvoiceDocument] ADD CONSTRAINT [fkInvoiceDocumentPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
