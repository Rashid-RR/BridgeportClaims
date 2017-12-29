SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	CRUD Proc inserting into [dbo].[DocumentIndex]
	Sample Execute:
					EXEC [dbo].[uspDocumentIndexInsert]
*/
CREATE PROC [dbo].[uspDocumentIndexInsert]
(
	@DocumentID INT,
    @ClaimID INT,
    @DocumentTypeID TINYINT,
    @RxDate DATETIME2,
    @RxNumber VARCHAR(100),
    @InvoiceNumber VARCHAR(100),
    @InjuryDate DATETIME2,
    @AttorneyName VARCHAR(255),
	@IndexedByUserID NVARCHAR(128)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;

		DECLARE @UtcNow DATETIME2 = [dtme].[udfGetLocalDate]()
	
		INSERT [dbo].[DocumentIndex] ([DocumentID], [ClaimID], [DocumentTypeID], [RxDate], [RxNumber], [InvoiceNumber],
				[InjuryDate], [AttorneyName], [IndexedByUserID], [CreatedOnUTC], [UpdatedOnUTC])
		SELECT @DocumentID, @ClaimID, @DocumentTypeID, @RxDate, @RxNumber, @InvoiceNumber, @InjuryDate,
				@AttorneyName, @IndexedByUserID, @UtcNow, @UtcNow

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
