CREATE TABLE [dbo].[PrescriptionNoteType]
(
[PrescriptionNoteTypeID] [int] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteTypeCreatedOn] DEFAULT (sysdatetime()),
[UpdatedOn] [datetime2] NOT NULL CONSTRAINT [dfPrescriptionNoteTypeUpdatedOn] DEFAULT (sysdatetime()),
[DataVersion] [timestamp] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PrescriptionNoteType] ADD CONSTRAINT [pkPrescriptionNoteType] PRIMARY KEY CLUSTERED  ([PrescriptionNoteTypeID]) WITH (FILLFACTOR=95) ON [PRIMARY]
GO
