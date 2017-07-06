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
EXECUTE utilities.uspCreateAuditTableAndTrigger 'calcs.GraceBalanceOverrides'
*/
CREATE PROC [util].[uspCreateAuditTableAndTrigger]
    @TableName NVARCHAR(4000) /* Must include Schema Name */
AS
    BEGIN
        BEGIN TRY
            BEGIN TRAN
			SELECT @TableName = REPLACE(REPLACE(@TableName, '[',''),']','')
            DECLARE @OriginalTableColumnList NVARCHAR(4000)
               ,@AuditTableColumnList NVARCHAR(4000)

            SELECT  @OriginalTableColumnList = COALESCE(@OriginalTableColumnList
                                                        + ', ', '')
                    + c.COLUMN_NAME + ' ' + c.DATA_TYPE
                    + CASE WHEN c.CHARACTER_MAXIMUM_LENGTH IS NOT NULL
                           THEN '(' + RTRIM(c.CHARACTER_MAXIMUM_LENGTH) + ')'
                           ELSE ''
                      END + CASE WHEN c.IS_NULLABLE = 'NO' THEN ' NOT NULL'
                                 ELSE ' NULL'
                            END
            FROM    INFORMATION_SCHEMA.COLUMNS c
            WHERE   c.table_name = PARSENAME(@TableName, 1)

            SET @AuditTableColumnList = @OriginalTableColumnList
                + ', Operation VARCHAR(15) NULL, SystemUser NVARCHAR(100) NULL, AuditDate DATETIME2 NOT NULL'

            IF @TableName IS NULL
                OR @OriginalTableColumnList IS NULL
                OR @AuditTableColumnList IS NULL
                RAISERROR(N'Error: not all scalar variables were populated for procedure utilities.uspCreateAuditTableAndTrigger', 16, 1)

            DECLARE @SQL NVARCHAR(4000) = 'CREATE TABLE ' + @TableName
                + 'Audit (' + PARSENAME(@TableName, 1)
                + 'AuditID INT IDENTITY(1,1) NOT NULL, '
                + @AuditTableColumnList + ', CONSTRAINT [Pk'
                + PARSENAME(@TableName, 1) + 'Audit] PRIMARY KEY CLUSTERED ('
                + PARSENAME(@TableName, 1)
                + 'AuditID ASC
    		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
			) ON [PRIMARY]
			'
            EXEC sys.sp_executesql @SQL

            EXEC [util].[uspCreateAuditTrigger] @TableName = @TableName
			IF @@TRANCOUNT > 0
				COMMIT /* If we made it this far without an error, commit */
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK

            DECLARE @ErrMsg NVARCHAR(4000)
               ,@ErrSeverity INT
            SELECT  @ErrMsg = ERROR_MESSAGE()
                   ,@ErrSeverity = ERROR_SEVERITY()

            RAISERROR(@ErrMsg, @ErrSeverity, 1)      
			
        END CATCH
    END
GO
