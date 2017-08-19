SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/1/2017
	Description:	Imports data for the Payment table from an Excel file.
	Sample Execute:
					EXEC dbo.uspImportPaymentFromDataTable
*/
CREATE PROC [dbo].[uspImportPaymentFromDataTable] @Payment [dbo].[udtPayment] READONLY
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		WITH PaymentsCTE AS
        (
			SELECT p.CheckNumber,
				   p.LastName,
				   p.FirstName,
				   p.RxNumber,
				   p.RxDate,
				   p.InvoiceNumber,
				   p.AmountPaid,
				   p.DateScanned,
				   p.PrescriptionID,
				   p.ClaimID
			FROM @Payment AS p
			WHERE NOT (p.CheckNumber IS NULL
				  AND p.LastName IS NULL
				  AND p.FirstName IS NULL
				  AND p.RxNumber IS NULL
				  AND p.RxDate IS NULL
				  AND p.InvoiceNumber IS NULL
				  AND p.AmountPaid IS NULL
				  AND p.DateScanned IS NULL
				  AND p.PrescriptionID IS NULL
				  AND p.ClaimID IS NULL)
		)
		INSERT [dbo].[Payment] ([CheckNumber],[AmountPaid],[DateScanned],[PrescriptionID],[ClaimID])
		SELECT [p].[CheckNumber]
			 , [p].[AmountPaid]
			 , [p].[DateScanned]
			 , [p].[PrescriptionID]
			 , [p].[ClaimID]
		FROM PaymentsCTE AS [p];
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
