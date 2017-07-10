SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
		Author:			Jordan Gurney
		Create Date:	7/3/2017
		Description:	Function for a Check Constraint that does not allow
						a Prescription Note to be associated to any Prescriptions that are 
						children of different Claims.
		Sample Execute:
						SELECT dbo.udfCheckPrescriptionNoteClaimID(3)
	*/
	CREATE FUNCTION [dbo].[udfCheckPrescriptionNoteClaimID] (@PrescriptionNoteID INT)
	RETURNS BIT
	WITH SCHEMABINDING
	AS BEGIN RETURN
	(
		SELECT CONVERT(BIT,IIF(COUNT(x.[ClaimID]) = 1 OR @PrescriptionNoteID IS NULL, 1, 0))
		FROM 
		(
			SELECT [p].[ClaimID]
			FROM   [dbo].[PrescriptionNote] AS [pn]
				   INNER JOIN [dbo].[PrescriptionNoteMapping] AS [pnm] ON [pnm].[PrescriptionNoteID] = [pn].[PrescriptionNoteID]
				   INNER JOIN [dbo].[Prescription] AS [p] ON [p].[PrescriptionID] = [pnm].[PrescriptionID]
			WHERE  [pn].[PrescriptionNoteID] = @PrescriptionNoteID
			GROUP BY [p].[ClaimID]
		) AS x
	)
	END
GO
