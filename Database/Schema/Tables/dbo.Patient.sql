CREATE TABLE [dbo].[Patient]
(
[PatientID] [int] NOT NULL IDENTITY(1, 1),
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
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPatientCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPatientUpdatedOnUTC] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Patient] ADD CONSTRAINT [pkPaitent] PRIMARY KEY CLUSTERED  ([PatientID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPatientGenderID] ON [dbo].[Patient] ([GenderID], [StateID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Patient] ADD CONSTRAINT [fkPatientGenderIDGenderGenderID] FOREIGN KEY ([GenderID]) REFERENCES [dbo].[Gender] ([GenderID])
GO
ALTER TABLE [dbo].[Patient] ADD CONSTRAINT [fkPatientStateIDUsStateStateID] FOREIGN KEY ([StateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
