SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	09/23/2018
	Description:	Proc that returns the grid results for the Documents page for checks (invalid).
	Sample Execute:
					DECLARE @TotalRows INT
					EXEC [dbo].[uspGetInvalidCheckDocuments] NULL, 0, NULL, 'CreationTime', 'DESC', 1, 5000, @TotalRows OUTPUT
					SELECT @TotalRows TotalRows
*/
CREATE PROC [dbo].[uspGetInvalidCheckDocuments]
(
	@Date DATE,
	@FileName VARCHAR(1000),
	@SortColumn VARCHAR(50),
	@SortDirection VARCHAR(5),
	@PageNumber INTEGER,
	@PageSize INTEGER,
	@TotalRows INTEGER OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	SET DEADLOCK_PRIORITY HIGH;
	
	DECLARE @FileTypeID INT
	SELECT @FileTypeID = ft.FileTypeID FROM dbo.FileType AS ft WHERE ft.Code = 'CK';

	-- QA
	IF NOT EXISTS (SELECT * FROM dbo.FileType AS ft WHERE ft.FileTypeID = @FileTypeID)
		BEGIN
			IF (@@TRANCOUNT > 0)
				ROLLBACK;
			RAISERROR(N'I am struck inside of my stored procedure [dbo].[uspGetInvalidCheckDocuments]...', 16, 1) WITH NOWAIT;
			RETURN -1;
		END

	DECLARE @WildCard CHAR(1) = '%';
	CREATE TABLE #Document
	(
		[DocumentID] [int] NOT NULL PRIMARY KEY,
		[FileName] [varchar] (1000) NOT NULL,
		[Extension] [varchar] (50) NOT NULL,
		[FileSize] [varchar] (50) NOT NULL,
		[CreationTimeLocal] [datetime2] NOT NULL,
		[LastAccessTimeLocal] [datetime2] NOT NULL,
		[LastWriteTimeLocal] [datetime2] NOT NULL,
		[FullFilePath] [nvarchar] (4000) NOT NULL,
		[FileUrl] [nvarchar] (4000) NOT NULL,
		[ByteCount] [bigint] NOT NULL
	);

	INSERT [#Document]
		([DocumentID],[FileName],[Extension],[FileSize],[CreationTimeLocal],[LastAccessTimeLocal],
		[LastWriteTimeLocal],[FullFilePath],[FileUrl],[ByteCount])
	SELECT [d].[DocumentID],[d].[FileName],[d].[Extension],[d].[FileSize],[d].[CreationTimeLocal]
		,[d].[LastAccessTimeLocal],[d].[LastWriteTimeLocal],[d].[FullFilePath],[d].[FileUrl],[d].[ByteCount]
	FROM [dbo].[Document] AS [d] 
		 LEFT JOIN [dbo].[DocumentIndex] AS [di] ON [di].[DocumentID] = [d].[DocumentID]
		 LEFT JOIN [dbo].[InvoiceIndex] AS [ii] ON [ii].[DocumentID] = [d].[DocumentID]
		 LEFT JOIN [dbo].[CheckIndex] AS [ci] ON ci.DocumentID = d.DocumentID
	WHERE 1 = 1
		AND [di].[DocumentID] IS NULL
		AND [ii].[DocumentID] IS NULL
		AND [ci].[DocumentID] IS NULL
		AND (@Date IS NULL OR d.DocumentDate = @Date)
		AND ([d].[FileName] LIKE CONCAT(CONCAT(@WildCard, @FileName), @WildCard) OR @FileName IS NULL)
		AND [d].[FileTypeID] = @FileTypeID
		-- Valid Check File Types
		AND 1 = CASE WHEN [d].[FileTypeID] <> 3 AND [d].[FileTypeID] = 0
					 THEN 0
					 ELSE 1
				END

	SELECT @TotalRows = COUNT(*) FROM [#Document]

	SELECT [DocumentId] = d.DocumentID
		 , d.[FileName]
		 , d.Extension
		 , d.FileSize
		 , d.CreationTimeLocal
		 , d.LastAccessTimeLocal
		 , d.LastWriteTimeLocal
		 , d.FullFilePath
		 , d.FileUrl
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
