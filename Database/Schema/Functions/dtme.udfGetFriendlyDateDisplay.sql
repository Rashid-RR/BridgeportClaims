SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/25/2017
	Description:	Displays a friendly date and time for the Claims system.
	Sample Execute:
					SELECT dtme.udfGetFriendlyDateDisplay()
*/
CREATE FUNCTION [dtme].[udfGetFriendlyDateDisplay]()
RETURNS VARCHAR(255)
AS BEGIN RETURN
(      
	SELECT IIF([c].[CalendarQuarterDescription] IS NOT NULL
			 , [c].[CalendarQuarterDescription]
			 , '') + '  |  ' + [c].[MonthDayYearNameLong]
		   + IIF([c].[DateDescription] IS NOT NULL
			   , '  |  ' + [c].[DateDescription]
			   , '')
	FROM   [dtme].[Calendar] AS [c]
	WHERE  [c].[DateID] = CONVERT(DATE, [dtme].[udfGetLocalDateTime](SYSUTCDATETIME()))
)
END
GO
