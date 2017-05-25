CREATE TABLE [dbo].[UsState]
(
[StateID] [int] NOT NULL IDENTITY(1, 1),
[StateCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateName] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsTerritory] [bit] NOT NULL CONSTRAINT [dfUSState] DEFAULT ((0))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsState] ADD CONSTRAINT [pkUsState] PRIMARY KEY CLUSTERED  ([StateID]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxUsStateStateNameStateIDInclude] ON [dbo].[UsState] ([StateName], [StateID]) INCLUDE ([IsTerritory]) ON [PRIMARY]
GO
