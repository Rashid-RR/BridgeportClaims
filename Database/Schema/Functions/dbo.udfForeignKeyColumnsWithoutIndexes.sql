SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	6/19/2017
	Description:	Function that returns the data necessary to identify, and then script
					a Create Index statement for tables / columns that are referenced in 
					foreign keys but do not have indexes.
	Sample Execute:
					SELECT * FROM dbo.udfForeignKeyColumnsWithoutIndexes()
*/
CREATE FUNCTION [dbo].[udfForeignKeyColumnsWithoutIndexes]()
RETURNS @Table TABLE 
(
	SchemaName VARCHAR(255),
	TableName VARCHAR(255) NOT NULL,
	ColumnName VARCHAR(255) NOT NULL,
	ForeignKeyName SYSNAME NOT NULL,
	IndexScript VARCHAR(4000) NOT NULL
)
AS BEGIN
	DECLARE 
		@SchemaName VARCHAR(255),
		@TableName VARCHAR(255),
		@ColumnName VARCHAR(255),
		@ForeignKeyName SYSNAME;

	DECLARE ForeignKeysCursor CURSOR LOCAL FAST_FORWARD FOR
	SELECT cu.TABLE_SCHEMA
		 , cu.TABLE_NAME
		 , cu.COLUMN_NAME
		 , cu.CONSTRAINT_NAME
	FROM   INFORMATION_SCHEMA.TABLE_CONSTRAINTS ic
		   INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE cu ON ic.CONSTRAINT_NAME = cu.CONSTRAINT_NAME
	WHERE  ic.CONSTRAINT_TYPE = 'FOREIGN KEY'

	DECLARE @ForeignKeyData AS TABLE (
		SchemaName VARCHAR(255),
		TableName VARCHAR(255),
		ColumnName VARCHAR(255),
		ForeignKeyName SYSNAME)

	OPEN ForeignKeysCursor  
	FETCH NEXT FROM ForeignKeysCursor INTO @SchemaName,@TableName, @ColumnName, @ForeignKeyName

	WHILE @@FETCH_STATUS = 0  
	BEGIN

		IF  (   SELECT COUNT(*)
				FROM   sys.sysobjects o
					   INNER JOIN sys.sysindexes x ON x.id = o.id
					   INNER JOIN sys.syscolumns c ON o.id = c.id
					   INNER JOIN sys.sysindexkeys xk ON c.colid = xk.colid
														 AND o.id = xk.id
														 AND x.indid = xk.indid
				WHERE  o.type IN ( 'U' )
					   AND xk.keyno <= x.keycnt
					   AND PERMISSIONS(o.id, c.name) <> 0
					   AND ( x.status & 32 ) = 0
					   AND o.name = @TableName
					   AND c.name = @ColumnName
			) = 0
			BEGIN
				INSERT @ForeignKeyData (SchemaName, TableName, ColumnName, ForeignKeyName)
				SELECT @SchemaName
					 , @TableName
					 , @ColumnName
					 , @ForeignKeyName
			END

		FETCH NEXT FROM ForeignKeysCursor INTO @SchemaName,@TableName, @ColumnName, @ForeignKeyName
	END  
	CLOSE ForeignKeysCursor  
	DEALLOCATE ForeignKeysCursor 

	INSERT @Table (SchemaName, TableName, ColumnName, ForeignKeyName, IndexScript)
	SELECT SchemaName, TableName, ColumnName, ForeignKeyName, 
		'CREATE INDEX idx' + SUBSTRING(ForeignKeyName, 3, LEN(ForeignKeyName)) + ' ON ' +
		 SchemaName + '.' + TableName + '(' + ColumnName +')' IndexScript
	FROM @ForeignKeyData 
	ORDER BY TableName
	RETURN
END
GO
