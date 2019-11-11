CREATE TABLE [dbo].[UserTimeSheet]
(
[UserTimeSheetID] [int] NOT NULL IDENTITY(1, 1),
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StartTime] [datetime2] NOT NULL,
[EndTime] [datetime2] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfUserTimeSheetCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfUserTimeSheetUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[UserTimeSheet] ADD CONSTRAINT [pkUserTimeSheet] PRIMARY KEY CLUSTERED  ([UserTimeSheetID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserTimeSheet] ADD CONSTRAINT [fkUserTimeSheetUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
