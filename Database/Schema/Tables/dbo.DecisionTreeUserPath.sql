CREATE TABLE [dbo].[DecisionTreeUserPath]
(
[DecisionTreeUserPathID] [int] NOT NULL IDENTITY(1, 1),
[SessionID] [uniqueidentifier] NOT NULL,
[ParentTreeID] [int] NOT NULL,
[SelectedTreeID] [int] NOT NULL,
[UserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreeUserPathCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDecisionTreeUserPathUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[DecisionTreeUserPath] ADD CONSTRAINT [pkDecisionTreeUserPath] PRIMARY KEY CLUSTERED  ([DecisionTreeUserPathID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DecisionTreeUserPath] ADD CONSTRAINT [fkDecisionTreeUserPathSessionIDDecisionTreeUserPathHeaderDecisionTreeUserPathHeaderID] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[DecisionTreeUserPathHeader] ([SessionID])
GO
ALTER TABLE [dbo].[DecisionTreeUserPath] ADD CONSTRAINT [fkDecisionTreeUserPathUserIDAspNetUsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[DecisionTreeUserPath] ADD CONSTRAINT [FK__DecisionT__Paren__4EC8A2F6] FOREIGN KEY ([ParentTreeID]) REFERENCES [dbo].[DecisionTree] ([TreeID])
GO
ALTER TABLE [dbo].[DecisionTreeUserPath] ADD CONSTRAINT [FK__DecisionT__Selec__4FBCC72F] FOREIGN KEY ([SelectedTreeID]) REFERENCES [dbo].[DecisionTree] ([TreeID])
GO
