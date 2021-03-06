CREATE TABLE [dbo].[ClaimNoteType]
(
[ClaimNoteTypeID] [int] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimNoteTypeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimNoteTypeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ClaimNoteType] ADD CONSTRAINT [pkClaimNoteType] PRIMARY KEY CLUSTERED  ([ClaimNoteTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUqClaimNoteTypeCodeIncludeTypeName] ON [dbo].[ClaimNoteType] ([Code]) INCLUDE ([TypeName]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
GRANT DELETE ON  [dbo].[ClaimNoteType] TO [acondie]
GO
GRANT INSERT ON  [dbo].[ClaimNoteType] TO [acondie]
GO
GRANT SELECT ON  [dbo].[ClaimNoteType] TO [acondie]
GO
GRANT UPDATE ON  [dbo].[ClaimNoteType] TO [acondie]
GO
