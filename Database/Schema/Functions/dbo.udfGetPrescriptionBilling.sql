SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/24/2017
	Description:	Returns the Billed Amounts for Prescriptions within a given time frame,
					with optional filters for Payor.GroupName or Pharmacy.PharmacyName.
	Sample Execute:
					SELECT * FROM dbo.udfGetPrescriptionBilling('1/1/2017', '12/1/2017', NULL, NULL)
*/
CREATE FUNCTION [dbo].[udfGetPrescriptionBilling]
(
	 @StartDate DATE
	,@EndDate DATE
	,@GroupName VARCHAR(255)
	,@PharmacyName VARCHAR(60)
)
RETURNS TABLE
AS
	RETURN
		(
			SELECT   p.PrescriptionID
					,MONTH(i.InvoiceDate)  InvoicedMonth
					,YEAR(i.InvoiceDate)  InvoicedYear
					,p.BilledAmount
			FROM   dbo.Prescription AS p
					INNER JOIN dbo.Claim AS c ON c.ClaimID = p.ClaimID
					INNER JOIN dbo.Payor AS pay ON pay.PayorID = c.PayorID
					INNER JOIN dbo.Invoice AS i ON i.InvoiceID = p.InvoiceID
					INNER JOIN dbo.Pharmacy AS ph ON ph.NABP = p.PharmacyNABP
			WHERE	1 = 1
					AND p.BilledAmount > 0.00
					AND (@GroupName = pay.GroupName OR @GroupName IS NULL)
					AND (@PharmacyName = ph.PharmacyName OR @PharmacyName IS NULL)
					AND i.InvoiceDate BETWEEN @StartDate AND EOMONTH(@EndDate)
		)
GO
