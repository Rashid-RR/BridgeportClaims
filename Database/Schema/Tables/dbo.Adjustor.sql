CREATE TABLE [dbo].[Adjustor]
(
[AdjustorID] [int] NOT NULL IDENTITY(1, 1),
[PayorID] [int] NOT NULL,
[AdjustorName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FaxNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailAddress] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Extension] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfAdjustorCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfAdjustorUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Adjustor] ADD CONSTRAINT [pkAdjustor] PRIMARY KEY CLUSTERED  ([AdjustorID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxAdjustorPayorIDIncludeAll] ON [dbo].[Adjustor] ([PayorID]) INCLUDE ([AdjustorID], [AdjustorName], [CreatedOnUTC], [DataVersion], [EmailAddress], [Extension], [FaxNumber], [PhoneNumber], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Adjustor] ADD CONSTRAINT [fkAdjustorPayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO
