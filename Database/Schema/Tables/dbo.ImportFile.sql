CREATE TABLE [dbo].[ImportFile]
(
[ImportFileID] [int] NOT NULL IDENTITY(1, 1),
[FileBytes] [varbinary] (max) NOT NULL,
[FileName] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FileExtension] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FileDescription] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfImportFileCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfImportFileUpdatedOnUTC] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ImportFile] ADD CONSTRAINT [pkImportFile] PRIMARY KEY CLUSTERED  ([ImportFileID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
