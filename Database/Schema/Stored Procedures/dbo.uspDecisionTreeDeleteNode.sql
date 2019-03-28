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
                    EXECUTE [dbo].[uspDecisionTreeDeleteNode] 3, @RowCount = @I OUTPUT;
 =============================================
*/
CREATE PROC [dbo].[uspDecisionTreeDeleteNode] @TreeID INT, @RowCount INT OUTPUT
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

		DECLARE @NodeToDelete HIERARCHYID;
        SELECT @NodeToDelete = [dt].[TreeNode] FROM [dbo].[DecisionTree] AS [dt] WHERE [dt].[TreeID] = @TreeID;

		DECLARE @NodesToDelete table (TreeNode hierarchyid NOT NULL PRIMARY KEY);
		INSERT @NodesToDelete (TreeNode)
		SELECT [dt].TreeNode FROM [dbo].[DecisionTree] AS [dt]
		WHERE [dt].[TreeNode].IsDescendantOf(@NodeToDelete) = 1;

		IF EXISTS (
			SELECT * FROM dbo.DecisionTree AS DT INNER JOIN @NodesToDelete AS N ON N.TreeNode = DT.TreeNode
			INNER JOIN dbo.DecisionTreeChoice AS DTC ON DTC.LeafTreeID = DT.TreeID
		)
			BEGIN
				UPDATE DT SET DT.IsDeleted = 1 FROM dbo.DecisionTree AS DT INNER JOIN @NodesToDelete AS NTD ON NTD.TreeNode = DT.TreeNode
			END
		ELSE
			BEGIN
				DELETE DT FROM dbo.DecisionTree AS DT INNER JOIN @NodesToDelete AS NTD ON NTD.TreeNode = DT.TreeNode
			END

		SET @RowCount = @@ROWCOUNT;

        IF (@@TRANCOUNT > 0)
			COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
			ROLLBACK;	
		DECLARE @ErrLine INT = ERROR_LINE()
              , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @Msg NVARCHAR(2000) = FORMATMESSAGE(N'An error occurred: %s Line Number: %u', @ErrMsg, @ErrLine);
		THROW 50000, @Msg, 0;
    END CATCH
END
GO
