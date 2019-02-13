SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[udfGetUpline]
	(
	 @TreeID INT
	,@MaxLevels INT = NULL
	)
RETURNS @Tree TABLE
	(
	 TreeID INT NOT NULL
	,ParentTreeID INT NULL
	,TreeLevel INT NOT NULL
	)
AS
BEGIN  
  
	DECLARE	@LVL INT  
	SET @LVL = 0  
   
	IF @MaxLevels IS NULL
		SET @MaxLevels = 9999999 --PREVENT INFINITE LOOP
   
 -- INSERT ROOT/ANCHOR LEVEL  
	INSERT	@Tree
			SELECT	pt.[TreeID]
				   ,pt.[ParentTreeID]
				   ,@LVL
			FROM	dbo.DecisionTree pt
			WHERE	pt.TreeID = @TreeID
   
 -- LOOP SUB-LEVELS  
	WHILE @@ROWCOUNT > 0
		BEGIN  
			SET @LVL = @LVL + 1  
     
			INSERT	@Tree
					SELECT	 a.[TreeID]
							,a.[ParentTreeID]
						   ,@LVL
					FROM	@Tree TREE
							INNER JOIN dbo.DecisionTree n ON n.[TreeID] = TREE.TreeID
							INNER JOIN dbo.DecisionTree a ON a.TreeID = n.[ParentTreeID]
					WHERE	TREE.TreeLevel < @MaxLevels;
		END 
	RETURN  
END
GO
