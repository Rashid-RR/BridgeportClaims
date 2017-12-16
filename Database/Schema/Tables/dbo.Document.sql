CREATE TABLE [dbo].[Document]
(
[DocumentID] [int] NOT NULL IDENTITY(1, 1),
[FileName] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Extension] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FileSize] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreationTimeLocal] [datetime2] NOT NULL,
[LastAccessTimeLocal] [datetime2] NOT NULL,
[LastWriteTimeLocal] [datetime2] NOT NULL,
[DirectoryName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FullFilePath] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FileUrl] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DocumentDate] AS (TRY_CAST(substring([FileName],(4),(8)) AS [date])),
[ByteCount] [bigint] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL,
[UpdatedOnUTC] [datetime2] NOT NULL,
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [pkDocument] PRIMARY KEY CLUSTERED  ([DocumentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [idxUqDocumentFileName] UNIQUE NONCLUSTERED  ([FileName]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [idxUqDocumentFileUrl] UNIQUE NONCLUSTERED  ([FileUrl]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
