SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/20/2019
	Description:	Proc that populates the Invoices Processes
	Sample Execute:
					EXECUTE [dbo].[uspInvoicesProcess]
*/
CREATE PROCEDURE [dbo].[uspInvoicesProcess]
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @ImportType INT = [etl].[udfGetImportTypeByCode]('ENVISION');
    SELECT   p.[DateFilled] RxDate
            ,Carrier = car.GroupName
            ,PatientName = pt.LastName + ', ' + pt.FirstName
            ,C.ClaimNumber
            ,InQueue = COUNT(DISTINCT p.[PrescriptionID])
    FROM     dbo.Prescription AS p
             INNER JOIN dbo.Claim AS C ON C.ClaimID = P.ClaimID
             INNER JOIN dbo.Payor AS car ON car.PayorID = C.PayorID
             INNER JOIN dbo.Patient AS pt ON pt.PatientID = C.PatientID
	WHERE	p.[ImportTypeID] = @ImportType
			AND NOT EXISTS (SELECT * FROM [dbo].[InvoiceDocument] AS [id] WHERE [id].[PrescriptionID] = [p].[PrescriptionID])
    GROUP BY p.[DateFilled]
            ,car.GroupName
            ,pt.LastName + ', ' + pt.FirstName
            ,C.ClaimNumber;
END;
GO
