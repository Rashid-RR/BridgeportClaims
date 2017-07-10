SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:				Bill Roe
	Date Written:		2014-Mar-07
	Description:		Returns a table of values based on comma-delimited string passed in.

	TEST:
		SELECT Item FROM utilities.udfSplit('Aa',',');
		SELECT Item FROM utilities.udfSplit('Aa,Bb,Cc,Dd,Ee',',');
		SELECT Item FROM utilities.udfSplit('Apples|Berries|Cherries|Double Deckers|Eggberts','|');

	MODIFICATION HISTORY
	--------------------
	2014-Mar-07 BGR		Created.
*/
CREATE FUNCTION [dbo].[udfSplit]
(
	@input		AS VARCHAR(MAX),
	@delimiter	AS CHAR(1)
)
RETURNS @Result TABLE(Item VARCHAR(100))
AS  
BEGIN  
	DECLARE
		@str VARCHAR(20),  
		@ind INT;

	IF(@input IS NOT NULL)
	BEGIN
		SET @ind = CHARINDEX(@delimiter,@input);
		WHILE (@ind > 0)
		BEGIN
			SET @str = LTRIM(RTRIM(SUBSTRING(@input,1,@ind-1)));
			SET @input = SUBSTRING(@input,@ind+1,LEN(@input)-@ind);
			INSERT INTO @Result VALUES (@str)
				SET @ind = CHARINDEX(@delimiter,@input);
		END;
		SET @str = @input;
		INSERT INTO @Result VALUES (@str)
	END;
	RETURN;
END;
GO
