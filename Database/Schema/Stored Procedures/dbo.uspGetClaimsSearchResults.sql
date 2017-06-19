SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/10/2017
	Description:	Returns Claims data for Blade 1
	Sample Execute:
					EXEC dbo.uspGetClaimsSearchResults NULL, 'Brandie'
*/
CREATE PROC [dbo].[uspGetClaimsSearchResults]
(
    @ClaimNumber VARCHAR(255) = NULL, @FirstName VARCHAR(155) = NULL, 
    @LastName VARCHAR(155) = NULL, @RxNumber VARCHAR(100) = NULL, @InvoiceNumber NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    WITH ClaimsCTE AS
    (
          SELECT c.ClaimID FROM dbo.Claim c WHERE c.ClaimNumber = @ClaimNumber UNION
          SELECT c.ClaimID FROM dbo.Claim AS c INNER JOIN dbo.Patient p ON c.PatientID = p.PatientID
			WHERE p.FirstName = @FirstName OR p.LastName = @LastName UNION
          SELECT i.ClaimID FROM dbo.Invoice i WHERE i.InvoiceNumber = @InvoiceNumber UNION
          SELECT p.ClaimID FROM dbo.Prescription p WHERE p.RxNumber = @RxNumber
    )
    SELECT DISTINCT c.ClaimId
		 , c.PayorId
         , c.ClaimNumber
         , c.LastName
         , c.FirstName
         , c.Carrier
         , c.InjuryDate
    FROM   dbo.vwClaims c 
           INNER JOIN ClaimsCTE cte ON cte.ClaimID = c.ClaimId
    WHERE  1 = 1
           AND (c.ClaimNumber = @ClaimNumber OR @ClaimNumber IS NULL)
           AND (c.FirstName = @FirstName OR @FirstName IS NULL)
           AND (c.LastName = @LastName OR @LastName IS NULL)
           AND (c.InvoiceNumber = @InvoiceNumber OR @InvoiceNumber IS NULL)
           AND (c.RxNumber = @RxNumber OR @RxNumber IS NULL);
END
GO
