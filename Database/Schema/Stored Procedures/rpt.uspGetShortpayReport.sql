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
                    EXECUTE [rpt].[uspGetShortpayReport] 'AmountPaid', 'ASC', 1, 5000, @TotalRowCount = @TotalRowCount OUTPUT
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
		[PrescriptionId] [int] NOT NULL PRIMARY KEY,
		ClaimId INT NOT NULL,
		[RxNumber] [varchar](100) NOT NULL,
		[RxDate] [nvarchar](4000) NULL,
		[BilledAmount] [money] NOT NULL,
		[AmountPaid] [money] NOT NULL,
		[LastName] [varchar](155) NOT NULL,
		[FirstName] [varchar](155) NOT NULL,
		[ClaimNumber] [varchar](255) NOT NULL,
		[PrescriptionStatus] [varchar](100) NULL
	);

	INSERT #Results (PrescriptionId,ClaimId,RxNumber,RxDate,BilledAmount,AmountPaid,LastName,FirstName,ClaimNumber,PrescriptionStatus)
	SELECT  p1.PrescriptionID PrescriptionId
		   ,c1.ClaimID ClaimId
		   ,p1.RxNumber
		   ,RxDate = FORMAT(p1.DateFilled, 'M/d/yyyy')
		   ,p1.BilledAmount
		   ,p2.AmountPaid
		   ,p3.LastName
		   ,p3.FirstName
		   ,c1.ClaimNumber
		   ,ps1.StatusName PrescriptionStatus
	FROM    dbo.Prescription p1
			CROSS APPLY 
            (
                SELECT  AmountPaid = SUM(ip2.AmountPaid)
                FROM    dbo.PrescriptionPayment ip2
                WHERE   p1.PrescriptionID = ip2.PrescriptionID
            ) AS p2
			INNER JOIN dbo.Claim c1 ON p1.ClaimID = c1.ClaimID
			INNER JOIN dbo.Patient p3 ON c1.PatientID = p3.PatientID
			LEFT JOIN dbo.PrescriptionStatus ps1 ON p1.PrescriptionStatusID = ps1.PrescriptionStatusID
			LEFT JOIN dbo.ShortPayExclusion AS spe ON spe.PrescriptionID = p1.PrescriptionID
	WHERE   ((p1.BilledAmount - p2.AmountPaid) / p1.BilledAmount > 0.75)
			AND p1.BilledAmount > @BilledAmountThreshold
			AND p2.AmountPaid > 0
			AND p1.IsReversed = 0
			AND p1.PrescriptionStatusID != @NoActionNeeded
			AND spe.ShortPayExclusionID IS NULL;

	SELECT @TotalRowCount = COUNT(*) FROM #Results AS r

	SELECT r.PrescriptionId
		  ,r.ClaimId
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
			CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PrescriptionId' AND @SortDirection = 'ASC'
				  THEN r.PrescriptionId END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PrescriptionId' AND @SortDirection = 'DESC'
			      THEN r.PrescriptionId END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
				  THEN r.RxNumber END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
			      THEN r.RxNumber END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'RxDate' AND @SortDirection = 'ASC'
				  THEN r.RxDate END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'RxDate' AND @SortDirection = 'DESC'
			      THEN r.RxDate END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'BilledAmount' AND @SortDirection = 'ASC'
				  THEN r.BilledAmount END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'BilledAmount' AND @SortDirection = 'DESC'
			      THEN r.BilledAmount END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'AmountPaid' AND @SortDirection = 'ASC'
				  THEN r.AmountPaid END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'AmountPaid' AND @SortDirection = 'DESC'
			      THEN r.AmountPaid END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'LastName' AND @SortDirection = 'ASC'
				  THEN r.LastName END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'LastName' AND @SortDirection = 'DESC'
			      THEN r.LastName END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'FirstName' AND @SortDirection = 'ASC'
				  THEN r.FirstName END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'FirstName' AND @SortDirection = 'DESC'
			      THEN r.FirstName END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'ClaimNumber' AND @SortDirection = 'ASC'
				  THEN r.ClaimNumber END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'ClaimNumber' AND @SortDirection = 'DESC'
			      THEN r.ClaimNumber END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PrescriptionStatus' AND @SortDirection = 'ASC'
				  THEN r.PrescriptionStatus END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PrescriptionStatus' AND @SortDirection = 'DESC'
			      THEN r.PrescriptionStatus END DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END



GO
