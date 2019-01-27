SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/29/2018
 Description:       Get the Attorneys for the new References page.
 Example Execute:
					DECLARE @I INT;
                    EXECUTE [dbo].[uspGetReferencesAttorneys] NULL, 'AttorneyName', 'ASC', 1, 5000, @TotalRows = @I OUTPUT;
					SELECT @I TotalRows;
 =============================================
*/
CREATE PROC [dbo].[uspGetReferencesAttorneys]
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

	CREATE TABLE [#Attorney]
	(
		[AttorneyId] [int] NOT NULL PRIMARY KEY,
		[AttorneyName] [varchar] (255) NOT NULL,
		[Address1] [varchar] (255) NULL,
		[Address2] [varchar] (255) NULL,
		[City] [varchar] (255) NULL,
		[StateName] [varchar] (64) NULL,
		[PostalCode] [varchar] (255) NULL,
		[PhoneNumber] [varchar] (30) NULL,
		[FaxNumber] [varchar] (30) NULL,
		[ModifiedBy] [nvarchar] (202) NOT NULL
	);


	INSERT INTO [#Attorney] ([AttorneyId],[AttorneyName],[Address1],[Address2],[City],[StateName],[PostalCode],[PhoneNumber],[FaxNumber],[ModifiedBy])
    SELECT  [AttorneyId] = [a].[AttorneyID]
		   ,[a].[AttorneyName]
		   ,[a].[Address1]
		   ,[a].[Address2]
		   ,[a].[City]
		   ,[us].[StateName]
		   ,[a].[PostalCode]
		   ,[a].[PhoneNumber]
		   ,[a].[FaxNumber]
		   ,[ModifiedBy] = [x].[FirstName] + ' ' + [x].[LastName]
	FROM    [dbo].[Attorney] AS [a]
			LEFT JOIN [dbo].[AspNetUsers] AS [x] ON [a].[ModifiedByUserID] = [x].[ID]
			LEFT JOIN [dbo].[UsState] AS [us] ON [a].[StateID] = [us].[StateID]
	WHERE   [a].[AttorneyName] LIKE CONCAT(@WildCard, @SearchText, @WildCard);

	SELECT @TotalRows = COUNT(*) FROM [#Attorney] AS [a]

	SELECT  [a].[AttorneyId]
           ,[a].[AttorneyName]
           ,[a].[Address1]
           ,[a].[Address2]
           ,[a].[City]
           ,[a].[StateName]
           ,[a].[PostalCode]
           ,[a].[PhoneNumber]
           ,[a].[FaxNumber]
           ,[a].[ModifiedBy]
	FROM    [#Attorney] AS [a]
	ORDER BY CASE WHEN @SortColumn = 'AttorneyId' AND @SortDirection = 'ASC' THEN [a].[AttorneyId] END ASC,
			 CASE WHEN @SortColumn = 'AttorneyId' AND  @SortDirection = 'DESC' THEN [a].[AttorneyId] END DESC,
			 CASE WHEN @SortColumn = 'AttorneyName' AND @SortDirection = 'ASC' THEN [a].[AttorneyName] END ASC,
			 CASE WHEN @SortColumn = 'AttorneyName' AND @SortDirection = 'DESC' THEN [a].[AttorneyName] END DESC,
			 CASE WHEN @SortColumn = 'Address1' AND @SortDirection = 'ASC' THEN [a].[Address1] END ASC,
			 CASE WHEN @SortColumn = 'Address1' AND  @SortDirection = 'DESC' THEN [a].[Address1] END DESC,
			 CASE WHEN @SortColumn = 'Address2' AND @SortDirection = 'ASC' THEN [a].[Address2] END ASC,
			 CASE WHEN @SortColumn = 'Address2' AND  @SortDirection = 'DESC' THEN [a].[Address2] END DESC,
			 CASE WHEN @SortColumn = 'City' AND @SortDirection = 'ASC' THEN [a].[City] END ASC,
			 CASE WHEN @SortColumn = 'City' AND  @SortDirection = 'DESC' THEN [a].[City] END DESC,
			 CASE WHEN @SortColumn = 'StateName' AND @SortDirection = 'ASC' THEN [a].[StateName] END ASC,
			 CASE WHEN @SortColumn = 'StateName' AND  @SortDirection = 'DESC' THEN [a].[StateName] END DESC,
			 CASE WHEN @SortColumn = 'PostalCode' AND @SortDirection = 'ASC' THEN [a].[PostalCode] END ASC,
			 CASE WHEN @SortColumn = 'PostalCode' AND  @SortDirection = 'DESC' THEN [a].[PostalCode] END DESC,
			 CASE WHEN @SortColumn = 'PhoneNumber' AND @SortDirection = 'ASC' THEN [a].[PhoneNumber] END ASC,
			 CASE WHEN @SortColumn = 'PhoneNumber' AND  @SortDirection = 'DESC' THEN [a].[PhoneNumber] END DESC,
			 CASE WHEN @SortColumn = 'FaxNumber' AND @SortDirection = 'ASC' THEN [a].[FaxNumber] END ASC,
			 CASE WHEN @SortColumn = 'FaxNumber' AND  @SortDirection = 'DESC' THEN [a].[FaxNumber] END DESC,
			 CASE WHEN @SortColumn = 'ModifiedBy' AND @SortDirection = 'ASC' THEN [a].[ModifiedBy] END ASC,
			 CASE WHEN @SortColumn = 'ModifiedBy' AND  @SortDirection = 'DESC' THEN [a].[ModifiedBy] END DESC
	 OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
END



GO
