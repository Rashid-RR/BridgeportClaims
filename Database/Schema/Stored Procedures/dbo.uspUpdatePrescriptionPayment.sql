SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/10/2018
 Description:       Updates a Prescription Payment.
 Example Execute:
                    EXECUTE [dbo].[uspUpdatePrescriptionPayment]
 =============================================
*/
CREATE PROC [dbo].[uspUpdatePrescriptionPayment]
(
    @PrescriptionPaymentID INT
   ,@CheckNumber VARCHAR(50)
   ,@AmountPaid MONEY
   ,@DatePosted DATE
   ,@UserID NVARCHAR(128)
)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        BEGIN TRY
            BEGIN TRAN;

            UPDATE  dbo.PrescriptionPayment
            SET     CheckNumber = @CheckNumber
                   ,AmountPaid = @AmountPaid
                   ,DatePosted = @DatePosted
                   ,ModifiedByUserID = @UserID
            WHERE   PrescriptionPaymentID = @PrescriptionPaymentID;

            IF (@@TRANCOUNT > 0) COMMIT;
        END TRY
        BEGIN CATCH
            IF (@@TRANCOUNT > 0)
                ROLLBACK;

            DECLARE @ErrSeverity INT = ERROR_SEVERITY()
                   ,@ErrState INT = ERROR_STATE()
                   ,@ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
                   ,@ErrLine INT = ERROR_LINE()
                   ,@ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

            RAISERROR(   N'%s (line %d): %s' -- Message text w formatting
                        ,@ErrSeverity -- Severity
                        ,@ErrState -- State
                        ,@ErrProc -- First argument (string)
                        ,@ErrLine -- Second argument (int)
                        ,@ErrMsg
                     ); -- First argument (string)
        END CATCH
    END
GO
