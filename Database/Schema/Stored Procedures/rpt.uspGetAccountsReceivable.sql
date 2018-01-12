SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/12/2017
	Description:	Report that shows when the invoiced amounts for the month were actually paid.
	Sample Execute:
					EXEC rpt.uspGetAccountsReceivable NULL, NULL
*/
CREATE PROCEDURE [rpt].[uspGetAccountsReceivable]
(
	@GroupName VARCHAR(255),
	@PharmacyName VARCHAR(60)
)
WITH RECOMPILE
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @InternalGroupName VARCHAR(255) = @GroupName,
			@InternalPharmacyName VARCHAR(60) = @PharmacyName;

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
	DECLARE @GroupName VARCHAR(255)
			,@PharmacyName VARCHAR(60)
			,@InternalGroupName VARCHAR(255)
			,@InternalPharmacyName VARCHAR(60)
	*/
	

	CREATE TABLE #Master (MonthBilled VARCHAR(100) NOT NULL, YearBilled SMALLINT, CalendarMonth TINYINT NOT NULL,
			CalendarYear SMALLINT NULL, TotalInvoiced MONEY NOT NULL, Mnth1 MONEY NOT NULL, Mnth2 MONEY NOT NULL, 
			Mnth3 MONEY NOT NULL, Mnth4 MONEY NOT NULL, Mnth5 MONEY NOT NULL, Mnth6 MONEY NOT NULL, Mnth7 MONEY NOT NULL,
			Mnth8 MONEY NOT NULL, Mnth9 MONEY NOT NULL, Mnth10 MONEY NOT NULL, Mnth11 MONEY NOT NULL, Mnth12 MONEY NOT NULL);
	DECLARE @CalendarMonths TABLE (CalendarMonth TINYINT NOT NULL, CalendarYear SMALLINT NOT NULL, [CalendarMonthNameLong] VARCHAR(100) NOT NULL,
								PRIMARY KEY ([CalendarMonth], [CalendarYear]));

	DECLARE @CalendarYear INT = YEAR([dtme].[udfGetLocalDate]());
	DECLARE @CalendarMonth INT = MONTH([dtme].[udfGetLocalDate]());
	DECLARE @StartOfThisMonth DATE = CAST(DATEADD(MONTH, DATEDIFF(MONTH, 0, [dtme].[udfGetLocalDate]()), 0) AS DATE);
	DECLARE @InnerEndDate DATE = EOMONTH([dtme].[udfGetLocalDate]());
	DECLARE @InnerStartDate DATE = DATEADD(MONTH, -11, @StartOfThisMonth);

	WITH CalendarOrderingTrickCTE AS
	(
		SELECT	DISTINCT c.CalendarMonth, c.CalendarYear, c.[CalendarMonthNameLong]
		FROM	dtme.Calendar AS c
		WHERE	c.DateID BETWEEN EOMONTH(@InnerStartDate) AND EOMONTH(@InnerEndDate)
	)
	INSERT	@CalendarMonths (CalendarMonth, CalendarYear, CalendarMonthNameLong)
	SELECT c.CalendarMonth,c.CalendarYear,c.CalendarMonthNameLong
	FROM CalendarOrderingTrickCTE AS c 
	ORDER BY c.CalendarYear ASC, c.CalendarMonth ASC
	
	CREATE TABLE #MonthYearBillingTotals(
		InvoicedMonth TINYINT NOT NULL,
		InvoicedYear SMALLINT NOT NULL,
		TotalBilledAmount MONEY NOT NULL,
		CHECK (InvoicedMonth BETWEEN 1 AND 12),
		PRIMARY KEY CLUSTERED (InvoicedMonth ASC, InvoicedYear ASC)
		WITH (FILLFACTOR = 90, DATA_COMPRESSION = ROW))

	INSERT #MonthYearBillingTotals (InvoicedMonth,InvoicedYear,TotalBilledAmount)
	SELECT  [cm].[CalendarMonth], [cm].[CalendarYear], ISNULL([A].[TotalBilledAmount], 0.0)
	FROM    @CalendarMonths AS [cm]
	LEFT JOIN
		(   SELECT      f.InvoicedMonth
						, f.InvoicedYear
						, SUM(f.BilledAmount) TotalBilledAmount
			FROM        dbo.udfGetPrescriptionBilling(@InnerStartDate, @InnerEndDate, @InternalGroupName, @InternalPharmacyName) AS f
			GROUP BY    f.InvoicedMonth
						, f.InvoicedYear) AS A ON A.[InvoicedMonth] = [cm].[CalendarMonth]
												AND [A].[InvoicedYear] = [cm].[CalendarYear]

	INSERT #Master ( MonthBilled,[YearBilled],CalendarMonth,[CalendarYear],TotalInvoiced,Mnth1,Mnth2,Mnth3,Mnth4,Mnth5
						,Mnth6,Mnth7,Mnth8,Mnth9,Mnth10,Mnth11,Mnth12)
	SELECT	 c.[CalendarMonthNameLong]
			,c.CalendarYear
			,c.[CalendarMonth]
			,c.[CalendarYear]
			,ISNULL(t.TotalBilledAmount, 0.00),0,0,0,0,0,0,0,0,0,0,0,0
	FROM     @CalendarMonths AS c
			 LEFT JOIN #MonthYearBillingTotals AS t ON t.InvoicedMonth = c.CalendarMonth AND t.InvoicedYear = c.CalendarYear
	ORDER BY c.[CalendarYear] ASC, c.[CalendarMonth] ASC

	-- Run through each billable month and update when payments were received for those billed amounts

	-- Create a mapping for 1 - 12 regardless of month and year, as long as its in chronological order, we can map it back.
	DECLARE @ChronologicalMapping TABLE
	(Idx TINYINT NOT NULL, Interval CHAR(5) NOT NULL)
	INSERT @ChronologicalMapping
	([Idx],[Interval])
	SELECT      ROW_NUMBER() OVER (ORDER BY [t].[InvoicedYear] ASC) Idx, 
				FORMAT(DATEFROMPARTS([t].[InvoicedYear], [t].[InvoicedMonth], 1), 'MM-yy')
	FROM        #MonthYearBillingTotals AS t
	ORDER BY    t.[InvoicedYear] ASC
			  , t.[InvoicedMonth] ASC

	DECLARE PrescriptionPaymentsUpdater CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
	SELECT  c.CalendarMonth
		  , c.CalendarYear
	FROM    @CalendarMonths AS c INNER JOIN #MonthYearBillingTotals AS m ON m.InvoicedMonth = c.CalendarMonth
	WHERE	m.InvoicedYear = c.CalendarYear
	ORDER BY c.[CalendarYear] ASC, [c].[CalendarMonth] ASC

	OPEN PrescriptionPaymentsUpdater;
	
	FETCH NEXT FROM PrescriptionPaymentsUpdater INTO @CalendarMonth, @CalendarYear

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @LoopStart DATE = DATEFROMPARTS(@CalendarYear, @CalendarMonth, 1),
				@LoopEnd DATE = EOMONTH(DATEFROMPARTS(@CalendarYear, @CalendarMonth, 1));
	    WITH PivotCTE AS
		(
				SELECT  @CalendarMonth CalendarMonth
					  , pvt.[1] [Mnth1]
					  , pvt.[2] [Mnth2]
					  , pvt.[3] [Mnth3]
					  , pvt.[4] [Mnth4]
					  , pvt.[5] [Mnth5]
					  , pvt.[6] [Mnth6]
					  , pvt.[7] [Mnth7]
					  , pvt.[8] [Mnth8]
					  , pvt.[9] [Mnth9]
					  , pvt.[10] [Mnth10]
					  , pvt.[11] [Mnth11]
					  , pvt.[12] [Mnth12]
				FROM
						(   SELECT	 [cm].[Idx]
									,pp.AmountPaid
							FROM	dbo.PrescriptionPayment AS pp
									INNER JOIN dbo.udfGetPrescriptionBilling(@LoopStart, @LoopEnd, @InternalGroupName, @InternalPharmacyName) AS f ON f.PrescriptionID = pp.PrescriptionID
									INNER JOIN @ChronologicalMapping AS [cm] ON FORMAT(pp.DatePosted, 'MM-yy') = [cm].[Interval]
							) AS SourceTable
				PIVOT
				(   SUM(AmountPaid)
					FOR [Idx] IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS pvt
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

	SELECT m.MonthBilled DateBilled
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
