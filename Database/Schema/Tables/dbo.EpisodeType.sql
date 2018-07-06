CREATE TABLE [dbo].[EpisodeType]
(
[EpisodeTypeID] [tinyint] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SortOrder] [tinyint] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeTypeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeTypeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[EpisodeType] ADD CONSTRAINT [pkEpisodeType] PRIMARY KEY CLUSTERED  ([EpisodeTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EpisodeType] ADD CONSTRAINT [idxUqEpisodeTypeCode] UNIQUE NONCLUSTERED  ([Code]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EpisodeType] ADD CONSTRAINT [idxUqEpisodeTypeSortOrder] UNIQUE NONCLUSTERED  ([SortOrder]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
GRANT DELETE ON  [dbo].[EpisodeType] TO [acondie]
GO
GRANT INSERT ON  [dbo].[EpisodeType] TO [acondie]
GO
GRANT SELECT ON  [dbo].[EpisodeType] TO [acondie]
GO
GRANT UPDATE ON  [dbo].[EpisodeType] TO [acondie]
GO
