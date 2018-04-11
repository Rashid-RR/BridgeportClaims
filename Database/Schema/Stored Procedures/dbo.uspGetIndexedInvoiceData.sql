SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		4/8/2018
 Description:		Checks whether or not an Invoice Number already exists and
					returns the Indexed Invoice data if so.
 Example Execute:
					EXECUTE [dbo].[uspGetIndexedInvoiceData] '28062'
 =============================================
*/
CREATE PROC [dbo].[uspGetIndexedInvoiceData]
(
	@InvoiceNumber VARCHAR(100)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

		SELECT          InvoiceNumberIsAlreadyIndexed = CONVERT(BIT, 1)
					  , [d].[DocumentID] DocumentId
					  , [d].[FileUrl]
                      , [d].[FileName]
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
