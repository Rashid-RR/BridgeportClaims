SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspCollectionAssignmentInsert]
    @UserID NVARCHAR(128),
    @PayorID INT,
	@ModifiedByUserID NVARCHAR(128)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	INSERT INTO [dbo].[CollectionAssignment] ([UserID], [PayorID], [ModifiedByUserID], [CreatedOnUTC], [UpdatedOnUTC])
	VALUES (@UserID, @PayorID, @ModifiedByUserID, @UtcNow, @UtcNow);
END
GO
