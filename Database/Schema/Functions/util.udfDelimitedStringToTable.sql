SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/31/2017
	Description:	
	Sample Execute:
					SELECT * FROM [util].[udfDelimitedStringToTable](',', '1,2,3,4,5,55,342,232,2324,324,23,23,23,232,43,53,24,234,232,42,42,42,432,5,54,2,2,43,23,43,42,42,23,43,858555,354,34,3,42,4,2,2,32,32,4848484,45235,2523')
*/
CREATE FUNCTION [util].[udfDelimitedStringToTable]
--===== Define I/O parameters
        (@Delimiter CHAR(1), @StringToSplit VARCHAR(8000))
--WARNING!!! DO NOT USE MAX DATA-TYPES HERE!  IT WILL KILL PERFORMANCE!
RETURNS TABLE WITH SCHEMABINDING AS
RETURN
	(
		--===== "Inline" CTE Driven "Tally Table" produces values from 1 up to 10,000...
		-- enough to cover VARCHAR(8000)
		WITH E1(N) AS (
                 SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL
                 SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL
                 SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1
                ),                          --10E+1 or 10 rows
			E2(N) AS (SELECT 1 FROM E1 a, E1 b), --10E+2 or 100 rows
			E4(N) AS (SELECT 1 FROM E2 a, E2 b), --10E+4 or 10,000 rows max
			cteTally(N) AS (--==== This provides the "base" CTE and limits the number of rows right up front
                     -- for both a performance gain and prevention of accidental "overruns"
                 SELECT TOP (ISNULL(DATALENGTH(@StringToSplit),0)) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM E4
                ),
			cteStart(N1) AS (--==== This returns N+1 (starting position of each "element" just once for each delimiter)
                 SELECT 1 UNION ALL
                 SELECT t.N+1 FROM cteTally t WHERE SUBSTRING(@StringToSplit,t.N,1) = @Delimiter
                ),
			cteLen(N1,L1) AS(--==== Return start and length (for use in substring)
                 SELECT s.N1,
                        ISNULL(NULLIF(CHARINDEX(@Delimiter,@StringToSplit,s.N1),0)-s.N1,8000)
                   FROM cteStart s
                )
			--===== Do the actual split. The ISNULL/NULLIF combo handles the length for the final element when no delimiter is found.
			SELECT ItemNumber = ROW_NUMBER() OVER(ORDER BY l.N1),
				   Item       = SUBSTRING(@StringToSplit, l.N1, l.L1)
			FROM cteLen l
	)
GO
