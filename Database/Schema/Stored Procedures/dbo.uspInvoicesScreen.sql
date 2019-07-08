SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/10/2019
	Description:	Proc that populates the Invoices Screen
	Sample Execute:
					EXECUTE [dbo].[uspInvoicesScreen]
*/
CREATE PROCEDURE [dbo].[uspInvoicesScreen]
WITH RECOMPILE
AS BEGIN
    SELECT   I.InvoiceDate
            ,Carrier = car.GroupName
            ,PatientName = pt.LastName + ', ' + pt.FirstName
            ,C.ClaimNumber
            ,InvoiceCount = Count(I.InvoiceID)
            ,ScriptCount = Count(P.PrescriptionID)
            ,Printed = 0
            ,TotalToPrint = 0
    FROM     dbo.Invoice AS I
                INNER JOIN dbo.Prescription AS P ON P.InvoiceID = I.InvoiceID
                INNER JOIN dbo.Claim AS C ON C.ClaimID = P.ClaimID
                INNER JOIN dbo.Payor AS car ON car.PayorID = C.PayorID
                INNER JOIN dbo.Patient AS pt ON pt.PatientID = C.PatientID
    GROUP BY I.InvoiceDate
            ,car.GroupName
            ,pt.LastName + ', ' + pt.FirstName
            ,C.ClaimNumber;
END;
GO
