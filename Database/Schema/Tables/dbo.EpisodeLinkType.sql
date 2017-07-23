CREATE TABLE [dbo].[EpisodeLinkType]
(
[EpisodeLinkTypeID] [int] NOT NULL IDENTITY(1, 1),
[EpisodeLinkName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EpisodeLinkCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeLinkTypeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeLinkTypeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[EpisodeLinkType] ADD CONSTRAINT [pkEpisodeLinkType] PRIMARY KEY CLUSTERED  ([EpisodeLinkTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
