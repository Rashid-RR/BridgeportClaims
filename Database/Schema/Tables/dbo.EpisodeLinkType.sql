CREATE TABLE [dbo].[EpisodeLinkType]
(
[EpisodeLinkTypeID] [int] NOT NULL IDENTITY(1, 1),
[EpisodeLinkName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EpisodeLinkCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfEpisodeLinkTypeCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfEpisodeLinkTypeUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = PAGE
)
GO
ALTER TABLE [dbo].[EpisodeLinkType] ADD CONSTRAINT [pkEpisodeLinkType] PRIMARY KEY CLUSTERED  ([EpisodeLinkTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
