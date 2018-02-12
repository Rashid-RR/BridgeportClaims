CREATE TABLE [dbo].[EpisodeTypeUsersMapping]
(
[EpisodeTypeID] [tinyint] NOT NULL,
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeTypeUsersMappingCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeTypeUsersMappingUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[EpisodeTypeUsersMapping] ADD CONSTRAINT [pkEpisodeTypeUsersMapping] PRIMARY KEY CLUSTERED  ([EpisodeTypeID], [UserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EpisodeTypeUsersMapping] ADD CONSTRAINT [fkEpisodeTypeUsersMappingEpisodeTypeIDEpisodeTypeEpisodeTypeID] FOREIGN KEY ([EpisodeTypeID]) REFERENCES [dbo].[EpisodeType] ([EpisodeTypeID])
GO
ALTER TABLE [dbo].[EpisodeTypeUsersMapping] ADD CONSTRAINT [fkEpisodeTypeUsersMappingUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
