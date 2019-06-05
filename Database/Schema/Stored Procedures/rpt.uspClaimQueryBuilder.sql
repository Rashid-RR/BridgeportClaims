SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/3/2019
	Description:	Report proc to populate the Claims Query Builder report.
	Sample Execute:
					EXEC rpt.uspClaimQueryBuilder
*/
CREATE PROCEDURE [rpt].[uspClaimQueryBuilder]
AS
    BEGIN
        SELECT ClaimId = cl.ClaimID
              ,PrescriptionId = pre.PrescriptionID
              ,car.GroupName
              ,Pharmacy = ph.PharmacyName
              ,st.StateCode
              ,pre.DateSubmitted
              ,Billed = pre.BilledAmount
              ,Payable = pre.PayableAmount
              ,Collected = pay.AmountPaid
              ,pre.Prescriber
              ,PatientLast = pat.LastName
              ,PatientFirst = pat.FirstName
              ,ClaimNumber = cl.ClaimNumber
              ,cl.IsAttorneyManaged
              ,a.AttorneyName
			  ,pre.LabelName
			  ,pre.RxNumber
			  ,pre.DateFilled
			  ,Ndc = pre.NDC
			  ,i.InvoiceNumber
			  ,i.InvoiceDate
        FROM   dbo.Prescription AS pre
               INNER JOIN dbo.Claim AS cl ON pre.ClaimID = cl.ClaimID
               INNER JOIN dbo.Pharmacy AS ph ON pre.PharmacyNABP = ph.NABP
               INNER JOIN dbo.UsState AS st ON st.StateID = ph.StateID
               INNER JOIN dbo.Patient AS pat ON cl.PatientID = pat.PatientID
               INNER JOIN dbo.Payor AS car ON cl.PayorID = car.PayorID
               OUTER APPLY (
								SELECT AmountPaid = Sum(ipay.AmountPaid)
								FROM   dbo.PrescriptionPayment AS ipay
								WHERE  pre.PrescriptionID = ipay.PrescriptionID
							) AS pay
               LEFT JOIN dbo.Attorney AS a ON a.AttorneyID = cl.AttorneyID
			   LEFT JOIN dbo.Invoice AS i ON i.InvoiceID = pre.InvoiceID;
    END;
GO
