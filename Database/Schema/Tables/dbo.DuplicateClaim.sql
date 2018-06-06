CREATE TABLE [dbo].[DuplicateClaim]
(
[DuplicateClaimID] [int] NOT NULL,
[ReplacementClaimID] [int] NOT NULL,
[CreatedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDuplicateClaimCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDuplicateClaimUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [pkDuplicateClaim] PRIMARY KEY CLUSTERED  ([DuplicateClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDuplicateClaimCreatedByUserID] ON [dbo].[DuplicateClaim] ([CreatedByUserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDuplicateClaimReplacementClaimID] ON [dbo].[DuplicateClaim] ([ReplacementClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [fkDuplicateClaimCreatedByUserIDAspNetUsersID] FOREIGN KEY ([CreatedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [fkDuplicateClaimReplacementClaimIDClaimClaimID] FOREIGN KEY ([ReplacementClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
