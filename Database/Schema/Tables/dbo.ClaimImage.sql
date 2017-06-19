CREATE TABLE [dbo].[ClaimImage]
(
[ClaimImageID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NULL,
[ImageType] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Daterec] [datetime2] NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfImageCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfImageUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ClaimImage] ADD CONSTRAINT [pkClaimImage] PRIMARY KEY CLUSTERED  ([ClaimImageID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClaimImage] ADD CONSTRAINT [fkClaimImageClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
