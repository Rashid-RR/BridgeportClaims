SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       10/28/2018
 Description:       Deletes all Payments and Check Indexes for a Document.
 Example Execute:
                    EXECUTE [dbo].[uspRePostCheck] 89738
 =============================================
*/
CREATE PROC [dbo].[uspRePostCheck] @DocumentID INT
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
            
        DELETE	[pp]
		FROM    [dbo].[PrescriptionPayment] AS [pp]
		WHERE   [pp].[DocumentID] = @DocumentID;

		DELETE	[ci]
		FROM    [dbo].[CheckIndex] AS [ci]
		WHERE   [ci].[DocumentID] = @DocumentID;
            
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
