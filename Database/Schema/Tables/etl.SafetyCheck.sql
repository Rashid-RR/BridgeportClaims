CREATE TABLE [etl].[SafetyCheck]
(
[SafetyCheckID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
[PatientID] [int] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfSafetyCheckCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfSafetyCheckUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [etl].[SafetyCheck] ADD CONSTRAINT [pkSafetyCheck] PRIMARY KEY CLUSTERED  ([SafetyCheckID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [etl].[SafetyCheck] ADD CONSTRAINT [fkSafetyCheckClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [etl].[SafetyCheck] ADD CONSTRAINT [fkSafetyCheckPatientIDPatientPatientID] FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Patient] ([PatientID])
GO
