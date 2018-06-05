SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vwClaimInfo]
WITH SCHEMABINDING
AS
SELECT [c].[ClaimID] ClaimId
	 , [c].[ClaimNumber]
	 , [pa].[PayorID] PayorId
	 , [p].[FirstName]
	 , [p].[LastName]
     , Carrier = [pa].[GroupName]
     , InjuryDate = [c].[DateOfInjury]
FROM   [dbo].[Claim] AS [c]
       INNER JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
       INNER JOIN [dbo].[Payor] AS pa ON [pa].[PayorID] = [c].[PayorID]
GO

CREATE UNIQUE CLUSTERED INDEX [pkVwClaimInfoClaimID] ON [dbo].[vwClaimInfo] ([ClaimId]) WITH (DATA_COMPRESSION = ROW) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX [idxVwClaimInfoClaimIdLastNameFirstNameClaimNumberIncludes] ON [dbo].[vwClaimInfo] ([ClaimId], [LastName], [FirstName], [ClaimNumber]) INCLUDE ([Carrier], [InjuryDate], [PayorId]) WITH (DATA_COMPRESSION = PAGE) ON [PRIMARY]
GO
