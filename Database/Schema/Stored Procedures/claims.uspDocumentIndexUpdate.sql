SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	CRUD Proc Updating the table [dbo].[DocumentIndex]
	Sample Execute:
					EXEC [claims].[uspDocumentIndexUpdate] 
*/
CREATE PROC [claims].[uspDocumentIndexUpdate]
    @DocumentID INT,
    @ClaimID INT,
    @DocumentTypeID TINYINT,
    @RxDate DATETIME2,
    @RxNumber VARCHAR(100),
    @InvoiceNumber VARCHAR(100),
    @InjuryDate DATETIME2,
    @AttorneyName VARCHAR(255),
	@IndexedByUserID NVARCHAR(128)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;

		DECLARE @UtcNow DATETIME2 = [dtme].[udfGetLocalDate]();
	
		UPDATE [dbo].[DocumentIndex]
		SET    [ClaimID] = @ClaimID, [DocumentTypeID] = @DocumentTypeID, [RxDate] = @RxDate, [RxNumber] = @RxNumber, 
			   [InvoiceNumber] = @InvoiceNumber, [InjuryDate] = @InjuryDate, [AttorneyName] = @AttorneyName,
			   [IndexedByUserID] = @IndexedByUserID, [UpdatedOnUTC] = @UtcNow
		WHERE  [DocumentID] = @DocumentID
	
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
