SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/11/2018
 Description:       Gets Scripts Notes 
 Example Execute:
                    EXECUTE [claims].[uspGetPrescriptionNotes] 775
 =============================================
*/
CREATE PROC [claims].[uspGetPrescriptionNotes] (@ClaimID INT)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        SELECT  DISTINCT
                [ClaimId] = a.ClaimID
               ,[PrescriptionNoteId] = a.PrescriptionNoteID
               ,RxDate = a.DateFilled
               ,a.RxNumber
               ,[Type] = a.PrescriptionNoteType
               ,[EnteredBy] = a.NoteAuthor
               ,[Note] = a.NoteText
               ,[NoteUpdatedOn] = a.NoteUpdatedOn
               ,HasDiaryEntry = CAST(CASE WHEN d.DiaryID IS NOT NULL THEN 1 ELSE 0 END AS BIT)
               ,DiaryId = d.DiaryID
        FROM    dbo.vwPrescriptionNote AS a WITH (NOEXPAND)
                LEFT JOIN dbo.Diary AS d ON d.PrescriptionNoteID = a.PrescriptionNoteID AND d.DateResolved IS NULL
        WHERE   a.ClaimID = @ClaimID
        ORDER BY a.DateFilled DESC, a.RxNumber ASC;
    END
GO
