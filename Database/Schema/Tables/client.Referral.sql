CREATE TABLE [client].[Referral]
(
[ReferralID] [int] NOT NULL IDENTITY(1, 1),
[ClaimNumber] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[JurisdictionStateID] [int] NOT NULL,
[LastName] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DateOfBirth] [date] NOT NULL,
[InjuryDate] [date] NOT NULL,
[Notes] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReferredBy] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferralDate] [datetime2] NOT NULL,
[ReferralTypeID] [tinyint] NOT NULL,
[EligibilityStart] [datetime2] NULL,
[EligibilityEnd] [datetime2] NULL,
[Address1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Address2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateID] [int] NOT NULL,
[PostalCode] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PatientPhone] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AdjustorName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AdjustorPhone] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfReferralCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfReferralUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [client].[Referral] ADD CONSTRAINT [pkReferral] PRIMARY KEY CLUSTERED  ([ReferralID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [client].[Referral] ADD CONSTRAINT [fkReferralJurisdictionStateIDUsStateStateID] FOREIGN KEY ([JurisdictionStateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
ALTER TABLE [client].[Referral] ADD CONSTRAINT [fkReferralReferralTypeIDReferralTypeReferralTypeID] FOREIGN KEY ([ReferralTypeID]) REFERENCES [client].[ReferralType] ([ReferralTypeID])
GO
ALTER TABLE [client].[Referral] ADD CONSTRAINT [fkReferralReferredByAspNetUsersID] FOREIGN KEY ([ReferredBy]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [client].[Referral] ADD CONSTRAINT [fkReferralStateIDUsStateStateID] FOREIGN KEY ([StateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
