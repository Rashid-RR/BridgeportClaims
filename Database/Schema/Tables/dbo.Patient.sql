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
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPatientCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPatientUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL,
[ETLRowID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
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
CREATE TRIGGER [dbo].[utPatientAudit] ON [dbo].[Patient] FOR INSERT, UPDATE, DELETE
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
        
                    INSERT  INTO dbo.PatientAudit
                            ( PatientID, LastName, FirstName, Address1, Address2, City, PostalCode, StateID, PhoneNumber, AlternatePhoneNumber, EmailAddress, DateOfBirth, GenderID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID, Operation, SystemUser, AuditDateUTC)
                            SELECT  PatientID, LastName, FirstName, Address1, Address2, City, PostalCode, StateID, PhoneNumber, AlternatePhoneNumber, EmailAddress, DateOfBirth, GenderID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID,'UPDATE'
                                   ,SUSER_SNAME()
                                   ,SYSUTCDATETIME()
                            FROM    INSERTED
           
                END 
            ELSE 
                BEGIN 
                    INSERT  INTO dbo.PatientAudit
                            ( PatientID, LastName, FirstName, Address1, Address2, City, PostalCode, StateID, PhoneNumber, AlternatePhoneNumber, EmailAddress, DateOfBirth, GenderID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID, Operation, SystemUser, AuditDateUTC
                            )
                            SELECT  PatientID, LastName, FirstName, Address1, Address2, City, PostalCode, StateID, PhoneNumber, AlternatePhoneNumber, EmailAddress, DateOfBirth, GenderID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID,'INSERT'
                                   ,SUSER_SNAME()
                                   ,SYSUTCDATETIME()
                            FROM    INSERTED
                END 
        END 
    ELSE 
        BEGIN 
            INSERT  INTO dbo.PatientAudit
                    ( PatientID, LastName, FirstName, Address1, Address2, City, PostalCode, StateID, PhoneNumber, AlternatePhoneNumber, EmailAddress, DateOfBirth, GenderID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID, Operation, SystemUser, AuditDateUTC)
                    SELECT  PatientID, LastName, FirstName, Address1, Address2, City, PostalCode, StateID, PhoneNumber, AlternatePhoneNumber, EmailAddress, DateOfBirth, GenderID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID,'DELETE'
                           ,SUSER_SNAME()
                           ,SYSUTCDATETIME()
                    FROM    DELETED
        END
END
GO
ALTER TABLE [dbo].[Patient] ADD CONSTRAINT [pkPaitent] PRIMARY KEY CLUSTERED  ([PatientID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPatientAddress2] ON [dbo].[Patient] ([Address2]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPatientLastNameFirstNameIncludes] ON [dbo].[Patient] ([LastName], [FirstName]) INCLUDE ([PatientID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Patient] ADD CONSTRAINT [fkPatientGenderIDGenderGenderID] FOREIGN KEY ([GenderID]) REFERENCES [dbo].[Gender] ([GenderID])
GO
ALTER TABLE [dbo].[Patient] ADD CONSTRAINT [fkPatientModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Patient] ADD CONSTRAINT [fkPatientStateIDUsStateStateID] FOREIGN KEY ([StateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
