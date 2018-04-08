SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		4/7/2018
 Description:		Deletes any InvoiceIndex record associated with it first, and then updates
					the document to Archived = t
 Example Execute:
					EXECUTE [dbo].[uspArchiveDocument]
 =============================================
*/
CREATE PROC [dbo].[uspArchiveDocument]
(
	@DocumentID INT,
	@ModifiedByUserID NVARCHAR(128)
)
AS BEGIN
	SET NOCOUNT, XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
			
		-- Remove any Index Invoicing.
		DELETE	[ii]
		FROM    [dbo].[InvoiceIndex] AS [ii]
		WHERE   [ii].[DocumentID] = @DocumentID
	
		-- Update the actual document to Archivel
		UPDATE  [dbo].[Document]
		SET     [Archived] = 1,
				[UpdatedOnUTC] = SYSUTCDATETIME(), 
				[ModifiedByUserID] = @ModifiedByUserID
		WHERE   [DocumentID] = @DocumentID
			
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
