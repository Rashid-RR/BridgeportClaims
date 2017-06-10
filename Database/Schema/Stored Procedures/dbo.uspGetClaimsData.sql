SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/10/2017
	Description:	Returns Claims data for Blade 1
	Sample Execute:
					EXEC dbo.uspGetClaimsData NULL, 'Brandie'
*/
CREATE PROC [dbo].[uspGetClaimsData]
(
    @ClaimNumber VARCHAR(255) = NULL, @FirstName VARCHAR(155) = NULL, 
    @LastName VARCHAR(155) = NULL, @RxNumber VARCHAR(100) = NULL, @InvoiceNumber NVARCHAR(100) = NULL
)
AS
BEGIN
    DECLARE @Format CHAR(10) = 'M/d/yyyy'
    SET NOCOUNT ON;

    WITH ClaimsCTE AS
    (
          SELECT c.ClaimID FROM dbo.Claim c WHERE c.ClaimNumber = @ClaimNumber UNION
          SELECT p.ClaimID FROM dbo.Patient p WHERE p.FirstName = @FirstName OR p.LastName = @LastName UNION
          SELECT i.ClaimID FROM dbo.Invoice i WHERE i.InvoiceNumber = @InvoiceNumber UNION
          SELECT p.ClaimID FROM dbo.Prescription p WHERE p.RxNumber = @RxNumber
    )
    SELECT ClaimId = c.ClaimID
         , [Name] = NULLIF(ISNULL(LTRIM(RTRIM(p.FirstName)), '') + ' ' + ISNULL(LTRIM(RTRIM(p.LastName)), ''), ' ')
         , c.ClaimNumber
         , DateOfBirth = FORMAT(p.DateOfBirth, @Format)
         , InjuryDate = FORMAT(c.DateOfInjury, @Format)
         , Gender = g.GenderName
         , Carrier = pa.BillToName -- ?
         , Adjustor = a.AdjustorName
         , AdjustorPhoneNumber = a.PhoneNumber
         , DateEntered = FORMAT(c.TermDate, @Format) -- ?
         , AdjustorFaxNumber = a.FaxNumber
    FROM   dbo.Claim c 
           INNER JOIN ClaimsCTE cte ON cte.ClaimID = c.ClaimID
           LEFT JOIN dbo.Patient p INNER JOIN dbo.Gender g ON g.GenderID = p.GenderID ON p.ClaimID = c.ClaimID
           LEFT JOIN dbo.Payor pa ON pa.PayorID = c.PayorID
           LEFT JOIN dbo.Adjustor a ON a.AdjustorID = c.AdjusterID
           LEFT JOIN dbo.Invoice i ON i.ClaimID = c.ClaimID
           LEFT JOIN dbo.Prescription pre ON pre.ClaimID = c.ClaimID
    WHERE  1 = 1
           AND (c.ClaimNumber = @ClaimNumber OR @ClaimNumber IS NULL)
           AND (p.FirstName = @FirstName OR @FirstName IS NULL)
           AND (p.LastName = @LastName OR @LastName IS NULL)
           AND (i.InvoiceNumber = @InvoiceNumber OR @InvoiceNumber IS NULL)
           AND (pre.RxNumber = @RxNumber OR @RxNumber IS NULL);
END
GO
