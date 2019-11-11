SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Description:	Getting the user clocked in time
	Created Date:	11/10/2019
*/
CREATE PROC [dbo].[uspGetUserTimeSheetClockInTime]
    @UserID NVARCHAR(128)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        SELECT [u].[StartTime]
        FROM [dbo].[UserTimeSheet] AS [u]
        WHERE [u].[UserID] = @UserID
              AND [u].[EndTime] IS NULL;
    END;
GO
