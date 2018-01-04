SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/12/2017
	Description:	Report that shows when the invoiced amounts for the month were actually paid.
	Sample Execute:
					EXEC rpt.uspGetAccountsReceivable '1/1/2017', '12/1/2017', NULL, NULL
*/
CREATE PROCEDURE [rpt].[uspGetAccountsReceivable]
(
	@GroupName VARCHAR(255),
	@PharmacyName VARCHAR(60)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @MnthOne TINYINT = 1
		   ,@MnthTwo TINYINT = 2
		   ,@MnthThree TINYINT = 3
		   ,@MnthFour TINYINT = 4
		   ,@MnthFive TINYINT = 5
		   ,@MnthSix TINYINT = 6
		   ,@MnthSeven TINYINT = 7
		   ,@MnthEight TINYINT = 8
		   ,@MnthNine TINYINT = 9
		   ,@MnthTen TINYINT = 10
		   ,@MnthEleven TINYINT = 11
		   ,@MnthTwelve TINYINT = 12;

	/*   
	-- Testing
	IF OBJECT_ID('tempdb..[#Master]') IS NOT NULL
		DROP TABLE [#Master]
	IF OBJECT_ID('tempdb..[#PrescriptionTotals]') IS NOT NULL
		DROP TABLE [#PrescriptionTotals]
	IF OBJECT_ID('tempdb..[#MonthYearBillingTotals]') IS NOT NULL
		DROP TABLE [#MonthYearBillingTotals]
	DECLARE @StartDate DATE = '1/1/2017'
	DECLARE @EndDate DATE = '12/1/2017'
			,@GroupName VARCHAR(255)
			,@PharmacyName VARCHAR(60)
	*/

	CREATE TABLE #Master (MonthBilled VARCHAR(10) NOT NULL, YearBilled SMALLINT, CalendarMonth TINYINT NOT NULL,
			CalendarYear SMALLINT NULL, TotalInvoiced MONEY NOT NULL, Mnth1 MONEY NOT NULL, Mnth2 MONEY NOT NULL, 
			Mnth3 MONEY NOT NULL, Mnth4 MONEY NOT NULL, Mnth5 MONEY NOT NULL, Mnth6 MONEY NOT NULL, Mnth7 MONEY NOT NULL,
			Mnth8 MONEY NOT NULL, Mnth9 MONEY NOT NULL, Mnth10 MONEY NOT NULL, Mnth11 MONEY NOT NULL, Mnth12 MONEY NOT NULL);
	DECLARE @CalendarMonths TABLE (CalendarMonthNameShort VARCHAR(50) NOT NULL, 
								CalendarMonth TINYINT NOT NULL, CalendarYear SMALLINT NOT NULL,
								PRIMARY KEY ([CalendarMonth], [CalendarYear]));

	DECLARE @CalendarYear INT = YEAR([dtme].[udfGetLocalDate]())
	DECLARE @CalendarMonth INT = MONTH([dtme].[udfGetLocalDate]())
	DECLARE @InnerStartDate DATE = DATEFROMPARTS(@CalendarYear, @CalendarMonth, 1)
	DECLARE @InnerEndDate DATE = EOMONTH(@InnerStartDate);

	WITH CalendarOrderingTrickCTE AS
	(
		SELECT	DISTINCT c.CalendarMonthNameShort, c.CalendarMonth, c.CalendarYear
		FROM	dtme.Calendar AS c
		WHERE	c.DateID BETWEEN EOMONTH(@InnerStartDate) AND EOMONTH(@InnerEndDate)
	)
	INSERT	@CalendarMonths (CalendarMonthNameShort, CalendarMonth, CalendarYear)
	SELECT c.CalendarMonthNameShort,c.CalendarMonth,c.CalendarYear 
	FROM CalendarOrderingTrickCTE AS c ORDER BY c.CalendarMonth ASC

	CREATE TABLE #MonthYearBillingTotals(
		InvoicedMonth TINYINT NOT NULL,
		InvoicedYear SMALLINT NOT NULL,
		TotalBilledAmount MONEY NOT NULL,
		CHECK (InvoicedMonth BETWEEN 1 AND 12),
		PRIMARY KEY CLUSTERED (InvoicedMonth ASC, InvoicedYear ASC)
		WITH (FILLFACTOR = 90, DATA_COMPRESSION = ROW))
	INSERT #MonthYearBillingTotals (InvoicedMonth,InvoicedYear,TotalBilledAmount)
	SELECT  f.InvoicedMonth
			,f.InvoicedYear
			,SUM(f.BilledAmount) TotalBilledAmount
	FROM    dbo.udfGetPrescriptionBilling(@InnerStartDate, @InnerEndDate, @GroupName, @PharmacyName) AS f
	GROUP BY f.InvoicedMonth, f.InvoicedYear

	INSERT #Master ( MonthBilled,CalendarMonth,CalendarYear,TotalInvoiced,Mnth1,Mnth2,Mnth3,Mnth4,Mnth5
						,Mnth6,Mnth7,Mnth8,Mnth9,Mnth10,Mnth11,Mnth12)
	SELECT   c.CalendarMonthNameShort
			,c.CalendarMonth
			,c.CalendarYear
			,ISNULL(t.TotalBilledAmount, 0.00),0,0,0,0,0,0,0,0,0,0,0,0
	FROM     @CalendarMonths AS c
			 LEFT JOIN #MonthYearBillingTotals AS t ON t.InvoicedMonth = c.CalendarMonth AND t.InvoicedYear = c.CalendarYear

	-- Run through each billable month and update when payments were received for those billed amounts
	
	DECLARE PrescriptionPaymentsUpdater CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
	SELECT  c.CalendarMonth
		  , c.CalendarYear
	FROM    @CalendarMonths AS c INNER JOIN #MonthYearBillingTotals AS m ON m.InvoicedMonth = c.CalendarMonth
	WHERE	m.InvoicedYear = c.CalendarYear
			
	OPEN PrescriptionPaymentsUpdater;
	
	FETCH NEXT FROM PrescriptionPaymentsUpdater INTO @CalendarMonth, @CalendarYear
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	    WITH PivotCTE AS
			(
				SELECT  @CalendarMonth CalendarMonth
					  , pvt.[01] [Mnth1]
					  , pvt.[02-17] [Mnth2]
					  , pvt.[03-17] [Mnth3]
					  , pvt.[04-17] [Mnth4]
					  , pvt.[05-17] [Mnth5]
					  , pvt.[06-17] [Mnth6]
					  , pvt.[07-17] [Mnth7]
					  , pvt.[08-17] [Mnth8]
					  , pvt.[09-17] [Mnth9]
					  , pvt.[10-17] [Mnth10]
					  , pvt.[11-17] [Mnth11]
					  , pvt.[12-17] [Mnth12]
				FROM
						(   SELECT	FORMAT(pp.DatePosted, 'MM-yy') MonthDay
									,pp.AmountPaid
							FROM	dbo.PrescriptionPayment AS pp
									INNER JOIN dbo.udfGetPrescriptionBilling(@InnerStartDate, @InnerEndDate, @GroupName, @PharmacyName) AS f ON f.PrescriptionID = pp.PrescriptionID
							) AS SourceTable
				PIVOT
				(   SUM(AmountPaid)
					FOR MonthDay IN ([01],[02-17],[03-17],[04-17],[05-17],[06-17],[07-17],[08-17],[09-17],[10-17],[11-17],[12-17])) AS pvt
			)
			UPDATE m SET				  
				   m.Mnth1 = ISNULL(p.[Mnth1], 0.00)
				 , m.Mnth2 = ISNULL(p.[Mnth2], 0.00)
				 , m.Mnth3 = ISNULL(p.[Mnth3], 0.00)
				 , m.Mnth4 = ISNULL(p.[Mnth4], 0.00)
				 , m.Mnth5 = ISNULL(p.[Mnth5], 0.00)
				 , m.Mnth6 = ISNULL(p.[Mnth6], 0.00)
				 , m.Mnth7 = ISNULL(p.[Mnth7], 0.00)
				 , m.Mnth8 = ISNULL(p.[Mnth8], 0.00)
				 , m.Mnth9 = ISNULL(p.[Mnth9], 0.00)
				 , m.Mnth10 = ISNULL(p.[Mnth10], 0.00)
				 , m.Mnth11 = ISNULL(p.[Mnth11], 0.00)
				 , m.Mnth12 = ISNULL(p.[Mnth12], 0.00)
			FROM   #Master AS m INNER JOIN PivotCTE AS p ON p.CalendarMonth = m.CalendarMonth
	
	    FETCH NEXT FROM PrescriptionPaymentsUpdater INTO @CalendarMonth, @CalendarYear
	END
	
	CLOSE PrescriptionPaymentsUpdater;
	DEALLOCATE PrescriptionPaymentsUpdater;

	SELECT m.MonthBilled
		 , m.[CalendarYear] YearBilled
         , m.TotalInvoiced
         , m.Mnth1
         , m.Mnth2
         , m.Mnth3
         , m.Mnth4
         , m.Mnth5
         , m.Mnth6
         , m.Mnth7
         , m.Mnth8
         , m.Mnth9
         , m.Mnth10
         , m.Mnth11
         , m.Mnth12 
	FROM #Master AS m
	ORDER BY m.[CalendarYear] ASC, m.CalendarMonth ASC
END
GO
