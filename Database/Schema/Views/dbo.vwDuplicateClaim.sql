SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwDuplicateClaim]
AS
WITH CTE
AS ( SELECT   [c].[ClaimNumber]
            , [c].[PersonCode]
     FROM     [dbo].[Claim] AS [c]
     GROUP BY [c].[ClaimNumber]
            , [c].[PersonCode]
     HAVING   COUNT(*) > 1
   )
SELECT [ci].[ClaimNumber]
     , [c].[PersonCode]
     , PatientName = ci.[FirstName] + ' ' + ci.[LastName]
     , [ci].[Carrier]
     , [ci].[InjuryDate]
     , [c].[PolicyNumber]
     , [c].[RelationCode]
     , TermDate = FORMAT(CONVERT(DATE, [c].[TermDate]), 'M/d/yyyy')
FROM   [CTE] AS ct
       INNER JOIN [dbo].[Claim] AS c ON [c].[ClaimNumber] = [ct].[ClaimNumber]
                                        AND [c].[PersonCode] = [ct].[PersonCode]
       INNER JOIN [dbo].[vwClaimInfo] AS [ci] ON [ci].[ClaimID] = [c].[ClaimID]
GO
