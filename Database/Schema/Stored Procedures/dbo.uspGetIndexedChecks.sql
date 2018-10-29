SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       10/27/2018
 Description:       Gets all Indexed Checks 
 Example Execute:
                    DECLARE @TotalRows INT;
					EXEC [dbo].[uspGetIndexedChecks] NULL, NULL, 'IndexedOn', 'DESC', 1, 30, @TotalRows OUTPUT;
					SELECT @TotalRows TotalRows;
 =============================================
*/
CREATE PROC [dbo].[uspGetIndexedChecks]
(
	@Date DATE
   ,@FileName VARCHAR(1000)
   ,@SortColumn VARCHAR(50)
   ,@SortDirection VARCHAR(5)
   ,@PageNumber INTEGER
   ,@PageSize INTEGER
   ,@TotalRows INTEGER OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    DECLARE @Check AS INT = [dbo].[udfGetFileTypeByCode]('CK');
	DECLARE @WildCard CHAR(1) = '%';

	CREATE TABLE #IndexedChecks (
		[DocumentId] [int] NOT NULL PRIMARY KEY,
		[FileName] [varchar](1000) NOT NULL,
		[CheckNumber] [varchar](100) NOT NULL,
		[IndexedBy] [nvarchar](201) NOT NULL,
		[FileUrl] [nvarchar](500) NOT NULL,
		[NumberOfPayments] [bigint] NULL,
		[TotalAmountPaid] [money] NULL,
		[IndexedOn] [datetime2](7) NOT NULL
	);

	INSERT [#IndexedChecks] ([DocumentID],[FileName],[CheckNumber],[IndexedBy],[FileUrl],[NumberOfPayments],[TotalAmountPaid],[IndexedOn])
	SELECT  [ci].[DocumentID]
			,[d].[FileName]
			,[ci].[CheckNumber]
			,[IndexedBy] = [u].[FirstName] + ' ' + [u].[LastName]
			,[d].[FileUrl]
			,[NumberOfPayments] =
			(
				SELECT  [Cnt] = COUNT_BIG(*)
				FROM    [dbo].[PrescriptionPayment] AS [pp]
				WHERE   [pp].[DocumentID] = [d].[DocumentID]
			)
			,[TotalAmountPaid] =
			(
				SELECT      [AP] = SUM([pp2].[AmountPaid])
				FROM    [dbo].[PrescriptionPayment] AS [pp2]
				WHERE [pp2].[DocumentID] = [d].[DocumentID]
			)
			,[IndexedOn] = [ci].[CreatedOnUTC]
	FROM    [dbo].[Document] AS [d]
			INNER JOIN [dbo].[CheckIndex] AS [ci] ON [d].[DocumentID] = [ci].[DocumentID]
			INNER JOIN [dbo].[AspNetUsers] AS [u] ON [ci].[ModifiedByUserID] = [u].[ID]
	WHERE   [d].[FileTypeID] = @Check
			AND [d].[IsValid] = 1
			AND (@Date IS NULL OR d.DocumentDate = @Date)
			AND ([d].[FileName] LIKE CONCAT(CONCAT(@WildCard, @FileName), @WildCard) OR @FileName IS NULL);

	SELECT  @TotalRows = COUNT(*) FROM [#IndexedChecks]

	SELECT [d].[DocumentId]
            ,[d].[FileName]
            ,[d].[CheckNumber]
            ,[d].[IndexedBy]
            ,[d].[FileUrl]
            ,[d].[NumberOfPayments]
            ,[d].[TotalAmountPaid]
            ,[d].[IndexedOn]
	FROM [#IndexedChecks] AS [d]
	ORDER BY CASE WHEN @SortColumn = 'DocumentID' AND @SortDirection = 'ASC'
				THEN d.DocumentID END ASC,
			CASE WHEN @SortColumn = 'DocumentID' AND @SortDirection = 'DESC'
				THEN d.DocumentID END DESC,
			CASE WHEN @SortColumn = 'FileName' AND @SortDirection = 'ASC'
				THEN d.[FileName] END ASC,
			CASE WHEN @SortColumn = 'FileName' AND @SortDirection = 'DESC'
				THEN d.[FileName] END DESC,
			CASE WHEN @SortColumn = 'CheckNumber' AND @SortDirection = 'ASC'
				THEN d.[CheckNumber] END ASC,
			CASE WHEN @SortColumn = 'CheckNumber' AND @SortDirection = 'DESC'
				THEN d.[CheckNumber] END DESC,
			CASE WHEN @SortColumn = 'IndexedBy' AND @SortDirection = 'ASC'
				THEN d.[IndexedBy] END ASC,
			CASE WHEN @SortColumn = 'IndexedBy' AND @SortDirection = 'DESC'
				THEN d.[IndexedBy] END DESC,
			CASE WHEN @SortColumn = 'FileUrl' AND @SortDirection = 'ASC'
				THEN d.[FileUrl] END ASC,
			CASE WHEN @SortColumn = 'FileUrl' AND @SortDirection = 'DESC'
				THEN d.[FileUrl] END DESC,
			CASE WHEN @SortColumn = 'NumberOfPayments' AND @SortDirection = 'ASC'
				THEN d.[NumberOfPayments] END ASC,
			CASE WHEN @SortColumn = 'NumberOfPayments' AND @SortDirection = 'DESC'
				THEN d.[NumberOfPayments] END DESC,
			CASE WHEN @SortColumn = 'TotalAmountPaid' AND @SortDirection = 'ASC'
				THEN d.[TotalAmountPaid] END ASC,
			CASE WHEN @SortColumn = 'TotalAmountPaid' AND @SortDirection = 'DESC'
				THEN d.[TotalAmountPaid] END DESC,
			CASE WHEN @SortColumn = 'IndexedOn' AND @SortDirection = 'ASC'
				THEN d.[IndexedOn] END ASC,
			CASE WHEN @SortColumn = 'IndexedOn' AND @SortDirection = 'DESC'
				THEN d.[IndexedOn] END DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END
GO
