SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/11/2017
	Description:	Creates the Prescription Note View Definition. This is needed for 
					routine schema changes that are blocked by the SCHEMABINDING of this VIEW.
	Sample Execute:
					EXEC dbo.uspCreateIndexedPrescriptionNoteView
*/
CREATE PROC [dbo].[uspCreateIndexedPrescriptionNoteView]
AS BEGIN
	SET NOCOUNT ON;
	SET ARITHABORT ON;
	SET CONCAT_NULL_YIELDS_NULL ON;
	SET QUOTED_IDENTIFIER ON;
	SET ANSI_NULLS ON;
	SET ANSI_PADDING ON;
	SET ANSI_WARNINGS ON;
	SET NUMERIC_ROUNDABORT OFF;
	DECLARE @SQLStatement NVARCHAR(4000)
	IF EXISTS (SELECT * FROM sys.[views] AS [v] WHERE [v].[name] = 'vwPrescriptionNote')
		BEGIN
			RAISERROR(N'The Prescription Note View already exists. Execute the statement: DROP VIEW dbo.vwPrescriptionNote', 10, 1) WITH NOWAIT
			RETURN
		END
	RAISERROR(N'Creating Prescription Note View...', 10, 1) WITH NOWAIT
	SELECT @SQLStatement = N'CREATE VIEW [dbo].[vwPrescriptionNote]
	WITH SCHEMABINDING
	AS	
		SELECT [p].[ClaimID], [p].[PrescriptionID], p.[RxNumber], p.[DateFilled], p.[LabelName],
			[pn].[PrescriptionNoteID], [pnt].[TypeName] PrescriptionNoteType, [pn].[NoteText], 
			[u].[FirstName] + '' ''+ [u].[LastName] NoteAuthor,
			[pn].[CreatedOnUTC] NoteCreatedOn, [pn].[UpdatedOnUTC] NoteUpdatedOn
		FROM [dbo].[Prescription] AS [p]
			INNER JOIN [dbo].[PrescriptionNoteMapping] AS [pnm] ON [pnm].[PrescriptionID] = [p].[PrescriptionID]
			INNER JOIN [dbo].[PrescriptionNote] AS [pn] INNER JOIN [dbo].[PrescriptionNoteType] AS [pnt] ON [pnt].[PrescriptionNoteTypeID] = [pn].[PrescriptionNoteTypeID]
				ON [pn].[PrescriptionNoteID] = [pnm].[PrescriptionNoteID]
			INNER JOIN [dbo].[AspNetUsers] AS [u] ON [u].[ID] = [pn].[EnteredByUserID]';
	-- Create View
	EXEC sys.sp_executesql @SQLStatement;

	SET @SQLStatement = N'CREATE UNIQUE CLUSTERED INDEX [idxUqClusVwPrescriptionNote] ON [dbo].[vwPrescriptionNote]
					(
						[PrescriptionID] ASC,
						[PrescriptionNoteID] ASC
					)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF,
					 ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, DATA_COMPRESSION = ROW)';
	EXEC sys.sp_executesql @SQLStatement;

	SET @SQLStatement =
	N'CREATE NONCLUSTERED INDEX [idxVwPrescriptionNoteClaimIDPrescriptionIDIncludeAll] ON [dbo].[vwPrescriptionNote]
	(
		[ClaimID] ASC,
		[PrescriptionID] ASC
	)
	INCLUDE ( 	[RxNumber],
		[DateFilled],
		[LabelName],
		[PrescriptionNoteID],
		[PrescriptionNoteType],
		[NoteText],
		[NoteAuthor],
		[NoteCreatedOn],
		[NoteUpdatedOn]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF,
		 ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, DATA_COMPRESSION = PAGE)';
	EXEC sys.sp_executesql @SQLStatement;
END

GO
