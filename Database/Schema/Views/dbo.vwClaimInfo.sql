SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwClaimInfo]
WITH SCHEMABINDING
AS
SELECT [c].[ClaimID]
	 , [c].[ClaimNumber]
     , [Name] = NULLIF(ISNULL(LTRIM(RTRIM([p].[FirstName])), '') + ' ' + ISNULL(LTRIM(RTRIM([p].[LastName])), ''), ' ')
     , Carrier = [pa].[BillToName]
     , InjuryDate = [c].[DateOfInjury]
FROM   [dbo].[Claim] AS [c]
       INNER JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
       INNER JOIN [dbo].[Payor] AS pa ON [pa].[PayorID] = [c].[PayorID]
GO
CREATE UNIQUE NONCLUSTERED INDEX [idxUqVwClaimInfoClaimIDIncludeAll] ON [dbo].[vwClaimInfo] ([ClaimID]) INCLUDE ([Carrier], [ClaimNumber], [InjuryDate], [Name]) WITH (FILLFACTOR=90, DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO

CREATE UNIQUE CLUSTERED INDEX [pkVwClaimInfoClaimID] ON [dbo].[vwClaimInfo] ([ClaimID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]

GO
