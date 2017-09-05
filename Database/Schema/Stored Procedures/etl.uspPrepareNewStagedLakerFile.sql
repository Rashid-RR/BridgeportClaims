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
CREATE PROC [etl].[uspPrepareNewStagedLakerFile]
AS BEGIN
	SET NOCOUNT ON;
	
	-- Perform some sanity checks first as to the validity of the etl.StagedLakerFile
	IF (SELECT COUNT(*) FROM etl.StagedLakerFile) != 
	(SELECT COUNT(*) FROM etl.StagedLakerFile AS s
	WHERE EXISTS (SELECT * FROM etl.StagedLakerFile iis WHERE iis.AcctPayableID IS NOT NULL)
		AND s.PayorID IS NOT NULL
		AND s.PatientID IS NOT NULL
		AND s.ClaimID IS NOT NULL
		AND s.PrescriptionID IS NOT NULL
		AND EXISTS (SELECT * FROM etl.StagedLakerFile AS iiis WHERE iiis.PharmacyID IS NOT NULL)
		AND EXISTS (SELECT * FROM etl.StagedLakerFile AS iiiis WHERE iiiis.InvoiceID IS NOT NULL)
		AND EXISTS (SELECT * FROM etl.StagedLakerFile AS iiiiis WHERE iiiiis.AdjustorID IS NOT NULL))
		BEGIN
			RAISERROR(N'Sanity check failed. Bailing out....', 16, 1) WITH NOWAIT
			RETURN
		END


	DECLARE @FileDate VARCHAR(20), @SQLStatement NVARCHAR(4000), @ArchiveTableName SYSNAME,
		@BackupFileName SYSNAME = QUOTENAME('etl')+'.'+QUOTENAME('StagedLakerFileBackup')
	SET @FileDate = CONVERT(VARCHAR(20),GETDATE(),112)
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
			EXEC sys.sp_executesql @SQLStatement
			FETCH NEXT FROM ArchivedLakerFileBackupsCleanerCursor INTO @ArchiveTableName
		END
		CLOSE ArchivedLakerFileBackupsCleanerCursor
		DEALLOCATE ArchivedLakerFileBackupsCleanerCursor

	-- If today's staged laker file backup table already exists, dump it.
	SET @SQLStatement =
		N'IF OBJECT_ID(N''' + @TodaysLakerFileBackupName + ''', N''U'') IS NOT NULL' 
			+ CHAR(10) + CHAR(13) + ' DROP TABLE ' + @TodaysLakerFileBackupName
	EXEC sys.sp_executesql @SQLStatement

	-- Create today's backup file.
	SET @SQLStatement =
	N'SELECT * INTO ' + @TodaysLakerFileBackupName + ' FROM ' + @BackupFileName
	EXEC sys.sp_executesql @SQLStatement

	-- Drop the proper backup table.
	SET @SQLStatement = N'DROP TABLE ' + @BackupFileName
	EXEC sys.sp_executesql @SQLStatement

	-- Archive true Staged Laker File into Backup Table
END
GO
