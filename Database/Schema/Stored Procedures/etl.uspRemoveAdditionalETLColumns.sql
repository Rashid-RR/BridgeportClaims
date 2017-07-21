SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/20/2017
	Description:	Removes columns from the staging tables after ordinal position 154
	Sample Execute:
					EXEC etl.uspRemoveAdditionalETLColumns 'etl.StagedLakerFile20170712'
*/
CREATE PROC [etl].[uspRemoveAdditionalETLColumns] @TableName SYSNAME -- Includes Schema Name
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @Name SYSNAME, @SQLStatement NVARCHAR(1000)
	DECLARE C CURSOR LOCAL FAST_FORWARD FOR
	SELECT [c].[name] FROM [sys].[columns] AS [c]
	INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
	INNER JOIN [sys].[schemas] AS [s] ON [s].[schema_id] = [t].[schema_id]
	WHERE [t].[name] = PARSENAME(@TableName, 1)
			AND CONVERT(VARCHAR,[c].[column_id]) != [c].[name]
			AND [s].[name] = PARSENAME(@TableName, 2)
	OPEN [C];
	FETCH NEXT FROM [C] INTO @Name
	WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @SQLStatement = N'ALTER TABLE ' + QUOTENAME(PARSENAME(@TableName, 2)) + 
					'.' + QUOTENAME(PARSENAME(@TableName, 1)) + ' DROP COLUMN ' + QUOTENAME(@Name)
			EXEC [sys].[sp_executesql] @SQLStatement
			FETCH NEXT FROM [C] INTO @Name
		END
	CLOSE [C]
	DEALLOCATE [C]
END
GO
