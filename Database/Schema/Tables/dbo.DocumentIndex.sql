CREATE TABLE [dbo].[DocumentIndex]
(
[DocumentID] [int] NOT NULL,
[ClaimID] [int] NOT NULL,
[DocumentTypeID] [tinyint] NOT NULL,
[RxDate] [date] NULL,
[RxNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InjuryDate] [date] NULL,
[AttorneyName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentIndexCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentIndexUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DocumentIndex] ADD CONSTRAINT [pkDocumentIndex] PRIMARY KEY CLUSTERED  ([DocumentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DocumentIndex] ADD CONSTRAINT [fkDocumentIndexClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[DocumentIndex] ADD CONSTRAINT [fkDocumentIndexDocumentIDDocumentDocumentID] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[Document] ([DocumentID]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentIndex] ADD CONSTRAINT [fkDocumentIndexDocumentTypeIDDocumentTypeDocumentTypeID] FOREIGN KEY ([DocumentTypeID]) REFERENCES [dbo].[DocumentType] ([DocumentTypeID])
GO
