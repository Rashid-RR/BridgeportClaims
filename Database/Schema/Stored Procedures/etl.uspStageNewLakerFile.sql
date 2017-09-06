SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	9/4/2017
	Description:	Prepares the etl.StagedLakerFile for importing
	Sample Execute:
					EXEC etl.uspPrepareNewStagedLakerFile
*/
CREATE PROC [etl].[uspStageNewLakerFile]
(
	@Base [etl].[udtLakerFile] READONLY,
	@DebugOnly BIT = 0
)
AS BEGIN
	IF @DebugOnly IS NULL SET @DebugOnly = 0
	SET NOCOUNT ON;
	SET DEADLOCK_PRIORITY HIGH;
	SET TRAN ISOLATION LEVEL SERIALIZABLE;
	SET XACT_ABORT ON;
	BEGIN TRY
		BEGIN TRAN
		IF @DebugOnly = 1
			PRINT '-------------------------------------------------------------------------'
		-- Perform some sanity checks first as to the validity of the etl.StagedLakerFile
		IF (SELECT COUNT(*) FROM etl.StagedLakerFileBackup) != 
		(SELECT COUNT(*) FROM etl.StagedLakerFileBackup AS s
		WHERE EXISTS (SELECT * FROM etl.StagedLakerFileBackup iis WHERE iis.AcctPayableID IS NOT NULL)
			AND s.PayorID IS NOT NULL
			AND s.PatientID IS NOT NULL
			AND s.ClaimID IS NOT NULL
			AND s.PrescriptionID IS NOT NULL
			AND EXISTS (SELECT * FROM etl.StagedLakerFileBackup AS iiis WHERE iiis.PharmacyID IS NOT NULL)
			AND EXISTS (SELECT * FROM etl.StagedLakerFileBackup AS iiiis WHERE iiiis.InvoiceID IS NOT NULL)
			AND EXISTS (SELECT * FROM etl.StagedLakerFileBackup AS iiiiis WHERE iiiiis.AdjustorID IS NOT NULL))
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				RAISERROR(N'First sanity check failed. The StagedLakerFileBackup table doesn''t have the important ID''s populated.', 16, 1) WITH NOWAIT
				RETURN
			END


		DECLARE  @FileDate VARCHAR(20),
				 @PrntMsg NVARCHAR(1000),
				 @SQLStatement NVARCHAR(4000),
				 @ArchiveTableName SYSNAME,
				 @StagedLakerFileBackup SYSNAME = QUOTENAME('etl') + '.' + QUOTENAME('StagedLakerFileBackup'),
				 @StagedLakerFile SYSNAME = QUOTENAME('etl') + '.' + QUOTENAME('StagedLakerFile')

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

		-- Drop the proper backup table.
		SET @SQLStatement = N'DROP TABLE ' + @StagedLakerFileBackup
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly

		-- Archive true Staged Laker File into Backup Table
		SET @SQLStatement = N'SELECT * INTO ' + @StagedLakerFileBackup + ' FROM ' + @StagedLakerFile
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly

		-- Quality Check Backup table.
		IF (SELECT COUNT(*) FROM etl.StagedLakerFileBackup) != 
		(SELECT COUNT(*) FROM etl.StagedLakerFileBackup AS s
		WHERE EXISTS (SELECT * FROM etl.StagedLakerFileBackup iis WHERE iis.AcctPayableID IS NOT NULL)
			AND s.PayorID IS NOT NULL
			AND s.PatientID IS NOT NULL
			AND s.ClaimID IS NOT NULL
			AND s.PrescriptionID IS NOT NULL
			AND EXISTS (SELECT * FROM etl.StagedLakerFileBackup AS iiis WHERE iiis.PharmacyID IS NOT NULL)
			AND EXISTS (SELECT * FROM etl.StagedLakerFileBackup AS iiiis WHERE iiiis.InvoiceID IS NOT NULL)
			AND EXISTS (SELECT * FROM etl.StagedLakerFileBackup AS iiiiis WHERE iiiiis.AdjustorID IS NOT NULL))
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				RAISERROR(N'Second sanity check failed. Bailing out....', 16, 1) WITH NOWAIT
				RETURN
			END

		-- We made it this far, it's time to do it.
		-- One more sanity check that the count of rows coming in, is greater than those archived.
		IF (SELECT COUNT(*) FROM @Base) <= (SELECT COUNT(*) FROM etl.StagedLakerFileBackup)
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				SET @PrntMsg = N'We have a problem. The count of files coming' + CHAR(13) +
					' in is not greater than the count of files archived.'
				RAISERROR(@PrntMsg, 16, 1) WITH NOWAIT
			END

		-- Working up the courage....
		SET @SQLStatement = N'EXEC etl.uspCleanupStagedLakerFileAddedColumns'
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly

		-- check that our max ordinal count is right.
		IF 154 != (SELECT COUNT(*) FROM sys.columns AS c INNER JOIN sys.tables AS t ON t.object_id = c.object_id
		INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id AND s.name = 'etl'
		AND t.name = 'StagedLakerFile')
			BEGIN
				IF @@TRANCOUNT > 0
					ROLLBACK
				RAISERROR(N'Forget it, the etl.StagedLakerFile isn''t ready. It doesn''t have 154 columns.', 16, 1) WITH NOWAIT
				RETURN
			END

		SET @SQLStatement = N'DROP TABLE etl.StagedLakerFile'
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly
		
		IF @DebugOnly = 0
			SELECT * INTO etl.StagedLakerFile FROM @Base
		ELSE
			PRINT 'SELECT * INTO etl.StagedLakerFile FROM etl.StagedLakerFileBackup' -- Just something to create the table.

		SET @SQLStatement = N'EXEC etl.uspAddStagedLakerFileETLColumns'
		EXEC dbo.uspExecSQL @SQLStatement, @DebugOnly

		IF @@TRANCOUNT > 0
			BEGIN
				COMMIT
				SET @PrntMsg = 'Transaction Count: ' + CONVERT(NVARCHAR, @@TRANCOUNT)
				RAISERROR(@PrntMsg, 1, 1) WITH NOWAIT
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
