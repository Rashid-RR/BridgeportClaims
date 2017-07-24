SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[vwClaimInfo]
AS
SELECT ROW_NUMBER() OVER (ORDER BY [c].[ClaimNumber]) RowID
	 , [c].[ClaimID]
	 , [c].[ClaimNumber]
     , [Name] = NULLIF(ISNULL(LTRIM(RTRIM([p].[FirstName])), '') + ' ' + ISNULL(LTRIM(RTRIM([p].[LastName])), ''), ' ')
     , Carrier = [pa].[BillToName]
     , InjuryDate = FORMAT([c].[DateOfInjury], 'M/d/yyyy')
FROM   [dbo].[Claim] AS [c]
       LEFT JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
       LEFT JOIN [dbo].[Payor] AS pa ON [pa].[PayorID] = [c].[PayorID]
GO
