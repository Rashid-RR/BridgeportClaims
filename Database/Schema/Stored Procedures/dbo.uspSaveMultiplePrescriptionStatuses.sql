SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       8/20/2018
 Description:       Saves multiple Prescription Statuses at once.
 Example Execute:
                    EXECUTE [dbo].[uspSaveMultiplePrescriptionStatuses]
 =============================================
*/
CREATE PROC [dbo].[uspSaveMultiplePrescriptionStatuses]
(
    @Prescription [dbo].[udtID] READONLY,
    @PrescriptionStatusID INT,
    @UserID NVARCHAR(128)
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        DECLARE @UTCNow DATETIME2 = dtme.udfGetDate();
        UPDATE  p
        SET     p.PrescriptionStatusID = @PrescriptionStatusID,
                p.UpdatedOnUTC = @UTCNow,
                p.ModifiedByUserID = @UserID
        FROM    dbo.Prescription AS p
                INNER JOIN @Prescription AS a ON a.ID = p.PrescriptionID;

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
