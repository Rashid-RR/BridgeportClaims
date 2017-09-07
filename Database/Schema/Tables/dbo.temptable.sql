CREATE TABLE [dbo].[temptable]
(
[ImportFileID] [int] NULL,
[FileBytes] [varbinary] (max) NULL,
[FileName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FileExtension] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FileSize] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ImportFileTypeID] [int] NULL,
[Processed] [bit] NULL,
[CreatedOnUTC] [datetime2] NULL,
[UpdatedOnUTC] [datetime2] NULL,
[DataVersion] [varbinary] (8) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
