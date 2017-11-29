SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/28/2017
	Description:	CRUD Proc inserting into [dbo].[Document]
	Sample Execute:
					EXEC [dbo].[uspDocumentInsert]
*/
CREATE PROC [dbo].[uspDocumentInsert]
    @ClaimID INT,
    @DocumentTypeID TINYINT,
    @RxDate DATETIME2,
    @RxNumber VARCHAR(100),
    @InvoiceNumber VARCHAR(100),
    @InjuryDate DATETIME2,
    @AttorneyName VARCHAR(255),
    @FileName VARCHAR(1000),
    @Extension VARCHAR(50),
    @FileSize VARCHAR(50),
    @CreationTimeLocal DATETIME2,
    @LastAccessTimeLocal DATETIME2,
    @LastWriteTimeLocal DATETIME2,
    @DirectoryName VARCHAR(255),
    @FullFilePath NVARCHAR(4000),
    @FileUrl NVARCHAR(4000),
    @CreatedOnUTC DATETIME2,
    @UpdatedOnUTC DATETIME2
AS 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
	
		INSERT INTO [dbo].[Document] ([ClaimID], [DocumentTypeID], [RxDate], 
			[RxNumber], [InvoiceNumber], [InjuryDate], [AttorneyName], [FileName], [Extension], [FileSize],
			[CreationTimeLocal], [LastAccessTimeLocal], [LastWriteTimeLocal], [DirectoryName], [FullFilePath],
			[FileUrl], [CreatedOnUTC], [UpdatedOnUTC])
		SELECT @ClaimID, @DocumentTypeID, @RxDate, @RxNumber, @InvoiceNumber, @InjuryDate,
			 @AttorneyName, @FileName, @Extension, @FileSize, @CreationTimeLocal, @LastAccessTimeLocal,
			 @LastWriteTimeLocal, @DirectoryName, @FullFilePath, @FileUrl, @CreatedOnUTC, @UpdatedOnUTC

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
GO
