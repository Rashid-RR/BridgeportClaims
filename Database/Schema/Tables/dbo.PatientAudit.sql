CREATE TABLE [dbo].[PatientAudit]
(
[PatientAuditID] [int] NOT NULL IDENTITY(1, 1),
[PatientID] [int] NOT NULL,
[LastName] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Address1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PostalCode] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateID] [int] NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AlternatePhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailAddress] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateOfBirth] [date] NULL,
[GenderID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL,
[UpdatedOnUTC] [datetime2] NOT NULL,
[ETLRowID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Operation] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SystemUser] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AuditDateUTC] [datetime2] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[PatientAudit] ADD CONSTRAINT [pkPatientAudit] PRIMARY KEY CLUSTERED  ([PatientAuditID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
