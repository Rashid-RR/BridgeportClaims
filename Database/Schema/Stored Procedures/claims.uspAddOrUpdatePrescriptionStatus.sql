SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       8/6/2018
 Description:       Replaces NH code to update the Prescription Status of a Prescription.
 Example Execute:
                    EXECUTE [claims].[uspAddOrUpdatePrescriptionStatus]
 =============================================
*/
CREATE PROC [claims].[uspAddOrUpdatePrescriptionStatus]
(
    @PrescriptionID INT,
    @PrescriptionStatusID INT,
    @ModifiedByUserID NVARCHAR(128),
    @Add BIT OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @True BIT = CONVERT(BIT, 1),
                @False BIT = CONVERT(BIT, 0),
                @UtcNow DATETIME2 = dtme.udfGetDate(),
                @RowCount INT,
                @PrntMsg NVARCHAR(1000);

        SELECT  @Add = IIF(p.PrescriptionStatusID IS NOT NULL, @False, @True)
        FROM    dbo.Prescription AS p
        WHERE   p.PrescriptionID = @PrescriptionID;

        IF (@Add IS NULL)
            BEGIN
               IF (@@TRANCOUNT > 0)
                    ROLLBACK;
                RAISERROR(N'Could not determine current Prescription Status.', 16, 1) WITH NOWAIT;
                RETURN -1;
            END

        UPDATE dbo.Prescription SET PrescriptionStatusID = @PrescriptionStatusID,
                                    UpdatedOnUTC = @UtcNow,
                                    ModifiedByUserID = @ModifiedByUserID
        WHERE PrescriptionID = @PrescriptionID;

        SET @RowCount = @@ROWCOUNT;

        IF (@RowCount != 1)
            BEGIN
               IF (@@TRANCOUNT > 0)
                    ROLLBACK;
                SET @PrntMsg = N'Did not update 1 record, but instead updated ' + CONVERT(NVARCHAR(50), @RowCount);
                RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
                RETURN -1;
            END

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
