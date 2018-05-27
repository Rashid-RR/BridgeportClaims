CREATE TABLE [dbo].[DuplicateClaim]
(
[DuplicateClaimID] [int] NOT NULL,
[ReplacementClaimID] [int] NOT NULL,
[PolicyNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateOfInjury] [date] NULL,
[IsFirstParty] [bit] NOT NULL,
[ClaimNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PreviousClaimNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PersonCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PayorID] [int] NOT NULL,
[AdjustorID] [int] NULL,
[JurisdictionStateID] [int] NULL,
[RelationCode] [tinyint] NULL,
[TermDate] [datetime2] NULL,
[PatientID] [int] NOT NULL,
[ETLRowID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UniqueClaimNumber] AS (([ClaimNumber]+'-')+isnull([PersonCode],'')),
[ClaimFlex2ID] [int] NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDuplicateClaimCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDuplicateClaimUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [pkDuplicateClaim] PRIMARY KEY CLUSTERED  ([DuplicateClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDuplicateClaimReplacementClaimID] ON [dbo].[DuplicateClaim] ([ReplacementClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [fkDuplicateClaimAdjustorIDAdjustorAdjustorID] FOREIGN KEY ([AdjustorID]) REFERENCES [dbo].[Adjustor] ([AdjustorID])
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [fkDuplicateClaimClaimFlex2IDClaimFlex2ClaimFlex2ID] FOREIGN KEY ([ClaimFlex2ID]) REFERENCES [dbo].[ClaimFlex2] ([ClaimFlex2ID])
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [fkDuplicateClaimJurisdictionStateIDUsStateStateID] FOREIGN KEY ([JurisdictionStateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [fkDuplicateClaimModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [fkDuplicateClaimPatientIDPatientPatientID] FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Patient] ([PatientID])
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [fkDuplicateClaimPayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO
ALTER TABLE [dbo].[DuplicateClaim] ADD CONSTRAINT [fkDuplicateClaimReplacementClaimIDClaimClaimID] FOREIGN KEY ([ReplacementClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
