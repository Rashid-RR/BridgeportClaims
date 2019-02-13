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
AS
    BEGIN
        SELECT  DISTINCT
                [ClaimId] = [v].[ClaimId]
               ,[v].[ClaimNumber]
               ,[Name] = [v].[FirstName] + ' ' + [v].[LastName]
               ,[v].[InjuryDate]
               ,[v].[Carrier]
               ,[CreatedOnUtc] = MAX([c].[CreatedOnUTC])
        FROM    [dbo].[vwClaimInfo] AS [v] WITH (NOEXPAND)
                INNER JOIN [dbo].[ClaimsUserHistory] AS [c] ON [c].[ClaimID] = [v].[ClaimId]
        WHERE   [c].[UserID] = @UserID
        GROUP BY [v].[ClaimId], [v].[ClaimNumber], [v].[FirstName], [v].[LastName], [v].[InjuryDate], [v].[Carrier]
        ORDER BY MAX([c].[CreatedOnUTC]) DESC;
    END
GO
