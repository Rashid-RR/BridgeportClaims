SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/19/2017
	Description:	Does the outer level conversion of UTC to the default time Zone (currently Mount Time)
	Sample Execute:
					SELECT dtme.GetLocalDateTime(GETDATE())
*/
CREATE FUNCTION [dtme].[GetLocalDateTime]
(
	@UTCDate DATETIME
)
RETURNS DATETIME
AS
BEGIN
	DECLARE @LocalDateTime DATETIME
	DECLARE @DltBiasFactor SMALLINT

	DECLARE @Display NVARCHAR(50)
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

	DECLARE @DaylightDate DATETIME
	DECLARE @StandardDate DATETIME

	SET @DltBiasFactor = 0

	SELECT @Display = Display
		 , @Bias = ( -1 * Bias )
		 , @DltBias = ( -1 * DltBias )
		 , @StdMonth = StdMonth
		 , @StdDow = StdDayOfWeek + 1
		 , @StdWeek = StdWeek
		 , @StdHour = StdHour
		 , @DltMonth = DltMonth
		 , @DltDow = DltDayOfWeek + 1
		 , @DltWeek = DltWeek
		 , @DltHour = DltHour
	FROM   [dtme].[TimeZoneInfo]
	WHERE  IsDefault = 1

	IF @StdMonth = 0
		SET @LocalDateTime = DATEADD(MINUTE, @Bias , @UTCDate)
	ELSE
	BEGIN
		SET @StandardDate = dtme.udfGetDaylightStandardDateTime(DATEPART(YEAR, @UTCDate ), @StdMonth, @StdDow, @StdWeek, @StdHour)
		SET @DaylightDate = dtme.udfGetDaylightStandardDateTime(DATEPART(YEAR, @UTCDate ), @DltMonth, @DltDow, @DltWeek, @DltHour)

	
		IF @StandardDate > @DaylightDate
			IF DATEADD(MINUTE, @Bias, @UTCDate )  BETWEEN @DaylightDate AND @StandardDate
				SET @DltBiasFactor = 1
		ELSE
		BEGIN
			IF ( DATEADD( minute, @Bias, @UTCDate )  BETWEEN @StandardDate AND @DaylightDate )
				SET @DltBiasFactor = 0
		END
		SET @LocalDateTime = DATEADD(MINUTE, @Bias + ( @DltBiasFactor * @DltBias ) , @UTCDate )
	END
	
	RETURN @LocalDateTime
END 


GO
