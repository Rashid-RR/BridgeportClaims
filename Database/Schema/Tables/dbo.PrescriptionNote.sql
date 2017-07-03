CREATE TABLE [dbo].[PrescriptionNote]
(
[PrescriptionNoteID] [int] NOT NULL IDENTITY(1, 1),
[PrescriptionNoteTypeID] [int] NOT NULL,
[NoteText] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EnteredByUserID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ClaimID] [int] NOT NULL,
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
CREATE NONCLUSTERED INDEX [idxPrescriptionNoteClaimIDPrescriptionNoteTypeIDEnteredByUserIDIncludeAll] ON [dbo].[PrescriptionNote] ([ClaimID], [PrescriptionNoteTypeID], [EnteredByUserID]) INCLUDE ([CreatedOn], [NoteText], [PrescriptionNoteID], [UpdatedOn]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fkPrescriptionNoteClaimIDClaimClaimID] FOREIGN KEY ([ClaimID]) REFERENCES [dbo].[Claim] ([ClaimID])
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fkPrescriptionNoteEnteredByUserIDAspNetUsersID] FOREIGN KEY ([EnteredByUserID]) REFERENCES [dbo].[AspNetUsers] ([ID])
GO
ALTER TABLE [dbo].[PrescriptionNote] ADD CONSTRAINT [fkPrescriptionNotePrescriptionNoteTypeIDPrescriptionNoteTypePrescriptionNoteTypeID] FOREIGN KEY ([PrescriptionNoteTypeID]) REFERENCES [dbo].[PrescriptionNoteType] ([PrescriptionNoteTypeID])
GO
