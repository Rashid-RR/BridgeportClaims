SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/25/2017
	Description:	Inserts or Updates a Claim Note.
	Sample Execute:
					EXEC dbo.uspAddOrUpdateClaimNote
*/
CREATE PROC [claims].[uspAddOrUpdateClaimNote]
(
	@ClaimID INT,
	@NoteText VARCHAR(MAX),
	@EnteredByUserID NVARCHAR(128),
	@NoteTypeID INT
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @UTCNow DATETIME2 = dtme.udfGetUtcDate();
	
	IF EXISTS
		(
			SELECT *
			FROM   [dbo].[ClaimNote] AS [cn]
			WHERE  [cn].[ClaimID] = @ClaimID
		)
		BEGIN
			UPDATE [c] SET [c].[NoteText] = @NoteText,
						   [c].[EnteredByUserID] = @EnteredByUserID,
						   [c].[ClaimNoteTypeID] = @NoteTypeID,
						   [c].[UpdatedOnUTC] = @UTCNow
			FROM [dbo].[ClaimNote] AS [c]
			WHERE [c].[ClaimID] = @ClaimID
		END
	ELSE
		BEGIN
			INSERT [dbo].[ClaimNote] ([ClaimID],[ClaimNoteTypeID],[NoteText],[EnteredByUserID],[CreatedOnUTC],[UpdatedOnUTC])
			VALUES (@ClaimID, @NoteTypeID, @NoteText, @EnteredByUserID, @UTCNow, @UTCNow);
		END
END
GO
