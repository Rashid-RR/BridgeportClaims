SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[udfGetUpline] (@LeafTreeID INT)
RETURNS @Tree TABLE
(
    [TreePath] VARCHAR(255) NOT NULL
  , [TreeLevel] SMALLINT NOT NULL
  , [TreeID] INT NOT NULL
  , [NodeName] VARCHAR(255) NOT NULL
  , [ParentTreeID] INT NOT NULL
)
AS
    BEGIN
        DECLARE @TreeNode HIERARCHYID;
        SELECT @TreeNode = [t].[TreeNode] FROM [dbo].[DecisionTree] AS [t] WHERE [t].[TreeID] = @LeafTreeID;

        -- Insert Root/Anchor
        INSERT @Tree ([TreePath], [TreeLevel], [TreeID], [NodeName], [ParentTreeID])
        SELECT [t].[TreeNode].[ToString]()
             , [t].[TreeLevel]
             , [t].[TreeID]
             , [t].[NodeName]
             , [t].[ParentTreeID]
        FROM   [dbo].[DecisionTree] AS [t]
        WHERE  [t].[TreeID] = @LeafTreeID;

        WHILE @@ROWCOUNT > 0
            BEGIN
                INSERT @Tree ([TreePath], [TreeLevel], [TreeID], [NodeName], [ParentTreeID])
                SELECT [t].[TreeNode].[ToString]()
                     , [t].[TreeLevel]
                     , [t].[TreeID]
                     , [t].[NodeName]
                     , [t].[ParentTreeID]
                FROM   [dbo].[DecisionTree] AS [t]
                WHERE  @TreeNode.[GetAncestor](1) = [t].[TreeNode]
					   AND [t].[TreeLevel] <> 0;

                SELECT @TreeNode = [t].[TreeNode]
                FROM   [dbo].[DecisionTree] AS [t]
                WHERE  [t].[TreeNode] = @TreeNode.[GetAncestor](1);
            END;
        RETURN;
    END;

GO
