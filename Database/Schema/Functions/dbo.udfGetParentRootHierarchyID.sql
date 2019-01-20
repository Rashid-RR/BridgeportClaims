SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[udfGetParentRootHierarchyID](@TreeID INT)
RETURNS HIERARCHYID
AS BEGIN
	RETURN
	(
		SELECT  [dt].[TreeNode].[GetAncestor]([dt].[TreeLevel] - 1)
		FROM    [dbo].[DecisionTree] AS [dt]
		WHERE   [dt].[TreeID] = @TreeID
	)
END
GO
