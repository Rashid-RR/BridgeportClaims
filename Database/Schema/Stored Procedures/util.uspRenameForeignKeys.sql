SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Date:			5/25/2017
	Description:	Renames any foreign keys in the database to a specific naming standard (if the foreign key
					isn't named properly with this standard to begin with.
	Example Execute:
					EXEC util.uspRenameForeignKeys 1
*/
CREATE PROCEDURE [util].[uspRenameForeignKeys] @DebugOnly BIT = 0
AS BEGIN
	SET NOCOUNT ON
	BEGIN TRY
		BEGIN TRAN
		DECLARE @Data TABLE
			(
			 SourceTable sysname
			,SourceFkColumn sysname
			,DestinationTable sysname
			,DestinationPkColumn sysname
			,FkName sysname
			,Script NVARCHAR(4000)
			);
		WITH ForeignKeysCTE AS
		(
			SELECT  SourceTable = FK.TABLE_NAME
				   ,SourceTableSchema = FK.TABLE_SCHEMA
				   ,SourceFkColumn = CU.COLUMN_NAME
				   ,DestinationTable = PK.TABLE_NAME
				   ,DestinationPkColumn = PT.COLUMN_NAME
				   ,FkName = C.CONSTRAINT_NAME
				   ,NewFkName = FK.TABLE_NAME +	CU.COLUMN_NAME + PK.TABLE_NAME + PT.COLUMN_NAME
			FROM    INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
					INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
					INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
					INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
					INNER JOIN (SELECT  i1.TABLE_NAME
									   ,i2.COLUMN_NAME
								FROM    INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
										INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
								WHERE   i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
							   ) PT ON PT.TABLE_NAME = PK.TABLE_NAME
			WHERE	C.CONSTRAINT_NAME != 'fk' + FK.TABLE_NAME + CU.COLUMN_NAME + PK.TABLE_NAME + PT.COLUMN_NAME
		)
		INSERT @Data
				(SourceTable
				,SourceFkColumn
				,DestinationTable
				,DestinationPkColumn
				,FkName
				,Script)
		SELECT  c.SourceTable
			   ,c.SourceFkColumn
			   ,c.DestinationTable
			   ,c.DestinationPkColumn
			   ,c.FkName
			   ,'EXEC sys.sp_rename ''' + c.SourceTableSchema + '.' + c.FkName + 
					''', ''fk' + c.NewFkName + ''', ''OBJECT'''
		FROM    ForeignKeysCTE c
		IF (SELECT COUNT(*) FROM @Data AS d) < 1
			BEGIN
				PRINT 'No foreign keys to rename'
				IF @@TRANCOUNT > 0
					ROLLBACK
				RETURN 1
			END
		IF @DebugOnly = 1
			BEGIN
				SELECT  d.SourceTable
					   ,d.SourceFkColumn
					   ,d.DestinationTable
					   ,d.DestinationPkColumn
					   ,d.FkName
					   ,d.Script
				FROM    @Data AS d
				IF @@TRANCOUNT > 0
					ROLLBACK
				RETURN
			END

		DECLARE @SQL NVARCHAR(1000)

		-- Quickly iterate through each script to execute it.
		DECLARE C CURSOR LOCAL FAST_FORWARD FOR
        SELECT  d.Script
        FROM    @Data AS d

		OPEN C
		FETCH NEXT FROM C INTO @SQL
		WHILE @@FETCH_STATUS = 0
			BEGIN
				EXECUTE sys.sp_executesql @SQL
				FETCH NEXT FROM C INTO @SQL
			END
		CLOSE C
		DEALLOCATE C
		IF @@TRANCOUNT > 0
			COMMIT
		PRINT 'Success!'
		PRINT 'Foreign keys renamed.'
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK
				
        DECLARE @ErrSeverity INT = ERROR_SEVERITY()
            , @ErrState INT = ERROR_STATE()
            , @ErrProc NVARCHAR(MAX) = ERROR_PROCEDURE()
            , @ErrLine INT = ERROR_LINE()
            , @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE()

        RAISERROR(N'%s (line %d): %s',	-- Message text w formatting
			@ErrSeverity,		-- Severity
			@ErrState,			-- State
			@ErrProc,			-- First argument (string)
			@ErrLine,			-- Second argument (int)
			@ErrMsg)			-- First argument (string)
	END CATCH
END
GO
