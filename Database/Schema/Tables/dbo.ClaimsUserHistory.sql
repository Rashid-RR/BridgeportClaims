CREATE TABLE [dbo].[ClaimsUserHistory]
(
[ClaimsUserHistoryID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimsUserHistoryCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimsUserHistoryUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ClaimsUserHistory] ADD CONSTRAINT [pkClaimsUserHistory] PRIMARY KEY CLUSTERED  ([ClaimsUserHistoryID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimsUserHistoryClaimIDUserIDIncludeAll] ON [dbo].[ClaimsUserHistory] ([ClaimID], [UserID]) INCLUDE ([ClaimsUserHistoryID], [CreatedOnUTC], [UpdatedOnUTC]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClaimsUserHistory] ADD CONSTRAINT [fkClaimsUserHistoryClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[ClaimsUserHistory] ADD CONSTRAINT [fkClaimsUserHistoryUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
