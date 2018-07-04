SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/4/2018
 Description:       Archives a row from the Unpaid Scripts report page. 
 Example Execute:
                    EXECUTE [dbo].[uspUnpaidScriptsArchivedInsert]
 =============================================
*/
CREATE PROC [dbo].[uspUnpaidScriptsArchivedInsert]
(
	@PrescriptionID INTEGER,
	@ModifiedByUserID NVARCHAR(128)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

		DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();
            
        INSERT dbo.UnpaidScriptsArchived (PrescriptionID, ModifiedByUserID, CreatedOnUTC, UpdatedOnUTC)
        VALUES (@PrescriptionID, @ModifiedByUserID, @UtcNow, @UtcNow);
            
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

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
    END CATCH
END
GO
