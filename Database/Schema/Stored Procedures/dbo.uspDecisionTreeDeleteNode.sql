SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       1/19/2019
 Description:       Deletes a node, and all of it's children (if applicable)
 Example Execute:
					DECLARE @I INT
                    EXECUTE [dbo].[uspDecisionTreeDeleteNode] 40, @RowCount = @I OUTPUT;
 =============================================
*/
CREATE PROC [dbo].[uspDecisionTreeDeleteNode] @TreeID INT, @RowCount INT OUTPUT, @RootTreeID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
    BEGIN TRY
        BEGIN TRAN;

		IF NOT EXISTS
        (
			SELECT * FROM [dbo].[DecisionTree] AS [dt] WHERE [dt].[TreeID] = @TreeID
		)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'Error, the Node passed in does not exist, 16, 1.', 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		DECLARE @NodeToDelete HIERARCHYID, @RootHierarchyID HIERARCHYID;
        SELECT @NodeToDelete = [dt].[TreeNode] FROM [dbo].[DecisionTree] AS [dt] WHERE [dt].[TreeID] = @TreeID;

		-- Before deleting, we need to get the parent node of the parent we are deleting.
		SEt @RootHierarchyID = dbo.[udfGetParentRootHierarchyID](@TreeID)
		SET @RootTreeID = dbo.[udfGetTreeIDByHierarchyID](@RootHierarchyID);

		DELETE [dt] FROM [dbo].[DecisionTree] AS [dt]
		WHERE [dt].[TreeNode].IsDescendantOf(@NodeToDelete) = 1;

		SET @RowCount = @@ROWCOUNT;

		EXECUTE [dbo].[uspGetDecisionTree] @RootTreeID;

        IF (@@TRANCOUNT > 0)
			COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
            ROLLBACK;

		SET @RowCount = 0;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(4000) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
    END CATCH
END
GO
