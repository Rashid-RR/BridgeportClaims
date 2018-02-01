CREATE TABLE [dbo].[Episode]
(
[EpisodeID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
[Note] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EpisodeTypeID] [int] NULL,
[Role] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResolvedUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AcquiredUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AssignedUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RxNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Status] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Created] [date] NULL,
[Description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResolvedDateUTC] [datetime2] NULL,
[PharmacyNABP] [varchar] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DocumentID] [int] NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EpisodeCategoryID] [int] NOT NULL,
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
CREATE NONCLUSTERED INDEX [idxEpisodeAcquiredUserID] ON [dbo].[Episode] ([AcquiredUserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeAssignedUserID] ON [dbo].[Episode] ([AssignedUserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeCreatedClaimIDPharmacyNABPEpisodeTypeIDAssignedUserIDEpisodeCategoryID] ON [dbo].[Episode] ([Created], [ClaimID], [PharmacyNABP], [EpisodeTypeID], [AssignedUserID], [EpisodeCategoryID]) INCLUDE ([EpisodeID], [Note]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeEpisodeCategoryID] ON [dbo].[Episode] ([EpisodeCategoryID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodePharmacyNABP] ON [dbo].[Episode] ([PharmacyNABP]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeResolvedUserID] ON [dbo].[Episode] ([ResolvedUserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeAcquiredUserIDAspNetUsersID] FOREIGN KEY ([AcquiredUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeAssignedUserIDAspNetUsersID] FOREIGN KEY ([AssignedUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeDocumentIDDocumentIndexDocumentID] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[DocumentIndex] ([DocumentID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeEpisodeCategoryIDEpisodeCategoryEpisodeCategoryID] FOREIGN KEY ([EpisodeCategoryID]) REFERENCES [dbo].[EpisodeCategory] ([EpisodeCategoryID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeEpisodeTypeIDEpisodeTypeEpisodeTypeID] FOREIGN KEY ([EpisodeTypeID]) REFERENCES [dbo].[EpisodeType] ([EpisodeTypeID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodePharmacyNABPPharmacyNABP] FOREIGN KEY ([PharmacyNABP]) REFERENCES [dbo].[Pharmacy] ([NABP])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeResolvedUserIDAspNetUsersID] FOREIGN KEY ([ResolvedUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
