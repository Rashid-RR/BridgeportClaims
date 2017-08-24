SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/7/2017
	Description:	Gets the results in order to narrow down the search for the Payment Page.
	Sample Execute:
					EXEC [dbo].[uspGetClaimsWithPrescriptionDetails] '8,9,10,11'
*/
CREATE PROC [dbo].[uspGetClaimsWithPrescriptionDetails]
(
	@ClaimIDs VARCHAR(4000)
)
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL READ COMMITTED; -- SNAPSHOT
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		
		SELECT [pr].[PrescriptionID] PrescriptionId
			 , [c].[ClaimID] ClaimId
			 , [c].[ClaimNumber]
			 , PatientName = [p].[FirstName] + ' ' + [p].[LastName]
			 , RxDate = CAST(FORMAT([pr].[DateFilled], 'M/d/yyyy') AS DATE)
			 , [pr].[RxNumber]
			 , [pr].[LabelName]
			 , InvoicedAmount = [pr].[BilledAmount]
			 , Outstanding = [pr].[BilledAmount] - ISNULL([ot].[AmountPaid], 0)
			 , Payor = [py].[GroupName]
		FROM   [dbo].[Claim] AS [c]
			   INNER JOIN STRING_SPLIT(@ClaimIDs, ',') AS [ss] ON [ss].[value] = [c].[ClaimID]
			   INNER JOIN [dbo].[Payor] AS [py] ON [py].[PayorID] = [c].[PayorID]
			   INNER JOIN [dbo].[Patient] AS p ON [p].[PatientID] = [c].[PatientID]
			   INNER JOIN [dbo].[Prescription] AS [pr] ON [pr].[ClaimID] = [c].[ClaimID]
			   LEFT JOIN [dbo].[Invoice] AS [i] ON [i].[InvoiceID] = [pr].[InvoiceID]
			   OUTER APPLY (   SELECT AmountPaid = SUM([ipm].[AmountPaid])
							   FROM   [dbo].[PrescriptionPayment] AS [ipm]
							   WHERE  [ipm].[PrescriptionID] = [pr].[PrescriptionID]
						   ) AS ot
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
