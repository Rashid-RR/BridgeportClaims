SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       11/29/2018
 Description:       Get the Adjustors for the new References page.
 Example Execute:
					DECLARE @I INT;
                    EXECUTE [dbo].[uspGetReferencesAdjustors] NULL, 'AdjustorName', 'ASC', 1, 5000, @TotalRows = @I OUTPUT;
					SELECT @I TotalRows;
 =============================================
*/
CREATE PROC [dbo].[uspGetReferencesAdjustors]
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

	CREATE TABLE [#Adjustors] (
		[AdjustorId] [int] NOT NULL PRIMARY KEY,
		[AdjustorName] [varchar](255) NOT NULL,
		[Address1] [varchar](255) NULL,
		[Address2] [varchar](255) NULL,
		[City] [varchar](255) NULL,
		[StateName] [VARCHAR](64) NULL,
		[PostalCode] [varchar](255) NULL,
		[PhoneNumber] [varchar](30) NULL,
		[FaxNumber] [varchar](30) NULL,
		[EmailAddress] [varchar](155) NULL,
		[Extension] [varchar](10) NULL,
		[ModifiedBy] [nvarchar](201) NULL
	);

	INSERT INTO [#Adjustors] ([AdjustorId],[AdjustorName],[Address1],[Address2],[City],[StateName],[PostalCode],[PhoneNumber],[FaxNumber],[EmailAddress],[Extension],[ModifiedBy])
    SELECT  [AdjustorId] = [a].[AdjustorID]
		   ,[a].[AdjustorName]
		   ,[a].[Address1]
		   ,[a].[Address2]
		   ,[a].[City]
		   ,[us].[StateName]
		   ,[a].[PostalCode]
		   ,[a].[PhoneNumber]
		   ,[a].[FaxNumber]
		   ,[a].[EmailAddress]
		   ,[a].[Extension]
		   ,[ModifiedBy] = [x].[FirstName] + ' ' + [x].[LastName]
	FROM    [dbo].[Adjustor] AS [a]
			LEFT JOIN [dbo].[AspNetUsers] AS [x] ON [a].[ModifiedByUserID] = [x].[ID]
			LEFT JOIN [dbo].[UsState] AS [us] ON [a].[StateID] = [us].[StateID]
	WHERE   [a].[AdjustorName] LIKE CONCAT(@WildCard, @SearchText, @WildCard);

	SELECT @TotalRows = COUNT(*) FROM [#Adjustors] AS [a]

	SELECT  [a].[AdjustorId]
           ,[a].[AdjustorName]
           ,[a].[Address1]
           ,[a].[Address2]
           ,[a].[City]
           ,[a].[StateName]
           ,[a].[PostalCode]
           ,[a].[PhoneNumber]
           ,[a].[FaxNumber]
           ,[a].[EmailAddress]
           ,[a].[Extension]
           ,[a].[ModifiedBy]
	FROM    [#Adjustors] AS [a]
	ORDER BY CASE WHEN @SortColumn = 'AdjustorId' AND @SortDirection = 'ASC' THEN [a].[AdjustorId] END ASC,
			 CASE WHEN @SortColumn = 'AdjustorId' AND  @SortDirection = 'DESC' THEN [a].[AdjustorId] END DESC,
			 CASE WHEN @SortColumn = 'AdjustorName' AND @SortDirection = 'ASC' THEN [a].[AdjustorName] END ASC,
			 CASE WHEN @SortColumn = 'AdjustorName' AND @SortDirection = 'DESC' THEN [a].[AdjustorName] END DESC,
			 CASE WHEN @SortColumn = 'Address1' AND @SortDirection = 'ASC' THEN [a].[AdjustorName] END ASC,
			 CASE WHEN @SortColumn = 'Address1' AND @SortDirection = 'DESC' THEN [a].[AdjustorName] END DESC,
			 CASE WHEN @SortColumn = 'Address2' AND @SortDirection = 'ASC' THEN [a].[AdjustorName] END ASC,
			 CASE WHEN @SortColumn = 'Address2' AND @SortDirection = 'DESC' THEN [a].[AdjustorName] END DESC,
			 CASE WHEN @SortColumn = 'City' AND @SortDirection = 'ASC' THEN [a].[AdjustorName] END ASC,
			 CASE WHEN @SortColumn = 'City' AND @SortDirection = 'DESC' THEN [a].[AdjustorName] END DESC,
			 CASE WHEN @SortColumn = 'StateName' AND @SortDirection = 'ASC' THEN [a].[AdjustorName] END ASC,
			 CASE WHEN @SortColumn = 'StateName' AND @SortDirection = 'DESC' THEN [a].[AdjustorName] END DESC,
			 CASE WHEN @SortColumn = 'PostalCode' AND @SortDirection = 'ASC' THEN [a].[AdjustorName] END ASC,
			 CASE WHEN @SortColumn = 'PostalCode' AND @SortDirection = 'DESC' THEN [a].[AdjustorName] END DESC,
			 CASE WHEN @SortColumn = 'PhoneNumber' AND @SortDirection = 'ASC' THEN [a].[PhoneNumber] END ASC,
			 CASE WHEN @SortColumn = 'PhoneNumber' AND  @SortDirection = 'DESC' THEN [a].[PhoneNumber] END DESC,
			 CASE WHEN @SortColumn = 'FaxNumber' AND @SortDirection = 'ASC' THEN [a].[FaxNumber] END ASC,
			 CASE WHEN @SortColumn = 'FaxNumber' AND  @SortDirection = 'DESC' THEN [a].[FaxNumber] END DESC,
			 CASE WHEN @SortColumn = 'EmailAddress' AND @SortDirection = 'ASC' THEN [a].[EmailAddress] END ASC,
			 CASE WHEN @SortColumn = 'EmailAddress' AND  @SortDirection = 'DESC' THEN [a].[EmailAddress] END DESC,
			 CASE WHEN @SortColumn = 'Extension' AND @SortDirection = 'ASC' THEN [a].[Extension] END ASC,
			 CASE WHEN @SortColumn = 'Extension' AND  @SortDirection = 'DESC' THEN [a].[Extension] END DESC,
			 CASE WHEN @SortColumn = 'ModifiedBy' AND @SortDirection = 'ASC' THEN [a].[ModifiedBy] END ASC,
			 CASE WHEN @SortColumn = 'ModifiedBy' AND  @SortDirection = 'DESC' THEN [a].[ModifiedBy] END DESC
	 OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
END


GO
