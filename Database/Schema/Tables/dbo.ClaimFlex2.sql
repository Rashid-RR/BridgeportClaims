CREATE TABLE [dbo].[ClaimFlex2]
(
[ClaimFlex2ID] [int] NOT NULL IDENTITY(1, 1),
[Flex2] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimFlex2CreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimFlex2UpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ClaimFlex2] ADD CONSTRAINT [pkClaimFlex2] PRIMARY KEY CLUSTERED  ([ClaimFlex2ID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClaimFlex2] ADD CONSTRAINT [idxUqClaimFlex2Flex2] UNIQUE NONCLUSTERED  ([Flex2]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
