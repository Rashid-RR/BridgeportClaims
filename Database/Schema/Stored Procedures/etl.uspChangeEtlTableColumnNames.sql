SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/9/2017
	Description:	
	Sample Execute:
					EXEC etl.uspChangeEtlTableColumnNames '[etl].[BulkInsertLakerFile]'
*/
CREATE PROC [etl].[uspChangeEtlTableColumnNames] @TableName VARCHAR(100)
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @ColumnName VARCHAR(100), @SQLStatement NVARCHAR(4000), @NewColumnName VARCHAR(3), @Loop INT = 1
	IF @TableName LIKE '%[%' OR @TableName LIKE '%]%'
		SET @TableName = REPLACE(REPLACE(@TableName, '[', ''), ']', '')

	DECLARE ColumnRenamer CURSOR LOCAL FAST_FORWARD FOR
	SELECT c.[name] FROM sys.[columns] AS [c]
	INNER JOIN sys.[tables] AS [t] ON [t].[object_id] = [c].[object_id]
	INNER JOIN sys.[schemas] AS [s] ON [s].[schema_id] = [t].[schema_id]
	WHERE s.[name] = PARSENAME(@TableName, 2)
	AND t.[name] = PARSENAME(@TableName, 1)

	OPEN [ColumnRenamer];
	FETCH NEXT FROM [ColumnRenamer] INTO @ColumnName
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRY
				SELECT @NewColumnName = CONVERT(VARCHAR,(CAST(REPLACE(@ColumnName, 'column ', '') AS INT) + 1))
				SET @SQLStatement = N'EXEC [sys].[sp_rename] ''' + 
					QUOTENAME(PARSENAME(@TableName, 2)) + '.' + QUOTENAME(PARSENAME(@TableName, 1)) +
						 '.' + QUOTENAME(@ColumnName) + ''', ''' + @NewColumnName + ''', ''column'''
				EXEC [sys].[sp_executesql] @SQLStatement
			END TRY
			BEGIN CATCH
				THROW;
				BREAK;
			END CATCH
			SET @Loop += 1
			FETCH NEXT FROM [ColumnRenamer] INTO @ColumnName    
		END
	CLOSE [ColumnRenamer]
	DEALLOCATE [ColumnRenamer]
END
GO
