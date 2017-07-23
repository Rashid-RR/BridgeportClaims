CREATE TABLE [dbo].[Pharmacy]
(
[PharmacyID] [int] NOT NULL IDENTITY(1, 1),
[NABP] [varchar] (7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[NPI] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PharmacyName] [varchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address1] [varchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address2] [varchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [varchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateID] [int] NULL,
[PostalCode] [varchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AlternatePhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FaxNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Contact] [varchar] (55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContactPhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContactEmailAddress] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FederalTIN] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DispType] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPharmacyCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPharmacyUpdatedOnUTC] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Pharmacy] ADD CONSTRAINT [pkPharmacy] PRIMARY KEY CLUSTERED  ([PharmacyID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Pharmacy] ADD CONSTRAINT [fkPharmacyStateIDUsStateStateID] FOREIGN KEY ([StateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
