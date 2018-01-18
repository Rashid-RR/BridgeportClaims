SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Date:			1/18/2018
	Description:	Edits a Claim and associated entities from the Claims blade.
	Example Execute:
					EXECUTE dbo.uspEditClaim 775, '9/9/1981', 1
*/
CREATE PROCEDURE [dbo].[uspEditClaim]
(
	@ClaimID INTEGER,
	@ModifiedByUserID NVARCHAR(128),
	@DateOfBirth DATE = '1/1/1901',
	@GenderID INTEGER = -1,
	@PayorID INTEGER = -1,
	@AdjustorID INTEGER = -1,
	@AdjustorPhone VARCHAR(30) = 'NULL',
	@DateOfInjury DATE = '1/1/1901',
	@AdjustorFax VARCHAR(30) = 'NULL'
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		DECLARE @PrntMsg NVARCHAR(1000),
				@RowCount INTEGER,
				@UtcNow DATETIME2 = [dtme].[udfGetLocalDate]();

		IF NOT EXISTS
		(
			SELECT  *
			FROM    [dbo].[Claim] AS [c]
			WHERE   [c].[ClaimID] = @ClaimID
		)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				SET @PrntMsg = N'Error. The Claim # ' + CONVERT(NVARCHAR, @ClaimID) + ' does not exist.'
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT
				RETURN;
			END

		-- Ok, the fun begins.
		IF (@DateOfBirth != '1/1/1901' OR @DateOfBirth IS NULL OR @GenderID != -1)
			BEGIN
				UPDATE          [p]
				SET             [p].[DateOfBirth] = CASE WHEN @DateOfBirth = '1/1/1901' THEN [p].[DateOfBirth] ELSE @DateOfBirth END, 
								[p].[GenderID] = CASE WHEN @GenderID = -1 THEN [p].[GenderID] ELSE @GenderID END,
								[p].[ModifiedByUserID] = @ModifiedByUserID,
								[p].[UpdatedOnUTC] = @UtcNow
				FROM            [dbo].[Claim]   AS [c]
					INNER JOIN  [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
				WHERE           [c].[ClaimID] = @ClaimID
				SET @RowCount = @@ROWCOUNT

				IF (@RowCount != 1)
					BEGIN
						IF (@@TRANCOUNT > 0)
							ROLLBACK;
						SET @PrntMsg = N'The affected Row Count for updating the Patient was not 1, it was ' + CONVERT(NVARCHAR, @RowCount);
						RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
						RETURN;
					END
			END

		IF (@PayorID != -1 OR @AdjustorID != -1 OR @AdjustorID IS NULL 
			OR @DateOfInjury != '1/1/1901' OR @DateOfInjury IS NULL)
			BEGIN
				UPDATE  [c]
				SET     [c].[PayorID] = CASE WHEN @PayorID = -1 THEN [c].[PayorID] ELSE @PayorID END, 
						[c].[AdjusterID] = CASE WHEN @AdjustorID = -1 THEN [c].[AdjusterID] ELSE @AdjustorID END,
						[c].[DateOfInjury] = CASE WHEN @DateOfInjury = '1/1/1901' THEN [c].[DateOfInjury] ELSE @DateOfInjury END,
						[c].[ModifiedByUserID] = @ModifiedByUserID,
						[c].[UpdatedOnUTC] = @UtcNow
				FROM    [dbo].[Claim] AS [c]
				WHERE   [c].[ClaimID] = @ClaimID
				SET @RowCount = @@ROWCOUNT

				IF (@RowCount != 1)
					BEGIN
						IF (@@TRANCOUNT > 0)
							ROLLBACK;
						SET @PrntMsg = N'The affected Row Count for updating the Claim was not 1, it was ' + CONVERT(NVARCHAR, @RowCount);
						RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
						RETURN;
					END
			END

		IF (@AdjustorPhone != 'NULL' OR @AdjustorPhone IS NULL
			OR @AdjustorFax != 'NULL' OR @AdjustorFax IS NULL)
			BEGIN
				UPDATE          [a]
				SET             [a].[PhoneNumber] = CASE WHEN @AdjustorPhone = 'NULL' THEN [a].[PhoneNumber] ELSE @AdjustorPhone END,
								[a].[FaxNumber] = CASE WHEN @AdjustorFax = 'NULL' THEN [a].[FaxNumber] ELSE @AdjustorFax END,
								[a].[ModifiedByUserID] = @ModifiedByUserID,
								[a].[UpdatedOnUTC] = @UtcNow
				FROM            [dbo].[Claim]    AS [c]
					INNER JOIN  [dbo].[Adjustor] AS [a] ON [a].[AdjustorID] = [c].[AdjusterID]
				WHERE           [c].[ClaimID] = @ClaimID
				SET @RowCount = @@ROWCOUNT

				IF (@RowCount != 1)
					BEGIN
						IF (@@TRANCOUNT > 0)
							ROLLBACK;
						SET @PrntMsg = N'The affected Row Count for updating the Adjustor was not 1, it was ' + CONVERT(NVARCHAR, @RowCount);
						RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
						RETURN;
					END
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
