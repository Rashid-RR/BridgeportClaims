CREATE TABLE [dbo].[Notification]
(
[NotificationID] [int] NOT NULL IDENTITY(1, 1),
[MessageText] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GeneratedDate] [date] NOT NULL,
[IsDismissed] [bit] NOT NULL CONSTRAINT [dfNotificationIsDismissed] DEFAULT ((0)),
[DismissedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NotificationTypeID] [tinyint] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfNotificationCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfNotificationUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Notification] ADD CONSTRAINT [pkNotification] PRIMARY KEY CLUSTERED  ([NotificationID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Notification] ADD CONSTRAINT [fkNotificationDismissedByUserIDAspNetUsersID] FOREIGN KEY ([DismissedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Notification] ADD CONSTRAINT [fkNotificationNotificationTypeIDNotificationTypeNotificationTypeID] FOREIGN KEY ([NotificationTypeID]) REFERENCES [dbo].[NotificationType] ([NotificationTypeID])
GO
