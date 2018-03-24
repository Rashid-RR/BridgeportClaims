SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       3/19/2018
 Description:       Updates the first name, last name and extension.
 Example Execute:
                    EXECUTE [dbo].[uspUpdatePersonalData]
 =============================================
*/
CREATE PROC [dbo].[uspUpdatePersonalData]
(
    @UserID NVARCHAR(128),
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @Extension VARCHAR(30)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
        UPDATE  [u] SET [u].[FirstName] = @FirstName,
                        [u].[LastName] = @LastName,
                        [u].[Extension] = @Extension
        FROM    [dbo].[AspNetUsers] AS [u]
        WHERE   [u].[ID] = @UserID
            
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
