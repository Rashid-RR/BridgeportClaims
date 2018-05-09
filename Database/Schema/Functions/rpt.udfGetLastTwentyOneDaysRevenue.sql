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
    DECLARE @StartingDay DATE, @TwentyOne TINYINT = 21;

	DECLARE @Days TABLE (DatePosted DATE NOT NULL PRIMARY KEY)

	INSERT @Days (DatePosted)
	SELECT		TOP (@TwentyOne) c.DateID
	FROM		dtme.Calendar AS c
	WHERE		c.IsWeekend = 0
				AND c.DateID <= @Today
	ORDER BY	c.DateID DESC

    SELECT @StartingDay = MIN([d].[DatePosted]) FROM @Days AS [d]

	INSERT		@Table (DatePosted, TotalPosted)
	SELECT      d.DatePosted
			  , ISNULL(p.TotalPosted, 0.00) 
	FROM        @Days                         AS d
		LEFT JOIN
				(   SELECT      DatePosted  = pp.DatePosted
							  , TotalPosted = SUM(pp.AmountPaid)
					FROM        dbo.PrescriptionPayment AS pp
					WHERE       pp.DatePosted BETWEEN @StartingDay AND @Today
					GROUP   BY  pp.DatePosted) AS p ON p.DatePosted = d.DatePosted
	ORDER BY    d.DatePosted ASC
	RETURN
END
GO
