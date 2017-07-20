SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

/*
	Author:				Jordan Gurney
	Date:				7/30/2015
	Description:		Intended to rename ugly, unreliable names of primary keys
						that are system generated and have trailing numbers in them.
	Example Execute:
						EXEC dbo.usp_rename_primary_key '[dbo].[IncentiveLedgerModel]'
*/
CREATE PROC [util].[uspRenamePrimaryKey]
    @TableName SYSNAME -- Include Schema Name
AS
    BEGIN
        SET NOCOUNT ON
        SET TRAN ISOLATION LEVEL READ UNCOMMITTED
        BEGIN TRY
            BEGIN TRAN
			IF @TableName LIKE '%[%' OR @TableName LIKE '%]%'
				SET @TableName = REPLACE(REPLACE(@TableName, '[', ''), ']', '')

            DECLARE @PkName SYSNAME
              , @NewName SYSNAME
            SELECT  @PkName = [util].[udfGetPrimaryKeyIndexName](@TableName)
            SELECT  @NewName = 'Pl' + PARSENAME(@TableName, 1)
            DECLARE @SQLStatement NVARCHAR(1000)
            SET @SQLStatement = 'EXEC sys.sp_rename N''' + @PkName + ''', N'''
                + @NewName + ''', N''OBJECT'''
            EXEC sys.sp_executesql @SQLStatement
			--PRINT @SQLStatement
            COMMIT
			RAISERROR(N'The Primary key was renamed Successfully', 10, 1) WITH NOWAIT
        END TRY
        BEGIN CATCH
            WHILE @@TRANCOUNT > 0
                ROLLBACK

			RAISERROR(N'An Error Occurred. The Primary key was not renamed.', 10, 1) WITH NOWAIT
            DECLARE @ErrSeverity INT = ERROR_SEVERITY()
              , @ErrState INT = ERROR_STATE()
              , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
              , @ErrLine INT = ERROR_LINE()
              , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE()
              , @ErrNumber INT = ERROR_NUMBER()

            RAISERROR(N'error procedure %s (line %d): %s',     -- Message text w formatting
                           @ErrSeverity,        -- Severity
                           @ErrState,                 -- State
                           @ErrProc,                  -- First argument (string)
                           @ErrLine,                  -- Second argument (int)
                           @ErrMsg)                   -- First argument (string)
			WITH NOWAIT
        END CATCH
    END


GO
