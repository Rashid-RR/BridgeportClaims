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
	@AmountToPost MONEY,
	@AmountRemaining MONEY OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		DECLARE @Now DATETIME2 = SYSUTCDATETIME(), @RowCount INTEGER;
		DECLARE @DateScanned DATE = CAST(dtme.udfGetLocalDateTime(@Now) AS DATE);

		-- Full Payment.
		IF @AmountSelected = @AmountToPost
			BEGIN
				INSERT dbo.Payment(CheckNumber, AmountPaid, DateScanned, PrescriptionID, ClaimID,
						CreatedOnUTC, UpdatedOnUTC)
				SELECT @CheckNumber, p.BilledAmount, @DateScanned, p.PrescriptionID
						,p.ClaimID, @Now, @Now
				FROM @PrescriptionIDs AS pd
					 INNER JOIN dbo.Prescription AS p ON p.PrescriptionID = pd.PrescriptionID
				SET @RowCount = @@ROWCOUNT

				-- Ensure that the row count equals the number of records in @PrescriptionIDs
				IF @RowCount != (SELECT COUNT(*) FROM @PrescriptionIDs)
					BEGIN
						IF @@TRANCOUNT > 0
							ROLLBACK
						RAISERROR(N'Error. The count of Payments inserted does not match the count of Prescription records being processed.', 16, 1)
							WITH NOWAIT
						RETURN;
					END
			END
		ELSE IF (SELECT COUNT(*) FROM @PrescriptionIDs) = 1 -- Partial Payment. One line.
			BEGIN
				INSERT dbo.Payment(CheckNumber, AmountPaid, DateScanned, PrescriptionID, ClaimID,
					 CreatedOnUTC, UpdatedOnUTC)
				SELECT @CheckNumber, @AmountToPost, @DateScanned, p.PrescriptionID, p.ClaimID,
					 @Now, @Now
				FROM @PrescriptionIDs AS pd
					 INNER JOIN dbo.Prescription AS p ON p.PrescriptionID=pd.PrescriptionID
			END
		ELSE -- Partial Payment. Multi-Line.
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				RAISERROR(N'Error. Partial payments for multiple prescriptions is not supported.', 16, 1)
					WITH NOWAIT
				RETURN;
			END
		-- Leave this Audit Insert for a while. Match the CreatedOnUTC and UpdatedOnUTC
	    INSERT dbo.PostPaymentAudit
	    (
	        PrescriptionID,
	        CheckNumber,
	        CheckAmount,
	        AmountSelected,
	        AmountToPost,
			CreatedOnUTC,
			UpdatedOnUTC
	    )
		SELECT p.PrescriptionID,
			   @CheckNumber,
			   @CheckAmount,
			   @AmountSelected,
			   @AmountToPost,
			   @Now,
			   @Now
		FROM @PrescriptionIDs AS p

		-- Do something arbitrary for the @AmountRemaining OUTPUT param
		SET @AmountRemaining = ISNULL(@CheckAmount, 0.00) - ISNULL(@AmountToPost, 0.00)

		-- Select out @PrescriptionIDs and the Outstanding amount.
		SELECT	p.PrescriptionID, Outstanding = 
					ISNULL(pre.BilledAmount, 0.00) - ISNULL(pay.AmountPaid, 0.00)
		FROM	@PrescriptionIDs AS p
				INNER JOIN dbo.Prescription AS pre ON pre.PrescriptionID=p.PrescriptionID
				OUTER APPLY (  SELECT	AmountPaid = SUM(ipay.AmountPaid)
							   FROM		dbo.Payment AS ipay
							   WHERE	ipay.PrescriptionID=pre.PrescriptionID) AS pay
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
