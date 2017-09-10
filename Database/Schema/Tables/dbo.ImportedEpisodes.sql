CREATE TABLE [dbo].[ImportedEpisodes]
(
[ImportedEpisodeID] [int] NOT NULL IDENTITY(1, 1),
[EpisodeID] [int] NULL,
[ClaimID] [int] NULL,
[NoteText] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EpisodeTypeID] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Role] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Type] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResolvedUser] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AcquiredUser] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AssignedUserID] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RxNumber] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Status] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedDateUTC] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResolvedDate] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UpdatedOnUTC] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DataVersion] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ImportedEpisodes] ADD CONSTRAINT [pkImportedEpisodes] PRIMARY KEY CLUSTERED  ([ImportedEpisodeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
