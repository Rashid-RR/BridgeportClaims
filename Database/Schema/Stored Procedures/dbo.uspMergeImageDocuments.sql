SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/30/2017
	Description:	Uses a DataTable to do a SQL MERGE on [dbo].[vwImageDocument] (a view that filters to only image document types).
	Sample Execute:
					EXEC [dbo].[uspMergeImageDocuments]
*/
CREATE PROCEDURE [dbo].[uspMergeImageDocuments]
    @Documents dbo.udtDocument READONLY
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;

		DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME();
	
		MERGE [dbo].[vwImageDocument] AS [tgt]
		USING (SELECT [d].[FileName]
                    , [d].[Extension]
                    , [d].[FileSize]
                    , [d].[CreationTimeLocal]
                    , [d].[LastAccessTimeLocal]
                    , [d].[LastWriteTimeLocal]
                    , [d].[DirectoryName]
                    , [d].[FullFilePath]
                    , [d].[FileUrl]
                    , [d].[DocumentDate]
					, [d].[ByteCount]
					, [d].[FileTypeID]
				FROM @Documents AS [d]) AS [src]
				ON [tgt].[FullFilePath] = [src].[FullFilePath]
		WHEN NOT MATCHED BY TARGET THEN
			INSERT ([FileName], [Extension], [FileSize], [CreationTimeLocal], [LastAccessTimeLocal], [LastWriteTimeLocal],
				[DirectoryName], [FullFilePath], [FileUrl], [DocumentDate], [ByteCount], [FileTypeID], [CreatedOnUTC], [UpdatedOnUTC])
			VALUES ([src].[FileName], [src].[Extension], [src].[FileSize], [src].[CreationTimeLocal], [src].[LastAccessTimeLocal],
				[src].[LastWriteTimeLocal], [src].[DirectoryName], [src].[FullFilePath], [src].[FileUrl], [src].[DocumentDate], [src].[ByteCount],
				[src].[FileTypeID], @UtcNow, @UtcNow)
		WHEN MATCHED THEN
			UPDATE SET [tgt].[Extension] = [src].[Extension],
					   [tgt].[FileSize] = [src].[FileSize],
					   [tgt].[CreationTimeLocal] = [src].[CreationTimeLocal],
					   [tgt].[LastAccessTimeLocal] = [src].[LastAccessTimeLocal],
					   [tgt].[LastWriteTimeLocal] = [src].[LastWriteTimeLocal],
					   [tgt].[DirectoryName] = [src].[DirectoryName],
					   [tgt].[FullFilePath] = [src].[FullFilePath],
					   [tgt].[FileUrl] = [src].[FileUrl],
                       [tgt].[DocumentDate] = [src].[DocumentDate],
					   [tgt].[ByteCount] = [src].[ByteCount],
					   [tgt].[FileTypeID] = [src].[FileTypeID],
					   [tgt].[UpdatedOnUTC] = @UtcNow
		WHEN NOT MATCHED BY SOURCE
			THEN DELETE;

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
