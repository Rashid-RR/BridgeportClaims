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
CREATE PROCEDURE [rpt].[uspGetAccountsReceivable] @StartDate DATE
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @StartMonth INT = 1 -- TODO: HACK, FIX.
	DECLARE @StartYear INT = 2017 -- TODO: HACK, FIX.
	DECLARE @TimeInterval INT = 12 -- TODO: HACK, FIX.

	CREATE TABLE #Master (MonthBilled VARCHAR(10) NOT NULL, CalendarMonth TINYINT NOT NULL, TotalInvoiced MONEY NOT NULL, Jan17 MONEY NOT NULL, Feb17 MONEY NOT NULL,	
			Mar17 MONEY NOT NULL, Apr17 MONEY NOT NULL, May17 MONEY NOT NULL, Jun17 MONEY NOT NULL, Jul17 MONEY NOT NULL,
			Aug17 MONEY NOT NULL, Sep17 MONEY NOT NULL, Oct17 MONEY NOT NULL, Nov17 MONEY NOT NULL, Dec17 MONEY NOT NULL);
	
	DECLARE @CalendarMonths TABLE (CalendarMonthNameShort VARCHAR(50) NOT NULL PRIMARY KEY, 
									CalendarMonth TINYINT NOT NULL, CalendarYear INT NOT NULL)
	INSERT	@CalendarMonths (CalendarMonthNameShort, CalendarMonth, CalendarYear)
	SELECT	DISTINCT c.CalendarMonthNameShort, c.CalendarMonth, @StartYear CalendarYear
	FROM	dtme.Calendar AS c
	WHERE	c.DateID BETWEEN EOMONTH(@StartDate) AND DATEADD(MONTH, @TimeInterval, @StartDate)

	DECLARE @TotalInvoiced TABLE (Total MONEY NOT NULL)



	INSERT #Master (MonthBilled,CalendarMonth,TotalInvoiced,Jan17,Feb17,Mar17,Apr17,May17,Jun17,Jul17,Aug17,Sep17,Oct17,Nov17,Dec17)
	SELECT c.CalendarMonthNameShort + '-17',c.CalendarMonth,ISNULL(Jan.Total, 0.00),ISNULL(Feb.Total, 0.00),
			ISNULL(Mar.Total, 0.00), ISNULL(Apr.Total, 0.00),0,0,0,0,0,0,0,0,0
    FROM @CalendarMonths c
	LEFT JOIN
		(SELECT          SUM(p.BilledAmount) Total, @StartMonth CalendarMonth, @StartYear CalendarYear
		 FROM            dbo.Prescription AS p
						 INNER JOIN  dbo.Invoice AS i ON i.InvoiceID = p.InvoiceID
		 WHERE			 MONTH(i.InvoiceDate) = @StartMonth
						 AND YEAR(i.InvoiceDate) = @StartYear) Jan ON 
							Jan.CalendarMonth = c.CalendarMonth AND Jan.CalendarYear = c.CalendarYear
	LEFT JOIN
		(SELECT          SUM(p.BilledAmount) Total, @StartMonth + 1 CalendarMonth, @StartYear CalendarYear
		 FROM            dbo.Prescription AS p
						 INNER JOIN  dbo.Invoice AS i ON i.InvoiceID = p.InvoiceID
		 WHERE			 MONTH(i.InvoiceDate) = @StartMonth + 1
						 AND YEAR(i.InvoiceDate) = @StartYear) Feb ON 
							Jan.CalendarMonth = c.CalendarMonth AND Feb.CalendarYear = c.CalendarYear
	LEFT JOIN
		(SELECT          SUM(p.BilledAmount) Total, @StartMonth + 2 CalendarMonth, @StartYear CalendarYear
		 FROM            dbo.Prescription AS p
						 INNER JOIN  dbo.Invoice AS i ON i.InvoiceID = p.InvoiceID
		 WHERE			 MONTH(i.InvoiceDate) = @StartMonth + 2
						 AND YEAR(i.InvoiceDate) = @StartYear) Mar ON 
							Jan.CalendarMonth = c.CalendarMonth AND Mar.CalendarYear = c.CalendarYear
	LEFT JOIN
		(SELECT          SUM(p.BilledAmount) Total, @StartMonth + 3 CalendarMonth, @StartYear CalendarYear
		 FROM            dbo.Prescription AS p
						 INNER JOIN  dbo.Invoice AS i ON i.InvoiceID = p.InvoiceID
		 WHERE			 MONTH(i.InvoiceDate) = @StartMonth + 3
						 AND YEAR(i.InvoiceDate) = @StartYear) Apr ON 
							Jan.CalendarMonth = c.CalendarMonth AND Apr.CalendarYear = c.CalendarYear
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
