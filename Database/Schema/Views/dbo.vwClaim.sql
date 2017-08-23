SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/13/2017
	Description:	View to reduce code footprint of query for Claims Panel
	Sample Execute:
					SELECT * FROM dbo.vwClaims
*/
CREATE VIEW [dbo].[vwClaim]
AS 
	SELECT DISTINCT ClaimId = c.ClaimID
			, PayorId = pa.PayorID
			, [Name] = NULLIF(ISNULL(LTRIM(RTRIM(p.FirstName)), '') + ' ' + ISNULL(LTRIM(RTRIM(p.LastName)), ''), ' ')
			, c.ClaimNumber
			, DateOfBirth = FORMAT(p.DateOfBirth, 'M/d/yyyy')
			, InjuryDate = FORMAT(c.DateOfInjury, 'M/d/yyyy')
			, Gender = g.GenderName
			, Carrier = pa.BillToName
			, Adjustor = a.AdjustorName
			, AdjustorPhoneNumber = a.PhoneNumber
			, DateEntered = FORMAT(c.TermDate, 'M/d/yyyy')
			, AdjustorFaxNumber = a.FaxNumber
			, i.InvoiceNumber
			, p.FirstName
			, p.LastName
			, pre.RxNumber
    FROM   dbo.Claim c 
           LEFT JOIN dbo.Patient p INNER JOIN dbo.Gender g ON g.GenderID = p.GenderID ON p.PatientID = c.PatientID
           LEFT JOIN dbo.Payor pa ON pa.PayorID = c.PayorID
           LEFT JOIN dbo.Adjustor a ON a.AdjustorID = c.AdjusterID
           LEFT JOIN dbo.Prescription pre ON pre.ClaimID = c.ClaimID
		   LEFT JOIN dbo.Invoice i ON i.InvoiceID = pre.InvoiceID

GO
