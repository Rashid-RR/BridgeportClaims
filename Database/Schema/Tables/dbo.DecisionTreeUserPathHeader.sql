CREATE TABLE [dbo].[DecisionTreeUserPathHeader]
(
[DecisionTreeUserPathHeaderID] [int] NOT NULL IDENTITY(1, 1),
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TreeRootID] [int] NOT NULL,
[ClaimID] [int] NOT NULL,
[SessionID] [uniqueidentifier] NOT NULL CONSTRAINT [dfDecisionTreeUserPathHeaderSessionID] DEFAULT (newid()),
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreeUserPathHeaderCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreeUserPathHeaderUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DecisionTreeUserPathHeader] ADD CONSTRAINT [ckDecisionTreeUserPathHeaderTreeRootMustBeOnLevelOne] CHECK (([dbo].[udfTreeRootOnLevelOne]([TreeRootID])=(1)))
GO
ALTER TABLE [dbo].[DecisionTreeUserPathHeader] ADD CONSTRAINT [pkDecisionTreeUserPathHeader] PRIMARY KEY CLUSTERED  ([DecisionTreeUserPathHeaderID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DecisionTreeUserPathHeader] ADD CONSTRAINT [idxUqDecisionTreeUserPathHeaderSessionID] UNIQUE NONCLUSTERED  ([SessionID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DecisionTreeUserPathHeader] ADD CONSTRAINT [FK__DecisionT__TreeR__21F5FC7F] FOREIGN KEY ([TreeRootID]) REFERENCES [dbo].[DecisionTree] ([TreeID])
GO
ALTER TABLE [dbo].[DecisionTreeUserPathHeader] ADD CONSTRAINT [fkDecisionTreeUserPathHeaderClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[DecisionTreeUserPathHeader] ADD CONSTRAINT [fkDecisionTreeUserPathHeaderUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
