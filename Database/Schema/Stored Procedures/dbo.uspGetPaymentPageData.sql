SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/7/2017
	Description:	Gets the results in order to narrow down the search for the Payment Page.
	Sample Execute:
					EXEC dbo.uspGetPaymentPageData 695
*/
CREATE PROC [dbo].[uspGetPaymentPageData]
(
	@ClaimID INT
)
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		
		SELECT [c].[ClaimNumber]
			 , PatientName = [p].[FirstName] + ' ' + [p].[LastName]
			 , RxDate = FORMAT([pr].[DateFilled], 'M/d/yyyy')
			 , [pr].[RxNumber]
			 , [pr].[LabelName]
			 , InvoicedAmount = [pr].[BilledAmount]
			 , [ot].[AmountPaid]
			 , Outstanding = [pr].[BilledAmount] - [ot].[AmountPaid]
			 , Payor = [py].[GroupName]
		FROM   [dbo].[Claim] AS [c]
			   INNER JOIN [dbo].[Payor] AS [py] ON [py].[PayorID] = [c].[PayorID]
			   INNER JOIN [dbo].[Patient] AS p ON [p].[PatientID] = [c].[PatientID]
			   INNER JOIN [dbo].[Prescription] AS [pr] ON [pr].[ClaimID] = [c].[ClaimID]
			   LEFT JOIN [dbo].[Invoice] AS [i] ON [i].[InvoiceID] = [pr].[InvoiceID]
			   OUTER APPLY (   SELECT AmountPaid = SUM([ipm].[AmountPaid])
							   FROM   [dbo].[Payment] AS [ipm]
							   WHERE  [ipm].[PrescriptionID] = [pr].[PrescriptionID]
						   ) AS ot
		WHERE  [c].[ClaimID] = @ClaimID

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
