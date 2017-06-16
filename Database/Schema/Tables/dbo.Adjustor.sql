CREATE TABLE [dbo].[Adjustor]
(
[AdjustorID] [int] NOT NULL IDENTITY(1, 1),
[PayorID] [int] NOT NULL,
[AdjustorName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PhoneNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FaxNumber] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EmailAddress] [varchar] (155) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Extension] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfAdjustorCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfAdjustorUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = PAGE
)
GO
ALTER TABLE [dbo].[Adjustor] ADD CONSTRAINT [pkAdjustor] PRIMARY KEY CLUSTERED  ([AdjustorID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxAdjustorPayorIDIncludeAll] ON [dbo].[Adjustor] ([PayorID]) INCLUDE ([AdjustorID], [AdjustorName], [CreatedOn], [DataVersion], [EmailAddress], [Extension], [FaxNumber], [PhoneNumber], [UpdatedOn]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Adjustor] WITH NOCHECK ADD CONSTRAINT [fkAdjustorPayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO
