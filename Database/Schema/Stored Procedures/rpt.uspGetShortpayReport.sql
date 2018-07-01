SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       6/29/2018
 Description:       Gets the results of the Short Pay Report 
 Example Execute:
					DECLARE @TotalRowCount INT
                    EXECUTE [rpt].[uspGetShortpayReport] NULL, NULL, 1, 5000, @TotalRowCount = @TotalRowCount OUTPUT
					SELECT @TotalRowCount
 =============================================
*/
CREATE PROC [rpt].[uspGetShortpayReport]
(
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER,
	@TotalRowCount INTEGER OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

	DECLARE @IsDefaultSort BIT = 1

	IF (@SortColumn IS NOT NULL)
		SET @IsDefaultSort = 0;

    DECLARE @NoActionNeeded INT;
	SELECT  @NoActionNeeded = ps.PrescriptionStatusID
	FROM    dbo.PrescriptionStatus AS ps
	WHERE   ps.StatusName = 'No Action Needed';

	IF (@NoActionNeeded IS NULL)
		BEGIN
			RAISERROR(N'Error, no prescription status of No Action Needed was found.', 16, 1) WITH NOWAIT;
			RETURN -1;
		END

	DECLARE @BilledAmountThreshold MONEY = 10

	CREATE TABLE #Results (
		[PrescriptionPaymentId] [int] NOT NULL PRIMARY KEY,
		[RxNumber] [varchar](100) NOT NULL,
		[RxDate] [nvarchar](4000) NULL,
		[BilledAmount] [money] NOT NULL,
		[AmountPaid] [money] NOT NULL,
		[LastName] [varchar](155) NOT NULL,
		[FirstName] [varchar](155) NOT NULL,
		[ClaimNumber] [varchar](255) NOT NULL,
		[PrescriptionStatus] [varchar](100) NULL
	);

	INSERT #Results (PrescriptionPaymentId,RxNumber,RxDate,BilledAmount,AmountPaid,LastName,FirstName,ClaimNumber,PrescriptionStatus)
	SELECT  p2.PrescriptionPaymentID PrescriptionPaymentId
		   ,p1.RxNumber
		   ,RxDate = FORMAT(p1.DateFilled, 'M/d/yyyy')
		   ,p1.BilledAmount
		   ,p2.AmountPaid
		   ,p3.LastName
		   ,p3.FirstName
		   ,c1.ClaimNumber
		   ,ps1.StatusName PrescriptionStatus
	FROM    dbo.Prescription p1
			INNER JOIN dbo.PrescriptionPayment p2 ON p1.PrescriptionID = p2.PrescriptionID
			LEFT JOIN dbo.PrescriptionStatus ps1 ON p1.PrescriptionStatusID = ps1.PrescriptionStatusID
			INNER JOIN dbo.Claim c1 ON p1.ClaimID = c1.ClaimID
			INNER JOIN dbo.Patient p3 ON c1.PatientID = p3.PatientID
			LEFT JOIN dbo.ShortPayExclusion s ON s.PrescriptionPaymentID = p2.PrescriptionPaymentID
	WHERE   ((p1.BilledAmount - p2.AmountPaid) / p1.BilledAmount > 0.75)
			AND p1.BilledAmount > @BilledAmountThreshold
			AND p2.AmountPaid > 0
			AND p1.IsReversed = 0
			AND p1.PrescriptionStatusID != @NoActionNeeded
			AND s.ShortPayExclusionID IS NULL

	SELECT @TotalRowCount = COUNT(*) FROM #Results AS r

	SELECT r.PrescriptionPaymentId
          ,r.RxNumber
          ,r.RxDate
          ,r.BilledAmount
          ,r.AmountPaid
          ,r.LastName
          ,r.FirstName
          ,r.ClaimNumber
          ,r.PrescriptionStatus
	FROM #Results AS r
   ORDER BY CASE WHEN @IsDefaultSort = 1 THEN r.LastName END ASC,
			CASE WHEN @IsDefaultSort = 1 THEN r.FirstName END ASC,
			CASE WHEN @IsDefaultSort = 1 THEN r.ClaimNumber END ASC,
			CASE WHEN @SortColumn = 'PrescriptionPaymentId' AND @SortDirection = 'ASC'
				  THEN r.PrescriptionPaymentId END ASC,
			 CASE WHEN @SortColumn = 'PrescriptionPaymentId' AND @SortDirection = 'DESC'
			      THEN r.PrescriptionPaymentId END DESC,
			 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
				  THEN r.RxNumber END ASC,
			 CASE WHEN @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
			      THEN r.RxNumber END DESC,
			 CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'ASC'
				  THEN r.RxNumber END ASC,
			 CASE WHEN @SortColumn = 'RxDate' AND @SortDirection = 'DESC'
			      THEN r.RxNumber END DESC,
			 CASE WHEN @SortColumn = 'BilledAmount' AND @SortDirection = 'ASC'
				  THEN r.RxNumber END ASC,
			 CASE WHEN @SortColumn = 'BilledAmount' AND @SortDirection = 'DESC'
			      THEN r.RxNumber END DESC,
			 CASE WHEN @SortColumn = 'AmountPaid' AND @SortDirection = 'ASC'
				  THEN r.RxNumber END ASC,
			 CASE WHEN @SortColumn = 'AmountPaid' AND @SortDirection = 'DESC'
			      THEN r.RxNumber END DESC,
			 CASE WHEN @SortColumn = 'LastName' AND @SortDirection = 'ASC'
				  THEN r.RxNumber END ASC,
			 CASE WHEN @SortColumn = 'LastName' AND @SortDirection = 'DESC'
			      THEN r.RxNumber END DESC,
			 CASE WHEN @SortColumn = 'FirstName' AND @SortDirection = 'ASC'
				  THEN r.RxNumber END ASC,
			 CASE WHEN @SortColumn = 'FirstName' AND @SortDirection = 'DESC'
			      THEN r.RxNumber END DESC,
			 CASE WHEN @SortColumn = 'ClaimNumber' AND @SortDirection = 'ASC'
				  THEN r.RxNumber END ASC,
			 CASE WHEN @SortColumn = 'ClaimNumber' AND @SortDirection = 'DESC'
			      THEN r.RxNumber END DESC,
			 CASE WHEN @SortColumn = 'PrescriptionStatus' AND @SortDirection = 'ASC'
				  THEN r.RxNumber END ASC,
			 CASE WHEN @SortColumn = 'PrescriptionStatus' AND @SortDirection = 'DESC'
			      THEN r.RxNumber END DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END
GO