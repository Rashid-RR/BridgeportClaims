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
					DECLARE @PrescriptionID INT, @AmountRemaining MONEY
					SELECT TOP (1) @PrescriptionID = PrescriptionID FROM dbo.Prescription
					DECLARE @Base [dbo].[udtID]
					INSERT @Base (ID) VALUES (@PrescriptionID)
					EXEC dbo.uspPostPayment @Base, 1, 'my check #', 55.21, 47481.54, 0.05, @AmountRemaining = @AmountRemaining OUTPUT
					SELECT * FROM dbo.PostPaymentAudit
					TRUNCATE TABLE dbo.PostPaymentAudit
*/
CREATE PROC [dbo].[uspPostPayment]
(
	@PrescriptionIDs [dbo].[udtID] READONLY,
	@DocumentID INTEGER,
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
		DECLARE @UTCNow DATETIME2 = dtme.udfGetUtcDate()
			  , @RowCount INTEGER;
		DECLARE @DatePosted DATE = CAST(dtme.udfGetLocalDate() AS DATE);

		-- Full Payment.
		IF @AmountSelected = @AmountToPost
			BEGIN
				INSERT dbo.PrescriptionPayment(CheckNumber, AmountPaid, DatePosted, PrescriptionID,
						DocumentID, CreatedOnUTC, UpdatedOnUTC)
				SELECT @CheckNumber, p.BilledAmount, @DatePosted, p.PrescriptionID, @DocumentID, @UTCNow, @UTCNow
				FROM @PrescriptionIDs AS pd
					 INNER JOIN dbo.Prescription AS p ON p.PrescriptionID = pd.ID
				SET @RowCount = @@ROWCOUNT

				-- Ensure that the row count equals the number of records in @PrescriptionIDs
				IF @RowCount <> (SELECT COUNT(*) FROM @PrescriptionIDs)
					BEGIN
						IF @@TRANCOUNT > 0
							ROLLBACK;
						RAISERROR(N'Error. The count of Payments inserted does not match the count of Prescription records being processed.', 16, 1) WITH NOWAIT;
						RETURN -1;
					END
			END
		ELSE IF (SELECT COUNT(*) FROM @PrescriptionIDs) = 1 -- Partial Payment. One line.
			BEGIN
				INSERT dbo.PrescriptionPayment(CheckNumber, AmountPaid, DatePosted, PrescriptionID,
					 DocumentID, CreatedOnUTC, UpdatedOnUTC)
				SELECT @CheckNumber, @AmountToPost, @DatePosted, p.PrescriptionID, @DocumentID, @UTCNow, @UTCNow
				FROM @PrescriptionIDs AS pd
					 INNER JOIN dbo.Prescription AS p ON p.PrescriptionID=pd.ID
			END
		ELSE -- Partial Payment. Multi-Line.
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'Error. Partial payments for multiple prescriptions is not supported.', 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		-- Do something arbitrary for the @AmountRemaining OUTPUT param
		SET @AmountRemaining = ISNULL(@CheckAmount, 0.00) - ISNULL(@AmountToPost, 0.00)

		-- Select out @PrescriptionIDs and the Outstanding amount.
		SELECT  PrescriptionId = p.ID
			   ,Outstanding = ISNULL(pre.BilledAmount, 0.00)
							  - ISNULL((
										   SELECT   AmountPaid = SUM(ipay.AmountPaid)
										   FROM     dbo.PrescriptionPayment AS ipay
										   WHERE    ipay.PrescriptionID = pre.PrescriptionID
									   )
									  ,0.00
									  )
		FROM    @PrescriptionIDs AS p
				INNER JOIN dbo.Prescription AS pre ON pre.PrescriptionID = p.ID;

		-- Second Result
		-- Return Document Info: 
		SELECT d.DocumentID DocumentId, d.[FileName], d.FileUrl FROM dbo.Document AS d WHERE d.DocumentID = @DocumentID;

		IF (@@TRANCOUNT > 0)
			COMMIT;
	END TRY
	BEGIN CATCH
	    IF (@@TRANCOUNT > 0)
			ROLLBACK;
				
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(4000) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
	END CATCH
END

GO
