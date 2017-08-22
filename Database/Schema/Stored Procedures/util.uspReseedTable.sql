SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/21/2017
	Description:	Finds the last ID used and reseeds table to that value.
	Sample Execute:
					EXEC util.uspReseedTable 'dbo.Payment'
*/
CREATE PROC [util].[uspReseedTable] @TableName SYSNAME, -- Include schema name.
	@DebugOnly BIT = 0
AS BEGIN
	SET NOCOUNT ON;
	IF (@TableName LIKE '%]%' OR @TableName LIKE '%[%')
		SET @TableName = REPLACE(REPLACE(@TableName, '[', ''), ']', '')
	DECLARE @SQLStatement NVARCHAR(1000),
			@TableSchema SYSNAME = PARSENAME(@TableName, 2),
			@TableNameOnly SYSNAME = PARSENAME(@TableName, 1)
	DECLARE @FullyQualifiedName SYSNAME = QUOTENAME(@TableSchema) + '.' + QUOTENAME(@TableNameOnly)
	SET @SQLStatement =
		N'DECLARE @MaxID INT; SELECT @MaxID = MAX(' + util.udfGetPrimaryKeyColumnName(@FullyQualifiedName) +
		 N') FROM ' + @FullyQualifiedName
	SET @SQLStatement += N'; IF @MaxID IS NOT NULL BEGIN DBCC CHECKIDENT(''' +
		@FullyQualifiedName + N''', RESEED, @MaxID) END;'
	IF @DebugOnly = 1
		PRINT @SQLStatement
	ELSE
		EXECUTE sys.sp_executesql @SQLStatement
END
GO
