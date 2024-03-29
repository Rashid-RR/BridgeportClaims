CREATE TABLE [dbo].[CollectionAssignment]
(
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateID] [int] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfCollectionAssignmentCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfCollectionAssignmentUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[CollectionAssignment] ADD CONSTRAINT [pkCollectionAssignment] PRIMARY KEY CLUSTERED  ([UserID], [PayorID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxCollectionAssignmentStateIDIncludes] ON [dbo].[CollectionAssignment] ([StateID]) INCLUDE ([CreatedOnUTC], [ModifiedByUserID], [PayorID], [UpdatedOnUTC], [UserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CollectionAssignment] ADD CONSTRAINT [fkCollectionAssignmentModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[CollectionAssignment] ADD CONSTRAINT [fkCollectionAssignmentPayorIDPayorPayorID] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payor] ([PayorID])
GO
ALTER TABLE [dbo].[CollectionAssignment] ADD CONSTRAINT [fkCollectionAssignmentStateIDUsStateStateID] FOREIGN KEY ([StateID]) REFERENCES [dbo].[UsState] ([StateID])
GO
ALTER TABLE [dbo].[CollectionAssignment] ADD CONSTRAINT [fkCollectionAssignmentUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
