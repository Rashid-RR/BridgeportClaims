SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/19/2017
	Description:	Deals with the Daylight Savings time aspect of converting a Date and Time.
	Sample Execute:
					SELECT dtme.udfGetDaylightStandardDateTime()
*/
CREATE FUNCTION [dtme].[udfGetDaylightStandardDateTime]
(
    @Year int,        -- a valid year value
    @Month int,        -- 1..12
    @DayOfWeek smallint,    -- 1..7
    @Week smallint,      -- 1..5, 1 - first week, 2 - second, etc.,  5 - the last week
    @Hour smallint -- hour value when daylight or standard time begins.
)
RETURNS datetime
AS
BEGIN
    DECLARE @FirstOfMonth datetime
    DECLARE @DoW smallint
    DECLARE @Ret datetime

    -- find day of the week of the first day of a given month:
    SET @FirstOfMonth = CAST( @Year AS NVARCHAR ) + '/' +  CAST( @Month AS NVARCHAR ) + '/01'

    -- 5th week means the last week of the month, so go one month forth, then one week back
    IF @Week = 5
    BEGIN
        SET @FirstOfMonth = DATEADD( Month, 1, @FirstOfMonth )
    END

    SET @DoW = DATEPART( weekday, @FirstOfMonth )

    -- find first given day of the week of the given month:
    IF @DoW > @DayOfWeek
        SET @Ret = DATEADD( Day, 7 + @DayOfWeek - @DoW , @FirstOfMonth )
    ELSE
        SET @Ret = DATEADD( Day, @DayOfWeek - @DoW , @FirstOfMonth )

    -- advance to the given week ( 5th week means the last one of the month )
    IF @Week < 5
    BEGIN
        SET @Ret = DATEADD( Week, @Week - 1, @Ret )
    END
    ELSE
    BEGIN
        -- the last week of the previous month; go one week backward
        SET @Ret = DATEADD( Week, -1, @Ret )
    END


   SET @Ret = DATEADD( Hour, @Hour, @Ret )

    RETURN @Ret
END

GO
