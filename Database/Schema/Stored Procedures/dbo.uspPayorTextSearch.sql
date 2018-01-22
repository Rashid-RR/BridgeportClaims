SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	1/21/2018
	Description:	Proc that searches the Payor (Carrier) via an
					auto-suggest drop-down.
	Sample Execute:
					EXEC [dbo].[uspPayorTextSearch] 'mi'
*/
CREATE PROC [dbo].[uspPayorTextSearch]
(
    @SearchText VARCHAR(800)
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		DECLARE @WildCard CHAR(1) = '%', @NewLine CHAR(1) = CHAR(10) + CHAR(13);
		IF (@SearchText IS NULL)
			BEGIN
				IF (@@TRANCOUNT > 0)
					ROLLBACK;
				RAISERROR(N'Error. The @SearchText parameter cannot be NULL.', 16, 1) WITH NOWAIT;
				RETURN -1;
			END

		SELECT      PayorId = [p].[PayorID],
					GroupName = [p].[GroupName]
		FROM        [dbo].[Payor] AS [p]
		WHERE		[p].[GroupName] LIKE CONCAT(@WildCard, @SearchText, @WildCard)

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
