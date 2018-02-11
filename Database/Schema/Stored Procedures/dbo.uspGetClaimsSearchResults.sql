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
			FROM   [dbo].[vwClaimInfo] AS c WITH (NOEXPAND)
			WHERE  c.ClaimNumber LIKE '%' + @ClaimNumber + '%'
		  
			UNION

			SELECT c.ClaimID
			FROM   [dbo].[vwClaimInfo] c WITH (NOEXPAND)
			WHERE  c.FirstName LIKE '%' + @FirstName + '%'
				   OR c.LastName LIKE '%' + @LastName + '%'
			
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
    SELECT c.ClaimId
		 , c.PayorId
         , c.ClaimNumber
         , c.LastName
         , c.FirstName
         , c.Carrier
         , c.InjuryDate
    FROM   [dbo].[vwClaimInfo] c WITH (NOEXPAND)
           INNER JOIN ClaimsCTE cte ON cte.ClaimID = c.ClaimId
END

GO
