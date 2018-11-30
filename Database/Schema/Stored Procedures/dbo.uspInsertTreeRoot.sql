SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/30/2018
 Description:       Inserts a new tree root, sibling of TRUE root.
 Example Execute:
                    EXECUTE [dbo].[uspInsertTreeRoot]
 =============================================
*/
CREATE PROC [dbo].[uspInsertTreeRoot]
(
	@NodeName VARCHAR(255),
	@NodeDescription VARCHAR(4000),
	@TreeID INT OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @TreeID = (NEXT VALUE FOR [dbo].[seqDecisionTree]);
	INSERT [dbo].[DecisionTree] ([TreeNode], [TreeID], [NodeName], [NodeDescription])
	VALUES (hierarchyid::GetRoot(), @TreeID, @NodeName, @NodeDescription);
END
GO
