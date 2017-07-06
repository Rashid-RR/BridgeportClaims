SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/* =============================================
 Author:      Jordan Gurney
 Create date: 04/17/2012
 Description: Creates an Audit table an a trigger for that table	
 Modifieid:   11/01/2013 by Jordan Gurney to include additional functionality to account
			  for creating a trigger and audit table within a different schema than dbo.
 =============================================
EXECUTE utilities.uspCreateAuditTrigger 'etl.OrderReturns'
*/
CREATE PROC [util].[uspCreateAuditTrigger]
    @TableName NVARCHAR(4000) /* Must include schema name */
AS /* Create Original Table Column List and Audit Table List */
    /*
	-- testing
    DECLARE @TableName NVARCHAR(4000) = 'etl.OrderReturns'
	*/
    DECLARE @AuditTableColumns NVARCHAR(4000)
       ,@OriginalColumns NVARCHAR(4000)
       ,@SchemaName NVARCHAR(4000)
       ,@SQL NVARCHAR(4000)

	/* Ensure that the audit table doesn't exist multiple times in multiple schemas */
    IF EXISTS ( SELECT  c.[TABLE_NAME]
                       ,C.TABLE_SCHEMA
                       ,COUNT(*)
                FROM    INFORMATION_SCHEMA.TABLES c
                WHERE   c.TABLE_NAME = PARSENAME(@TableName, 1) + 'Audit'
                GROUP BY c.[TABLE_NAME]
                       ,C.TABLE_SCHEMA
                HAVING  COUNT(*) > 1 )
        RAISERROR(N'Error: the audit table exists multiple times in multiple schemas', 16, 1)

    SELECT  @OriginalColumns = COALESCE(@OriginalColumns + ', ', '')
            + c.COLUMN_NAME
    FROM    INFORMATION_SCHEMA.COLUMNS c
    WHERE   c.TABLE_NAME = PARSENAME(@TableName, 1)
            AND c.DATA_TYPE != 'timestamp'

    SELECT  @SchemaName = s.[name]
    FROM    sys.schemas s
            INNER JOIN sys.[tables] t ON [t].[schema_id] = [s].[schema_id]
    WHERE   t.[name] = PARSENAME(@TableName, 1)

    SELECT  @AuditTableColumns = COALESCE(@AuditTableColumns + ', ', '')
            + c.COLUMN_NAME
    FROM    INFORMATION_SCHEMA.COLUMNS c
    WHERE   c.TABLE_NAME = PARSENAME(@TableName, 1) + 'Audit'
            AND c.ORDINAL_POSITION != 1
            AND c.DATA_TYPE != 'timestamp'

    IF @TableName IS NULL
        OR @AuditTableColumns IS NULL
        OR @OriginalColumns IS NULL
        OR @SchemaName IS NULL
        RAISERROR(N'Error: not all scalar variables were populated for procedure utilities.procCreateAuditTrigger', 16, 1)
    
    SET @SQL = 'CREATE TRIGGER ' + QUOTENAME(@SchemaName) + '.[ut'
        + PARSENAME(@TableName, 1) + 'Audit] ON ' + @TableName
        + '   FOR INSERT, UPDATE, DELETE
	AS
    IF ( SELECT COUNT(*)
         FROM   INSERTED
       ) > 0 
        BEGIN 
            IF ( SELECT COUNT(*)
                 FROM   DELETED
               ) > 0 
                BEGIN 
        
                    INSERT  INTO ' + @TableName + 'Audit
                            ( ' + @AuditTableColumns + ')
                            SELECT  ' + @OriginalColumns + ',''UPDATE''
                                   ,SUSER_SNAME()
                                   ,SYSDATETIME()
                            FROM    INSERTED
           
                END 
            ELSE 
                BEGIN 
                    INSERT  INTO ' + @TableName + 'Audit
                            ( ' + @AuditTableColumns + '
                            )
                            SELECT  ' + @OriginalColumns + ',''INSERT''
                                   ,SUSER_SNAME()
                                   ,SYSDATETIME()
                            FROM    INSERTED
                END 
        END 
    ELSE 
        BEGIN 
            INSERT  INTO ' + @TableName + 'Audit
                    ( ' + @AuditTableColumns + ')
                    SELECT  ' + @OriginalColumns + ',''DELETE''
                           ,SUSER_SNAME()
                           ,SYSDATETIME()
                    FROM    DELETED
        END'
    PRINT @SQL
    EXEC sys.sp_executesql @SQL


GO
