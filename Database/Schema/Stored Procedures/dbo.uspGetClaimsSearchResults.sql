SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/10/2017
	Description:	Returns Claims data for Blade 1
	Sample Execute:
					EXEC dbo.uspGetClaimsSearchResults @FirstName = 'amie'
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
			SELECT c.ClaimID
			FROM   dbo.Claim AS c
			WHERE  c.ClaimNumber LIKE '%' + @ClaimNumber + '%'
		  
			UNION

			SELECT c.ClaimID
			FROM   dbo.Claim AS c
				   INNER JOIN dbo.Patient AS p ON c.PatientID = p.PatientID
			WHERE  p.FirstName LIKE '%' + @FirstName + '%'
				   OR p.LastName LIKE '%' + @LastName + '%'
			
			UNION

			SELECT p.ClaimID
			FROM   dbo.Invoice AS i
					INNER JOIN dbo.Prescription AS p ON p.InvoiceID = i.InvoiceID
			WHERE  i.InvoiceNumber = @InvoiceNumber

			UNION

			SELECT p.ClaimID
			FROM   dbo.Prescription AS p
			WHERE  p.RxNumber = @RxNumber
    )
    SELECT DISTINCT c.ClaimId
		 , c.PayorId
         , c.ClaimNumber
         , c.LastName
         , c.FirstName
         , c.Carrier
         , c.InjuryDate
    FROM   [dbo].[vwClaim] c 
           INNER JOIN ClaimsCTE cte ON cte.ClaimID = c.ClaimId
    WHERE  1 = 1
           AND (c.ClaimNumber LIKE '%' + @ClaimNumber + '%' OR @ClaimNumber IS NULL)
           AND (c.FirstName LIKE '%' + @FirstName + '%' OR @FirstName IS NULL)
           AND (c.LastName LIKE '%' + @LastName + '%' OR @LastName IS NULL)
           AND (c.InvoiceNumber = @InvoiceNumber OR @InvoiceNumber IS NULL)
           AND (c.RxNumber = @RxNumber OR @RxNumber IS NULL);
END
GO
