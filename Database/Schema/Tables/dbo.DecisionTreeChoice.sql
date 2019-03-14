CREATE TABLE [dbo].[DecisionTreeChoice]
(
[DecisionTreeChoiceID] [int] NOT NULL IDENTITY(1, 1),
[ClaimID] [int] NULL,
[LeafTreeID] [int] NOT NULL,
[ModifiedByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreeChoiceCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreeChoiceUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DecisionTreeChoice] ADD CONSTRAINT [pkDecisionTreeChoice] PRIMARY KEY CLUSTERED  ([DecisionTreeChoiceID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DecisionTreeChoice] ADD CONSTRAINT [fkDecisionTreeChoiceClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[DecisionTreeChoice] ADD CONSTRAINT [fkDecisionTreeChoiceLeafTreeIDDecisionTreeTreeID] FOREIGN KEY ([LeafTreeID]) REFERENCES [dbo].[DecisionTree] ([TreeID])
GO
ALTER TABLE [dbo].[DecisionTreeChoice] ADD CONSTRAINT [fkDecisionTreeChoiceModifiedByUserIDAspNetUsersID] FOREIGN KEY ([ModifiedByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
