SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/6/2017
	Description:	
	Sample Execute:
					EXEC [util].[uspRemoveDataWarehouseColumns] @TableName = '[dbo].[CollectionAssignment]', @DebugOnly = 1
*/
CREATE PROC [util].[uspRemoveDataWarehouseColumns]
(
	@TableName SYSNAME, -- Include schema name
	@DebugOnly BIT = 0
)
AS BEGIN
	DECLARE @PrntMsg NVARCHAR(100)
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN;
		DECLARE @SQLStatement   NVARCHAR(4000)
				, @Suffix       NCHAR(4)      = 'Temp'
				, @PkColumn     NVARCHAR(100) = util.udfGetPrimaryKeyColumnName(@TableName)
		SET @SQLStatement = N'SELECT ' + @PkColumn + 
			', CreatedOnUTC, UpdatedOnUTC INTO ' + QUOTENAME(REPLACE(REPLACE(@TableName, ']', ''), '[', '') + @Suffix) + ' FROM ' + @TableName
		IF @DebugOnly = 1 BEGIN PRINT @SQLStatement END ELSE BEGIN EXEC sys.sp_executesql @SQLStatement END
		SET @SQLStatement = N'ALTER TABLE ' + @TableName + ' DROP CONSTRAINT df' + PARSENAME(@TableName, 1) + 'CreatedOnUTC'
		IF @DebugOnly = 1 BEGIN PRINT @SQLStatement END ELSE BEGIN EXEC sys.sp_executesql @SQLStatement END
		SET @SQLStatement = N'ALTER TABLE ' + @TableName + ' DROP CONSTRAINT df' + PARSENAME(@TableName, 1) + 'UpdatedOnUTC'
		IF @DebugOnly = 1 BEGIN PRINT @SQLStatement END ELSE BEGIN EXEC sys.sp_executesql @SQLStatement END
		SET @SQLStatement = N'ALTER TABLE ' + @TableName + ' DROP COLUMN CreatedOnUTC'
		IF @DebugOnly = 1 BEGIN PRINT @SQLStatement END ELSE BEGIN EXEC sys.sp_executesql @SQLStatement END
		SET @SQLStatement = N'ALTER TABLE ' + @TableName + ' DROP COLUMN UpdatedOnUTC'
		IF @DebugOnly = 1 BEGIN PRINT @SQLStatement END ELSE BEGIN EXEC sys.sp_executesql @SQLStatement END
		SET @SQLStatement = N'ALTER TABLE ' + @TableName + ' DROP COLUMN DataVersion'
		IF @DebugOnly = 1 BEGIN PRINT @SQLStatement END ELSE BEGIN EXEC sys.sp_executesql @SQLStatement END
		-- Step 2, populate those columns back. In most cases, this is not necessary. 
		/*EXEC util.uspAddDataWarehouseColumnsToTable @TableName = @TableName
		SET @SQLStatement = N'UPDATE a SET a.CreatedOnUTC = t.CreatedOnUTC, a.UpdatedOnUTC = t.UpdatedOnUTC FROM ' +
			@TableName + ' AS a INNER JOIN ' + @TableName + @Suffix + ' AS t ON t.' + @PkColumn + ' = a.' + @PkColumn
		IF @DebugOnly = 1 BEGIN PRINT @SQLStatement END ELSE BEGIN EXEC sys.sp_executesql @SQLStatement END*/
		SET @SQLStatement = N'DROP TABLE ' + QUOTENAME(REPLACE(REPLACE(@TableName, ']', ''), '[', '') + @Suffix)
		IF @DebugOnly = 1 BEGIN PRINT @SQLStatement END ELSE BEGIN EXEC sys.sp_executesql @SQLStatement END
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
		
		SET @PrntMsg = N'Error Line: ' + CAST(@ErrLine AS NVARCHAR(4000))
		PRINT @PrntMsg

        RAISERROR(N'%s (line %d): %s', @ErrSeverity, @ErrState, @ErrProc, @ErrLine, @ErrMsg);
	END CATCH
END
GO
