SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       9/13/2018
 Description:       Gets Prescription Notes.
 Example Execute:
                    EXECUTE [dbo].[uspGetPrescriptionNotes] 5000
 =============================================
*/
CREATE PROC [dbo].[uspGetPrescriptionNotes]
(
	@PrescriptionID INT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	SELECT    [p].[ClaimID] ClaimId
			, [p].[PrescriptionNoteId] PrescriptionNoteId
			, [p].[NoteUpdatedOn] [RxDate]
			, [p].[PrescriptionNoteType] [Type]
			, [p].[NoteAuthor] EnteredBy
			, [p].[NoteText] [Note]
			, [p].[NoteUpdatedOn] NoteUpdatedOn
	FROM     [dbo].[vwPrescriptionNote] AS [p] WITH ( NOEXPAND )
	WHERE    [p].[PrescriptionID] = @PrescriptionID
	ORDER BY [p].[NoteUpdatedOn] ASC
END
GO
