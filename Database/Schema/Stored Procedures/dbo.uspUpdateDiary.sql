SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/9/2017
	Description:	Updates a Diary entry to save today's date as the Resolved Date.
	Sample Execute:
					EXEC dbo.uspUpdateDiary @PrescriptionNoteID = 1
*/
CREATE PROC [dbo].[uspUpdateDiary] @PrescriptionNoteID INT
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @LocalDate DATE = CAST(dtme.udfGetLocalDate() AS DATE)
	DECLARE @UTCNow DATETIME2 = SYSUTCDATETIME()
	UPDATE d SET d.DateResolved = @LocalDate, d.UpdatedOnUTC = @UTCNow
	FROM   dbo.Diary AS d INNER JOIN dbo.PrescriptionNote AS pn ON pn.PrescriptionNoteID = d.PrescriptionNoteID
	WHERE  pn.PrescriptionNoteID = @PrescriptionNoteID
		   AND d.DateResolved IS NULL
END
GO
