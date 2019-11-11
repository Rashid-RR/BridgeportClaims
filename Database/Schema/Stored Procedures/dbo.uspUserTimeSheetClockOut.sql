SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Description:	Clocking Out
	Created Date:	11/10/2019
*/
CREATE PROC [dbo].[uspUserTimeSheetClockOut]
    @UserID NVARCHAR(128)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
		DECLARE @EndTime DATETIME2 = [dtme].[udfGetLocalDate]();
        DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
        UPDATE [u]
        SET [u].[EndTime] = @EndTime
           ,[u].[UpdatedOnUTC] = @UtcNow
        FROM [dbo].[UserTimeSheet] AS [u]
        WHERE [u].[UserID] = @UserID
              AND [u].[EndTime] IS NULL;
    END;
GO
