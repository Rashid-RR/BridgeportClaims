CREATE TABLE [util].[ImportFile]
(
[ImportFileID] [int] NOT NULL IDENTITY(1, 1),
[FileBytes] [varbinary] (max) NOT NULL,
[FileName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FileExtension] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FileDescription] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfImportFileCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfImportFileUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [util].[ImportFile] ADD CONSTRAINT [pkImportFile] PRIMARY KEY CLUSTERED  ([ImportFileID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUqImportFileFileName] ON [util].[ImportFile] ([FileName]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxImportFileFileNameIncludeImportFileIDFileExtensionCreatedOnUTC] ON [util].[ImportFile] ([FileName]) INCLUDE ([CreatedOnUTC], [FileExtension], [ImportFileID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
