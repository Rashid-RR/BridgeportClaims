CREATE TABLE [dbo].[PrescriptionNote]
(
[PrescriptionNoteID] [int] NOT NULL IDENTITY(1, 1),
[PrescriptionID] [int] NOT NULL,
[PrescriptionNoteTypeID] [int] NOT NULL,
[NoteText] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EnteredByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [pkPrescriptionNote] PRIMARY KEY CLUSTERED  ([PrescriptionNoteID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fkPrescriptionNoteEnteredByUserIDAspNetUsersID] FOREIGN KEY ([EnteredByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fkPrescriptionNotePrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fkPrescriptionNotePrescriptionNoteTypeIDPrescriptionNoteTypePrescriptionNoteTypeID] FOREIGN KEY ([PrescriptionNoteTypeID]) REFERENCES [dbo].[PrescriptionNoteType] ([PrescriptionNoteTypeID])
GO
