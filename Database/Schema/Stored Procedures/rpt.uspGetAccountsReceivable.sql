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
	@StartDate DATE, -- Covers whole month
	@EndDate DATE, -- Covers whole month
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

	CREATE TABLE #Master (MonthBilled VARCHAR(10) NOT NULL, CalendarMonth TINYINT NOT NULL,
			CalendarYear SMALLINT, TotalInvoiced MONEY NOT NULL, Jan17 MONEY NOT NULL, Feb17 MONEY NOT NULL, 
			Mar17 MONEY NOT NULL, Apr17 MONEY NOT NULL, May17 MONEY NOT NULL, Jun17 MONEY NOT NULL, Jul17 MONEY NOT NULL,
			Aug17 MONEY NOT NULL, Sep17 MONEY NOT NULL, Oct17 MONEY NOT NULL, Nov17 MONEY NOT NULL, Dec17 MONEY NOT NULL);
	DECLARE @CalendarMonths TABLE (CalendarMonthNameShort VARCHAR(50) NOT NULL PRIMARY KEY, 
								CalendarMonth TINYINT NOT NULL, CalendarYear SMALLINT NOT NULL);
	WITH CalendarOrderingTrickCTE AS
	(
		SELECT	DISTINCT c.CalendarMonthNameShort, c.CalendarMonth, c.CalendarYear
		FROM	dtme.Calendar AS c
		WHERE	c.DateID BETWEEN EOMONTH(@StartDate) AND EOMONTH(@EndDate)
	)
	INSERT	@CalendarMonths (CalendarMonthNameShort, CalendarMonth, CalendarYear)
	SELECT c.CalendarMonthNameShort,c.CalendarMonth,c.CalendarYear 
	FROM CalendarOrderingTrickCTE AS c ORDER BY c.CalendarMonth ASC

	CREATE TABLE [#MonthYearBillingTotals](
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
	FROM    dbo.udfGetPrescriptionBilling(@StartDate, @EndDate, @GroupName, @PharmacyName) AS f
	GROUP BY f.InvoicedMonth, f.InvoicedYear

	INSERT #Master ( MonthBilled,CalendarMonth,CalendarYear,TotalInvoiced,Jan17,Feb17,Mar17,Apr17,May17
						,Jun17,Jul17,Aug17,Sep17,Oct17,Nov17,Dec17)
	SELECT   c.CalendarMonthNameShort
			,c.CalendarMonth
			,c.CalendarYear
			,ISNULL(t.TotalBilledAmount, 0.00),0,0,0,0,0,0,0,0,0,0,0,0
	FROM     @CalendarMonths AS c
			 LEFT JOIN #MonthYearBillingTotals AS t ON t.InvoicedMonth = c.CalendarMonth AND t.InvoicedYear = c.CalendarYear

	-- Run through each billable month and update when payments were received for those billed amounts.
	DECLARE @CalendarMonth TINYINT, @CalendarYear SMALLINT
	
	DECLARE PrescriptionPaymentsUpdater CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
	SELECT  c.CalendarMonth
		  , c.CalendarYear
	FROM    @CalendarMonths AS c INNER JOIN #MonthYearBillingTotals AS m ON m.InvoicedMonth = c.CalendarMonth
	WHERE	m.InvoicedYear = c.CalendarYear
			
	OPEN PrescriptionPaymentsUpdater;
	
	FETCH NEXT FROM PrescriptionPaymentsUpdater INTO @CalendarMonth, @CalendarYear
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @InnerStartDate DATE = DATEFROMPARTS(@CalendarYear, @CalendarMonth, 1)
		DECLARE @InnerEndDate DATE = EOMONTH(@InnerStartDate);
	    WITH PivotCTE AS
			(
				SELECT  @CalendarMonth CalendarMonth
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
						(   SELECT	MonthDay = FORMAT(pp.DatePosted, 'MM-yy')
									,pp.AmountPaid
							FROM	dbo.PrescriptionPayment AS pp
									INNER JOIN dbo.udfGetPrescriptionBilling(@InnerStartDate, @InnerEndDate, @GroupName, @PharmacyName) AS f ON f.PrescriptionID = pp.PrescriptionID
							) AS SourceTable
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
	
	    FETCH NEXT FROM PrescriptionPaymentsUpdater INTO @CalendarMonth, @CalendarYear
	END
	
	CLOSE PrescriptionPaymentsUpdater;
	DEALLOCATE PrescriptionPaymentsUpdater;

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
