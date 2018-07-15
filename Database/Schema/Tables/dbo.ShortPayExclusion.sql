CREATE TABLE [dbo].[ShortPayExclusion]
(
[ShortPayExclusionID] [int] NOT NULL IDENTITY(1, 1),
[PrescriptionID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfShortPayExclusionCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfShortPayExclusionUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ShortPayExclusion] ADD CONSTRAINT [pkShortPayExclusion] PRIMARY KEY CLUSTERED  ([ShortPayExclusionID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ShortPayExclusion] ADD CONSTRAINT [fkShortPayExclusionModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[ShortPayExclusion] ADD CONSTRAINT [fkShortPayExclusionPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
