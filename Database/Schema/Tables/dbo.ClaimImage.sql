CREATE TABLE [dbo].[ClaimImage]
(
[ClaimImageID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
[ClaimImageTypeID] [int] NOT NULL,
[DateRecorded] [datetime2] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfImageCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfImageUpdatedOnUTC] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ClaimImage] ADD CONSTRAINT [pkClaimImage] PRIMARY KEY CLUSTERED  ([ClaimImageID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimImageClaimIDClaimClaimIDIncludeAll] ON [dbo].[ClaimImage] ([ClaimID]) INCLUDE ([ClaimImageID], [ClaimImageTypeID], [CreatedOnUTC], [DateRecorded], [UpdatedOnUTC]) WITH (DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClaimImage] ADD CONSTRAINT [fkClaimImageClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[ClaimImage] ADD CONSTRAINT [fkClaimImageClaimImageTypeIDClaimImageTypeClaimImageTypeID] FOREIGN KEY ([ClaimImageTypeID]) REFERENCES [dbo].[ClaimImageType] ([ClaimImageTypeID])
GO
