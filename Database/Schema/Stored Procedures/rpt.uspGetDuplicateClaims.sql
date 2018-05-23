SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       5/23/2018
 Description:       Finds duplicate claims based on the patient names, and SOUNDEX matches of those names.
 Example Execute:
                    EXECUTE [rpt].[uspGetDuplicateClaims]
 =============================================
*/
CREATE PROC [rpt].[uspGetDuplicateClaims]
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        WITH [CTE] AS
        (
            SELECT      [S1] = SOUNDEX([p].[LastName])
                       ,[S2] = SOUNDEX([p].[FirstName])
                       ,[Cnt] = COUNT(*)
            FROM        [dbo].[Claim] AS [c]
                        INNER JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
            GROUP BY    SOUNDEX([p].[LastName])
                       ,SOUNDEX([p].[FirstName])
            HAVING      COUNT(*) > 1
        )
        SELECT      [p].[LastName]
                   ,[p].[FirstName]
                   ,[c].[ClaimID]
                   ,[p].[DateOfBirth]
                   ,[c].[ClaimNumber]
                   ,[c].[PersonCode]
                   ,[pay].[GroupName]
        FROM        [dbo].[Claim] AS [c]
                    INNER JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
                    INNER JOIN [dbo].[Payor] AS [pay] ON [pay].[PayorID] = [c].[PayorID]
                    INNER JOIN [CTE] AS [cte] ON [cte].[S2] = SOUNDEX([p].[FirstName])
                                                 AND   [cte].[S1] = SOUNDEX([p].[LastName])
        ORDER BY    [cte].[S1]
                   ,[cte].[S2]
    END
GO
