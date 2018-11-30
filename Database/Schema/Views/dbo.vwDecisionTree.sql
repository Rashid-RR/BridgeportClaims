SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwDecisionTree]
AS
SELECT [dt].[TreeNode], [dt].[TreeLevel], [dt].[TreeID], [dt].[NodeName], [dt].[NodeDescription]
FROM [dbo].[DecisionTree] AS [dt]
WHERE [dt].[TreeNode] != hierarchyid::GetRoot();
GO
