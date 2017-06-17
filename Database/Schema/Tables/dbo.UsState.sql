CREATE TABLE [dbo].[UsState]
(
[StateID] [int] NOT NULL IDENTITY(1, 1),
[StateCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateName] [varchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsTerritory] [bit] NOT NULL CONSTRAINT [dfUSState] DEFAULT ((0))
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[UsState] ADD CONSTRAINT [pkUsState] PRIMARY KEY CLUSTERED  ([StateID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxUsStateStateNameStateCodeStateIDIncludeIsTerritory] ON [dbo].[UsState] ([StateName], [StateCode], [StateID]) INCLUDE ([IsTerritory]) WITH (FILLFACTOR=100, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
