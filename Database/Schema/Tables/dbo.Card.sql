CREATE TABLE [dbo].[Card]
(
[CardID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
[GroupNumber] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CardHolderID] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfCardCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfCardUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Card] ADD CONSTRAINT [pkCard] PRIMARY KEY CLUSTERED  ([CardID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxCardClaimClaimID] ON [dbo].[Card] ([ClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Card] ADD CONSTRAINT [fkCardClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
