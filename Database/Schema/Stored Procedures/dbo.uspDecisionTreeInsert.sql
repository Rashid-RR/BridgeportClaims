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
					DECLARE @RootTreeID INT = 1
                    EXECUTE dbo.uspDecisionTreeInsert 1, 'Yes De4ar!', '7fff6083-6f89-46dc-afd2-0af36601b312', @RootTreeID OUTPUT;
 =============================================
*/
CREATE PROC [dbo].[uspDecisionTreeInsert]
(
    @ParentTreeID INT
  , @NodeName VARCHAR(255)
  , @ModifiedByUserID NVARCHAR(128)
  , @RootTreeID INT OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @Tree TABLE (TreeID INT NOT NULL PRIMARY KEY);
        DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();

        DECLARE @ParentNode HIERARCHYID
                , @LeftChild HIERARCHYID;
        SELECT @ParentNode = [TreeNode] FROM [dbo].[DecisionTree] WHERE [TreeID] = @ParentTreeID;

        IF (@ParentNode IS NULL)
            BEGIN
                IF (@@TRANCOUNT > 0)
                    ROLLBACK;
                RAISERROR(N'Error, could not find the Parent Tree ID.', 16, 1) WITH NOWAIT;
                RETURN -1;
            END;

        SELECT @LeftChild = MAX([TreeNode])
        FROM   [dbo].[DecisionTree]
        WHERE  [TreeNode].[GetAncestor](1) = @ParentNode;

        DECLARE @InsertedHierarchyID HIERARCHYID = @ParentNode.[GetDescendant](@LeftChild, NULL);

        INSERT INTO [dbo].[DecisionTree]
        (
            [TreeNode]
            , [NodeName]
            , [ParentTreeID]
            , [ModifiedByUserID]
            , [CreatedOnUTC]
            , [UpdatedOnUTC]
        )
		OUTPUT [Inserted].[TreeID] INTO @Tree
        VALUES
                (@InsertedHierarchyID, @NodeName, @ParentTreeID, @ModifiedByUserID, @UtcNow, @UtcNow);

        SELECT [d].[TreePath]
                , [d].[TreeLevel]
                , [d].[TreeID] AS [TreeId]
                , [d].[NodeName]
                , [d].[NodeDescription]
                , [d].[ParentTreeID] AS [ParentTreeId]
        FROM   [dbo].[DecisionTree] AS [d]
				INNER JOIN @Tree AS [t] ON [t].[TreeID] = [d].[TreeID]

        IF (@@TRANCOUNT > 0) COMMIT;
    END TRY
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
            ROLLBACK;

        DECLARE @ErrLine INT = ERROR_LINE()
                , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @Msg NVARCHAR(2000) = FORMATMESSAGE(N'An error occurred: %s Line Number: %u', @ErrMsg, @ErrLine);
		THROW 50000, @Msg, 0;
    END CATCH;
END;
GO
