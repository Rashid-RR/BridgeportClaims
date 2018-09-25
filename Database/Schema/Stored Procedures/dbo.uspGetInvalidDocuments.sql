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
					DECLARE @TotalRows INT
					EXEC [dbo].[uspGetInvalidDocuments] NULL, NULL, 'CreationTime', 'DESC', 1, 5000, @TotalRows OUTPUT
					SELECT @TotalRows TotalRows
*/
CREATE PROC [dbo].[uspGetInvalidDocuments]
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
	BEGIN TRY;
		BEGIN TRAN;
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
		AND ci.DocumentID IS NULL
		AND (@Date IS NULL OR d.DocumentDate = @Date)
		AND ([d].[FileName] LIKE CONCAT(CONCAT(@WildCard, @FileName), @WildCard) OR @FileName IS NULL)
		AND d.Archived = 0
		AND d.IsValid = 0;

	SELECT @TotalRows = COUNT(*) FROM [#Document]

	SELECT [DocumentId] = d.DocumentID
		 , d.[FileName]
		 , d.Extension
		 , d.FileSize
		 , d.CreationTimeLocal CreationTime
		 , d.LastAccessTimeLocal LastAccessTime
		 , d.LastWriteTimeLocal LastWriteTime
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

	IF (@@TRANCOUNT > 0)
		COMMIT;
	END TRY
    BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK;
				
		DECLARE @ErrSeverity INT = ERROR_SEVERITY()
			, @ErrState INT = ERROR_STATE()
			, @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
			, @ErrLine INT = ERROR_LINE()
			, @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();

		RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg);			-- First argument (string)
    END CATCH
END

GO
