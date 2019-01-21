SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[udfGetTreeIDByHierarchyID](@HierarchyID HIERARCHYID)
RETURNS INT
AS BEGIN
	RETURN
	(
		SELECT [dt].[TreeID] FROM [dbo].[DecisionTree] AS [dt]
		WHERE [dt].[TreeNode] = @HierarchyID
	)
END
GO
