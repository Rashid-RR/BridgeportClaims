SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Description:	Clocking In
	Created Date:	11/10/2019
*/
CREATE PROC [dbo].[uspUserTimeSheetClockIn]
    @UserID nvarchar(128)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
	DECLARE @StartTime DATETIME2 = [dtme].[udfGetLocalDate]();
	INSERT INTO [dbo].[UserTimeSheet] ([UserID], [StartTime], [EndTime], [CreatedOnUTC], [UpdatedOnUTC])
	SELECT @UserID, @StartTime, NULL, @UtcNow, @UtcNow
END
GO
