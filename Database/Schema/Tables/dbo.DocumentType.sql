CREATE TABLE [dbo].[DocumentType]
(
[DocumentTypeID] [tinyint] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatesEpisode] [bit] NOT NULL CONSTRAINT [dfDocumentTypeCreatesEpisode] DEFAULT ((0)),
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentTypeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentTypeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DocumentType] ADD CONSTRAINT [pkDocumentType] PRIMARY KEY CLUSTERED  ([DocumentTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DocumentType] ADD CONSTRAINT [idxUqDocumentTypeCode] UNIQUE NONCLUSTERED  ([Code]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
GRANT DELETE ON  [dbo].[DocumentType] TO [acondie]
GO
GRANT INSERT ON  [dbo].[DocumentType] TO [acondie]
GO
GRANT SELECT ON  [dbo].[DocumentType] TO [acondie]
GO
GRANT UPDATE ON  [dbo].[DocumentType] TO [acondie]
GO
