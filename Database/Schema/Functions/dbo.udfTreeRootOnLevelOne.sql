SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[udfTreeRootOnLevelOne](@TreeRootID INT)
RETURNS BIT
AS BEGIN
	DECLARE @RetVal BIT = 0, @TreeLevel SMALLINT;
	SELECT  @TreeLevel = [t].[TreeLevel]
	FROM    [dbo].[DecisionTree] AS [t]
	WHERE   [t].[TreeID] = @TreeRootID;
	SET @RetVal = CASE WHEN @TreeLevel = 1 THEN 1 ELSE 0 END
	RETURN @RetVal;
END

GO
