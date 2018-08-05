SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       7/29/2018
 Description:       Gets the invoice information needed to return data to 
					the user to select a prescription to change the Billing Amt (AKA Inv Amount) 
 Example Execute:
                    EXECUTE [dbo].[uspGetInvoiceInfo] 775, '291', '5/11/2017', '2594'
 =============================================
*/
CREATE PROC [dbo].[uspGetInvoiceInfo]
(
	@ClaimID INTEGER, 
	@RxNumber VARCHAR(100),
	@RxDate DATETIME2 = NULL,
	@InvoiceNumber VARCHAR(100) = NULL
)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
		DECLARE @DateFormat VARCHAR(10) = 'M/d/yyyy'
        DECLARE @WildCard CHAR(1) = '%';
        SELECT  p.PrescriptionID PrescriptionId, p.RxNumber, p.LabelName, FORMAT(p.DateFilled, @DateFormat) RxDate, 
				p.BilledAmount, p2.GroupName Carrier, I.InvoiceNumber, FORMAT(i.InvoiceDate, @DateFormat) InvoiceDate, p.IsReversed
        FROM    dbo.Prescription AS p LEFT JOIN dbo.Invoice AS i ON p.InvoiceID = i.InvoiceID
				INNER JOIN dbo.Claim AS c ON p.ClaimID = c.ClaimID
				INNER JOIN dbo.Payor AS p2 ON c.PayorID = p2.PayorID
        WHERE   p.ClaimID = @ClaimID
                AND p.RxNumber LIKE CONCAT(@RxNumber, @WildCard)
				AND p.DateFilled = ISNULL(@RxDate, p.DateFilled)
				AND (i.InvoiceNumber = @InvoiceNumber OR i.InvoiceNumber IS NULL);
    END
GO
