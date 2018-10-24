SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	12/1/2017
	Description:	Proc that returns the grid results for the Documents page.
	Modified:		1/16/2018 to add an Archived bit flag.
	Sample Execute:
					DECLARE @TotalRows INT;
					EXEC [dbo].[uspGetDocuments] NULL, 0, NULL, 'CreationTime', 'DESC', 1, 5000, @TotalRows OUTPUT;
					SELECT @TotalRows TotalRows;
*/
CREATE PROC [dbo].[uspGetDocuments]
(
    @Date DATE
   ,@Archived BIT
   ,@FileName VARCHAR(1000)
   ,@FileTypeID TINYINT
   ,@SortColumn VARCHAR(50)
   ,@SortDirection VARCHAR(5)
   ,@PageNumber INTEGER
   ,@PageSize INTEGER
   ,@TotalRows INTEGER OUTPUT
)
AS
    BEGIN
        SET NOCOUNT ON;
        SET XACT_ABORT ON;
		DECLARE @WildCard CHAR(1) = '%';
		DECLARE @CheckType TINYINT = [dbo].[udfGetFileTypeByCode]('CK');
		DECLARE @IsCheckType BIT = CAST(CASE WHEN @CheckType = @FileTypeID THEN 1 ELSE 0 END AS BIT);
		DECLARE @CheckDateFilter DATE = '10/23/2018';

		CREATE TABLE #Document
		(
			[DocumentID] [INT] NOT NULL PRIMARY KEY
			,[FileName] [VARCHAR](1000) NOT NULL
			,[Extension] [VARCHAR](50) NOT NULL
			,[FileSize] [VARCHAR](50) NOT NULL
			,[CreationTimeLocal] [DATETIME2] NOT NULL
			,[LastAccessTimeLocal] [DATETIME2] NOT NULL
			,[LastWriteTimeLocal] [DATETIME2] NOT NULL
			,[FullFilePath] [NVARCHAR](4000) NOT NULL
			,[FileUrl] [NVARCHAR](4000) NOT NULL
			,[ByteCount] [BIGINT] NOT NULL
		);

		INSERT  [#Document] ([DocumentID]
							,[FileName]
							,[Extension]
							,[FileSize]
							,[CreationTimeLocal]
							,[LastAccessTimeLocal]
							,[LastWriteTimeLocal]
							,[FullFilePath]
							,[FileUrl]
							,[ByteCount])
		SELECT  d.DocumentID
				,d.[FileName]
				,d.Extension
				,d.FileSize
				,d.CreationTimeLocal
				,d.LastAccessTimeLocal
				,d.LastWriteTimeLocal
				,d.FullFilePath
				,d.FileUrl
				,d.ByteCount
		FROM    dbo.Document AS [d]
				LEFT JOIN dbo.DocumentIndex AS [di] ON di.DocumentID = d.DocumentID
				LEFT JOIN dbo.InvoiceIndex AS [ii] ON ii.DocumentID = d.DocumentID
				LEFT JOIN [dbo].[CheckIndex] AS [ci] ON ci.DocumentID = d.DocumentID
		WHERE   1 = 1
				AND di.DocumentID IS NULL
				AND ii.DocumentID IS NULL
				AND ci.DocumentID IS NULL
				AND (@Date IS NULL OR d.DocumentDate = @Date)
				AND ([d].[FileName] LIKE CONCAT(CONCAT(@WildCard, @FileName), @WildCard) OR @FileName IS NULL)
				AND d.Archived = @Archived
				AND d.FileTypeID = @FileTypeID
				AND d.IsValid = 1
				AND 1 = CASE WHEN @IsCheckType = 1 AND d.DocumentDate IS NOT NULL AND d.DocumentDate >= @CheckDateFilter THEN 1
							 WHEN @IsCheckType = 0 THEN 1
							 WHEN @IsCheckType = 1 AND @Archived = 1 THEN 1
							 ELSE 0
						END;

		SELECT  @TotalRows = COUNT(*) FROM  [#Document]

		SELECT  [DocumentId] = d.DocumentID
				,d.FileName
				,d.Extension
				,d.FileSize
				,d.CreationTimeLocal
				,d.LastAccessTimeLocal
				,d.LastWriteTimeLocal
				,d.FullFilePath
				,d.FileUrl
		FROM   [#Document] AS [d]
		ORDER BY CASE WHEN @SortColumn = 'DocumentID' AND @SortDirection = 'ASC'
					THEN d.DocumentID END ASC,
				CASE WHEN @SortColumn = 'DocumentID' AND @SortDirection = 'DESC'
					THEN d.DocumentID END DESC,
				CASE WHEN @SortColumn = 'FileName' AND @SortDirection = 'ASC'
					THEN d.[FileName] END ASC,
				CASE WHEN @SortColumn = 'FileName' AND @SortDirection = 'DESC'
					THEN d.[FileName] END DESC,
				CASE WHEN @SortColumn = 'Extension' AND @SortDirection = 'ASC'
					THEN d.Extension END ASC,
				CASE WHEN @SortColumn = 'Extension' AND @SortDirection = 'DESC'
					THEN d.Extension END DESC,
				CASE WHEN @SortColumn = 'FileSize' AND @SortDirection = 'ASC'
					THEN d.ByteCount END ASC,
				CASE WHEN @SortColumn = 'FileSize' AND @SortDirection = 'DESC'
					THEN d.ByteCount END DESC,
				CASE WHEN @SortColumn = 'CreationTime' AND @SortDirection = 'ASC'
					THEN d.CreationTimeLocal END ASC,
				CASE WHEN @SortColumn = 'CreationTime' AND @SortDirection = 'DESC'
					THEN d.CreationTimeLocal END DESC,
				CASE WHEN @SortColumn = 'LastAccessTime' AND @SortDirection = 'ASC'
					THEN d.LastAccessTimeLocal END ASC,
				CASE WHEN @SortColumn = 'LastAccessTime' AND @SortDirection = 'DESC'
					THEN d.LastAccessTimeLocal END DESC,
				CASE WHEN @SortColumn = 'LastWriteTime' AND @SortDirection = 'ASC'
					THEN d.LastWriteTimeLocal END ASC,
				CASE WHEN @SortColumn = 'LastWriteTime' AND @SortDirection = 'DESC'
					THEN d.LastWriteTimeLocal END DESC,
				CASE WHEN @SortColumn = 'FullFilePath' AND @SortDirection = 'ASC'
					THEN d.FullFilePath END ASC,
				CASE WHEN @SortColumn = 'FullFilePath' AND @SortDirection = 'DESC'
					THEN d.FullFilePath END DESC,
				CASE WHEN @SortColumn = 'FileUrl' AND @SortDirection = 'ASC'
					THEN d.FileUrl END ASC,
				CASE WHEN @SortColumn = 'FileUrl' AND @SortDirection = 'DESC'
					THEN d.FileUrl END DESC
		OFFSET @PageSize * (@PageNumber - 1) ROWS
		FETCH NEXT @PageSize ROWS ONLY;
    END
GO
