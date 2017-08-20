SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/19/2017
	Description:	Executes the Payment Processing logic (which is
					calculation logic) YAY Commissions! for Payments.
	Sample Execute:
					DECLARE @PrescriptionID INT
					SELECT TOP (1) @PrescriptionID = PrescriptionID FROM dbo.Prescription
					DECLARE @Base [dbo].[udtPrescriptionID]
					INSERT @Base (PrescriptionID) VALUES (@PrescriptionID)
					EXEC dbo.uspPostPayment @Base, 'my check #', 55.21, 47481.54, 0.05
					SELECT * FROM dbo.PostPaymentAudit
					TRUNCATE TABLE dbo.PostPaymentAudit
*/
CREATE PROC [dbo].[uspPostPayment]
(
	@PrescriptionIDs [dbo].[udtPrescriptionID] READONLY,
	@CheckNumber VARCHAR(50),
	@CheckAmount MONEY,
	@AmountSelected MONEY,
	@AmountToPost MONEY
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
	    INSERT dbo.PostPaymentAudit
	    (
	        PrescriptionID,
	        CheckNumber,
	        CheckAmount,
	        AmountSelected,
	        AmountToPost
	    )
		SELECT p.PrescriptionID,
			   @CheckNumber,
			   @CheckAmount,
			   @AmountSelected,
			   @AmountToPost
		FROM @PrescriptionIDs AS p
		IF @@TRANCOUNT > 0
			COMMIT
	END TRY
	BEGIN CATCH
	    IF @@TRANCOUNT > 0
			ROLLBACK
				
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE()

        RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg)			-- First argument (string)
	END CATCH
END
GO
