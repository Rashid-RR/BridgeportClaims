SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/09/2018
	Description:	Gets the Data for the Outstanding blade
	Sample Execute:
					EXEC [claims].[uspGetOutstandingBlade] 734, 1, 50, 'RxDate', 'ASC';
*/
CREATE PROC [claims].[uspGetOutstandingBlade]
(
	@ClaimID INTEGER,
	@PageNumber INTEGER,
	@PageSize INTEGER,
	@SortColumn VARCHAR(50) = NULL,
	@SortDirection VARCHAR(5) = NULL
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	-- Parameter Sniffing Remedy
	DECLARE @iSortColumn VARCHAR(50) = @SortColumn
		  , @iSortDirection VARCHAR(5) = @SortDirection
		  , @iPageNumber INTEGER = @PageNumber
		  , @iPageSize INTEGER = @PageSize
		  , @iClaimID INTEGER = @ClaimID;

	IF @SortColumn IS NULL
		SET @SortColumn = 'RxDate';
	IF @SortDirection IS NULL
		SET @SortDirection = 'DESC';
	DECLARE @MI        INT				= dbo.udfGetStateIDByCode('MI')
		  , @LocalDate DATE				= CONVERT(DATE, [dtme].[udfGetLocalDate]())
		  , @NonMichiganThreshold INT	= 45
		  , @MichiganThreshold INT		= 60;

	CREATE TABLE #Outstanding
	(
		[PrescriptionId] INT NOT NULL PRIMARY KEY
	   ,[InvoiceDate] DATE NOT NULL
	   ,[InvoiceNumber] VARCHAR(100) NOT NULL
	   ,[LabelName] VARCHAR(25) NULL
	   ,[BillTo] VARCHAR(255) NOT NULL
	   ,[RxNumber] VARCHAR(100) NOT NULL
	   ,[RxDate] DATETIME2 NOT NULL
	   ,[InvAmt] MONEY NOT NULL
	   ,[AmountPaid] MONEY NOT NULL
	   ,[Outstanding] MONEY NOT NULL
	   ,[Status] VARCHAR(100) NULL
	   ,[NoteCount] INT NOT NULL
	);

	CREATE TABLE #OutstandingFiltered
	(
		[PrescriptionId] INT NOT NULL PRIMARY KEY
	   ,[InvoiceDate] DATE NOT NULL
	   ,[InvoiceNumber] VARCHAR(100) NOT NULL
	   ,[LabelName] VARCHAR(25) NULL
	   ,[BillTo] VARCHAR(255) NOT NULL
	   ,[RxNumber] VARCHAR(100) NOT NULL
	   ,[RxDate] DATETIME2 NOT NULL
	   ,[InvAmt] MONEY NOT NULL
	   ,[AmountPaid] MONEY NOT NULL
	   ,[Outstanding] MONEY NOT NULL
	   ,[Status] VARCHAR(100) NULL
	   ,[NoteCount] INT NOT NULL
	);

	INSERT #Outstanding (PrescriptionId,InvoiceDate,InvoiceNumber,LabelName,BillTo,RxNumber,RxDate,InvAmt,AmountPaid,Outstanding,[Status],NoteCount)
	SELECT           PrescriptionId   = p.PrescriptionID
					, InvoiceDate      = i.InvoiceDate
					, InvoiceNumber    = i.InvoiceNumber
					, LabelName        = p.LabelName
					, BillTo           = pay.GroupName
					, RxNumber         = p.RxNumber
					, RxDate           = p.DateFilled
					, InvAmt           = p.BilledAmount
					, AmountPaid	   = ISNULL((	SELECT  SUM(ipp.AmountPaid)
													FROM    dbo.PrescriptionPayment AS ipp
													WHERE   p.PrescriptionID = ipp.PrescriptionID), 0.00)
					, Outstanding = p.BilledAmount - ISNULL((  SELECT   SUM(ipm.AmountPaid)
                                                               FROM		dbo.PrescriptionPayment AS [ipm]
                                                               WHERE	ipm.PrescriptionID = [p].[PrescriptionID]), 0.00)
					, [Status] = ps.StatusName
					, NoteCount = ISNULL((	   SELECT COUNT(*)
											   FROM   [dbo].[PrescriptionNoteMapping] AS [pnm]
											   WHERE  [pnm].[PrescriptionID] = [p].[PrescriptionID]
										   ), 0)
	FROM            dbo.Prescription	AS p
		INNER JOIN  dbo.Claim			AS c ON p.ClaimID = c.ClaimID
		INNER JOIN  dbo.Invoice			AS i ON p.InvoiceID = i.InvoiceID
		INNER JOIN  dbo.Payor			AS pay ON c.PayorID = pay.PayorID
		INNER JOIN  dbo.Pharmacy		AS ph ON p.PharmacyNABP = ph.NABP
		INNER JOIN  dbo.UsState			AS us ON ph.StateID = us.StateID
		LEFT JOIN   dbo.Adjustor		AS a ON [c].[AdjustorID] = a.AdjustorID
		LEFT JOIN   dbo.PrescriptionStatus AS ps ON p.PrescriptionStatusID = ps.PrescriptionStatusID
	WHERE           1 = 1
					AND 1 = CASE WHEN ph.StateID != @MI AND DATEDIFF(DAY, i.InvoiceDate, @LocalDate) >= @NonMichiganThreshold THEN 1
									WHEN ph.StateID = @MI AND DATEDIFF(DAY, i.InvoiceDate, @LocalDate) >= @MichiganThreshold THEN 1
									ELSE 0
							END
					AND p.IsReversed = 0
					AND p.ClaimID = @iClaimID;
	
	INSERT #OutstandingFiltered (PrescriptionId,InvoiceDate,InvoiceNumber,LabelName,BillTo,RxNumber,RxDate,InvAmt,AmountPaid,Outstanding,[Status],NoteCount)
	SELECT u.PrescriptionId
          ,u.InvoiceDate
          ,u.InvoiceNumber
          ,u.LabelName
          ,u.BillTo
          ,u.RxNumber
          ,u.RxDate
          ,u.InvAmt
          ,u.AmountPaid
          ,u.Outstanding
          ,u.[Status]
          ,u.NoteCount
	FROM   #Outstanding AS u
	WHERE  ((u.AmountPaid < (u.InvAmt * 0.75)) OR u.InvAmt = 0)

	DECLARE @TotalRows INTEGER,	@TotalOutstanding MONEY;
	SELECT @TotalRows = COUNT(*) FROM #OutstandingFiltered;
	SELECT @TotalOutstanding = SUM(ISNULL(Outstanding, 0.00)) FROM #OutstandingFiltered;

	SELECT u.PrescriptionId
          ,u.InvoiceDate
          ,u.InvoiceNumber
          ,u.LabelName
          ,u.BillTo
          ,u.RxNumber
          ,u.RxDate
          ,u.InvAmt
          ,u.AmountPaid
          ,u.Outstanding
          ,u.[Status]
          ,u.NoteCount
	FROM #OutstandingFiltered AS u
	ORDER BY CASE WHEN @iSortColumn = 'PrescriptionId' AND @iSortDirection = 'ASC'
				  THEN u.PrescriptionId END ASC,
			 CASE WHEN @iSortColumn = 'PrescriptionId' AND @iSortDirection = 'DESC'
				  THEN u.PrescriptionId END DESC,
			 CASE WHEN @iSortColumn = 'InvoiceDate' AND @iSortDirection = 'ASC'
				  THEN u.InvoiceDate END ASC,
			 CASE WHEN @iSortColumn = 'InvoiceDate' AND @iSortDirection = 'DESC'
				  THEN u.InvoiceDate END DESC,
			 CASE WHEN @iSortColumn = 'InvoiceNumber' AND @iSortDirection = 'ASC'
				  THEN u.InvoiceNumber END ASC,
			 CASE WHEN @iSortColumn = 'InvoiceNumber' AND @iSortDirection = 'DESC'
				  THEN u.InvoiceNumber END DESC,
			 CASE WHEN @iSortColumn = 'LabelName' AND @iSortDirection = 'ASC'
				  THEN u.LabelName END ASC,
			 CASE WHEN @iSortColumn = 'LabelName' AND @iSortDirection = 'DESC'
				  THEN u.LabelName END DESC,
			 CASE WHEN @iSortColumn = 'BillTo' AND @iSortDirection = 'ASC'
				  THEN u.BillTo END ASC,
			 CASE WHEN @iSortColumn = 'BillTo' AND @iSortDirection = 'DESC'
				  THEN u.BillTo END DESC,
			 CASE WHEN @iSortColumn = 'RxNumber' AND @iSortDirection = 'ASC'
				  THEN u.RxNumber END ASC,
			 CASE WHEN @iSortColumn = 'RxNumber' AND @iSortDirection = 'DESC'
				  THEN u.RxNumber END DESC,
			 CASE WHEN @iSortColumn = 'RxDate' AND @iSortDirection = 'ASC'
				  THEN u.RxDate END ASC,
			 CASE WHEN @iSortColumn = 'RxDate' AND @iSortDirection = 'DESC'
				  THEN u.RxDate END DESC,
			 CASE WHEN @iSortColumn = 'InvAmt' AND @iSortDirection = 'ASC'
				  THEN u.InvAmt END ASC,
			 CASE WHEN @iSortColumn = 'InvAmt' AND @iSortDirection = 'DESC'
				  THEN u.InvAmt END DESC,
			 CASE WHEN @iSortColumn = 'AmountPaid' AND @iSortDirection = 'ASC'
				  THEN u.AmountPaid END ASC,
			 CASE WHEN @iSortColumn = 'AmountPaid' AND @iSortDirection = 'DESC'
				  THEN u.AmountPaid END DESC,
			 CASE WHEN @iSortColumn = 'Outstanding' AND @iSortDirection = 'ASC'
				  THEN u.Outstanding END ASC,
			 CASE WHEN @iSortColumn = 'Outstanding' AND @iSortDirection = 'DESC'
				  THEN u.Outstanding END DESC,
			 CASE WHEN @iSortColumn = 'Status' AND @iSortDirection = 'ASC'
				  THEN u.[Status] END ASC,
			 CASE WHEN @iSortColumn = 'Status' AND @iSortDirection = 'DESC'
				  THEN u.[Status] END DESC,
			 CASE WHEN @iSortColumn = 'NoteCount' AND @iSortDirection = 'ASC'
				  THEN u.NoteCount END ASC,
			 CASE WHEN @iSortColumn = 'NoteCount' AND @iSortDirection = 'DESC'
				  THEN u.NoteCount END DESC
			OFFSET @iPageSize * (@iPageNumber - 1) ROWS
			FETCH NEXT @iPageSize ROWS ONLY;

	SELECT @TotalRows TotalRows, @TotalOutstanding TotalOutstanding

END
GO
