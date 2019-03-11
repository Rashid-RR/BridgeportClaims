SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Date:			1/18/2018
	Description:	Edits a Claim and associated entities from the Claims blade.
	Example Execute:
					DECLARE @UserID NVARCHAR(128);
					SELECT TOP (1) @UserID = x.ID FROM dbo.AspNetUsers AS x ORDER BY NEWID();
					EXECUTE dbo.uspEditClaim 775, @UserID, '20190218'
*/
CREATE PROCEDURE [dbo].[uspEditClaim]
(
	@ClaimID INTEGER,
	@ModifiedByUserID NVARCHAR(128),
	@DateOfBirth DATE = '1/1/1901',
	@GenderID INTEGER = -1,
	@PayorID INTEGER = -1,
	@AdjustorID INTEGER = -1,
	@AttorneyID INTEGER = -1,
	@DateOfInjury DATE = '1/1/1901',
	@Address1 VARCHAR(255) = 'NULL',
	@Address2 VARCHAR(255) = 'NULL',
	@City VARCHAR(155) = 'NULL',
	@StateID INTEGER = -1,
	@PostalCode varchar(100) = 'NULL',
	@ClaimFlex2ID INTEGER = -1
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
				SET @PrntMsg = N'Error. The Claim # ' + CONVERT(NVARCHAR(500), @ClaimID) + ' does not exist.'
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT
				RETURN -1;
			END

		-- UPDATE Patient
		IF (@DateOfBirth != '1/1/1901' OR @DateOfBirth IS NULL OR @GenderID != -1 OR @Address1 != 'NULL'
			OR @Address1 IS NULL OR @Address2 != 'NULL' OR @Address2 IS NULL OR @City != 'NULL' OR @City IS NULL
			OR @StateID != -1 OR @StateID IS NULL OR @PostalCode != 'NULL' OR @PostalCode IS NULL)
			BEGIN
				UPDATE          [p]
				SET             [p].[DateOfBirth] = CASE WHEN @DateOfBirth = '1/1/1901' THEN [p].[DateOfBirth] ELSE @DateOfBirth END, 
								[p].[GenderID] = CASE WHEN @GenderID = -1 THEN [p].[GenderID] ELSE @GenderID END,
								[p].[Address1] = CASE when @Address1 = 'NULL' THEN [p].[Address1] ELSE @Address1 END,
								[p].[Address2] = CASE WHEN @Address2 = 'NULL' THEN [p].[Address2] ELSE @Address2 END,
								[p].[City] = CASE WHEN @City = 'NULL' THEN [p].[City] ELSE @City END,
								[p].[StateID] = CASE WHEN @StateID = -1 THEN [p].[StateID] ELSE @StateID END,
								[p].[PostalCode] = CASE WHEN @PostalCode = 'NULL' THEN [p].[PostalCode] ELSE @PostalCode END,
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
						SET @PrntMsg = N'The affected Row Count for updating the Patient was not 1, it was ' + CONVERT(NVARCHAR(500), @RowCount);
						RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
						RETURN -1;
					END
			END

		-- UPDATE Claim
		IF (@PayorID != -1 OR @AdjustorID != -1 OR @AdjustorID IS NULL 
			OR @DateOfInjury != '1/1/1901' OR @DateOfInjury IS NULL
			OR @ClaimFlex2ID IS NULL OR @ClaimFlex2ID != -1 OR @AttorneyID != -1)
			BEGIN
				UPDATE  [c]
				SET     [c].[PayorID] = CASE WHEN @PayorID = -1 THEN [c].[PayorID] ELSE @PayorID END, 
						[c].[AdjustorID] = CASE WHEN @AdjustorID = -1 THEN [c].[AdjustorID] ELSE @AdjustorID END,
						[c].[DateOfInjury] = CASE WHEN @DateOfInjury = '1/1/1901' THEN [c].[DateOfInjury] ELSE @DateOfInjury END,
						[c].[ClaimFlex2ID] = CASE WHEN @ClaimFlex2ID = -1 THEN [c].[ClaimFlex2ID] ELSE @ClaimFlex2ID END,
						[c].[ModifiedByUserID] = @ModifiedByUserID,
						[c].[AttorneyID] = CASE WHEN @AttorneyID = -1 THEN [c].[AttorneyID] ELSE @AttorneyID END,
						[c].[UpdatedOnUTC] = @UtcNow
				FROM    [dbo].[Claim] AS [c]
				WHERE   [c].[ClaimID] = @ClaimID
				SET @RowCount = @@ROWCOUNT

				IF (@RowCount != 1)
					BEGIN
						IF (@@TRANCOUNT > 0)
							ROLLBACK;
						SET @PrntMsg = N'The affected Row Count for updating the Claim was not 1, it was ' + CONVERT(NVARCHAR(500), @RowCount);
						RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
						RETURN -1;
					END
			END

		IF (@@TRANCOUNT > 0)
			COMMIT;
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK;
		DECLARE @Msg NVARCHAR(4000) = FORMATMESSAGE(N'An error has occurred: %s Line number: %u', ERROR_MESSAGE(), ERROR_LINE());
        THROW 50000, @Msg, 0;
	END CATCH
END
GO
