CREATE TABLE [dbo].[NotificationType]
(
[NotificationTypeID] [tinyint] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[NotificationConfigDescription] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfNotificationTypeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfNotificationTypeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[NotificationType] ADD CONSTRAINT [pkNotificationType] PRIMARY KEY CLUSTERED  ([NotificationTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[NotificationType] ADD CONSTRAINT [idxUqNotificationTypeCode] UNIQUE NONCLUSTERED  ([Code]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
GRANT DELETE ON  [dbo].[NotificationType] TO [acondie]
GO
GRANT INSERT ON  [dbo].[NotificationType] TO [acondie]
GO
GRANT SELECT ON  [dbo].[NotificationType] TO [acondie]
GO
GRANT UPDATE ON  [dbo].[NotificationType] TO [acondie]
GO
