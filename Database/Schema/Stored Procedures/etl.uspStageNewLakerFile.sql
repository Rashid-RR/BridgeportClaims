SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	9/4/2017
	Description:	Prepares the etl.StagedLakerFile for importing. This assumes that the etl.StagedLakerFile
					table is now the most updated source of truth. And all of it's key ID's are NOT NULL.
	Sample Execute:
					DECLARE @Base [etl].[udtLakerFile]
					INSERT @Base (RowID) SELECT TOP (6376) NEWID() FROM sys.all_columns
					EXEC [etl].[uspStageNewLakerFile] @Base, 1
*/
CREATE PROC [etl].[uspStageNewLakerFile]
(
	@Base [etl].[udtLakerFile] READONLY,
	@DebugOnly BIT = 0
)
AS BEGIN
	
	/* Testing */
	/*DECLARE @Base [etl].[udtLakerFile]
	INSERT @Base (RowID) SELECT TOP (6376) NEWID() FROM sys.all_columns
	DECLARE @DebugOnly BIT = 0*/
	IF @DebugOnly IS NULL SET @DebugOnly = 0
	SET NOCOUNT ON;
	SET DEADLOCK_PRIORITY HIGH;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN
		DECLARE  @FileDate VARCHAR(20),
				 @PrntMsg NVARCHAR(1000),
				 @SQLStatement NVARCHAR(4000),
				 @ArchiveTableName SYSNAME,
				 @StagedLakerFileBackup SYSNAME = QUOTENAME('etl') + '.' + QUOTENAME('StagedLakerFileBackup'),
				 @StagedLakerFile SYSNAME = QUOTENAME('etl') + '.' + QUOTENAME('StagedLakerFile'),
				 @StagedLakerFileBackupRecordCount INT,
				 @StagedLakerFileRecordCount INT,
				 @MinimumThresholdOfRecords INT = 4000 -- This means that we expect the Laker tables to have at least this many records.

		-- We made it this far, it's time to do it.
		-- One more sanity check that the count of rows coming in, is greater than those archived.
		IF ISNULL((SELECT COUNT(*) FROM @Base HAVING COUNT(*) > @MinimumThresholdOfRecords), -2) <= 
				ISNULL((SELECT COUNT(*) FROM etl.StagedLakerFileBackup), -1)
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				SET @PrntMsg = N'We have a problem. The count of files coming' + CHAR(13) +
					' in is not greater than the count of files archived.'
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT
			END

		-- Let's check the state of our StagedLakerFile and StagedLakerFileBackup tables.
		-- We need to have: 1) more records in StagedLakerFile than the Backup file. 
		--					2) both tables full of non-nullable ID's
		SELECT @StagedLakerFileBackupRecordCount = COUNT(*) FROM etl.StagedLakerFileBackup
		IF @DebugOnly = 1
			BEGIN
				PRINT '-------------------------------------------------------------------------'
				SET @PrntMsg = N'Total Count of etl.StagedLakerFileBackup: ' + CONVERT(NVARCHAR, @StagedLakerFileBackupRecordCount)
				PRINT @PrntMsg
			END
		-- Perform some sanity checks first as to the validity of the etl.StagedLakerFile
		IF ISNULL((SELECT COUNT(*) FROM etl.StagedLakerFileBackup HAVING COUNT(*) > @MinimumThresholdOfRecords), -1)
		 != (SELECT COUNT(*) FROM etl.StagedLakerFileBackup AS s
		WHERE EXISTS (SELECT * FROM etl.StagedLakerFileBackup iis WHERE iis.AcctPayableID IS NOT NULL)
			AND s.PayorID IS NOT NULL
			AND s.PatientID IS NOT NULL
			AND s.ClaimID IS NOT NULL
			AND s.PrescriptionID IS NOT NULL
			AND EXISTS (SELECT * FROM etl.StagedLakerFileBackup AS iiis WHERE iiis.NABP IS NOT NULL)
			AND EXISTS (SELECT * FROM etl.StagedLakerFileBackup AS iiiis WHERE iiiis.InvoiceID IS NOT NULL)
			AND EXISTS (SELECT * FROM etl.StagedLakerFileBackup AS iiiiis WHERE iiiiis.AdjustorID IS NOT NULL))
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				RAISERROR(N'First sanity check failed. The StagedLakerFileBackup table doesn''t have the important ID''s populated.', 16, 1) WITH NOWAIT
				RETURN
			END

		SELECT @StagedLakerFileRecordCount = COUNT(*) FROM etl.StagedLakerFile
		IF @DebugOnly = 1
			BEGIN
				PRINT '-------------------------------------------------------------------------'
				SET @PrntMsg = N'Total Count of etl.StagedLakerFile: ' + CONVERT(NVARCHAR, @StagedLakerFileRecordCount)
				PRINT @PrntMsg
				PRINT '-------------------------------------------------------------------------'
			END

		-- Quality Check  table.
		IF ISNULL((SELECT COUNT(*) FROM etl.StagedLakerFile HAVING COUNT(*) > @MinimumThresholdOfRecords), -1)
		 != (SELECT COUNT(*) FROM etl.StagedLakerFile AS s
		WHERE EXISTS (SELECT * FROM etl.StagedLakerFile iis WHERE iis.AcctPayableID IS NOT NULL)
			AND s.PayorID IS NOT NULL
			AND s.PatientID IS NOT NULL
			AND s.ClaimID IS NOT NULL
			AND s.PrescriptionID IS NOT NULL
			AND EXISTS (SELECT * FROM etl.StagedLakerFile AS iiis WHERE iiis.NABP IS NOT NULL)
			AND EXISTS (SELECT * FROM etl.StagedLakerFile AS iiiis WHERE iiiis.InvoiceID IS NOT NULL)
			AND EXISTS (SELECT * FROM etl.StagedLakerFile AS iiiiis WHERE iiiiis.AdjustorID IS NOT NULL))
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				RAISERROR(N'Second sanity check failed. The StagedLakerFile table doesn''t have the important ID''s populated.', 16, 1) WITH NOWAIT
				RETURN
			END

		SET @FileDate = CONVERT(VARCHAR(20), dtme.udfGetLocalDateTime(SYSUTCDATETIME()) ,112)

		DECLARE @TodaysLakerFileBackupName VARCHAR(255) = '[etl].[StagedLakerFileBackup' + @FileDate + ']'

		-- Cleanup by backed up staged laker files

			DECLARE ArchivedLakerFileBackupsCleanerCursor CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
			SELECT QUOTENAME(s.name) + '.' + QUOTENAME(t.name)
			FROM sys.tables AS t
					INNER JOIN sys.schemas AS s ON s.schema_id=t.schema_id
			WHERE s.name='etl' AND t.name LIKE 'StagedLakerFileBackup%' AND LEN(t.name) > 22
			OPEN ArchivedLakerFileBackupsCleanerCursor
			FETCH NEXT FROM ArchivedLakerFileBackupsCleanerCursor INTO @ArchiveTableName
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQLStatement = N'DROP TABLE ' + @ArchiveTableName
				EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly
				FETCH NEXT FROM ArchivedLakerFileBackupsCleanerCursor INTO @ArchiveTableName
			END
			CLOSE ArchivedLakerFileBackupsCleanerCursor
			DEALLOCATE ArchivedLakerFileBackupsCleanerCursor

		-- If today's staged laker file backup table already exists, dump it.
		SET @SQLStatement =
			N'IF OBJECT_ID(N''' + @TodaysLakerFileBackupName + ''', N''U'') IS NOT NULL' 
				+ CHAR(10) + ' DROP TABLE ' + @TodaysLakerFileBackupName
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly

		-- Create today's backup file.
		SET @SQLStatement =
		N'SELECT * INTO ' + @TodaysLakerFileBackupName + ' FROM ' + @StagedLakerFileBackup
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly

		-- Check to see that the backup record count made it into the backup of the backup record count.
		SET @SQLStatement = N'IF (SELECT COUNT(*) FROM ' + @TodaysLakerFileBackupName + N') != ' + CONVERT(NVARCHAR, @StagedLakerFileBackupRecordCount) +
			CHAR(10) + N'   RAISERROR(N''The record count from the backup of the backup file doesn''''t match the original count of records from the backup file'', 16, 1) WITH NOWAIT';
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly

		-- Drop the proper backup table.
		SET @SQLStatement = N'DROP TABLE ' + @StagedLakerFileBackup
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly

		-- Archive true Staged Laker File into Backup Table
		SET @SQLStatement = N'SELECT * INTO ' + @StagedLakerFileBackup + ' FROM ' + @StagedLakerFile
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly
		
		-- Working up the courage....
		SET @SQLStatement = N'DROP TABLE etl.StagedLakerFile'
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly
		
		IF @DebugOnly = 0
			SELECT * INTO etl.StagedLakerFile FROM @Base
		ELSE
			PRINT 'SELECT * INTO etl.StagedLakerFile FROM etl.StagedLakerFileBackup WHERE 1 = 2' -- Just something to create the table.
			
		-- check that our max ordinal count is right.
		IF (CASE WHEN @DebugOnly = 1 THEN 162 ELSE 154 END) != (SELECT COUNT(*) FROM sys.columns AS c INNER JOIN sys.tables AS t ON t.object_id = c.object_id
		INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id AND s.name = 'etl'
		AND t.name = 'StagedLakerFile')
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				RAISERROR(N'Forget it, the etl.StagedLakerFile isn''t ready. It doesn''t have 154 columns.', 16, 1) WITH NOWAIT
				RETURN
			END

		-- Prepare the newly created etl.StagedLakerFile to go forth... and make us proud...
		SET @SQLStatement = N'EXEC etl.uspAddStagedLakerFileETLColumns'
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly

		-- Before our final commit, ensure the backup table now has total records of the original staged laker file table.
		SET @SQLStatement = N'IF (SELECT COUNT(*) FROM etl.StagedLakerFileBackup) != ' +
			 CONVERT(NVARCHAR, CASE WHEN @DebugOnly = 1 THEN @StagedLakerFileBackupRecordCount ELSE @StagedLakerFileRecordCount END) +
			CHAR(10) + '    RAISERROR(N''Error. The count in the backup file doesn''''t match the count captured in the original StagedLakerFile.'', 16, 1) WITH NOWAIT;'
		IF @DebugOnly = 1
			PRINT @SQLStatement
		-- But EXEC it whether we're debugging or not, because it's a QA check.
		EXEC dbo.uspExecSQL @SQLStatement, 0

		IF @@TRANCOUNT > 0
			BEGIN
				COMMIT
				SET @PrntMsg = 'Transaction Count: ' + CONVERT(NVARCHAR, @@TRANCOUNT)
				PRINT @PrntMsg
			END
			
		IF @DebugOnly = 1
			PRINT '-------------------------------------------------------------------------'
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
