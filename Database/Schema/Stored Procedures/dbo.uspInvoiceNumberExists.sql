SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		4/3/2018
 Description:		Checks whether or not an Invoice Number already exists.
 Example Execute:
					EXECUTE [dbo].[uspInvoiceNumberExists] '28062'
 =============================================
*/
CREATE PROC [dbo].[uspInvoiceNumberExists]
(
	@InvoiceNumber VARCHAR(100),
	@FileUrl NVARCHAR(500) OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
		
		SELECT          @FileUrl = [d].[FileUrl]
		FROM            [dbo].[InvoiceIndex] AS [ii]
			INNER JOIN  [dbo].[Document]     AS [d] ON [d].[DocumentID] = [ii].[DocumentID]
		WHERE           [ii].[InvoiceNumber] = @InvoiceNumber
	
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
