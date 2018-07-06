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
                    DECLARE @TotalRows INT;
                    EXECUTE [rpt].[uspGetDuplicateClaims] NULL, NULL, 1, 5000, @TotalRows = @TotalRows OUTPUT
                    SELECT @TotalRows
 =============================================
*/
CREATE PROC [rpt].[uspGetDuplicateClaims]
(
    @SortColumn VARCHAR(50),
    @SortDirection VARCHAR(5),
    @PageNumber INTEGER,
    @PageSize INTEGER,
    @TotalRows INTEGER OUTPUT
)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
        DECLARE @iSortColumn VARCHAR(50) = @SortColumn
          , @iSortDirection VARCHAR(5) = @SortDirection
          , @iPageNumber INTEGER = @PageNumber
          , @iPageSize INTEGER = @PageSize
          , @iIsDefaultSort BIT = 0;

        IF @iSortColumn IS NULL AND @iSortDirection IS NULL
            BEGIN
                SET @iIsDefaultSort = 1;
            END;

        CREATE TABLE #Duplicates
        (
            [LastName] [varchar] (155) NOT NULL,
            [FirstName] [varchar] (155) NOT NULL,
            [ClaimID] [int] NOT NULL,
            [DateOfBirth] [date] NULL,
            [ClaimNumber] [varchar] (255) NOT NULL,
            [PersonCode] [char] (2) NULL,
            [GroupName] [varchar] (255) NOT NULL
        );

        WITH [CTE] AS
        (
            SELECT      [S1] = SOUNDEX([p].[LastName])
                       ,[S2] = SOUNDEX([p].[FirstName])
                       ,[Cnt] = COUNT(*)
            FROM        [dbo].[Claim] AS [c]
                        INNER JOIN [dbo].[Patient] AS [p] ON [p].[PatientID] = [c].[PatientID]
                        LEFT JOIN [dbo].[DuplicateClaim] AS [dc] ON [dc].[DuplicateClaimID] = [c].[ClaimID]
            WHERE       [dc].[DuplicateClaimID] IS NULL
            GROUP BY    SOUNDEX([p].[LastName])
                       ,SOUNDEX([p].[FirstName])
            HAVING      COUNT(*) > 1
        )
        INSERT [#Duplicates] ([LastName], [FirstName], [ClaimID], [DateOfBirth], [ClaimNumber], [PersonCode], [GroupName])
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
                                                 AND   [cte].[S1] = SOUNDEX([p].[LastName]);

        SELECT @TotalRows = COUNT(*) FROM [#Duplicates] AS [d]

        SELECT      [d].[LastName]
                   ,[d].[FirstName]
                   ,[d].[ClaimID]
                   ,[d].[DateOfBirth]
                   ,[d].[ClaimNumber]
                   ,[d].[PersonCode]
                   ,[d].[GroupName]
        FROM        [#Duplicates] AS [d]
        ORDER BY    CASE WHEN @iIsDefaultSort = 1 THEN [d].[LastName] END ASC,
                    CASE WHEN @iIsDefaultSort = 1 THEN [d].[FirstName] END ASC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'LastName' AND @iSortDirection = 'ASC' THEN [d].[LastName] END ASC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'LastName' AND @iSortDirection = 'DESC' THEN [d].[LastName] END DESC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'FirstName' AND @iSortDirection = 'ASC' THEN [d].[FirstName] END ASC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'FirstName' AND @iSortDirection = 'DESC' THEN [d].[FirstName] END DESC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'ClaimID' AND @iSortDirection = 'ASC' THEN [d].[ClaimID] END ASC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'ClaimID' AND @iSortDirection = 'DESC' THEN [d].[ClaimID] END DESC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'DateOfBirth' AND @iSortDirection = 'ASC' THEN [d].[DateOfBirth] END ASC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'DateOfBirth' AND @iSortDirection = 'DESC' THEN [d].[DateOfBirth] END DESC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'ClaimNumber' AND @iSortDirection = 'ASC' THEN [d].[ClaimNumber] END ASC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'ClaimNumber' AND @iSortDirection = 'DESC' THEN [d].[ClaimNumber] END DESC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'PersonCode' AND @iSortDirection = 'ASC' THEN [d].[PersonCode] END ASC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'PersonCode' AND @iSortDirection = 'DESC' THEN [d].[PersonCode] END DESC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'GroupName' AND @iSortDirection = 'ASC' THEN [d].[GroupName] END ASC,
                    CASE WHEN @iIsDefaultSort = 0 AND @iSortColumn = 'GroupName' AND @iSortDirection = 'DESC' THEN [d].[GroupName] END DESC
        OFFSET @iPageSize * (@iPageNumber - 1) ROWS
        FETCH NEXT @iPageSize ROWS ONLY;
    END

GO
GRANT EXECUTE ON  [rpt].[uspGetDuplicateClaims] TO [acondie]
GO
GRANT VIEW DEFINITION ON  [rpt].[uspGetDuplicateClaims] TO [acondie]
GO
