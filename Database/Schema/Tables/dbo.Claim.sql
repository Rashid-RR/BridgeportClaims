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
[AdjustorID] [int] NULL,
[JurisdictionStateID] [int] NULL,
[RelationCode] [tinyint] NULL,
[TermDate] [datetime2] NULL,
[PatientID] [int] NOT NULL,
[ETLRowID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UniqueClaimNumber] AS (([ClaimNumber]+'-')+isnull([PersonCode],'')),
[ClaimFlex2ID] [int] NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfClaimUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE TRIGGER [dbo].[utClaimAudit] ON [dbo].[Claim] FOR INSERT, UPDATE, DELETE
AS BEGIN
	SET NOCOUNT ON;
    IF ( SELECT COUNT(*)
         FROM   INSERTED
       ) > 0 
        BEGIN 
            IF ( SELECT COUNT(*)
                 FROM   DELETED
               ) > 0 
                BEGIN 
        
                    INSERT  INTO dbo.ClaimAudit
                            ( ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, Operation, SystemUser, AuditDateUTC)
                            SELECT  ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC,'UPDATE'
                                   ,SUSER_SNAME()
                                   ,SYSUTCDATETIME()
                            FROM    INSERTED
           
                END 
            ELSE 
                BEGIN 
                    INSERT  INTO dbo.ClaimAudit
                            ( ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, Operation, SystemUser, AuditDateUTC
                            )
                            SELECT  ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC,'INSERT'
                                   ,SUSER_SNAME()
                                   ,SYSUTCDATETIME()
                            FROM    INSERTED
                END 
        END 
    ELSE 
        BEGIN 
            INSERT  INTO dbo.ClaimAudit
                    ( ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, Operation, SystemUser, AuditDateUTC)
                    SELECT  ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC,'DELETE'
                           ,SUSER_SNAME()
                           ,SYSUTCDATETIME()
                    FROM    DELETED
        END
END
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [ckClaimRelationCode] CHECK (([RelationCode]>=(1) AND [RelationCode]<=(9)))
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [pkClaim] PRIMARY KEY CLUSTERED  ([ClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimClaimNumber] ON [dbo].[Claim] ([ClaimNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimCreatedOnUTCUpdatedOnUTC] ON [dbo].[Claim] ([CreatedOnUTC], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPatientIDPatientPatientID] ON [dbo].[Claim] ([PatientID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPayorIDIncludes] ON [dbo].[Claim] ([PayorID]) INCLUDE ([AdjustorID], [ClaimID], [ClaimNumber], [CreatedOnUTC], [DateOfInjury], [IsFirstParty], [JurisdictionStateID], [PersonCode], [PolicyNumber], [PreviousClaimNumber], [RelationCode], [TermDate], [UniqueClaimNumber], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPayorIDIncludeClaimNumberPatientID] ON [dbo].[Claim] ([PayorID]) INCLUDE ([ClaimNumber], [PatientID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimAdjusterIDAdjustorAdjustorID] FOREIGN KEY ([AdjustorID]) REFERENCES [dbo].[Adjustor] ([AdjustorID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimClaimFlex2IDClaimFlex2ClaimFlex2ID] FOREIGN KEY ([ClaimFlex2ID]) REFERENCES [dbo].[ClaimFlex2] ([ClaimFlex2ID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimJurisdictionStateIDUsStateStateID] FOREIGN KEY ([JurisdictionStateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimPatientIDPatientPatientID] FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Patient] ([PatientID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimPayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO
