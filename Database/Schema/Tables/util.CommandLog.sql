CREATE TABLE [util].[CommandLog]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[DatabaseName] [sys].[sysname] NULL,
[SchemaName] [sys].[sysname] NULL,
[ObjectName] [sys].[sysname] NULL,
[ObjectType] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IndexName] [sys].[sysname] NULL,
[IndexType] [tinyint] NULL,
[StatisticsName] [sys].[sysname] NULL,
[PartitionNumber] [int] NULL,
[ExtendedInfo] [xml] NULL,
[Command] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommandType] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StartTime] [datetime] NOT NULL,
[EndTime] [datetime] NULL,
[ErrorNumber] [int] NULL,
[ErrorMessage] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [util].[CommandLog] ADD CONSTRAINT [pkCommandLog] PRIMARY KEY CLUSTERED  ([ID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
