SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       12/1/2018
 Description:       Restarts the Tree Sequence the Inserts the Root Node of the Tree.
 Example Execute:
                    EXECUTE [util].[uspDecisionTreeInsertRoot]
 =============================================
*/
CREATE PROC [util].[uspDecisionTreeInsertRoot]
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	IF EXISTS (SELECT * FROM [dbo].[DecisionTree] AS [dt])
		BEGIN
			RAISERROR(N'Error, the DecisionTree table is not empty.', 16, 1) WITH NOWAIT;
			RETURN -1;
		END

	DECLARE @ModifiedByUserID NVARCHAR(128), @One INT = 1, @UtcNow DATETIME2 = dtme.[udfGetDate]();
	SELECT  TOP (@One)
			@ModifiedByUserID = [x].[UserID]
	FROM    [dbo].[vwAspNetUserAndRole] AS [x]
	WHERE   [x].[RoleName] = 'Admin'
	ORDER BY [x].[RegisteredDate] DESC;
	DECLARE @SQL NVARCHAR(400) = N'ALTER SEQUENCE [dbo].[seqDecisionTree] RESTART WITH 1;';
	EXEC [sys].[sp_executesql] @SQL;
	DECLARE @ParentTreeID INT = (NEXT VALUE FOR [dbo].[seqDecisionTree]);
    INSERT INTO [dbo].[DecisionTree] ([TreeNode],[TreeID],[NodeName],[NodeDescription],[ParentTreeID],[ModifiedByUserID],[CreatedOnUTC],[UpdatedOnUTC])
	VALUES (hierarchyid::GetRoot(), @ParentTreeID, 'Root', 'The Root Node of the Tree', @ParentTreeID, @ModifiedByUserID, @UtcNow, @UtcNow);
END
GO
