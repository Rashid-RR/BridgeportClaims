SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[uspDecisionTreeUserPathInsert]
    @SessionID UNIQUEIDENTIFIER,
    @SelectedTreeID INT,
    @UserID NVARCHAR(128)
AS 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	INSERT INTO [dbo].[DecisionTreeUserPath] ([SessionID], [SelectedTreeID], [UserID], [UpdatedOnUTC])
	SELECT @SessionID, @SelectedTreeID, @UserID, @UtcNow;
GO
