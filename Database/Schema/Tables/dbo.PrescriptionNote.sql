CREATE TABLE [dbo].[PrescriptionNote]
(
[PrescriptionNoteID] [int] NOT NULL IDENTITY(1, 1),
[PrescriptionNoteTypeID] [int] NOT NULL,
[NoteText] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EnteredByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [pkPrescriptionNote] PRIMARY KEY CLUSTERED  ([PrescriptionNoteID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fjk] FOREIGN KEY ([PrescriptionNoteTypeID]) REFERENCES [dbo].[PrescriptionNoteType] ([PrescriptionNoteTypeID])
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fkkkk] FOREIGN KEY ([EnteredByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
