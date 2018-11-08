SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspCollectionAssignmentDelete]
    @UserID NVARCHAR(128),
    @PayorID INT
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DELETE
	FROM   [dbo].[CollectionAssignment]
	WHERE  [UserID] = @UserID
	       AND [PayorID] = @PayorID;
END
GO
