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
	@DateOfBirth DATE,
	@GenderID INTEGER,
	@PayorID INTEGER,
	@AdjustorID INTEGER,
	@AdjustorPhone VARCHAR(30),
	@DateOfInjury DATE,
	@AdjustorFax VARCHAR(30),
	@ModifiedByUserID NVARCHAR(128)
)
-- SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] AS [c] WHERE c.[COLUMN_NAME] = 'GenderID'
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
		UPDATE          [p]
		SET             [p].[DateOfBirth] = @DateOfBirth, 
						[p].[GenderID] = @GenderID,
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

		UPDATE  [c]
		SET     [c].[PayorID] = @PayorID, 
				[c].[AdjusterID] = @AdjustorID, 
				[c].[DateOfInjury] = @DateOfInjury,
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
