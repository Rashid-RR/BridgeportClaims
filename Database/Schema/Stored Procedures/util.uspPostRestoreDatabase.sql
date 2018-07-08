SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* 
 =============================================
 Author:            Jordan Gurney
 Create date:       2/16/2018
 Description:       Utility proc that cleans up the restored BACPAC restore of the Production database locally.
 Example Execute:
                    EXECUTE [util].[uspPostRestoreDatabase]
 =============================================
*/
CREATE PROC [util].[uspPostRestoreDatabase]
(
	@IgnoreUsers BIT = 1
)
AS BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

	IF @IgnoreUsers IS NULL
		SET @IgnoreUsers = 0;

	IF @@SERVERNAME NOT LIKE '%\DB1'
		RETURN -1;

    BEGIN TRY
        BEGIN TRAN;

		-- Rebuild all indexes to standardized fillfactor and data compression options.    
        DECLARE @Script NVARCHAR(1000);
		DECLARE ScriptCrsor CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
		SELECT          N'ALTER INDEX ' + QUOTENAME([i].[name]) + N' ON ' + QUOTENAME([s].[name]) + N'.' + QUOTENAME([t].[name]) +
						N' REBUILD WITH (FILLFACTOR = 90, DATA_COMPRESSION = ' + CASE i.[type] WHEN 2 THEN N'PAGE' ELSE N'ROW' END + N');'
		FROM            [sys].[indexes] i
			INNER JOIN  [sys].[tables]  t ON [t].[object_id] = [i].[object_id]
			INNER JOIN  [sys].[schemas] AS [s] ON [s].[schema_id] = [t].[schema_id]
		WHERE           [t].[is_ms_shipped] = 0

		OPEN ScriptCrsor

		FETCH NEXT FROM ScriptCrsor INTO @Script

		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXECUTE [sys].[sp_executesql] @Script
			FETCH NEXT FROM ScriptCrsor INTO @Script
		END

		CLOSE ScriptCrsor;
		DEALLOCATE ScriptCrsor;

		IF (@IgnoreUsers = 0)
			BEGIN
				-- remove all extraneous users.
				DECLARE UserScriptCrsor CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
				SELECT  N'DROP USER ' + QUOTENAME([i].[name])
				FROM    [sys].[database_principals] i
				WHERE   [i].[name] NOT IN ('public', 'dbo', 'guest', 'INFORMATION_SCHEMA', 'sys')
						AND [i].[type_desc] = 'SQL_USER'

				OPEN UserScriptCrsor;

				FETCH NEXT FROM UserScriptCrsor INTO @Script

				WHILE @@FETCH_STATUS = 0
				BEGIN
					EXECUTE [sys].[sp_executesql] @Script
					FETCH NEXT FROM UserScriptCrsor INTO @Script
				END

				CLOSE UserScriptCrsor;
				DEALLOCATE UserScriptCrsor;
			END;

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

        RAISERROR(N'%s (line %d): %s',    -- Message text w formatting
            @ErrSeverity,        -- Severity
            @ErrState,            -- State
            @ErrProc,            -- First argument (string)
            @ErrLine,            -- Second argument (int)
            @ErrMsg);            -- First argument (string)
    END CATCH
END
GO
