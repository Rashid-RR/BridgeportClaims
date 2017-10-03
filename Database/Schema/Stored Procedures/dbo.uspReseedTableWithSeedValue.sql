SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/1/2017
	Description:	Executes util.uspReseedTable, but returns the next identity
					value. Must hold an exclusive lock on the table.
	Sample Execute:
					EXEC dbo.uspReseedTableWithSeedValue
*/
CREATE PROC [dbo].[uspReseedTableWithSeedValue]
(
	@TableName SYSNAME, -- Include schema name.
	@SeedValue INT OUTPUT
)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @PrntMsg NVARCHAR(1000);
	SET DEADLOCK_PRIORITY HIGH;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRY
		BEGIN TRAN;
		EXEC util.uspReseedTable @TableName = @TableName, @DebugOnly = 0;
		SELECT @SeedValue = IDENT_CURRENT(@TableName);
		IF (@@TRANCOUNT > 0)
			COMMIT;
	END TRY
    BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK;
			
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(4000) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
		
		SET @PrntMsg = N'Error Line: ' + CONVERT(NVARCHAR, @ErrLine)
		PRINT @PrntMsg

        RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg);			-- First argument (string)
	END CATCH
END
GO
