SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Date:			5/25/2017
	Description:	Creates a "CreatedOn" and "UpdatedOn" column for the table (DATETIME2 DEFAULT SYSDATETIME()),
					and a "DataVersion" column that is a TIMESTAMP.
	Example Execute:
					EXEC util.uspAddDataWarehouseColumnsToTable 'dbo.Patient', 1
*/
CREATE PROCEDURE [util].[uspAddDataWarehouseColumnsToTable] @TableName SYSNAME -- Includes Schema Name
				, @DebugOnly BIT = 0
AS BEGIN
	SET NOCOUNT ON
		BEGIN TRY
			BEGIN TRAN
			DECLARE @TableNameWithoutSchema SYSNAME = PARSENAME(@TableName, 1)
					, @SchemaName SYSNAME = PARSENAME(@TableName, 2)
					, @SQLStatement NVARCHAR(1000)
					, @Return CHAR(1) = CHAR(10) + CHAR(13)

			IF NOT EXISTS ( SELECT  *
							FROM    sys.columns AS c
							WHERE   c.object_id = OBJECT_ID(@TableName, N'U') AND c.name IN (
									'CreatedOn', 'UpdatedOn', 'DataVersion') )
				BEGIN
					SET @SQLStatement = N'ALTER TABLE ' + QUOTENAME(@SchemaName) + '.' 
						+ QUOTENAME(@TableNameWithoutSchema) + ' ADD [CreatedOn] DATETIME2 NOT NULL CONSTRAINT [df' +
						@TableNameWithoutSchema + 'CreatedOn] DEFAULT (SYSDATETIME());' + @Return
					SET @SQLStatement += N'ALTER TABLE ' + QUOTENAME(@SchemaName) + '.' 
						+ QUOTENAME(@TableNameWithoutSchema) + ' ADD [UpdatedOn] DATETIME2 NOT NULL CONSTRAINT [df' +
						@TableNameWithoutSchema + 'UpdatedOn] DEFAULT (SYSDATETIME());' + @Return
					SET @SQLStatement += N'ALTER TABLE ' + QUOTENAME(@SchemaName) + '.' 
						+ QUOTENAME(@TableNameWithoutSchema) + ' ADD [DataVersion] ROWVERSION NOT NULL;'
					IF @DebugOnly = 1
						PRINT @SQLStatement
					ELSE
						BEGIN
							EXEC sys.sp_executesql @SQLStatement
							PRINT 'Success!'
							PRINT N'Added columns successfully'
						END
				END
			IF @@TRANCOUNT > 0
				COMMIT
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
