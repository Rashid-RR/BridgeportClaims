SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/3/2017
	Description:	Adds or Inserts a Prescription Note and modifies
					the associated Prescription records accordingly
	Sample Execute:
					EXEC dbo.uspSavePrescriptionNote
*/
CREATE PROC [dbo].[uspSavePrescriptionNote]
(
	@ClaimID INT,
	@PrescriptionNoteTypeID INT,
	@NoteText VARCHAR(8000),
	@FollowUpDate DATE,
	@EnteredByUserID NVARCHAR(128),
	@PrescriptionNoteID INT = NULL,
	@Prescription [dbo].[udtID] READONLY
)
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @UTCNow DATETIME2 = dtme.udfGetUtcDate(),
			@OutputPrescriptionNoteID INTEGER;
	DECLARE @LocalNowDate DATEtime = dtme.udfGetLocalDate();

	DECLARE @PrescriptionNoteMergeChangeResult TABLE (ChangeType VARCHAR(10) NOT NULL, PrescriptionNoteID INTEGER NOT NULL)

	-- Testing
	/*BEGIN TRAN; -- COMMIT -- ROLLBACK
	DECLARE @ClaimID INT = 95
		  , @PrescriptionNoteTypeID INT = 3
		  , @NoteText VARCHAR(8000) = 'I am a baffled'
		  , @EnteredByUserID NVARCHAR(128) = (SELECT TOP (1) u.ID FROM dbo.AspNetUsers AS u)
		  , @PrescriptionNoteID INT --= -4
		  , @FollowUpDate DATE = '10/19/2017'
	DECLARE @Prescription dbo.udtID
	INSERT @Prescription ( [ID] )
	VALUES ( 993),(994),(995),(996),(1013),(1000)*/
	
	IF @ClaimID IS NULL OR @PrescriptionNoteTypeID IS NULL OR @NoteText IS NULL OR @EnteredByUserID IS NULL
		BEGIN
			RAISERROR(N'Error. One or more required arguements were not supplied', 16, 1) WITH NOWAIT
			RETURN -1;
		END

	-- Upsert PrescriptionNote
	MERGE [dbo].[PrescriptionNote] AS tgt
	USING (SELECT @PrescriptionNoteID PrescriptionNoteID,
				@PrescriptionNoteTypeID PrescriptionNoteTypeID,
				@NoteText NoteText,
				@EnteredByUserID EnteredByUserID) AS src 
	       ON [tgt].[PrescriptionNoteID] = [src].[PrescriptionNoteID]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([PrescriptionNoteTypeID],[NoteText],[EnteredByUserID])
		VALUES (src.[PrescriptionNoteTypeID],src.[NoteText],src.[EnteredByUserID])
	WHEN MATCHED THEN
		UPDATE SET [tgt].[PrescriptionNoteTypeID] = [src].[PrescriptionNoteTypeID],
					[tgt].[NoteText] = [src].[NoteText],
					[tgt].[EnteredByUserID] = [src].[EnteredByUserID],
					[tgt].[UpdatedOnUTC] = @UTCNow
	OUTPUT $action, Inserted.[PrescriptionNoteID] INTO @PrescriptionNoteMergeChangeResult ([ChangeType], [PrescriptionNoteID]);

	IF NOT EXISTS (SELECT * FROM @PrescriptionNoteMergeChangeResult AS [pnmcr])
		BEGIN
			RAISERROR(N'Error. No new Prescription Note was Updated or Inserted into the Prescription', 16, 1) WITH NOWAIT
			RETURN -1;
		END
	
	SELECT @OutputPrescriptionNoteID = [x].[PrescriptionNoteID]
	FROM   @PrescriptionNoteMergeChangeResult AS x

	-- Dupsert PrescriptionNoteMapping
	MERGE [dbo].[PrescriptionNoteMapping] AS tgt
	USING (SELECT [p].[ID] AS PrescriptionID, @OutputPrescriptionNoteID PrescriptionNoteID
		   FROM @Prescription AS [p]) AS src
	ON [tgt].[PrescriptionID] = [src].[PrescriptionID]
		AND [tgt].[PrescriptionNoteID] = [src].[PrescriptionNoteID]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([PrescriptionID], [PrescriptionNoteID])
		VALUES ([src].[PrescriptionID], [src].[PrescriptionNoteID])
	WHEN MATCHED THEN
		UPDATE SET [tgt].[UpdatedOnUTC] = @UTCNow
	WHEN NOT MATCHED BY SOURCE AND [tgt].[PrescriptionNoteID] = @OutputPrescriptionNoteID
	 THEN DELETE;

	IF @FollowUpDate IS NOT NULL
		BEGIN
			MERGE dbo.Diary AS tgt
			USING (SELECT @OutputPrescriptionNoteID PrescriptionNoteID) AS src
				ON tgt.PrescriptionNoteID = src.PrescriptionNoteID
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (AssignedToUserID, PrescriptionNoteID, FollowUpDate, DateResolved, CreatedDate, CreatedOnUTC, UpdatedOnUTC)
				VALUES (@EnteredByUserID, src.PrescriptionNoteID, @FollowUpDate, NULL, @LocalNowDate, @UTCNow, @UTCNow)
			WHEN MATCHED THEN
				UPDATE SET tgt.AssignedToUserID = @EnteredByUserID, tgt.UpdatedOnUTC = @UTCNow, tgt.FollowUpDate = @FollowUpDate,
					tgt.CreatedDate = @LocalNowDate;
		END
END
GO
