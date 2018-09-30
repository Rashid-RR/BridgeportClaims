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
	@HasSuspense BIT,
	@SuspenseAmountRemaining MONEY,
	@ToSuspenseNoteText VARCHAR(255),
	@AmountToPost MONEY,
	@UserID NVARCHAR(128),
	@PaymentPostings dbo.udtPaymentPosting READONLY
)
AS BEGIN
	DECLARE @PrntMsg NVARCHAR(100)
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		DECLARE @UTCNow DATETIME2 = dtme.udfGetUtcDate()
			  , @LocalNow DATETIME2 = dtme.udfGetLocalDate();
		DECLARE @LocalNowDateOnly DATE = CAST(@LocalNow AS DATE);

		INSERT dbo.PrescriptionPayment(CheckNumber, AmountPaid, DatePosted, PrescriptionID,
		CreatedOnUTC, UpdatedOnUTC, ModifiedByUserID)
		
		SELECT  @CheckNumber CheckNumber,
				p.AmountPosted AmountPaid,
				@LocalNowDateOnly,
				p.PrescriptionID,
				@UTCNow CreatedOnUTC,
				@UTCNow UpdatedOnUTC,
				@UserID UserID
		FROM @PaymentPostings AS p;

		IF (@HasSuspense = 1)
			BEGIN
				INSERT dbo.Suspense(CheckNumber, AmountRemaining, SuspenseDate, NoteText, UserID,
					CreatedOnUTC, UpdatedOnUTC)
				SELECT @CheckNumber, @SuspenseAmountRemaining, @LocalNowDateOnly, @ToSuspenseNoteText,
					@UserID, @UTCNow, @UTCNow
			END

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
		
		SET @PrntMsg = N'Error Line: ' + CONVERT(NVARCHAR, @ErrLine)
		PRINT @PrntMsg

        RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg);			-- First argument (string)
	END CATCH
END
GO
