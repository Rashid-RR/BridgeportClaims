SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/23/2017
	Description:	
	Sample Execute:
					EXEC dbo.uspInsertClaimsUserHistory 9, 'b33804ac-6bd2-4895-a6d3-3cb59a0dc830'
*/
CREATE PROC [dbo].[uspInsertClaimsUserHistory]
(
	@ClaimID INT,
	@UserID NVARCHAR(128)
)
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	BEGIN TRY
		BEGIN TRANSACTION;
		
		DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME()
			   ,@MagicNumberOfHistoryToKeep TINYINT = 20
			   ,@PrntMsg VARCHAR(1000)
			   ,@RowCount INT

		-- Quick QA Check
		IF (SELECT COUNT(*) FROM [dbo].[ClaimsUserHistory] WHERE [UserID] = @UserID) > @MagicNumberOfHistoryToKeep
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				SET @PrntMsg = N'Whoa, we cannot have more than ' + CAST(@MagicNumberOfHistoryToKeep AS VARCHAR) + ' rows per user'
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT
                RETURN
			END

		-- If we've hit the max, remove one entry
		IF (SELECT COUNT(*)
			FROM   [dbo].[ClaimsUserHistory] AS h
			WHERE  [h].[UserID] = @UserID) = @MagicNumberOfHistoryToKeep
			BEGIN
				DELETE	h
				FROM	[dbo].[ClaimsUserHistory] AS h
						CROSS APPLY (SELECT   TOP 1 ih.[ClaimsUserHistoryID] -- Grab earliest entry
									 FROM     [dbo].[ClaimsUserHistory] AS ih
									 WHERE    [h].[UserID] = [ih].[UserID]
									 ORDER BY [ih].[CreatedOnUTC] ASC
								   ) AS d
				WHERE	h.[ClaimsUserHistoryID] = d.[ClaimsUserHistoryID]
					    AND h.[UserID] = @UserID
				SET @RowCount = @@ROWCOUNT 
				
				IF @RowCount != 1
					BEGIN
						IF @@TRANCOUNT > 0
							ROLLBACK
						SET @PrntMsg = N'There should have been one row removed, but ' + CAST(@RowCount AS VARCHAR) + ' row(s) were removed instead'
						RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT
						RETURN
					END
			END

	
		INSERT INTO dbo.[ClaimsUserHistory] WITH (TABLOCKX) ([ClaimID],[UserID],[CreatedOnUTC],[UpdatedOnUTC])
		VALUES (@ClaimID,@UserID,@UtcNow,@UtcNow)

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
