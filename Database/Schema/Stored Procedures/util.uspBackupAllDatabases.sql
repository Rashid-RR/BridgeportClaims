SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
	Author:			Jordan Gurney
	Create Date:	11/5/2017
	Description:	Utility proc that creates a backup of all user databases
	Sample Execute:
					EXEC util.uspBackupAllDatabases
*/
CREATE PROC [util].[uspBackupAllDatabases]
(
	@BackupPath NVARCHAR(1000) = NULL,
	@IncludeSystemDatabases BIT = 0
)
AS BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	DECLARE @DbName SYSNAME, @SQL NVARCHAR(4000)
	DECLARE BackupDatabases CURSOR LOCAL FAST_FORWARD READ_ONLY FOR
	SELECT  d.[name]
	FROM    sys.databases AS d
	WHERE	1 = CASE WHEN @IncludeSystemDatabases = 1 THEN 1
					 WHEN ISNULL(@IncludeSystemDatabases, 0) = 0 AND d.[name] 
						  NOT IN ('master', 'tempdb', 'distributor', 'distribution', 'msdb', 'model')
						  AND d.[name] NOT LIKE 'ReportServer%'
						  AND d.[name] NOT LIKE 'RedGate%'
					 THEN 1
					 ELSE 0
				 END
	
	OPEN BackupDatabases
	
	FETCH NEXT FROM BackupDatabases INTO @DbName
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @SQL = N'USE ' + QUOTENAME(@DbName) + ';'
		SET @SQL += N'EXECUTE dbo.sp_BackupLocalDatabase @BackupPath'
		EXECUTE sys.sp_executesql @SQL, N'@BackupPath NVARCHAR(1000)', @BackupPath = @BackupPath
	    FETCH NEXT FROM BackupDatabases INTO @DbName
	END
	
	CLOSE BackupDatabases;
	DEALLOCATE BackupDatabases;
END
GO
