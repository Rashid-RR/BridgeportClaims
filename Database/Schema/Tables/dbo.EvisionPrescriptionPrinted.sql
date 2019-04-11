CREATE TABLE [dbo].[EvisionPrescriptionPrinted]
(
[PrescriptionID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEvisionPrescriptionPrintedCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEvisionPrescriptionPrintedUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[EvisionPrescriptionPrinted] ADD CONSTRAINT [pkEvisionPrescriptionPrinted] PRIMARY KEY CLUSTERED  ([PrescriptionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEvisionPrescriptionPrinted] ON [dbo].[EvisionPrescriptionPrinted] ([ModifiedByUserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EvisionPrescriptionPrinted] ADD CONSTRAINT [fkEvisionPrescriptionPrintedModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[EvisionPrescriptionPrinted] ADD CONSTRAINT [fkEvisionPrescriptionPrintedPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
