SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspCollectionAssignmentUpdate]
    @UserID NVARCHAR(128),
    @PayorID INT,
    @ModifiedByUserID NVARCHAR(128)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	UPDATE [dbo].[CollectionAssignment]
	SET    [UpdatedOnUTC] = @UtcNow, [ModifiedByUserID] = @ModifiedByUserID
	WHERE  [UserID] = @UserID
	       AND [PayorID] = @PayorID;
END
GO
