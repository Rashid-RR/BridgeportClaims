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
	@DocumentID INT,
	@HasSuspense BIT,
	@SuspenseAmountRemaining MONEY,
	@ToSuspenseNoteText VARCHAR(255),
	@UserID NVARCHAR(128),
	@PaymentPostings dbo.udtPaymentPosting READONLY
)
AS BEGIN
	DECLARE @PrntMsg NVARCHAR(100)
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;

		IF @DocumentID IS NULL
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK;
				RAISERROR(N'The @DocumentID parameter cannot be null.', 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		DECLARE @UTCNow DATETIME2 = dtme.udfGetUtcDate()
			  , @LocalNow DATETIME2 = dtme.udfGetLocalDate();
		DECLARE @LocalNowDateOnly DATE = CAST(@LocalNow AS DATE);

		INSERT dbo.PrescriptionPayment(CheckNumber, AmountPaid, DatePosted, PrescriptionID,
		CreatedOnUTC, UpdatedOnUTC, ModifiedByUserID, DocumentID)
		
		SELECT  @CheckNumber CheckNumber,
				p.AmountPosted AmountPaid,
				@LocalNowDateOnly,
				p.PrescriptionID,
				@UTCNow CreatedOnUTC,
				@UTCNow UpdatedOnUTC,
				@UserID UserID,
				@DocumentID DocumentID
		FROM @PaymentPostings AS p;

		IF (@HasSuspense = 1)
			BEGIN
				INSERT dbo.Suspense(CheckNumber, AmountRemaining, SuspenseDate, NoteText, UserID,
					CreatedOnUTC, UpdatedOnUTC, DocumentID)
				SELECT @CheckNumber, @SuspenseAmountRemaining, @LocalNowDateOnly, @ToSuspenseNoteText,
					@UserID, @UTCNow, @UTCNow, @DocumentID;
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
		
		SET @PrntMsg = N'Error Line: ' + CONVERT(NVARCHAR(4000), @ErrLine)
		PRINT @PrntMsg

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
	END CATCH
END

GO
