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
[Archived] [bit] NOT NULL CONSTRAINT [dfDocumentArchivedFalse] DEFAULT ((0)),
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FileTypeID] [tinyint] NOT NULL CONSTRAINT [dfDocumentFileTypeOne] DEFAULT ((1)),
[IsValid] AS (CONVERT([bit],case  when [FileTypeID]=(3) AND len([FileName])<>(18) then (0) else (1) end,(0))),
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDocumentUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE TRIGGER [dbo].[utDocumentInsteadOfDelete]
ON [dbo].[Document]
INSTEAD OF DELETE
AS
    BEGIN
        SET NOCOUNT ON;
        IF EXISTS
        (
            SELECT *
            FROM   [DELETED] AS [d]
                   INNER JOIN [dbo].[PrescriptionPayment] AS [pp] ON [d].[DocumentID] = [pp].[DocumentID]
        )
            BEGIN
                UPDATE [pp]
                SET    [pp].[DocumentID] = NULL
                FROM   [DELETED] AS [d]
                       INNER JOIN [dbo].[PrescriptionPayment] AS [pp] ON [d].[DocumentID] = [pp].[DocumentID];
            END;
        IF EXISTS
        (
            SELECT *
            FROM   [DELETED] AS [d]
                   INNER JOIN [dbo].[DocumentIndex] AS [di] ON [di].[DocumentID] = [d].[DocumentID]
        )
            BEGIN
                DELETE [di]
                FROM [DELETED] AS [d]
                     INNER JOIN [dbo].[DocumentIndex] AS [di] ON [di].[DocumentID] = [d].[DocumentID];
            END;
        DELETE [do]
        FROM [DELETED] AS [d]
             INNER JOIN [dbo].[DOCUMENT] AS [do] ON [do].[DocumentID] = [d].[DocumentID];
    END;
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE TRIGGER [dbo].[utDocumentInsteadOfInsert] ON [dbo].[Document]
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT dbo.Document ([FileName],Extension,FileSize,CreationTimeLocal,LastAccessTimeLocal,LastWriteTimeLocal
	                     ,DirectoryName,FullFilePath,FileUrl,DocumentDate,ByteCount,Archived,ModifiedByUserID
	                     ,FileTypeID,CreatedOnUTC,UpdatedOnUTC)
	SELECT i.[FileName]
          ,i.Extension
          ,i.FileSize
          ,i.CreationTimeLocal
          ,i.LastAccessTimeLocal
          ,i.LastWriteTimeLocal
          ,i.DirectoryName
          ,i.FullFilePath
          ,i.FileUrl
          ,i.DocumentDate
          ,i.ByteCount
          ,i.Archived
          ,i.ModifiedByUserID
          ,i.FileTypeID
          ,i.CreatedOnUTC
          ,i.UpdatedOnUTC
	FROM INSERTED i  
END
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE TRIGGER [dbo].[utDocumentInsteadOfUpdate] ON [dbo].[Document]
INSTEAD OF UPDATE
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE d SET d.[FileName] = i.[FileName],
				 d.Extension = i.Extension,
				 d.FileSize = i.FileSize,
				 d.CreationTimeLocal = i.CreationTimeLocal,
				 d.LastAccessTimeLocal = i.LastAccessTimeLocal,
				 d.LastWriteTimeLocal = i.LastWriteTimeLocal,
				 d.DirectoryName = i.DirectoryName,
				 d.FullFilePath = i.FullFilePath,
				 d.FileUrl = i.FileUrl,
				 d.DocumentDate = i.DocumentDate,
				 d.ByteCount = i.ByteCount,
				 d.Archived = i.Archived,
				 d.ModifiedByUserID = i.ModifiedByUserID,
				 d.FileTypeID = i.FileTypeID,
				 d.CreatedOnUTC = i.CreatedOnUTC,
				 d.UpdatedOnUTC = i.UpdatedOnUTC
	FROM INSERTED i INNER JOIN dbo.Document AS d ON d.DocumentID = i.DocumentID;
END
GO
ALTER TABLE [dbo].[Document] ADD CONSTRAINT [pkDocument] PRIMARY KEY CLUSTERED  ([DocumentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDocumentDocumentDateArchivedFileTypeIDIncludes] ON [dbo].[Document] ([DocumentDate], [Archived], [FileTypeID]) INCLUDE ([ByteCount], [CreatedOnUTC], [CreationTimeLocal], [DataVersion], [DirectoryName], [Extension], [FileName], [FileSize], [FileUrl], [FullFilePath], [LastAccessTimeLocal], [LastWriteTimeLocal], [ModifiedByUserID], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
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
