CREATE TABLE [dbo].[EpisodeNote]
(
[EpisodeNoteID] [int] NOT NULL IDENTITY(1, 1),
[EpisodeID] [int] NOT NULL,
[NoteText] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[WrittenByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Created] [date] NOT NULL,
[DecisionTreeChoiceID] [int] NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeNoteCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfEpisodeNoteUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[EpisodeNote] ADD CONSTRAINT [pkEpisodeNote] PRIMARY KEY CLUSTERED  ([EpisodeNoteID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxEpisodeNoteEpisodeIDWrittenByUserIDIncludes] ON [dbo].[EpisodeNote] ([EpisodeID], [WrittenByUserID]) INCLUDE ([Created], [NoteText]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EpisodeNote] ADD CONSTRAINT [fkEpisodeNoteDecisionTreeChoiceIDDecisionTreeChoiceDecisionTreeChoiceID] FOREIGN KEY ([DecisionTreeChoiceID]) REFERENCES [dbo].[DecisionTreeChoice] ([DecisionTreeChoiceID])
GO
ALTER TABLE [dbo].[EpisodeNote] ADD CONSTRAINT [fkEpisodeNoteEpisodeIDEpisodeEpisodeID] FOREIGN KEY ([EpisodeID]) REFERENCES [dbo].[Episode] ([EpisodeID]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EpisodeNote] ADD CONSTRAINT [fkEpisodeNoteWrittenByUserIDAspNetUsersID] FOREIGN KEY ([WrittenByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
