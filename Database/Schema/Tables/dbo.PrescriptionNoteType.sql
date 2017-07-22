CREATE TABLE [dbo].[PrescriptionNoteType]
(
[PrescriptionNoteTypeID] [int] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteTypeCreatedOnUTC] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteTypeUpdatedOnUTC] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[PrescriptionNoteType] ADD CONSTRAINT [pkPrescriptionNoteType] PRIMARY KEY CLUSTERED  ([PrescriptionNoteTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUqPrescriptionNoteTypeCodeIncludeTypeName] ON [dbo].[PrescriptionNoteType] ([Code]) INCLUDE ([TypeName]) ON [PRIMARY]
GO
