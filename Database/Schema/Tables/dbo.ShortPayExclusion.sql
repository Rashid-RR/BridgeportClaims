CREATE TABLE [dbo].[ShortPayExclusion]
(
[ShortPayExclusionID] [int] NOT NULL IDENTITY(1, 1),
[PrescriptionPaymentID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfShortPayExclusionCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfShortPayExclusionUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ShortPayExclusion] ADD CONSTRAINT [pkShortPayExclusion] PRIMARY KEY CLUSTERED  ([ShortPayExclusionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ShortPayExclusion] ADD CONSTRAINT [fkShortPayExclusionModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[ShortPayExclusion] ADD CONSTRAINT [fkShortPayExclusionPrescriptionPaymentIDPrescriptionPrescriptionPaymentID] FOREIGN KEY ([PrescriptionPaymentID]) REFERENCES [dbo].[PrescriptionPayment] ([PrescriptionPaymentID])
GO
