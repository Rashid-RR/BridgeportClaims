SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	CRUD Proc Updating the table [dbo].[Document]
	Sample Execute:
					EXEC [dbo].[uspDocumentUpdate] 
*/
CREATE PROC [dbo].[uspDocumentUpdate]
    @DocumentID INT,
    @FileName VARCHAR(1000),
    @Extension VARCHAR(50),
    @FileSize VARCHAR(50),
    @CreationTimeLocal DATETIME2,
    @LastAccessTimeLocal DATETIME2,
    @LastWriteTimeLocal DATETIME2,
    @DirectoryName VARCHAR(255),
    @FullFilePath NVARCHAR(4000),
    @FileUrl NVARCHAR(4000),
	@ByteCount BIGINT
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;

		DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME();
	
		UPDATE [dbo].[Document]
		SET    [FileName] = @FileName, [Extension] = @Extension, [FileSize] = @FileSize, [CreationTimeLocal] = @CreationTimeLocal,
			 [LastAccessTimeLocal] = @LastAccessTimeLocal, [LastWriteTimeLocal] = @LastWriteTimeLocal, [DirectoryName]
			  = @DirectoryName, [FullFilePath] = @FullFilePath, [FileUrl] = @FileUrl, [ByteCount] = @ByteCount, [UpdatedOnUTC] = @UtcNow
		WHERE  [DocumentID] = @DocumentID
	
		IF (@@TRANCOUNT > 0)
			COMMIT
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
