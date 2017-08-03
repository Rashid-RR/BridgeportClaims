CREATE TABLE [dbo].[Episode]
(
[EpisodeID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
[EpisodeTypeID] [int] NULL,
[Note] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Role] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Type] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResolvedUser] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AcquiredUser] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AssignedUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RxNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Status] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedDateUTC] [datetime2] NULL,
[Description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResolvedDate] [datetime2] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [pkEpisode] PRIMARY KEY CLUSTERED  ([EpisodeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeClaimID] ON [dbo].[Episode] ([ClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeEpisodeTypeIDEpisodeTypeEpisodeTypeID] FOREIGN KEY ([EpisodeTypeID]) REFERENCES [dbo].[EpisodeType] ([EpisodeTypeID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkfkfk] FOREIGN KEY ([AssignedUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
