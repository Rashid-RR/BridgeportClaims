SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/21/2015
	Description:	Runs a DBCC CHECKIDENT to reseed a table to the last PK value used,
					or to the value explicitly passed in.
	Sample Execute:
					EXEC util.uspReseedTable 'dbo.Diary', NULL, 1
*/
CREATE PROC [util].[uspReseedTable]
(
	@TableName SYSNAME, -- Include schema name.
	@SeedValue INT = NULL, -- Optional param to specify Seed Value. This is where new rows will start.
	@DebugOnly BIT = 0
)
AS BEGIN
	SET NOCOUNT ON;
	IF (@TableName LIKE '%]%' OR @TableName LIKE '%[%')
		SET @TableName = REPLACE(REPLACE(@TableName, '[', ''), ']', '')
	DECLARE @SQLStatement NVARCHAR(1000),
			@TableSchema SYSNAME = PARSENAME(@TableName, 2),
			@TableNameOnly SYSNAME = PARSENAME(@TableName, 1)
	DECLARE @FullyQualifiedName SYSNAME = QUOTENAME(@TableSchema) + '.' + QUOTENAME(@TableNameOnly)
	SET @SQLStatement =
		N'DECLARE @MaxID INT; SELECT @MaxID = ' + 
		CASE WHEN @SeedValue IS NULL THEN
		'MAX(' + util.udfGetPrimaryKeyColumnName(@FullyQualifiedName) +
		 N') FROM ' + @FullyQualifiedName ELSE CONVERT(NVARCHAR, (@SeedValue - 1)) END;
	SET @SQLStatement += N'; IF @MaxID IS NULL SET @MaxID = 0; IF @MaxID IS NOT NULL BEGIN DBCC CHECKIDENT(''' +
		@FullyQualifiedName + N''', RESEED, @MaxID) END;'
	IF @DebugOnly = 1
		PRINT @SQLStatement
	ELSE
		EXECUTE sys.sp_executesql @SQLStatement
END

GO
