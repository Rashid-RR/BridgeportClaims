CREATE TABLE [dbo].[Suspense]
(
[SuspenseID] [int] NOT NULL IDENTITY(1, 1),
[CheckNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmountRemaining] [money] NOT NULL,
[SuspenseDate] [date] NOT NULL CONSTRAINT [dfSuspenseSuspenseDate] DEFAULT (CONVERT([date],[dtme].[udfGetLocalDateTime](sysutcdatetime()),(0))),
[NoteText] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DocumentID] [int] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfSuspenseCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfSuspenseUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Suspense] ADD CONSTRAINT [pkSuspense] PRIMARY KEY CLUSTERED  ([SuspenseID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxSuspenseUserID] ON [dbo].[Suspense] ([UserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Suspense] ADD CONSTRAINT [fkSuspenseDocumentIDDocumentDocumentID] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[Document] ([DocumentID])
GO
ALTER TABLE [dbo].[Suspense] ADD CONSTRAINT [fkSuspenseUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
