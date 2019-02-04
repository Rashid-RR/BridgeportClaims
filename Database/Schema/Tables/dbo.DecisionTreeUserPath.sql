CREATE TABLE [dbo].[DecisionTreeUserPath]
(
[DecisionTreeUserPathID] [int] NOT NULL IDENTITY(1, 1),
[SessionID] [uniqueidentifier] NOT NULL,
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
ALTER TABLE [dbo].[DecisionTreeUserPath] ADD CONSTRAINT [FK__DecisionT__UserI__2C738AF2] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[DecisionTreeUserPath] ADD CONSTRAINT [FK__DecisionT__Sessi__2B7F66B9] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[DecisionTreeUserPathHeader] ([SessionID])
GO
