CREATE TABLE [dbo].[FileType]
(
[FileTypeID] [tinyint] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfFileTypeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfFileTypeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[FileType] ADD CONSTRAINT [pkFileType] PRIMARY KEY CLUSTERED  ([FileTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FileType] ADD CONSTRAINT [idxUqFileTypeCode] UNIQUE NONCLUSTERED  ([Code]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FileType] ADD CONSTRAINT [idxUqFileTypeTypeName] UNIQUE NONCLUSTERED  ([TypeName]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
GRANT DELETE ON  [dbo].[FileType] TO [acondie]
GO
GRANT INSERT ON  [dbo].[FileType] TO [acondie]
GO
GRANT SELECT ON  [dbo].[FileType] TO [acondie]
GO
GRANT UPDATE ON  [dbo].[FileType] TO [acondie]
GO
