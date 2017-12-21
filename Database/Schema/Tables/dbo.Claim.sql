CREATE TABLE [dbo].[Claim]
(
[ClaimID] [int] NOT NULL IDENTITY(1, 1),
[PolicyNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateOfInjury] [date] NULL,
[IsFirstParty] [bit] NOT NULL,
[ClaimNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PreviousClaimNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PersonCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayorID] [int] NOT NULL,
[AdjusterID] [int] NULL,
[JurisdictionStateID] [int] NULL,
[RelationCode] [tinyint] NULL,
[TermDate] [datetime2] NULL,
[PatientID] [int] NOT NULL,
[ETLRowID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UniqueClaimNumber] AS (([ClaimNumber]+'-')+isnull([PersonCode],'')),
[ClaimFlex2ID] [int] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [ckClaimRelationCode] CHECK (([RelationCode]>=(1) AND [RelationCode]<=(9)))
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [pkClaim] PRIMARY KEY CLUSTERED  ([ClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimAdjusterID] ON [dbo].[Claim] ([AdjusterID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimClaimFlex2ID] ON [dbo].[Claim] ([ClaimFlex2ID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimClaimNumber] ON [dbo].[Claim] ([ClaimNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimCreatedOnUTCUpdatedOnUTC] ON [dbo].[Claim] ([CreatedOnUTC], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimJurisdictionStateID] ON [dbo].[Claim] ([JurisdictionStateID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPatientIDPatientPatientID] ON [dbo].[Claim] ([PatientID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPayorIDIncludes] ON [dbo].[Claim] ([PayorID]) INCLUDE ([AdjusterID], [ClaimID], [ClaimNumber], [CreatedOnUTC], [DateOfInjury], [IsFirstParty], [JurisdictionStateID], [PersonCode], [PolicyNumber], [PreviousClaimNumber], [RelationCode], [TermDate], [UniqueClaimNumber], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPayorIDIncludeClaimNumberPatientID] ON [dbo].[Claim] ([PayorID]) INCLUDE ([ClaimNumber], [PatientID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimUpdatedOnUTC] ON [dbo].[Claim] ([UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimAdjusterIDAdjustorAdjustorID] FOREIGN KEY ([AdjusterID]) REFERENCES [dbo].[Adjustor] ([AdjustorID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimClaimFlex2IDClaimFlex2ClaimFlex2ID] FOREIGN KEY ([ClaimFlex2ID]) REFERENCES [dbo].[ClaimFlex2] ([ClaimFlex2ID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimJurisdictionStateIDUsStateStateID] FOREIGN KEY ([JurisdictionStateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimPatientIDPatientPatientID] FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Patient] ([PatientID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimPayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO
