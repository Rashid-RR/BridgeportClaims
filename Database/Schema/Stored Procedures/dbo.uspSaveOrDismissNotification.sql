SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       3/14/2018
 Description:       Save or Dismiss a Notification
 Example Execute:
                    EXECUTE [dbo].[uspSaveOrDismissNotification]
 =============================================
*/
CREATE   PROC [dbo].[uspSaveOrDismissNotification]
(
    @NotificationID INTEGER,
    @DismissedByUserID NVARCHAR(128)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

            DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME(),
                    @Today DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]());

            UPDATE  [n]
            SET     [n].[DismissedByUserID] = @DismissedByUserID
                  , [n].[UpdatedOnUTC] = @UtcNow
                  , [n].[IsDismissed] = 1
                  , [n].[DismissedDate] = @Today
            FROM    [dbo].[Notification] AS [n]
            WHERE   [n].[NotificationID] = @NotificationID;

        IF (@@TRANCOUNT > 0)
            COMMIT;
    END TRY
    BEGIN CATCH     
        IF (@@TRANCOUNT > 0)
            ROLLBACK;
                
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s',    -- Message text w formatting
            @ErrSeverity,        -- Severity
            @ErrState,            -- State
            @ErrProc,            -- First argument (string)
            @ErrLine,            -- Second argument (int)
            @ErrMsg);            -- First argument (string)
    END CATCH
END
GO
