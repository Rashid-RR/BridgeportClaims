CREATE TYPE [dbo].[udtDocument] AS TABLE
(
[DocumentID] [int] NOT NULL,
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
[FileTypeID] [tinyint] NOT NULL
)
GO
GRANT EXECUTE ON TYPE:: [dbo].[udtDocument] TO [bridgeportClaimsWindowsServiceUser]
GO
