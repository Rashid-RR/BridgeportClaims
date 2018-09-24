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
[DocumentDate] [date] NULL,
[ByteCount] [bigint] NOT NULL,
[Archived] [bit] NOT NULL CONSTRAINT [dfDocumentArchived] DEFAULT ((0)),
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FileTypeID] [tinyint] NOT NULL CONSTRAINT [dfDocumentFileType] DEFAULT ((1)),
[IsValid] [bit] NOT NULL CONSTRAINT [ckDocumentIsValidTrue] DEFAULT ((1)),
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
CREATE NONCLUSTERED INDEX [idxDocumentFileNameIncludes] ON [dbo].[Document] ([FileName]) INCLUDE ([CreationTimeLocal], [DocumentID], [Extension], [FileSize], [FileUrl], [FullFilePath], [LastAccessTimeLocal], [LastWriteTimeLocal]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDocumentFileTypeIDIncludeFileName] ON [dbo].[Document] ([FileTypeID]) INCLUDE ([FileName]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [idxUqDocumentFileUrl] UNIQUE NONCLUSTERED  ([FileUrl]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [fkDocumentFileTypeIDFileTypeFileTypeID] FOREIGN KEY ([FileTypeID]) REFERENCES [dbo].[FileType] ([FileTypeID])
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [fkDocumentModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
