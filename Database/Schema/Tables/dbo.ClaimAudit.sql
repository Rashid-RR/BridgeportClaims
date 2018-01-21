CREATE TABLE [dbo].[ClaimAudit]
(
[ClaimAuditID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NOT NULL,
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
[UniqueClaimNumber] [varchar] (258) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ClaimFlex2ID] [int] NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL,
[UpdatedOnUTC] [datetime2] NOT NULL,
[DataVersion] [timestamp] NOT NULL,
[Operation] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SystemUser] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AuditDateUTC] [datetime2] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[ClaimAudit] ADD CONSTRAINT [pkClaimAudit] PRIMARY KEY CLUSTERED  ([ClaimAuditID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
