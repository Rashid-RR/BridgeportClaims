CREATE TABLE [dbo].[AdjustorAudit]
(
[AdjustorAuditID] [int] NOT NULL IDENTITY(1, 1),
[AdjustorID] [int] NOT NULL,
[AdjustorName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FaxNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailAddress] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Extension] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
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
ALTER TABLE [dbo].[AdjustorAudit] ADD CONSTRAINT [pkAdjustorAudit] PRIMARY KEY CLUSTERED  ([AdjustorAuditID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
