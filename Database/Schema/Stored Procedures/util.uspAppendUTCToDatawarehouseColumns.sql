SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	7/22/2017
	Description:	Renames all CreatedOn and UpdatedOn columns to append UTC at the end
	Sample Execute:
					EXEC util.uspAppendUTCToDatawarehouseColumns
*/
CREATE PROC [util].[uspAppendUTCToDatawarehouseColumns]
AS BEGIN
	SET NOCOUNT ON;
	DECLARE @SQLStatement NVARCHAR(1000);
	DECLARE renamer CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
	SELECT 'EXEC sys.sp_rename ''' + QUOTENAME([s].[name]) + '.' + QUOTENAME([t].[name]) +
			 '.' + QUOTENAME([c].[name]) + ''', ''' + [c].[name] + 'UTC'', ''column'''
	FROM   [sys].[columns] AS [c]
		   INNER JOIN [sys].[tables] AS [t] ON [t].[object_id] = [c].[object_id]
		   INNER JOIN [sys].[schemas] AS [s] ON [s].[schema_id] = [t].[schema_id]
	WHERE  [c].[name] IN ( 'CreatedOn', 'UpdatedOn' )
	OPEN [renamer];
	FETCH NEXT FROM [renamer] INTO @SQLStatement
	WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC [sys].[sp_executesql] @SQLStatement
			FETCH NEXT FROM [renamer] INTO @SQLStatement
		END
	CLOSE [renamer];
	DEALLOCATE [renamer];
END
GO
