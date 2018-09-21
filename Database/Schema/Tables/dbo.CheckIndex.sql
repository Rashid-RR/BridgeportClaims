CREATE TABLE [dbo].[CheckIndex]
(
[DocumentID] [int] NOT NULL,
[CheckNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfCheckIndexCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfCheckIndexUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[CheckIndex] ADD CONSTRAINT [pkCheckIndex] PRIMARY KEY CLUSTERED  ([DocumentID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxCheckIndexCheckNumberIncludes] ON [dbo].[CheckIndex] ([CheckNumber]) INCLUDE ([CreatedOnUTC], [DocumentID], [ModifiedByUserID], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CheckIndex] ADD CONSTRAINT [fkCheckIndexDocumentIDDocumentDocumentID] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[Document] ([DocumentID]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CheckIndex] ADD CONSTRAINT [fkCheckIndexModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
