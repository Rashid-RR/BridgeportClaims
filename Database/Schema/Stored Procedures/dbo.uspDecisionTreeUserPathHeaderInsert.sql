SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspDecisionTreeUserPathHeaderInsert]
    @UserID NVARCHAR(128),
    @TreeRootID INT,
    @ClaimID INT,
    @SessionID UNIQUEIDENTIFIER
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	INSERT INTO [dbo].[DecisionTreeUserPathHeader] ([UserID], [TreeRootID], [ClaimID], [SessionID], [CreatedOnUTC], [UpdatedOnUTC])
	SELECT @UserID, @TreeRootID, @ClaimID, @SessionID, @UtcNow, @UtcNow;
END
GO
