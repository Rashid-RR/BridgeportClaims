SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/30/2018
 Description:       Inserts a new Node into the Decision Tree.
 Example Execute:
                    EXECUTE dbo.uspDecisionTreeInsert 13, 'Yes De4ar!', '7fff6083-6f89-46dc-afd2-0af36601b312';
 =============================================
*/
CREATE PROC [dbo].[uspDecisionTreeInsert]
(
	@ParentTreeID INT,
	@NodeName VARCHAR(255),
	@ModifiedByUserID NVARCHAR(128),
	@RootTreeID INT OUTPUT
)
AS BEGIN
	DECLARE @SeqNum INT = (SELECT MAX([dt].[TreeID]) FROM [dbo].[DecisionTree] AS [dt]) + 1;
	DECLARE @SQL NVARCHAR(1000) = N'ALTER SEQUENCE [dbo].[seqDecisionTree] RESTART WITH ' + CONVERT(NVARCHAR(500), @SeqNum) + N';';
	EXEC [sys].[sp_executesql] @SQL;
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRY
        BEGIN TRAN;
		DECLARE @TreeID INT;
		DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();

		DECLARE @ParentNode HIERARCHYID, @LeftChild HIERARCHYID;
		SELECT	@ParentNode = [TreeNode]
		FROM	[dbo].[DecisionTree]
		WHERE	[TreeID] = @ParentTreeID;

		IF (@ParentNode IS NULL)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'Error, could not find the Parent Tree ID.', 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		SELECT	@LeftChild = MAX([TreeNode])
		FROM	[dbo].[DecisionTree]
		WHERE	[TreeNode].GetAncestor(1) = @ParentNode;

		SET @TreeID = (NEXT VALUE FOR seqDecisionTree);
		DECLARE @InsertedHierarchyID HIERARCHYID = @ParentNode.GetDescendant(@LeftChild, NULL);

		INSERT INTO [dbo].[DecisionTree] ([TreeNode], [TreeID], [NodeName], [ParentTreeID], [ModifiedByUserID], [CreatedOnUTC], [UpdatedOnUTC])
		VALUES (@InsertedHierarchyID, @TreeID, @NodeName, @ParentTreeID, @ModifiedByUserID, @UtcNow, @UtcNow);
        
		DECLARE @LevelOneRootHierarchyID HIERARCHYID;
		SELECT @LevelOneRootHierarchyID = dbo.udfGetParentRootHierarchyID(@TreeID);
		SELECT @RootTreeID = dbo.udfGetTreeIDByHierarchyID(@LevelOneRootHierarchyID);

		EXECUTE [dbo].[uspGetDecisionTree] @RootTreeID;
        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(1000) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
    END CATCH
END
GO
