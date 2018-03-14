SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       3/14/2018
 Description:       Is called on the Save button when someone saves a Payor Letter Name notification.
 Example Execute:
                    EXECUTE [dbo].[uspSavePayorLetterNameNotification] 1, 
 =============================================
*/
CREATE PROCEDURE [dbo].[uspSavePayorLetterNameNotification]
(
    @NotificationID INT,
    @ModifiedByUserID NVARCHAR(128),
    @LetterName VARCHAR(255)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
            DECLARE @PayorID INT, @MessageText VARCHAR(255);

            SELECT  @MessageText = SUBSTRING([n].[MessageText], PATINDEX('%Payor (ID %', [n].[MessageText]) + 10, 5)
            FROM    [dbo].[Notification] AS [n]
            WHERE   [n].[NotificationID] = @NotificationID

            SELECT @PayorID = CONVERT(INT, dbo.udfGetNumeric(@MessageText))

            EXEC [dbo].[uspUpdatePayorLetterName] @PayorID = @PayorID, @LetterName = @LetterName
            EXEC [dbo].[uspSaveOrDismissNotification] @NotificationID = @NotificationID, @DismissedByUserID = @ModifiedByUserID;
            
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
