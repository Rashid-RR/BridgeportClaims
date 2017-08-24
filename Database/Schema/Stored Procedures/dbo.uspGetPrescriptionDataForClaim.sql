SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/2/2017
	Description:	Executes the SELECT statement for Prescriptions. The goal behind putting this in a proc
					is to allow malleability to change it without a managed code deployment.
	Sample Execute:
					EXEC dbo.uspGetPrescriptionDataForClaim @ClaimID = 775
*/
CREATE PROC [dbo].[uspGetPrescriptionDataForClaim] @ClaimID INTEGER
AS BEGIN
	SET NOCOUNT ON;
	SELECT PrescriptionId = [p].[PrescriptionID]
		 , RxDate = [p].[DateFilled]
		 , AmountPaid = [pm].[AmountPaid]
		 , RxNumber = [p].[RxNumber]
		 , LabelName = [p].[LabelName]
		 , BillTo = [py].[BillToName]
		 , InvoiceAmount = [p].[BilledAmount]
		 , InvoiceDate = [i].[InvoiceDate]
		 , InvoiceNumber = [i].[InvoiceNumber]
		 , Outstanding = [p].[BilledAmount] - [pm].[AmountPaid]
		 , NoteCount = (   SELECT COUNT(*)
						   FROM   [dbo].[PrescriptionNoteMapping] AS [pnm]
						   WHERE  [pnm].[PrescriptionID] = [p].[PrescriptionID]
					   )
	FROM   [dbo].[Prescription] AS [p]
		   INNER JOIN [dbo].[Claim] AS [c] ON [c].[ClaimID] = [p].[ClaimID]
		   INNER JOIN [dbo].[Payor] AS [py] ON [py].[PayorID] = [c].[PayorID]
		   LEFT JOIN [dbo].[Invoice] AS [i] ON [i].[InvoiceID] = [p].[InvoiceID]
		   OUTER APPLY (
				SELECT SUM([ipm].[AmountPaid]) AmountPaid 
				FROM dbo.PrescriptionPayment AS [ipm] 
				WHERE [ipm].[PrescriptionID] = [p].[PrescriptionID]) AS pm
	WHERE  [p].[ClaimID] = @ClaimID
END
GO
