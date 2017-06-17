CREATE TABLE [dbo].[NLog]
(
[NLogID] [int] NOT NULL IDENTITY(1, 1),
[MachineName] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SiteName] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Logged] [datetime2] NOT NULL,
[Level] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UserName] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Message] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Logger] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Properties] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ServerName] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Port] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Url] [nvarchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Https] [bit] NULL,
[ServerAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RemoteAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Callsite] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Exception] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[NLog] ADD CONSTRAINT [pkNLog] PRIMARY KEY CLUSTERED  ([NLogID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
