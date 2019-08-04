SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[udfIsInvoiceDocumentWithinPrescriptionLimit]()
RETURNS bit
AS BEGIN
	IF EXISTS
    (
		SELECT [i].[InvoiceNumber]
		FROM [dbo].[InvoiceDocument] [i]
		GROUP BY [i].[InvoiceNumber]
		HAVING COUNT(*) > 6	
	)
		RETURN 0
	RETURN 1
end
GO
