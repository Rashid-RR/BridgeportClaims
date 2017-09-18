SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	9/16/2017
	Description:	Inserts the stored, cached set of 
					Payment Posting(s) and Suspensed amount(s)
	Sample Execute:
					EXEC dbo.uspInsertPaymentPostings
*/
CREATE PROC [dbo].[uspInsertPaymentPostings]
(
	@CheckNumber VARCHAR(155),
	@CheckAmount MONEY,
	@ClaimID INT,
	@AmountSelected MONEY,
	@HasSuspense BIT,
	@SuspenseAmountRemaining MONEY,
	@ToSuspenseNoteText VARCHAR(255),
	@AmountToPost MONEY,
	@UserID NVARCHAR(128),
	@PaymentPostings dbo.udtPaymentPosting READONLY
)
AS BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRAN;
		DECLARE @Now DATETIME2 = SYSUTCDATETIME();

		INSERT dbo.PrescriptionPayment(CheckNumber, AmountPaid, DatePosted, PrescriptionID,
		CreatedOnUTC, UpdatedOnUTC, UserID)
		
		SELECT  @CheckNumber CheckNumber,
				@AmountToPost AmountPaid,
				CAST(dtme.udfGetLocalDateTime(@Now) AS DATE),
				p.PrescriptionID,
				@Now CreatedOnUTC,
				@Now UpdatedOnUTC,
				@UserID UserID
		FROM @PaymentPostings AS p

		IF (@HasSuspense = 1)
			BEGIN
				INSERT dbo.Suspense(ClaimID, CheckNumber, AmountRemaining, SuspenseDate, NoteText,
					CreatedOnUTC, UpdatedOnUTC)
				SELECT @ClaimID, @CheckNumber, @SuspenseAmountRemaining, @Now, @ToSuspenseNoteText,
					@Now, @Now
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

        RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg);			-- First argument (string)
	END CATCH
END
GO
