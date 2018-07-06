CREATE TABLE [util].[ImportFileType]
(
[ImportFileTypeID] [int] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfImportFileTypeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfImportFileTypeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [util].[ImportFileType] ADD CONSTRAINT [pkImportFileType] PRIMARY KEY CLUSTERED  ([ImportFileTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUqImportFileTypeTypeNameIncludeAll] ON [util].[ImportFileType] ([TypeName]) INCLUDE ([Code], [CreatedOnUTC], [ImportFileTypeID], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
GRANT DELETE ON  [util].[ImportFileType] TO [acondie]
GO
GRANT INSERT ON  [util].[ImportFileType] TO [acondie]
GO
GRANT SELECT ON  [util].[ImportFileType] TO [acondie]
GO
GRANT UPDATE ON  [util].[ImportFileType] TO [acondie]
GO
