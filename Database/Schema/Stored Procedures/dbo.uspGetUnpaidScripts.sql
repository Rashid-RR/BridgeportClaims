SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/11/2017
	Description:	Gets the Unpaid Scripts
	Sample Execute:
					DECLARE @Carriers [dbo].[udtPayorID];
					INSERT @Carriers (PayorID) VALUES (1),(49);
					DECLARE @TotalRows INT; EXEC dbo.uspGetUnpaidScripts 1, NULL, NULL, 'RxDate', 'ASC', 1, 30, @Carriers, @TotalRows OUTPUT SELECT @TotalRows
					DELETE @Carriers
					EXEC dbo.uspGetUnpaidScripts 1, NULL, NULL, 'RxDate', 'ASC', 1, 30, @Carriers, @TotalRows OUTPUT SELECT @TotalRows
*/
CREATE PROC [dbo].[uspGetUnpaidScripts]
(
	@IsDefaultSort BIT,
	@StartDate DATE,
	@EndDate DATE,
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER,
	@Carriers [dbo].[udtPayorID] READONLY,
	@TotalRows INTEGER OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @iIsDefaultSort BIT = @IsDefaultSort
		  , @iStartDate DATE = @StartDate
		  , @iEndDate DATE = @EndDate
		  , @iSortColumn VARCHAR(50) = @SortColumn
		  , @iSortDirection VARCHAR(5) = @SortDirection
		  , @iPageNumber INTEGER = @PageNumber
		  , @iPageSize INTEGER = @PageSize;

	CREATE TABLE #Carriers (PayorID INT NOT NULL PRIMARY KEY)

	IF NOT EXISTS (SELECT * FROM @Carriers)
		BEGIN
			INSERT #Carriers (PayorID) SELECT p.PayorID FROM dbo.Payor AS p
		END
	ELSE
		BEGIN
			INSERT #Carriers (PayorID) SELECT PayorID FROM @Carriers
		END

	IF @iIsDefaultSort IS NULL
		SET @iIsDefaultSort = 0;
	DECLARE @MI        INT				= dbo.udfGetStateIDByCode('MI')
		  , @LocalDate DATE				= CONVERT(DATE, [dtme].[udfGetLocalDate]())
		  , @NonMichiganThreshold INT	= 45
		  , @MichiganThreshold INT		= 60;

	CREATE TABLE #UnpaidScripts (PrescriptionId INT NOT NULL PRIMARY KEY, ClaimId INT NOT NULL,
		PatientName VARCHAR(500) NOT NULL, ClaimNumber VARCHAR(255) NOT NULL, InvoiceNumber VARCHAR(100) NOT NULL,
		InvoiceDate DATE NOT NULL, InvAmt MONEY NOT NULL, RxNumber VARCHAR(100) NOT NULL, RxDate DATETIME2 NOT NULL,
		LabelName VARCHAR(25), PayorId INT NOT NULL, InsuranceCarrier VARCHAR(255) NOT NULL, PharmacyState CHAR(2) NOT NULL,
		AdjustorName VARCHAR(255), AdjustorPhone VARCHAR(30), AmountPaid MONEY NOT NULL,
		LastName VARCHAR(155) NOT NULL, FirstName VARCHAR(155) NOT NULL);

	CREATE TABLE #UnpaidScriptsFiltered (PrescriptionId INT NOT NULL PRIMARY KEY, ClaimId INT NOT NULL,
		PatientName VARCHAR(500) NOT NULL, ClaimNumber VARCHAR(255) NOT NULL, InvoiceNumber VARCHAR(100) NOT NULL,
		InvoiceDate DATE NOT NULL, InvAmt MONEY NOT NULL, RxNumber VARCHAR(100) NOT NULL, RxDate DATETIME2 NOT NULL,
		LabelName VARCHAR(25), PayorId INT NOT NULL, InsuranceCarrier VARCHAR(255) NOT NULL,PharmacyState CHAR(2) NOT NULL,AdjustorName 
		VARCHAR(255), AdjustorPhone VARCHAR(30), LastName VARCHAR(155) NOT NULL, FirstName VARCHAR(155) NOT NULL);

	INSERT #UnpaidScripts (PrescriptionId,ClaimId,PatientName,ClaimNumber,InvoiceNumber,InvoiceDate
					,InvAmt,RxNumber,RxDate,LabelName,PayorId,InsuranceCarrier,PharmacyState,AdjustorName
					,AdjustorPhone,AmountPaid,LastName,FirstName)
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
					, PayorId          = pay.PayorID
					, InsuranceCarrier = pay.GroupName
					, PharmacyState    = us.StateCode
					, AdjustorName     = a.AdjustorName
					, AdjustorPhone    = a.PhoneNumber
					, AmountPaid	   = ISNULL((  SELECT  SUM(ipp.AmountPaid)
											FROM    dbo.PrescriptionPayment AS ipp
											WHERE   p.PrescriptionID = ipp.PrescriptionID), 0.00)
					, LastName		 = pat.LastName
					, FirstName		 = pat.FirstName
	FROM            dbo.Prescription	AS p
		INNER JOIN  dbo.Claim			AS c ON p.ClaimID = c.ClaimID
		INNER JOIN  dbo.Invoice			AS i ON p.InvoiceID = i.InvoiceID
		INNER JOIN  dbo.Patient			AS pat ON c.PatientID = pat.PatientID
		INNER JOIN  dbo.Payor			AS pay ON c.PayorID = pay.PayorID
		INNER JOIN  #Carriers           AS car ON car.PayorID = pay.PayorID
		INNER JOIN  dbo.Pharmacy		AS ph ON p.PharmacyNABP = ph.NABP
		INNER JOIN  dbo.UsState			AS us ON ph.StateID = us.StateID
		LEFT JOIN   dbo.Adjustor		AS a ON [c].[AdjustorID] = a.AdjustorID
	WHERE           (i.InvoiceDate >= @iStartDate OR @iStartDate IS NULL)
					AND (i.InvoiceDate <= @iEndDate OR @iEndDate IS NULL)
					AND 1 = CASE WHEN ph.StateID != @MI AND DATEDIFF(DAY, i.InvoiceDate, @LocalDate) > @NonMichiganThreshold THEN 1
									WHEN ph.StateID = @MI AND DATEDIFF(DAY, i.InvoiceDate, @LocalDate) > @MichiganThreshold THEN 1
									ELSE 0
							END
					AND p.IsReversed = 0
					AND c.TermDate > @LocalDate;

	INSERT #UnpaidScriptsFiltered (PrescriptionId,ClaimId,PatientName,ClaimNumber,InvoiceNumber,InvoiceDate
	 ,InvAmt,RxNumber,RxDate,LabelName,PayorId,InsuranceCarrier,PharmacyState,AdjustorName,AdjustorPhone,LastName,FirstName)
	SELECT u.PrescriptionId
			, u.ClaimId
			, u.PatientName
			, u.ClaimNumber
			, u.InvoiceNumber
			, u.InvoiceDate
			, u.InvAmt
			, u.RxNumber
			, u.RxDate
			, u.LabelName
			, u.PayorId
			, u.InsuranceCarrier
			, u.PharmacyState
			, u.AdjustorName
			, u.AdjustorPhone
			, u.LastName
			, u.FirstName
	FROM   #UnpaidScripts AS u
		   LEFT JOIN dbo.UnpaidScriptsArchived AS a ON u.PrescriptionID = a.PrescriptionID
	WHERE  ((u.AmountPaid < (u.InvAmt * 0.75)) OR u.InvAmt = 0)
		   AND a.UnpaidScriptsArchivedID IS NULL

	SELECT @TotalRows = COUNT(*) FROM #UnpaidScriptsFiltered
	
	SELECT u.PrescriptionId
         , u.ClaimId
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
	FROM #UnpaidScriptsFiltered AS u
	ORDER BY CASE WHEN @iIsDefaultSort = 1 THEN u.InsuranceCarrier END ASC,
			 CASE WHEN @iIsDefaultSort = 1 THEN u.LastName END ASC,
			 CASE WHEN @iIsDefaultSort = 1 THEN u.FirstName END ASC,
			 CASE WHEN @iIsDefaultSort = 1 THEN u.RxDate END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'PrescriptionId' AND @iSortDirection = 'ASC'
				  THEN u.PrescriptionId END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'PrescriptionId' AND @iSortDirection = 'DESC'
				  THEN u.PrescriptionId END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'ClaimId' AND @iSortDirection = 'ASC'
				  THEN u.ClaimId END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'ClaimId' AND @iSortDirection = 'DESC'
				  THEN u.ClaimId END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'PatientName' AND @iSortDirection = 'ASC'
				  THEN u.PatientName END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'PatientName' AND @iSortDirection = 'DESC'
				  THEN u.PatientName END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'ClaimNumber' AND @iSortDirection = 'ASC'
				  THEN u.ClaimNumber END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'ClaimNumber' AND @iSortDirection = 'DESC'
				  THEN u.ClaimNumber END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'InvoiceNumber' AND @iSortDirection = 'ASC'
				  THEN u.InvoiceNumber END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'InvoiceNumber' AND @iSortDirection = 'DESC'
				  THEN u.InvoiceNumber END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'InvoiceDate' AND @iSortDirection = 'ASC'
				  THEN u.InvoiceDate END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'InvoiceDate' AND @iSortDirection = 'DESC'
				  THEN u.InvoiceDate END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'InvAmt' AND @iSortDirection = 'ASC'
				  THEN u.InvAmt END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'InvAmt' AND @iSortDirection = 'DESC'
				  THEN u.InvAmt END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'RxNumber' AND @iSortDirection = 'ASC'
				  THEN u.RxNumber END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'RxNumber' AND @iSortDirection = 'DESC'
				  THEN u.RxNumber END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'RxDate' AND @iSortDirection = 'ASC'
				  THEN u.RxDate END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'RxDate' AND @iSortDirection = 'DESC'
				  THEN u.RxDate END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'LabelName' AND @iSortDirection = 'ASC'
				  THEN u.LabelName END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'LabelName' AND @iSortDirection = 'DESC'
				  THEN u.LabelName END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'InsuranceCarrier' AND @iSortDirection = 'ASC'
				  THEN u.InsuranceCarrier END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'InsuranceCarrier' AND @iSortDirection = 'DESC'
				  THEN u.InsuranceCarrier END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'PharmacyState' AND @iSortDirection = 'ASC'
				  THEN u.PharmacyState END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'PharmacyState' AND @iSortDirection = 'DESC'
				  THEN u.PharmacyState END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'AdjustorName' AND @iSortDirection = 'ASC'
				  THEN u.AdjustorName END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'AdjustorName' AND @iSortDirection = 'DESC'
				  THEN u.AdjustorName END DESC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'AdjustorPhone' AND @iSortDirection = 'ASC'
				  THEN u.AdjustorPhone END ASC,
			 CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'AdjustorPhone' AND @iSortDirection = 'DESC'
				  THEN u.AdjustorPhone END DESC
			OFFSET @iPageSize * (@iPageNumber - 1) ROWS
			FETCH NEXT @iPageSize ROWS ONLY;
END
GO
