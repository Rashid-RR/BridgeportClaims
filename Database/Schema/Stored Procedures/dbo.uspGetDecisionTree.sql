SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/30/2018
 Description:       Gets all the downline nodes of a tree.
 Example Execute:
                    EXECUTE [dbo].[uspGetDecisionTre2e] 1;
 =============================================
*/
CREATE PROC [dbo].[uspGetDecisionTree] @ParentTreeID INT
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        DECLARE @RootNode HIERARCHYID;
				
        SELECT  @RootNode = [dt].[TreeNode]
        FROM    [dbo].[DecisionTree] AS [dt]
        WHERE   [dt].[TreeID] = @ParentTreeID

        SELECT  [d].[TreePath]
               ,[d].[TreeLevel]
               ,[TreeId] = [d].[TreeID]
               ,[d].[NodeName]
               ,[d].[NodeDescription]
               ,[ParentTreeId] = [d].[ParentTreeID]
        FROM    [dbo].[DecisionTree] AS [d]
        WHERE   [d].[TreeNode].[IsDescendantOf](@RootNode) = 1;
    END
GO
