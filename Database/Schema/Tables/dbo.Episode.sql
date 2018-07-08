CREATE TABLE [dbo].[Episode]
(
[EpisodeID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NULL,
[EpisodeTypeID] [tinyint] NOT NULL,
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
CREATE NONCLUSTERED INDEX [idxEpisodeClaimID] ON [dbo].[Episode] ([ClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeCreatedClaimIDPharmacyNABPEpisodeTypeIDAssignedUserIDEpisodeCategoryIDIncludeEpisodeID] ON [dbo].[Episode] ([Created], [ClaimID], [PharmacyNABP], [EpisodeTypeID], [AssignedUserID], [EpisodeCategoryID]) INCLUDE ([EpisodeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeDocumentIDIncludes] ON [dbo].[Episode] ([DocumentID]) INCLUDE ([AcquiredUserID], [AssignedUserID], [ClaimID], [Created], [CreatedOnUTC], [DataVersion], [Description], [EpisodeCategoryID], [EpisodeTypeID], [ModifiedByUserID], [PharmacyNABP], [ResolvedDateUTC], [ResolvedUserID], [Role], [RxNumber], [Status], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeEpisodeCategoryIDIncludes] ON [dbo].[Episode] ([EpisodeCategoryID]) INCLUDE ([AssignedUserID], [Created], [EpisodeID], [EpisodeTypeID], [PharmacyNABP], [ResolvedDateUTC], [Role], [RxNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeEpisodeTypeIDIncludes] ON [dbo].[Episode] ([EpisodeTypeID]) INCLUDE ([AssignedUserID], [ClaimID], [Created], [DocumentID], [EpisodeCategoryID], [PharmacyNABP], [ResolvedDateUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeAcquiredUserIDAspNetUsersID] FOREIGN KEY ([AcquiredUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeAssignedUserIDAspNetUsersID] FOREIGN KEY ([AssignedUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[Episode] ADD CONSTRAINT [fkEpisodeDocumentIDDocumentIndexDocumentID] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[DocumentIndex] ([DocumentID]) ON DELETE CASCADE
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
