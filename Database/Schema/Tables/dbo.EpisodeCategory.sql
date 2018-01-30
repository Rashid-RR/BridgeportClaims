CREATE TABLE [dbo].[EpisodeCategory]
(
[EpisodeCategoryID] [int] NOT NULL IDENTITY(1, 1),
[CategoryName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeCategoryCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeCategoryUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[EpisodeCategory] ADD CONSTRAINT [pkEpisodeCategory] PRIMARY KEY CLUSTERED  ([EpisodeCategoryID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EpisodeCategory] ADD CONSTRAINT [idxUqEpisodeCategoryCode] UNIQUE NONCLUSTERED  ([Code]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
