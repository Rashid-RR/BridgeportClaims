SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspDecisionTreeUserPathHeaderDelete] 
    @TreeRootID INT
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DELETE
	FROM   [dbo].[DecisionTreeUserPathHeader]
	WHERE  [DecisionTreeUserPathHeaderID] = @TreeRootID;
END
GO
