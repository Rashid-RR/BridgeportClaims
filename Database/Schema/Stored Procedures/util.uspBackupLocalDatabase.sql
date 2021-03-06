SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	10/30/2013
	Description:	Backs up the current database to a specified location
					with today's date stamp (overwriting any existing backup
					with today's date stamp that happens to be there already).
	Sample Execute:
					EXEC [util].[uspBackupLocalDatabase] @BackupPath = 'D:\Jordan\Development\SQL\Backups'
*/
CREATE PROC [util].[uspBackupLocalDatabase]
	@BackupPath NVARCHAR(1000) = NULL
AS
	BEGIN
		DECLARE @Today DATE = CONVERT(DATE, [dtme].[udfGetLocalDate]())
		IF ISNULL(RTRIM(LTRIM(@BackupPath)),'') = ''
			SELECT  @BackupPath = 'F:\Development\SQL\Local Backups\'
		/* If the File Path does not contain a slash at the end, create one. */
		IF RIGHT(@BackupPath,1) != '\'
			SET @BackupPath += '\'
		PRINT 'Backing up to ' + @BackupPath
		DECLARE @DbName VARCHAR(50) = DB_NAME()-- database name 

		DECLARE @FileName VARCHAR(256) = @DbName -- filename for backup 
		DECLARE @FileDate VARCHAR(20) -- used for file name 
		DECLARE @LogicalDbFileName VARCHAR(255) = @FileName
			+ '-Full Database Backup'

		SELECT  @FileDate = CONVERT(VARCHAR(20),@Today,112);
		
		SET @FileName = @BackupPath + @FileName + '' + @FileDate + '.bak' 
		BACKUP DATABASE @DbName TO DISK = @FileName
		WITH  COPY_ONLY, FORMAT, INIT, NAME = @LogicalDbFileName,
		SKIP, COMPRESSION, NOREWIND, NOUNLOAD, STATS = 10;
	END
GO
