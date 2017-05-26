CREATE TABLE [dbo].[Episode]
(
[EpisodeID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
[EpisodeNumber] [int] NULL,
[Note] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Role] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Type] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResolvedUser] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AcquiredUser] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AssignUser] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RxNumber] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Status] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedDate] [datetime2] NULL,
[Description] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResolvedDate] [datetime2] NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfEpisodeCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfEpisodeUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [pkEpisode] PRIMARY KEY CLUSTERED  ([EpisodeID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
