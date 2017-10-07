CREATE TABLE [dbo].[PrescriptionStatus]
(
[PrescriptionStatusID] [int] NOT NULL IDENTITY(1, 1),
[StatusName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionStatusCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionStatusUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[PrescriptionStatus] ADD CONSTRAINT [pkPrescriptionStatus] PRIMARY KEY CLUSTERED  ([PrescriptionStatusID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionStatus] ADD CONSTRAINT [idxUqPrescriptionStatusStatusName] UNIQUE NONCLUSTERED  ([StatusName]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
