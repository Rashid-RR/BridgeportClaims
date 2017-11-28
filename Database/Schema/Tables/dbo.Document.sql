CREATE TABLE [dbo].[Document]
(
[DocumentID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NULL,
[DocumentTypeID] [tinyint] NOT NULL,
[CreatedDateLocal] [datetime2] NOT NULL,
[IsIndexed] AS (CONVERT([bit],case  when [ClaimID] IS NOT NULL then (1) else (0) end,(0))),
[RxDate] [datetime2] NULL,
[RxNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InvoiceNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InjuryDate] [datetime2] NULL,
[AttorneyName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FileName] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FileUrl] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [pkDocument] PRIMARY KEY CLUSTERED  ([DocumentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [fkDocumentClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [fkDocumentDocumentTypeIDDocumentTypeDocumentTypeID] FOREIGN KEY ([DocumentTypeID]) REFERENCES [dbo].[DocumentType] ([DocumentTypeID])
GO
