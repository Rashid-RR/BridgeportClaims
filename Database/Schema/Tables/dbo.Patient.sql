CREATE TABLE [dbo].[Patient]
(
[PatientID] [int] NOT NULL,
[ClaimID] [int] NULL,
[LastName] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Address1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PostalCode] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateCode] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AlternatePhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailAddress] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateOfBirth] [datetime2] NOT NULL,
[Gender] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPatientCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPatientUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
