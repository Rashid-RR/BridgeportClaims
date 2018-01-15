CREATE TABLE [dbo].[Prescriber]
(
[PrescriberNPI] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ExpDate] [date] NULL,
[PrescriberName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Addr1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Addr2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateID] [int] NULL,
[Zip] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FLDOHNUM] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Phone] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Fax] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriberCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriberUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Prescriber] ADD CONSTRAINT [pkPrescriber] PRIMARY KEY CLUSTERED  ([PrescriberNPI]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Prescriber] ADD CONSTRAINT [fkPrescriberStateIDUsStateStateID] FOREIGN KEY ([StateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
