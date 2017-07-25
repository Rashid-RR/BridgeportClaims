SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/24/2017
	Description:	Simply removes the data from the ETL columns
	Sample Execute:
					EXEC etl.uspClearDataFromAddedColumns
*/
CREATE PROC [etl].[uspClearDataFromAddedColumns]
AS BEGIN
	SET NOCOUNT ON;
	UPDATE [etl].[StagedLakerFile]
	SET    PayorID = NULL
		 , AdjustorID = NULL
		 , PatientID = NULL
		 , InvoiceID = NULL
		 , ClaimID = NULL
		 , PrescriptionID = NULL
		 , PharmacyID = NULL
		 , PaymentID = NULL
END
GO
