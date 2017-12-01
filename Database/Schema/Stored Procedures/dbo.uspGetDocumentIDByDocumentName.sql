SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/29/2017
	Description:	CRUD Proc for Deleting in table [dbo].[Document]
	Sample Execute:
					DECLARE @DocumentID INT
					EXEC [dbo].[uspGetDocumentIDByDocumentName] 'csp201711255300.pdf', @DocumentID = @DocumentID OUTPUT
					SELECT @DocumentID DocumentID
*/
CREATE PROC [dbo].[uspGetDocumentIDByDocumentName]
	@FileName VARCHAR(1000),
	@DocumentID INT OUTPUT
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
	
		SELECT @DocumentID = [d].[DocumentID]
		FROM   [dbo].[Document] AS [d]
		WHERE  [d].[FileName] = @FileName
	
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
