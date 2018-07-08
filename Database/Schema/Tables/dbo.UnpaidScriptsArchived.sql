CREATE TABLE [dbo].[UnpaidScriptsArchived]
(
[UnpaidScriptsArchivedID] [int] NOT NULL IDENTITY(1, 1),
[PrescriptionID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfUnpaidScriptsArchivedCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfUnpaidScriptsArchivedUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[UnpaidScriptsArchived] ADD CONSTRAINT [pkUnpaidScriptsArchived] PRIMARY KEY CLUSTERED  ([UnpaidScriptsArchivedID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UnpaidScriptsArchived] ADD CONSTRAINT [idxUqUnpaidScriptsArchivedPrescriptionID] UNIQUE NONCLUSTERED  ([PrescriptionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UnpaidScriptsArchived] ADD CONSTRAINT [fkUnpaidScriptsArchivedModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[UnpaidScriptsArchived] ADD CONSTRAINT [fkUnpaidScriptsArchivedPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
