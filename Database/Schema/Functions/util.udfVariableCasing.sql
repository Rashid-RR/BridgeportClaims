SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       10/12/2018
 Description:       Transforms input string into a C#-like, camel-case variable.
 Example Execute:   
                    SELECT [util].[udfVariableCasing](N'DocumentID', 1)
 =============================================
*/
CREATE FUNCTION [util].[udfVariableCasing](@Input NVARCHAR(4000), @FirstLetterCapital BIT)
RETURNS NVARCHAR(4000)
AS BEGIN
DECLARE @Length BIGINT = LEN(@Input), @Position INT = 1, @RetVal NVARCHAR(4000);
DECLARE @Letters TABLE (Position INT NOT NULL PRIMARY KEY, Letter NCHAR(1) NOT NULL, IsCapital BIT NOT NULL);

WHILE (@Position <= @Length)
	BEGIN
		IF (@Position = 1)
			BEGIN
				INSERT @Letters (Position, Letter, IsCapital)
				SELECT 1, IIF(@FirstLetterCapital = 0, LOWER(SUBSTRING(@Input, @Position, 1)),  UPPER(SUBSTRING(@Input, @Position, 1))), 0
			END
		ELSE
			BEGIN
				WITH LettersCTE (Position, Letter, IsCapital) AS
				(
					SELECT  @Position
						   ,CAST(SUBSTRING(@Input, @Position, 1) AS NCHAR(1))
						   ,CAST(CASE WHEN SUBSTRING(@Input, @Position, 1) COLLATE Latin1_General_CS_AS LIKE '[ABCDEFGHIJKLMNOPQRSTUVWXYZ]%' THEN 1 ELSE 0 END AS BIT)
				)
				INSERT @Letters (Position, Letter, IsCapital)
				SELECT c.Position, c.Letter, c.IsCapital FROM LettersCTE c
			END
		SET @Position += 1;
	END;

WITH RecursiveSequenceCTE AS
(
	-- Anchor: First Row.
	SELECT  l.Position, l.Letter, l.IsCapital, Seq = 1
	FROM    @Letters AS l
	WHERE   l.Position =
	(
		SELECT  MIN(id.Position)
		FROM    @Letters AS id
	)

	UNION ALL
    
	-- Recursive part
	SELECT
		q.Position,
		q.Letter,
		q.IsCapital,
		q.Seq
	FROM 
	(
		-- Next row in sequence of Position
		SELECT
			l.Position, l.Letter, l.IsCapital,
			Seq = 
				CASE
					-- Same Capitalization, increment sequence
					WHEN l.IsCapital = r.IsCapital
						THEN r.Seq + 1 
					-- Otherwise, restart sequence at 1
					ELSE 1
				END,
			Rn = ROW_NUMBER() OVER (ORDER BY r.Position)
		FROM RecursiveSequenceCTE AS r INNER JOIN @Letters AS l ON l.Position > r.Position
	) AS q
	WHERE
		q.Rn = 1
)
SELECT @RetVal = COALESCE(@RetVal + '', '') + CASE WHEN l.IsCapital = 1 AND l.Seq > 1 THEN LOWER(l.Letter) ELSE l.letter end FROM RecursiveSequenceCTE l
RETURN @RetVal;
END
GO
