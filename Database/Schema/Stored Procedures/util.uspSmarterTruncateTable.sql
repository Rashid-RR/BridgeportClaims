SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =================================================================================================================================
-- Change:      8/03/2011: Added schema name to generated SQL so proc will work with base schema objects
--                  The value passed in for @TableName should include the base schema qualifier
--              8/23/2011: Modified WHERE clause in subquery to add "f.parent_object_id = sob.parent_obj"
--                  The old code was comparing names only, without schema. This produced duplicates when there were objects
--                  in multiple schemas with the same name (i.e. dbo.Titles and base.Titles)
-- =================================================================================================================================
CREATE PROCEDURE [util].[uspSmarterTruncateTable]  --'etl.Accounts'
     @TableName VARCHAR(100)  
AS  
BEGIN  
	 IF @TableName LIKE '%[%' OR @TableName LIKE '%]%'
		SET @TableName = REPLACE(REPLACE(@TableName, '[', ''), ']', '')
     DECLARE @UQTableName VARCHAR(100) = ( SELECT SUBSTRING(@TableName, CHARINDEX('.', @TableName) + 1, LEN(@TableName)) )
     DECLARE @SchemaName VARCHAR(20) = ( SELECT LEFT(@TableName, CHARINDEX('.', @TableName)- 1 ))
     DECLARE @NewLineChar AS CHAR(2) = CHAR(13) + CHAR(10)
     DECLARE @foreignKeyName sysname, @foreignKeyTableName sysname, @createScript VARCHAR(MAX)
     DECLARE @dropKeysSQL VARCHAR(MAX) = '', @createKeysSQL VARCHAR(MAX) = ''

     DECLARE rename_cursor CURSOR FOR
     WITH AllPKs AS (
           SELECT OBJECT_SCHEMA_NAME(sob.parent_obj) + '.' + OBJECT_NAME(sob.parent_obj) AS ForeignKeyTableName, 
                  sob.name AS ForeignKeyName,
            CAST(STUFF(( SELECT  DISTINCT
                                ';ALTER TABLE ' + OBJECT_SCHEMA_NAME(f.parent_object_id) + '.'
                                                + OBJECT_NAME(f.parent_object_id) + ' ADD CONSTRAINT '
                                + f.name + ' FOREIGN KEY ('  
                                + SUBSTRING(( SELECT    ',' + COL_NAME(fci.parent_object_id,  
                                                                       fci.parent_column_id)  
                                              FROM      sys.[foreign_key_columns] fci  
                                              WHERE     fci.[constraint_object_id] = fc.[constraint_object_id]  
                                            FOR  
                                              XML PATH('')  
                                            ), 2,  
                                            LEN(( SELECT    ',' + COL_NAME(fci.parent_object_id,  
                                                                           fci.parent_column_id)  
                                                  FROM      sys.[foreign_key_columns] fci  
                                                  WHERE     fci.[constraint_object_id] = fc.[constraint_object_id]  
                                                FOR  
                                                  XML PATH('')  
                                                ))) + ')' + ' REFERENCES ['  
                                + OBJECT_SCHEMA_NAME(f.referenced_object_id) + '].[' + OBJECT_NAME(f.referenced_object_id) + '] ('  
                                + SUBSTRING(( SELECT   ',' + COL_NAME(fci.[referenced_object_id],  
                                                                      fci.[referenced_column_id])  
                                               FROM     sys.[foreign_key_columns] fci  
                                               WHERE    fci.[constraint_object_id] = fc.[constraint_object_id]  
                                             FOR  
                                               XML PATH('')  
                                             ), 2,  
                                             LEN(( SELECT   ',' + COL_NAME(fci.[referenced_object_id],  
                                                                           fci.[referenced_column_id])  
                                                   FROM     sys.[foreign_key_columns] fci  
                                                   WHERE    fci.[constraint_object_id] = fc.[constraint_object_id]  
                                                 FOR  
                                                   XML PATH('')  
                                                 ))) + ')' AS Scripts  
                         FROM   sys.foreign_keys AS f  
                                INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id  
                         WHERE  --OBJECT_NAME(f.parent_object_id) = OBJECT_NAME(sob.parent_obj)  
								f.parent_object_id = sob.parent_obj
                                AND f.name = sob.NAME  
                       --FOR  
                         --XML PATH('')  
                                   ), 1, 1, '') AS VARCHAR(5000)) AS CreateScript  
           FROM sysobjects sob  
           JOIN sysobjects c   
                ON sob.parent_obj = c.id  
           JOIN sysreferences r   
                ON sob.id =  r.constid  
           JOIN sysobjects p   
                ON r.rkeyid = p.id  
           WHERE sob.[type] =  'F' AND  p.name = @UQTableName --AND OBJECT_SCHEMA_NAME(sob.parent_obj) = @SchemaName
     )
     SELECT * FROM AllPKs  

     OPEN rename_cursor  

     FETCH NEXT FROM rename_cursor   
     INTO @foreignKeyTableName, @foreignKeyName, @createScript  

     WHILE @@FETCH_STATUS = 0  
     BEGIN  
        SELECT @dropKeysSQL = @dropKeysSQL + ' ALTER TABLE ' + @foreignKeyTableName + ' DROP CONSTRAINT ' + @foreignKeyName + @NewLineChar  
        SELECT @createKeysSQL = @createKeysSQL + ' ' + @createScript + @NewLineChar  
        FETCH NEXT FROM rename_cursor   
        INTO @foreignKeyTableName, @foreignKeyName, @createScript  
     END  

     CLOSE rename_cursor  
     DEALLOCATE rename_cursor  


     -- STEP 1: Start the transaction  
     BEGIN TRANSACTION  

         DECLARE @SQL NVARCHAR(MAX)   
         DECLARE @intTableCount INT  
         SELECT @SQL = @DropKeysSQL + ' TRUNCATE TABLE ' + @tableName + ' ' + @NewLineChar + @CreateKeysSQL     
         PRINT '----- Running Script -----' + @NewLineChar
         PRINT @SQL
         PRINT '----- End of Script -----' + @NewLineChar

         PRINT 'NOTE: If error occurs, it may be nessassary to truncate a referencing tables data first.' + @NewLineChar
         EXEC sp_executesql @SQL, N'@intTableCount INT OUTPUT', @intTableCount OUTPUT

     IF (@intTableCount <> 0)
     BEGIN
           PRINT 'Rolling Back the Transaction.'
           ROLLBACK TRANSACTION
     END
     ELSE
     BEGIN
           COMMIT TRANSACTION
           PRINT 'Truncated Successfully.'
     END
END

GO
