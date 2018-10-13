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
					EXEC dbo.uspGetPrescriptionPayments 705, 'PrescriptionPaymentId', 'desc', 1, 20, 'CheckNumber', 'desc'
*/
CREATE PROC [dbo].[uspGetPrescriptionPayments]
(
	@ClaimID INTEGER,
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER,
	@SecondarySortColumn VARCHAR(50),
	@SecondarySortDirection VARCHAR(5)
)
AS BEGIN
	SET NOCOUNT ON;
	SELECT PrescriptionPaymentId = pp.PrescriptionPaymentID
		 , PrescriptionId = [p].[PrescriptionID]
		 , PostedDate = pp.DatePosted
		 , pp.CheckNumber
		 , CheckAmt = pp.AmountPaid
		 , RxNumber = [p].[RxNumber]
		 , RxDate = [p].[DateFilled]
		 , InvoiceNumber = [i].[InvoiceNumber]
		 , p.IsReversed
		 , d.DocumentID DocumentId
		 , d.[FileName]
		 , d.FileUrl
	FROM   [dbo].[Prescription] AS [p]
		   INNER JOIN dbo.PrescriptionPayment AS pp ON pp.PrescriptionID = p.PrescriptionID
		   LEFT JOIN dbo.Document AS d ON pp.DocumentID = d.DocumentID
		   LEFT JOIN dbo.Invoice AS i ON i.InvoiceID = p.InvoiceID
	WHERE  [p].[ClaimID] = @ClaimID
	ORDER BY CASE WHEN @SortColumn = 'PrescriptionPaymentId' AND @SortDirection = 'ASC'
				THEN pp.PrescriptionPaymentID END ASC,
			 CASE WHEN @SortColumn = 'PrescriptionPaymentId' AND @SortDirection = 'DESC'
				THEN pp.PrescriptionPaymentID END DESC,
			 CASE WHEN @SortColumn = 'PrescriptionId' AND @SortDirection = 'ASC'
				THEN [p].[PrescriptionID] END ASC,
			 CASE WHEN @SortColumn = 'PrescriptionId' AND @SortDirection = 'DESC'
				THEN [p].[PrescriptionID] END DESC,
			 CASE WHEN @SortColumn = 'PostedDate' AND @SortDirection = 'ASC'
				THEN pp.DatePosted END ASC,
			 CASE WHEN @SortColumn = 'PostedDate' AND @SortDirection = 'DESC'
				THEN pp.DatePosted END DESC,
			 CASE WHEN @SortColumn = 'CheckNumber' AND @SortDirection = 'ASC'
				THEN pp.CheckNumber END ASC,
			 CASE WHEN @SortColumn = 'CheckNumber' AND @SortDirection = 'DESC'
				THEN pp.CheckNumber END DESC,
			 CASE WHEN @SortColumn = 'CheckAmt' AND @SortDirection = 'ASC'
				THEN pp.AmountPaid END ASC,
			 CASE WHEN @SortColumn = 'CheckAmt' AND @SortDirection = 'DESC'
				THEN pp.AmountPaid END DESC,
			 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
				THEN [p].[RxNumber] END ASC,
			 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
				THEN [p].[RxNumber] END DESC,
			 CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'ASC'
				THEN [p].[DateFilled] END ASC,
			 CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'DESC'
				THEN [p].[DateFilled] END DESC,
			 CASE WHEN @SortColumn = 'InvoiceNumber' AND @SortDirection = 'ASC'
				THEN [i].[InvoiceNumber] END ASC,
			 CASE WHEN @SortColumn = 'InvoiceNumber' AND @SortDirection = 'DESC'
				THEN [i].[InvoiceNumber] END DESC,
			 CASE WHEN @SortColumn = 'IsReversed' AND @SortDirection = 'ASC'
				THEN p.IsReversed END ASC,
			 CASE WHEN @SortColumn = 'IsReversed' AND @SortDirection = 'DESC'
				THEN p.IsReversed END DESC,
		     CASE WHEN @SortColumn = 'DocumentId' AND @SortDirection = 'ASC'
				THEN d.DocumentID END ASC,
			 CASE WHEN @SortColumn = 'DocumentId' AND @SortDirection = 'DESC'
				THEN d.DocumentID END DESC,
			 CASE WHEN @SortColumn = 'FileName' AND @SortDirection = 'ASC'
				THEN [d].[FileName] END ASC,
			 CASE WHEN @SortColumn = 'FileName' AND @SortDirection = 'DESC'
				THEN [d].[FileName] END DESC,
			 CASE WHEN @SortColumn = 'FileUrl' AND @SortDirection = 'ASC'
				THEN [d].[FileUrl] END ASC,
			 CASE WHEN @SortColumn = 'FileUrl' AND @SortDirection = 'DESC'
				THEN [d].[FileUrl] END DESC,
			 CASE WHEN @SecondarySortColumn = 'PrescriptionPaymentId' AND @SecondarySortDirection = 'ASC'
				THEN pp.PrescriptionPaymentID END ASC,
			 CASE WHEN @SecondarySortColumn = 'PrescriptionPaymentId' AND @SecondarySortDirection = 'DESC'
				THEN pp.PrescriptionPaymentID END DESC,
			 CASE WHEN @SecondarySortColumn = 'PrescriptionId' AND @SecondarySortDirection = 'ASC'
				THEN [p].[PrescriptionID] END ASC,
			 CASE WHEN @SecondarySortColumn = 'PrescriptionId' AND @SecondarySortDirection = 'DESC'
				THEN [p].[PrescriptionID] END DESC,
			 CASE WHEN @SecondarySortColumn = 'PostedDate' AND @SecondarySortDirection = 'ASC'
				THEN pp.DatePosted END ASC,
			 CASE WHEN @SecondarySortColumn = 'PostedDate' AND @SecondarySortDirection = 'DESC'
				THEN pp.DatePosted END DESC,
			 CASE WHEN @SecondarySortColumn = 'CheckNumber' AND @SecondarySortDirection = 'ASC'
				THEN pp.CheckNumber END ASC,
			 CASE WHEN @SecondarySortColumn = 'CheckNumber' AND @SecondarySortDirection = 'DESC'
				THEN pp.CheckNumber END DESC,
			 CASE WHEN @SecondarySortColumn = 'CheckAmt' AND @SecondarySortDirection = 'ASC'
				THEN pp.AmountPaid END ASC,
			 CASE WHEN @SecondarySortColumn = 'CheckAmt' AND @SecondarySortDirection = 'DESC'
				THEN pp.AmountPaid END DESC,
			 CASE WHEN @SecondarySortColumn = 'RxNumber' AND @SecondarySortDirection = 'ASC'
				THEN [p].[RxNumber] END ASC,
			 CASE WHEN @SecondarySortColumn = 'RxNumber' AND @SecondarySortDirection = 'DESC'
				THEN [p].[RxNumber] END DESC,
			 CASE WHEN @SecondarySortColumn = 'RxDate' AND @SecondarySortDirection = 'ASC'
				THEN [p].[DateFilled] END ASC,
			 CASE WHEN @SecondarySortColumn = 'RxDate' AND @SecondarySortDirection = 'DESC'
				THEN [p].[DateFilled] END DESC,
			 CASE WHEN @SecondarySortColumn = 'InvoiceNumber' AND @SecondarySortDirection = 'ASC'
				THEN [i].[InvoiceNumber] END ASC,
			 CASE WHEN @SecondarySortColumn = 'InvoiceNumber' AND @SecondarySortDirection = 'DESC'
				THEN [i].[InvoiceNumber] END DESC,
			 CASE WHEN @SecondarySortColumn = 'IsReversed' AND @SecondarySortDirection = 'ASC'
				THEN p.IsReversed END ASC,
			  CASE WHEN @SecondarySortColumn = 'IsReversed' AND @SecondarySortDirection = 'DESC'
				THEN p.IsReversed END DESC,
		      CASE WHEN @SecondarySortColumn = 'DocumentId' AND @SecondarySortDirection = 'ASC'
				THEN d.DocumentID END ASC,
			  CASE WHEN @SecondarySortColumn = 'DocumentId' AND @SecondarySortDirection = 'DESC'
				THEN d.DocumentID END DESC,
			  CASE WHEN @SecondarySortColumn = 'FileName' AND @SecondarySortDirection = 'ASC'
				THEN [d].[FileName] END ASC,
			  CASE WHEN @SecondarySortColumn = 'FileName' AND @SecondarySortDirection = 'DESC'
				THEN [d].[FileName] END DESC,
			  CASE WHEN @SecondarySortColumn = 'FileUrl' AND @SecondarySortDirection = 'ASC'
				THEN [d].[FileUrl] END ASC,
			  CASE WHEN @SecondarySortColumn = 'FileUrl' AND @SecondarySortDirection = 'DESC'
				THEN [d].[FileUrl] END DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END


GO
