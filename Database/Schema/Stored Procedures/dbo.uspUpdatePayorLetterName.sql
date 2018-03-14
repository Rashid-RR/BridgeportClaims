SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		3/13/2018
 Description:		Updates the Payor LetterName column from the Payor ID.
 Example Execute:
					EXECUTE [dbo].[uspUpdatePayorLetterName] 1, 'AAA'
 =============================================
*/
CREATE   PROC [dbo].[uspUpdatePayorLetterName] @PayorID INTEGER, @LetterName VARCHAR(255)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
			
		UPDATE  [dbo].[Payor]
		SET     [LetterName] = @LetterName
		WHERE   [PayorID] = @PayorID
			
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
