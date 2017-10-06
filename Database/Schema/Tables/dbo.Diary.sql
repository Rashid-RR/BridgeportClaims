CREATE TABLE [dbo].[Diary]
(
[DiaryID] [int] NOT NULL IDENTITY(1, 1),
[AssignedToUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PrescriptionNoteID] [int] NOT NULL,
[FollowUpDate] [date] NOT NULL,
[DateResolved] [date] NULL,
[CreatedDate] [date] NOT NULL CONSTRAINT [dfDiaryCreatedDate] DEFAULT (CONVERT([date],[dtme].[udfGetLocalDate](),(0))),
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDiaryCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfDiaryUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[Diary] ADD CONSTRAINT [pkDiary] PRIMARY KEY CLUSTERED  ([DiaryID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxDiaryPrescriptionNoteIDAssignedToUserIDIncludeAll] ON [dbo].[Diary] ([PrescriptionNoteID], [AssignedToUserID]) INCLUDE ([CreatedOnUTC], [DateResolved], [DiaryID], [FollowUpDate], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Diary] ADD CONSTRAINT [fkDiaryAssignedToUserIDAspNetUsersID] FOREIGN KEY ([AssignedToUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[Diary] ADD CONSTRAINT [fkDiaryPrescriptionNoteIDPrescriptionNotePrescriptionNoteID] FOREIGN KEY ([PrescriptionNoteID]) REFERENCES [dbo].[PrescriptionNote] ([PrescriptionNoteID])
GO
