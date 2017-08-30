SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/2/2017
	Description:	Executes the SELECT statement for Prescriptions. The goal behind putting 
					this in a proc is to allow malleability to change it without a managed
					code deployment.
	Modified:		8/24/2017 by Jordan Gurney. Added dynamic sorting, sort direction, and pagination.
	Sample Execute:
					EXEC dbo.uspGetPrescriptionDataForClaim 775, 'PrescriptionId', 'asc', 1, 5
*/
CREATE PROC [dbo].[uspGetPrescriptionDataForClaim]
(
	@ClaimID INTEGER,
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER
)
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
	ORDER BY CASE WHEN @SortColumn = 'PrescriptionId' AND @SortDirection = 'ASC'
				THEN [p].[PrescriptionID] END ASC,
			 CASE WHEN @SortColumn = 'PrescriptionId' AND @SortDirection = 'DESC'
				THEN [p].[PrescriptionID] END DESC,
			 CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'ASC'
				THEN [p].[DateFilled] END ASC,
			 CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'DESC'
				THEN [p].[DateFilled] END DESC,
			 CASE WHEN @SortColumn = 'AmountPaid' AND @SortDirection = 'ASC'
				THEN [pm].[AmountPaid] END ASC,
			 CASE WHEN @SortColumn = 'AmountPaid' AND @SortDirection = 'DESC'
				THEN [pm].[AmountPaid] END DESC,
			 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
				THEN [p].[RxNumber] END ASC,
			 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
				THEN [p].[RxNumber] END DESC,
			 CASE WHEN @SortColumn = 'LabelName' AND @SortDirection = 'ASC'
				THEN [p].[LabelName] END ASC,
			 CASE WHEN @SortColumn = 'LabelName' AND @SortDirection = 'DESC'
				THEN [p].[LabelName] END DESC,
			 CASE WHEN @SortColumn = 'BillTo' AND @SortDirection = 'ASC'
				THEN [py].[BillToName] END ASC,
			 CASE WHEN @SortColumn = 'BillTo' AND @SortDirection = 'DESC'
				THEN [py].[BillToName] END DESC,
			 CASE WHEN @SortColumn = 'InvoiceAmount' AND @SortDirection = 'ASC'
				THEN [p].[BilledAmount] END ASC,
			 CASE WHEN @SortColumn = 'InvoiceAmount' AND @SortDirection = 'DESC'
				THEN [p].[BilledAmount] END DESC,
			 CASE WHEN @SortColumn = 'InvoiceDate' AND @SortDirection = 'ASC'
				THEN [i].[InvoiceDate] END ASC,
			 CASE WHEN @SortColumn = 'InvoiceDate' AND @SortDirection = 'DESC'
				THEN [i].[InvoiceDate] END DESC,
			 CASE WHEN @SortColumn = 'InvoiceNumber' AND @SortDirection = 'ASC'
				THEN [i].[InvoiceNumber] END ASC,
			 CASE WHEN @SortColumn = 'InvoiceNumber' AND @SortDirection = 'DESC'
				THEN [i].[InvoiceNumber] END DESC,
			 CASE WHEN @SortColumn = 'Outstanding' AND @SortDirection = 'ASC'
				THEN [p].[BilledAmount] - [pm].[AmountPaid] END ASC,
			 CASE WHEN @SortColumn = 'Outstanding' AND @SortDirection = 'DESC'
				THEN [p].[BilledAmount] - [pm].[AmountPaid] END DESC,
			 CASE WHEN @SortColumn = 'NoteCount' AND @SortDirection = 'ASC'
				THEN (SELECT COUNT(*)
					  FROM   [dbo].[PrescriptionNoteMapping] AS [pnm]
					  WHERE  [pnm].[PrescriptionID] = [p].[PrescriptionID]) END ASC,
			 CASE WHEN @SortColumn = 'NoteCount' AND @SortDirection = 'DESC'
				THEN (SELECT COUNT(*)
					  FROM   [dbo].[PrescriptionNoteMapping] AS [pnm]
					  WHERE  [pnm].[PrescriptionID] = [p].[PrescriptionID]) END DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END

GO
