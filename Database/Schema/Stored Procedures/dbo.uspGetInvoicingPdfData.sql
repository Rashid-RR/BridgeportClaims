SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
--Can you fit an Rx Date in the message as well so we can pinpoint which script we are trying to figure a billed amount for? (
-- Will you also find an area for the claimant name to appear in the message so we can see that without having to open up the claim?
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
	DECLARE @ImportTypeEnvision INT = [etl].[udfGetImportTypeByCode]('ENVISION');
	WITH CTE AS
	(
    SELECT c1.[ClaimID] AS ClaimId,
		   p2.BillToName,
           p2.BillToAddress1,
           p2.BillToAddress2,
           p2.BillToCity,
           p2s.StateCode BillToStateCode,
           p2.BillToPostalCode,
           i1.InvoiceNumber,
           InvoiceDate = FORMAT(i1.InvoiceDate, 'MM/dd/yyyy'),
           c1.ClaimNumber,
		   p3.LastName AS PatientLastName,
		   p3.FirstName AS PatientFirstName,
		   p3.Address1 AS PatientAddress1,
		   p3.Address2 AS PatientAddress2,
		   p3.City AS PatientCity,
		   s1.StateCode AS PatientStateCode,
		   p3.PostalCode AS PatientPostalCode,
		   p3.PhoneNumber AS PatientPhoneNumber,
           DateOfBirthDay = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(DAY, p3.DateOfBirth)), 2),
		   DateOfBirthMonth = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(MONTH, p3.DateOfBirth)), 2),
		   DateOfBirthYear = RIGHT(CONVERT(VARCHAR(4), DATEPART(YEAR, p3.DateOfBirth)), 2),
		   IsMale = CAST(CASE WHEN g.GenderCode = 'M' THEN 1 ELSE 0 END AS BIT),
		   IsFemale = CAST(CASE WHEN g.GenderCode = 'F' THEN 1 ELSE 0 END AS BIT),
		   DateOfInjuryDay = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(DAY, c1.DateOfInjury)), 2),
		   DateOfInjuryMonth = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(MONTH, c1.DateOfInjury)), 2),
		   DateOfInjuryYear = RIGHT(CONVERT(VARCHAR(4), DATEPART(YEAR, c1.DateOfInjury)), 2),
		   [p1].[PrescriptionID] AS PrescriptionId,
		   p1.Prescriber,
		   p1.PrescriberNPI AS PrescriberNpi,
		   DateFilledDay = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(DAY, p1.DateFilled)), 2),
		   DateFilledMonth = RIGHT('00' + CONVERT(VARCHAR(2), DATEPART(MONTH, p1.DateFilled)), 2),
		   DateFilledYear = RIGHT(CONVERT(VARCHAR(4), DATEPART(YEAR, p1.DateFilled)), 2),
		   p1.NDC AS Ndc,
		   p1.LabelName,
		   p1.RxNumber,
		   p1.BilledAmount,
		   TRY_CAST(FLOOR(p1.BilledAmount) AS INT) AS BilledAmountDollars,
		   TRY_CAST(p1.BilledAmount % 1 AS VARCHAR(4)) AS [BilledAmountCents],
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
		INNER JOIN dbo.UsState AS phars ON phars.StateID = p4.StateID
        INNER JOIN dbo.Claim AS c1 ON c1.ClaimID = p1.ClaimID
        INNER JOIN dbo.Payor AS p2 ON p2.PayorID = c1.PayorID
        INNER JOIN dbo.Patient AS p3 ON p3.PatientID = c1.PatientID
		INNER JOIN dbo.Gender AS g ON g.GenderID = p3.GenderID
		LEFT JOIN dbo.Invoice AS i1 ON i1.InvoiceID = p1.InvoiceID
		LEFT JOIN dbo.UsState AS s1 ON s1.StateID = p3.StateID
		LEFT JOIN dbo.UsState AS p2s ON p2.BillToStateID = p2s.StateID
	WHERE p1.[ImportTypeID] = @ImportTypeEnvision
	-- testing
	      AND c1.ClaimID = 5946
	)
	SELECT * FROM [CTE] AS c
END;
GO
