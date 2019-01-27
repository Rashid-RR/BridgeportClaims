SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       1/22/2019
 Description:       Gets the list of Payors for the References page.
 Example Execute:
					DECLARE @I INT
                    EXECUTE [dbo].[uspGetReferencesPayors] 'AAA', 'Notes', 'DESC', 1, 5000, @TotalRows = @I OUTPUT;
					SELECT @I TotalRows;
 =============================================
*/
CREATE PROC [dbo].[uspGetReferencesPayors]
(
    @SearchText VARCHAR(4000)
   ,@SortColumn VARCHAR(50)
   ,@SortDirection VARCHAR(5)
   ,@PageNumber INTEGER
   ,@PageSize INTEGER
   ,@TotalRows INTEGER OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @WildCard CHAR(1) = '%'

	CREATE TABLE [#Payors](
		[PayorId] [int] NOT NULL,
		[GroupName] [varchar](255) NOT NULL,
		[BillToName] [varchar](255) NOT NULL,
		[BillToAddress1] [varchar](255) NULL,
		[BillToAddress2] [varchar](255) NULL,
		[BillToCity] [varchar](155) NULL,
		[BillToStateName] [varchar](64) NULL,
		[BillToPostalCode] [varchar](100) NULL,
		[PhoneNumber] [varchar](30) NULL,
		[AlternatePhoneNumber] [varchar](30) NULL,
		[FaxNumber] [varchar](30) NULL,
		[Notes] [varchar](8000) NULL,
		[Contact] [varchar](255) NULL,
		[LetterName] [varchar](255) NOT NULL,
		[ModifiedBy] [nvarchar](201) NULL
	);

	INSERT INTO [#Payors] ([PayorId],[GroupName],[BillToName],[BillToAddress1],[BillToAddress2],[BillToCity],[BillToStateName],[BillToPostalCode],[PhoneNumber],
		[AlternatePhoneNumber],[FaxNumber],[Notes],[Contact],[LetterName],[ModifiedBy])
    SELECT  [PayorId] = [p].[PayorID]
            ,[p].[GroupName]
			,[p].[BillToName]
            ,[p].[BillToAddress1]
			,[p].[BillToAddress2]
			,[p].[BillToCity]
			,[us].[StateName]
			,[p].[BillToPostalCode]
            ,[p].[PhoneNumber]
            ,[p].[AlternatePhoneNumber]
            ,[p].[FaxNumber]
            ,[p].[Notes]
            ,[p].[Contact]
            ,[p].[LetterName]
            ,[ModifiedBy] = [x].[FirstName] + ' ' + [x].[LastName]
    FROM    [dbo].[Payor] AS [p]
            LEFT JOIN [dbo].[AspNetUsers] AS [x] ON [p].[ModifiedByUserID] = [x].[ID]
			LEFT JOIN [dbo].[UsState] AS [us] ON [p].[BillToStateID] = [us].[StateID]
	WHERE	[p].[GroupName] LIKE CONCAT(@WildCard, @SearchText, @WildCard);

	SELECT @TotalRows = COUNT(*) FROM [#Payors] AS [p];

	SELECT [p].[PayorId]
            ,[p].[GroupName]
            ,[p].[BillToName]
            ,[p].[BillToAddress1]
            ,[p].[BillToAddress2]
            ,[p].[BillToCity]
            ,[p].[BillToStateName]
            ,[p].[BillToPostalCode]
            ,[p].[PhoneNumber]
            ,[p].[AlternatePhoneNumber]
            ,[p].[FaxNumber]
            ,[p].[Notes]
            ,[p].[Contact]
            ,[p].[LetterName]
            ,[p].[ModifiedBy]
	FROM [#Payors] AS [p]
	ORDER BY CASE WHEN @SortColumn = 'PayorId' AND @SortDirection = 'ASC' THEN [p].[PayorId] END ASC
		,CASE WHEN @SortColumn = 'PayorId' AND @SortDirection = 'DESC' THEN [p].[PayorId] END DESC
		,CASE WHEN @SortColumn = 'GroupName' AND @SortDirection = 'ASC' THEN [p].[GroupName] END ASC
		,CASE WHEN @SortColumn = 'GroupName' AND @SortDirection = 'DESC' THEN [p].[GroupName] END DESC
		,CASE WHEN @SortColumn = 'BillToName' AND @SortDirection = 'ASC' THEN [p].[BillToName] END ASC
		,CASE WHEN @SortColumn = 'BillToName' AND @SortDirection = 'DESC' THEN [p].[BillToName] END DESC
		,CASE WHEN @SortColumn = 'BillToAddress1' AND @SortDirection = 'ASC' THEN [p].[BillToAddress1] END ASC
		,CASE WHEN @SortColumn = 'BillToAddress1' AND @SortDirection = 'DESC' THEN [p].[BillToAddress1] END DESC
		,CASE WHEN @SortColumn = 'BillToAddress2' AND @SortDirection = 'ASC' THEN [p].[BillToAddress2] END ASC
		,CASE WHEN @SortColumn = 'BillToAddress2' AND @SortDirection = 'DESC' THEN [p].[BillToAddress2] END DESC
		,CASE WHEN @SortColumn = 'BillToCity' AND @SortDirection = 'ASC' THEN [p].[BillToCity] END ASC
		,CASE WHEN @SortColumn = 'BillToCity' AND @SortDirection = 'DESC' THEN [p].[BillToCity] END DESC
		,CASE WHEN @SortColumn = 'BillToStateName' AND @SortDirection = 'ASC' THEN [p].[BillToStateName] END ASC
		,CASE WHEN @SortColumn = 'BillToStateName' AND @SortDirection = 'DESC' THEN [p].[BillToStateName] END DESC
		,CASE WHEN @SortColumn = 'BillToPostalCode' AND @SortDirection = 'ASC' THEN [p].[BillToPostalCode] END ASC
		,CASE WHEN @SortColumn = 'BillToPostalCode' AND @SortDirection = 'DESC' THEN [p].[BillToPostalCode] END DESC
		,CASE WHEN @SortColumn = 'PhoneNumber' AND @SortDirection = 'ASC' THEN [p].[PhoneNumber] END ASC
		,CASE WHEN @SortColumn = 'PhoneNumber' AND @SortDirection = 'DESC' THEN [p].[PhoneNumber] END DESC
		,CASE WHEN @SortColumn = 'AlternatePhoneNumber' AND @SortDirection = 'ASC' THEN [p].[AlternatePhoneNumber] END ASC
		,CASE WHEN @SortColumn = 'AlternatePhoneNumber' AND @SortDirection = 'DESC' THEN [p].[AlternatePhoneNumber] END DESC
		,CASE WHEN @SortColumn = 'FaxNumber' AND @SortDirection = 'ASC' THEN [p].[FaxNumber] END ASC
		,CASE WHEN @SortColumn = 'FaxNumber' AND @SortDirection = 'DESC' THEN [p].[FaxNumber] END DESC
		,CASE WHEN @SortColumn = 'Notes' AND @SortDirection = 'ASC' THEN [p].[Notes] END ASC
		,CASE WHEN @SortColumn = 'Notes' AND @SortDirection = 'DESC' THEN [p].[Notes] END DESC
		,CASE WHEN @SortColumn = 'Contact' AND @SortDirection = 'ASC' THEN [p].[Contact] END ASC
		,CASE WHEN @SortColumn = 'Contact' AND @SortDirection = 'DESC' THEN [p].[Contact] END DESC
		,CASE WHEN @SortColumn = 'LetterName' AND @SortDirection = 'ASC' THEN [p].[LetterName] END ASC
		,CASE WHEN @SortColumn = 'LetterName' AND @SortDirection = 'DESC' THEN [p].[LetterName] END DESC
		,CASE WHEN @SortColumn = 'ModifiedBy' AND @SortDirection = 'ASC' THEN [p].[ModifiedBy] END ASC
		,CASE WHEN @SortColumn = 'ModifiedBy' AND @SortDirection = 'DESC' THEN [p].[ModifiedBy] END DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO
