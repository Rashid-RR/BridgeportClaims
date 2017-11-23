SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/12/2017
	Description:	Report that shows when the invoiced amounts for the month were actually paid.
	Sample Execute:
					EXEC rpt.uspGetAccountsReceivable '1/1/2017'
*/
CREATE PROCEDURE [rpt].[uspGetAccountsReceivable]
(
	@StartDate DATE,
	@GroupName VARCHAR(255),
	@PharmacyName VARCHAR(60)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @Jan TINYINT = 1
		   ,@Feb TINYINT = 2
		   ,@Mar TINYINT = 3
		   ,@Apr TINYINT = 4
		   ,@May TINYINT = 5
		   ,@Jun TINYINT = 6
		   ,@Jul TINYINT = 7
		   ,@Aug TINYINT = 8
		   ,@Sep TINYINT = 9
		   ,@Oct TINYINT = 10
		   ,@Nov TINYINT = 11
		   ,@Dec TINYINT = 12
		   ,@Looper TINYINT = 1
	
	DECLARE @StartYear INT = 2017 -- TODO: HACK, FIX.
	DECLARE @TimeIntervalLength INT = 12 -- TODO: HACK, FIX.

	CREATE TABLE #Master (MonthBilled VARCHAR(10) NOT NULL, CalendarMonth TINYINT NOT NULL, TotalInvoiced MONEY NOT NULL, Jan17 MONEY NOT NULL, 
			Feb17 MONEY NOT NULL, Mar17 MONEY NOT NULL, Apr17 MONEY NOT NULL, May17 MONEY NOT NULL, Jun17 MONEY NOT NULL, Jul17 MONEY NOT NULL,
			Aug17 MONEY NOT NULL, Sep17 MONEY NOT NULL, Oct17 MONEY NOT NULL, Nov17 MONEY NOT NULL, Dec17 MONEY NOT NULL);
	DECLARE @CalendarMonths TABLE (CalendarMonthNameShort VARCHAR(50) NOT NULL PRIMARY KEY, 
									CalendarMonth TINYINT NOT NULL, CalendarYear INT NOT NULL);
	WITH CalendarOrderingTrickCTE AS
	(
		SELECT	DISTINCT c.CalendarMonthNameShort, c.CalendarMonth, @StartYear CalendarYear
		FROM	dtme.Calendar AS c
		WHERE	c.DateID BETWEEN EOMONTH(@StartDate) AND DATEADD(MONTH, @TimeIntervalLength, @StartDate)
	)
	INSERT	@CalendarMonths (CalendarMonthNameShort, CalendarMonth, CalendarYear)
	SELECT c.CalendarMonthNameShort,c.CalendarMonth,c.CalendarYear 
	FROM CalendarOrderingTrickCTE AS c ORDER BY c.CalendarMonth ASC

	/*
		-- Testing
		DECLARE @StartDate DATE = '1/1/2017'
		IF OBJECT_ID('tempdb..[#Master]') IS NOT NULL
			DROP TABLE [#Master]
		IF OBJECT_ID('tempdb..[#PrescriptionTotals]') IS NOT NULL
			DROP TABLE [#PrescriptionTotals]
		IF OBJECT_ID('tempdb..[#Unpivoted]') IS NOT NULL
			DROP TABLE [#Unpivoted]
	*/
	
	CREATE TABLE [#Unpivoted](
		[PrescriptionID] [INT] NOT NULL,
		CalendarMonth TINYINT NOT NULL,
		CalendarYear INT NOT NULL,
		[Jan] [MONEY] NULL,
		[Feb] [MONEY] NULL,
		[Mar] [MONEY] NULL,
		[Apr] [MONEY] NULL,
		[May] [MONEY] NULL,
		[Jun] [MONEY] NULL,
		[Jul] [MONEY] NULL,
		[Aug] [MONEY] NULL,
		[Sep] [MONEY] NULL,
		[Oct] [MONEY] NULL,
		[Nov] [MONEY] NULL,
		[Dec] [MONEY] NULL,
		CHECK (CalendarMonth BETWEEN 1 AND 12),
		PRIMARY KEY CLUSTERED (PrescriptionID ASC)
		WITH (FILLFACTOR = 90, DATA_COMPRESSION = ROW)
		)
	INSERT #Unpivoted (PrescriptionID,CalendarMonth,CalendarYear,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,[Dec])
	SELECT          p.PrescriptionID,MONTH(i.InvoiceDate),YEAR(i.InvoiceDate)
					, Jan = (   SELECT          JanInvAmt = jan.BilledAmount
						FROM            dbo.Prescription AS jan
							INNER JOIN  dbo.Invoice      AS i ON i.InvoiceID = jan.InvoiceID
							INNER JOIN  dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE           1 = 1
										AND jan.PrescriptionID = p.PrescriptionID
										AND c.CalendarYear = @StartYear
										AND c.CalendarMonth = @Jan)
					, Feb = (	SELECT		FebInvAmt = feb.BilledAmount
						FROM		dbo.Prescription AS feb
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = feb.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND feb.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Feb)
					, Mar = (	SELECT		MarInvAmt = mar.BilledAmount
						FROM		dbo.Prescription AS mar
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = mar.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND mar.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Mar)
					, Apr = (	SELECT		RecInvAmt = apr.BilledAmount
						FROM		dbo.Prescription AS apr
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = apr.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND apr.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Apr)
					, May = ( SELECT		MayInvAmt = may.BilledAmount
						FROM		dbo.Prescription AS may
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = may.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND may.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @May)
					, Jun = ( SELECT		JunInvAmt = jun.BilledAmount
						FROM		dbo.Prescription AS jun
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = jun.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND jun.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Jun)
					, Jul = ( SELECT		JulInvAmt = jul.BilledAmount
						FROM		dbo.Prescription AS jul
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = jul.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND jul.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Jul)
					, Aug = ( SELECT		AugInvAmt = aug.BilledAmount
						FROM		dbo.Prescription AS aug
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = aug.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND aug.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Aug)
					, Sep = ( SELECT		SepInvAmt = sep.BilledAmount
						FROM		dbo.Prescription AS sep
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = sep.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND sep.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Sep)
					, Oct = ( SELECT		OctInvAmt = oct.BilledAmount
						FROM		dbo.Prescription AS oct
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = oct.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND oct.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Oct)
					, Nov = ( SELECT		NovInvAmt = nov.BilledAmount
						FROM		dbo.Prescription AS nov
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = nov.InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND nov.PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Nov)
					, [Dec] = ( SELECT		DecInvAmt = [dec].BilledAmount
						FROM		dbo.Prescription AS [dec]
							INNER JOIN dbo.Invoice AS i ON i.InvoiceID = [dec].InvoiceID
							INNER JOIN dtme.Calendar AS c ON c.DateID = i.InvoiceDate
						WHERE		1 = 1
									AND [dec].PrescriptionID = p.PrescriptionID
									AND c.CalendarYear = @StartYear
									AND c.CalendarMonth = @Dec)
	FROM            dbo.Prescription AS p
					INNER JOIN  dbo.Invoice AS i ON i.InvoiceID = p.InvoiceID

	CREATE TABLE #PrescriptionTotals (PrescriptionID INT NOT NULL PRIMARY KEY, 
						CalendarMonth TINYINT NOT NULL, InvAmt MONEY NOT NULL)
	INSERT #PrescriptionTotals (PrescriptionID, CalendarMonth, InvAmt)
	SELECT u.PrescriptionID, @Jan ,u.Jan CalendarDay FROM #Unpivoted AS u WHERE u.Jan IS NOT NULL
	UNION ALL
	SELECT u.PrescriptionID, @Feb ,u.Feb FROM #Unpivoted AS u WHERE u.Feb IS NOT NULL
	UNION ALL
    SELECT u.PrescriptionID, @Mar, u.Mar FROM #Unpivoted AS u WHERE u.Mar IS NOT NULL
	UNION ALL
	SELECT u.PrescriptionID, @Apr, u.Apr FROM #Unpivoted AS u WHERE u.Apr IS NOT NULL
	UNION ALL
	SELECT u.PrescriptionID, @May, u.May FROM #Unpivoted AS u WHERE u.May IS NOT NULL
	UNION ALL
	SELECT u.PrescriptionID, @Jun, u.Jun FROM #Unpivoted AS u WHERE u.Jun IS NOT NULL
	UNION ALL
	SELECT u.PrescriptionID, @Jul, u.Jul FROM #Unpivoted AS u WHERE u.Jul IS NOT NULL
    UNION ALL
	SELECT u.PrescriptionID, @Aug, u.Aug FROM #Unpivoted AS u WHERE u.Aug IS NOT NULL
	UNION ALL
	SELECT u.PrescriptionID, @Sep, u.Sep FROM #Unpivoted AS u WHERE U.Sep IS NOT NULL
	UNION ALL
	SELECT u.PrescriptionID, @Oct, u.Oct FROM #Unpivoted AS u WHERE U.Oct IS NOT NULL
	UNION ALL
	SELECT u.PrescriptionID, @Nov, u.Nov FROM #Unpivoted AS u WHERE U.Nov IS NOT NULL
	UNION ALL
	SELECT u.PrescriptionID, @Dec, u.[Dec] FROM #Unpivoted AS u WHERE U.[Dec] IS NOT NULL;

	-- #Unpivoted has dissovled and column-combined #PrescriptionTotals, which can give me SUM's of when
	-- all of the payments were made.
	
	WITH TotalsGroupsCTE AS
	(
		SELECT      p.CalendarMonth
				  , SUM(ISNULL(p.InvAmt, 0.00)) TotalInvoiced
		FROM        #PrescriptionTotals AS p
		GROUP BY    p.CalendarMonth
	)
	INSERT #Master ( MonthBilled,CalendarMonth,TotalInvoiced,Jan17,Feb17,Mar17,Apr17,May17
						,Jun17,Jul17,Aug17,Sep17,Oct17,Nov17,Dec17)
	SELECT   c.CalendarMonthNameShort
			,c.CalendarMonth
			,ISNULL(T.TotalInvoiced, 0.00),0,0,0,0,0,0,0,0,0,0,0,0
	FROM     @CalendarMonths AS c
			 LEFT JOIN TotalsGroupsCTE AS t ON t.CalendarMonth = c.CalendarMonth
			 LEFT JOIN #PrescriptionTotals AS p ON p.CalendarMonth = c.CalendarMonth
	GROUP BY c.CalendarMonthNameShort
		   , c.CalendarMonth
		   , t.TotalInvoiced;

	-- Run through Pivot magic 12 times, rather than writing 12, wasteful, PIVOT statements (Have you tried them!!??!!? They're Painful!)
	WHILE (@Looper <= @Dec)
		BEGIN
			WITH PivotCTE AS
			(
				SELECT  @Looper CalendarMonth
					  , pvt.[01-17] 'Jan17'
					  , pvt.[02-17] 'Feb17'
					  , pvt.[03-17] 'Mar17'
					  , pvt.[04-17] 'Apr17'
					  , pvt.[05-17] 'May17'
					  , pvt.[06-17] 'Jun17'
					  , pvt.[07-17] 'Jul17'
					  , pvt.[08-17] 'Aug17'
					  , pvt.[09-17] 'Sep17'
					  , pvt.[10-17] 'Oct17'
					  , pvt.[11-17] 'Nov17'
					  , pvt.[12-17] 'Dec17'
				FROM
						(   SELECT          MonthDay = FORMAT(pp.DatePosted, 'MM-yy')
										  , pp.AmountPaid
							FROM
											(   SELECT  pt.PrescriptionID, DatePosted = DATEFROMPARTS(2017, pt.CalendarMonth, 1)
												FROM    #PrescriptionTotals AS pt
												WHERE	pt.CalendarMonth = @Looper
											) AS o
							INNER JOIN  dbo.PrescriptionPayment AS pp ON pp.PrescriptionID = o.PrescriptionID) AS SourceTable
				PIVOT
				(   SUM(AmountPaid)
					FOR MonthDay IN ([01-17],[02-17],[03-17],[04-17],[05-17],[06-17],[07-17],[08-17],[09-17],[10-17],[11-17],[12-17])) AS pvt
			)
			UPDATE m SET				  
				   m.Jan17 = ISNULL(p.Jan17, 0.00)
				 , m.Feb17 = ISNULL(p.Feb17, 0.00)
				 , m.Mar17 = ISNULL(p.Mar17, 0.00)
				 , m.Apr17 = ISNULL(p.Apr17, 0.00)
				 , m.May17 = ISNULL(p.May17, 0.00)
				 , m.Jun17 = ISNULL(p.Jun17, 0.00)
				 , m.Jul17 = ISNULL(p.Jul17, 0.00)
				 , m.Aug17 = ISNULL(p.Aug17, 0.00)
				 , m.Sep17 = ISNULL(p.Sep17, 0.00)
				 , m.Oct17 = ISNULL(p.Oct17, 0.00)
				 , m.Nov17 = ISNULL(p.Nov17, 0.00)
				 , m.Dec17 = ISNULL(p.Dec17, 0.00)
			FROM   #Master AS m INNER JOIN PivotCTE AS p ON p.CalendarMonth = m.CalendarMonth
			IF @@ROWCOUNT NOT IN (1, 0) -- We're in a time warp!!!!! Loooppp!!!
				BEGIN
					IF @@TRANCOUNT > 0
						ROLLBACK
					RAISERROR(N'Nope, nope, nope, we can only update 12 records.', 16, 1) WITH NOWAIT
					RETURN
				END
			-- Continue Loop
			SET @Looper += 1
		END
	SELECT m.MonthBilled
         , m.TotalInvoiced
         , m.Jan17
         , m.Feb17
         , m.Mar17
         , m.Apr17
         , m.May17
         , m.Jun17
         , m.Jul17
         , m.Aug17
         , m.Sep17
         , m.Oct17
         , m.Nov17
         , m.Dec17 
	FROM #Master AS m
	ORDER BY m.CalendarMonth ASC


END

GO
