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
    @ClaimID INT,
    @DocumentTypeID TINYINT,
    @RxDate DATETIME2,
    @RxNumber VARCHAR(100),
    @InvoiceNumber VARCHAR(100),
    @InjuryDate DATETIME2,
    @AttorneyName VARCHAR(255),
    @CreatedOnUTC DATETIME2,
    @UpdatedOnUTC DATETIME2
AS 
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
	
		INSERT INTO [dbo].[DocumentIndex] ([ClaimID], [DocumentTypeID], [RxDate], [RxNumber], [InvoiceNumber], [InjuryDate],
			 [AttorneyName], [CreatedOnUTC], [UpdatedOnUTC])
		SELECT @ClaimID, @DocumentTypeID, @RxDate, @RxNumber, @InvoiceNumber, @InjuryDate, @AttorneyName, @CreatedOnUTC, 
			@UpdatedOnUTC

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
