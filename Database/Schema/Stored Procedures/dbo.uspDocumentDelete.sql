SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	CRUD Proc for Deleting in table [dbo].[Document]
	Sample Execute:
					EXEC [dbo].[uspDocumentDelete] 
*/
CREATE PROC [dbo].[uspDocumentDelete]
	@DocumentID INT
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @Msg NVARCHAR(500), @RowCnt INT
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
	
		DELETE [dbo].[Document] WHERE [DocumentID] = @DocumentID
		SET @RowCnt = @@ROWCOUNT
		IF @RowCnt != 1
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK
				SET @Msg = N'Error, the delete statement removed ' + CONVERT(NVARCHAR, @RowCnt) + ' rows. Not 1 like it should have.'
				RAISERROR(@Msg, 16, 1) WITH NOWAIT
				RETURN
			END
	
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
GRANT EXECUTE ON  [dbo].[uspDocumentDelete] TO [bridgeportClaimsWindowsServiceUser]
GO
