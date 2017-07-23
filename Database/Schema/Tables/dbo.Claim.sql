CREATE TABLE [dbo].[Claim]
(
[ClaimID] [int] NOT NULL IDENTITY(1, 1),
[PolicyNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateOfInjury] [datetime2] NULL,
[IsFirstParty] [bit] NOT NULL,
[ClaimNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PreviousClaimNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PersonCode] [int] NULL,
[PayorID] [int] NOT NULL,
[AdjusterID] [int] NULL,
[JurisdictionStateID] [int] NULL,
[RelationCode] [tinyint] NULL,
[TermDate] [datetime2] NULL,
[UniqueClaimNumber] AS ([ClaimNumber]+isnull('-'+CONVERT([char](2),[PersonCode],(0)),'')),
[PatientID] [int] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimUpdatedOnUTC] DEFAULT (sysdatetime()),
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
CREATE NONCLUSTERED INDEX [idxClaimJurisdictionStateID] ON [dbo].[Claim] ([JurisdictionStateID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPatientIDPatientPatientID] ON [dbo].[Claim] ([PatientID]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPayorIDIncludes] ON [dbo].[Claim] ([PayorID]) INCLUDE ([AdjusterID], [ClaimID], [ClaimNumber], [CreatedOnUTC], [DateOfInjury], [IsFirstParty], [JurisdictionStateID], [PersonCode], [PolicyNumber], [PreviousClaimNumber], [RelationCode], [TermDate], [UniqueClaimNumber], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimAdjusterIDAdjustorAdjustorID] FOREIGN KEY ([AdjusterID]) REFERENCES [dbo].[Adjustor] ([AdjustorID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimJurisdictionStateIDUsStateStateID] FOREIGN KEY ([JurisdictionStateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimPatientIDPatientPatientID] FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Patient] ([PatientID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimPayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO
