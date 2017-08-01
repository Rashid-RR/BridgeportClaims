SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	8/1/2017
	Description:	Takes two duplicate Patient ID's and merges them into one.
	Sample Execute:
					EXEC dbo.uspDeDupePatient 9, 124
*/
CREATE PROC [dbo].[uspDeDupePatient]
(
	@PatientIDToRemove INT,
	@PatientIDToKeep INT,
	@DebugOnly BIT = 0
)
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	SET DEADLOCK_PRIORITY HIGH;
	BEGIN TRY
		BEGIN TRANSACTION;
		
		DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME()
			   ,@PrntMsg VARCHAR(1000)
			   ,@RowCount INT
			   ,@FkTableSchema SYSNAME
			   ,@FkTableName SYSNAME
			   ,@FkColumnName SYSNAME
			   ,@SQLStatement NVARCHAR(4000)

		DECLARE @TableObjID INT = OBJECT_ID(N'dbo.Patient', N'U')
		IF @TableObjID IS NULL
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				RAISERROR(N'Error. Could no populate the @TableObjID variable', 16, 1) WITH NOWAIT
				RETURN
			END
		
		DECLARE FkCursor CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
		SELECT FkTableSchema = CONVERT(SYSNAME, schema_name([o2].[schema_id]))
			 , FkTableName = CONVERT(SYSNAME, [o2].[name])
			 , FkColumnName = CONVERT(SYSNAME, [c2].[name])
		FROM   [sys].[objects] AS o1
			 , [sys].[objects] AS o2
			 , [sys].[columns] AS c1
			 , [sys].[columns] AS c2
			 , [sys].[foreign_keys] AS f
			   INNER JOIN [sys].[foreign_key_columns] AS k ON ( [k].[constraint_object_id] = [f].[object_id] )
			   INNER JOIN [sys].[indexes] AS i ON (   [f].[referenced_object_id] = [i].[object_id]
													  and [f].[key_index_id] = [i].[index_id]
												  )
		WHERE  [o1].[object_id] = [f].[referenced_object_id]
			   AND ( [o1].[object_id] = @TableObjID )
			   AND [o2].[object_id] = [f].[parent_object_id]
			   AND [c1].[object_id] = [f].[referenced_object_id]
			   AND [c2].[object_id] = [f].[parent_object_id]
			   AND [c1].[column_id] = [k].[referenced_column_id]
			   AND [c2].[column_id] = [k].[parent_column_id]

		OPEN [FkCursor];
		FETCH NEXT FROM [FkCursor] INTO @FkTableSchema, @FkTableName, @FkColumnName
		WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQLStatement = N'UPDATE ' + QUOTENAME(@FkTableSchema) + '.' + QUOTENAME(@FkTableName)
					+ N' SET ' + QUOTENAME(@FkColumnName) + ' = ' + CONVERT(NVARCHAR, @PatientIDToKeep)
					+ N' WHERE ' + QUOTENAME(@FkColumnName) + ' = ' + CONVERT(NVARCHAR, @PatientIDToRemove)
				IF @DebugOnly = 1
					PRINT @SQLStatement
				ELSE
					EXECUTE [sys].[sp_executesql] @SQLStatement

				SET @SQLStatement = N'DELETE [dbo].[Patient] WHERE [PatientID] = ' + CONVERT(NVARCHAR, @PatientIDToRemove)
				IF @DebugOnly = 1
					PRINT @SQLStatement
				ELSE
					EXECUTE [sys].[sp_executesql] @SQLStatement
				FETCH NEXT FROM [FkCursor] INTO @FkTableSchema, @FkTableName, @FkColumnName
			END
		CLOSE [FkCursor];
		DEALLOCATE [FkCursor];

		IF @@TRANCOUNT > 0
			COMMIT
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
