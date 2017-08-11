SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/8/2017
	Description:	Returns unique, Claims, and count of their Prescriptions
	Sample Execute:
					EXEC [dbo].[uspGetClaimsWithPrescriptionCounts]
												@ClaimNumber = NULL,
												@FirstName = NULL,
												@LastName = 'WILLIAMS',
												@RxDate = NULL,
												@InvoiceNumber = NULL
*/
CREATE PROC [dbo].[uspGetClaimsWithPrescriptionCounts]
(
	@ClaimNumber VARCHAR(255),
	@FirstName VARCHAR(155),
	@LastName VARCHAR(155),
	@RxDate DATETIME2,
	@InvoiceNumber VARCHAR(100)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	SELECT    [c].[ClaimID] ClaimId
			, [c].[ClaimNumber]
			, PatientName = [p].[FirstName] + ' ' + [p].[LastName]
			, Payor = [py].[GroupName]
			, NumberOfPrescriptions = COUNT(*)
	FROM   [dbo].[Claim] AS [c]
			INNER JOIN [dbo].[Payor] AS [py] ON [py].[PayorID] = [c].[PayorID]
			INNER JOIN [dbo].[Patient] AS p ON [p].[PatientID] = [c].[PatientID]
			INNER JOIN [dbo].[Prescription] AS [pr] ON [pr].[ClaimID] = [c].[ClaimID]
			LEFT JOIN [dbo].[Invoice] AS [i] ON [i].[InvoiceID] = [pr].[InvoiceID]
	WHERE  (   [c].[ClaimNumber] LIKE + '%' + @ClaimNumber + '%'
				OR @ClaimNumber IS NULL
			)
			AND (   [p].[FirstName] LIKE '%' + @FirstName + '%'
					OR @FirstName IS NULL
				)
			AND (   [p].[LastName] LIKE '%' + @LastName + '%'
					OR @LastName IS NULL
				)
			AND (   [pr].[DateFilled] = @RxDate
					OR @RxDate IS NULL
				)
			AND (   [i].[InvoiceNumber] = @InvoiceNumber
					OR @InvoiceNumber IS NULL
				)
	GROUP BY [c].[ClaimID], [c].[ClaimNumber],[p].[FirstName],[p].[LastName],[py].[GroupName]
END

GO
