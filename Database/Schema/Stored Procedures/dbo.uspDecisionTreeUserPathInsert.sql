SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspDecisionTreeUserPathInsert]
    @SessionID UNIQUEIDENTIFIER,
	@ParentTreeID INT,
    @SelectedTreeID INT,
    @UserID NVARCHAR(128)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	INSERT INTO [dbo].[DecisionTreeUserPath] ([SessionID], [ParentTreeID], [SelectedTreeID], [UserID], [UpdatedOnUTC])
	SELECT @SessionID, @ParentTreeID, @SelectedTreeID, @UserID, @UtcNow;
END
GO
