CREATE TABLE [dbo].[SkippedPaymentExclusion]
(
[SkippedPaymentExclusionID] [int] NOT NULL IDENTITY(1, 1),
[PrescriptionID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfSkippedPaymentExclusionCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfSkippedPaymentExclusionUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[SkippedPaymentExclusion] ADD CONSTRAINT [pkSkippedPaymentExclusion] PRIMARY KEY CLUSTERED  ([SkippedPaymentExclusionID]) WITH (DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SkippedPaymentExclusion] ADD CONSTRAINT [fkSkippedPaymentExclusionModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[SkippedPaymentExclusion] ADD CONSTRAINT [fkSkippedPaymentExclusionPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
