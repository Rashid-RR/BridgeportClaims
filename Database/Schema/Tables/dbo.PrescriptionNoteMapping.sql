CREATE TABLE [dbo].[PrescriptionNoteMapping]
(
[PrescriptionID] [int] NOT NULL,
[PrescriptionNoteID] [int] NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteMappingCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteMappingUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[PrescriptionNoteMapping] ADD CONSTRAINT [pkPrescriptionNoteMapping] PRIMARY KEY CLUSTERED  ([PrescriptionID], [PrescriptionNoteID]) WITH (FILLFACTOR=95, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [idxPrescriptionNoteMappingIncludeAll] ON [dbo].[PrescriptionNoteMapping] ([PrescriptionID], [PrescriptionNoteID]) INCLUDE ([CreatedOn], [UpdatedOn]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionNoteMapping] ADD CONSTRAINT [fkPrescriptionNoteMappingPrescriptionIDPrescriptionPrescriptionID] FOREIGN KEY ([PrescriptionID]) REFERENCES [dbo].[Prescription] ([PrescriptionID])
GO
ALTER TABLE [dbo].[PrescriptionNoteMapping] ADD CONSTRAINT [fkPrescriptionNoteMappingPrescriptionNoteIDPrescriptionNotePrescriptionNoteID] FOREIGN KEY ([PrescriptionNoteID]) REFERENCES [dbo].[PrescriptionNote] ([PrescriptionNoteID])
GO
