SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[uspDismissNotification]
    @NotificationID INT
  , @DismissedByUserID NVARCHAR(128)
AS
    BEGIN
        DECLARE @Today DATE = CAST([dtme].[udfGetLocalDate]() AS DATE);
		DECLARE @UtcNow DATETIME2 = [dtme].[udfGetDate]();
        IF EXISTS
        (
            SELECT *
            FROM   [dbo].[Notification] AS [n]
            WHERE  [n].[NotificationID] = @NotificationID AND [n].[IsDismissed] = 1
        )
			BEGIN
				RETURN -1;
			END

        UPDATE [n]
        SET    [n].[IsDismissed] = 1
             , [n].[DismissedByUserID] = @DismissedByUserID
             , [n].[DismissedDate] = @Today
			 , [n].[UpdatedOnUTC] = @UtcNow
        FROM   [dbo].[Notification] AS [n]
        WHERE  [n].[NotificationID] = @NotificationID;
    END;
GO
