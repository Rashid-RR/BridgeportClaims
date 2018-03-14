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
[Archived] [bit] NOT NULL CONSTRAINT [dfDocumentArchived] DEFAULT ((0)),
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FileTypeID] [tinyint] NOT NULL CONSTRAINT [dfDocumentFileType] DEFAULT ((1)),
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [pkDocument] PRIMARY KEY CLUSTERED  ([DocumentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDocumentArchivedFileTypeIDIncludes] ON [dbo].[Document] ([Archived], [FileTypeID]) INCLUDE ([ByteCount], [CreatedOnUTC], [CreationTimeLocal], [DataVersion], [DirectoryName], [Extension], [FileName], [FileSize], [FileUrl], [FullFilePath], [LastAccessTimeLocal], [LastWriteTimeLocal], [ModifiedByUserID], [UpdatedOnUTC]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [idxUqDocumentFileNameFileTypeID] UNIQUE NONCLUSTERED  ([FileName], [FileTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDocumentFileTypeID] ON [dbo].[Document] ([FileTypeID]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDocumentFileTypeIDIncludeFileName] ON [dbo].[Document] ([FileTypeID]) INCLUDE ([FileName]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [idxUqDocumentFileUrl] UNIQUE NONCLUSTERED  ([FileUrl]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [fkDocumentFileTypeIDFileTypeFileTypeID] FOREIGN KEY ([FileTypeID]) REFERENCES [dbo].[FileType] ([FileTypeID])
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [fkDocumentModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
