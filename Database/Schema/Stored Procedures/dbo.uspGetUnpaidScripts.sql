SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/11/2017
	Description:	Gets the Unpaid Scripts
	Sample Execute:
					EXEC dbo.uspGetUnpaidScripts 1, NULL, NULL, 'RxDate', 'ASC', 1, 5000
*/
CREATE PROC [dbo].[uspGetUnpaidScripts]
(
	@IsDefaultSort BIT,
	@StartDate DATE,
	@EndDate DATE,
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	IF @IsDefaultSort IS NULL
		SET @IsDefaultSort = 0;
	DECLARE @MI        INT				= dbo.udfGetStateIDByCode('MI')
		  , @LocalDate DATE				= CONVERT(DATE, [dtme].[udfGetLocalDate]())
		  , @NonMichiganThreshold INT	= 45
		  , @MichiganThreshold INT		= 60;

	WITH UnpaidScriptsCTE AS
	(
		SELECT          PrescriptionId   = p.PrescriptionID
					  , ClaimId          = c.ClaimID
					  , PatientName      = RTRIM(LTRIM(pat.LastName)) + ', ' + RTRIM(LTRIM(pat.FirstName))
					  , ClaimNumber      = c.ClaimNumber
					  , InvoiceNumber    = i.InvoiceNumber
					  , InvoiceDate      = i.InvoiceDate
					  , InvAmt           = p.BilledAmount
					  , RxNumber         = p.RxNumber
					  , RxDate           = p.DateFilled
					  , LabelName        = p.LabelName
					  , InsuranceCarrier = pay.GroupName
					  , PharmacyState    = us.StateCode
					  , AdjustorName     = a.AdjustorName
					  , AdjustorPhone    = a.PhoneNumber
					  ,	AmountPaid		 = CONVERT(MONEY, ISNULL(pp.AmtPaid, 0.00))
					  , LastName		 = pat.LastName
					  , FirstName		 = pat.FirstName
		FROM            dbo.Prescription	AS p
			INNER JOIN  dbo.Claim			AS c ON p.ClaimID = c.ClaimID
			INNER JOIN  dbo.Invoice			AS i ON p.InvoiceID = i.InvoiceID
			INNER JOIN  dbo.Patient			AS pat ON c.PatientID = pat.PatientID
			INNER JOIN  dbo.Payor			AS pay ON c.PayorID = pay.PayorID
			INNER JOIN  dbo.Pharmacy		AS ph ON p.PharmacyNABP = ph.NABP
			INNER JOIN  dbo.UsState			AS us ON ph.StateID = us.StateID
			LEFT JOIN   dbo.Adjustor		AS a ON c.AdjusterID = a.AdjustorID
			OUTER APPLY (SELECT  AmtPaid = SUM(ipp.AmountPaid)
						 FROM    dbo.PrescriptionPayment AS ipp
						 WHERE   p.PrescriptionID = ipp.PrescriptionID) AS pp
		WHERE           (p.DateFilled >= @StartDate OR @StartDate IS NULL)  -- TODO: find out which date we are really filtering on.
						AND (p.DateFilled <= @EndDate OR @EndDate IS NULL)
						AND 1 = CASE WHEN ph.StateID != @MI AND DATEDIFF(DAY, i.InvoiceDate, @LocalDate) > @NonMichiganThreshold THEN 1
									 WHEN ph.StateID = @MI AND DATEDIFF(DAY, i.InvoiceDate, @LocalDate) > @MichiganThreshold THEN 1
									 ELSE 0
								END
						AND p.IsReversed = 0
						AND c.TermDate > @LocalDate
	)
	SELECT u.PrescriptionId
		 , u.ClaimId
		 , '' [Owner]
		 , CONVERT(DATETIME, dtme.udfGetLocalDate()) [Created]
		 , u.PatientName
		 , u.ClaimNumber
		 , u.InvoiceNumber
		 , u.InvoiceDate
		 , u.InvAmt
		 , u.RxNumber
		 , u.RxDate
		 , u.LabelName
		 , u.InsuranceCarrier
		 , u.PharmacyState
		 , u.AdjustorName
		 , u.AdjustorPhone
	FROM UnpaidScriptsCTE u
	WHERE ((u.AmountPaid < (u.InvAmt * 0.75)) OR u.InvAmt = 0)
	ORDER BY CASE WHEN @IsDefaultSort = 1 THEN u.InsuranceCarrier END ASC,
			 CASE WHEN @IsDefaultSort = 1 THEN u.LastName END ASC,
			 CASE WHEN @IsDefaultSort = 1 THEN u.FirstName END ASC,
			 CASE WHEN @IsDefaultSort = 1 THEN u.RxDate END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PrescriptionId' AND @SortDirection = 'ASC'
				  THEN u.PrescriptionId END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PrescriptionId' AND @SortDirection = 'DESC'
				  THEN u.PrescriptionId END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'ClaimId' AND @SortDirection = 'ASC'
				  THEN u.ClaimId END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'ClaimId' AND @SortDirection = 'DESC'
				  THEN u.ClaimId END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PatientName' AND @SortDirection = 'ASC'
				  THEN u.PatientName END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PatientName' AND @SortDirection = 'DESC'
				  THEN u.PatientName END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'ClaimNumber' AND @SortDirection = 'ASC'
				  THEN u.ClaimNumber END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'ClaimNumber' AND @SortDirection = 'DESC'
				  THEN u.ClaimNumber END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'InvoiceNumber' AND @SortDirection = 'ASC'
				  THEN u.InvoiceNumber END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'InvoiceNumber' AND @SortDirection = 'DESC'
				  THEN u.InvoiceNumber END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'InvoiceDate' AND @SortDirection = 'ASC'
				  THEN u.InvoiceDate END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'InvoiceDate' AND @SortDirection = 'DESC'
				  THEN u.InvoiceDate END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'InvAmt' AND @SortDirection = 'ASC'
				  THEN u.InvAmt END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'InvAmt' AND @SortDirection = 'DESC'
				  THEN u.InvAmt END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'RxNumber' AND @SortDirection = 'ASC'
				  THEN u.RxNumber END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'RxNumber' AND @SortDirection = 'DESC'
				  THEN u.RxNumber END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'RxDate' AND @SortDirection = 'ASC'
				  THEN u.RxDate END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'RxDate' AND @SortDirection = 'DESC'
				  THEN u.RxDate END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'LabelName' AND @SortDirection = 'ASC'
				  THEN u.LabelName END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'LabelName' AND @SortDirection = 'DESC'
				  THEN u.LabelName END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'InsuranceCarrier' AND @SortDirection = 'ASC'
				  THEN u.InsuranceCarrier END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'InsuranceCarrier' AND @SortDirection = 'DESC'
				  THEN u.InsuranceCarrier END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PharmacyState' AND @SortDirection = 'ASC'
				  THEN u.PharmacyState END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'PharmacyState' AND @SortDirection = 'DESC'
				  THEN u.PharmacyState END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'AdjustorName' AND @SortDirection = 'ASC'
				  THEN u.AdjustorName END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'AdjustorName' AND @SortDirection = 'DESC'
				  THEN u.AdjustorName END DESC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'AdjustorPhone' AND @SortDirection = 'ASC'
				  THEN u.AdjustorPhone END ASC,
			 CASE WHEN @IsDefaultSort = 0 AND @SortColumn = 'AdjustorPhone' AND @SortDirection = 'DESC'
				  THEN u.AdjustorPhone END DESC
			OFFSET @PageSize * (@PageNumber - 1) ROWS
			FETCH NEXT @PageSize ROWS ONLY;
END
GO
