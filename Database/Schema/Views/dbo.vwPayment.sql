SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwPayment]
AS
	SELECT PaymentID = pp.PrescriptionPaymentID
		 , pp.CheckNumber
		 , pp.AmountPaid
		 , pp.DatePosted
		 , pp.PrescriptionID
		 , CAST(NULL AS INT) ClaimID
		 , pp.CreatedOnUTC
		 , pp.UpdatedOnUTC
	FROM   dbo.PrescriptionPayment AS pp
	UNION ALL
	SELECT PaymentID = cp.ClaimPaymentID
		 , cp.CheckNumber
		 , cp.AmountPaid
		 , cp.DatePosted
		 , CAST(NULL AS INT) PrescriptionID
		 , cp.ClaimID
		 , cp.CreatedOnUTC
		 , cp.UpdatedOnUTC
	FROM   dbo.ClaimPayment AS cp
GO
