SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       4/6/2019
 Description:       Gets the data needed to populate an invoice PDF.
 Example Execute:
                    EXECUTE [dbo].[uspGetInvoicingPdfData]
 =============================================
*/
CREATE PROCEDURE [dbo].[uspGetInvoicingPdfData]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SELECT TOP (1) p2.BillToName,
           p2.BillToAddress1,
           p2.BillToAddress2,
           p2.BillToCity,
           p2s.StateCode BillToStateCode,
           p2.BillToPostalCode,
           i1.InvoiceNumber,
           InvoiceDate = FORMAT(i1.InvoiceDate, 'MM/dd/yyyy'),
           c1.ClaimNumber,
           DateOfBirthDay = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(DAY, p3.DateOfBirth)), 2),
		   DateOfBirthMonth = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(MONTH, p3.DateOfBirth)), 2),
		   DateOfBirthYear = RIGHT(CONVERT(VARCHAR(4), DATEPART(YEAR, p3.DateOfBirth)), 2),
		   IsMale = CAST(CASE WHEN g.GenderCode = 'M' THEN 1 ELSE 0 END AS BIT),
		   IsFemale = CAST(CASE WHEN g.GenderCode = 'F' THEN 1 ELSE 0 END AS BIT),
		   p3.PhoneNumber,
		   DateOfInjuryDay = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(DAY, c1.DateOfInjury)), 2),
		   DateOfInjuryMonth = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(MONTH, c1.DateOfInjury)), 2),
		   DateOfInjuryYear = RIGHT(CONVERT(VARCHAR(4), DATEPART(YEAR, c1.DateOfInjury)), 2),
		   p1.Prescriber,
		   p1.PrescriberNPI AS PrescriberNpi,
		   DateFilledDay = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(DAY, p1.DateFilled)), 2),
		   DateFilledMonth = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(MONTH, p1.DateFilled)), 2),
		   DateFilledYear = RIGHT(CONVERT(VARCHAR(4), DATEPART(YEAR, p1.DateFilled)), 2),
		   p1.NDC AS Ndc,
		   p1.LabelName,
		   p1.RxNumber,
		   p1.BilledAmount,
		   p1.Quantity,
		   p4.PharmacyName,
		   p4.Address1,
		   p4.Address2,
		   p4.City,
		   PharmacyState = phars.StateCode,
		   p4.PostalCode,
		   p4.FederalTIN AS FederalTin,
		   p4.NPI AS Npi,
		   p4.NABP Nabp
    FROM dbo.Prescription AS p1
        INNER JOIN dbo.Pharmacy AS p4 ON p4.NABP = p1.PharmacyNABP
        INNER JOIN dbo.Claim AS c1 ON c1.ClaimID = p1.ClaimID
        INNER JOIN dbo.Payor AS p2 ON p2.PayorID = c1.PayorID
        INNER JOIN dbo.Patient AS p3 ON p3.PatientID = c1.PatientID
		INNER JOIN dbo.Gender AS g ON g.GenderID = p3.GenderID
		LEFT JOIN dbo.Invoice AS i1 ON i1.InvoiceID = p1.InvoiceID
		LEFT JOIN dbo.UsState AS s1 ON s1.StateID = p3.StateID
		LEFT JOIN dbo.UsState AS p2s ON p2.BillToStateID = p2s.StateID
		LEFT JOIN dbo.UsState AS phars ON phars.StateID = p4.StateID;
END;
GO
