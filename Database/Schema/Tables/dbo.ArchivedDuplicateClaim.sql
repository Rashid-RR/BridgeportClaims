CREATE TABLE [dbo].[ArchivedDuplicateClaim]
(
[ArchivedDuplicateClaimID] [int] NOT NULL,
[ExcludeClaimID] [int] NOT NULL,
[ExcludedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfArchivedDuplicateClaimCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfArchivedDuplicateClaimUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ArchivedDuplicateClaim] ADD CONSTRAINT [pkArchivedDuplicateClaim] PRIMARY KEY CLUSTERED  ([ArchivedDuplicateClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ArchivedDuplicateClaim] ADD CONSTRAINT [idxUqArchivedDuplicateClaimExcludeClaimID] UNIQUE NONCLUSTERED  ([ExcludeClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ArchivedDuplicateClaim] ADD CONSTRAINT [fkArchivedDuplicateClaimExcludeClaimIDClaimClaimID] FOREIGN KEY ([ExcludeClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[ArchivedDuplicateClaim] ADD CONSTRAINT [fkArchivedDuplicateClaimExcludedByUserIDAspNetUsersID] FOREIGN KEY ([ExcludedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
