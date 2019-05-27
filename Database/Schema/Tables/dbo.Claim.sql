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
[IsMaxBalance] [bit] NOT NULL CONSTRAINT [dfClaimIsMaxBalanceFalse] DEFAULT ((0)),
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AttorneyID] [int] NULL,
[IsAttorneyManagedDate] [date] NULL,
[IsAttorneyManaged] AS (CONVERT([bit],case  when [IsAttorneyManagedDate] IS NOT NULL then (1) else (0) end,(0))),
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
    DECLARE @Now DATETIME2 = dtme.udfGetDate(), @User NVARCHAR(128) = SUSER_SNAME();
    IF EXISTS ( SELECT *
                FROM   INSERTED
              )
        BEGIN 
            IF EXISTS ( SELECT *
                        FROM   DELETED
                      )
                BEGIN 
        
                    INSERT  INTO dbo.ClaimAudit
                            ( ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, [IsMaxBalance], ModifiedByUserID, AttorneyID, CreatedOnUTC, UpdatedOnUTC, Operation, SystemUser, AuditDateUTC)
                            SELECT  ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, [IsMaxBalance], ModifiedByUserID, AttorneyID, CreatedOnUTC, UpdatedOnUTC,'UPDATE'
                                   ,@User
                                   ,@Now
                            FROM    INSERTED
           
                END 
            ELSE 
                BEGIN 
                    INSERT  INTO dbo.ClaimAudit
                            ( ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, [IsMaxBalance], ModifiedByUserID, AttorneyID, CreatedOnUTC, UpdatedOnUTC, Operation, SystemUser, AuditDateUTC
                            )
                            SELECT  ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, [IsMaxBalance], ModifiedByUserID, AttorneyID, CreatedOnUTC, UpdatedOnUTC,'INSERT'
                                   ,@User
                                   ,@Now
                            FROM    INSERTED
                END 
        END 
    ELSE 
        BEGIN 
            INSERT  INTO dbo.ClaimAudit
                    ( ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, [IsMaxBalance], ModifiedByUserID, AttorneyID, CreatedOnUTC, UpdatedOnUTC, Operation, SystemUser, AuditDateUTC)
                    SELECT  ClaimID, PolicyNumber, DateOfInjury, IsFirstParty, ClaimNumber, PreviousClaimNumber, PersonCode, PayorID, [AdjustorID], JurisdictionStateID, RelationCode, TermDate, PatientID, ETLRowID, UniqueClaimNumber, ClaimFlex2ID, [IsMaxBalance], ModifiedByUserID, AttorneyID, CreatedOnUTC, UpdatedOnUTC,'DELETE'
                           ,@User
                           ,@Now
                    FROM    DELETED
        END
END
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [ckClaimIsAttorneyManagedDate] CHECK (((1)=case  when [IsAttorneyManagedDate] IS NOT NULL AND [AttorneyID] IS NOT NULL then (1) when [IsAttorneyManagedDate] IS NULL then (1) else (0) end))
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [ckClaimRelationCode] CHECK (([RelationCode]>=(1) AND [RelationCode]<=(9)))
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [pkClaim] PRIMARY KEY CLUSTERED  ([ClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimAdjustorID] ON [dbo].[Claim] ([AdjustorID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET ARITHABORT ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE NONCLUSTERED INDEX [idxClaimAttorneyID] ON [dbo].[Claim] ([AttorneyID]) WHERE ([AttorneyID] IS NOT NULL) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimClaimNumber] ON [dbo].[Claim] ([ClaimNumber]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimCreatedOnUTCUpdatedOnUTC] ON [dbo].[Claim] ([CreatedOnUTC], [UpdatedOnUTC]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPatientIDPatientPatientID] ON [dbo].[Claim] ([PatientID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxClaimPayorIDIncludeClaimNumberPatientID] ON [dbo].[Claim] ([PayorID]) INCLUDE ([ClaimNumber], [PatientID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimAdjustorIDAdjustorAdjustorID] FOREIGN KEY ([AdjustorID]) REFERENCES [dbo].[Adjustor] ([AdjustorID])
GO
ALTER TABLE [dbo].[Claim] ADD CONSTRAINT [fkClaimAttorneyIDAttorneyAttorneyID] FOREIGN KEY ([AttorneyID]) REFERENCES [dbo].[Attorney] ([AttorneyID])
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
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET ARITHABORT ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
