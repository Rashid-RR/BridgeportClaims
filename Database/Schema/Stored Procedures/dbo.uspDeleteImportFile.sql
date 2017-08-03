SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/3/2017
	Description:	Deletes an ImportFile record by ImportFileID
	Sample Execute:
					EXEC dbo.uspDeleteImportFile @ImportFileID
*/
CREATE PROC [dbo].[uspDeleteImportFile]
    @ImportFileID INTEGER
AS
    BEGIN
        SET NOCOUNT ON;
		SET DEADLOCK_PRIORITY HIGH;
		SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
		SET XACT_ABORT ON
        BEGIN TRY
			BEGIN TRAN;
			DELETE [i]
			FROM  [util].[ImportFile] AS [i]
			WHERE [i].[ImportFileID] = @ImportFileID
			IF @@ROWCOUNT = 1 AND @@TRANCOUNT > 0
				COMMIT
			ELSE IF @@TRANCOUNT > 0
				ROLLBACK
        END TRY
        BEGIN CATCH
			IF @@TRANCOUNT > 0
				ROLLBACK
			DECLARE @ErrSeverity INT = ERROR_SEVERITY()
				, @ErrState INT = ERROR_STATE()
				, @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
				, @ErrLine INT = ERROR_LINE()
				, @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE()

			RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
				@ErrSeverity,		-- Severity
				@ErrState,			-- State
				@ErrProc,			-- First argument (string)
				@ErrLine,			-- Second argument (int)
				@ErrMsg)			-- First argument (string)
		END CATCH
    END
GO
