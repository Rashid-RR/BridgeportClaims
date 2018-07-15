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
CREATE PROC [claims].[uspGetClaimsSearchResults]
(
    @ClaimNumber VARCHAR(255) = NULL, @FirstName VARCHAR(155) = NULL, 
    @LastName VARCHAR(155) = NULL, @RxNumber VARCHAR(100) = NULL, @InvoiceNumber NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
	SET XACT_ABORT ON;
	IF (@RxNumber IS NOT NULL OR @InvoiceNumber IS NOT NULL)
		BEGIN
			CREATE TABLE #Prescriptions
			(
				ClaimID INT NOT NULL PRIMARY KEY
			);
			INSERT INTO #Prescriptions ([ClaimID])
			SELECT	p.[ClaimID] 
			FROM	[dbo].[Prescription] AS [p]
					LEFT JOIN [dbo].[Invoice] AS [i] ON [i].[InvoiceID] = [p].[InvoiceID]
			WHERE	(@InvoiceNumber = i.[InvoiceNumber] OR @InvoiceNumber IS NULL)
					AND ([p].[RxNumber] = @RxNumber OR @RxNumber IS NULL)
			GROUP BY p.[ClaimID]

			SELECT c.ClaimId
				 , c.PayorId
				 , c.ClaimNumber
				 , c.LastName
				 , c.FirstName
				 , c.Carrier
				 , c.InjuryDate
			FROM   [dbo].[vwClaimInfo] c WITH (NOEXPAND)
				   INNER JOIN [#Prescriptions] AS [p] ON [p].[ClaimID] = [c].[ClaimID]
			WHERE  1 = 1
				   AND (c.ClaimNumber LIKE '%' + @ClaimNumber + '%' OR @ClaimNumber IS NULL)
				   AND (c.FirstName LIKE '%' + @FirstName + '%' OR @FirstName IS NULL)
				   AND (c.LastName LIKE '%' + @LastName + '%' OR @LastName IS NULL)
		END
	ELSE
		BEGIN
			SELECT c.ClaimId
				 , c.PayorId
				 , c.ClaimNumber
				 , c.LastName
				 , c.FirstName
				 , c.Carrier
				 , c.InjuryDate
			FROM   [dbo].[vwClaimInfo] c WITH (NOEXPAND)
			WHERE  1 = 1
				   AND (c.ClaimNumber LIKE '%' + @ClaimNumber + '%' OR @ClaimNumber IS NULL)
				   AND (c.FirstName LIKE '%' + @FirstName + '%' OR @FirstName IS NULL)
				   AND (c.LastName LIKE '%' + @LastName + '%' OR @LastName IS NULL)
		END
END
GO
