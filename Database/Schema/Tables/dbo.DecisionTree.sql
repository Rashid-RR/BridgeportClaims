CREATE TABLE [dbo].[DecisionTree]
(
[TreeNode] [sys].[hierarchyid] NOT NULL,
[TreeLevel] AS ([TreeNode].[GetLevel]()),
[TreeID] [int] NOT NULL,
[NodeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[NodeDescription] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DecisionTree] ADD CONSTRAINT [pkDecisionTree] PRIMARY KEY CLUSTERED  ([TreeNode]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DecisionTree] ADD CONSTRAINT [idxUqDecisionTreeNodeName] UNIQUE NONCLUSTERED  ([NodeName]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
