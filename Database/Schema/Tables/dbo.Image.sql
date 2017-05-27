CREATE TABLE [dbo].[Image]
(
[ImageID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NULL,
[ImageNumber] [int] NULL,
[ImageType] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Daterec] [datetime2] NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfImageCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfImageUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Image] ADD CONSTRAINT [pkImage] PRIMARY KEY CLUSTERED  ([ImageID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Image] ADD CONSTRAINT [fkImageClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
