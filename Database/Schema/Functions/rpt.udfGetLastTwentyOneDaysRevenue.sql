SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/6/2017
	Description:	Returns the Revenue for the last 21 days report.
	Sample Execute:
					SELECT * FROM rpt.udfGetLastTwentyOneDaysRevenue()
*/
CREATE FUNCTION [rpt].[udfGetLastTwentyOneDaysRevenue]()
RETURNS @Table TABLE
(   DatePosted  DATE  NOT NULL
  , TotalPosted MONEY NOT NULL)
AS
BEGIN
    DECLARE @Today DATE = CONVERT(DATE, dtme.udfGetLocalDate())
    DECLARE @TwentyOneDaysAgo DATE = DATEADD(DAY, -21, @Today)

	DECLARE @Days TABLE (DatePosted DATE NOT NULL PRIMARY KEY)

	INSERT @Days (DatePosted)
	SELECT  c.DateID
	FROM    dtme.Calendar AS c
	WHERE   c.DateID BETWEEN @TwentyOneDaysAgo AND @Today

	INSERT @Table (DatePosted, TotalPosted)
	SELECT      d.DatePosted
			  , ISNULL(p.TotalPosted, 0.00)
	FROM        @Days                         AS d
		LEFT JOIN
				(   SELECT      DatePosted  = pp.DatePosted
							  , TotalPosted = SUM(pp.AmountPaid)
					FROM        dbo.PrescriptionPayment AS pp
					WHERE       pp.DatePosted BETWEEN @TwentyOneDaysAgo AND @Today
					GROUP   BY  pp.DatePosted) AS p ON p.DatePosted = d.DatePosted
	ORDER BY    d.DatePosted ASC
	RETURN
END
GO
