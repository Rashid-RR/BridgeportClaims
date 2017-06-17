CREATE TABLE [dbo].[EpisodeLink]
(
[EpisodeLinkID] [int] NOT NULL IDENTITY(1, 1),
[EpisodeLinkTypeID] [int] NOT NULL,
[LinkTransNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EpisodeNumber] [int] NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfEpisodeLinkCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfEpisodeLinkUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = PAGE
)
GO
ALTER TABLE [dbo].[EpisodeLink] ADD CONSTRAINT [pkEpisodeLink] PRIMARY KEY CLUSTERED  ([EpisodeLinkID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EpisodeLink] ADD CONSTRAINT [fkEpisodeLinkEpisodeLinkTypeIDEpisodeLinkTypeEpisodeLinkTypeID] FOREIGN KEY ([EpisodeLinkTypeID]) REFERENCES [dbo].[EpisodeLinkType] ([EpisodeLinkTypeID])
GO
