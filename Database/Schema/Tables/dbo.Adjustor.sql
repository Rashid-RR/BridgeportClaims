CREATE TABLE [dbo].[Adjustor]
(
[AdjustorID] [int] NOT NULL IDENTITY(1, 1),
[AdjustorName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Address1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateID] [int] NULL,
[PostalCode] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FaxNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailAddress] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Extension] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfAdjustorCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfAdjustorUpdatedOnUTC] DEFAULT (sysutcdatetime()),
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
CREATE TRIGGER [dbo].[utAdjustorAudit] ON [dbo].[Adjustor] FOR INSERT, UPDATE, DELETE
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
        
                    INSERT  INTO dbo.AdjustorAudit
                            ( AdjustorID, AdjustorName, PhoneNumber, FaxNumber, EmailAddress, Extension, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID, Operation, SystemUser, AuditDateUTC)
                            SELECT  AdjustorID, AdjustorName, PhoneNumber, FaxNumber, EmailAddress, Extension, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID,'UPDATE'
                                   ,SUSER_SNAME()
                                   ,SYSUTCDATETIME()
                            FROM    INSERTED
           
                END 
            ELSE 
                BEGIN 
                    INSERT  INTO dbo.AdjustorAudit
                            ( AdjustorID, AdjustorName, PhoneNumber, FaxNumber, EmailAddress, Extension, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID, Operation, SystemUser, AuditDateUTC
                            )
                            SELECT  AdjustorID, AdjustorName, PhoneNumber, FaxNumber, EmailAddress, Extension, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID,'INSERT'
                                   ,SUSER_SNAME()
                                   ,SYSUTCDATETIME()
                            FROM    INSERTED
                END 
        END 
    ELSE 
        BEGIN 
            INSERT  INTO dbo.AdjustorAudit
                    ( AdjustorID, AdjustorName, PhoneNumber, FaxNumber, EmailAddress, Extension, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID, Operation, SystemUser, AuditDateUTC)
                    SELECT  AdjustorID, AdjustorName, PhoneNumber, FaxNumber, EmailAddress, Extension, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC, ETLRowID,'DELETE'
                           ,SUSER_SNAME()
                           ,SYSUTCDATETIME()
                    FROM    DELETED
        END
END
GO
ALTER TABLE [dbo].[Adjustor] ADD CONSTRAINT [pkAdjustor] PRIMARY KEY CLUSTERED  ([AdjustorID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Adjustor] ADD CONSTRAINT [fkAdjustorModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Adjustor] ADD CONSTRAINT [fkAdjustorStateIDUsStateStateID] FOREIGN KEY ([StateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
