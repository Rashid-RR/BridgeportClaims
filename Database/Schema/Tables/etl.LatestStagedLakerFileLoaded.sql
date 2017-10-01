CREATE TABLE [etl].[LatestStagedLakerFileLoaded]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[LastFileNameLoaded] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [etl].[LatestStagedLakerFileLoaded] ADD CONSTRAINT [pkLatestStagedLakerFileLoaded] PRIMARY KEY CLUSTERED  ([ID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
