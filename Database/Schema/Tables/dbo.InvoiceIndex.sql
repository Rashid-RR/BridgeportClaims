CREATE TABLE [dbo].[InvoiceIndex]
(
[DocumentID] [int] NOT NULL,
[InvoiceNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfInvoiceIndexCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfInvoiceIndexUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[InvoiceIndex] ADD CONSTRAINT [pkInvoiceIndex] PRIMARY KEY CLUSTERED  ([DocumentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxInvoiceIndexInvoiceNumber] ON [dbo].[InvoiceIndex] ([InvoiceNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[InvoiceIndex] ADD CONSTRAINT [fkInvoiceIndexDocumentIDDocumentDocumentID] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[Document] ([DocumentID])
GO
ALTER TABLE [dbo].[InvoiceIndex] ADD CONSTRAINT [fkInvoiceIndexModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
