CREATE TABLE [dbo].[DecisionTreePath]
(
[DecisionTreePathID] [int] NOT NULL IDENTITY(1, 1),
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ClaimID] [int] NULL,
[EpisodeID] [int] NULL,
[SessionID] [uniqueidentifier] NOT NULL,
[RootTreeNode] [sys].[hierarchyid] NOT NULL,
[LeafTreeNode] [sys].[hierarchyid] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreePathCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreePathUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DecisionTreePath] ADD CONSTRAINT [pkDecisionTreePath] PRIMARY KEY CLUSTERED  ([DecisionTreePathID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDecisionTreePathClaimID] ON [dbo].[DecisionTreePath] ([ClaimID]) WHERE ([ClaimID] IS NOT NULL) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDecisionTreePathEpisodeID] ON [dbo].[DecisionTreePath] ([EpisodeID]) WHERE ([EpisodeID] IS NOT NULL) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDecisionTreePathLeafTreeNodeIncludes] ON [dbo].[DecisionTreePath] ([LeafTreeNode]) INCLUDE ([ClaimID], [EpisodeID], [RootTreeNode], [SessionID], [UserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDecisionTreePathRootTreeNodeIncludes] ON [dbo].[DecisionTreePath] ([RootTreeNode]) INCLUDE ([ClaimID], [EpisodeID], [LeafTreeNode], [SessionID], [UserID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DecisionTreePath] ADD CONSTRAINT [idxUqDecisionTreePathSessionID] UNIQUE NONCLUSTERED  ([SessionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDecisionTreePathUserIDIncludeSessionID] ON [dbo].[DecisionTreePath] ([UserID]) INCLUDE ([SessionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DecisionTreePath] ADD CONSTRAINT [fkDecisionTreePathClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[DecisionTreePath] ADD CONSTRAINT [fkDecisionTreePathEpisodeIDEpisodeEpisodeID] FOREIGN KEY ([EpisodeID]) REFERENCES [dbo].[Episode] ([EpisodeID])
GO
ALTER TABLE [dbo].[DecisionTreePath] ADD CONSTRAINT [fkDecisionTreePathLeafTreeNodeDecisionTreeTreeNode] FOREIGN KEY ([LeafTreeNode]) REFERENCES [dbo].[DecisionTree] ([TreeNode])
GO
ALTER TABLE [dbo].[DecisionTreePath] ADD CONSTRAINT [fkDecisionTreePathRootTreeNodeDecisionTreeTreeNode] FOREIGN KEY ([RootTreeNode]) REFERENCES [dbo].[DecisionTree] ([TreeNode])
GO
ALTER TABLE [dbo].[DecisionTreePath] ADD CONSTRAINT [fkDecisionTreePathUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
