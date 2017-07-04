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
	SELECT CONVERT(BIT,IIF(COUNT(x.[ClaimID]) <= 1, 1, 0))
	FROM 
	(
		SELECT [p].[ClaimID]
		FROM   [dbo].[Prescription] AS [p]
			   INNER JOIN [dbo].[PrescriptionNoteMapping] AS [pnm] ON [pnm].[PrescriptionID] = [p].[PrescriptionID]
			   INNER JOIN [dbo].[PrescriptionNote] AS [pn] ON [pn].[PrescriptionNoteID] = [pnm].[PrescriptionNoteID]
		WHERE  [pn].[PrescriptionNoteID] = @PrescriptionNoteID
		GROUP BY [p].[ClaimID]
	) AS x
)
END
GO
