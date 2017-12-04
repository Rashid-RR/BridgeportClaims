SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	12/03/2017
	Description:	Proc that determines whether or not an Insert or Update
					is necessary, based on the existence of a record in the DocumentIndex
					table for the @DocumentID passed in. Tran isolation level set to avoid race conditions.
	Sample Execute:
					EXEC [dbo].[uspDocumentIndexUpsert]
*/
CREATE PROC [dbo].[uspDocumentIndexUpsert]
(
	@DocumentID INT,
    @ClaimID INT,
    @DocumentTypeID TINYINT,
    @RxDate DATETIME2,
    @RxNumber VARCHAR(100),
    @InvoiceNumber VARCHAR(100),
    @InjuryDate DATETIME2,
    @AttorneyName VARCHAR(255),
	@Exists BIT OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		SET @Exists = CAST(
			CASE WHEN EXISTS
				(	SELECT  *
					FROM    [dbo].[DocumentIndex] AS [di]
					WHERE   [di].[DocumentID] = @DocumentID)
				THEN 1 ELSE 0 END AS BIT);

		IF (@Exists = 1)
			EXEC [dbo].[uspDocumentIndexUpdate] @DocumentID = @DocumentID, @ClaimID = @ClaimID
				, @DocumentTypeID = @DocumentTypeID, @RxDate = @RxDate, @RxNumber = @RxNumber
				, @InvoiceNumber = @InvoiceNumber, @InjuryDate = @InjuryDate, @AttorneyName = @AttorneyName;
		ELSE
			EXEC [dbo].[uspDocumentIndexInsert] @DocumentID = @DocumentID, @ClaimID = @ClaimID
				, @DocumentTypeID = @DocumentTypeID, @RxDate = @RxDate, @RxNumber = @RxNumber
				, @InvoiceNumber = @InvoiceNumber, @InjuryDate = @InjuryDate, @AttorneyName = @AttorneyName;

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
