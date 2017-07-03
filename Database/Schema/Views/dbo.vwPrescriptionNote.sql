SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwPrescriptionNote]
WITH SCHEMABINDING
AS
	SELECT [p].[PrescriptionID], p.[RxNumber], p.[DateFilled], p.[LabelName],
		[pn].[PrescriptionNoteID], [pnt].[TypeName] PrescriptionNoteType, [pn].[NoteText], 
		[u].[FirstName] + ' ' + [u].[LastName] NoteAuthor,
		 [pn].[CreatedOn] NoteCreatedOn, [pn].[UpdatedOn] NoteUpdatedOn
	FROM [dbo].[Prescription] AS [p]
	INNER JOIN [dbo].[PrescriptionNoteMapping] AS [pnm] ON [pnm].[PrescriptionID] = [p].[PrescriptionID]
	INNER JOIN [dbo].[PrescriptionNote] AS [pn] INNER JOIN [dbo].[PrescriptionNoteType] AS [pnt] ON [pnt].[PrescriptionNoteTypeID] = [pn].[PrescriptionNoteTypeID]
		ON [pn].[PrescriptionNoteID] = [pnm].[PrescriptionNoteID]
	INNER JOIN [dbo].[AspNetUsers] AS [u] ON [u].[ID] = [pn].[EnteredByUserID]
GO
CREATE UNIQUE CLUSTERED INDEX [pkVwPrescriptionNote] ON [dbo].[vwPrescriptionNote] ([PrescriptionID], [PrescriptionNoteID]) ON [PRIMARY]
GO
