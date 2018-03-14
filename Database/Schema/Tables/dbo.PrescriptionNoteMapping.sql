CREATE TABLE [dbo].[PrescriptionNoteMapping]
(
[PrescriptionID] [int] NOT NULL,
[PrescriptionNoteID] [int] NOT NULL,
[CreatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteMappingCreatedOnUTC] DEFAULT (sysutcdatetime()),
[UpdatedOnUTC] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteMappingUpdatedOnUTC] DEFAULT (sysutcdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[PrescriptionNoteMapping] ADD CONSTRAINT [pkPrescriptionNoteMapping] PRIMARY KEY CLUSTERED  ([PrescriptionID], [PrescriptionNoteID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionNoteMappingIncludeAll] ON [dbo].[PrescriptionNoteMapping] ([PrescriptionID], [PrescriptionNoteID]) INCLUDE ([CreatedOnUTC], [UpdatedOnUTC]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionNoteMappingPrescriptionNoteID] ON [dbo].[PrescriptionNoteMapping] ([PrescriptionNoteID]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionNoteMapping] ADD CONSTRAINT [fkPrescriptionNoteMappingPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
ALTER TABLE [dbo].[PrescriptionNoteMapping] ADD CONSTRAINT [fkPrescriptionNoteMappingPrescriptionNoteIDPrescriptionNotePrescriptionNoteID] FOREIGN KEY ([PrescriptionNoteID]) REFERENCES [dbo].[PrescriptionNote] ([PrescriptionNoteID])
GO
