CREATE TABLE [util].[MaintenanceSchedule]
(
[MaintenanceScheduleID] [int] NOT NULL IDENTITY(1, 1),
[DateRunLocal] [datetime2] NOT NULL CONSTRAINT [dfMaintenanceScheduleDateRunToday] DEFAULT ([dtme].[udfGetLocalDate]()),
[RunTimeMilliseconds] [int] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfMaintenanceScheduleCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfMaintenanceScheduleUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [util].[MaintenanceSchedule] ADD CONSTRAINT [pkMaintenanceSchedule] PRIMARY KEY CLUSTERED  ([MaintenanceScheduleID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
