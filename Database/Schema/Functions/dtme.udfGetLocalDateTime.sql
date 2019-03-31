SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/19/2017
	Description:	Final function that returns local MST instead of UTC time.
	Sample Execute:
					SELECT dtme.udfGetLocalDateTime(SYSUTCDATETIME())
*/
CREATE FUNCTION [dtme].[udfGetLocalDateTime] (@UTCDate DATETIME2)
RETURNS DATETIME2
WITH SCHEMABINDING
AS BEGIN
	RETURN (
		SELECT CAST(DATEADD(MINUTE, DATEPART(tz, @UTCDate AT TIME ZONE 'Mountain Standard Time'), @UTCDate AT TIME ZONE 'Mountain Standard Time') AS DATETIME2)
	)
END
GO
