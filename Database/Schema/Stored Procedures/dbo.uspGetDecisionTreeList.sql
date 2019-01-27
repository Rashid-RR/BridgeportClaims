SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       1/18/2019
 Description:       Gets all of the Root Trees
 Example Execute:
					DECLARE @I INT;
                    EXECUTE [dbo].[uspGetDecisionTreeList] NULL, 'TreeId', 'ASC', 1, 5000, @TotalRows = @I OUTPUT;
					SELECT @I TotalRows;
 =============================================
*/
CREATE PROC [dbo].[uspGetDecisionTreeList]
(
	@SearchText VARCHAR(4000),
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER,
	@TotalRows INTEGER OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	DECLARE @WildCard CHAR(1) = '%';

	CREATE TABLE [#TreeRoots](
		[TreeId] [int] NOT NULL PRIMARY KEY,
		[NodeName] [varchar](255) NOT NULL,
		[NodeDescription] [varchar](4000) NULL,
		[CreatedBy] [nvarchar](201) NOT NULL,
		[CreatedOn] [date] NULL
	);

	INSERT [#TreeRoots] ([TreeId], [NodeName], [NodeDescription], [CreatedBy], [CreatedOn])
    SELECT [dt].[TreeID] [TreeId]
          ,[dt].[NodeName]
          ,[dt].[NodeDescription]
          ,[x].[FirstName] + ' ' + [x].[LastName] [CreatedBy]
          ,CAST([dtme].[udfGetLocalDateTime]([dt].[CreatedOnUTC]) AS DATE) [CreatedOn]
     FROM [dbo].[DecisionTree] AS [dt]
		  INNER JOIN [dbo].[AspNetUsers] AS [x] ON [dt].[ModifiedByUserID] = [x].[ID]
	WHERE [dt].[TreeLevel] = 1
		  AND (ISNULL(@SearchText, '') = '' OR [dt].[NodeName] LIKE CONCAT(@WildCard, @SearchText, @WildCard));

	SELECT @TotalRows = COUNT(*) FROM [#TreeRoots] AS [tr]

	SELECT  [a].[TreeId], [a].[NodeName], [a].[NodeDescription], [a].[CreatedBy], [a].[CreatedOn]
	FROM    [#TreeRoots] AS [a]
	ORDER BY CASE WHEN @SortColumn = 'TreeId' AND @SortDirection = 'ASC' THEN [a].[TreeId] END ASC
			,CASE WHEN @SortColumn = 'TreeId' AND @SortDirection = 'DESC' THEN [a].[TreeId] END DESC
			,CASE WHEN @SortColumn = 'NodeName' AND @SortDirection = 'ASC' THEN [a].[NodeName] END ASC
			,CASE WHEN @SortColumn = 'NodeName' AND @SortDirection = 'DESC' THEN [a].[NodeName] END DESC
			,CASE WHEN @SortColumn = 'NodeDescription' AND @SortDirection = 'ASC' THEN [a].[NodeDescription] END ASC
			,CASE WHEN @SortColumn = 'NodeDescription' AND @SortDirection = 'DESC' THEN [a].[NodeDescription] END DESC
			,CASE WHEN @SortColumn = 'CreatedBy' AND @SortDirection = 'ASC' THEN [a].[CreatedBy] END ASC
			,CASE WHEN @SortColumn = 'CreatedBy' AND @SortDirection = 'DESC' THEN [a].[CreatedBy] END DESC
			,CASE WHEN @SortColumn = 'CreatedOn' AND @SortDirection = 'ASC' THEN [a].[CreatedOn] END ASC
			,CASE WHEN @SortColumn = 'CreatedOn' AND @SortDirection = 'DESC' THEN [a].[CreatedOn] END DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO
