SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:			Jordan Gurney
 Create date:		3/16/2018
 Description:		Takes in a list (user-defined table type) of Prescription ID's and returns the list
					of Invoice URL's based on the assumption that these are all Indexed Invoices that
					are being passed in.
 Example Execute:
					EXECUTE [dbo].[uspGetInvoiceUrlsFromPrescriptionIDs]
 =============================================
*/
CREATE PROC [dbo].[uspGetInvoiceUrlsFromPrescriptionIDs]
(
	@PrescriptionIDs [dbo].[udtID] READONLY
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

		DECLARE @PrntMsg NVARCHAR(1000);
			
		IF EXISTS
        (
			SELECT          *
			FROM            @PrescriptionIDs     AS [pid]
				LEFT JOIN   [dbo].[Prescription] AS [p] ON [p].[PrescriptionID] = [pid].[ID]
			WHERE           [p].[PrescriptionID] IS NULL
		)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				SET @PrntMsg = N'Error, not all Prescription ID''s passed in match up to real Prescription records.';
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		IF EXISTS
        (
			SELECT          *
			FROM            @PrescriptionIDs     AS [pid]
				INNER JOIN  [dbo].[Prescription] AS [p] ON [p].[PrescriptionID] = [pid].[ID]
				LEFT JOIN   [dbo].[Invoice]      AS [i] ON [i].[InvoiceID] = [p].[InvoiceID]
			WHERE           [i].[InvoiceID] IS NULL
		)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				SET @PrntMsg = N'Error, not all Prescription ID''s passed in match up to real Invoice records.';
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		IF EXISTS
        (
			SELECT          *
			FROM            @PrescriptionIDs     AS [pid]
				INNER JOIN  [dbo].[Prescription] AS [p] ON [p].[PrescriptionID] = [pid].[ID]
				INNER JOIN  [dbo].[Invoice]      AS [i] ON [i].[InvoiceID] = [p].[InvoiceID]
				LEFT JOIN	[dbo].[InvoiceIndex] AS [ii] ON [ii].[InvoiceNumber] = [i].[InvoiceNumber]
			WHERE           [ii].[DocumentID] IS NULL
		)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				SET @PrntMsg = N'Error, not all Prescription ID''s passed in match up to real Invoice Index records.';
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		SELECT DISTINCT [d].[FileUrl] InvoiceUrl
		FROM            @PrescriptionIDs     AS [pid]
			INNER JOIN  [dbo].[Prescription] AS [p] ON [p].[PrescriptionID] = [pid].[ID]
			INNER JOIN  [dbo].[Invoice]      AS [i] ON [i].[InvoiceID] = [p].[InvoiceID]
			INNER JOIN  [dbo].[InvoiceIndex] AS [ii] ON [ii].[InvoiceNumber] = [i].[InvoiceNumber]
			INNER JOIN  [dbo].[Document]     AS [d] ON [d].[DocumentID] = [ii].[DocumentID]
			
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
