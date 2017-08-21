SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/21/2017
	Description:	Updates all "MONEY" type column values to be 
					rounded to 2 decimal places. This is to ensure
					consistent calculations on Staging and Production.
	Sample Execute:
					EXEC util.uspRoundMoneyTypeColumns
*/
CREATE PROC [util].[uspRoundMoneyTypeColumns]
AS BEGIN
	SET NOCOUNT ON;

	DECLARE @Script NVARCHAR(1000)
	
	DECLARE RoundMoneyTypesCursor CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
	SELECT 'UPDATE '+QUOTENAME(s.name)+'.'+QUOTENAME(tt.name)+' SET '+QUOTENAME(c.name)+' = '+'ROUND('
		   +QUOTENAME(c.name)+', 2)' Script
	FROM sys.columns AS c
		 INNER JOIN sys.tables AS tt ON tt.object_id=c.object_id
		 INNER JOIN sys.schemas AS s ON s.schema_id=tt.schema_id
		 INNER JOIN sys.types AS t ON c.system_type_id=t.system_type_id
	WHERE t.name='money'

	OPEN RoundMoneyTypesCursor;
	
	FETCH NEXT FROM RoundMoneyTypesCursor INTO @Script
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC sys.sp_executesql @Script
	    FETCH NEXT FROM RoundMoneyTypesCursor INTO @Script
	END
	
	CLOSE RoundMoneyTypesCursor;
	DEALLOCATE RoundMoneyTypesCursor;
END
GO
