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
CREATE FUNCTION [dtme].[udfGetLocalDateTime]
(
	@UTCDate DATETIME2
)
RETURNS DATETIME2
WITH SCHEMABINDING
AS
BEGIN

DECLARE @TimeZoneID SMALLINT
      , @LocalDateTime DATETIME2
      , @DltBiasFactor SMALLINT
      , @Display NVARCHAR(50)
      , @Bias INT
      , @DltBias INT
      , @StdMonth SMALLINT
      , @StdDow SMALLINT
      , @StdWeek SMALLINT
      , @StdHour SMALLINT
      , @DltMonth SMALLINT
      , @DltDow SMALLINT
      , @DltWeek SMALLINT
      , @DltHour SMALLINT

SELECT @TimeZoneID = tz.TimeZoneID
FROM   [dtme].[TimeZone] AS [tz]
WHERE  tz.IsDefault = 1

IF @TimeZoneID IS NULL
	-- Print error. RAISERROR not allowed within a function
	SET @TimeZoneID = CONVERT(TINYINT, N'Error, a default time zone was not found')

DECLARE @DaylightDate DATETIME2
DECLARE @StandardDate DATETIME2

SET @DltBiasFactor = 0

SELECT @Display = Display
     , @Bias = ( -1 * Bias)
     , @DltBias = ( -1 * DltBias)
     , @StdMonth = StdMonth
     , @StdDow = StdDayOfWeek + 1
     , @StdWeek = StdWeek
     , @StdHour = StdHour
     , @DltMonth = DltMonth
     , @DltDow = DltDayOfWeek + 1
     , @DltWeek = DltWeek
     , @DltHour = DltHour
FROM   [dtme].[TimeZone]
WHERE  TimeZoneID = @TimeZoneID

IF @StdMonth = 0
	SET @LocalDateTime = DATEADD(MINUTE, @Bias , @UTCDate)
ELSE
	BEGIN
		SET @StandardDate =  dtme.udfGetDaylightStandardDateTime(DATEPART(YEAR, @UTCDate), @StdMonth, @StdDow, @StdWeek, @StdHour)
		SET @DaylightDate = dtme.udfGetDaylightStandardDateTime(DATEPART(YEAR, @UTCDate), @DltMonth, @DltDow, @DltWeek, @DltHour)	
		IF @StandardDate > @DaylightDate
			IF DATEADD(MINUTE, @Bias, @UTCDate)  BETWEEN @DaylightDate AND @StandardDate
				SET @DltBiasFactor = 1
		ELSE
			IF DATEADD(MINUTE, @Bias, @UTCDate)  BETWEEN @StandardDate AND @DaylightDate
				SET @DltBiasFactor = 0
		SET @LocalDateTime = DATEADD(MINUTE, @Bias + (@DltBiasFactor * @DltBias) , @UTCDate)
	END
	-- Very Verbose
	-- RETURN  'Time Zone ID:' + CAST( @TimeZoneID  AS CHAR(2)) + ' - '  + @Display + ' - <UTC DT:' + CAST ( @UTCDate AS CHAR(20)) + '> - <Local DT:' + CAST(  @LocalDateTime AS CHAR(20)) + '>'
	RETURN @LocalDateTime
END 
GO
