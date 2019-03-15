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
AS
    BEGIN
        DECLARE @DateTime DATETIMEOFFSET;
        SELECT @DateTime = @UTCDate AT TIME ZONE 'Mountain Standard Time';
        RETURN CAST(DATEADD(MINUTE, DATEPART(tz, @DateTime), @DateTime) AS DATETIME2);
    END;
GO
