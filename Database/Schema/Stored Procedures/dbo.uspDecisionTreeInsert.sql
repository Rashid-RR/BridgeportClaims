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
                    EXECUTE dbo.DecisionTreeInsert 7, 'Coffey', '';
 =============================================
*/
CREATE PROC [dbo].[uspDecisionTreeInsert]
(
	@ParentTreeID INT,
	@NodeName VARCHAR(255),
	@NodeDescription VARCHAR(4000),
	@TreeID INT OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRY
        BEGIN TRAN;

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

		INSERT INTO [dbo].[DecisionTree] ([TreeNode], [TreeID], [NodeName], [NodeDescription])
		VALUES (@ParentNode.GetDescendant(@LeftChild, NULL), @TreeID, @NodeName, @NodeDescription);
            
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
