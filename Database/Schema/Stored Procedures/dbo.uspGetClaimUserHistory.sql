SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Date:			12/18/2017
	Description:	Proc to replace NHibernate query to get the user history results
					for recently viewed Claims.
	Example Execute:
					DECLARE @UserID NVARCHAR(128)
					SELECT TOP 1 @UserID = ID FROM dbo.AspNetUsers ORDER BY NEWID()
					EXEC dbo.uspGetClaimUserHistory @UserID
*/
CREATE PROCEDURE [dbo].[uspGetClaimUserHistory] @UserID NVARCHAR(128)
AS BEGIN
	SELECT          ClaimId = v.[ClaimID]
				  , v.[ClaimNumber]
				  , v.[FirstName] + ' ' + v.[LastName] [Name]
				  , v.[InjuryDate]
				  , v.[Carrier]
				  , CreatedOnUtc = c.[CreatedOnUTC]
	FROM            dbo.[vwClaimInfo]         AS v WITH (NOEXPAND)
		INNER JOIN  [dbo].[ClaimsUserHistory] AS c ON [c].[ClaimID] = [v].[ClaimID]
	WHERE	[c].[UserID] = @UserID
END
GO
