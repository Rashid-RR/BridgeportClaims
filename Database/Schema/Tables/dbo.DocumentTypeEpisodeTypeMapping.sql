CREATE TABLE [dbo].[DocumentTypeEpisodeTypeMapping]
(
[DocumentTypeID] [tinyint] NOT NULL,
[EpisodeTypeID] [tinyint] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentTypeEpisodeTypeMappingCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentTypeEpisodeTypeMappingUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DocumentTypeEpisodeTypeMapping] ADD CONSTRAINT [pkDocumentTypeEpisodeTypeMapping] PRIMARY KEY CLUSTERED  ([DocumentTypeID], [EpisodeTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DocumentTypeEpisodeTypeMapping] ADD CONSTRAINT [fkDocumentTypeEpisodeTypeMappingDocumentTypeIDDocumentTypeDocumentTypeID] FOREIGN KEY ([DocumentTypeID]) REFERENCES [dbo].[DocumentType] ([DocumentTypeID])
GO
ALTER TABLE [dbo].[DocumentTypeEpisodeTypeMapping] ADD CONSTRAINT [fkDocumentTypeEpisodeTypeMappingEpisodeTypeIDEpisodeTypeEpisodeTypeID] FOREIGN KEY ([EpisodeTypeID]) REFERENCES [dbo].[EpisodeType] ([EpisodeTypeID])
GO
GRANT DELETE ON  [dbo].[DocumentTypeEpisodeTypeMapping] TO [acondie]
GO
GRANT INSERT ON  [dbo].[DocumentTypeEpisodeTypeMapping] TO [acondie]
GO
GRANT SELECT ON  [dbo].[DocumentTypeEpisodeTypeMapping] TO [acondie]
GO
GRANT UPDATE ON  [dbo].[DocumentTypeEpisodeTypeMapping] TO [acondie]
GO
