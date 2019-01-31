CREATE TABLE [dbo].[DecisionTree]
(
[TreeNode] [sys].[hierarchyid] NOT NULL,
[TreePath] AS ([TreeNode].[ToString]()),
[TreeLevel] AS ([TreeNode].[GetLevel]()),
[TreeID] [int] NOT NULL CONSTRAINT [dfDecisionTreeTreeIDSequence] DEFAULT (NEXT VALUE FOR [dbo].[seqDecisionTree]),
[NodeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[NodeDescription] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ParentTreeID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreeCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreeUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DecisionTree] ADD CONSTRAINT [pkDecisionTree] PRIMARY KEY CLUSTERED  ([TreeNode]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDecisionTreeModifiedByUserID] ON [dbo].[DecisionTree] ([ModifiedByUserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDecisionTreeParentTreeIDIncludes] ON [dbo].[DecisionTree] ([ParentTreeID]) INCLUDE ([ModifiedByUserID], [NodeDescription], [NodeName], [TreeID], [TreeLevel], [TreeNode], [TreePath]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUqDecisionTreeTreeIDIncludes] ON [dbo].[DecisionTree] ([TreeID]) INCLUDE ([ModifiedByUserID], [NodeDescription], [NodeName], [ParentTreeID], [TreeLevel], [TreeNode], [TreePath]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DecisionTree] ADD CONSTRAINT [fkDecisionTreeModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[DecisionTree] ADD CONSTRAINT [fkDecisionTreeParentTreeIDDecisionTreeTreeID] FOREIGN KEY ([ParentTreeID]) REFERENCES [dbo].[DecisionTree] ([TreeID])
GO
