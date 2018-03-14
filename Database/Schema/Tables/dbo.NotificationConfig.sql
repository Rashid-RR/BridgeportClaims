CREATE TABLE [dbo].[NotificationConfig]
(
[NotificationConfigID] [int] NOT NULL IDENTITY(1, 1),
[NotificationTypeID] [tinyint] NOT NULL,
[NotificationValue] [sql_variant] NOT NULL,
[SQLVariantDataType] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EffectiveDate] [date] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfNotificationConfigCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfNotificationConfigUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[NotificationConfig] ADD CONSTRAINT [pkNotificationConfig] PRIMARY KEY CLUSTERED  ([NotificationConfigID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxNotificationConfigNotificationTypeID] ON [dbo].[NotificationConfig] ([NotificationTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[NotificationConfig] ADD CONSTRAINT [fkcaNotificationTypeIDNotificationTypeNotificationTypeID] FOREIGN KEY ([NotificationTypeID]) REFERENCES [dbo].[NotificationType] ([NotificationTypeID])
GO
