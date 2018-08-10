SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/30/2018
 Description:       Updates a BilledAmount field for a Prescription 
 Example Execute:
                    EXECUTE [dbo].[uspUpdateBilledAmount] 2154, 666.2
 =============================================
*/
CREATE PROC [dbo].[uspUpdateBilledAmount]
(
	@PrescriptionID INT,
	@BilledAmount MONEY,
    @ModifiedByUserID NVARCHAR(128)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        IF (@PrescriptionID IS NULL)
            BEGIN
                IF (@@TRANCOUNT > 0)
                    ROLLBACK;
                RAISERROR(N'Error, the Prescription ID parameter cannot be null.', 16, 1) WITH NOWAIT;
                RETURN -1;
            END

        DECLARE @UtcNow DATETIME2 = dtme.udfGetDate();

        UPDATE  p
        SET     p.BilledAmount = @BilledAmount, p.UpdatedOnUTC = @UtcNow, p.ModifiedByUserID = @ModifiedByUserID
        FROM    dbo.Prescription AS p
        WHERE   p.PrescriptionID = @PrescriptionID;
            
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
