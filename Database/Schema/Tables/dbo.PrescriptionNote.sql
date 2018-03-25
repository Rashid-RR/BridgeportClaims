CREATE TABLE [dbo].[PrescriptionNote]
(
[PrescriptionNoteID] [int] NOT NULL IDENTITY(1, 1),
[PrescriptionNoteTypeID] [int] NOT NULL,
[NoteText] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EnteredByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [pkPrescriptionNote] PRIMARY KEY CLUSTERED  ([PrescriptionNoteID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionNotePrescriptionNoteTypeIDEnteredByUserIDIncludeAll] ON [dbo].[PrescriptionNote] ([PrescriptionNoteTypeID], [EnteredByUserID]) INCLUDE ([CreatedOnUTC], [NoteText], [PrescriptionNoteID], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fkPrescriptionNoteEnteredByUserIDAspNetUsersID] FOREIGN KEY ([EnteredByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fkPrescriptionNotePrescriptionNoteTypeIDPrescriptionNoteTypePrescriptionNoteTypeID] FOREIGN KEY ([PrescriptionNoteTypeID]) REFERENCES [dbo].[PrescriptionNoteType] ([PrescriptionNoteTypeID])
GO
