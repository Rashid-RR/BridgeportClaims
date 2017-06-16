CREATE TABLE [dbo].[Pharmacy]
(
[PharmacyID] [int] NOT NULL IDENTITY(1, 1),
[NABP] [nvarchar] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[NPI] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PharmacyName] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address1] [nvarchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address2] [nvarchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateID] [int] NULL,
[PostalCode] [nvarchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AlternatePhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FaxNumber] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Contact] [nvarchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContactPhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContactEmailAddress] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FederalTIN] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DispType] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPharmacyCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPharmacyUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Pharmacy] ADD CONSTRAINT [pkPharmacy] PRIMARY KEY CLUSTERED  ([PharmacyID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Pharmacy] ADD CONSTRAINT [fkPharmacyStateIDUsStateStateID] FOREIGN KEY ([StateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
